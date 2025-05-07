using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces.Repositories;

public interface IBookBorrowingRequestRepository
{
    Task<IEnumerable<BookBorrowingRequest>> GetAllAsync();
    Task<BookBorrowingRequest?> GetByIdAsync(Guid id);

    Task<IEnumerable<BookBorrowingRequest>> GetByRequestorIdAsync(Guid requestorId);
    Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request); 
    Task<BookBorrowingRequest?> UpdateAsync(BookBorrowingRequest request);
    Task<IEnumerable<BookBorrowingRequest>> GetActiveRequestsByBookIdAsync(Guid bookId);
    Task<int> CountRequestsInMonthAsync(Guid requestorId, int year, int month);
}