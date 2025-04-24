namespace LibraryManagement.Entities;


public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }

    public Guid CategoryId { get; set; } // FK

    //public Category? Category { get; set; }
}