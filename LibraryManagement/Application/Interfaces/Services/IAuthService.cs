using LibraryManagement.Application.DTOs.Auth;

namespace LibraryManagement.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, string userRole);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
}