using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories;

public interface IBookBorrowingRequestRepository
{
 
    Task<List<BookBorrowingRequest>> GetAllAsync();
    Task<BookBorrowingRequest?> GetByIdAsync(Guid id);

    Task<List<BookBorrowingRequest>> GetByRequestorIdAsync(Guid requestorId);
    Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request); 
    Task<BookBorrowingRequest?> UpdateAsync(BookBorrowingRequest request);
    Task<List<BookBorrowingRequest>> GetActiveRequestsByBookIdAsync(Guid bookId);
    Task<int> CountRequestsInMonthAsync(Guid requestorId, int year, int month);
}