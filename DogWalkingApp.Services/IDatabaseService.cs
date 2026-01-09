using DogWalkingApp.Data;

namespace DogWalkingApp.Services;

/// <summary>
/// Interface for database services.
/// </summary>
public interface IDatabaseService
{
    /// <summary>
    /// Creates a new database context instance.
    /// </summary>
    /// <returns>A new database context instance.</returns>
    DogWalkingContext CreateContext();

    /// <summary>
    /// Gets the connection string from configuration.
    /// </summary>
    /// <returns>The connection string.</returns>
    string GetConnectionString();
}