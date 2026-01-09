using DogWalkingApp.Data.Repositories;
using DogWalkingApp.Domain.Entities;
using DogWalkingApp.Services;
using FluentAssertions;
using Moq;

namespace DogWalkingApp.Tests.Services;

/// <summary>
/// Contains unit tests for the <see cref="WalkService"/> class.
/// </summary>
public class WalkServiceTests
{
    private readonly Mock<IWalkRepository> _mockWalkRepository;
    private readonly Mock<IClientRepository> _mockClientRepository;
    private readonly WalkService _walkService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WalkServiceTests"/> class,
    /// setting up mocked repositories and the service under test.
    /// </summary>
    public WalkServiceTests()
    {
        _mockWalkRepository = new Mock<IWalkRepository>();
        _mockClientRepository = new Mock<IClientRepository>();
        _walkService = new WalkService(_mockWalkRepository.Object, _mockClientRepository.Object);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetRecentWalksAsync()"/> returns walks from the repository.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_ShouldReturnWalksFromRepository()
    {
        var expectedWalks = new List<Walk>
        {
            new() { Id = 1, ClientId = 1, DogId = 1 },
            new() { Id = 2, ClientId = 2, DogId = 2 }
        };
        _mockWalkRepository.Setup(x => x.GetRecentWalksAsync(50)).ReturnsAsync(expectedWalks);

        var result = await _walkService.GetRecentWalksAsync();

        result.Should().BeEquivalentTo(expectedWalks);
        _mockWalkRepository.Verify(x => x.GetRecentWalksAsync(50), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetRecentWalksAsync(int)"/> passes the custom count to the repository.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_WithCustomCount_ShouldPassCountToRepository()
    {
        var count = 25;
        var expectedWalks = new List<Walk>();
        _mockWalkRepository.Setup(x => x.GetRecentWalksAsync(count)).ReturnsAsync(expectedWalks);

        var result = await _walkService.GetRecentWalksAsync(count);

        result.Should().BeEquivalentTo(expectedWalks);
        _mockWalkRepository.Verify(x => x.GetRecentWalksAsync(count), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetAllClientsWithDetailsAsync()"/> returns clients from the repository.
    /// </summary>
    [Fact]
    public async Task GetAllClientsWithDetailsAsync_ShouldReturnClientsFromRepository()
    {
        var expectedClients = new List<Client>
        {
            new() { Id = 1, Name = "Client 1" },
            new() { Id = 2, Name = "Client 2" }
        };
        _mockClientRepository.Setup(x => x.GetAllWithDetailsAsync()).ReturnsAsync(expectedClients);

        var result = await _walkService.GetAllClientsWithDetailsAsync();

        result.Should().BeEquivalentTo(expectedClients);
        _mockClientRepository.Verify(x => x.GetAllWithDetailsAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.SearchWalksAsync(string)"/> with an empty search term returns recent walks.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_WithEmptySearchTerm_ShouldReturnRecentWalks()
    {
        var expectedWalks = new List<Walk> { new() { Id = 1 } };
        _mockWalkRepository.Setup(x => x.GetRecentWalksAsync(50)).ReturnsAsync(expectedWalks);

        var result = await _walkService.SearchWalksAsync("");

        result.Should().BeEquivalentTo(expectedWalks);
        _mockWalkRepository.Verify(x => x.GetRecentWalksAsync(50), Times.Once);
        _mockWalkRepository.Verify(x => x.SearchWalksAsync(It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.SearchWalksAsync(string)"/> with a null search term returns recent walks.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_WithNullSearchTerm_ShouldReturnRecentWalks()
    {
        var expectedWalks = new List<Walk> { new() { Id = 1 } };
        _mockWalkRepository.Setup(x => x.GetRecentWalksAsync(50)).ReturnsAsync(expectedWalks);

        var result = await _walkService.SearchWalksAsync(null!);

        result.Should().BeEquivalentTo(expectedWalks);
        _mockWalkRepository.Verify(x => x.GetRecentWalksAsync(50), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.SearchWalksAsync(string)"/> with a valid search term calls the search method.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_WithValidSearchTerm_ShouldCallSearchMethod()
    {
        var searchTerm = "test search";
        var expectedWalks = new List<Walk> { new() { Id = 1, Notes = "test search notes" } };
        _mockWalkRepository.Setup(x => x.SearchWalksAsync(searchTerm)).ReturnsAsync(expectedWalks);

        var result = await _walkService.SearchWalksAsync(searchTerm);

        result.Should().BeEquivalentTo(expectedWalks);
        _mockWalkRepository.Verify(x => x.SearchWalksAsync(searchTerm), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.SaveWalkAsync(Walk)"/> with a new walk calls AddAsync.
    /// </summary>
    [Fact]
    public async Task SaveWalkAsync_WithNewWalk_ShouldCallAddAsync()
    {
        var newWalk = new Walk { Id = 0, ClientId = 1, DogId = 1 };
        var savedWalk = new Walk { Id = 1, ClientId = 1, DogId = 1 };
        _mockWalkRepository.Setup(x => x.AddAsync(newWalk)).ReturnsAsync(savedWalk);

        var result = await _walkService.SaveWalkAsync(newWalk);

        result.Should().Be(savedWalk);
        _mockWalkRepository.Verify(x => x.AddAsync(newWalk), Times.Once);
        _mockWalkRepository.Verify(x => x.UpdateAsync(It.IsAny<Walk>()), Times.Never);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.SaveWalkAsync(Walk)"/> with an existing walk calls UpdateAsync.
    /// </summary>
    [Fact]
    public async Task SaveWalkAsync_WithExistingWalk_ShouldCallUpdateAsync()
    {
        var existingWalk = new Walk { Id = 1, ClientId = 1, DogId = 1 };
        var updatedWalk = new Walk { Id = 1, ClientId = 1, DogId = 1 };
        _mockWalkRepository.Setup(x => x.UpdateAsync(existingWalk)).ReturnsAsync(updatedWalk);

        var result = await _walkService.SaveWalkAsync(existingWalk);

        result.Should().Be(updatedWalk);
        _mockWalkRepository.Verify(x => x.UpdateAsync(existingWalk), Times.Once);
        _mockWalkRepository.Verify(x => x.AddAsync(It.IsAny<Walk>()), Times.Never);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.DeleteWalkAsync(int)"/> calls the repository's DeleteAsync.
    /// </summary>
    [Fact]
    public async Task DeleteWalkAsync_ShouldCallRepositoryDeleteAsync()
    {
        var walkId = 123;

        await _walkService.DeleteWalkAsync(walkId);

        _mockWalkRepository.Verify(x => x.DeleteAsync(walkId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetOrCreateClientAsync(string, string)"/> returns an existing client when found.
    /// </summary>
    [Fact]
    public async Task GetOrCreateClientAsync_WhenClientExists_ShouldReturnExistingClient()
    {
        var name = "John Doe";
        var phone = "555-0123";
        var existingClient = new Client { Id = 1, Name = name, Phone = phone };
        _mockClientRepository.Setup(x => x.FindByNameAndPhoneAsync(name, phone)).ReturnsAsync(existingClient);

        var result = await _walkService.GetOrCreateClientAsync(name, phone);

        result.Should().Be(existingClient);
        _mockClientRepository.Verify(x => x.FindByNameAndPhoneAsync(name, phone), Times.Once);
        _mockClientRepository.Verify(x => x.AddAsync(It.IsAny<Client>()), Times.Never);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetOrCreateClientAsync(string, string)"/> creates a new client when not found.
    /// </summary>
    [Fact]
    public async Task GetOrCreateClientAsync_WhenClientDoesNotExist_ShouldCreateNewClient()
    {
        var name = "Jane Smith";
        var phone = "555-0456";
        var newClient = new Client { Id = 2, Name = name, Phone = phone, CreatedDate = DateTime.UtcNow };
        
        _mockClientRepository.Setup(x => x.FindByNameAndPhoneAsync(name, phone)).ReturnsAsync((Client)null!);
        _mockClientRepository.Setup(x => x.AddAsync(It.IsAny<Client>())).ReturnsAsync(newClient);

        var result = await _walkService.GetOrCreateClientAsync(name, phone);

        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.Phone.Should().Be(phone);
        result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        _mockClientRepository.Verify(x => x.FindByNameAndPhoneAsync(name, phone), Times.Once);
        _mockClientRepository.Verify(x => x.AddAsync(It.IsAny<Client>()), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetOrCreateDogAsync(int, string, string, int)"/> returns an existing dog without update.
    /// </summary>
    [Fact]
    public async Task GetOrCreateDogAsync_WhenDogExists_ShouldReturnExistingDogWithoutUpdate()
    {
        var clientId = 1;
        var name = "Buddy";
        var breed = "Golden Retriever";
        var age = 5;
        var existingDog = new Dog { Id = 1, ClientId = clientId, Name = name, Breed = breed, Age = age };
        
        _mockClientRepository.Setup(x => x.FindDogByClientAndNameAsync(clientId, name)).ReturnsAsync(existingDog);

        var result = await _walkService.GetOrCreateDogAsync(clientId, name, breed, age);

        result.Should().Be(existingDog);
        _mockClientRepository.Verify(x => x.FindDogByClientAndNameAsync(clientId, name), Times.Once);
        _mockClientRepository.Verify(x => x.UpdateDogAsync(It.IsAny<Dog>()), Times.Never);
        _mockClientRepository.Verify(x => x.AddDogAsync(It.IsAny<Dog>()), Times.Never);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetOrCreateDogAsync(int, string, string, int)"/> updates an existing dog when breed differs.
    /// </summary>
    [Fact]
    public async Task GetOrCreateDogAsync_WhenDogExistsButDifferentBreed_ShouldUpdateExistingDog()
    {
        var clientId = 1;
        var name = "Buddy";
        var newBreed = "Labrador";
        var age = 5;
        var existingDog = new Dog { Id = 1, ClientId = clientId, Name = name, Breed = "Golden Retriever", Age = age };
        
        _mockClientRepository.Setup(x => x.FindDogByClientAndNameAsync(clientId, name)).ReturnsAsync(existingDog);
        _mockClientRepository.Setup(x => x.UpdateDogAsync(existingDog)).ReturnsAsync(existingDog);

        var result = await _walkService.GetOrCreateDogAsync(clientId, name, newBreed, age);

        result.Should().Be(existingDog);
        result!.Breed.Should().Be(newBreed);
        _mockClientRepository.Verify(x => x.UpdateDogAsync(existingDog), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.GetOrCreateDogAsync(int, string, string, int)"/> creates a new dog when not found.
    /// </summary>
    [Fact]
    public async Task GetOrCreateDogAsync_WhenDogDoesNotExist_ShouldCreateNewDog()
    {
        var clientId = 1;
        var name = "Max";
        var breed = "German Shepherd";
        var age = 3;
        var newDog = new Dog { Id = 2, ClientId = clientId, Name = name, Breed = breed, Age = age, CreatedDate = DateTime.UtcNow };
        
        _mockClientRepository.Setup(x => x.FindDogByClientAndNameAsync(clientId, name)).ReturnsAsync((Dog)null!);
        _mockClientRepository.Setup(x => x.AddDogAsync(It.IsAny<Dog>())).ReturnsAsync(newDog);

        var result = await _walkService.GetOrCreateDogAsync(clientId, name, breed, age);

        result.Should().NotBeNull();
        result!.ClientId.Should().Be(clientId);
        result.Name.Should().Be(name);
        result.Breed.Should().Be(breed);
        result.Age.Should().Be(age);
        result.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        _mockClientRepository.Verify(x => x.AddDogAsync(It.IsAny<Dog>()), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.CreateClientAsync(Client)"/> calls the repository's AddAsync.
    /// </summary>
    [Fact]
    public async Task CreateClientAsync_ShouldCallRepositoryAddAsync()
    {
        var client = new Client { Name = "Test Client", Phone = "555-0001" };
        var createdClient = new Client { Id = 1, Name = "Test Client", Phone = "555-0001" };
        _mockClientRepository.Setup(x => x.AddAsync(client)).ReturnsAsync(createdClient);

        var result = await _walkService.CreateClientAsync(client);

        result.Should().Be(createdClient);
        _mockClientRepository.Verify(x => x.AddAsync(client), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.UpdateClientAsync(Client)"/> calls the repository's UpdateAsync.
    /// </summary>
    [Fact]
    public async Task UpdateClientAsync_ShouldCallRepositoryUpdateAsync()
    {
        var client = new Client { Id = 1, Name = "Updated Client", Phone = "555-0001" };
        var updatedClient = new Client { Id = 1, Name = "Updated Client", Phone = "555-0001" };
        _mockClientRepository.Setup(x => x.UpdateAsync(client)).ReturnsAsync(updatedClient);

        var result = await _walkService.UpdateClientAsync(client);

        result.Should().Be(updatedClient);
        _mockClientRepository.Verify(x => x.UpdateAsync(client), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.DeleteClientAsync(int)"/> calls the repository's DeleteAsync.
    /// </summary>
    [Fact]
    public async Task DeleteClientAsync_ShouldCallRepositoryDeleteAsync()
    {
        var clientId = 123;

        await _walkService.DeleteClientAsync(clientId);

        _mockClientRepository.Verify(x => x.DeleteAsync(clientId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.CreateDogAsync(Dog)"/> calls the repository's AddDogAsync.
    /// </summary>
    [Fact]
    public async Task CreateDogAsync_ShouldCallRepositoryAddDogAsync()
    {
        var dog = new Dog { ClientId = 1, Name = "Test Dog", Breed = "Test Breed", Age = 2 };
        var createdDog = new Dog { Id = 1, ClientId = 1, Name = "Test Dog", Breed = "Test Breed", Age = 2 };
        _mockClientRepository.Setup(x => x.AddDogAsync(dog)).ReturnsAsync(createdDog);

        var result = await _walkService.CreateDogAsync(dog);

        result.Should().Be(createdDog);
        _mockClientRepository.Verify(x => x.AddDogAsync(dog), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.UpdateDogAsync(Dog)"/> calls the repository's UpdateDogAsync.
    /// </summary>
    [Fact]
    public async Task UpdateDogAsync_ShouldCallRepositoryUpdateDogAsync()
    {
        var dog = new Dog { Id = 1, ClientId = 1, Name = "Updated Dog", Breed = "Updated Breed", Age = 3 };
        var updatedDog = new Dog { Id = 1, ClientId = 1, Name = "Updated Dog", Breed = "Updated Breed", Age = 3 };
        _mockClientRepository.Setup(x => x.UpdateDogAsync(dog)).ReturnsAsync(updatedDog);

        var result = await _walkService.UpdateDogAsync(dog);

        result.Should().Be(updatedDog);
        _mockClientRepository.Verify(x => x.UpdateDogAsync(dog), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="WalkService.DeleteDogAsync(int)"/> calls the repository's DeleteDogAsync.
    /// </summary>
    [Fact]
    public async Task DeleteDogAsync_ShouldCallRepositoryDeleteDogAsync()
    {
        var dogId = 123;

        await _walkService.DeleteDogAsync(dogId);

        _mockClientRepository.Verify(x => x.DeleteDogAsync(dogId), Times.Once);
    }
}