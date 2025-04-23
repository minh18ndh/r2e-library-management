namespace LibraryManagement.DTOs.Book;


public class BookResponseDTO
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public required string CategoryName { get; set; }
}