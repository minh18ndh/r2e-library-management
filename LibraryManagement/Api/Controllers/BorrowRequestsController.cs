using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/borrow-requests")]
[Authorize(Roles = "Admin")]
public class BorrowRequestsController : ControllerBase
{
    private readonly IBookBorrowingRequestService _service;

    public BorrowRequestsController(IBookBorrowingRequestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _service.GetAllAsync();
        return Ok(requests);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = await _service.GetByIdAsync(id);
        return Ok(request);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] BookBorrowingRequestUpdateStatusRequestDto dto)
    {
        var approverId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await _service.UpdateStatusAsync(id, dto, approverId);
        return Ok(updated);
    }
}