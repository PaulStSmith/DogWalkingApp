namespace DogWalkingApp.Domain.Entities
{
    /// <summary>
    /// Defines a contract for objects that can be validated and provide validation results.
    /// </summary>
    /// <remarks>
    /// Implementations should use the IsValid method to perform validation logic and return a
    /// <see cref="ValidationResult"/> that indicates whether the object is valid and includes any relevant error messages. 
    /// This interface is typically used to ensure that entities meet required business or data integrity rules before
    /// further processing.
    /// </remarks>
    public interface IValidatable
    {
        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <returns>A <see cref="ValidationResult"/> indicating if the entity is valid and any error messages.</returns>
        ValidationResult IsValid();
    }
}