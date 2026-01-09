using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using DogWalkingApp.Data;
using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Services;

/// <summary>
/// Service class for handling user authentication.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthenticationService"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
public class AuthenticationService(DogWalkingContext context) : IAuthenticationService
{
    private readonly DogWalkingContext _context = context;

    /// <inheritdoc/>
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        var user = await GetUserAsync(username);
        if (user == null) return false;

        // Simple hash comparison (for demo purposes)
        var hashedPassword = HashPassword(password);
        return user.PasswordHash == hashedPassword;
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserAsync(string username)
    {

#pragma warning disable CA1862 
        /*
         * CA1862: asks to use ‘StringComparison’ for case-insensitive comparison
         * However, EF Core does not support ‘StringComparison’ in LINQ to Entities queries.
         * EF Core also does not support ‘ToLowerInvariant()’ or ‘ToUpperInvariant()’ in LINQ to Entities queries.
         * It translates LINQ queries to SQL, and SQL databases have their own mechanisms for handling case sensitivity.
         */

        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
#pragma warning restore CA1862
    }

    /// <inheritdoc/>
    public async Task<User> CreateDefaultUserAsync()
    {
        // Create a default user for demo purposes
        var defaultUser = new User
        {
            Username = "admin",
            PasswordHash = HashPassword("admin123"),
            CreatedDate = DateTime.Now
        };

        // Check if user already exists
        var existingUser = await GetUserAsync(defaultUser.Username);
        if (existingUser != null)
            return existingUser;

        _context.Users.Add(defaultUser);
        await _context.SaveChangesAsync();
        return defaultUser;
    }

    /// <summary>
    /// Hashes the password using SHA256 with a salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password as a base64 string.</returns>
    private static string HashPassword(string password)
    {
        // Simple hash for demo purposes (not production-ready)
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + "salt"));
        return Convert.ToBase64String(hashedBytes);
    }
}