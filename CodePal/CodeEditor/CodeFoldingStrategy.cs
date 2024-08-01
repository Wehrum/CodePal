using System.Collections.Generic;
using AvaloniaEdit.Document;
using AvaloniaEdit.Folding;


namespace CodePal.CodeEditor;

public class CodeFoldingStrategy
{
    public void UpdateFoldings(FoldingManager manager, TextDocument document)
    {
        var foldings = CreateNewFoldings(document, out var firstErrorOffset);
        manager.UpdateFoldings(foldings, firstErrorOffset);
    }

    public IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
    {
        firstErrorOffset = -1;
        var newFoldings = new List<NewFolding>();
        var startOffsets = new Stack<int>();

        for (int offset = 0; offset < document.TextLength; offset++)
        {
            char c = document.GetCharAt(offset);
            switch (c)
            {
                case '{':
                    startOffsets.Push(offset);
                    break;
                case '}':
                    if (startOffsets.Count > 0)
                    {
                        int startOffset = startOffsets.Pop();
                        // Add a folding if the block spans more than one line
                        int startLine = document.GetLineByOffset(startOffset).LineNumber;
                        int endLine = document.GetLineByOffset(offset).LineNumber;
                        if (startLine < endLine)
                        {
                            newFoldings.Add(new NewFolding(startOffset, offset + 1));
                        }
                    }
                    break;
            }
        }

        newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
        return newFoldings;
    }
}