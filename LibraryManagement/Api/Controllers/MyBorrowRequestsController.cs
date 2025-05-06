using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/my-borrow-requests")]
[Authorize(Roles = "User")]
public class MyBorrowRequestsController : ControllerBase
{
    private readonly IBookBorrowingRequestService _service;

    public MyBorrowRequestsController(IBookBorrowingRequestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requestorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var requests = await _service.GetByRequestorAsync(requestorId);
        return Ok(requests);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = await _service.GetByIdAsync(id);
        return Ok(request);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookBorrowingRequestCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var requestorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var created = await _service.CreateAsync(dto, requestorId);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }
}