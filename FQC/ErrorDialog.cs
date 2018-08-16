using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FQC
{
    public partial class ErrorDialog : Form
    {
        private bool moving = false;
        private Point oldMousePosition;

        public ErrorDialog(string errorMessage)
        {
            InitializeComponent();
            lbResult.Text = errorMessage;
        }
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
        private void tlpmain_Paint(object sender, PaintEventArgs e)
        {
            Pen borderPen = new Pen(Color.FromArgb(19, 113, 185));
            e.Graphics.DrawRectangle(borderPen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }

        private void pnlHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return;
            }
            oldMousePosition = e.Location;
            moving = true;
        }

        private void pnlHead_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && moving)
            {
                Point newPosition = new Point(e.Location.X - oldMousePosition.X, e.Location.Y - oldMousePosition.Y);
                this.Location += new Size(newPosition);
            }
        }

        private void pnlHead_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }
    }
}
