using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto> GetByIdAsync(Guid id);
    Task<BookResponseDto> AddAsync(BookCreateRequestDto dto);
    Task<BookResponseDto> UpdateAsync(Guid id, BookUpdateRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}