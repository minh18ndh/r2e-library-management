using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Book;

public class BookCreateRequestDto
{
    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Author { get; set; }

    [Required]
    public required string Description { get; set; }

    [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100.")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Category is required.")]
    public Guid CategoryId { get; set; }
}