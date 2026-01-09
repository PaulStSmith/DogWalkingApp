using System.ComponentModel;
using DogWalkingApp.Domain.Entities;
using DogWalkingApp.UI.ViewModels;
using DogWalkingApp.Resources;

namespace DogWalkingApp.UI.Forms;

/// <summary>
/// Form for editing walk details with data binding.
/// </summary>
public partial class WalkEditorForm : EditorForm
{
    private WalkEditorViewModel _viewModel = null!;
    private readonly bool _isNewWalk;
    
    /// <summary>
    /// Gets the edited walk entity after saving.
    /// </summary>
    public Walk? EditedWalk { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WalkEditorForm"/> class.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="dogId">The ID of the dog.</param>
    /// <param name="dogName">The name of the dog.</param>
    /// <param name="existingWalk">The existing walk to edit, or null for a new walk.</param>
    public WalkEditorForm(int clientId, int dogId, string dogName, Walk? existingWalk = null)
    {
        InitializeComponent();
        LoadLocalizedText(dogName);
        _isNewWalk = existingWalk == null;
        
        InitializeDataBinding(clientId, dogId, dogName, existingWalk);
        ConfigureForm();
    }

    /// <summary>
    /// Loads localized text from resources for all UI controls.
    /// </summary>
    /// <param name="dogName">The name of the dog for display.</param>
    private void LoadLocalizedText(string dogName)
    {
        // Form title
        Text = FormTitles.WalkEditor;
        lblTitle.Text = FormTitles.WalkEditorWithIcon;
        
        // Dynamic dog info label
        lblDogInfo.Text = Labels.WalkFor.Replace("[Dog]", dogName);
        
        // Field labels
        lblDateTime.Text = Labels.DateTime;
        lblDuration.Text = Labels.DurationMinutes;
        lblMinutes.Text = Labels.Minutes;
        lblNotes.Text = Labels.Notes;
        
        // Buttons
        btnSave.Text = ButtonText.Save;
        btnCancel.Text = ButtonText.Cancel;
        
        // Apply localized sizing
        ApplyLocalizedSizing();
    }
    
    /// <summary>
    /// Applies culture-specific sizing to UI elements.
    /// </summary>
    private void ApplyLocalizedSizing()
    {
        ConversionHelper.SetIntValue(w => btnSave.Width = w, UISizes.ButtonSaveWidth);
        ConversionHelper.SetIntValue(w => btnCancel.Width = w, UISizes.ButtonCancelWidth);
        ConversionHelper.SetIntValue(w => lblDateTime.Width = w, UISizes.LabelDateTimeWidth);
        ConversionHelper.SetIntValue(w => lblDuration.Width = w, UISizes.LabelDurationWidth);
        ConversionHelper.SetIntValue(w => lblMinutes.Width = w, UISizes.LabelMinutesWidth);
        ConversionHelper.SetIntValue(w => lblNotes.Width = w, UISizes.LabelNotesWidth);
        ConversionHelper.SetIntValue(l => dtpWalkDateTime.Left = l, UISizes.DateTimeInputLeft);
        ConversionHelper.SetIntValue(l => numDuration.Left = l, UISizes.DurationInputLeft);
        ConversionHelper.SetIntValue(w => this.Width = w, UISizes.FormWalkEditorWidth);
    }
    
    /// <summary>
    /// Initializes data binding for the form controls.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="dogId">The ID of the dog.</param>
    /// <param name="dogName">The name of the dog.</param>
    /// <param name="existingWalk">The existing walk to edit, or null for a new walk.</param>
    private void InitializeDataBinding(int clientId, int dogId, string dogName, Walk? existingWalk)
    {
        _viewModel = new WalkEditorViewModel();
        
        if (_isNewWalk)
        {
            _viewModel.ClientId = clientId;
            _viewModel.DogId = dogId;
            _viewModel.DogName = dogName;
            _viewModel.WalkDateTime = DateTime.Now;
            _viewModel.DurationMinutes = 30;
            _viewModel.Notes = string.Empty;
        }
        else if (existingWalk != null)
        {
            _viewModel.WalkId = existingWalk.Id;
            _viewModel.ClientId = existingWalk.ClientId;
            _viewModel.DogId = existingWalk.DogId;
            _viewModel.DogName = dogName;
            _viewModel.WalkDateTime = existingWalk.WalkDateTime;
            _viewModel.DurationMinutes = existingWalk.DurationMinutes;
            _viewModel.Notes = existingWalk.Notes ?? string.Empty;
        }
        
        // Setup data binding
        lblDogInfo.Text = $"Walk for: {_viewModel.DogName}";
        dtpWalkDateTime.DataBindings.Add("Value", _viewModel, "WalkDateTime", false, DataSourceUpdateMode.OnPropertyChanged);
        numDuration.DataBindings.Add("Value", _viewModel, "DurationMinutes", false, DataSourceUpdateMode.OnPropertyChanged);
        txtNotes.DataBindings.Add("Text", _viewModel, "Notes", false, DataSourceUpdateMode.OnPropertyChanged);
    }
    
    /// <summary>
    /// Configures the form's title, button text, and event handlers.
    /// </summary>
    private void ConfigureForm()
    {
        Text = _isNewWalk ? FormTitles.NewWalk : FormTitles.EditWalk;
        btnSave.Text = _isNewWalk ? ButtonText.AddWalk : ButtonText.Save;
        
        // Wire up events
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += BtnCancel_Click;
        
        // Set focus
        dtpWalkDateTime.Focus();
    }
    
    /// <summary>
    /// Handles the save button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        
        // Create or update the walk entity
        if (_isNewWalk)
        {
            EditedWalk = new Walk
            {
                ClientId = _viewModel.ClientId,
                DogId = _viewModel.DogId,
                WalkDateTime = _viewModel.WalkDateTime,
                DurationMinutes = _viewModel.DurationMinutes,
                Notes = string.IsNullOrWhiteSpace(_viewModel.Notes) ? null : _viewModel.Notes
            };
        }
        else
        {
            EditedWalk = new Walk
            {
                Id = _viewModel.WalkId,
                ClientId = _viewModel.ClientId,
                DogId = _viewModel.DogId,
                WalkDateTime = _viewModel.WalkDateTime,
                DurationMinutes = _viewModel.DurationMinutes,
                Notes = string.IsNullOrWhiteSpace(_viewModel.Notes) ? null : _viewModel.Notes
            };
        }
        
        DialogResult = DialogResult.OK;
        Close();
    }
    
