using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        var categories = await _categoryService.GetAllAsync(search);
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id); // throws NotFoundException if not found
        return Ok(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequestDto dto)
    {
        var createdCategory = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateRequestDto dto)
    {
        var updatedCategory = await _categoryService.UpdateAsync(id, dto); // throws NotFoundException if not found
        return Ok(updatedCategory);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id); // might throw NotFoundException or BadRequestException inside service depends
        return Ok(new { message = $"Category with ID '{id}' deleted." });
    }
}