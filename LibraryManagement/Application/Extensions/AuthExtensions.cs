using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.Extensions;

public static class AuthExtensions
{
    public static AuthResponseDto ToResponseDto (this User user, string accessToken, string refreshToken)
    {
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public static User ToEntity(this RegisterRequestDto dto, string hashedPassword, UserRole role)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = hashedPassword,
            Role = role
        };
    }
}