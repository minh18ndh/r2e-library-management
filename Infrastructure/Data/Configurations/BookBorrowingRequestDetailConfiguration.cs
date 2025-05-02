using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class BookBorrowingRequestDetailConfiguration : IEntityTypeConfiguration<BookBorrowingRequestDetail>
{
    public void Configure(EntityTypeBuilder<BookBorrowingRequestDetail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.HasOne(d => d.Book)
            .WithMany(b => b.BorrowingDetails)
            .HasForeignKey(d => d.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.BookBorrowingRequest)
            .WithMany(r => r.Details)
            .HasForeignKey(d => d.BookBorrowingRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}