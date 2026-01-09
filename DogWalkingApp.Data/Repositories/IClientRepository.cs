using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Data.Repositories;

/// <summary>
/// Interface for client repository operations.
/// </summary>
public interface IClientRepository
{
    /// <summary>
    /// Finds a client by name and phone asynchronously.
    /// </summary>
    /// <param name="name">The name of the client.</param>
    /// <param name="phone">The phone number of the client.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the client, or null if not found.</returns>
    Task<Client?> FindByNameAndPhoneAsync(string name, string phone);

    /// <summary>
    /// Adds a client asynchronously.
    /// </summary>
    /// <param name="client">The client to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added client.</returns>
    Task<Client> AddAsync(Client client);

    /// <summary>
    /// Finds a dog by client identifier and dog name asynchronously.
    /// </summary>
    /// <param name="clientId">The identifier of the client.</param>
    /// <param name="dogName">The name of the dog.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the dog, or null if not found.</returns>
    Task<Dog?> FindDogByClientAndNameAsync(int clientId, string dogName);

    /// <summary>
    /// Adds a dog asynchronously.
    /// </summary>
    /// <param name="dog">The dog to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added dog.</returns>
    Task<Dog> AddDogAsync(Dog dog);

    /// <summary>
    /// Updates a dog asynchronously.
    /// </summary>
    /// <param name="dog">The dog to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated dog.</returns>
    Task<Dog> UpdateDogAsync(Dog dog);

    /// <summary>
    /// Updates a client asynchronously.
    /// </summary>
    /// <param name="client">The client to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated client.</returns>
    Task<Client> UpdateAsync(Client client);

    /// <summary>
    /// Deletes a client asynchronously.
    /// </summary>
    /// <param name="clientId">The identifier of the client to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(int clientId);

    /// <summary>
    /// Deletes a dog asynchronously.
    /// </summary>
    /// <param name="dogId">The identifier of the dog to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteDogAsync(int dogId);

    /// <summary>
    /// Gets a client by identifier asynchronously.
    /// </summary>
    /// <param name="clientId">The identifier of the client.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the client, or null if not found.</returns>
    Task<Client?> GetByIdAsync(int clientId);

    /// <summary>
    /// Gets all clients with their dogs and walks asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of clients with their related data.</returns>
    Task<IEnumerable<Client>> GetAllWithDetailsAsync();
}