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
        private int _channel = 2;       //默认是从第一道先测试，弹出第二道提示框

        public TestAgainDialog()
        {
            InitializeComponent();
        }

        public TestAgainDialog(int channel)
        {
            InitializeComponent();
            _channel = channel;
            HintFormat(channel);
        }

        private void HintFormat(int channel)
        {
            string format = "点击确定继续测试第{0}道泵";
            string msg = string.Format(format, channel);
            lbResult.Text = msg;
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
    }
}
