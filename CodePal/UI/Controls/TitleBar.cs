using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodePal.UI.Controls
{
    public class TitleBar : Panel // or TableLayoutPanel, etc.
    {
        private Point _mouseDown;
        private Point _formLocation;
        private bool _capture;

        // NOTE: we cannot use the WM_NCHITTEST / HTCAPTION trick because the table is in control, not the owning form...
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _capture = true;
            _mouseDown = e.Location;
            _formLocation = ((Form)TopLevelControl).Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _capture = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_capture)
            {
                int dx = e.Location.X - _mouseDown.X;
                int dy = e.Location.Y - _mouseDown.Y;
                Point newLocation = new Point(_formLocation.X + dx, _formLocation.Y + dy);
                ((Form)TopLevelControl).Location = newLocation;
                _formLocation = newLocation;
            }
        }
    }
}
