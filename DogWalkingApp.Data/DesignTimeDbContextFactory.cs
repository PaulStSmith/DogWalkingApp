using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace DogWalkingApp.Data;

/// <summary>
/// Factory class for creating DbContext instances at design time.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DogWalkingContext>
{
    /// <summary>
    /// Creates a new instance of the <see cref="DogWalkingContext"/> class.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The created DbContext instance.</returns>
    public DogWalkingContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DogWalkingContext>();
        
        // Get connection string from configuration
        var connectionString = (ConfigurationManager.ConnectionStrings["DogWalkingDb"]?.ConnectionString) 
            ?? throw new ConfigurationErrorsException("The connection string 'DogWalkingDb' is required in the configuration for design-time DbContext creation.");
        optionsBuilder.UseSqlServer(connectionString);

        return new DogWalkingContext(optionsBuilder.Options);
    }
}