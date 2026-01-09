using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Services;

/// <summary>
/// Interface for walk-related services.
/// </summary>
public interface IWalkService
{
    /// <summary>
    /// Gets the most recent walks asynchronously.
    /// </summary>
    /// <param name="count">The number of recent walks to retrieve. Default is 50.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of walks.</returns>
    Task<IEnumerable<Walk>> GetRecentWalksAsync(int count = 50);

    /// <summary>
    /// Gets all clients with their dogs and walks asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of clients.</returns>
    Task<IEnumerable<Client>> GetAllClientsWithDetailsAsync();

    /// <summary>
    /// Searches for walks based on a search term asynchronously.
    /// </summary>
    /// <param name="searchTerm">The term to search for in walks.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of walks matching the search term.</returns>
    Task<IEnumerable<Walk>> SearchWalksAsync(string searchTerm);

    /// <summary>
    /// Saves a walk asynchronously.
    /// </summary>
    /// <param name="walk">The walk to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the saved walk.</returns>
    Task<Walk> SaveWalkAsync(Walk walk);

    /// <summary>
    /// Deletes a walk asynchronously.
    /// </summary>
    /// <param name="walkId">The identifier of the walk to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteWalkAsync(int walkId);

    /// <summary>
    /// Gets or creates a client asynchronously.
    /// </summary>
    /// <param name="name">The name of the client.</param>
    /// <param name="phone">The phone number of the client.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the client, or null if not found or created.</returns>
    Task<Client?> GetOrCreateClientAsync(string name, string phone);

    /// <summary>
    /// Gets or creates a dog asynchronously.
    /// </summary>
    /// <param name="clientId">The identifier of the client.</param>
    /// <param name="name">The name of the dog.</param>
    /// <param name="breed">The breed of the dog.</param>
    /// <param name="age">The age of the dog.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the dog, or null if not found or created.</returns>
    Task<Dog?> GetOrCreateDogAsync(int clientId, string name, string breed, int age);

    /// <summary>
    /// Creates a new client asynchronously.
    /// </summary>
    /// <param name="client">The client to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created client.</returns>
    Task<Client> CreateClientAsync(Client client);

    /// <summary>
    /// Updates a client asynchronously.
    /// </summary>
    /// <param name="client">The client to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated client.</returns>
    Task<Client> UpdateClientAsync(Client client);

    /// <summary>
    /// Deletes a client asynchronously.
    /// </summary>
    /// <param name="clientId">The identifier of the client to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteClientAsync(int clientId);

    /// <summary>
    /// Creates a new dog asynchronously.
    /// </summary>
    /// <param name="dog">The dog to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created dog.</returns>
    Task<Dog> CreateDogAsync(Dog dog);

    /// <summary>
    /// Updates a dog asynchronously.
    /// </summary>
    /// <param name="dog">The dog to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated dog.</returns>
    Task<Dog> UpdateDogAsync(Dog dog);

    /// <summary>
    /// Deletes a dog asynchronously.
    /// </summary>
    /// <param name="dogId">The identifier of the dog to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteDogAsync(int dogId);
}