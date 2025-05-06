using LibraryManagement.Application.DTOs.BookBorrowingRequest;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Extensions;

public static class BookBorrowingRequestExtensions
{
    public static BookBorrowingRequestResponseDto ToResponseDto(this BookBorrowingRequest request)
    {
        return new BookBorrowingRequestResponseDto
        {
            Id = request.Id,
            RequestorId = request.RequestorId,
            RequestorName = request.Requestor?.FullName,
            ApproverId = request.ApproverId,
            ApproverName = request.Approver?.FullName,
            DateRequested = request.DateRequested,
            Status = request.Status,
            Details = request.Details.Select(detail => new BookBorrowingRequestDetailResponseDto
            {
                Id = detail.Id,
                BookId = detail.BookId,
                BookTitle = detail.Book?.Title ?? "Unknown"
            }).ToList()
        };
    }
    
    public static BookBorrowingRequest ToEntity(this BookBorrowingRequestCreateRequestDto dto, Guid requestorId)
    {
        var request = new BookBorrowingRequest
        {
            Id = Guid.NewGuid(),
            RequestorId = requestorId,
            DateRequested = DateTime.UtcNow,
            Status = Domain.Enums.BorrowRequestStatus.Pending,
            Details = dto.BookIds.Select(bookId => new BookBorrowingRequestDetail
            {
                Id = Guid.NewGuid(),
                BookId = bookId
            }).ToList()
        };

        return request;
    }
}