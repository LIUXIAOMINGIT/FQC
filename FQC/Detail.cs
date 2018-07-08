using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Misc = ComunicationProtocol.Misc;

namespace FQC
{
    public struct FQCData
    {
        public string brand;
        public int syrangeSize;
        public float pressureN;
        public float pressureL;
        public float pressureC;
        public float pressureH;
    }

    public partial class Detail : UserControl
    {
        private int    m_SyrangeSize     = 50;
        private PumpID m_LocalPid = PumpID.GrasebyF8;//默认显示的是F8
        private int    m_Channel  = 1;               //1号通道，默认值

        public int SyrangeSize
        {
            set { m_SyrangeSize = value; }
            get { return m_SyrangeSize ; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        public PumpID Pid
        {
            set { m_LocalPid = value; }
            get { return m_LocalPid; }
        }

        /// <summary>
        /// 第几道泵的详细信息
        /// </summary>
        public int Channel
        {
            set { m_Channel = value; }
            get { return m_Channel; }
        }
        
        public Detail()
        {
            InitializeComponent();
        }

        public void ClearLabelValue()
        {
            lbNValue.Text     = "";
            lbCValue.Text     = "";
            lbSizeValue.Text  = "";
            lbLValue.Text     = "";
            lbHValue.Text     = "";
            lbBrandValue.Text = "";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void SetFQCResult(string brand, int size, float n, float l, float c, float h)
        {
            lbBrandValue.Text = brand;
            lbSizeValue.Text  = size.ToString();
            lbNValue.Text     = n.ToString("F1");
            lbLValue.Text     = l.ToString("F1");
            lbCValue.Text     = c.ToString("F1");
            lbHValue.Text     = h.ToString("F1");
        }

        public void SetFQCResult(FQCData data)
        {
            lbBrandValue.Text = data.brand;
            lbSizeValue.Text = data.syrangeSize.ToString();
            lbNValue.Text = data.pressureN.ToString("F1");
            lbLValue.Text = data.pressureL.ToString("F1");
            lbCValue.Text = data.pressureC.ToString("F1");
            lbHValue.Text = data.pressureH.ToString("F1");
        }

       
    }
}
