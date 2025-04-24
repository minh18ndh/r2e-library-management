using LibraryManagement.Entities;
using LibraryManagement.Interfaces;

namespace LibraryManagement.Repositories;
public class BookRepository : IBookRepository
{
    private readonly List<Book> _books;

    public BookRepository()
    {
        _books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Description = "A Handbook of Agile Software Craftsmanship",
                Quantity = 5,
                CategoryId = Guid.NewGuid() // placeholder
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Pragmatic Programmer",
                Author = "Andy Hunt, Dave Thomas",
                Description = "Your Journey to Mastery",
                Quantity = 7,
                CategoryId = Guid.NewGuid()
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Design Patterns",
                Author = "Erich Gamma et al.",
                Description = "Elements of Reusable Object-Oriented Software",
                Quantity = 4,
                CategoryId = Guid.NewGuid()
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Refactoring",
                Author = "Martin Fowler",
                Description = "Improving the Design of Existing Code",
                Quantity = 6,
                CategoryId = Guid.NewGuid()
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Introduction to Algorithms",
                Author = "Cormen, Leiserson, Rivest, Stein",
                Description = "CLRS Book - Bible of Algorithms",
                Quantity = 2,
                CategoryId = Guid.NewGuid()
            }
        };
    }

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(Guid id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public Book Add(Book book)
    {
        _books.Add(book);
        return book;
    }

    public Book? Update(Book book)
    {
        var existing = GetById(book.Id);
        if (existing == null) return null;

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.Description = book.Description;
        existing.Quantity = book.Quantity;
        existing.CategoryId = book.CategoryId;

        return existing;
    }

    public bool Delete(Guid id)
    {
        var book = GetById(id);
        if (book == null) return false;
        _books.Remove(book);
        return true;
    }
}