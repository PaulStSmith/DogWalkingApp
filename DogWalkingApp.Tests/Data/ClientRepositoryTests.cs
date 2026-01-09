using DogWalkingApp.Data.Repositories;
using DogWalkingApp.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DogWalkingApp.Tests.Data;

/// <summary>
/// Tests for the ClientRepository class.
/// </summary>
public class ClientRepositoryTests : TestBase
{
    private ClientRepository _clientRepository;

    public ClientRepositoryTests()
    {
        _clientRepository = new ClientRepository(Context);
    }

    /// <summary>
    /// Verifies that FindByNameAndPhoneAsync returns the client when it exists.
    /// </summary>
    [Fact]
    public async Task FindByNameAndPhoneAsync_WhenClientExists_ShouldReturnClient()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.FindByNameAndPhoneAsync("Test Client", "123-456-7890");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Client");
        result.Phone.Should().Be("123-456-7890");
    }

    /// <summary>
    /// Verifies that FindByNameAndPhoneAsync returns null when the client does not exist.
    /// </summary>
    [Fact]
    public async Task FindByNameAndPhoneAsync_WhenClientDoesNotExist_ShouldReturnNull()
    {
        var result = await _clientRepository.FindByNameAndPhoneAsync("Non Existent", "999-999-9999");

        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that AddAsync adds a client to the database.
    /// </summary>
    [Fact]
    public async Task AddAsync_ShouldAddClientToDatabase()
    {
        var client = new Client
        {
            Name = "New Client",
            Phone = "555-0001",
            CreatedDate = DateTime.UtcNow
        };

        var result = await _clientRepository.AddAsync(client);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("New Client");
        result.Phone.Should().Be("555-0001");

        var clientInDb = await Context.Clients.FirstOrDefaultAsync(c => c.Id == result.Id);
        clientInDb.Should().NotBeNull();
        clientInDb!.Name.Should().Be("New Client");
    }

    /// <summary>
    /// Verifies that FindDogByClientAndNameAsync returns the dog when it exists.
    /// </summary>
    [Fact]
    public async Task FindDogByClientAndNameAsync_WhenDogExists_ShouldReturnDog()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.FindDogByClientAndNameAsync(1, "Test Dog");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Dog");
        result.ClientId.Should().Be(1);
    }

    /// <summary>
    /// Verifies that FindDogByClientAndNameAsync returns null when the dog does not exist.
    /// </summary>
    [Fact]
    public async Task FindDogByClientAndNameAsync_WhenDogDoesNotExist_ShouldReturnNull()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.FindDogByClientAndNameAsync(1, "Non Existent Dog");

        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that FindDogByClientAndNameAsync returns null when the client does not exist.
    /// </summary>
    [Fact]
    public async Task FindDogByClientAndNameAsync_WhenClientDoesNotExist_ShouldReturnNull()
    {
        var result = await _clientRepository.FindDogByClientAndNameAsync(999, "Any Dog");

        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that AddDogAsync adds a dog to the database.
    /// </summary>
    [Fact]
    public async Task AddDogAsync_ShouldAddDogToDatabase()
    {
        await SeedTestDataAsync();

        var dog = new Dog
        {
            ClientId = 1,
            Name = "New Dog",
            Breed = "Poodle",
            Age = 2,
            CreatedDate = DateTime.UtcNow
        };

        var result = await _clientRepository.AddDogAsync(dog);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Name.Should().Be("New Dog");
        result.ClientId.Should().Be(1);

        var dogInDb = await Context.Dogs.FirstOrDefaultAsync(d => d.Id == result.Id);
        dogInDb.Should().NotBeNull();
        dogInDb!.Name.Should().Be("New Dog");
    }

    /// <summary>
    /// Verifies that UpdateDogAsync updates a dog in the database.
    /// </summary>
    [Fact]
    public async Task UpdateDogAsync_ShouldUpdateDogInDatabase()
    {
        await SeedTestDataAsync();

        var dog = await Context.Dogs.FirstAsync();
        dog.Name = "Updated Dog Name";
        dog.Breed = "Updated Breed";
        dog.Age = 5;

        var result = await _clientRepository.UpdateDogAsync(dog);

        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Dog Name");
        result.Breed.Should().Be("Updated Breed");
        result.Age.Should().Be(5);

        var dogInDb = await Context.Dogs.FirstOrDefaultAsync(d => d.Id == dog.Id);
        dogInDb.Should().NotBeNull();
        dogInDb!.Name.Should().Be("Updated Dog Name");
        dogInDb.Breed.Should().Be("Updated Breed");
        dogInDb.Age.Should().Be(5);
    }

    /// <summary>
    /// Verifies that UpdateAsync updates a client in the database.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateClientInDatabase()
    {
        await SeedTestDataAsync();

        var client = await Context.Clients.FirstAsync();
        client.Name = "Updated Client Name";
        client.Phone = "555-9999";

        var result = await _clientRepository.UpdateAsync(client);

        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Client Name");
        result.Phone.Should().Be("555-9999");

        var clientInDb = await Context.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);
        clientInDb.Should().NotBeNull();
        clientInDb!.Name.Should().Be("Updated Client Name");
        clientInDb.Phone.Should().Be("555-9999");
    }

    /// <summary>
    /// Verifies that DeleteAsync removes a client from the database.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveClientFromDatabase()
    {
        await SeedTestDataAsync();

        var client = await Context.Clients.FirstAsync();
        var clientId = client.Id;

        await _clientRepository.DeleteAsync(clientId);

        var deletedClient = await Context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        deletedClient.Should().BeNull();
    }

    /// <summary>
    /// Verifies that DeleteDogAsync removes a dog from the database.
    /// </summary>
    [Fact]
    public async Task DeleteDogAsync_ShouldRemoveDogFromDatabase()
    {
        await SeedTestDataAsync();

        var dog = await Context.Dogs.FirstAsync();
        var dogId = dog.Id;

        await _clientRepository.DeleteDogAsync(dogId);

        var deletedDog = await Context.Dogs.FirstOrDefaultAsync(d => d.Id == dogId);
        deletedDog.Should().BeNull();
    }

    /// <summary>
    /// Verifies that GetByIdAsync returns the client when it exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_WhenClientExists_ShouldReturnClient()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Test Client");
    }

    /// <summary>
    /// Verifies that GetByIdAsync returns null when the client does not exist.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_WhenClientDoesNotExist_ShouldReturnNull()
    {
        var result = await _clientRepository.GetByIdAsync(999);

        result.Should().BeNull();
    }

    /// <summary>
    /// Verifies that DeleteAsync cascades to delete associated dogs and walks.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldCascadeDeleteDogsAndWalks()
    {
        await SeedTestDataAsync();

        var client = await Context.Clients.FirstAsync();
        var clientId = client.Id;
        var dogCount = await Context.Dogs.CountAsync(d => d.ClientId == clientId);
        var walkCount = await Context.Walks.CountAsync(w => w.ClientId == clientId);

        dogCount.Should().BeGreaterThan(0);
        walkCount.Should().BeGreaterThan(0);

        await _clientRepository.DeleteAsync(clientId);

        var remainingDogs = await Context.Dogs.CountAsync(d => d.ClientId == clientId);
        var remainingWalks = await Context.Walks.CountAsync(w => w.ClientId == clientId);

        remainingDogs.Should().Be(0);
        remainingWalks.Should().Be(0);
    }

    /// <summary>
    /// Verifies that GetAllWithDetailsAsync returns clients with navigation properties.
    /// </summary>
    [Fact]
    public async Task GetAllWithDetailsAsync_ShouldReturnClientsWithNavigationProperties()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.GetAllWithDetailsAsync();

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
        
        var client = result.First();
        client.Dogs.Should().NotBeEmpty();
        client.Walks.Should().NotBeEmpty();
        client.Dogs.First().Name.Should().Be("Test Dog");
        client.Walks.First().Notes.Should().Be("Test walk notes");
    }

    /// <summary>
    /// Verifies that DeleteDogAsync cascades to delete associated walks.
    /// </summary>
    [Fact]
    public async Task DeleteDogAsync_ShouldCascadeDeleteWalks()
    {
        await SeedTestDataAsync();

        var dog = await Context.Dogs.FirstAsync();
        var dogId = dog.Id;
        var walkCount = await Context.Walks.CountAsync(w => w.DogId == dogId);

        walkCount.Should().BeGreaterThan(0);

        await _clientRepository.DeleteDogAsync(dogId);

        var remainingWalks = await Context.Walks.CountAsync(w => w.DogId == dogId);
        remainingWalks.Should().Be(0);
    }

    /// <summary>
    /// Verifies that GetAllWithDetailsAsync includes all related data.
    /// </summary>
    [Fact]
    public async Task GetAllWithDetailsAsync_ShouldIncludeAllRelatedData()
    {
        await SeedTestDataAsync();

        var result = await _clientRepository.GetAllWithDetailsAsync();

        result.Should().HaveCount(1);
        var client = result.First();
        client.Dogs.Should().NotBeEmpty();
        client.Walks.Should().NotBeEmpty();
        client.Dogs.First().Name.Should().Be("Test Dog");
        client.Walks.First().Notes.Should().Be("Test walk notes");
    }
}