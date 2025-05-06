using System.Security.Claims;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Tests.Controllers;

[TestFixture]
public class BorrowRequestsControllerTests
{
    private Mock<IBookBorrowingRequestService> _serviceMock = null!;
    private BorrowRequestsController _controller = null!;
    private readonly Guid _adminId = Guid.NewGuid();

    [SetUp]
    public void SetUp()
    {
        _serviceMock = new Mock<IBookBorrowingRequestService>();
        _controller = new BorrowRequestsController(_serviceMock.Object);

        // mock ClaimsPrincipal with NameIdentifier
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _adminId.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Test]
    public async Task GetAll_ReturnsOkWithRequests()
    {
        var requests = new List<BookBorrowingRequestResponseDto>
        {
            new() { Id = Guid.NewGuid(), Status = Domain.Enums.BorrowRequestStatus.Pending }
        };

        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(requests);

        var result = await _controller.GetAll() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(requests));
    }

    [Test]
    public async Task GetById_ReturnsOkWithRequest()
    {
        var request = new BookBorrowingRequestResponseDto
        {
            Id = Guid.NewGuid(),
            Status = Domain.Enums.BorrowRequestStatus.Pending
        };

        _serviceMock.Setup(s => s.GetByIdAsync(request.Id)).ReturnsAsync(request);

        var result = await _controller.GetById(request.Id) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(request));
    }

    [Test]
    public async Task UpdateStatus_InvalidModelState_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Status", "Required");

        var dto = new BookBorrowingRequestUpdateStatusRequestDto();

        var result = await _controller.UpdateStatus(Guid.NewGuid(), dto) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(400));
    }
}