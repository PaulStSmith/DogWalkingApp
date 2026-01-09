using DogWalkingApp.Domain.Entities;
using DogWalkingApp.Services;
using FluentAssertions;

namespace DogWalkingApp.Tests.Services;

/// <summary>
/// Tests for the AuthenticationService class.
/// </summary>
public class AuthenticationServiceTests : TestBase
{
    private AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        _authenticationService = new AuthenticationService(Context);
    }

    /// <summary>
    /// Tests that AuthenticateAsync with valid credentials returns true.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnTrue()
    {
        await _authenticationService.CreateDefaultUserAsync();

        var result = await _authenticationService.AuthenticateAsync("admin", "admin123");

        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with invalid password returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ShouldReturnFalse()
    {
        await _authenticationService.CreateDefaultUserAsync();

        var result = await _authenticationService.AuthenticateAsync("admin", "wrongpassword");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with invalid username returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithInvalidUsername_ShouldReturnFalse()
    {
        await _authenticationService.CreateDefaultUserAsync();

        var result = await _authenticationService.AuthenticateAsync("invaliduser", "admin123");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with empty username returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithEmptyUsername_ShouldReturnFalse()
    {
        var result = await _authenticationService.AuthenticateAsync("", "admin123");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with null username returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithNullUsername_ShouldReturnFalse()
    {
        var result = await _authenticationService.AuthenticateAsync(null!, "admin123");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with empty password returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithEmptyPassword_ShouldReturnFalse()
    {
        var result = await _authenticationService.AuthenticateAsync("admin", "");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with null password returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithNullPassword_ShouldReturnFalse()
    {
        var result = await _authenticationService.AuthenticateAsync("admin", null!);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with whitespace credentials returns false.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithWhitespaceCredentials_ShouldReturnFalse()
    {
        var result = await _authenticationService.AuthenticateAsync("   ", "   ");

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that GetUserAsync with existing user returns the user.
    /// </summary>
    [Fact]
    public async Task GetUserAsync_WithExistingUser_ShouldReturnUser()
    {
        await _authenticationService.CreateDefaultUserAsync();

        var user = await _authenticationService.GetUserAsync("admin");

        user.Should().NotBeNull();
        user!.Username.Should().Be("admin");
    }

    /// <summary>
    /// Tests that GetUserAsync with non-existing user returns null.
    /// </summary>
    [Fact]
    public async Task GetUserAsync_WithNonExistingUser_ShouldReturnNull()
    {
        var user = await _authenticationService.GetUserAsync("nonexistent");

        user.Should().BeNull();
    }

    /// <summary>
    /// Tests that GetUserAsync with case insensitive username returns the user.
    /// </summary>
    [Fact]
    public async Task GetUserAsync_WithCaseInsensitiveUsername_ShouldReturnUser()
    {
        await _authenticationService.CreateDefaultUserAsync();

        var user = await _authenticationService.GetUserAsync("ADMIN");

        user.Should().NotBeNull();
        user!.Username.Should().Be("admin");
    }

    /// <summary>
    /// Tests that CreateDefaultUserAsync when user does not exist creates the user.
    /// </summary>
    [Fact]
    public async Task CreateDefaultUserAsync_WhenUserDoesNotExist_ShouldCreateUser()
    {
        var user = await _authenticationService.CreateDefaultUserAsync();

        user.Should().NotBeNull();
        user.Username.Should().Be("admin");
        user.PasswordHash.Should().NotBeNullOrEmpty();
        user.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(10));

        var userInDatabase = await _authenticationService.GetUserAsync("admin");
        userInDatabase.Should().NotBeNull();
        userInDatabase!.Id.Should().Be(user.Id);
    }

    /// <summary>
    /// Tests that CreateDefaultUserAsync when user already exists returns the existing user.
    /// </summary>
    [Fact]
    public async Task CreateDefaultUserAsync_WhenUserAlreadyExists_ShouldReturnExistingUser()
    {
        var firstUser = await _authenticationService.CreateDefaultUserAsync();
        var secondUser = await _authenticationService.CreateDefaultUserAsync();

        secondUser.Should().Be(firstUser);
        secondUser.Id.Should().Be(firstUser.Id);
    }

    /// <summary>
    /// Tests that AuthenticateAsync is consistent with hashed passwords.
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_ShouldBeConsistentWithHashedPasswords()
    {
        var user = await _authenticationService.CreateDefaultUserAsync();
        
        var firstAuth = await _authenticationService.AuthenticateAsync("admin", "admin123");
        var secondAuth = await _authenticationService.AuthenticateAsync("admin", "admin123");

        firstAuth.Should().BeTrue();
        secondAuth.Should().BeTrue();
    }

    /// <summary>
    /// Tests that AuthenticateAsync with mixed case handles case correctly.
    /// </summary>
    /// <param name="username">The username to test.</param>
    /// <param name="password">The password to test.</param>
    [Theory]
    [InlineData("admin", "Admin123")]
    [InlineData("Admin", "admin123")]
    [InlineData("ADMIN", "admin123")]
    public async Task AuthenticateAsync_WithMixedCase_ShouldHandleCaseCorrectly(string username, string password)
    {
        await _authenticationService.CreateDefaultUserAsync();

        var result = await _authenticationService.AuthenticateAsync(username, password);

        if (username.Equals("admin", StringComparison.CurrentCultureIgnoreCase) && password == "admin123")
        {
            result.Should().BeTrue();
        }
        else
        {
            result.Should().BeFalse();
        }
    }
}