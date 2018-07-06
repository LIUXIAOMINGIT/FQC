using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace SerialDevice
{
    /// <summary>
    /// 压力表单位
    /// </summary>
    //public enum PressureUnit
    //{
    //    m = 0,
    //    KPa = 1,
    //    MPa = 2,
    //    C = 3,      //℃
    //    mA = 4,
    //    A = 5,
    //    V = 6,
    //    Other = 7,
    //}

    /// <summary>
    /// { : 返回数据的启示符；
    /// DP : 小数点位置：
    /// 0 代表没有小数点；1 代表有 1 位小数点；
    /// 2 代表有 2 位小数点；3 代表有 3 位小数点；
    /// D5: 返回数据的万位； （数据类型为有符号整形）
    /// D4: 返回数据的千位；
    /// D3: 返回数据的百位；
    /// D2: 返回数据的十位；
    /// D1: 返回数据的个位；
    /// UNIT：从机的单位：
    /// 0- m；1-KPa；2-MPa；3- ℃；4-mA；5-A；6-V；7- 其他；
    /// } ：返回数据的结束符；
    /// </summary>
    public class SerialPressureGauge
    {
        private List<byte> m_ReadBuffer = new List<byte>(); //存放数据缓存，如果数据到达数量少于指定长度，等待下次接受
        private readonly string READLOCK = "lock";
        public event EventHandler<PressureGaugeDataEventArgs> PressureGaugeDataRecerived;
        private List<string> m_OccupancyComList = new List<string>();

        protected SerialBase m_SerialBase = null;
        protected const int FRESHCMDCHECKBYTELENGTH = 9;                    //刷新命令的回应只有9个字节
        protected byte[] m_FreshCmd;
        protected string m_PortName;
        protected int m_BaudRate;
        protected int m_DataBits;
        protected StopBits m_StopBits;
        protected Parity m_Parity;
        protected Hashtable bufferByCom = new Hashtable();      //不同串口，不同的buffer
        protected string m_PluggedPortName = string.Empty;
        protected AutoResetEvent m_FreshEvent = new AutoResetEvent(false);
        protected byte[] m_FreshCmdCheckByte = new byte[7] { 0x55, 0xAA, 0x05, 0x00, 0x00, 0x58, 0x01 };


        /// <summary>
        /// 被其他进程使用的串口列表
        /// </summary>
        public List<string> OccupancyComList
        {
            get { return m_OccupancyComList; }
            set { m_OccupancyComList = value; }
        }

        public SerialPressureGauge()
        {
            m_SerialBase = new SerialBase();
            m_SerialBase.DataReceived += OnDataReceived;
            m_SerialBase.ReadCount = FRESHCMDCHECKBYTELENGTH;
            m_FreshCmd = new byte[5] { 0x40, 0x30, 0x30, 0x31, 0x21 };//@001！
        }

        public SerialPressureGauge(int baudRate, int dataBits, StopBits stopBits, Parity parity, string portName = "")
        {
            m_PortName = portName;
            m_BaudRate = baudRate;
            m_DataBits = dataBits;
            m_StopBits = stopBits;
            m_Parity = parity;
            if (!string.IsNullOrEmpty(portName))
            {
                m_SerialBase = new SerialBase(portName, baudRate, dataBits, stopBits, parity);
                m_SerialBase.DataReceived += OnDataReceived;
            }

            m_FreshCmd = new byte[5] { 0x40, 0x30, 0x30, 0x31, 0x21 };//@001！
            if (!string.IsNullOrEmpty(portName))
                m_SerialBase.ReadCount = FRESHCMDCHECKBYTELENGTH;
        }

        /// <summary>
        /// 处理串口传入的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDataReceived(object sender, DataTransEventArgs e)
        {
            byte[] temp = new byte[FRESHCMDCHECKBYTELENGTH];
            //-32768 ~ 32767在此范围的值，当传过来的值超过32767时一定是负值，此时要异或0xFFFF后再加1补码是为负数的绝对值
            // { DP D5 D4 D3 D2 D1 UNIT }\{ E R R O R 0 1 }\{ E R R O R 0 2 }\{ E R R O R 0 3 }
            try
            {
                m_ReadBuffer.AddRange(e.EventData);
                for (int iLoop = 0; iLoop < m_ReadBuffer.Count; iLoop++)
                {
                    System.Diagnostics.Debug.Write(m_ReadBuffer[iLoop].ToString("X") + " ");
                }
                System.Diagnostics.Debug.WriteLine("");
                PressureGaugeDataEventArgs args = Analyze(m_ReadBuffer);
                if (args!=null && PressureGaugeDataRecerived != null)
                    PressureGaugeDataRecerived(this, args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }

        protected void OnFreshDataReceived(object sender, DataTransEventArgs e)
        {
            List<byte> buffer = (List<byte>)bufferByCom[e.PortName];
            if (e.EventData.Length < FRESHCMDCHECKBYTELENGTH)
            {
                buffer.AddRange(e.EventData);
                if (buffer.Count >= FRESHCMDCHECKBYTELENGTH)
                {
                    if (CompareResponseByte(buffer.ToArray()))
                    {
                        buffer.Clear();
                        m_PluggedPortName = e.PortName;
                        m_FreshEvent.Set();
                    }
                }
            }
            else
            {
                if (CompareResponseByte(e.EventData))
                {
                    buffer.Clear();
                    m_PluggedPortName = e.PortName;
                    m_FreshEvent.Set();
                }
            }
        }

        private PressureGaugeDataEventArgs Analyze(byte[] eventData)
        {
            PressureGaugeDataEventArgs args = null;
            if (eventData.Length != FRESHCMDCHECKBYTELENGTH)
                return null;
            //开始和结束字符错误
            if (eventData[0] != 0x7B || eventData[FRESHCMDCHECKBYTELENGTH - 1] != 0x7D)
                return null;
            //小数点错误
            if (eventData[1] > 0x33 || eventData[1] < 0x30)
                return null;
            //单位错误
            if (eventData[7] > 0x37 || eventData[1] < 0x30)
                return null;

            for (int iLoop = 2; iLoop <= 6; iLoop++)
            {
                if (eventData[iLoop] < 0x30 || eventData[iLoop] > 0x39)
                {
                    return null;
                }
            }
            int D5 = (eventData[2] - 0x30) * 10000;
            int D4 = (eventData[3] - 0x30) * 1000;
            int D3 = (eventData[4] - 0x30) * 100;
            int D2 = (eventData[5] - 0x30) * 10;
            int D1 = (eventData[6] - 0x30);
            int total = D1 + D2 + D3 + D4 + D5;
            if (total > 0xFFFF) //错误数字
                return null;
            ushort num = 0;
            float sum = 0;
            if (total > 32767) //负数
            {
                num = (ushort)total;
                num = (ushort)(num ^ 0xFFFF + 0x0001);
                sum = -1 * num;
            }
            else
            {
                num = (ushort)total;
                sum = num;
            }
            switch (eventData[1])
            {
                case 0x31:
                    sum *= 0.1f;
                    break;
                case 0x32:
                    sum *= 0.01f;
                    break;
                case 0x33:
                    sum *= 0.001f;
                    break;
            }
            PressureUnit unit = (PressureUnit)(eventData[7] - 0x30);
            args = new PressureGaugeDataEventArgs(unit, sum);
            return args;

        }


        private PressureGaugeDataEventArgs Analyze(List<byte> eventData)
        {
            PressureGaugeDataEventArgs args = null;
            if (eventData.Count < FRESHCMDCHECKBYTELENGTH)
                return null;
            byte[] buffer = new byte[FRESHCMDCHECKBYTELENGTH];
            bool bFind = false;
            while (eventData.Count >= FRESHCMDCHECKBYTELENGTH)
            {
                if (eventData[0] != 0x7B)
                {
                    eventData.RemoveAt(0);
                    continue;
                }
                else
                {
                    if (eventData[FRESHCMDCHECKBYTELENGTH - 1] != 0x7D)
                    {
                        eventData.RemoveAt(0);
                        continue;
                    }
                    else
                    {
                        bFind = true;
                        eventData.CopyTo(0, buffer, 0, FRESHCMDCHECKBYTELENGTH);
                        eventData.RemoveRange(0, FRESHCMDCHECKBYTELENGTH);
                    }
                }
            }

            if (bFind)
            {
                //小数点错误
                if (buffer[1] > 0x33 || buffer[1] < 0x30)
                    return null;
                //单位错误
                if (buffer[7] > 0x37 || buffer[1] < 0x30)
                    return null;

                for (int iLoop = 2; iLoop <= 6; iLoop++)
                {
                    if (buffer[iLoop] < 0x30 || buffer[iLoop] > 0x39)
                    {
                        return null;
                    }
                }
                int D5 = (buffer[2] - 0x30) * 10000;
                int D4 = (buffer[3] - 0x30) * 1000;
                int D3 = (buffer[4] - 0x30) * 100;
                int D2 = (buffer[5] - 0x30) * 10;
                int D1 = (buffer[6] - 0x30);
                int total = D1 + D2 + D3 + D4 + D5;
                if (total > 0xFFFF) //错误数字
                    return null;
                ushort num = 0;
                float sum = 0;
                if (total > 32767) //负数
                {
                    num = (ushort)total;
                    num = (ushort)(num ^ 0xFFFF + 0x0001);
                    sum = -1 * num;
                }
                else
                {
                    num = (ushort)total;
                    sum = num;
                }
                switch (buffer[1])
                {
                    case 0x31:
                        sum *= 0.1f;
                        break;
                    case 0x32:
                        sum *= 0.01f;
                        break;
                    case 0x33:
                        sum *= 0.001f;
                        break;
                }
                PressureUnit unit = (PressureUnit)(buffer[7] - 0x30);
                args = new PressureGaugeDataEventArgs(unit, sum);
                return args;
            }
            else
            {
                return null;
            }

        }

        public bool Open()
        {
            if (m_SerialBase != null)
                return m_SerialBase.Open();
            return false;
        }

        public bool IsOpen()
        {
            return this.m_SerialBase.IsOpen();
        }

        public void Close()
        {
            if (m_SerialBase != null && m_SerialBase.IsOpen())
                m_SerialBase.Close();
        }

        /// <summary>
        /// 刷新已连接串口，并返回该串口号
        /// </summary>
        /// <returns>串口号</returns>
        public string FreshCom(string portName = "")
        {
            m_PluggedPortName = "";
            m_FreshEvent.Reset();
            string connectedCom = string.Empty;
            string[] portNames = new string[] { portName };
            List<Thread> threadPool = new List<Thread>();
            List<SerialBase> serialPortPool = new List<SerialBase>();
            bufferByCom.Clear();
            foreach (string port in portNames)
            {
                if (m_OccupancyComList.FindIndex((x) => { return string.Compare(x, port, true) == 0; }) >= 0)
                {
                    continue;
                }
                //开启多线程，每个串口开一个
                bufferByCom.Add(port, new List<byte>());
                Thread freshThread = new Thread(new ParameterizedThreadStart(CheckPlugged));
                SerialBase serialPort = new SerialBase(port,
                                                     m_BaudRate,
                                                     m_DataBits,
                                                     m_StopBits,
                                                     m_Parity
                                                    );
                serialPort.DataReceived += OnFreshDataReceived;
                serialPortPool.Add(serialPort);
                freshThread.Start(serialPort);
                threadPool.Add(freshThread);
            }
            for (int i = 0; i < threadPool.Count; i++)
            {
                threadPool[i].Join();
            }
            if (m_FreshEvent.WaitOne(2000/*WAITFOREVENTTIMEOUT*/))
            {
            }
            for (int i = 0; i < serialPortPool.Count; i++)
            {
                serialPortPool[i].Close();
            }
            for (int i = 0; i < threadPool.Count; i++)
            {
                threadPool[i].Abort();
            }
            return connectedCom = m_PluggedPortName;
        }

        public void SendQueryCmd(/*TestSheet2Result result = null*/)
        {
            //m_CurrentResult = result;
            m_SerialBase.SendData(m_FreshCmd);
        }

        protected void CheckPlugged(object com)
        {
            SerialBase serialPort = com as SerialBase;
            serialPort.ReadCount = FRESHCMDCHECKBYTELENGTH;
            bool bOpen = serialPort.Open();
            if (!bOpen)
                return;
            SendFreshCmd(serialPort);
        }

        protected void SendFreshCmd(SerialBase serialPort)
        {
            serialPort.SendData(m_FreshCmd);
        }

        protected bool CompareResponseByte(byte[] eventData)
        {
            bool bEqual = true;
            if (eventData.Length <= m_FreshCmdCheckByte.Length && eventData.Length != FRESHCMDCHECKBYTELENGTH)
                bEqual = false;
            else
            {
                if (eventData[0] != 0x7B || eventData[FRESHCMDCHECKBYTELENGTH - 1] != 0x7D)
                {
                    bEqual = false;
                }
                else
                {
                    for (int iLoop = 1; iLoop < FRESHCMDCHECKBYTELENGTH - 1; iLoop++)
                    {
                        if (eventData[iLoop] < 0x30 || eventData[iLoop] > 0x39)
                        {
                            bEqual = false;
                            break;
                        }
                    }
                }
            }
            return bEqual;
        }

    }

    public class PressureGaugeDataEventArgs : EventArgs
    {
        public PressureUnit Unit { get; set; }
        public float PressureValue { get; set; }
        public string ErrorMessage { get; set; }

        public PressureGaugeDataEventArgs(PressureGaugeDataEventArgs data)
        {
            this.Unit = data.Unit;
            this.PressureValue = data.PressureValue;
            this.ErrorMessage = data.ErrorMessage;
        }

        public PressureGaugeDataEventArgs(PressureUnit unit, float pressureValue, string errorMessage = "")
        {
            this.Unit = unit;
            this.PressureValue = pressureValue;
            this.ErrorMessage = errorMessage;
        }
    }

}
