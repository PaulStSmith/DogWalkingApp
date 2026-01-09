using Microsoft.EntityFrameworkCore;
using DogWalkingApp.Domain.Entities;

namespace DogWalkingApp.Data.Repositories;

/// <summary>
/// Repository class for walk operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="WalkRepository"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
public class WalkRepository(DogWalkingContext context) : IWalkRepository
{
    private readonly DogWalkingContext _context = context;

    /// <inheritdoc/>
    public async Task<IEnumerable<Walk>> GetRecentWalksAsync(int count = 50)
    {
        return await _context.Walks
            .Include(w => w.Client)
            .Include(w => w.Dog)
            .OrderByDescending(w => w.WalkDateTime)
            .Take(count)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Walk>> SearchWalksAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        
        return await _context.Walks
            .Include(w => w.Client)
            .Include(w => w.Dog)
            .Where(w => w.Client.Name.Contains(lowerSearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       w.Client.Phone.Contains(searchTerm) ||
                       w.Dog.Name.Contains(lowerSearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       w.Dog.Breed.Contains(lowerSearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                       (w.Notes != null && w.Notes.ToLower().Contains(lowerSearchTerm)))
            .OrderByDescending(w => w.WalkDateTime)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Walk> AddAsync(Walk walk)
    {
        walk.CreatedDate = DateTime.UtcNow;
        _context.Walks.Add(walk);
        await _context.SaveChangesAsync();
        return walk;
    }

    /// <inheritdoc/>
    public async Task<Walk> UpdateAsync(Walk walk)
    {
        var existingWalk = await _context.Walks.FindAsync(walk.Id);
        if (existingWalk != null)
        {
            // Update only the fields we want to change
            existingWalk.ClientId = walk.ClientId;
            existingWalk.DogId = walk.DogId;
            existingWalk.WalkDateTime = walk.WalkDateTime;
            existingWalk.DurationMinutes = walk.DurationMinutes;
            existingWalk.Notes = walk.Notes;
            await _context.SaveChangesAsync();
            return existingWalk;
        }
        
        // If not found, add as new (fallback)
        _context.Walks.Add(walk);
        await _context.SaveChangesAsync();
        return walk;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int walkId)
    {
        var walk = await _context.Walks.FindAsync(walkId);
        if (walk != null)
        {
            _context.Walks.Remove(walk);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<Walk?> GetByIdAsync(int walkId)
    {
        return await _context.Walks
            .Include(w => w.Client)
            .Include(w => w.Dog)
            .FirstOrDefaultAsync(w => w.Id == walkId);
    }
}