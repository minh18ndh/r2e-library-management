using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

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
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id); // throws NotFoundException if not found
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdCategory = await _categoryService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedCategory = await _categoryService.UpdateAsync(id, dto); // throws NotFoundException if not found
        return Ok(updatedCategory);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id); // may throws NotFoundException or BadRequestException inside service depends
        return Ok(new { message = $"Category with ID '{id}' deleted." });
    }
}