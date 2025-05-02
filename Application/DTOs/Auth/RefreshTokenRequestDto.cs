using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public required string RefreshToken { get; set; }
}