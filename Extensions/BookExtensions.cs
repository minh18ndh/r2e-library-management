using LibraryManagement.DTOs.Book;
using LibraryManagement.Entities;

namespace LibraryManagement.Extensions;

public static class BookExtensions
{
    public static BookResponseDTO ToDto(this Book book)
    {
        return new BookResponseDTO
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Quantity = book.Quantity,
            CategoryName = "N/A" // update later
        };
    }

    public static Book ToEntity(this BookCreateRequestDTO dto)
    {
        return new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Author = dto.Author,
            Description = dto.Description,
            Quantity = dto.Quantity,
            CategoryId = dto.CategoryId
        };
    }
}