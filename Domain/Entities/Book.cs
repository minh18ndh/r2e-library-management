using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }

    public required string Description { get; set; }

    public int Quantity { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }

    public ICollection<BookBorrowingRequestDetail> BorrowingDetails { get; set; } = new List<BookBorrowingRequestDetail>();
}