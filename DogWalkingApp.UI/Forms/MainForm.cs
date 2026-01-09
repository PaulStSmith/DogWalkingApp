using System.ComponentModel;
using DogWalkingApp.Data.Repositories;
using DogWalkingApp.Services;
using DogWalkingApp.UI.ViewModels;
using DogWalkingApp.UI.Forms;
using DogWalkingApp.Resources;

// Disable marking members as static warnings for UI code
#pragma warning disable CA1822

namespace DogWalkingApp.UI;

/// <summary>
/// The main form of the DogWalkingApp UI - new hierarchical design.
/// </summary>
public partial class MainForm : Form
{
    private HierarchicalTreeViewModel _treeViewModel = null!;
    private WalkService? _walkService = null!;

    // Data binding properties
    private ClientDetailViewModel _clientDetailViewModel = null!;
    private BindingSource _dogsBindingSource = null!;
    private BindingSource _walksBindingSource = null!;
    private BindingList<DogGridRowViewModel> _dogsList = null!;
    private BindingList<WalkGridRowViewModel> _walksList = null!;

    // Store original values for cancel functionality
    private string _originalClientName = string.Empty;
    private string _originalClientPhone = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();
        LoadLocalizedText();
        InitializeDataBinding();
        _ = LoadHierarchicalDataAsync(); // Fire and forget for UI initialization
    }

    /// <summary>
    /// Loads localized text from resources for all UI controls.
    /// </summary>
    private void LoadLocalizedText()
    {
        // Form title
        Text = FormTitles.MainFormHierarchical;

        // Section headers
        lblClientTitle.Text = Labels.ClientDetailsWithIcon;
        lblDogsTitle.Text = Labels.DogsForSelectedClient;
        lblWalksTitle.Text = Labels.WalksForSelectedDog;
        label1.Text = Labels.SearchWithIcon;

        // Field labels
        lblClientName.Text = Labels.Name;
        lblClientPhone.Text = Labels.Phone;

        // Client buttons
        btnClientNew.Text = ButtonText.New;
        btnClientEdit.Text = ButtonText.Edit;
        btnClientSave.Text = ButtonText.Save;
        btnClientDelete.Text = ButtonText.Delete;

        // Dog buttons
        btnDogNew.Text = ButtonText.New;
        btnDogEdit.Text = ButtonText.Edit;
        btnDogDelete.Text = ButtonText.Delete;

        // Walk buttons
        btnWalkNew.Text = ButtonText.New;
        btnWalkEdit.Text = ButtonText.Edit;
        btnWalkDelete.Text = ButtonText.Delete;

        // Apply localized sizing
        ApplyLocalizedSizing();
    }

    /// <summary>
    /// Applies culture-specific sizing to UI elements.
    /// </summary>
    private void ApplyLocalizedSizing()
    {
        ConversionHelper.SetIntValue(width =>
        {
            btnClientNew.Width = width;
            btnDogNew.Width = width;
            btnWalkNew.Width = width;
        }, UISizes.ButtonNewWidth);

        ConversionHelper.SetIntValue(width =>
        {
            btnClientEdit.Width = width;
            btnDogEdit.Width = width;
            btnWalkEdit.Width = width;
        }, UISizes.ButtonEditWidth);

        ConversionHelper.SetIntValue(width =>
        {
            btnClientDelete.Width = width;
            btnDogDelete.Width = width;
            btnWalkDelete.Width = width;
        }, UISizes.ButtonDeleteWidth);

        ConversionHelper.SetIntValue(width => btnClientSave.Width = width, UISizes.ButtonSaveWidth);
        ConversionHelper.SetIntValue(width => lblClientName.Width = width, UISizes.LabelNameWidth);
        ConversionHelper.SetIntValue(width => lblClientPhone.Width = width, UISizes.LabelPhoneWidth);
        ConversionHelper.SetIntValue(left => txtClientName.Left = left, UISizes.NameInputLeft);
        ConversionHelper.SetIntValue(left => txtClientPhone.Left = left, UISizes.PhoneInputLeft);
    }

    /// <summary>
    /// Initializes data binding components and sets up the UI bindings.
    /// </summary>
    private void InitializeDataBinding()
    {
        // Initialize database context and services
        var databaseService = new DatabaseService();
        var context = databaseService.CreateContext();

        var walkRepo = new WalkRepository(context);
        var clientRepo = new ClientRepository(context);
        _walkService = new WalkService(walkRepo, clientRepo);

        // Initialize tree view model
        _treeViewModel = new HierarchicalTreeViewModel();

        // Initialize data binding objects
        _clientDetailViewModel = new ClientDetailViewModel();
        _dogsList = [];
        _walksList = [];
        _dogsBindingSource = new BindingSource(_dogsList, null);
        _walksBindingSource = new BindingSource(_walksList, null);

        // Setup SIMPLE binding for client details
        txtClientName.DataBindings.Add("Text", _clientDetailViewModel, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
        txtClientPhone.DataBindings.Add("Text", _clientDetailViewModel, "Phone", false, DataSourceUpdateMode.OnPropertyChanged);

        // Make client textboxes readonly initially
        txtClientName.ReadOnly = true;
        txtClientPhone.ReadOnly = true;

        // Hide save button initially
        btnClientSave.Visible = false;

        // Setup COMPLEX binding for DataGridViews
        dgvDogs.DataSource = _dogsBindingSource;
        dgvWalks.DataSource = _walksBindingSource;

        // Configure DataGridView columns
        ConfigureDataGridViewColumns();

        // Wire up event handlers
        treeViewHierarchy.AfterSelect += TreeViewHierarchy_AfterSelect;
        dgvDogs.SelectionChanged += DgvDogs_SelectionChanged;

        // Wire up client button events
        btnClientNew.Click += BtnClientNew_Click;
        btnClientEdit.Click += BtnClientEdit_Click;
        btnClientSave.Click += BtnClientSave_Click;
        btnClientDelete.Click += BtnClientDelete_Click;

        // Wire up dog button events
        btnDogNew.Click += BtnDogNew_Click;
        btnDogEdit.Click += BtnDogEdit_Click;
        btnDogDelete.Click += BtnDogDelete_Click;

        // Wire up walk button events
        btnWalkNew.Click += BtnWalkNew_Click;
        btnWalkEdit.Click += BtnWalkEdit_Click;
        btnWalkDelete.Click += BtnWalkDelete_Click;

        // Wire up search functionality
        txtSearchTerm.TextChanged += TxtSearchTerm_TextChanged;
    }

    /// <summary>
    /// Loads hierarchical data asynchronously and populates the tree view.
    /// </summary>
    /// <param name="preserveSelection">If true, attempts to preserve the current selection after reloading.</param>
    /// <param name="selectClientId">The client ID to select after loading, if specified.</param>
    /// <param name="selectDogId">The dog ID to select after loading, if specified.</param>
    /// <param name="selectWalkId">The walk ID to select after loading, if specified.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task LoadHierarchicalDataAsync(bool preserveSelection = false, int? selectClientId = null, int? selectDogId = null, int? selectWalkId = null)
    {
        try
        {
            // Remember current selection if requested
            TreeNodeViewModel? previousSelection = null;
            if (preserveSelection && _treeViewModel.SelectedNode != null)
            {
                previousSelection = _treeViewModel.SelectedNode;
            }

            // Load all clients with their dogs and walks
            var clients = await _walkService!.GetAllClientsWithDetailsAsync();

            // Convert to view models
            var clientGroups = clients.Select(client => new ClientTreeNodeViewModel
            {
                ClientId = client.Id,
                ClientName = client.Name,
                Phone = client.Phone,
                Dogs = [.. client.Dogs.Select(dog => new DogTreeNodeViewModel
                {
                    DogId = dog.Id,
                    ClientId = dog.ClientId,
                    DogName = dog.Name,
                    Breed = dog.Breed,
                    Age = dog.Age,
                    Walks = [.. dog.Walks
                        .Select(walk => new WalkTreeNodeViewModel
                        {
                            WalkId = walk.Id,
                            ClientId = walk.ClientId,
                            DogId = walk.DogId,
                            WalkDateTime = walk.WalkDateTime,
                            DurationMinutes = walk.DurationMinutes,
                            Notes = walk.Notes
                        })
                        .OrderByDescending(w => w.WalkDateTime)]
                })
                .OrderBy(d => d.DogName)]
            })
            .OrderBy(c => c.ClientName)
            .ToList();

            _treeViewModel.Clients = clientGroups;
            PopulateTreeView();

            // Select specific items if requested, otherwise restore previous selection
            if (selectClientId.HasValue || selectDogId.HasValue || selectWalkId.HasValue)
            {
                SelectSpecificItem(selectClientId, selectDogId, selectWalkId);
            }
            else if (preserveSelection && previousSelection != null)
            {
                RestoreSelection(previousSelection);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading hierarchical data: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Restores the previous selection in the tree view.
    /// </summary>
    /// <param name="previousSelection">The previously selected tree node view model.</param>
    private void RestoreSelection(TreeNodeViewModel previousSelection)
    {
        TreeNode? nodeToSelect = null;

        switch (previousSelection.NodeType)
        {
            case TreeNodeType.Client:
                var clientToFind = (ClientTreeNodeViewModel)previousSelection;
                nodeToSelect = FindTreeNode(n => n.Tag is ClientTreeNodeViewModel client && client.ClientId == clientToFind.ClientId);
                break;
            case TreeNodeType.Dog:
                var dogToFind = (DogTreeNodeViewModel)previousSelection;
                nodeToSelect = FindTreeNode(n => n.Tag is DogTreeNodeViewModel dog && dog.DogId == dogToFind.DogId);
                break;
            case TreeNodeType.Walk:
                var walkToFind = (WalkTreeNodeViewModel)previousSelection;
                nodeToSelect = FindTreeNode(n => n.Tag is WalkTreeNodeViewModel walk && walk.WalkId == walkToFind.WalkId);
                break;
        }

        if (nodeToSelect != null)
        {
            treeViewHierarchy.SelectedNode = nodeToSelect;
        }
    }

    /// <summary>
    /// Finds a tree node that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match the tree node.</param>
    /// <returns>The matching tree node, or null if not found.</returns>
    private TreeNode? FindTreeNode(Func<TreeNode, bool> predicate)
    {
        return FindTreeNodeRecursive(treeViewHierarchy.Nodes.Cast<TreeNode>(), predicate);
    }

    /// <summary>
    /// Recursively finds a tree node that matches the specified predicate.
    /// </summary>
    /// <param name="nodes">The collection of tree nodes to search.</param>
    /// <param name="predicate">The predicate to match the tree node.</param>
    /// <returns>The matching tree node, or null if not found.</returns>
    private TreeNode? FindTreeNodeRecursive(IEnumerable<TreeNode> nodes, Func<TreeNode, bool> predicate)
    {
        foreach (var node in nodes)
        {
            if (predicate(node))
                return node;

            var found = FindTreeNodeRecursive(node.Nodes.Cast<TreeNode>(), predicate);
            if (found != null)
                return found;
        }
        return null;
    }

    /// <summary>
    /// Selects a specific item in the tree view based on the provided IDs.
    /// </summary>
    /// <param name="clientId">The client ID to select.</param>
    /// <param name="dogId">The dog ID to select.</param>
    /// <param name="walkId">The walk ID to select.</param>
    private void SelectSpecificItem(int? clientId, int? dogId, int? walkId)
    {
        TreeNode? nodeToSelect = null;

        if (walkId.HasValue)
        {
            // Select specific walk
            nodeToSelect = FindTreeNode(n => n.Tag is WalkTreeNodeViewModel walk && walk.WalkId == walkId.Value);
        }
        else if (dogId.HasValue)
        {
            // Select specific dog
            nodeToSelect = FindTreeNode(n => n.Tag is DogTreeNodeViewModel dog && dog.DogId == dogId.Value);
        }
        else if (clientId.HasValue)
        {
            // Select specific client
            nodeToSelect = FindTreeNode(n => n.Tag is ClientTreeNodeViewModel client && client.ClientId == clientId.Value);
        }

        if (nodeToSelect != null)
        {
            treeViewHierarchy.SelectedNode = nodeToSelect;
            nodeToSelect.EnsureVisible();
        }
    }

    /// <summary>
    /// Refreshes the dogs list for the specified client.
    /// </summary>
    /// <param name="clientId">The client ID to refresh dogs for.</param>
    private void RefreshDogsListForClient(int clientId)
    {
        _dogsList.Clear();

        // Find the client in the fresh tree view model data
        var client = _treeViewModel.Clients.FirstOrDefault(c => c.ClientId == clientId);
        if (client != null)
        {
            foreach (var dog in client.Dogs)
            {
                _dogsList.Add(new DogGridRowViewModel
                {
                    DogId = dog.DogId,
                    ClientId = dog.ClientId,
                    Name = dog.DogName,
                    Breed = dog.Breed,
                    Age = dog.Age,
                    WalkCount = dog.Walks.Count
                });
            }
        }
    }

    /// <summary>
    /// Refreshes the walks list for the specified dog.
    /// </summary>
    /// <param name="dogId">The dog ID to refresh walks for.</param>
    private void RefreshWalksListForDog(int dogId)
    {
        _walksList.Clear();

        // Find the dog in the fresh tree view model data
        var dog = _treeViewModel.Clients
            .SelectMany(c => c.Dogs)
            .FirstOrDefault(d => d.DogId == dogId);

        if (dog != null)
        {
            foreach (var walk in dog.Walks)
            {
                _walksList.Add(new WalkGridRowViewModel
                {
                    WalkId = walk.WalkId,
                    ClientId = walk.ClientId,
                    DogId = walk.DogId,
                    WalkDateTime = walk.WalkDateTime,
                    DurationMinutes = walk.DurationMinutes,
                    Notes = walk.Notes
                });
            }
        }
    }

    /// <summary>
    /// Populates the tree view with the hierarchical data.
    /// </summary>
    private void PopulateTreeView()
    {
        treeViewHierarchy.BeginUpdate();
        treeViewHierarchy.Nodes.Clear();

        foreach (var client in _treeViewModel.Clients)
        {
            var clientNode = new TreeNode(client.DisplayText)
            {
                Tag = client,
                ImageIndex = 0
            };

            foreach (var dog in client.Dogs)
            {
                var dogNode = new TreeNode(dog.DisplayText)
                {
                    Tag = dog,
                    ImageIndex = 1
                };

                foreach (var walk in dog.Walks.Take(10)) // Show latest 10 walks per dog
                {
                    var walkNode = new TreeNode(walk.DisplayText)
                    {
                        Tag = walk,
                        ImageIndex = 2
                    };
                    dogNode.Nodes.Add(walkNode);
                }

                clientNode.Nodes.Add(dogNode);
            }

            treeViewHierarchy.Nodes.Add(clientNode);
        }

        // Expand first level by default
        foreach (TreeNode node in treeViewHierarchy.Nodes)
        {
            node.Expand();
        }

        treeViewHierarchy.EndUpdate();
    }

    /// <summary>
    /// Handles the TextChanged event of the search term textbox.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void TxtSearchTerm_TextChanged(object? sender, EventArgs e)
    {
        var searchTerm = txtSearchTerm.Text.Trim();
        PopulateFilteredTreeView(searchTerm);
    }

    /// <summary>
    /// Populates the tree view with filtered hierarchical data based on the search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    private void PopulateFilteredTreeView(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // No search term, show all data
            PopulateTreeView();
            return;
        }

        treeViewHierarchy.BeginUpdate();
        treeViewHierarchy.Nodes.Clear();

        searchTerm = searchTerm.ToLowerInvariant();

        foreach (var client in _treeViewModel.Clients)
        {
            var clientMatches = client.ClientName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
            var clientNode = new TreeNode(client.DisplayText)
            {
                Tag = client,
                ImageIndex = 0
            };
            var hasMatchingChildren = false;

            foreach (var dog in client.Dogs)
            {
                var dogMatches = dog.DogName.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                               dog.Breed.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);
                var dogNode = new TreeNode(dog.DisplayText)
                {
                    Tag = dog,
                    ImageIndex = 1
                };
                var hasMatchingWalks = false;

                foreach (var walk in dog.Walks.Take(10)) // Show latest 10 walks per dog
                {
                    var walkMatches = !string.IsNullOrEmpty(walk.Notes) &&
                                    walk.Notes.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (walkMatches || dogMatches || clientMatches)
                    {
                        var walkNode = new TreeNode(walk.DisplayText)
                        {
                            Tag = walk,
                            ImageIndex = 2
                        };
                        dogNode.Nodes.Add(walkNode);
                        hasMatchingWalks = true;
                    }
                }

                if (dogMatches || clientMatches || hasMatchingWalks)
                {
                    clientNode.Nodes.Add(dogNode);
                    hasMatchingChildren = true;
                }
            }

            if (clientMatches || hasMatchingChildren)
            {
                treeViewHierarchy.Nodes.Add(clientNode);
            }
        }

        // Expand all nodes to show matches
        foreach (TreeNode node in treeViewHierarchy.Nodes)
        {
            node.ExpandAll();
        }

        treeViewHierarchy.EndUpdate();
    }

    /// <summary>
    /// Configures the columns for the DataGridViews.
    /// </summary>
    private void ConfigureDataGridViewColumns()
    {
        // Configure Dogs DataGridView columns
        dgvDogs.AutoGenerateColumns = false;
        dgvDogs.Columns.Clear();

        dgvDogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Name",
            HeaderText = Labels.ColumnName,
            Width = 120
        });

        dgvDogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Breed",
            HeaderText = Labels.ColumnBreed,
            Width = 100
        });

        dgvDogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "Age",
            HeaderText = Labels.ColumnAge,
            Width = 60
        });

        dgvDogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "WalkCount",
            HeaderText = Labels.ColumnWalks,
            Width = 80
        });

        // Configure Walks DataGridView columns
        dgvWalks.AutoGenerateColumns = false;
        dgvWalks.Columns.Clear();

        dgvWalks.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DateDisplay",
            HeaderText = Labels.ColumnDate,
            Width = 100
        });

        dgvWalks.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TimeDisplay",
            HeaderText = Labels.ColumnTime,
            Width = 80
        });

        dgvWalks.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DurationDisplay",
            HeaderText = Labels.ColumnDuration,
            Width = 80
        });

        dgvWalks.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "NotesDisplay",
            HeaderText = Labels.ColumnNotes,
            Width = 200
        });
    }

    /// <summary>
    /// Handles the AfterSelect event of the tree view hierarchy.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void TreeViewHierarchy_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is TreeNodeViewModel selectedNode)
        {
            _treeViewModel.SelectedNode = selectedNode;

            // Update panels based on selection type
            switch (selectedNode.NodeType)
            {
                case TreeNodeType.Client:
                    HandleClientSelection((ClientTreeNodeViewModel)selectedNode);
                    break;
                case TreeNodeType.Dog:
                    HandleDogSelection((DogTreeNodeViewModel)selectedNode);
                    break;
                case TreeNodeType.Walk:
                    HandleWalkSelection((WalkTreeNodeViewModel)selectedNode);
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the selection of a client in the tree view.
    /// </summary>
    /// <param name="client">The selected client tree node view model.</param>
    private void HandleClientSelection(ClientTreeNodeViewModel client)
    {
        // Update client details (SIMPLE BINDING)
        _clientDetailViewModel.ClientId = client.ClientId;
        _clientDetailViewModel.Name = client.ClientName;
        _clientDetailViewModel.Phone = client.Phone;
        _clientDetailViewModel.IsEditing = false;
        txtClientName.ReadOnly = true;
        txtClientPhone.ReadOnly = true;

        // Reset button states
        btnClientEdit.Text = ButtonText.Edit;
        btnClientEdit.Enabled = true;
        btnClientDelete.Enabled = true;
        btnClientNew.Text = ButtonText.New;
        btnClientSave.Visible = false;

        // Update dogs list using fresh data from tree model (COMPLEX BINDING)
        RefreshDogsListForClient(client.ClientId);

        // Auto-select first dog if available to load its walks
        if (_dogsList.Count > 0)
        {
            dgvDogs.ClearSelection();
            dgvDogs.Rows[0].Selected = true;
            // Manually trigger the selection changed event since programmatic selection doesn't fire it
            DgvDogs_SelectionChanged(dgvDogs, EventArgs.Empty);
        }
        else
        {
            // Clear walks since no dogs are available
            _walksList.Clear();
        }
    }

    /// <summary>
    /// Handles the selection of a dog in the tree view.
    /// </summary>
    /// <param name="dog">The selected dog tree node view model.</param>
    private void HandleDogSelection(DogTreeNodeViewModel dog)
    {
        var client = _treeViewModel.Clients.First(c => c.ClientId == dog.ClientId);

        // Update client details (SIMPLE BINDING)
        _clientDetailViewModel.ClientId = client.ClientId;
        _clientDetailViewModel.Name = client.ClientName;
        _clientDetailViewModel.Phone = client.Phone;
        _clientDetailViewModel.IsEditing = false;
        txtClientName.ReadOnly = true;
        txtClientPhone.ReadOnly = true;

        // Reset button states
        btnClientEdit.Text = ButtonText.Edit;
        btnClientEdit.Enabled = true;
        btnClientDelete.Enabled = true;
        btnClientNew.Text = ButtonText.New;
        btnClientSave.Visible = false;

        // Update dogs list using fresh data (COMPLEX BINDING)
        RefreshDogsListForClient(client.ClientId);

        // Update walks for selected dog using fresh data (COMPLEX BINDING)
        RefreshWalksListForDog(dog.DogId);

        // Highlight the selected dog in the DataGridView
        for (var i = 0; i < dgvDogs.Rows.Count; i++)
        {
            if (dgvDogs.Rows[i].DataBoundItem is DogGridRowViewModel dogRow && dogRow.DogId == dog.DogId)
            {
                dgvDogs.ClearSelection();
                dgvDogs.Rows[i].Selected = true;
                break;
            }
        }
    }

    /// <summary>
    /// Handles the selection of a walk in the tree view.
    /// </summary>
    /// <param name="walk">The selected walk tree node view model.</param>
    private void HandleWalkSelection(WalkTreeNodeViewModel walk)
    {
        var client = _treeViewModel.Clients.First(c => c.ClientId == walk.ClientId);
        var dog = client.Dogs.First(d => d.DogId == walk.DogId);

        // Update client details (SIMPLE BINDING)
        _clientDetailViewModel.ClientId = client.ClientId;
        _clientDetailViewModel.Name = client.ClientName;
        _clientDetailViewModel.Phone = client.Phone;
        _clientDetailViewModel.IsEditing = false;
        txtClientName.ReadOnly = true;
        txtClientPhone.ReadOnly = true;

        // Reset button states
        btnClientEdit.Text = ButtonText.Edit;
        btnClientEdit.Enabled = true;
        btnClientDelete.Enabled = true;
        btnClientNew.Text = ButtonText.New;
        btnClientSave.Visible = false;

        // Update dogs list using fresh data (COMPLEX BINDING)
        RefreshDogsListForClient(client.ClientId);

        // Update walks for selected dog using fresh data (COMPLEX BINDING)
        RefreshWalksListForDog(walk.DogId);

        // Highlight the selected dog in the dogs DataGridView
        for (var i = 0; i < dgvDogs.Rows.Count; i++)
        {
            if (dgvDogs.Rows[i].DataBoundItem is DogGridRowViewModel dogRow && dogRow.DogId == walk.DogId)
            {
                dgvDogs.ClearSelection();
                dgvDogs.Rows[i].Selected = true;
                break;
            }
        }

        // Highlight the selected walk in the walks DataGridView
        for (var i = 0; i < dgvWalks.Rows.Count; i++)
        {
            if (dgvWalks.Rows[i].DataBoundItem is WalkGridRowViewModel walkRow && walkRow.WalkId == walk.WalkId)
            {
                dgvWalks.ClearSelection();
                dgvWalks.Rows[i].Selected = true;
                break;
            }
        }
    }

    /// <summary>
    /// Handles the SelectionChanged event of the dogs DataGridView.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void DgvDogs_SelectionChanged(object? sender, EventArgs e)
    {
        // Only handle if there's a selected row and it's not during binding updates
        if (dgvDogs.SelectedRows.Count > 0 && dgvDogs.SelectedRows[0].DataBoundItem is DogGridRowViewModel selectedDog)
        {
            // Update walks for selected dog using fresh data (COMPLEX BINDING)
            RefreshWalksListForDog(selectedDog.DogId);
        }
        else
        {
            // No dog selected, clear walks
            _walksList.Clear();
        }
    }

    #region Client Button Event Handlers

    /// <summary>
    /// Cancels the current client editing operation.
    /// </summary>
    private void CancelClientEditing()
    {
        // Restore original values
        _clientDetailViewModel.Name = _originalClientName;
        _clientDetailViewModel.Phone = _originalClientPhone;

        // Exit edit mode
        _clientDetailViewModel.IsEditing = false;
        txtClientName.ReadOnly = true;
        txtClientPhone.ReadOnly = true;

        // Reset button states
        btnClientNew.Text = ButtonText.New;
        btnClientEdit.Text = ButtonText.Edit;
        btnClientEdit.Enabled = true;
        btnClientDelete.Enabled = true;
        btnClientSave.Visible = false;

        // If we were creating a new client, restore the previous selection
        if (_clientDetailViewModel.ClientId == 0)
        {
            // Restore the previously selected client if any
            if (treeViewHierarchy.SelectedNode?.Tag is ClientTreeNodeViewModel selectedClient)
            {
                HandleClientSelection(selectedClient);
            }
            else
            {
                // Clear everything if no previous selection
                _clientDetailViewModel.ClientId = 0;
                _clientDetailViewModel.Name = string.Empty;
                _clientDetailViewModel.Phone = string.Empty;
                _dogsList.Clear();
                _walksList.Clear();
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the new client button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnClientNew_Click(object? sender, EventArgs e)
    {
        if (btnClientNew.Text.Contains(ButtonText.Cancel))
        {
            // Cancel new client creation
            CancelClientEditing();
        }
        else
        {
            // Start new client creation
            // Store original values (empty for new client)
            _originalClientName = string.Empty;
            _originalClientPhone = string.Empty;

            // Clear client details and enable editing
            _clientDetailViewModel.ClientId = 0;
            _clientDetailViewModel.Name = string.Empty;
            _clientDetailViewModel.Phone = string.Empty;
            _clientDetailViewModel.IsEditing = true;

            // Make textboxes editable
            txtClientName.ReadOnly = false;
            txtClientPhone.ReadOnly = false;
            txtClientName.Focus();

            // Update button states
            btnClientNew.Text = ButtonText.Cancel;
            btnClientEdit.Enabled = false;
            btnClientDelete.Enabled = false;
            btnClientSave.Visible = true;

            // Clear dogs and walks panels
            _dogsList.Clear();
            _walksList.Clear();

            // Clear tree selection
            treeViewHierarchy.SelectedNode = null;
        }
    }

    /// <summary>
    /// Handles the Click event of the edit client button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void BtnClientEdit_Click(object? sender, EventArgs e)
    {
        if (_clientDetailViewModel.IsEditing)
        {
            // Cancel editing
            CancelClientEditing();
        }
        else
        {
            if (_clientDetailViewModel.ClientId > 0)
            {
                // Store original values for cancel functionality
                _originalClientName = _clientDetailViewModel.Name;
                _originalClientPhone = _clientDetailViewModel.Phone;

                // Enter edit mode
                _clientDetailViewModel.IsEditing = true;
                txtClientName.ReadOnly = false;
                txtClientPhone.ReadOnly = false;
                txtClientName.Focus();

                // Update button states
                btnClientEdit.Text = ButtonText.Cancel;
                btnClientSave.Visible = true;
            }
            else
            {
                MessageBox.Show(UserMessages.PleaseSelectClientToEdit, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the save client button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnClientSave_Click(object? sender, EventArgs e)
    {
        if (!_clientDetailViewModel.IsEditing) return;

        // Create client entity for validation
        var clientToValidate = new DogWalkingApp.Domain.Entities.Client
        {
            Id = _clientDetailViewModel.ClientId,
            Name = _clientDetailViewModel.Name,
            Phone = _clientDetailViewModel.Phone
        };

        // Validate using centralized validation
        var validationResult = clientToValidate.IsValid();
        if (!validationResult.IsValid)
        {
            MessageBox.Show(validationResult.GetErrorMessage(), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtClientName.Focus();
            return;
        }

        try
        {
            // Save client through service
            if (_clientDetailViewModel.ClientId == 0)
            {
                // Create new client
                var newClient = await _walkService!.CreateClientAsync(clientToValidate);
                _clientDetailViewModel.ClientId = newClient.Id;
            }
            else
            {
                // Update existing client
                await _walkService!.UpdateClientAsync(clientToValidate);
            }

            _clientDetailViewModel.IsEditing = false;
            txtClientName.ReadOnly = true;
            txtClientPhone.ReadOnly = true;

            // Reset button states
            btnClientEdit.Text = ButtonText.Edit;
            btnClientEdit.Enabled = true;
            btnClientDelete.Enabled = true;
            btnClientNew.Text = ButtonText.New;
            btnClientSave.Visible = false;

            // Reload data and select the saved client
            await LoadHierarchicalDataAsync(false, _clientDetailViewModel.ClientId);

            MessageBox.Show(UserMessages.ClientSavedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(string.Format(UserMessages.ErrorSavingClient, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Handles the Click event of the delete client button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnClientDelete_Click(object? sender, EventArgs e)
    {
        if (_clientDetailViewModel.ClientId <= 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectClientToDelete, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(string.Format(UserMessages.ConfirmDeleteClient, _clientDetailViewModel.Name),
            FormTitles.ConfirmDelete, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                // Delete client through service
                await _walkService!.DeleteClientAsync(_clientDetailViewModel.ClientId);

                // Clear panels
                _clientDetailViewModel.ClientId = 0;
                _clientDetailViewModel.Name = string.Empty;
                _clientDetailViewModel.Phone = string.Empty;
                _clientDetailViewModel.IsEditing = false;
                txtClientName.ReadOnly = true;
                txtClientPhone.ReadOnly = true;

                // Reset button states
                btnClientEdit.Text = ButtonText.Edit;
                btnClientEdit.Enabled = true;
                btnClientDelete.Enabled = true;
                btnClientNew.Text = ButtonText.New;
                btnClientSave.Visible = false;

                _dogsList.Clear();
                _walksList.Clear();

                // Clear tree selection
                treeViewHierarchy.SelectedNode = null;

                // Reload data without any selection
                await LoadHierarchicalDataAsync();

                MessageBox.Show(UserMessages.ClientDeletedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorDeletingClient, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    #region Dog Button Event Handlers

    /// <summary>
    /// Handles the Click event of the new dog button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnDogNew_Click(object? sender, EventArgs e)
    {
        if (_clientDetailViewModel.ClientId <= 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectClientForDog, UserMessages.NoClientSelected, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dogEditor = new DogEditorForm(_clientDetailViewModel.ClientId);
        if (dogEditor.ShowDialog(this) == DialogResult.OK && dogEditor.EditedDog != null)
        {
            try
            {
                // Save new dog through service
                var savedDog = await _walkService!.CreateDogAsync(dogEditor.EditedDog);
                await LoadHierarchicalDataAsync(false, null, savedDog.Id);
                MessageBox.Show(UserMessages.DogAddedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorAddingDog, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the edit dog button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnDogEdit_Click(object? sender, EventArgs e)
    {
        if (dgvDogs.SelectedRows.Count == 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectDogToEdit, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedDog = (DogGridRowViewModel)dgvDogs.SelectedRows[0].DataBoundItem;

        // Create a Dog entity for editing
        var dogToEdit = new DogWalkingApp.Domain.Entities.Dog
        {
            Id = selectedDog.DogId,
            ClientId = selectedDog.ClientId,
            Name = selectedDog.Name,
            Breed = selectedDog.Breed,
            Age = selectedDog.Age
        };

        using var dogEditor = new DogEditorForm(selectedDog.ClientId, dogToEdit);
        if (dogEditor.ShowDialog(this) == DialogResult.OK && dogEditor.EditedDog != null)
        {
            try
            {
                // Save updated dog through service
                var updatedDog = await _walkService!.UpdateDogAsync(dogEditor.EditedDog);
                await LoadHierarchicalDataAsync(false, null, updatedDog.Id);
                MessageBox.Show(UserMessages.DogUpdatedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorUpdatingDog, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the delete dog button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnDogDelete_Click(object? sender, EventArgs e)
    {
        if (dgvDogs.SelectedRows.Count == 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectDogToDelete, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedDog = (DogGridRowViewModel)dgvDogs.SelectedRows[0].DataBoundItem;
        var result = MessageBox.Show(string.Format(UserMessages.ConfirmDeleteDog, selectedDog.Name),
            FormTitles.ConfirmDelete, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                // Delete dog through service
                await _walkService!.DeleteDogAsync(selectedDog.DogId);

                // Clear walks panel since the dog is deleted
                _walksList.Clear();

                // Reload data and select the parent client
                await LoadHierarchicalDataAsync(false, selectedDog.ClientId);
                MessageBox.Show(UserMessages.DogDeletedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorDeletingDog, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    #region Walk Button Event Handlers

    /// <summary>
    /// Handles the Click event of the new walk button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnWalkNew_Click(object? sender, EventArgs e)
    {
        if (dgvDogs.SelectedRows.Count == 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectDogForWalk, UserMessages.NoDogSelected, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedDog = (DogGridRowViewModel)dgvDogs.SelectedRows[0].DataBoundItem;

        using var walkEditor = new WalkEditorForm(_clientDetailViewModel.ClientId, selectedDog.DogId, selectedDog.Name);
        if (walkEditor.ShowDialog(this) == DialogResult.OK && walkEditor.EditedWalk != null)
        {
            try
            {
                // Save new walk through service
                var savedWalk = await _walkService!.SaveWalkAsync(walkEditor.EditedWalk);
                await LoadHierarchicalDataAsync(false, null, null, savedWalk.Id);
                MessageBox.Show(UserMessages.WalkAddedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorAddingWalk, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the edit walk button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnWalkEdit_Click(object? sender, EventArgs e)
    {
        if (dgvWalks.SelectedRows.Count == 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectWalkToEdit, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedWalk = (WalkGridRowViewModel)dgvWalks.SelectedRows[0].DataBoundItem;
        var selectedDog = (DogGridRowViewModel)dgvDogs.SelectedRows[0].DataBoundItem;

        // Create a Walk entity for editing
        var walkToEdit = new DogWalkingApp.Domain.Entities.Walk
        {
            Id = selectedWalk.WalkId,
            ClientId = selectedWalk.ClientId,
            DogId = selectedWalk.DogId,
            WalkDateTime = selectedWalk.WalkDateTime,
            DurationMinutes = selectedWalk.DurationMinutes,
            Notes = selectedWalk.Notes
        };

        using var walkEditor = new WalkEditorForm(selectedWalk.ClientId, selectedWalk.DogId, selectedDog.Name, walkToEdit);
        if (walkEditor.ShowDialog(this) == DialogResult.OK && walkEditor.EditedWalk != null)
        {
            try
            {
                // Save updated walk through service
                var updatedWalk = await _walkService!.SaveWalkAsync(walkEditor.EditedWalk);
                await LoadHierarchicalDataAsync(false, null, null, updatedWalk.Id);
                MessageBox.Show(UserMessages.WalkUpdatedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorUpdatingWalk, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles the Click event of the delete walk button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private async void BtnWalkDelete_Click(object? sender, EventArgs e)
    {
        if (dgvWalks.SelectedRows.Count == 0)
        {
            MessageBox.Show(UserMessages.PleaseSelectWalkToDelete, UserMessages.NoSelection, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedWalk = (WalkGridRowViewModel)dgvWalks.SelectedRows[0].DataBoundItem;
        var result = MessageBox.Show(string.Format(UserMessages.ConfirmDeleteWalk, selectedWalk.DateDisplay),
            FormTitles.ConfirmDelete, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                // Delete walk through service
                await _walkService!.DeleteWalkAsync(selectedWalk.WalkId);

                // Reload data and select the parent dog
                await LoadHierarchicalDataAsync(false, null, selectedWalk.DogId);
                MessageBox.Show(UserMessages.WalkDeletedSuccessfully, UserMessages.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(UserMessages.ErrorDeletingWalk, ex.Message), UserMessages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    /// <summary>
    /// Handles the Click event of the exit file menu item.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void ExitFileMenuItem_Click(object sender, EventArgs e) => Close();
}