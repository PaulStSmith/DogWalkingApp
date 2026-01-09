namespace DogWalkingApp.UI
{
    /// <summary>
    /// Provides helper methods for converting and setting values.
    /// </summary>
    internal static class ConversionHelper
    {
        /// <summary>
        /// Attempts to parse the input string as an integer and sets the value using the provided setter action if successful.
        /// </summary>
        /// <param name="setter">The action to invoke with the parsed integer value.</param>
        /// <param name="input">The string input to parse as an integer.</param>
        public static void SetIntValue(Action<int> setter, string? input)
        {
            if (int.TryParse(input, out var result))
                setter(result);
        }
    }
}
