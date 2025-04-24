using LibraryManagement.DTOs.Book;

namespace LibraryManagement.Interfaces;

public interface IBookService
{
    IEnumerable<BookResponseDTO> GetAll();
    BookResponseDTO? GetById(Guid id);
    BookResponseDTO Create(BookCreateRequestDTO dto);
    BookResponseDTO? Update(Guid id, BookUpdateRequestDTO dto);
    bool Delete(Guid id);
}