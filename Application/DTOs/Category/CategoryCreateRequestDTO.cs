using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Category;

public class CategoryCreateRequestDto
{
    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }
}