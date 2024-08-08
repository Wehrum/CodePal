using CodePal.UI.Controls;

namespace CodePal
{
    partial class CodePal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CodeEditor = new ScintillaNET.Scintilla();
            tglSyntaxHighlighting = new ToggleButton();
            TitleBar = new TitleBar();
            AppTitle = new Label();
            lblSyntaxHighLighting = new Label();
            btnCloseApplication = new Button();
            btnNewTask = new Button();
            TitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // CodeEditor
            // 
            CodeEditor.AutocompleteListSelectedBackColor = Color.FromArgb(0, 120, 215);
            CodeEditor.BorderStyle = ScintillaNET.BorderStyle.None;
            CodeEditor.LexerName = null;
            CodeEditor.Location = new Point(282, 58);
            CodeEditor.Name = "CodeEditor";
            CodeEditor.ScrollWidth = 49;
            CodeEditor.Size = new Size(390, 280);
            CodeEditor.TabIndex = 1;
            // 
            // tglSyntaxHighlighting
            // 
            tglSyntaxHighlighting.Location = new Point(633, 32);
            tglSyntaxHighlighting.Name = "tglSyntaxHighlighting";
            tglSyntaxHighlighting.Padding = new Padding(6);
            tglSyntaxHighlighting.Size = new Size(39, 20);
            tglSyntaxHighlighting.TabIndex = 0;
            tglSyntaxHighlighting.UseVisualStyleBackColor = true;
            tglSyntaxHighlighting.CheckedChanged += tglSyntaxHighlighting_CheckedChanged;
            // 
            // TitleBar
            // 
            TitleBar.BackColor = Color.FromArgb(30, 30, 30);
            TitleBar.Controls.Add(AppTitle);
            TitleBar.Location = new Point(0, 0);
            TitleBar.Name = "TitleBar";
            TitleBar.Size = new Size(684, 26);
            TitleBar.TabIndex = 2;
            // 
            // AppTitle
            // 
            AppTitle.AutoSize = true;
            AppTitle.ForeColor = Color.White;
            AppTitle.Location = new Point(316, 6);
            AppTitle.Name = "AppTitle";
            AppTitle.Size = new Size(54, 15);
            AppTitle.TabIndex = 3;
            AppTitle.Text = "Code Pal";
            // 
            // lblSyntaxHighLighting
            // 
            lblSyntaxHighLighting.AutoSize = true;
            lblSyntaxHighLighting.ForeColor = Color.White;
            lblSyntaxHighLighting.Location = new Point(515, 34);
            lblSyntaxHighLighting.Name = "lblSyntaxHighLighting";
            lblSyntaxHighLighting.Size = new Size(112, 15);
            lblSyntaxHighLighting.TabIndex = 3;
            lblSyntaxHighLighting.Text = "Syntax Highlighting";
            // 
            // btnCloseApplication
            // 
            btnCloseApplication.BackColor = Color.Red;
            btnCloseApplication.FlatAppearance.BorderColor = Color.Black;
            btnCloseApplication.FlatAppearance.BorderSize = 0;
            btnCloseApplication.FlatStyle = FlatStyle.Flat;
            btnCloseApplication.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCloseApplication.ForeColor = Color.White;
            btnCloseApplication.Location = new Point(661, 3);
            btnCloseApplication.Name = "btnCloseApplication";
            btnCloseApplication.Size = new Size(20, 20);
            btnCloseApplication.TabIndex = 4;
            btnCloseApplication.Text = "✖️";
            btnCloseApplication.UseVisualStyleBackColor = false;
            btnCloseApplication.Click += btnCloseApplication_Click;
            // 
            // btnNewTask
            // 
            btnNewTask.FlatAppearance.BorderSize = 0;
            btnNewTask.FlatStyle = FlatStyle.Flat;
            btnNewTask.Font = new Font("Cascadia Mono SemiBold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNewTask.ForeColor = Color.White;
            btnNewTask.Location = new Point(12, 299);
            btnNewTask.Name = "btnNewTask";
            btnNewTask.Size = new Size(40, 40);
            btnNewTask.TabIndex = 5;
            btnNewTask.Text = "+";
            btnNewTask.UseVisualStyleBackColor = true;
            btnNewTask.Click += btnNewTask_Click;
            // 
            // CodePal.WinForms
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(684, 350);
            Controls.Add(btnNewTask);
            Controls.Add(btnCloseApplication);
            Controls.Add(lblSyntaxHighLighting);
            Controls.Add(TitleBar);
            Controls.Add(tglSyntaxHighlighting);
            Controls.Add(CodeEditor);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(684, 350);
            MinimizeBox = false;
            MinimumSize = new Size(684, 350);
            Name = "CodePal";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Code Pal";
            Load += CodePal_Load;
            TitleBar.ResumeLayout(false);
            TitleBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScintillaNET.Scintilla CodeEditor;
        private ToggleButton tglSyntaxHighlighting;
        private TitleBar TitleBar;
        private Label AppTitle;
        private Label lblSyntaxHighLighting;
        private Button btnCloseApplication;
        private Button btnNewTask;
    }
}
