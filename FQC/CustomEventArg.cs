using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace FQC
{

    public class StartOrStopArgs : EventArgs
    {
        private bool m_IsStart = false;

        public bool IsStart
        {
            get { return m_IsStart; }
            set { m_IsStart = value; }
        }

        public StartOrStopArgs(bool isStart)
        {
            m_IsStart = isStart;
        }
    }

    /// <summary>
    /// 当测量结束后，将数据发给主界面，由主界面统一调度
    /// </summary>
    public class DoublePumpDataArgs : EventArgs
    {
        public bool IsPass { get; set; }

        private FQCData m_Data = new FQCData();

        public FQCData Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public DoublePumpDataArgs(FQCData data, bool isPass = true)
        {
            m_Data = data;
            IsPass = isPass;
        }
    }


    public class SerialPortParameter
    {
        private int m_BaudRate = 9600;
        private int m_DataBits = 8;
        private StopBits m_StopBits = StopBits.One;
        private Parity m_Parity = Parity.None;

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return m_BaudRate; }
            set { m_BaudRate = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits
        {
            get { return m_DataBits; }
            set { m_DataBits = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBit
        {
            get { return m_StopBits; }
            set { m_StopBits = value; }
        }

        /// <summary>
        /// 奇偶检验
        /// </summary>
        public Parity ParityType
        {
            get { return m_Parity; }
            set { m_Parity = value; }
        }

        public SerialPortParameter()
        {

        }

        public SerialPortParameter(int baudRate, int dataBits, StopBits stopBit, Parity parity)
        {
            m_BaudRate = baudRate;
            m_DataBits = dataBits;
            m_StopBits = stopBit;
            m_Parity = parity;
        }
    }


    public class TestResult
    {
        public float pressureN;
        public bool isPassN;
        public float pressureL;
        public bool isPassL;
        public float pressureC;
        public bool isPassC;
        public float pressureH;
        public bool isPassH;
    }



}
 
