using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookBorrowingRequestRepository _bookBorrowingRequestRepository;

    public BookService(IBookRepository bookRepository, IBookBorrowingRequestRepository bookBorrowingRequestRepository)
    {
        _bookRepository = bookRepository;
        _bookBorrowingRequestRepository = bookBorrowingRequestRepository;
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync(string? search, string? sort, Guid? categoryId)
    {
        var books = await _bookRepository.GetAllAsync(search, sort, categoryId);
        return books.Select(b => b.ToResponseDto());
    }

    public async Task<BookResponseDto> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            throw new NotFoundException($"Book with ID '{id}' not found");

        return book.ToResponseDto();
    }

    public async Task<BookResponseDto> CreateAsync(BookCreateRequestDto dto)
    {
        var book = dto.ToEntity();
        var createdBook = await _bookRepository.AddAsync(book);
        //return createdBook.ToResponseDto();

        // Re-fetch full book with Category included
        var fullBook = await _bookRepository.GetByIdAsync(createdBook.Id);
        //return fullBook!.ToResponseDto(); // use ! to assert it won't be null

        if (fullBook == null)
            throw new NotFoundException($"Book with ID '{createdBook.Id}' not found after creation");

        return fullBook.ToResponseDto();
    }

    public async Task<BookResponseDto> UpdateAsync(Guid id, BookUpdateRequestDto dto)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null)
            throw new NotFoundException($"Book with ID '{id}' not found");

        existingBook.Title = dto.Title;
        existingBook.Author = dto.Author;
        existingBook.Description = dto.Description;
        existingBook.Quantity = dto.Quantity;
        existingBook.CategoryId = dto.CategoryId;

        var updatedBook = await _bookRepository.UpdateAsync(existingBook);
        if (updatedBook == null)
            throw new Exception("Unexpected error updating book");

        return updatedBook.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            throw new NotFoundException($"Book with ID '{id}' not found");

        var activeRequests = await _bookBorrowingRequestRepository.GetActiveRequestsByBookIdAsync(id);
        if (activeRequests.Any())
            throw new BadRequestException("Cannot delete book that is part of a pending or approved borrow request.");

        var success = await _bookRepository.DeleteAsync(id);
        if (!success)
            throw new Exception("Unexpected error deleting book");

        return true;
    }
}