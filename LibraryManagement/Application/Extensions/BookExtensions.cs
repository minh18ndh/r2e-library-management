using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Extensions;

public static class BookExtensions
{
    public static BookResponseDto ToResponseDto(this Book book)
    {
        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Quantity = book.Quantity,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? "Unknown"
        };
    }

    public static Book ToEntity(this BookCreateRequestDto dto)
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