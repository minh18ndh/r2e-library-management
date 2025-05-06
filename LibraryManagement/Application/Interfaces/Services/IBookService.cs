using LibraryManagement.Application.DTOs.Book;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookResponseDto>> GetAllAsync(string? search, string? sort, Guid? categoryId);
    Task<BookResponseDto> GetByIdAsync(Guid id);
    Task<BookResponseDto> CreateAsync(BookCreateRequestDto dto);
    Task<BookResponseDto> UpdateAsync(Guid id, BookUpdateRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}