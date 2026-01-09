using System.Text.RegularExpressions;
using DogWalkingApp.Resources;

namespace DogWalkingApp.Domain.Entities;

/// <summary>
/// Represents a client in the dog walking application.
/// </summary>
public class Client : IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the client.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the client.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the client was added.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    // Navigation properties
    /// <summary>
    /// Gets or sets the collection of dogs owned by the client.
    /// </summary>
    public virtual ICollection<Dog> Dogs { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of walks associated with the client.
    /// </summary>
    public virtual ICollection<Walk> Walks { get; set; } = [];

    /// <summary>
    /// Validates the client entity.
    /// </summary>
    /// <returns>A ValidationResult indicating if the client is valid and any error messages.</returns>
    public ValidationResult IsValid()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(ValidationMessages.ClientNameRequired);

        if (Name?.Length > 100)
            errors.Add(ValidationMessages.ClientNameTooLong);

        if (string.IsNullOrWhiteSpace(Phone))
            errors.Add(ValidationMessages.PhoneRequired);

        if (!string.IsNullOrWhiteSpace(Phone) && !IsValidPhoneNumber(Phone))
            errors.Add(ValidationMessages.PhoneInvalid);

        return new ValidationResult(errors);
    }

    /// <summary>
    /// Validates if a phone number has a valid format.
    /// </summary>
    /// <param name="phone">The phone number to validate.</param>
    /// <returns>True if the phone number is valid; otherwise, false.</returns>
    private static bool IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        // Remove all non-digit characters for validation
        var digitsOnly = Regex.Replace(phone, @"\D", "");
        
        // Accept 10-digit numbers (US format) or 11-digit numbers (with country code)
        return digitsOnly.Length is 10 or 11;
    }
}