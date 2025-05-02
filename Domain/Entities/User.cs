using System.ComponentModel.DataAnnotations;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public UserRole Role { get; set; }

    // Navigation properties
    public ICollection<BookBorrowingRequest> CreatedRequests { get; set; } = new List<BookBorrowingRequest>();
    public ICollection<BookBorrowingRequest> ApprovedRequests { get; set; } = new List<BookBorrowingRequest>();
}