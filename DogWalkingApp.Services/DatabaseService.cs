using Microsoft.EntityFrameworkCore;
using System.Configuration;
using DogWalkingApp.Data;

namespace DogWalkingApp.Services;

/// <summary>
/// Service class for database operations.
/// </summary>
public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseService"/> class.
    /// </summary>
    public DatabaseService()
    {
        _connectionString = GetConnectionString();
    }

    /// <inheritdoc/>
    public DogWalkingContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DogWalkingContext>();
        optionsBuilder.UseSqlServer(_connectionString);
        return new DogWalkingContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public string GetConnectionString()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["DogWalkingDb"]?.ConnectionString;
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DogWalkingDb' not found in configuration. " +
                "Please ensure App.config contains the connection string.");
        }

        return connectionString;
    }
}