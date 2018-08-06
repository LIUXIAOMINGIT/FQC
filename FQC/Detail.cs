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
        public int    Channel;
        public string brand;
        public int    syrangeSize;
        public float  rate;
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
        private TestResult _result = new TestResult();

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
            if (string.IsNullOrEmpty(brand))
                return;

            lbBrandValue.Text = brand;
            lbSizeValue.Text  = size.ToString();
            lbNValue.Text     = n.ToString("F1");
            lbLValue.Text     = l.ToString("F1");
            lbCValue.Text     = c.ToString("F1");
            lbHValue.Text     = h.ToString("F1");
        }

        public void SetFQCResult(FQCData data)
        {
            if(string.IsNullOrEmpty(data.brand))
                return;

            lbBrandValue.Text = data.brand;
            lbSizeValue.Text = data.syrangeSize.ToString();

            if(data.pressureN<=0)
                lbNValue.Text = "----";
            else
                lbNValue.Text = data.pressureN.ToString("F1");

            if(data.pressureL<=0)
                lbLValue.Text = "----";
            else
                lbLValue.Text = data.pressureL.ToString("F1");

            if (data.pressureC <= 0)
                lbCValue.Text = "----";
            else
                lbCValue.Text = data.pressureC.ToString("F1");

            if (data.pressureH <= 0)
                lbHValue.Text = "----";
            else
                lbHValue.Text = data.pressureH.ToString("F1");
            IsPass(data.pressureN, data.pressureL, data.pressureC, data.pressureH);
        }

        public void IsPass(float n, float l, float c, float h)
        {
            ProductID pid = ProductIDConvertor.PumpID2ProductID(m_LocalPid);
            PressureConfig cfg = PressureManager.Instance().Get(pid);
            var parameter = cfg.Find(Misc.OcclusionLevel.N);
            if (parameter != null && n>0)
                if (n >= parameter.Item2 && n <= parameter.Item3)
                    lbNValue.ForeColor = Color.White;
                else
                    lbNValue.ForeColor = Color.Red;

            parameter = cfg.Find(Misc.OcclusionLevel.L);
            if (parameter != null && l > 0)
                if (l >= parameter.Item2 && l <= parameter.Item3)
                    lbLValue.ForeColor = Color.White;
                else
                    lbLValue.ForeColor = Color.Red;

            parameter = cfg.Find(Misc.OcclusionLevel.C);
            if (parameter != null && c > 0)
                if (c >= parameter.Item2 && c <= parameter.Item3)
                    lbCValue.ForeColor = Color.White;
                else
                    lbCValue.ForeColor = Color.Red;

            parameter = cfg.Find(Misc.OcclusionLevel.H);
            if (parameter != null && h > 0)
                if (h >= parameter.Item2 && h <= parameter.Item3)
                    lbHValue.ForeColor = Color.White;
                else
                    lbHValue.ForeColor = Color.Red;
        }

       
    }
}
