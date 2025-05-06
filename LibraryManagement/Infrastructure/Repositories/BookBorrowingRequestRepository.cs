using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookBorrowingRequestRepository : IBookBorrowingRequestRepository
{
    private readonly LibraryDbContext _context;

    public BookBorrowingRequestRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookBorrowingRequest>> GetAllAsync()
    {
        return await _context.BookBorrowingRequests
            .Include(r => r.Requestor)
            .Include(r => r.Approver)
            .Include(r => r.Details)
                .ThenInclude(d => d.Book)
            .OrderByDescending(r => r.DateRequested)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BookBorrowingRequest?> GetByIdAsync(Guid id)
    {
        return await _context.BookBorrowingRequests
            .Include(r => r.Requestor)
            .Include(r => r.Approver)
            .Include(r => r.Details)
                .ThenInclude(d => d.Book)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<BookBorrowingRequest>> GetByRequestorIdAsync(Guid requestorId)
    {
        return await _context.BookBorrowingRequests
            .Where(r => r.RequestorId == requestorId)
            .Include(r => r.Requestor)
            .Include(r => r.Approver)
            .Include(r => r.Details)
                .ThenInclude(d => d.Book)
            .OrderByDescending(r => r.DateRequested)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest request)
    {
        await _context.BookBorrowingRequests.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<BookBorrowingRequest?> UpdateAsync(BookBorrowingRequest request)
    {
        var existingRequest = await _context.BookBorrowingRequests.FindAsync(request.Id);
        if (existingRequest == null) return null;

        // Only updating status and approver here
        existingRequest.Status = request.Status;
        existingRequest.ApproverId = request.ApproverId;

        _context.BookBorrowingRequests.Update(existingRequest);
        await _context.SaveChangesAsync();

        return existingRequest;
    }

    public async Task<List<BookBorrowingRequest>> GetActiveRequestsByBookIdAsync(Guid bookId)
    {
        return await _context.BookBorrowingRequests
            .Include(r => r.Details)
            .Where(r =>
                (r.Status == BorrowRequestStatus.Pending || r.Status == BorrowRequestStatus.Approved) &&
                r.Details.Any(d => d.BookId == bookId))
            .ToListAsync();
    }

    public async Task<int> CountRequestsInMonthAsync(Guid requestorId, int year, int month)
    {
        return await _context.BookBorrowingRequests
            .Where(r =>
                r.RequestorId == requestorId &&
                (r.Status == BorrowRequestStatus.Approved || r.Status == BorrowRequestStatus.Pending) &&
                r.DateRequested.Year == year &&
                r.DateRequested.Month == month)
            .CountAsync();
    }
}