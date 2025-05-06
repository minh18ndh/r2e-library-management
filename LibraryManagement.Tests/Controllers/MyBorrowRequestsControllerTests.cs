using System.Security.Claims;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Tests.Controllers;

[TestFixture]
public class MyBorrowRequestsControllerTests
{
    private Mock<IBookBorrowingRequestService> _serviceMock = null!;
    private MyBorrowRequestsController _controller = null!;
    private readonly Guid _userId = Guid.NewGuid();

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IBookBorrowingRequestService>();
        _controller = new MyBorrowRequestsController(_serviceMock.Object);

        // Set up fake ClaimsPrincipal with user ID
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _userId.ToString()),
            new Claim(ClaimTypes.Role, "User")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Test]
    public async Task GetAll_ReturnsRequestsForCurrentUser()
    {
        var requests = new List<BookBorrowingRequestResponseDto>
        {
            new() { Id = Guid.NewGuid(), RequestorId = _userId }
        };

        _serviceMock.Setup(s => s.GetByRequestorAsync(_userId)).ReturnsAsync(requests);

        var result = await _controller.GetAll() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(requests));
    }

    [Test]
    public async Task GetById_ReturnsRequest()
    {
        var request = new BookBorrowingRequestResponseDto
        {
            Id = Guid.NewGuid(),
            RequestorId = _userId
        };

        _serviceMock.Setup(s => s.GetByIdAsync(request.Id)).ReturnsAsync(request);

        var result = await _controller.GetById(request.Id) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(request));
    }
}