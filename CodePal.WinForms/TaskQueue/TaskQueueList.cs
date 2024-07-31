using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePal.TaskQueue
{
    public class TaskQueueList : RichTextBox
    {
        #region Import GetScollPos from user32.dll
        // Import GetScrollPos from user32.dll
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        private const int SB_VERT = 1; // Vertical scroll bar 
        #endregion

        // Create instance of the TaskQueue specific to the instance of TaskQueueList created by the user.
        TaskQueue myQueue = new TaskQueue();

        // Make the DeleteButton accessible
        private Button DeleteButton;

        #region Constructor and Initial Formatting
        // Constructor takes the desired x/y position for the placement of the control.
        public TaskQueueList(int locationX, int locationY)
        {
            // Title text and first bullet point to be changed by the user.
            if (this.Text.Length == 0)
            {
                this.Text = "Title\nEnter text here";
            }

            // Positioning
            this.Size = new System.Drawing.Size(263, 20);
            this.Location = new System.Drawing.Point(locationX, locationY);

            // Styling
            this.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            this.BorderStyle = BorderStyle.None;
            this.Font = new Font("Cascadia Mono", 10, FontStyle.Regular);
            this.ForeColor = Color.White;

            // Other Properties

            //Disabled because it is being handled by the title styler
            //this.SelectionBullet = true;

            this.SelectionIndent = 8;
            this.SelectionHangingIndent = 3;
            this.SelectionRightIndent = 12;
            this.ScrollBars = RichTextBoxScrollBars.None;

            // Event Handling
            this.KeyDown += KeyDown_Event;
            this.GotFocus += GotFocus_Event;
            this.LostFocus += LostFocus_Event;
            this.SizeChanged += TaskQueueList_SizeChanged; // Handle size change
            this.TextChanged += TextChanged_Event; // Handle text changes
            this.VScroll += TaskQueueList_VScroll; // Handle vertical scroll

            CreateDeleteButton();
            StyleTitle(); // Apply title styling initially
        }
        #endregion

        #region Set The Title Line
        // Create the title of the TaskQueueList so that when the control loses focus, the user can still know what the control contains.
        private void StyleTitle()
        {
            if (this.Lines.Length > 0)
            {
                this.SelectionStart = 0;
                this.SelectionLength = this.Lines[0].Length;

                this.SelectionFont = new Font("Sergoe UI", 12, FontStyle.Regular);
                this.SelectionColor = Color.Yellow;
                this.SelectionAlignment = HorizontalAlignment.Center;


                // Reset selection to end
                this.SelectionStart = this.Text.Length;
                this.SelectionLength = 0;

                this.SelectionBullet = true; // Re-enable bullets for subsequent lines
            }
        }
        #endregion

        #region TaskQueueList Related Events
        // Check if the button pressed during the KeyDown event is 'Enter', if it is, add the line to the TaskQueue as a task.
        private void KeyDown_Event(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string[] TodoEntries = this.Lines;

                for (int i = 0; i < TodoEntries.Length; i++)
                {
                    if (TodoEntries[i] == TodoEntries[TodoEntries.Length - 1])
                    {
                        myQueue.AddTask(TodoEntries[i]);
                    }
                }
                UpdateDeleteButtonPosition(); // Update button position when a new line is added
            }
        }

        // Increase the size of the TaskQueueList to display all bullet points when the control has Focus.
        private void GotFocus_Event(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(264, 160);
            UpdateDeleteButtonPosition();
            StyleTitle(); // Ensure title styling is applied
        }

        // Reduce the size of the TaskQueueList to only show the title and delete button of the TaskQueueList when the control loses Focus.
        private void LostFocus_Event(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(264, 20);
            UpdateDeleteButtonPosition();
            StyleTitle(); // Ensure title styling is applied
        }
        #endregion

        #region DeleteButton & Delettion Event
        // Create our delete button which deletes the control. Event handling for this also disposes of the instance of the TaskQueue and TaskQueueList
        private void CreateDeleteButton()
        {
            DeleteButton = new Button();
            DeleteButton.Size = new System.Drawing.Size(25, 25);
            DeleteButton.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
            DeleteButton.FlatStyle = FlatStyle.Flat;
            DeleteButton.FlatAppearance.BorderSize = 0;
            DeleteButton.Font = new Font("Cascadia Mono", 8, FontStyle.Regular);
            DeleteButton.ForeColor = Color.White;
            DeleteButton.TextAlign = ContentAlignment.TopLeft;
            DeleteButton.Text = "🗑";

            // Set initial position
            DeleteButton.Location = new Point(this.Width - DeleteButton.Width, 0);

            //Set the Event Handler
            this.DeleteButton.Click += DeleteButtonClick_Event;

            this.Controls.Add(DeleteButton);
        }

        private void DeleteButtonClick_Event(object sender, EventArgs e)
        {
            // Perform cleanup on myQueue if needed
            myQueue = null;

            // Remove the TaskQueueList control from its parent
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }

            // Dispose of the control to free resources
            this.Dispose();

            // Decrement the variable holding the Y position for the NewTask button in the main form
            TaskQueueManager.controlLocationY -= 50;
        }
        #endregion

        #region DeleteButton Positioning & Related Events
        // Check to make sure the DeleteButton is not null before updating the position.
        private void UpdateDeleteButtonPosition()
        {
            if (DeleteButton != null)
            {
                // Ensure the button stays at the top-right corner of the visible area
                Point scrollOffset = GetScrollOffset();
                DeleteButton.Location = new Point(this.Width - DeleteButton.Width, scrollOffset.Y);
                // For debugging, log the button's position
                Console.WriteLine($"Button Position: {DeleteButton.Location}");
            }
        }

        // Ensure that the DeleteButton is always displayed at the top right of the control by updating its positon when the size of the control changes
        private void TaskQueueList_SizeChanged(object sender, EventArgs e)
        {
            UpdateDeleteButtonPosition();
        }

        // Ensure that the DeleteButton is always displayed at the top right of the control by updating its position when text is modified
        private void TextChanged_Event(object sender, EventArgs e)
        {
            UpdateDeleteButtonPosition();
            //StyleTitle(); // Ensure title styling is applied
        }

        // Ensure that the DeleteButton is always displayed at the top right of the control by updating the position when the user scrolls.
        private void TaskQueueList_VScroll(object sender, EventArgs e)
        {
            UpdateDeleteButtonPosition();
        }

        // Use the user32.dll to get the scroll offset that assists in update the DeleteButton position.
        private Point GetScrollOffset()
        {
            // Get the scroll offset using Windows API
            int scrollPos = GetScrollPos(this.Handle, SB_VERT);
            return new Point(0, scrollPos);
        }
        #endregion
    }
}