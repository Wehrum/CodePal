using System;
using Avalonia.Input;
using AvaloniaEdit;
using AvaloniaEdit.Document;


namespace CodePal.CodeEditor;

public class BraceMatchingStrategy
{
    public static void HandleBraceMatching(TextEditor textEditor, TextInputEventArgs e)
    {
        var document = textEditor.Document;
        var inputText = e.Text;

        switch (inputText)
        {
            case "{":
                InsertMatchingBrace(textEditor, "}");
                break;
            case "[":
                InsertMatchingBrace(textEditor, "]");
                break;
            case "(":
                InsertMatchingBrace(textEditor, ")");
                break;
            case "<":
                InsertMatchingBrace(textEditor, ">");
                break;
        }
    }

    private static void InsertMatchingBrace(TextEditor textEditor, string closingBrace)
    {
        var document = textEditor.Document;
        var caretOffset = textEditor.CaretOffset;

        // Insert the closing brace
        document.Insert(caretOffset, closingBrace);

        // Move caret inside the braces
        textEditor.CaretOffset = caretOffset;

        if (closingBrace == "}")
        {
            // Insert new lines
            document.Insert(caretOffset, "\n");
            document.Insert(caretOffset + 1, "\n");

            // Get the current line and the new lines
            var currentLine = document.GetLineByOffset(caretOffset - 1);
            var nextLine = document.GetLineByOffset(caretOffset + 1);

            // Calculate the indentation based on the current line
            var indentation = GetIndentationForLine(document, currentLine);

            // Move caret to the first new line and apply the calculated indentation
            textEditor.CaretOffset = nextLine.Offset;
            document.Insert(nextLine.Offset, indentation);
            textEditor.CaretOffset = nextLine.Offset + indentation.Length;
        }
    }

    private static string GetIndentationForLine(TextDocument document, DocumentLine line)
    {
        var lineText = document.GetText(line.Offset, line.Length);
        var indentLevel = 0;

        // Count the number of opening and closing braces to determine the indentation level
        foreach (char c in lineText)
        {
            if (c == '{') indentLevel++;
            if (c == '}') indentLevel--;
        }

        // Return a string with the appropriate number of spaces for indentation
        return new string(' ', Math.Max(indentLevel, 0) * 4); // Adjust 4 to your preferred indentation size
    } 
}