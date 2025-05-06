using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using System.Security.Cryptography;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Services;
using LibraryManagement.Application.Interfaces.Services;

namespace LibraryManagement.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    private LibraryDbContext _context = null!;
    private Mock<IJwtService> _jwtMock = null!;
    private AuthService _authService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LibraryDbContext(options);
        _jwtMock = new Mock<IJwtService>();
        _authService = new AuthService(_context, _jwtMock.Object);
    }

    [Test]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsBadRequest()
    {
        _context.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            FullName = "Test",
            Email = "existing@mail.com",
            PasswordHash = "hashed",
            Role = UserRole.User
        });
        await _context.SaveChangesAsync();

        var dto = new RegisterRequestDto
        {
            FullName = "New User",
            Email = "existing@mail.com",
            Password = "pass"
        };

        Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterAsync(dto));
    }

    [Test]
    public async Task RegisterAsync_Success_ReturnsTokens()
    {
        _jwtMock.Setup(j => j.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("access-token");
        _jwtMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");

        var dto = new RegisterRequestDto
        {
            FullName = "New User",
            Email = "new@mail.com",
            Password = "pass123"
        };

        var result = await _authService.RegisterAsync(dto);

        Assert.That(result.AccessToken, Is.EqualTo("access-token"));
        Assert.That(result.RefreshToken, Is.EqualTo("refresh-token"));
    }

    [Test]
    public async Task LoginAsync_WhenEmailNotFound_ThrowsUnauthorized()
    {
        var dto = new LoginRequestDto
        {
            Email = "missing@mail.com",
            Password = "wrong"
        };

        Assert.ThrowsAsync<UnAuthorizedException>(() => _authService.LoginAsync(dto));
    }

    [Test]
    public async Task LoginAsync_WhenPasswordWrong_ThrowsUnauthorized()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "User",
            Email = "test@mail.com",
            PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes("hashed")),
            Role = UserRole.User
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var dto = new LoginRequestDto
        {
            Email = "test@mail.com",
            Password = "wrongpass"
        };

        Assert.ThrowsAsync<UnAuthorizedException>(() => _authService.LoginAsync(dto));
    }

    [Test]
    public async Task LoginAsync_Success_ReturnsTokens()
    {
        var password = "secret123";
        var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = "User",
            Email = "login@mail.com",
            PasswordHash = hash,
            Role = UserRole.User
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _jwtMock.Setup(j => j.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("access-token");
        _jwtMock.Setup(j => j.GenerateRefreshToken()).Returns("refresh-token");

        var dto = new LoginRequestDto
        {
            Email = "login@mail.com",
            Password = password
        };

        var result = await _authService.LoginAsync(dto);

        Assert.That(result.AccessToken, Is.EqualTo("access-token"));
        Assert.That(result.RefreshToken, Is.EqualTo("refresh-token"));
    }

    [Test]
    public void RefreshTokenAsync_WhenTokenInvalid_ThrowsUnauthorized()
    {
        var dto = new RefreshTokenRequestDto { RefreshToken = "invalid" };

        Assert.ThrowsAsync<UnAuthorizedException>(() => _authService.RefreshTokenAsync(dto));
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }
}