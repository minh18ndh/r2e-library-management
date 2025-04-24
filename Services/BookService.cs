using LibraryManagement.DTOs.Book;
using LibraryManagement.Extensions;
using LibraryManagement.Interfaces;

namespace LibraryManagement.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IEnumerable<BookResponseDTO> GetAll()
    {
        return _bookRepository.GetAll()
            .Select(book => book.ToDto()); // extension method
    }

    public BookResponseDTO? GetById(Guid id)
    {
        var book = _bookRepository.GetById(id);
        return book?.ToDto(); // null-safe
    }

    public BookResponseDTO Create(BookCreateRequestDTO dto)
    {
        var book = dto.ToEntity(); // extension method assigns ID
        var created = _bookRepository.Add(book);
        return created.ToDto();
    }

    public BookResponseDTO? Update(Guid id, BookUpdateRequestDTO dto)
    {
        var existing = _bookRepository.GetById(id);
        if (existing == null) return null;

        // update properties manually
        existing.Title = dto.Title;
        existing.Author = dto.Author;
        existing.Description = dto.Description;
        existing.Quantity = dto.Quantity;
        existing.CategoryId = dto.CategoryId;

        var updated = _bookRepository.Update(existing);
        return updated?.ToDto();
    }

    public bool Delete(Guid id)
    {
        return _bookRepository.Delete(id);
    }
}