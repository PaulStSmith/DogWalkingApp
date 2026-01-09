using System.ComponentModel;
using DogWalkingApp.Domain.Entities;
using DogWalkingApp.UI.ViewModels;
using DogWalkingApp.Resources;

namespace DogWalkingApp.UI.Forms;

/// <summary>
/// Form for editing dog details with data binding.
/// </summary>
public partial class DogEditorForm : EditorForm
{
    private DogEditorViewModel _viewModel = null!;
    private readonly bool _isNewDog;
    
    /// <summary>
    /// Gets the edited dog entity after saving.
    /// </summary>
    public Dog? EditedDog { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DogEditorForm"/> class.
    /// </summary>
    /// <param name="clientId">The ID of the client associated with the dog.</param>
    /// <param name="existingDog">The existing dog to edit, or null for a new dog.</param>
    public DogEditorForm(int clientId, Dog? existingDog = null)
    {
        InitializeComponent();
        LoadLocalizedText();
        _isNewDog = existingDog == null;
        
        InitializeDataBinding(clientId, existingDog);
        ConfigureForm();
    }

    /// <summary>
    /// Loads localized text from resources for all UI controls.
    /// </summary>
    private void LoadLocalizedText()
    {
        // Form title
        Text = FormTitles.DogEditor;
        lblTitle.Text = FormTitles.DogEditorWithIcon;
        
        // Field labels
        lblName.Text = Labels.Name;
        lblBreed.Text = Labels.Breed;
        lblAge.Text = Labels.Age;
        
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
        ConversionHelper.SetIntValue(value => btnSave.Width = value, UISizes.ButtonSaveWidth);
        ConversionHelper.SetIntValue(value => btnCancel.Width = value, UISizes.ButtonCancelWidth            );
        ConversionHelper.SetIntValue(value => lblName.Width = value, UISizes.LabelNameWidth);
        ConversionHelper.SetIntValue(value => lblBreed.Width = value, UISizes.LabelBreedWidth);
        ConversionHelper.SetIntValue(value => lblAge.Width = value, UISizes.LabelAgeWidth);
        ConversionHelper.SetIntValue(value => txtName.Left = value, UISizes.NameInputLeft);
        ConversionHelper.SetIntValue(value => txtBreed.Left = value, UISizes.BreedInputLeft);
        ConversionHelper.SetIntValue(value => numAge.Left = value, UISizes.AgeInputLeft);
        ConversionHelper.SetIntValue(value => this.Width = value, UISizes.FormDogEditorWidth);
    }
    
    /// <summary>
    /// Initializes data binding for the form controls.
    /// </summary>
    /// <param name="clientId">The ID of the client associated with the dog.</param>
    /// <param name="existingDog">The existing dog to edit, or null for a new dog.</param>
    private void InitializeDataBinding(int clientId, Dog? existingDog)
    {
        _viewModel = new DogEditorViewModel();
        
        if (_isNewDog)
        {
            _viewModel.ClientId = clientId;
            _viewModel.Name = string.Empty;
            _viewModel.Breed = string.Empty;
            _viewModel.Age = 1;
        }
        else if (existingDog != null)
        {
            _viewModel.DogId = existingDog.Id;
            _viewModel.ClientId = existingDog.ClientId;
            _viewModel.Name = existingDog.Name;
            _viewModel.Breed = existingDog.Breed;
            _viewModel.Age = existingDog.Age;
        }
        
        // Setup data binding
        txtName.DataBindings.Add("Text", _viewModel, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
        txtBreed.DataBindings.Add("Text", _viewModel, "Breed", false, DataSourceUpdateMode.OnPropertyChanged);
        numAge.DataBindings.Add("Value", _viewModel, "Age", false, DataSourceUpdateMode.OnPropertyChanged);
    }
    
    /// <summary>
    /// Configures the form's appearance and event handlers.
    /// </summary>
    private void ConfigureForm()
    {
        Text = _isNewDog ? FormTitles.NewDog : FormTitles.EditDog;
        btnSave.Text = _isNewDog ? ButtonText.AddDog : ButtonText.Save;
        
        // Wire up events
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += BtnCancel_Click;
        
        // Set focus
        txtName.Focus();
    }
    
    /// <summary>
    /// Handles the save button click event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (!ValidateInput()) return;
        
        // Create or update the dog entity
        if (_isNewDog)
        {
            EditedDog = new Dog
            {
                ClientId = _viewModel.ClientId,
                Name = _viewModel.Name,
                Breed = _viewModel.Breed,
                Age = _viewModel.Age
            };
        }
        else
        {
            EditedDog = new Dog
            {
                Id = _viewModel.DogId,
                ClientId = _viewModel.ClientId,
                Name = _viewModel.Name,
                Breed = _viewModel.Breed,
                Age = _viewModel.Age
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
        // Create dog entity for validation
        var dogToValidate = new Dog
        {
            Id = _viewModel.DogId,
            ClientId = _viewModel.ClientId,
            Name = _viewModel.Name,
            Breed = _viewModel.Breed,
            Age = _viewModel.Age
        };

        return ValidateInput(dogToValidate, txtName);
    }

    protected override Label GetErrorLabelControl() => lblValidationErrors;
}

/// <summary>
/// ViewModel for dog editor form with INotifyPropertyChanged.
/// </summary>
public class DogEditorViewModel : INotifyPropertyChanged
{
    private int _dogId;
    private int _clientId;
    private string _name = string.Empty;
    private string _breed = string.Empty;
    private int _age = 1;
    
    /// <summary>
    /// Gets or sets the dog ID.
    /// </summary>
    public int DogId
    {
        get => _dogId;
        set => SetField(ref _dogId, value);
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
    /// Gets or sets the dog's name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }
    
    /// <summary>
    /// Gets or sets the dog's breed.
    /// </summary>
    public string Breed
    {
        get => _breed;
        set => SetField(ref _breed, value);
    }
    
    /// <summary>
    /// Gets or sets the dog's age.
    /// </summary>
    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
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