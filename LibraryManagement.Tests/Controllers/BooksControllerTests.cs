using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Tests.Controllers;

[TestFixture]
public class BooksControllerTests
{
    private Mock<IBookService> _bookServiceMock = null!;
    private BooksController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _bookServiceMock = new Mock<IBookService>();
        _controller = new BooksController(_bookServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkResult_WithBooks()
    {
        var books = new List<BookResponseDto>
        {
            new() { Id = Guid.NewGuid(), Title = "T", Author = "A", Description = "D", Quantity = 1, CategoryId = Guid.NewGuid(), CategoryName = "C" }
        };

        _bookServiceMock.Setup(s => s.GetAllAsync(null, null, null)).ReturnsAsync(books);

        var result = await _controller.GetAll(null, null, null) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value, Is.EqualTo(books));
    }

    [Test]
    public async Task GetById_ReturnsOkResult_WithBook()
    {
        var bookId = Guid.NewGuid();
        var book = new BookResponseDto { Id = bookId, Title = "T", Author = "A", Description = "D", Quantity = 1, CategoryId = Guid.NewGuid(), CategoryName = "C" };

        _bookServiceMock.Setup(s => s.GetByIdAsync(bookId)).ReturnsAsync(book);

        var result = await _controller.GetById(bookId) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(book));
    }

    [Test]
    public async Task Create_WithValidModel_ReturnsCreatedAtAction()
    {
        var dto = new BookCreateRequestDto
        {
            Title = "T", Author = "A", Description = "D", Quantity = 1, CategoryId = Guid.NewGuid()
        };

        var response = new BookResponseDto
        {
            Id = Guid.NewGuid(), Title = "T", Author = "A", Description = "D", Quantity = 1, CategoryId = dto.CategoryId, CategoryName = "C"
        };

        _bookServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

        var result = await _controller.Create(dto) as CreatedAtActionResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.ActionName, Is.EqualTo(nameof(_controller.GetById)));
        Assert.That(result.Value, Is.EqualTo(response));
    }

    [Test]
    public async Task Delete_ReturnsOkResult_WithMessage()
    {
        var id = Guid.NewGuid();

        _bookServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value!.ToString(), Does.Contain("deleted"));
    }
}