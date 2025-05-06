using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class BookBorrowingRequestConfiguration : IEntityTypeConfiguration<BookBorrowingRequest>
{
    public void Configure(EntityTypeBuilder<BookBorrowingRequest> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.DateRequested);

        builder.Property(r => r.Status)
            .HasConversion<string>(); // store enum as string

        builder.HasOne(r => r.Requestor)
            .WithMany(u => u.CreatedRequests)
            .HasForeignKey(r => r.RequestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Approver)
            .WithMany(u => u.ApprovedRequests)
            .HasForeignKey(r => r.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}