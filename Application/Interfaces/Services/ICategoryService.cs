using LibraryManagement.Application.DTOs.Category;

namespace LibraryManagement.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto> GetByIdAsync(Guid id);
    Task<CategoryResponseDto> AddAsync(CategoryCreateRequestDto dto);
    Task<CategoryResponseDto> UpdateAsync(Guid id, CategoryUpdateRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}