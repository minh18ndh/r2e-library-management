using LibraryManagement.Application.DTOs.BookBorrowingRequest;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBookBorrowingRequestService
{
    Task<List<BookBorrowingRequestResponseDto>> GetAllAsync();
    Task<BookBorrowingRequestResponseDto> GetByIdAsync(Guid id);
    Task<List<BookBorrowingRequestResponseDto>> GetByRequestorAsync(Guid requestorId);
    Task<BookBorrowingRequestResponseDto> CreateAsync(BookBorrowingRequestCreateRequestDto dto, Guid requestorId);
    Task<BookBorrowingRequestResponseDto> UpdateStatusAsync(Guid id, BookBorrowingRequestUpdateStatusRequestDto dto, Guid approverId);
}