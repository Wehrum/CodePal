using ScintillaNET;
using CodePal.CodeEditor.Themes.CSharp;
using CodePal.UI;
using CodePal.TaskQueue;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace CodePal
{
    public partial class CodePal : Form
    {
        #region Click Anywhere To Drag
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);

                    if ((int)m.Result == HTCLIENT)
                        m.Result = (IntPtr)HTCAPTION;
                    return;
            }

            base.WndProc(ref m);
        }
        #endregion

        #region Syntax Highlighting Event Handling
        private void tglSyntaxHighlighting_CheckedChanged(object sender, EventArgs e)
        {
            CSharpDarkMode CSharpDarkMode = new CSharpDarkMode();

            if (tglSyntaxHighlighting.Checked)
            {
                CSharpDarkMode.EnableDarkMode(CodeEditor);
                CSharpDarkMode.SetKeywords(CodeEditor);
                EnableBraceMatching = true;
                EnableAutoIndent = true;
                EnableCodeFolding = true;
                SetCodeFolding();
            }
            else
            {
                CSharpDarkMode.DisableDarkMode(CodeEditor);
                EnableBraceMatching = false;
                EnableAutoIndent = false;
                EnableCodeFolding = false;
                SetCodeFolding();
            }
        }

        private int _maxLineNumberCharLength;
        private int _lastCaretPos = 0;

        bool EnableBraceMatching = false;
        bool EnableAutoIndent = false;
        bool EnableCodeFolding = false;

        char IndentChar = '{';
        char OutdentChar = '}';
        bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }
        private void scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            if (EnableBraceMatching)
            {
                CodeEditor.UpdateUI += BraceMatching_UpdateUI;
            }
            else
            {
                CodeEditor.UpdateUI -= BraceMatching_UpdateUI;
            }
        }
        private void scintilla_Insert(object sender, ModificationEventArgs e)
        {
            if (EnableAutoIndent)
            {
                CodeEditor.InsertCheck += AutoIndent_InsertCheck;
                CodeEditor.CharAdded += AutoIndent_CharAdded;
            }
            else
            {
                CodeEditor.InsertCheck -= AutoIndent_InsertCheck;
                CodeEditor.CharAdded -= AutoIndent_CharAdded;
            }
        }

        //Brace Matching
        private void BraceMatching_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            if (EnableBraceMatching)
            {
                var caretPos = CodeEditor.CurrentPosition;
                if (_lastCaretPos != caretPos)
                {
                    _lastCaretPos = caretPos;
                    var bracePos1 = -1;

                    // Is there a brace to the left or right?
                    if (caretPos > 0 && IsBrace(CodeEditor.GetCharAt(caretPos - 1)))
                        bracePos1 = (caretPos - 1);
                    else if (IsBrace(CodeEditor.GetCharAt(caretPos)))
                        bracePos1 = caretPos;

                    if (bracePos1 >= 0)
                    {
                        // Find the matching brace
                        var bracePos2 = CodeEditor.BraceMatch(bracePos1);
                        if (bracePos2 == ScintillaNET.Scintilla.InvalidPosition)
                            CodeEditor.BraceBadLight(bracePos1);
                        else
                            CodeEditor.BraceHighlight(bracePos1, bracePos2);
                    }
                    else
                    {
                        // Turn off brace matching
                        CodeEditor.BraceHighlight(ScintillaNET.Scintilla.InvalidPosition, ScintillaNET.Scintilla.InvalidPosition);
                    }
                }
            }
        }
        private void AutoIndent_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            if (e.Text.EndsWith("\r") || e.Text.EndsWith("\n"))
            {
                int startPos = CodeEditor.Lines[CodeEditor.LineFromPosition(CodeEditor.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = CodeEditor.GetTextRange(startPos, (endPos - startPos));
                //Text until the caret so that the whitespace is always equal in every line.

                Match indent = Regex.Match(curLineText, "^[ \\t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch(curLineText, IndentChar + "\\s*$"))
                {
                    e.Text = (e.Text + "    ");
                }
            }
        }
        //Auto Indent
        private void AutoIndent_CharAdded(object sender, CharAddedEventArgs e)
        {
            if (IndentChar == e.Char)
            {
                int curLine = CodeEditor.LineFromPosition(CodeEditor.CurrentPosition);

                if (CodeEditor.Lines[curLine].Text.Trim() == "}")
                {
                    //Check whether the bracket is the only thing on the line.. For cases like "if() { }".
                    SetIndent(curLine, GetIndent(curLine) - 4);
                }
            }
            else if (OutdentChar == e.Char)
            {
                int curLine = CodeEditor.LineFromPosition(CodeEditor.CurrentPosition);

                if (CodeEditor.Lines[curLine].Text.Trim() == "" + OutdentChar)
                {
                    SetIndent(curLine, GetIndent(curLine) - 4);
                }
            }
        }

        //Codes for the handling the Indention of the lines.
        //They are manually added here until they get officially added to the Scintilla control.
        const int SCI_SETLINEINDENTATION = 2126;
        const int SCI_GETLINEINDENTATION = 2127;
        private void SetIndent(int line, int indent)
        {
            CodeEditor.DirectMessage(SCI_SETLINEINDENTATION, new IntPtr(line), new IntPtr(indent));
        }
        private int GetIndent(int line)
        {
            return (CodeEditor.DirectMessage(SCI_GETLINEINDENTATION, new IntPtr(line), IntPtr.Zero).ToInt32());
        }

        public void SetCodeFolding()
        {
            if (EnableCodeFolding)
            {
                CodeEditor.SetProperty("fold", "1");

                // Use margin 2 for fold markers
                CodeEditor.Margins[2].Type = MarginType.Symbol;
                CodeEditor.Margins[2].Mask = Marker.MaskFolders;
                CodeEditor.Margins[2].Sensitive = true;
                CodeEditor.Margins[2].Width = 20;

                // Reset folder markers
                for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
                {
                    CodeEditor.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                    CodeEditor.Markers[i].SetBackColor(SystemColors.ControlDark);
                }

                // Style the folder markers
                CodeEditor.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
                CodeEditor.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
                CodeEditor.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
                CodeEditor.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
                CodeEditor.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
                CodeEditor.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
                CodeEditor.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
                CodeEditor.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
                CodeEditor.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

                // Enable automatic folding
                CodeEditor.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);
            }
            else
            {
                CodeEditor.SetProperty("fold", "0");
                CodeEditor.Margins[2].Width = 0;
            }
        }

        #endregion

        public CodePal()
        {
            InitializeComponent();
            
            // Handle important form properties
            this.Opacity = .90;
            this.TopMost = true;

            // Ensure Syntax Highlighting is enabled by default.
            tglSyntaxHighlighting.Checked = true;
        }

        private void CodePal_Load(object sender, EventArgs e)
        {

        }

        private void btnCloseApplication_Click(object sender, EventArgs e)
        {
            this.Dispose();
            GC.Collect();
            this.Close();
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            // Set the Location appropriately
            TaskQueue.TaskQueueList newTaskQueue = new TaskQueue.TaskQueueList(TaskQueueManager.controlLocationX, TaskQueueManager.controlLocationY);

            this.Controls.Add(newTaskQueue);

            TaskQueueManager.controlLocationY += 50;
        }
    }
}
