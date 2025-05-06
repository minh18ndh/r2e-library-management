using LibraryManagement.API.Controllers;
using LibraryManagement.Application.DTOs.Auth;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryManagement.Tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock = null!;
    private AuthController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Test]
    public async Task Register_ReturnsOkWithTokens()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            FullName = "Test User",
            Email = "test@mail.com",
            Password = "123456"
        };

        var expected = new AuthResponseDto
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token"
        };

        _authServiceMock.Setup(x => x.RegisterAsync(request)).ReturnsAsync(expected);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(okResult!.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task Login_ReturnsOkWithTokens()
    {
        var request = new LoginRequestDto
        {
            Email = "test@mail.com",
            Password = "123456"
        };

        var expected = new AuthResponseDto
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token"
        };

        _authServiceMock.Setup(x => x.LoginAsync(request)).ReturnsAsync(expected);

        var result = await _controller.Login(request);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(okResult!.Value, Is.EqualTo(expected));
    }
}