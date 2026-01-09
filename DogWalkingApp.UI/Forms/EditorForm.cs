using DogWalkingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DogWalkingApp.UI.Forms
{
    /// <summary>
    /// Abstract base class for editor forms that provides error message display and automatic clearing functionality.
    /// </summary>
    public class EditorForm : Form
    {
        private int _errorDisplayDurationMs = 3000;
        private System.Timers.Timer _errorClearTimer = new System.Timers.Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorForm"/> class.
        /// Sets up the error clearing timer based on configuration settings.
        /// </summary>
        protected EditorForm() 
        { 
            var errorMessagesTimeout = ConfigurationManager.AppSettings["errorMessagesTimeout"];
            if ((string.IsNullOrEmpty(errorMessagesTimeout) == false)
                && (int.TryParse(errorMessagesTimeout, out var duration)))
                    _errorDisplayDurationMs = duration * 1000;

            _errorClearTimer.Interval = _errorDisplayDurationMs;
            _errorClearTimer.AutoReset = false;
            _errorClearTimer.Elapsed += (s, e) =>
            {
                _errorClearTimer.Stop();
                var errorLabel = GetErrorLabelControl();
                if (errorLabel.InvokeRequired)
                    errorLabel.Invoke(new Action(ClearErrorLabel));
                else
                    ClearErrorLabel();
            };
        }

        /// <summary>
        /// Clears the text of the error label control.
        /// </summary>
        private void ClearErrorLabel()
        {
            var errorLabel = GetErrorLabelControl();
            errorLabel.Text = string.Empty;
        }

        /// <summary>
        /// Gets the error label control used for displaying validation errors.
        /// </summary>
        /// <returns>The <see cref="Label"/> control for error messages.</returns>
        protected virtual Label GetErrorLabelControl() => throw new NotImplementedException("Derived classes must implement GetErrorLabelControl method.");

        /// <summary>
        /// Validates the input value and displays an error message if invalid.
        /// Sets focus to the specified control and starts the error clearing timer.
        /// </summary>
        /// <param name="value">The validatable object to check.</param>
        /// <param name="controlToFocus">The control to focus if validation fails.</param>
        /// <returns>True if the input is valid; otherwise, false.</returns>
        internal bool ValidateInput(IValidatable value, Control controlToFocus)
        {
            var validationResult = value.IsValid();
            if (validationResult.IsValid)
                return true;

            var errorLabel = GetErrorLabelControl();
            errorLabel.Text = validationResult.GetErrorMessage();
            errorLabel.Visible = true;
            controlToFocus.Focus();
            _errorClearTimer.Start();
            return false;
        }
    }
}
