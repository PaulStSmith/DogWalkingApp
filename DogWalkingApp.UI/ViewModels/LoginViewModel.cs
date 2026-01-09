using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DogWalkingApp.UI.ViewModels
{
    /// <summary>
    /// ViewModel for login form (SIMPLE BINDING with INotifyPropertyChanged)
    /// </summary>
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the login is loading.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
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