using LibraryManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.BookBorrowingRequest;

public class BookBorrowingRequestUpdateStatusRequestDto
{
    [Required]
    public BorrowRequestStatus Status { get; set; }
}