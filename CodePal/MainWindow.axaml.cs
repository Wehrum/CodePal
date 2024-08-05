using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Folding;
using AvaloniaEdit.Indentation.CSharp;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;
using CodePal.CodeEditor;

namespace CodePal;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    // Declarations to be used throughout the MainWindow class.
    private TextEditor _textEditor;
    private RegistryOptions _registryOptions;
    private TextMate.Installation _textMateInstallation;
    private FoldingManager _foldingManager;
    private CodeFoldingStrategy _foldingStrategy;
    private bool IsHighlightingEnabled = true;
    private bool IsCodeFoldingEnabled = true;
    private bool IsBraceMatchingEnabled = true;
    private ComboBox opacityComboBox;
    private double windowOpacity = 0.8f;
    public MainWindow()
    {
        // Initializaition of components, editor, and main Event Handler for Syntax Highlighting toggle.
        DataContext = this;
        InitializeComponent();
        InitializeEditor();
        ToggleSyntaxHighlighting.IsChecked = true; // Enable by default
        ToggleSyntaxHighlighting.IsCheckedChanged += ToggleSyntaxHighlighting_IsCheckedChanged;
        opacityComboBox.ItemsSource = new string[] { "50%", "55%", "60%", "65%", "70%", "75%", "80%", "85%", "90%", "95%", "100%" };
        opacityComboBox.SelectedIndex = 6;
    }
    // PropertyChanged event for INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    // Method to raise PropertyChanged event
    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    public double WindowOpacity
    {
        get => windowOpacity;
        set
        {
            if (windowOpacity != value)
            {
                windowOpacity = value;
                OnPropertyChanged(nameof(WindowOpacity));
            }
        }
    }
    private void InitializeEditor()
    {
        // Find the editor created in the window xaml.
        _textEditor = this.FindControl<TextEditor>("Editor");
        // Set the theme to be used.
        _registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        // Install TextMate
        _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);
        // Set the language for syntax highlighting.
        _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".cs").Id));
        // Set the Indentation Strategy to be used. For now we are using the included CSharp Indentation Strategy.
        _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(_textEditor.Options);
        // Install the Code Folding Manager
        _foldingManager = FoldingManager.Install(_textEditor.TextArea);
        // Set the Folding Strategy to our CodeFoldingStrategy in /CodeEditor
        _foldingStrategy = new CodeFoldingStrategy();
        // Handle the TextChanged event for enabling/disabling code folding.
        _textEditor.TextChanged += OnTextChanged;
        // Handle the TextEntered even for enabling/disabling brace matching.
        _textEditor.TextArea.TextEntered += OnTextEntered;
        
        //
        opacityComboBox = this.FindControl<ComboBox>("OpacityComboBox");
        opacityComboBox.SelectionChanged += OpacitySelectionChanged;
    }

    // Event Handler for OnTextChanged
    private void OnTextChanged(object sender, EventArgs e)
    {
        // Check that IsCodeFolding is equal to true. If it is, update the locations where code can be folded and display it.
        if (IsCodeFoldingEnabled)
        {
            _foldingStrategy.UpdateFoldings(_foldingManager, _textEditor.Document);
        }
    }

    // Event handler for OnTextEntered
    private void OnTextEntered(object sender, TextInputEventArgs e)
    {
        // Check that IsBraceMatching is equal to true. If it is, begin to match braces as typed by the user.
        if (IsBraceMatchingEnabled)
        {
            BraceMatchingStrategy.HandleBraceMatching(_textEditor, e);
        }
    }

    // Event Handler for IsCheckChanged on the ToggleSyntaxHighlighting button.
    private void ToggleSyntaxHighlighting_IsCheckedChanged(object? sender, EventArgs e)
    {
        if (ToggleSyntaxHighlighting.IsChecked == true)
        {
            // Set the syntax language.
            _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".cs").Id));
            // Install the Folding Manager
            _foldingManager = FoldingManager.Install(_textEditor.TextArea);
            // Assign the TextChanged event.
            _textEditor.TextChanged += OnTextChanged;
            // Assign the TextEntered event.
            _textEditor.TextArea.TextEntered += OnTextEntered;
            // Set the Indentation Strategy,
            _textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(_textEditor.Options);
            // Set our boolean checks as needed.
            IsHighlightingEnabled = true;
            IsCodeFoldingEnabled = true;
            IsBraceMatchingEnabled = true;
        }
        else
        {
            // Set the syntax language to null.
            _textMateInstallation.SetGrammar(null);
            // Uninstall the Folding Manager.
            FoldingManager.Uninstall(_foldingManager);
            // Unassign the TextChanged event.
            _textEditor.TextChanged -= OnTextChanged;
            // Unassign the TextEntered event.
            _textEditor.TextArea.TextEntered -= OnTextEntered;
            // Set the Indentation Strategy to null.
            _textEditor.TextArea.IndentationStrategy = null;
            // Set our boolean checks as needed.
            IsHighlightingEnabled = false;
            IsCodeFoldingEnabled = false;
            IsBraceMatchingEnabled = false;
        }
    }

    private void OpacitySelectionChanged(object? sender, EventArgs e)
    {
        switch (opacityComboBox.SelectedIndex)
        {
            case 0:
                WindowOpacity = 0.5f;
                break;
            case 1:
                WindowOpacity = 0.55f;
                break;
            case 2:
                WindowOpacity = 0.6f;
                break;
            case 3:
                WindowOpacity = 0.65f;
                break;
            case 4:
                WindowOpacity = 0.70f;
                break;
            case 5:
                WindowOpacity = 0.75f;
                break;
            case 6:
                WindowOpacity = 0.8f;
                break;
            case 7:
                WindowOpacity = 0.85f;
                break;
            case 8:
                WindowOpacity = 0.9f;
                break;
            case 9:
                WindowOpacity = 0.95f;
                break;
            case 10:
                WindowOpacity = 1.0f;
                break;
        }
    }
}