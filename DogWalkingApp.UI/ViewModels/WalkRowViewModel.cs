using System.ComponentModel;

namespace DogWalkingApp.UI.ViewModels
{
    /// <summary>
    /// ViewModel for displaying walks in DataGridView (COMPLEX BINDING)
    /// </summary>
    public class WalkRowViewModel
    {
        /// <summary>
        /// Gets or sets the walk identifier.
        /// </summary>
        public int WalkId { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the dog identifier.
        /// </summary>
        public int DogId { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client phone.
        /// </summary>
        public string ClientPhone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dog name.
        /// </summary>
        public string DogName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dog breed.
        /// </summary>
        public string DogBreed { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the walk date and time.
        /// </summary>
        public DateTime WalkDateTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the walk in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the notes for the walk.
        /// </summary>
        public string? Notes { get; set; }

        // Display-friendly properties
        /// <summary>
        /// Gets the display-friendly walk date and time.
        /// </summary>
        [DisplayName("Date")]
        public string WalkDateDisplay => WalkDateTime.ToString("yyyy-MM-dd HH:mm");
        
        /// <summary>
        /// Gets the display-friendly client name.
        /// </summary>
        [DisplayName("Client")]
        public string ClientDisplay => ClientName;
        
        /// <summary>
        /// Gets the display-friendly dog name and breed.
        /// </summary>
        [DisplayName("Dog")]
        public string DogDisplay => $"{DogName} ({DogBreed})";
        
        /// <summary>
        /// Gets the display-friendly duration.
        /// </summary>
        [DisplayName("Duration")]
        public string DurationDisplay => $"{DurationMinutes} min";
    }
}