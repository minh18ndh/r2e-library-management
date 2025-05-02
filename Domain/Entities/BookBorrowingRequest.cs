using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities;

public class BookBorrowingRequest
{
    public Guid Id { get; set; }

    public Guid RequestorId { get; set; }

    public User? Requestor { get; set; }

    public Guid? ApproverId { get; set; } // Nullable for "not approved yet"

    public User? Approver { get; set; }

    public DateTime DateRequested { get; set; }

    public BorrowRequestStatus Status { get; set; }

    public ICollection<BookBorrowingRequestDetail> Details { get; set; } = new List<BookBorrowingRequestDetail>();
}