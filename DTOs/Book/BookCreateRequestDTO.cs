namespace LibraryManagement.DTOs.Book;


public class BookCreateRequestDTO
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public Guid CategoryId { get; set; }
}