    /// <summary>
    /// Handles the cancel button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
    
    /// <summary>
    /// Validates the user input.
    /// </summary>
    /// <returns>True if the input is valid; otherwise, false.</returns>
    private bool ValidateInput()
    {
        // Create walk entity for validation
        var walkToValidate = new Walk
        {
            Id = _viewModel.WalkId,
            ClientId = _viewModel.ClientId,
            DogId = _viewModel.DogId,
            WalkDateTime = _viewModel.WalkDateTime,
            DurationMinutes = _viewModel.DurationMinutes,
            Notes = string.IsNullOrWhiteSpace(_viewModel.Notes) ? null : _viewModel.Notes
        };

        return ValidateInput(walkToValidate, dtpWalkDateTime);
    }
    protected override Label GetErrorLabelControl() => lblValidationErrors;
}

/// <summary>
/// ViewModel for walk editor form with INotifyPropertyChanged.
/// </summary>
public class WalkEditorViewModel : INotifyPropertyChanged
{
    private int _walkId;
    private int _clientId;
    private int _dogId;
    private string _dogName = string.Empty;
    private DateTime _walkDateTime = DateTime.Now;
    private int _durationMinutes = 30;
    private string _notes = string.Empty;
    
    /// <summary>
    /// Gets or sets the walk ID.
    /// </summary>
    public int WalkId
    {
        get => _walkId;
        set => SetField(ref _walkId, value);
    }
    
    /// <summary>
    /// Gets or sets the client ID.
    /// </summary>
    public int ClientId
    {
        get => _clientId;
        set => SetField(ref _clientId, value);
    }
    
    /// <summary>
    /// Gets or sets the dog ID.
    /// </summary>
    public int DogId
    {
        get => _dogId;
        set => SetField(ref _dogId, value);
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
    /// Gets or sets the walk date and time.
    /// </summary>
    public DateTime WalkDateTime
    {
        get => _walkDateTime;
        set => SetField(ref _walkDateTime, value);
    }
    
    /// <summary>
    /// Gets or sets the walk duration in minutes.
    /// </summary>
    public int DurationMinutes
    {
        get => _durationMinutes;
        set => SetField(ref _durationMinutes, value);
    }
    
    /// <summary>
    /// Gets or sets the notes for the walk.
    /// </summary>
    public string Notes
    {
        get => _notes;
        set => SetField(ref _notes, value);
    }
    
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    /// <summary>
    /// Sets the field value and raises the <see cref="PropertyChanged"/> event if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <param name="field">The field reference.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>True if the value was changed; otherwise, false.</returns>
    protected bool SetField<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}