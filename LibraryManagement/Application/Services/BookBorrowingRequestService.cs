using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Application.Services;

public class BookBorrowingRequestService : IBookBorrowingRequestService
{
    private readonly IBookBorrowingRequestRepository _requestRepository;
    private readonly IBookRepository _bookRepository;

    public BookBorrowingRequestService(
        IBookBorrowingRequestRepository requestRepository,
        IBookRepository bookRepository)
    {
        _requestRepository = requestRepository;
        _bookRepository = bookRepository;
    }

    public async Task<List<BookBorrowingRequestResponseDto>> GetAllAsync()
    {
        var requests = await _requestRepository.GetAllAsync();
        return requests.Select(r => r.ToResponseDto()).ToList();
    }

    public async Task<BookBorrowingRequestResponseDto> GetByIdAsync(Guid id)
    {
        var request = await _requestRepository.GetByIdAsync(id);
        if (request == null)
            throw new NotFoundException($"Borrow request with ID '{id}' not found");

        return request.ToResponseDto();
    }

    public async Task<List<BookBorrowingRequestResponseDto>> GetByRequestorAsync(Guid requestorId)
    {
        var requests = await _requestRepository.GetByRequestorIdAsync(requestorId);
        return requests.Select(r => r.ToResponseDto()).ToList();
    }

    public async Task<BookBorrowingRequestResponseDto> CreateAsync(BookBorrowingRequestCreateRequestDto dto, Guid requestorId)
    {
        var now = DateTime.UtcNow;
        var count = await _requestRepository.CountRequestsInMonthAsync(requestorId, now.Year, now.Month);

        if (count >= 3)
            throw new BadRequestException("You can only make up to 3 borrow requests per month.");

        if (dto.BookIds.Distinct().Count() != dto.BookIds.Count)
            throw new BadRequestException("Each book in a borrow request must be unique.");

        // Check availability of each book
        var unavailableBooks = new List<Guid>();

        foreach (var bookId in dto.BookIds)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || book.Quantity < 1)
                unavailableBooks.Add(bookId);
        }

        if (unavailableBooks.Any())
            throw new BadRequestException($"Insufficient number of copies for book ID(s): {string.Join(", ", unavailableBooks)}");

        // Decrease quantity of each book
        foreach (var bookId in dto.BookIds)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book != null)
            {
                book.Quantity -= 1;
                await _bookRepository.UpdateAsync(book);
            }
        }

        // Create borrow request
        var entity = dto.ToEntity(requestorId);
        var createdRequest = await _requestRepository.AddAsync(entity);

        var fullRequest = await _requestRepository.GetByIdAsync(createdRequest.Id);
        if (fullRequest == null)
            throw new NotFoundException($"Borrow request with ID '{createdRequest.Id}' not found after creation");

        return fullRequest.ToResponseDto();
    }

    public async Task<BookBorrowingRequestResponseDto> UpdateStatusAsync(Guid id, BookBorrowingRequestUpdateStatusRequestDto dto, Guid approverId)
    {
        var existingRequest = await _requestRepository.GetByIdAsync(id);
        if (existingRequest == null)
            throw new NotFoundException($"Borrow request with ID '{id}' not found");

        if (existingRequest.Status != BorrowRequestStatus.Pending)
            throw new BadRequestException("Only pending requests can be updated");

        // Restore quantities if request is rejected
        if (dto.Status == BorrowRequestStatus.Rejected)
        {
            foreach (var detail in existingRequest.Details)
            {
                var book = await _bookRepository.GetByIdAsync(detail.BookId);
                if (book != null)
                {
                    book.Quantity += 1;
                    await _bookRepository.UpdateAsync(book);
                }
            }
        }

        existingRequest.Status = dto.Status;
        existingRequest.ApproverId = approverId;

        var updatedRequest = await _requestRepository.UpdateAsync(existingRequest);
        if (updatedRequest == null)
            throw new Exception("Unexpected error updating request");

        return updatedRequest.ToResponseDto();
    }
}