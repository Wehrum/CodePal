using System;
using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Folding;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;
using CodePal.CodeEditor;

namespace CodePal;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeEditor();
    }
    private void InitializeEditor()
    {
        //First of all you need to have a reference for your TextEditor for it to be used inside AvaloniaEdit.TextMate project.
        var _textEditor = this.FindControl<TextEditor>("Editor");

        //Here we initialize RegistryOptions with the theme we want to use.
        var  _registryOptions = new RegistryOptions(ThemeName.DarkPlus);

        //Initial setup of TextMate.
        var _textMateInstallation = _textEditor.InstallTextMate(_registryOptions);

        //Here we are getting the language by the extension and right after that we are initializing grammar with this language.
        //And that's all 😀, you are ready to use AvaloniaEdit with syntax highlighting!
        _textMateInstallation.SetGrammar(_registryOptions.GetScopeByLanguageId(_registryOptions.GetLanguageByExtension(".cs").Id));
        
        // Code Folding
        var foldingManager = FoldingManager.Install(_textEditor.TextArea);
        var foldingStrategy = new CodeFoldingStrategy();
        _textEditor.TextChanged += (s, e) => foldingStrategy.UpdateFoldings(foldingManager, ((TextEditor)s).Document);
        
        // Handle TextEntered event for brace matching
        _textEditor.TextArea.TextEntered += (sender, e) =>
        {
            BraceMatchingStrategy.HandleBraceMatching(_textEditor, e);
        };
    }
    
}

