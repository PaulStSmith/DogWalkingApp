using Microsoft.EntityFrameworkCore;
using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Data.Repositories;

/// <summary>
/// Repository class for client operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ClientRepository"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
public class ClientRepository(DogWalkingContext context) : IClientRepository
{
    private readonly DogWalkingContext _context = context;

    /// <inheritdoc/>
    public async Task<Client?> FindByNameAndPhoneAsync(string name, string phone)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Name == name && c.Phone == phone);
    }

    /// <inheritdoc/>
    public async Task<Client> AddAsync(Client client)
    {
        client.CreatedDate = DateTime.UtcNow;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    /// <inheritdoc/>
    public async Task<Dog?> FindDogByClientAndNameAsync(int clientId, string dogName)
    {
        return await _context.Dogs
            .FirstOrDefaultAsync(d => d.ClientId == clientId && d.Name == dogName);
    }

    /// <inheritdoc/>
    public async Task<Dog> AddDogAsync(Dog dog)
    {
        dog.CreatedDate = DateTime.UtcNow;
        _context.Dogs.Add(dog);
        await _context.SaveChangesAsync();
        return dog;
    }

    /// <inheritdoc/>
    public async Task<Dog> UpdateDogAsync(Dog dog)
    {
        var existingDog = await _context.Dogs.FindAsync(dog.Id);
        if (existingDog != null)
        {
            // Update only the fields we want to change
            existingDog.Name = dog.Name;
            existingDog.Breed = dog.Breed;
            existingDog.Age = dog.Age;
            existingDog.ClientId = dog.ClientId;
            await _context.SaveChangesAsync();
            return existingDog;
        }
        
        // If not found, add as new (fallback)
        _context.Dogs.Add(dog);
        await _context.SaveChangesAsync();
        return dog;
    }

    /// <inheritdoc/>
    public async Task<Client> UpdateAsync(Client client)
    {
        var existingClient = await _context.Clients.FindAsync(client.Id);
        if (existingClient != null)
        {
            // Update only the fields we want to change
            existingClient.Name = client.Name;
            existingClient.Phone = client.Phone;
            await _context.SaveChangesAsync();
            return existingClient;
        }
        
        // If not found, add as new (fallback)
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int clientId)
    {
        var client = await _context.Clients
            .Include(c => c.Dogs)
            .ThenInclude(d => d.Walks)
            .FirstOrDefaultAsync(c => c.Id == clientId);
            
        if (client != null)
        {
            // Hard cascade delete: remove client, all their dogs, and all walks from database
            foreach (var dog in client.Dogs)
            {
                // Remove all walks for this dog
                _context.Walks.RemoveRange(dog.Walks);
            }
            
            // Remove all dogs for this client
            _context.Dogs.RemoveRange(client.Dogs);
            
            // Remove the client
            _context.Clients.Remove(client);
            
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteDogAsync(int dogId)
    {
        var dog = await _context.Dogs
            .Include(d => d.Walks)
            .FirstOrDefaultAsync(d => d.Id == dogId);
            
        if (dog != null)
        {
            // Hard cascade delete: remove dog and all their walks from database
            _context.Walks.RemoveRange(dog.Walks);
            _context.Dogs.Remove(dog);
            
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<Client?> GetByIdAsync(int clientId)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Client>> GetAllWithDetailsAsync()
    {
        return await _context.Clients
            .Include(c => c.Dogs)
                .ThenInclude(d => d.Walks)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}