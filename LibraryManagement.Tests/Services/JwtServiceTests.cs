using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.Tests.Services;

[TestFixture]
public class JwtServiceTests
{
    private JwtService _jwtService = null!;
    private string _key = null!;
    private string _issuer = "test-issuer";
    private string _audience = "test-audience";

    [SetUp]
    public void SetUp()
    {
        _key = Convert.ToBase64String(Encoding.UTF8.GetBytes("supersecretkey12345678901234567890")); // at least 256-bit
        var configDict = new Dictionary<string, string?>
        {
            {"Jwt:Key", _key},
            {"Jwt:Issuer", _issuer},
            {"Jwt:Audience", _audience}
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        _jwtService = new JwtService(config);
    }

    [Test]
    public void GenerateAccessToken_ReturnsValidJwtWithClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var role = "Admin";

        // Act
        var token = _jwtService.GenerateAccessToken(userId, email, role);

        // Assert
        Assert.IsNotNull(token);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.That(jwt.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId.ToString()));
        Assert.That(jwt.Claims.Any(c => c.Type == ClaimTypes.Email && c.Value == email));
        Assert.That(jwt.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role));
        Assert.That(jwt.ValidTo > DateTime.UtcNow);
    }

    [Test]
    public void GetPrincipalFromExpiredToken_WithInvalidToken_ReturnsNull()
    {
        var garbageToken = "this.is.not.valid.jwt";

        var result = _jwtService.GetPrincipalFromExpiredToken(garbageToken);

        Assert.IsNull(result);
    }
}