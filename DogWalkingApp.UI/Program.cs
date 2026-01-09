using DogWalkingApp.Services;
using DogWalkingApp.Resources;
using Microsoft.EntityFrameworkCore;

namespace DogWalkingApp.UI;

/// <summary>
/// The main program class for the DogWalkingApp UI.
/// </summary>
static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Ensure database exists and create default user
        await EnsureDatabaseSetupAsync();

        // Show login form first
        using var loginForm = new LoginForm();
        var loginResult = loginForm.ShowDialog();

        if (loginResult == DialogResult.OK && loginForm.LoginSuccessful)
        {
            // Login successful, show main application
            Application.Run(new MainForm());
        }
        // If login cancelled or failed, application exits
    }

    /// <summary>
    /// Ensures the database is set up and creates a default user if needed.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task EnsureDatabaseSetupAsync()
    {
        try
        {
            var databaseService = new DatabaseService();
            
            using var context = databaseService.CreateContext();
            
            // Apply any pending migrations
            await context.Database.MigrateAsync();
            
            // Create default user if needed
            var authService = new AuthenticationService(context);
            await authService.CreateDefaultUserAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(UserMessages.DatabaseSetupError, ex.Message), UserMessages.Error, 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.Exit(1);
        }
    }

    /// <summary>
    /// Sets the application culture for internationalization.
    /// </summary>
    /// <param name="cultureCode">Culture code (e.g., "en-US", "es-ES")</param>
    /// <remarks>
    /// For testing purposes, you can change the culture code to see the application in different languages.
    /// When the app is deployed, the culture is based on the system settings.
    /// </remarks>
    private static void SetApplicationCulture(string cultureCode)
    {
        try
        {
            var culture = new System.Globalization.CultureInfo(cultureCode);
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            
            // Debug output for verification
            System.Diagnostics.Debug.WriteLine($"Application culture set to: {culture.DisplayName} ({cultureCode})");
        }
        catch (System.Globalization.CultureNotFoundException ex)
        {
            System.Diagnostics.Debug.WriteLine($"Culture '{cultureCode}' not found. Using default culture. Error: {ex.Message}");
        }
    }
}