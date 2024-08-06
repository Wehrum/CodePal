using System;
using Avalonia.Input;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Indentation.CSharp;


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

                // Move caret to the first new line
                textEditor.CaretOffset = caretOffset + 1;

                // Apply the indentation strategy
                var indentationStrategy = textEditor.TextArea.IndentationStrategy as CSharpIndentationStrategy;
                if (indentationStrategy != null)
                {
                    // Indent the line where the caret is currently located
                    var currentLine = document.GetLineByOffset(textEditor.CaretOffset);
                    indentationStrategy.IndentLine(document, currentLine);

                    // Move the caret to the new indented position
                    var newCaretOffset = currentLine.Offset + GetWhitespaceAfterOffset(currentLine, document);
                    textEditor.CaretOffset = newCaretOffset;

                    // Indent the closing brace line as well
                    var closingBraceLine = document.GetLineByOffset(textEditor.CaretOffset + 1);
                    if (closingBraceLine != null)
                    {
                        indentationStrategy.IndentLine(document, closingBraceLine);
                    }
                }
            }
        }

        private static int GetWhitespaceAfterOffset(DocumentLine line, TextDocument document)
        {
            var lineText = document.GetText(line.Offset, line.Length);
            var whitespaceCount = 0;
            foreach (var ch in lineText)
            {
                if (char.IsWhiteSpace(ch))
                    whitespaceCount++;
                else
                    break;
            }
            return whitespaceCount;
        }
    }