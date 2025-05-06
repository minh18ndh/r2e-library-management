using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Auth;

public class RegisterRequestDto
{
    [Required]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}