using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using DogWalkingApp.Data;
using DogWalkingApp.Services;
using DogWalkingApp.UI.ViewModels;
using DogWalkingApp.Resources;

namespace DogWalkingApp.UI;

// Disable naming convention warnings for UI controls
#pragma warning disable IDE1006 

/// <summary>
/// The login form of the DogWalkingApp UI.
/// </summary>
public partial class LoginForm : Form
{
    // SIMPLE BINDING: Individual controls bound to single object properties
    private BindingSource _loginBindingSource = null!;
    private LoginViewModel _loginViewModel = null!;
    private IAuthenticationService _authService = null!;

    /// <summary>
    /// Gets a value indicating whether the login was successful.
    /// </summary>
    public bool LoginSuccessful { get; private set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginForm"/> class.
    /// </summary>
    public LoginForm()
    {
        InitializeComponent();
        LoadLocalizedText();
        InitializeDataBinding();
    }

    /// <summary>
    /// Loads localized text from resources for all UI controls.
    /// </summary>
    private void LoadLocalizedText()
    {
        // Form title and main labels
        Text = FormTitles.Login;
        lblTitle.Text = Labels.ApplicationTitle;
        
        // Field labels
        lblUsername.Text = Labels.Username;
        lblPassword.Text = Labels.Password;
        
        // Group box
        grpLogin.Text = Labels.LoginCredentials;
        
        // Apply localized sizing
        ApplyLocalizedSizing();
    }
    
    /// <summary>
    /// Applies culture-specific sizing to UI elements.
    /// </summary>
    private void ApplyLocalizedSizing()
    {
        ConversionHelper.SetIntValue(value => lblUsername.Width = value, UISizes.LabelUsernameWidth);
        ConversionHelper.SetIntValue(value => lblPassword.Width = value, UISizes.LabelPasswordWidth);
        ConversionHelper.SetIntValue(value => grpLogin.Width = value, UISizes.GroupLoginCredentials);
        ConversionHelper.SetIntValue(value => this.Width = value, UISizes.FormLoginWidth);
        ConversionHelper.SetIntValue(value => txtUsername.Left = value, UISizes.UsernameInputLeft);
        ConversionHelper.SetIntValue(value => txtPassword.Left = value, UISizes.PasswordInputLeft);
        ConversionHelper.SetIntValue(value => btnLogin.Width = value, UISizes.ButtonLoginWidth);
        ConversionHelper.SetIntValue(value => btnCancel.Width = value, UISizes.ButtonCancelWidth);
        ConversionHelper.SetIntValue(value => btnLogin.Left = value, UISizes.ButtonLoginLeft);
    }

    /// <summary>
    /// Initializes the data binding for the form.
    /// </summary>
    private void InitializeDataBinding()
    {
        // Initialize database context and services
        var databaseService = new DatabaseService();
        var context = databaseService.CreateContext();

        _authService = new AuthenticationService(context);

        // SIMPLE BINDING: Individual controls bound to single object properties
        _loginViewModel = new LoginViewModel();
        _loginBindingSource = new BindingSource
        {
            DataSource = _loginViewModel
        };

        SetupSimpleBinding();

        // Wire up events
        btnLogin.Click += btnLogin_Click;
        btnCancel.Click += btnCancel_Click;

        // Allow Enter key to submit
        this.AcceptButton = btnLogin;
        this.CancelButton = btnCancel;
    }

    /// <summary>
    /// Sets up the simple binding for the login controls.
    /// </summary>
    private void SetupSimpleBinding()
    {
        // SIMPLE BINDING: Each control bound to a single property
        // DataSourceUpdateMode.OnPropertyChanged ensures immediate updates

        txtUsername.DataBindings.Add(
            "Text",
            _loginBindingSource,
            nameof(LoginViewModel.Username),
            true,
            DataSourceUpdateMode.OnPropertyChanged
        );

        txtPassword.DataBindings.Add(
            "Text",
            _loginBindingSource,
            nameof(LoginViewModel.Password),
            true,
            DataSourceUpdateMode.OnPropertyChanged
        );

        lblErrorMessage.DataBindings.Add(
            "Text",
            _loginBindingSource,
            nameof(LoginViewModel.ErrorMessage),
            true,
            DataSourceUpdateMode.OnPropertyChanged
        );

        // Bind the enabled state of login button to IsLoading (inverted)
        _loginViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(LoginViewModel.IsLoading))
            {
                btnLogin.Enabled = !_loginViewModel.IsLoading;
            }
        };
    }

    /// <summary>
    /// Handles the Click event of the Login button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            _loginViewModel.IsLoading = true;
            _loginViewModel.ErrorMessage = string.Empty;

            // Simple validation
            if (string.IsNullOrWhiteSpace(_loginViewModel.Username) ||
                string.IsNullOrWhiteSpace(_loginViewModel.Password))
            {
                _loginViewModel.ErrorMessage = UserMessages.PleaseEnterCredentials;
                return;
            }

            // Authenticate user
            var isAuthenticated = await _authService.AuthenticateAsync(_loginViewModel.Username, _loginViewModel.Password);

            if (isAuthenticated)
            {
                LoginSuccessful = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                _loginViewModel.ErrorMessage = UserMessages.InvalidCredentials;
            }
        }
        catch (Exception ex)
        {
            _loginViewModel.ErrorMessage = string.Format(UserMessages.LoginError, ex.Message);
        }
        finally
        {
            _loginViewModel.IsLoading = false;
        }
    }

    /// <summary>
    /// Handles the Click event of the Cancel button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void btnCancel_Click(object? sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}