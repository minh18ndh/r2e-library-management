using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Extensions;

public static class CategoryExtensions
{
    public static CategoryResponseDto ToResponseDto(this Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static Category ToEntity(this CategoryCreateRequestDto dto)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };
    }
}