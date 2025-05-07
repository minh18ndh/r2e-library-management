using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<string>(); // store enum as string

        builder.HasMany(u => u.CreatedRequests)
            .WithOne(r => r.Requestor)
            .HasForeignKey(r => r.RequestorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ApprovedRequests)
            .WithOne(r => r.Approver)
            .HasForeignKey(r => r.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}