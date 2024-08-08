using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CodePal.TaskManager;

// The TaskTracker class provides the functionality for utilizing the Task class.
public partial class TaskTracker : ObservableObject
{
    public TaskTracker()
    {
        
    }

    // Create a Constructor that takes a task item as the parameter.
    public TaskTracker(Task taskItem)
    {
        IsChecked = taskItem.IsChecked;
        Content = taskItem.Content;
    }
    
    // Create the private _isChecked property.
    private bool _isChecked;

    // Create a public IsChecked property for accessing _isChecked.
    public bool IsChecked
    {
        get { return _isChecked; }
        set { SetProperty(ref _isChecked, value); }
    }
    
    // Create the private _content property.
    [ObservableProperty] 
    private string? _content;

    // Method for getting and returning tasks.
    public Task GetTasks()
    {
        return new Task()
        {
            IsChecked = this.IsChecked,
            Content = this.Content
        };
    }
}

// The TaskManager class allows for adding and deleting of tasks.
public partial class TaskManager : ObservableObject
{
    public TaskManager()
    {
        
    }

    // Create a public ObserableCollection using the TaskTracker as our type. This will group the tasks created by the user.
    public ObservableCollection<TaskTracker> TaskList { get; } = new ObservableCollection<TaskTracker>();

    // Command for the AddItem method which first calls CanAddItem to ensure the content being added is valid.
    // If the content is valid, then a new task is created.
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        TaskList.Add(new TaskTracker() { Content = NewItemContent });
        NewItemContent = null;
    }

    // The private property _mewItemContent which generates a public property.
    // This is used to hold and pass the content for the task.
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _newItemContent;

    // CanAddItem check ensures that the content to be added is not null or white space.
    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    // Command for RemoveItem.
    // When the command is executed, the task is removed from the collection.
    [RelayCommand]
    private void RemoveItem(TaskTracker item)
    {
        TaskList.Remove(item);
    }
}