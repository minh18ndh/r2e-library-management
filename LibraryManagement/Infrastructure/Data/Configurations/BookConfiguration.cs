using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Description)
            .IsRequired();

        builder.Property(b => b.Quantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Books_Quantity_Range", "[Quantity] >= 0 AND [Quantity] <= 100");
        });

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}