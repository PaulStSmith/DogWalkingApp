using DogWalkingApp.Resources;

namespace DogWalkingApp.Domain.Entities;

/// <summary>
/// Represents a walk in the dog walking application.
/// </summary>
public class Walk : IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the walk.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the client for the walk.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the dog for the walk.
    /// </summary>
    public int DogId { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the walk.
    /// </summary>
    public DateTime WalkDateTime { get; set; }

    /// <summary>
    /// Gets or sets the duration of the walk in minutes.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets any notes about the walk.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the walk was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    /// <summary>
    /// Gets or sets the client associated with the walk.
    /// </summary>
    public virtual Client Client { get; set; } = null!;

    /// <summary>
    /// Gets or sets the dog associated with the walk.
    /// </summary>
    public virtual Dog Dog { get; set; } = null!;

    /// <summary>
    /// Validates the walk entity.
    /// </summary>
    /// <returns>A ValidationResult indicating if the walk is valid and any error messages.</returns>
    public ValidationResult IsValid()
    {
        var errors = new List<string>();

        if (ClientId <= 0)
            errors.Add(ValidationMessages.ClientIdRequired);

        if (DogId <= 0)
            errors.Add(ValidationMessages.DogIdRequired);

        if (WalkDateTime > DateTime.Now.AddDays(1))
            errors.Add(ValidationMessages.WalkDateInvalid);

        if (WalkDateTime < DateTime.Now.AddYears(-1))
            errors.Add(ValidationMessages.WalkDateTooOld);

        if (DurationMinutes < 5 || DurationMinutes > 240)
            errors.Add(ValidationMessages.DurationInvalid);

        if (Notes?.Length > 500)
            errors.Add(ValidationMessages.NotesTooLong);

        return new ValidationResult(errors);
    }
}