using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly LibraryDbContext _context;
    private readonly IJwtService _jwtService;

    // refreshToken -> userId (for now in-memory only)
    private static readonly Dictionary<string, string> _refreshTokens = new();

    public AuthService(LibraryDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, string userRole)
    {
        UserRole role = userRole == "Admin" ? UserRole.Admin : UserRole.User;

        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            throw new BadRequestException("Email already registered.");

        var user = request.ToEntity(HashPassword(request.Password), role);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var refreshToken = _jwtService.GenerateRefreshToken();

        _refreshTokens[refreshToken] = user.Id.ToString();

        return user.ToResponseDto(accessToken, refreshToken);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            throw new UnAuthorizedException("Invalid email or password.");

        var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var refreshToken = _jwtService.GenerateRefreshToken();

        _refreshTokens[refreshToken] = user.Id.ToString();

        return user.ToResponseDto(accessToken, refreshToken);
    }

    public Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var refreshToken = request.RefreshToken;

        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new UnAuthorizedException("Missing refresh token.");

        if (!_refreshTokens.TryGetValue(refreshToken, out var userId))
            throw new UnAuthorizedException("Invalid or expired refresh token.");

        var user = _context.Users.FirstOrDefault(u => u.Id.ToString().ToLower() == userId.ToLower());
        if (user == null)
            throw new UnAuthorizedException("User not found.");

        var newAccessToken = _jwtService.GenerateAccessToken(user.Id, user.Email, user.Role.ToString());
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        // Rotate: remove old, save new
        _refreshTokens.Remove(refreshToken);
        _refreshTokens[newRefreshToken] = user.Id.ToString();

        return Task.FromResult(user.ToResponseDto(newAccessToken, newRefreshToken));
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == storedHash;
    }
}