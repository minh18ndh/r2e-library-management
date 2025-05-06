using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.BookBorrowingRequest;

public class BookBorrowingRequestResponseDto
{
    public Guid Id { get; set; }
    public Guid RequestorId { get; set; }
    public string? RequestorName { get; set; }
    public Guid? ApproverId { get; set; }
    public string? ApproverName { get; set; }
    public DateTime DateRequested { get; set; }
    public BorrowRequestStatus Status { get; set; }
    public List<BookBorrowingRequestDetailResponseDto> Details { get; set; } = new();
}