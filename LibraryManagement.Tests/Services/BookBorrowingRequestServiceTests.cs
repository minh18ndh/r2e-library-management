using Moq;
using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Tests.Services;

[TestFixture]
public class BookBorrowingRequestServiceTests
{
    private Mock<IBookBorrowingRequestRepository> _requestRepoMock = null!;
    private Mock<IBookRepository> _bookRepoMock = null!;
    private BookBorrowingRequestService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _requestRepoMock = new Mock<IBookBorrowingRequestRepository>();
        _bookRepoMock = new Mock<IBookRepository>();
        _service = new BookBorrowingRequestService(_requestRepoMock.Object, _bookRepoMock.Object);
    }

    [Test]
    public async Task GetAllAsync_ReturnsDtos()
    {
        var list = new List<BookBorrowingRequest>
        {
            new() { Id = Guid.NewGuid(), RequestorId = Guid.NewGuid(), Status = BorrowRequestStatus.Pending, DateRequested = DateTime.UtcNow }
        };

        _requestRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(list);

        var result = await _service.GetAllAsync();

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetByRequestorAsync_ReturnsList()
    {
        var id = Guid.NewGuid();
        var list = new List<BookBorrowingRequest>
        {
            new() { Id = Guid.NewGuid(), RequestorId = id, Status = BorrowRequestStatus.Pending, DateRequested = DateTime.UtcNow }
        };

        _requestRepoMock.Setup(r => r.GetByRequestorIdAsync(id)).ReturnsAsync(list);

        var result = await _service.GetByRequestorAsync(id);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateAsync_Success_ReturnsDto()
    {
        var bookId = Guid.NewGuid();
        var dto = new BookBorrowingRequestCreateRequestDto { BookIds = new List<Guid> { bookId } };

        var book = new Book
        {
            Id = bookId,
            Title = "B",
            Author = "A",
            Description = "D",
            Quantity = 2,
            CategoryId = Guid.NewGuid()
        };

        var created = new BookBorrowingRequest { Id = Guid.NewGuid(), RequestorId = Guid.NewGuid() };

        _requestRepoMock.Setup(r => r.CountRequestsInMonthAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);
        _bookRepoMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(book);
        _requestRepoMock.Setup(r => r.AddAsync(It.IsAny<BookBorrowingRequest>())).ReturnsAsync(created);
        _requestRepoMock.Setup(r => r.GetByIdAsync(created.Id)).ReturnsAsync(created);

        var result = await _service.CreateAsync(dto, created.RequestorId);

        Assert.That(result.Id, Is.EqualTo(created.Id));
    }

    [Test]
    public async Task UpdateStatusAsync_WhenRejected_RestoresQuantities()
    {
        var bookId = Guid.NewGuid();

        var request = new BookBorrowingRequest
        {
            Id = Guid.NewGuid(),
            Status = BorrowRequestStatus.Pending,
            Details = new List<BookBorrowingRequestDetail>
            {
                new() { BookId = bookId }
            }
        };

        var book = new Book
        {
            Id = bookId,
            Title = "Restored",
            Author = "A",
            Description = "D",
            Quantity = 0,
            CategoryId = Guid.NewGuid()
        };

        _requestRepoMock.Setup(r => r.GetByIdAsync(request.Id)).ReturnsAsync(request);
        _bookRepoMock.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
        _bookRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(book);
        _requestRepoMock.Setup(r => r.UpdateAsync(It.IsAny<BookBorrowingRequest>())).ReturnsAsync(request);

        var dto = new BookBorrowingRequestUpdateStatusRequestDto { Status = BorrowRequestStatus.Rejected };

        var result = await _service.UpdateStatusAsync(request.Id, dto, Guid.NewGuid());

        Assert.That(result.Status, Is.EqualTo(BorrowRequestStatus.Rejected));
    }
}