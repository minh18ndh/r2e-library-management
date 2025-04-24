using Microsoft.AspNetCore.Mvc;
using LibraryManagement.DTOs.Book;
using LibraryManagement.Interfaces;

namespace LibraryManagement.Controllers;

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
    public IActionResult GetAll()
    {
        var books = _bookService.GetAll();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var book = _bookService.GetById(id);
        if (book == null) return NotFound($"Book with ID {id} not found.");
        return Ok(book);
    }

    [HttpPost]
    public IActionResult Create([FromBody] BookCreateRequestDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = _bookService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] BookUpdateRequestDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = _bookService.Update(id, dto);
        if (updated == null) return NotFound($"Book with ID {id} not found.");
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _bookService.Delete(id);
        return deleted ? NoContent() : NotFound($"Book with ID {id} not found.");
    }
}