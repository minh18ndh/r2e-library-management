namespace LibraryManagement.Application.DTOs.Auth;

public class AuthResponseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}