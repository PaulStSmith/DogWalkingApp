namespace DogWalkingApp.Domain.Entities
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the list of validation error messages.
        /// </summary>
        public IReadOnlyList<string> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class with no errors.
        /// </summary>
        public ValidationResult() : this([]) { }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class with the specified errors.
        /// </summary>
        /// <param name="errors">The collection of validation error messages.</param>
        public ValidationResult(IEnumerable<string> errors)
        {
            var errorList = errors?.ToList() ?? [];
            Errors = errorList.AsReadOnly();
            IsValid = errorList.Count == 0;
        }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class with a single error.
        /// </summary>
        /// <param name="error">The validation error message.</param>
        public ValidationResult(string error) : this([error]) { }

        /// <summary>
        /// Gets a successful validation result with no errors.
        /// </summary>
        public static ValidationResult Success => new ValidationResult();

        /// <summary>
        /// Gets a formatted error message containing all validation errors.
        /// </summary>
        /// <returns>A string containing all error messages separated by newlines.</returns>
        public string GetErrorMessage()
        {
            return string.Join("\n", Errors);
        }
    }
}