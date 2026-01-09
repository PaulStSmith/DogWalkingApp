using DogWalkingApp.Data;
using DogWalkingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogWalkingApp.Tests;

/// <summary>
/// Base class for tests that provides in-memory database setup.
/// </summary>
public abstract class TestBase : IDisposable
{
    /// <summary>
    /// Gets the in-memory database context for testing.
    /// </summary>
    protected DogWalkingContext Context { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class with an in-memory database.
    /// </summary>
    protected TestBase()
    {
        var options = new DbContextOptionsBuilder<DogWalkingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new DogWalkingContext(options);
        Context.Database.EnsureCreated();
    }

    /// <summary>
    /// Seeds the test database with sample data asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected async Task SeedTestDataAsync()
    {
        var client = new Client
        {
            Name = "Test Client",
            Phone = "123-456-7890",
            CreatedDate = DateTime.UtcNow
        };

        var dog = new Dog
        {
            Name = "Test Dog",
            Breed = "Golden Retriever",
            Age = 3,
            ClientId = 1,
            CreatedDate = DateTime.UtcNow
        };

        var walk = new Walk
        {
            ClientId = 1,
            DogId = 1,
            WalkDateTime = DateTime.UtcNow.AddHours(-1),
            DurationMinutes = 30,
            Notes = "Test walk notes",
            CreatedDate = DateTime.UtcNow
        };

        Context.Clients.Add(client);
        Context.Dogs.Add(dog);
        Context.Walks.Add(walk);
        await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Disposes the database context and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}