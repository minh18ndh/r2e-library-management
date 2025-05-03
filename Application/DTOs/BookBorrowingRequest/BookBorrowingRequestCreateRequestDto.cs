using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.BookBorrowingRequest;

public class BookBorrowingRequestCreateRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least 1 book must be selected.")]
    [MaxLength(5, ErrorMessage = "You can borrow at most 5 books per request.")]
    public required List<Guid> BookIds { get; set; }
}