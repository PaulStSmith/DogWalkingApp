using Microsoft.EntityFrameworkCore;
using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Data;

/// <summary>
/// The Entity Framework database context for the DogWalking application.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DogWalkingContext"/> class.
/// </remarks>
/// <param name="options">The options for this context.</param>
public class DogWalkingContext(DbContextOptions<DogWalkingContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the clients DbSet.
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// Gets or sets the dogs DbSet.
    /// </summary>
    public DbSet<Dog> Dogs { get; set; }

    /// <summary>
    /// Gets or sets the walks DbSet.
    /// </summary>
    public DbSet<Walk> Walks { get; set; }

    /// <summary>
    /// Gets or sets the users DbSet.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureClient(modelBuilder);
        ConfigureDog(modelBuilder);
        ConfigureWalk(modelBuilder);
        ConfigureUser(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Configures the Client entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureClient(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedDate).IsRequired();

            entity.HasMany(e => e.Dogs)
                  .WithOne(e => e.Client)
                  .HasForeignKey(e => e.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Walks)
                  .WithOne(e => e.Client)
                  .HasForeignKey(e => e.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the Dog entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureDog(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Breed).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.CreatedDate).IsRequired();

            entity.HasMany(e => e.Walks)
                  .WithOne(e => e.Dog)
                  .HasForeignKey(e => e.DogId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ClientId).HasDatabaseName("IX_Dogs_ClientId");
        });
    }

    /// <summary>
    /// Configures the Walk entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureWalk(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Walk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WalkDateTime).IsRequired();
            entity.Property(e => e.DurationMinutes).IsRequired();
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).IsRequired();

            entity.HasIndex(e => e.ClientId).HasDatabaseName("IX_Walks_ClientId");
            entity.HasIndex(e => e.DogId).HasDatabaseName("IX_Walks_DogId");
            entity.HasIndex(e => e.WalkDateTime).HasDatabaseName("IX_Walks_WalkDateTime");
        });
    }

    /// <summary>
    /// Configures the User entity.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedDate).IsRequired();

            entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("IX_Users_Username");
        });
    }
}