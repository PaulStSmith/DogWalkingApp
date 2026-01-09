using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DogWalkingApp.UI.ViewModels
{
    /// <summary>
    /// ViewModel for editing walk details (SIMPLE BINDING with INotifyPropertyChanged)
    /// </summary>
    public class WalkDetailViewModel : INotifyPropertyChanged
    {
        private int _walkId;
        private int _clientId;
        private int _dogId;
        private string _clientName = string.Empty;
        private string _clientPhone = string.Empty;
        private string _dogName = string.Empty;
        private string _dogBreed = string.Empty;
        private int _dogAge;
        private DateTime _walkDateTime = DateTime.Now;
        private int _durationMinutes = 30;
        private string? _notes;

        /// <summary>
        /// Gets or sets the walk identifier.
        /// </summary>
        public int WalkId
        {
            get => _walkId;
            set => SetField(ref _walkId, value);
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public int ClientId
        {
            get => _clientId;
            set => SetField(ref _clientId, value);
        }

        /// <summary>
        /// Gets or sets the dog identifier.
        /// </summary>
        public int DogId
        {
            get => _dogId;
            set => SetField(ref _dogId, value);
        }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName
        {
            get => _clientName;
            set => SetField(ref _clientName, value);
        }

        /// <summary>
        /// Gets or sets the client phone.
        /// </summary>
        public string ClientPhone
        {
            get => _clientPhone;
            set => SetField(ref _clientPhone, value);
        }

        /// <summary>
        /// Gets or sets the dog name.
        /// </summary>
        public string DogName
        {
            get => _dogName;
            set => SetField(ref _dogName, value);
        }

        /// <summary>
        /// Gets or sets the dog breed.
        /// </summary>
        public string DogBreed
        {
            get => _dogBreed;
            set => SetField(ref _dogBreed, value);
        }

        /// <summary>
        /// Gets or sets the dog age.
        /// </summary>
        public int DogAge
        {
            get => _dogAge;
            set => SetField(ref _dogAge, value);
        }

        /// <summary>
        /// Gets or sets the walk date and time.
        /// </summary>
        public DateTime WalkDateTime
        {
            get => _walkDateTime;
            set => SetField(ref _walkDateTime, value);
        }

        /// <summary>
        /// Gets or sets the duration of the walk in minutes.
        /// </summary>
        public int DurationMinutes
        {
            get => _durationMinutes;
            set => SetField(ref _durationMinutes, value);
        }

        /// <summary>
        /// Gets or sets the notes for the walk.
        /// </summary>
        public string? Notes
        {
            get => _notes;
            set => SetField(ref _notes, value);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field value and raises PropertyChanged if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="field">The field reference.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the value was set; otherwise, false.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}