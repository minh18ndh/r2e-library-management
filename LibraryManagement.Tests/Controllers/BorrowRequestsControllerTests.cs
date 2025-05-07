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
    public async Task UpdateStatus_ReturnsOkWithUpdatedRequest()
    {
        var requestId = Guid.NewGuid();
        var dto = new BookBorrowingRequestUpdateStatusRequestDto
        {
            Status = Domain.Enums.BorrowRequestStatus.Approved
        };

        var updated = new BookBorrowingRequestResponseDto
        {
            Id = requestId,
            Status = dto.Status
        };

        _serviceMock.Setup(s => s.UpdateStatusAsync(requestId, dto, _adminId)).ReturnsAsync(updated);

        var result = await _controller.UpdateStatus(requestId, dto) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(updated));
    }
}