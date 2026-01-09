using DogWalkingApp.Resources;

namespace DogWalkingApp.Domain.Entities;

/// <summary>
/// Represents a dog in the dog walking application.
/// </summary>
public class Dog : IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the dog.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the client who owns the dog.
    /// </summary>
    public int ClientId { get; set; }

    /// <summary>
    /// Gets or sets the name of the dog.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the breed of the dog.
    /// </summary>
    public string Breed { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the age of the dog.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the dog was added.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    /// <summary>
    /// Gets or sets the client associated with the dog.
    /// </summary>
    public virtual Client Client { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of walks associated with the dog.
    /// </summary>
    public virtual ICollection<Walk> Walks { get; set; } = [];

    /// <summary>
    /// Validates the dog entity.
    /// </summary>
    /// <returns>A ValidationResult indicating if the dog is valid and any error messages.</returns>
    public ValidationResult IsValid()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(ValidationMessages.DogNameRequired);

        if (Name?.Length > 50)
            errors.Add(ValidationMessages.DogNameTooLong);

        if (string.IsNullOrWhiteSpace(Breed))
            errors.Add(ValidationMessages.BreedRequired);

        if (Breed?.Length > 50)
            errors.Add(ValidationMessages.BreedTooLong);

        if (Age < 1 || Age > 30)
            errors.Add(ValidationMessages.AgeInvalid);

        if (ClientId <= 0)
            errors.Add(ValidationMessages.ClientIdRequired);

        return new ValidationResult(errors);
    }
}