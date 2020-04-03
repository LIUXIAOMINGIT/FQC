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
    public partial class MaxKpaAlertForm : Form
    {
        public MaxKpaAlertForm()
        {
            InitializeComponent();
        }

        public MaxKpaAlertForm(bool bPass)
        {
            InitializeComponent();
            //lbResult.Text = string.Format("{0}",bPass?"通过":"失败");
            //if(bPass)
            //{
            //    lbResult.Text = "通过";
            //    lbResult.ForeColor = Color.Green;
            //}
            //else
            //{
            //    lbResult.Text = "失败";
            //    lbResult.ForeColor = Color.Red;
            //}
        }

        public MaxKpaAlertForm(int maxThreshold)
        {
            InitializeComponent();
            lbResult.Text = string.Format("超过最大压力值上限{0}Kpa", maxThreshold);
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
    }
}
