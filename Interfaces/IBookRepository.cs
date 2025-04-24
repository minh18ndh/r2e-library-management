namespace LibraryManagement.Interfaces;

using LibraryManagement.Entities;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(Guid id);
    Book Add(Book book);
    Book? Update(Book book);
    bool Delete(Guid id);
}
