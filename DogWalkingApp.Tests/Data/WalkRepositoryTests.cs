    using DogWalkingApp.Data.Repositories;
using DogWalkingApp.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DogWalkingApp.Tests.Data;

/// <summary>
/// Contains unit tests for the WalkRepository class.
/// </summary>
public class WalkRepositoryTests : TestBase
{
    private readonly WalkRepository _walkRepository;

    public WalkRepositoryTests()
    {
        _walkRepository = new WalkRepository(Context);
    }

    /// <summary>
    /// Tests that GetRecentWalksAsync returns walks ordered by date descending.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_ShouldReturnWalksOrderedByDateDescending()
    {
        await SeedMultipleWalksAsync();

        var result = await _walkRepository.GetRecentWalksAsync(10);

        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCountGreaterThan(2);
        
        var walksList = result.ToList();
        for (var i = 0; i < walksList.Count - 1; i++)
        {
            walksList[i].WalkDateTime.Should().BeAfter(walksList[i + 1].WalkDateTime);
        }
    }

    /// <summary>
    /// Tests that GetRecentWalksAsync respects the count parameter.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_ShouldRespectCountParameter()
    {
        await SeedMultipleWalksAsync();

        var result = await _walkRepository.GetRecentWalksAsync(2);

        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that GetRecentWalksAsync includes navigation properties.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_ShouldIncludeNavigationProperties()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.GetRecentWalksAsync(10);

        result.Should().NotBeNullOrEmpty();
        var walk = result.First();
        walk.Client.Should().NotBeNull();
        walk.Dog.Should().NotBeNull();
        walk.Client.Name.Should().Be("Test Client");
        walk.Dog.Name.Should().Be("Test Dog");
    }

    /// <summary>
    /// Tests that GetRecentWalksAsync with multiple walks returns all.
    /// </summary>
    [Fact]
    public async Task GetRecentWalksAsync_WithMultipleWalks_ShouldReturnAll()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.GetRecentWalksAsync(10);

        result.Should().NotBeEmpty();
        result.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that SearchWalksAsync finds walks by client name.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_ShouldFindWalksByClientName()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.SearchWalksAsync("Test Client");

        result.Should().NotBeNullOrEmpty();
        result.First().Client.Name.Should().Be("Test Client");
    }

    /// <summary>
    /// Tests that SearchWalksAsync finds walks by dog name.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_ShouldFindWalksByDogName()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.SearchWalksAsync("Test Dog");

        result.Should().NotBeNullOrEmpty();
        result.First().Dog.Name.Should().Be("Test Dog");
    }

    /// <summary>
    /// Tests that SearchWalksAsync finds walks by notes.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_ShouldFindWalksByNotes()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.SearchWalksAsync("Test walk notes");

        result.Should().NotBeNullOrEmpty();
        result.First().Notes.Should().Be("Test walk notes");
    }

    /// <summary>
    /// Tests that SearchWalksAsync is case insensitive.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_ShouldBeCaseInsensitive()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.SearchWalksAsync("test client");

        result.Should().NotBeNullOrEmpty();
        result.First().Client.Name.Should().Be("Test Client");
    }

    /// <summary>
    /// Tests that SearchWalksAsync returns empty for no matches.
    /// </summary>
    [Fact]
    public async Task SearchWalksAsync_ShouldReturnEmptyForNoMatches()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.SearchWalksAsync("NonExistentSearchTerm");

        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that AddAsync adds a walk to the database.
    /// </summary>
    [Fact]
    public async Task AddAsync_ShouldAddWalkToDatabase()
    {
        await SeedTestDataAsync();

        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.UtcNow,
            DurationMinutes = 45,
            Notes = "New walk notes",
            CreatedDate = DateTime.UtcNow
        };

        var result = await _walkRepository.AddAsync(walk);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.DurationMinutes.Should().Be(45);
        result.Notes.Should().Be("New walk notes");

        var walkInDb = await Context.Walks.FirstOrDefaultAsync(w => w.Id == result.Id);
        walkInDb.Should().NotBeNull();
        walkInDb!.DurationMinutes.Should().Be(45);
    }

    /// <summary>
    /// Tests that UpdateAsync updates a walk in the database.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ShouldUpdateWalkInDatabase()
    {
        await SeedTestDataAsync();

        var walk = await Context.Walks.FirstAsync();
        walk.DurationMinutes = 60;
        walk.Notes = "Updated notes";

        var result = await _walkRepository.UpdateAsync(walk);

        result.Should().NotBeNull();
        result.DurationMinutes.Should().Be(60);
        result.Notes.Should().Be("Updated notes");

        var walkInDb = await Context.Walks.FirstOrDefaultAsync(w => w.Id == walk.Id);
        walkInDb.Should().NotBeNull();
        walkInDb!.DurationMinutes.Should().Be(60);
        walkInDb.Notes.Should().Be("Updated notes");
    }

    /// <summary>
    /// Tests that DeleteAsync removes a walk from the database.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveWalkFromDatabase()
    {
        await SeedTestDataAsync();

        var walk = await Context.Walks.FirstAsync();
        var walkId = walk.Id;

        await _walkRepository.DeleteAsync(walkId);

        var deletedWalk = await Context.Walks.FirstOrDefaultAsync(w => w.Id == walkId);
        deletedWalk.Should().BeNull();
    }

    /// <summary>
    /// Tests that GetByIdAsync returns a walk when it exists.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_WhenWalkExists_ShouldReturnWalk()
    {
        await SeedTestDataAsync();

        var result = await _walkRepository.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Notes.Should().Be("Test walk notes");
    }

    /// <summary>
    /// Tests that GetByIdAsync returns null when walk does not exist.
    /// </summary>
    [Fact]
    public async Task GetByIdAsync_WhenWalkDoesNotExist_ShouldReturnNull()
    {
        var result = await _walkRepository.GetByIdAsync(999);

        result.Should().BeNull();
    }

    /// <summary>
    /// Tests that DeleteAsync does not throw when walk does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_WhenWalkDoesNotExist_ShouldNotThrow()
    {
        var action = async () => await _walkRepository.DeleteAsync(999);

        await action.Should().NotThrowAsync();
    }

    /// <summary>
    /// Seeds multiple walks for testing.
    /// </summary>
    private async Task SeedMultipleWalksAsync()
    {
        await SeedTestDataAsync();

        var additionalWalks = new List<Walk>
        {
            new() {
                ClientId = 1,
                DogId = 1,
                WalkDateTime = DateTime.UtcNow.AddHours(-2),
                DurationMinutes = 20,
                Notes = "Earlier walk",
                CreatedDate = DateTime.UtcNow
            },
            new() {
                ClientId = 1,
                DogId = 1,
                WalkDateTime = DateTime.UtcNow.AddHours(-3),
                DurationMinutes = 25,
                Notes = "Even earlier walk",
                CreatedDate = DateTime.UtcNow
            }
        };

        Context.Walks.AddRange(additionalWalks);
        await Context.SaveChangesAsync();
    }
}