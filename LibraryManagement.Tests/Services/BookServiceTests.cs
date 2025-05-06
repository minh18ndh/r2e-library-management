using Moq;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Tests.Services;

[TestFixture]
public class BookServiceTests
{
    private Mock<IBookRepository> _bookRepoMock = null!;
    private Mock<IBookBorrowingRequestRepository> _borrowRequestRepoMock = null!;
    private BookService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _bookRepoMock = new Mock<IBookRepository>();
        _borrowRequestRepoMock = new Mock<IBookBorrowingRequestRepository>();
        _service = new BookService(_bookRepoMock.Object, _borrowRequestRepoMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsDtos()
    {
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = "Author",
                Description = "Desc",
                Quantity = 1,
                CategoryId = Guid.NewGuid()
            }
        };

        _bookRepoMock.Setup(r => r.GetAllAsync(null, null, null)).ReturnsAsync(books);

        var result = await _service.GetAllAsync(null, null, null);

        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result.First().Title, Is.EqualTo("Test Book"));
    }

    [Test]
    public void GetByIdAsync_WhenNotFound_ThrowsNotFoundException()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(Guid.NewGuid()));
    }

    [Test]
    public async Task GetByIdAsync_WhenFound_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var book = new Book
        {
            Id = id,
            Title = "Found",
            Author = "A",
            Description = "D",
            Quantity = 5,
            CategoryId = Guid.NewGuid()
        };

        _bookRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(book);

        var result = await _service.GetByIdAsync(id);

        Assert.That(result.Title, Is.EqualTo("Found"));
    }

    [Test]
    public async Task CreateAsync_Success_ReturnsDto()
    {
        var dto = new BookCreateRequestDto
        {
            Title = "T",
            Author = "A",
            Description = "D",
            Quantity = 2,
            CategoryId = Guid.NewGuid()
        };

        var created = new Book
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Author = dto.Author,
            Description = dto.Description,
            Quantity = dto.Quantity,
            CategoryId = dto.CategoryId
        };

        _bookRepoMock.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync(created);
        _bookRepoMock.Setup(r => r.GetByIdAsync(created.Id)).ReturnsAsync(created);

        var result = await _service.CreateAsync(dto);

        Assert.That(result.Title, Is.EqualTo("T"));
    }

    [Test]
    public async Task UpdateAsync_WhenUpdateFails_ThrowsException()
    {
        var id = Guid.NewGuid();
        var existing = new Book
        {
            Id = id,
            Title = "Old",
            Author = "Old",
            Description = "Old",
            Quantity = 1,
            CategoryId = Guid.NewGuid()
        };

        _bookRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _bookRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ReturnsAsync((Book?)null);

        var dto = new BookUpdateRequestDto
        {
            Title = "New",
            Author = "New",
            Description = "New",
            Quantity = 2,
            CategoryId = Guid.NewGuid()
        };

        Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(id, dto));
    }

    [Test]
    public async Task UpdateAsync_Success_ReturnsDto()
    {
        var id = Guid.NewGuid();
        var existing = new Book
        {
            Id = id,
            Title = "Old",
            Author = "Old",
            Description = "Old",
            Quantity = 1,
            CategoryId = Guid.NewGuid()
        };

        var updated = new Book
        {
            Id = id,
            Title = "New",
            Author = "New",
            Description = "New",
            Quantity = 2,
            CategoryId = Guid.NewGuid()
        };

        _bookRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _bookRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(updated);

        var dto = new BookUpdateRequestDto
        {
            Title = "New",
            Author = "New",
            Description = "New",
            Quantity = 2,
            CategoryId = Guid.NewGuid()
        };

        var result = await _service.UpdateAsync(id, dto);

        Assert.That(result.Title, Is.EqualTo("New"));
    }

    [Test]
    public async Task DeleteAsync_WhenBookNotFound_ThrowsNotFound()
    {
        _bookRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book?)null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(Guid.NewGuid()));
    }
}