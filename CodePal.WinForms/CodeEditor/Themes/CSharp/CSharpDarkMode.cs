using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePal.CodeEditor.Themes.CSharp
{
    public class CSharpDarkMode
    {
        private Color _backupCaretForeColor;
        public void EnableDarkMode(ScintillaNET.Scintilla scintilla)
        {
            //Some base styles
            scintilla.IndentationGuides = IndentView.LookBoth;
            scintilla.Styles[Style.BraceLight].BackColor = Color.LightGray;
            scintilla.Styles[Style.BraceLight].ForeColor = Color.BlueViolet;
            scintilla.Styles[Style.BraceBad].ForeColor = Color.Red;

            scintilla.Styles[Style.Default].Font = "Cascadia Mono";
            scintilla.Styles[Style.Default].Size = 10;

            var backColor = Color.FromArgb(30, 30, 30);
            var foreColor = Color.FromArgb(220, 220, 220);

            foreach (var style in scintilla.Styles)
                style.BackColor = backColor;

            // Configure the CPP (C#) lexer styles
            scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Cpp.Identifier].ForeColor = Color.FromArgb(220, 220, 220);
            scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(59, 170, 57); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(59, 170, 57); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(59, 170, 57); // Green
            scintilla.Styles[Style.Cpp.Number].ForeColor = Color.FromArgb(223, 255, 206); // Bright Green (nearly White)
            scintilla.Styles[Style.Cpp.Word].ForeColor = Color.FromArgb(93, 168, 230); // Blue
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.FromArgb(71, 200, 185); // Turqoise
            scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(252, 156, 108); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(252, 156, 108); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(252, 156, 108); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.FromArgb(200, 130, 130); // Dark Red
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.FromArgb(200, 230, 255); // Bright Blue (nearly White)
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.FromArgb(146, 206, 255); // Bright Blue

            scintilla.Styles[Style.IndentGuide].ForeColor = Color.FromArgb(105, 105, 105);
            scintilla.Styles[Style.LineNumber].ForeColor = Color.FromArgb(146, 206, 255); // Bright Blue

            _backupCaretForeColor = scintilla.CaretForeColor;
            scintilla.CaretForeColor = Color.FromArgb(220, 220, 220);

            scintilla.SetFoldMarginHighlightColor(true, backColor);
            scintilla.SetFoldMarginColor(true, backColor);

            scintilla.SetSelectionBackColor(true, Color.FromArgb(38, 79, 120));

            //scintilla.LexerName = "cpp";

            // Set the keywords
            //scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            //scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }

        public void DisableDarkMode(ScintillaNET.Scintilla scintilla)
        {
            scintilla.SetSelectionBackColor(true, Color.Silver);

            scintilla.StyleClearAll();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;

            // Default white background black text
            var backColor = Color.White;
            var foreColor = Color.Black;

            foreach (var style in scintilla.Styles)
            {
                style.BackColor = backColor;
                style.ForeColor = foreColor;
            }
        }

        public void SetKeywords(ScintillaNET.Scintilla scintilla)
        {
            scintilla.LexerName = "cpp";
            scintilla.SetKeywords(0, "abstract partial as base break case catch checked continue default" +
                                     " delegate do else event explicit extern false finally fixed for foreach" +
                                     " goto if implicit in interface internal is lock namespace new null" +
                                     " operator out override params private protected public readonly ref return" +
                                     " sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe" +
                                     " using virtual while volatile yield var async await" +
                                     " object bool byte char class const decimal double enum float int long sbyte short" +
                                     " static string struct uint ulong ushort void dynamic ");


            var builtInTypeNames = typeof(string).Assembly.GetTypes()
                .Where(t => t.IsPublic && t.IsVisible)
                .Select(t => new { t.Name, Length = t.Name.IndexOf('`') }) // remove generic type from "List`1"
                .Select(x => x.Length == -1 ? x.Name : x.Name.Substring(0, x.Length))
                .Distinct();

            scintilla.SetKeywords(1, string.Join(" ", builtInTypeNames));
        }
    }
}

