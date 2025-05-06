using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBookRepository _bookRepository;

    public CategoryService(ICategoryRepository categoryRepository, IBookRepository bookRepository)
    {
        _categoryRepository = categoryRepository;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(string? search)
    {
        var categories = await _categoryRepository.GetAllAsync(search);
        return categories.Select(c => c.ToResponseDto());
    }

    public async Task<CategoryResponseDto> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new NotFoundException($"Category with ID '{id}' not found");

        return category.ToResponseDto();
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryCreateRequestDto dto)
    {
        var category = dto.ToEntity();
        var createdCategory = await _categoryRepository.AddAsync(category);
        return createdCategory.ToResponseDto();
    }

    public async Task<CategoryResponseDto> UpdateAsync(Guid id, CategoryUpdateRequestDto dto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);
        if (existingCategory == null)
            throw new NotFoundException($"Category with ID '{id}' not found");

        existingCategory.Name = dto.Name;

        var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
        if (updatedCategory == null)
            throw new Exception("Unexpected error updating category");

        return updatedCategory.ToResponseDto();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new NotFoundException($"Category with ID '{id}' not found");

        var booksInCategory = await _bookRepository.GetByCategoryIdAsync(id);
        if (booksInCategory.Any())
            throw new BadRequestException("Cannot delete category that has associated books");

        var success = await _categoryRepository.DeleteAsync(id);
        if (!success)
            throw new Exception("Unexpected error deleting category");

        return true;
    }
}