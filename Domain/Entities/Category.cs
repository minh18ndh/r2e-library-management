namespace LibraryManagement.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    // Navigation property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}