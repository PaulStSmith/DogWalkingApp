using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Services;

/// <summary>
/// Interface for authentication services.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user asynchronously.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if authentication is successful; otherwise, false.</returns>
    Task<bool> AuthenticateAsync(string username, string password);

    /// <summary>
    /// Gets a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user, or null if not found.</returns>
    Task<User?> GetUserAsync(string username);

    /// <summary>
    /// Creates a default user asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created default user.</returns>
    Task<User> CreateDefaultUserAsync();
}