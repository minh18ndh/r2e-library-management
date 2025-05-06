using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(string? search, string? sort, Guid? categoryId)
    {
        var query = _context.Books
            .Include(b => b.Category) // include CategoryName for DTO
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId);

        if (sort == "asc")
            query = query.OrderBy(b => b.Title);
        else if (sort == "desc")
            query = query.OrderByDescending(b => b.Title);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> UpdateAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.Id);
        if (existingBook == null) return null;

        _context.Entry(existingBook).CurrentValues.SetValues(book);
        await _context.SaveChangesAsync();
        return existingBook;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Book>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _context.Books
            .Where(b => b.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
    }
}