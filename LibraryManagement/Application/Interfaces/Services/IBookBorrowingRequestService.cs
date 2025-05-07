using LibraryManagement.Application.DTOs.BookBorrowingRequest;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBookBorrowingRequestService
{
    Task<IEnumerable<BookBorrowingRequestResponseDto>> GetAllAsync();
    Task<BookBorrowingRequestResponseDto> GetByIdAsync(Guid id);
    Task<IEnumerable<BookBorrowingRequestResponseDto>> GetByRequestorAsync(Guid requestorId);
    Task<BookBorrowingRequestResponseDto> CreateAsync(BookBorrowingRequestCreateRequestDto dto, Guid requestorId);
    Task<BookBorrowingRequestResponseDto> UpdateStatusAsync(Guid id, BookBorrowingRequestUpdateStatusRequestDto dto, Guid approverId);
}