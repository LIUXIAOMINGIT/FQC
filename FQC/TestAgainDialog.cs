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
    public partial class TestAgainDialog : Form
    {
        private int _channel = 1;       //默认是从第一道先测试，弹出第二道提示框
        private bool moving = false;
        private Point oldMousePosition;
        public TestAgainDialog(int channel=1)
        {
            InitializeComponent();
            _channel = channel;
            InitUI4Channel2(channel);
        }

        private void InitUI4Channel2(int channel)
        {
            if (channel == 2)
            {
                lbResult.Text = "不合格，“确定”结束测试，“复测”重新测试";
                btnCancel.Visible = false;
            }
            else
            {
                lbResult.Text = "不合格，“确定”继续测试，“复测”重新测试，“取消”结束本次测试";
                btnCancel.Visible = true;
            }
        }

        private void tlpmain_Paint(object sender, PaintEventArgs e)
        {
            Pen borderPen = new Pen(Color.FromArgb(19, 113, 185));
            e.Graphics.DrawRectangle(borderPen, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

      
        private void btnTestAgain_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.Close();
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
