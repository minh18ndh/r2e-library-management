using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] Guid? categoryId)
    {
        var books = await _bookService.GetAllAsync(search, sort, categoryId);
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);
        return Ok(book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookCreateRequestDto dto)
    {
        var createdBook = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BookUpdateRequestDto dto)
    {
        var updatedBook = await _bookService.UpdateAsync(id, dto);
        return Ok(updatedBook);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _bookService.DeleteAsync(id);
        return Ok(new { message = $"Book with ID '{id}' deleted." });
    }
}