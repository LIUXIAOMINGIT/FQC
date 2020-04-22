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
    public partial class NewTestAgainDialog : Form
    {
        private int _channel = 1;       //默认是从第一道先测试，弹出第二道提示框
        private bool moving = false;
        private Point oldMousePosition;
        private List<LevelTips> mErrorList = new List<LevelTips>();
        private bool isPass = true;

        public NewTestAgainDialog(int channel, List<LevelTips> strErrorList)
        {
            InitializeComponent();
            _channel = channel;
            mErrorList = strErrorList;
            //InitUI4Channel2(channel);
            foreach (var error in strErrorList)
            {
                if (!error.isPass)
                    isPass = isPass && false;
                SetLevelTips(error.level, error.isPass, error.tips);
            }

            if (isPass)
            {
                lbTitle.Text = "合格";
                tlpTitle.BackColor = Color.Green;
            }
            else
            {
                lbTitle.Text = "失败";
                tlpTitle.BackColor = Color.Red;
            }
        }

        public NewTestAgainDialog(List<LevelTips> strErrorList)
        {
            InitializeComponent();
            mErrorList = strErrorList;
            //InitUI4Channel2(channel);

            foreach(var error in strErrorList)
            {
                if (!error.isPass)
                    isPass = isPass && false;
                SetLevelTips(error.level, error.isPass, error.tips);
            }

            if (isPass)
            {
                lbTitle.Text = "合格";
                tlpTitle.BackColor = Color.Green;
            }
            else
            {
                lbTitle.Text = "失败";
                tlpTitle.BackColor = Color.Red;
            }
        }

        private void InitUI4Channel2(int channel)
        {
            //if (channel           == 2)
            //{
            //    lbLevel5.Text     = "不合格，“确定”结束测试，“复测”重新测试";
            //    btnCancel.Visible = false;
            //}
            //else
            //{
            //    lbLevel5.Text     = "不合格，“确定”继续测试，“复测”重新测试，“取消”结束本次测试";
            //    btnCancel.Visible = true;
            //}
        }

        /// <summary>
        /// 压力等级从1到5（N到H算是从1到4）
        /// </summary>
        /// <param name="level"></param>
        public void SetLevelTips(int level, bool isPass, string tips)
        {
            switch(level)
            {
                case 1:
                    lbLevel1.Visible = true;
                    lbLevel1.Text = tips;
                    lbLevel1.ForeColor = isPass? Color.Black : Color.Red;
                    break;
                case 2:
                    lbLevel2.Visible = true;
                    lbLevel2.Text = tips;
                    lbLevel2.ForeColor = isPass ? Color.Black : Color.Red;
                    break;
                case 3:
                    lbLevel3.Visible = true;
                    lbLevel3.Text = tips;
                    lbLevel3.ForeColor = isPass ? Color.Black : Color.Red;
                    break;
                case 4:
                    lbLevel4.Visible = true;
                    lbLevel4.Text = tips;
                    lbLevel4.ForeColor = isPass ? Color.Black : Color.Red;
                    break;
                case 5:
                    lbLevel5.Visible = true;
                    lbLevel5.Text = tips;
                    lbLevel5.ForeColor = isPass ? Color.Black : Color.Red;
                    break;
                default:
                    break;
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
