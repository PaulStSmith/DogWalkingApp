using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Data.Repositories;

/// <summary>
/// Interface for walk repository operations.
/// </summary>
public interface IWalkRepository
{
    /// <summary>
    /// Gets the most recent walks asynchronously.
    /// </summary>
    /// <param name="count">The number of recent walks to retrieve. Default is 50.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of walks.</returns>
    Task<IEnumerable<Walk>> GetRecentWalksAsync(int count = 50);

    /// <summary>
    /// Searches for walks based on a search term asynchronously.
    /// </summary>
    /// <param name="searchTerm">The term to search for in walks.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of walks matching the search term.</returns>
    Task<IEnumerable<Walk>> SearchWalksAsync(string searchTerm);

    /// <summary>
    /// Adds a walk asynchronously.
    /// </summary>
    /// <param name="walk">The walk to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added walk.</returns>
    Task<Walk> AddAsync(Walk walk);

    /// <summary>
    /// Updates a walk asynchronously.
    /// </summary>
    /// <param name="walk">The walk to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated walk.</returns>
    Task<Walk> UpdateAsync(Walk walk);

    /// <summary>
    /// Deletes a walk asynchronously.
    /// </summary>
    /// <param name="walkId">The identifier of the walk to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int walkId);

    /// <summary>
    /// Gets a walk by its identifier asynchronously.
    /// </summary>
    /// <param name="walkId">The identifier of the walk.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the walk, or null if not found.</returns>
    Task<Walk?> GetByIdAsync(int walkId);
}