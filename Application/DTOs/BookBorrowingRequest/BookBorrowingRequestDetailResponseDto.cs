namespace LibraryManagement.Application.DTOs.BookBorrowingRequest;

public class BookBorrowingRequestDetailResponseDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
}