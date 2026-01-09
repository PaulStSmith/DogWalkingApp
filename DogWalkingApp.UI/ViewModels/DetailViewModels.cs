using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DogWalkingApp.UI.ViewModels
{
    /// <summary>
    /// ViewModel for client details (SIMPLE BINDING with INotifyPropertyChanged)
    /// </summary>
    public class ClientDetailViewModel : INotifyPropertyChanged
    {
        private int _clientId;
        private string _name = string.Empty;
        private string _phone = string.Empty;
        private bool _isEditing = false;

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public int ClientId
        {
            get => _clientId;
            set => SetField(ref _clientId, value);
        }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        /// <summary>
        /// Gets or sets the client phone.
        /// </summary>
        public string Phone
        {
            get => _phone;
            set => SetField(ref _phone, value);
        }

        /// <summary>
        /// Gets or sets whether the client is in edit mode.
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set => SetField(ref _isEditing, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    /// <summary>
    /// ViewModel for dog rows in DataGridView (COMPLEX BINDING)
    /// </summary>
    public class DogGridRowViewModel
    {
        /// <summary>
        /// Gets or sets the dog ID.
        /// </summary>
        public int DogId { get; set; }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the dog name.
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dog breed.
        /// </summary>
        [DisplayName("Breed")]
        public string Breed { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dog age.
        /// </summary>
        [DisplayName("Age")]
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the walk count for this dog.
        /// </summary>
        [DisplayName("Walks")]
        public int WalkCount { get; set; }
    }

    /// <summary>
    /// ViewModel for walk rows in DataGridView (COMPLEX BINDING)
    /// </summary>
    public class WalkGridRowViewModel
    {
        /// <summary>
        /// Gets or sets the walk ID.
        /// </summary>
        public int WalkId { get; set; }

        /// <summary>
        /// Gets or sets the client ID.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the dog ID.
        /// </summary>
        public int DogId { get; set; }

        /// <summary>
        /// Gets or sets the walk date and time.
        /// </summary>
        public DateTime WalkDateTime { get; set; }

        /// <summary>
        /// Gets or sets the duration in minutes.
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the walk notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets the display-friendly date.
        /// </summary>
        [DisplayName("Date")]
        public string DateDisplay => WalkDateTime.ToString("MMM dd, yyyy");

        /// <summary>
        /// Gets the display-friendly time.
        /// </summary>
        [DisplayName("Time")]
        public string TimeDisplay => WalkDateTime.ToString("HH:mm");

        /// <summary>
        /// Gets the display-friendly duration.
        /// </summary>
        [DisplayName("Duration")]
        public string DurationDisplay => $"{DurationMinutes} min";

        /// <summary>
        /// Gets the truncated notes for display.
        /// </summary>
        [DisplayName("Notes")]
        public string NotesDisplay => Notes?.Length > 30 ? Notes[..30] + "..." : Notes ?? "";
    }
}