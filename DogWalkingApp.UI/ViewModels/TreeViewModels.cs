using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DogWalkingApp.UI.ViewModels
{
    /// <summary>
    /// Base class for tree node view models.
    /// </summary>
    public abstract class TreeNodeViewModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        private bool _isExpanded = true;

        /// <summary>
        /// Gets the display text for the tree node.
        /// </summary>
        public abstract string DisplayText { get; }

        /// <summary>
        /// Gets the node type for identification.
        /// </summary>
        public abstract TreeNodeType NodeType { get; }

        /// <summary>
        /// Gets or sets whether this node is selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        /// <summary>
        /// Gets or sets whether this node is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value);
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
    /// Tree node type enumeration.
    /// </summary>
    public enum TreeNodeType
    {
        Client,
        Dog,
        Walk
    }

    /// <summary>
    /// ViewModel for client nodes in the tree.
    /// </summary>
    public class ClientTreeNodeViewModel : TreeNodeViewModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<DogTreeNodeViewModel> Dogs { get; set; } = new();
        public override string DisplayText => $"🙍 {ClientName}";
        public override TreeNodeType NodeType => TreeNodeType.Client;
    }

    /// <summary>
    /// ViewModel for dog nodes in the tree.
    /// </summary>
    public class DogTreeNodeViewModel : TreeNodeViewModel
    {
        public int DogId { get; set; }
        public int ClientId { get; set; }
        public string DogName { get; set; } = string.Empty;
        public string Breed { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<WalkTreeNodeViewModel> Walks { get; set; } = new();

        public override string DisplayText => $"🐕 {DogName} ({Breed})";
        public override TreeNodeType NodeType => TreeNodeType.Dog;
    }

    /// <summary>
    /// ViewModel for walk nodes in the tree.
    /// </summary>
    public class WalkTreeNodeViewModel : TreeNodeViewModel
    {
        public int WalkId { get; set; }
        public int ClientId { get; set; }
        public int DogId { get; set; }
        public DateTime WalkDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }

        public override string DisplayText => $"🚶 {WalkDateTime:MMM dd HH:mm} ({DurationMinutes}min)";
        public override TreeNodeType NodeType => TreeNodeType.Walk;
    }

    /// <summary>
    /// ViewModel for managing the hierarchical tree structure.
    /// </summary>
    public class HierarchicalTreeViewModel : INotifyPropertyChanged
    {
        private TreeNodeViewModel? _selectedNode;
        
        public List<ClientTreeNodeViewModel> Clients { get; set; } = new();

        /// <summary>
        /// Gets or sets the currently selected tree node.
        /// </summary>
        public TreeNodeViewModel? SelectedNode
        {
            get => _selectedNode;
            set => SetField(ref _selectedNode, value);
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
}