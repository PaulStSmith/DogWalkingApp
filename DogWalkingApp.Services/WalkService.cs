using DogWalkingApp.Data.Repositories;
using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Services;

/// <summary>
/// Service class for managing walks.
/// </summary>
public class WalkService : IWalkService
{
    private readonly IWalkRepository _walkRepository;
    private readonly IClientRepository _clientRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WalkService"/> class.
    /// </summary>
    /// <param name="walkRepository">The walk repository.</param>
    /// <param name="clientRepository">The client repository.</param>
    public WalkService(IWalkRepository walkRepository, IClientRepository clientRepository)
    {
        _walkRepository = walkRepository;
        _clientRepository = clientRepository;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Walk>> GetRecentWalksAsync(int count = 50)
    {
        return await _walkRepository.GetRecentWalksAsync(count);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Client>> GetAllClientsWithDetailsAsync()
    {
        return await _clientRepository.GetAllWithDetailsAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Walk>> SearchWalksAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetRecentWalksAsync();

        return await _walkRepository.SearchWalksAsync(searchTerm);
    }

    /// <inheritdoc/>
    public async Task<Walk> SaveWalkAsync(Walk walk)
    {
        if (walk.Id == 0)
            return await _walkRepository.AddAsync(walk);
        else
            return await _walkRepository.UpdateAsync(walk);
    }

    /// <inheritdoc/>
    public async Task DeleteWalkAsync(int walkId)
    {
        await _walkRepository.DeleteAsync(walkId);
    }

    /// <inheritdoc/>
    public async Task<Client?> GetOrCreateClientAsync(string name, string phone)
    {
        var existing = await _clientRepository.FindByNameAndPhoneAsync(name, phone);
        if (existing != null)
            return existing;

        var client = new Client
        {
            Name = name,
            Phone = phone,
            CreatedDate = DateTime.UtcNow
        };

        return await _clientRepository.AddAsync(client);
    }

    /// <inheritdoc/>
    public async Task<Dog?> GetOrCreateDogAsync(int clientId, string name, string breed, int age)
    {
        var existing = await _clientRepository.FindDogByClientAndNameAsync(clientId, name);
        if (existing != null)
        {
            // Update if different
            if (existing.Breed != breed || existing.Age != age)
            {
                existing.Breed = breed;
                existing.Age = age;
                await _clientRepository.UpdateDogAsync(existing);
            }
            return existing;
        }

        var dog = new Dog
        {
            ClientId = clientId,
            Name = name,
            Breed = breed,
            Age = age,
            CreatedDate = DateTime.UtcNow
        };

        return await _clientRepository.AddDogAsync(dog);
    }

    /// <inheritdoc/>
    public async Task<Client> CreateClientAsync(Client client)
    {
        return await _clientRepository.AddAsync(client);
    }

    /// <inheritdoc/>
    public async Task<Client> UpdateClientAsync(Client client)
    {
        return await _clientRepository.UpdateAsync(client);
    }

    /// <inheritdoc/>
    public async Task DeleteClientAsync(int clientId)
    {
        await _clientRepository.DeleteAsync(clientId);
    }

    /// <inheritdoc/>
    public async Task<Dog> CreateDogAsync(Dog dog)
    {
        return await _clientRepository.AddDogAsync(dog);
    }

    /// <inheritdoc/>
    public async Task<Dog> UpdateDogAsync(Dog dog)
    {
        return await _clientRepository.UpdateDogAsync(dog);
    }

    /// <inheritdoc/>
    public async Task DeleteDogAsync(int dogId)
    {
        await _clientRepository.DeleteDogAsync(dogId);
    }
}