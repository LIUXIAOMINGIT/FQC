using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.IO.Ports;
using System.Runtime.InteropServices;
using PCTool.CommonProcess;
using PCTool.CommonProcess.UserControls;
using ComunicationProtocol.Misc;

namespace VerificationPressure
{
    public class SerialPump
    {
        private ProductModel            m_ProductModel                 = ProductModel.GrasebyC8;
        private ProductID               m_ProductID                    = ProductID.GrasebyC8;
        private int                     m_ChannelNo                    = 1;                                        //F8通道编号

        protected SerialBase            m_SerialBase                   = null;
        protected string                m_PortName;
        protected int                   m_BaudRate;
        protected int                   m_DataBits;
        protected StopBits              m_StopBits;
        protected Parity                m_Parity;
        protected AutoResetEvent        m_FreshEvent                   = new AutoResetEvent(false);
        protected string                m_PluggedPortName              = string.Empty;
        //F8 2通道：55 aa 05 08 01 58 00 9a
        //F8 1通道：55 aa 05 07 01 58 00 9b
        protected  byte[]               m_FreshCmd                     = new byte[8] { 0x55, 0xaa, 0x05, 0x00, 0x01, 0x58, 0x00, 0xa2 };
        //F8 2通道：55 AA 05 08 00 58 01 01 99
        //F8 1通道：55 AA 05 07 00 58 01 01 9A
        protected  byte[]               m_FreshCmdCheckByte            = new byte[7] { 0x55, 0xAA, 0x05, 0x00, 0x00, 0x58, 0x01};
        protected Hashtable             bufferByCom                    = new Hashtable();      //不同串口，不同的buffer
        protected const int             FRESHCMDCHECKBYTELENGTH        = 9;                    //刷新C8命令的回应只有9个字节
        protected const int             WAITFOREVENTTIMEOUT            = 2000;                 //2秒
                                                                                               //protected


        public SerialPump()
        {
            m_SerialBase = new SerialBase();
            m_SerialBase.DataReceived += OnDataReceived;
        }

        public SerialPump(int baudRate, int dataBits, StopBits stopBits, Parity parity, string portName = "")
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
        }

        public SerialPump(ProductID productID, int channel, int baudRate, int dataBits, StopBits stopBits, Parity parity, string portName = "")
        {
            m_ProductID = productID;
            m_ChannelNo = channel;
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
            if(m_ProductID==ProductID.GrasebyF8)
            {
                if (m_ChannelNo == 1)
                {
                    m_FreshCmd[3]          = 0x07;
                    m_FreshCmd[7]          = 0x9b;
                    m_FreshCmdCheckByte[3] = 0x07;
                }
                else if (m_ChannelNo == 2)
                {
                    m_FreshCmd[3]          = 0x08;
                    m_FreshCmd[7]          = 0x9a;
                    m_FreshCmdCheckByte[3] = 0x08;
                }
                else
                {
                    m_FreshCmd[3]          = 0x07;
                    m_FreshCmd[7]          = 0x9b;
                    m_FreshCmdCheckByte[3] = 0x07;
                }
            }
            else if (m_ProductID == ProductID.GrasebyC8)
            {
                m_FreshCmd[3]          = 0x00;
                m_FreshCmd[7]          = 0xa2;
                m_FreshCmdCheckByte[3] = 0x00;
            }
        }

        /// <summary>
        /// 处理串口传入的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnDataReceived(object sender, DataTransEventArgs e)
        {
            //e.EventData
        }

        /// <summary>
        /// 处理串口传入的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnFreshDataReceived(object sender, DataTransEventArgs e)
        {
            List<byte> buffer =(List<byte>)bufferByCom[e.PortName];
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

        [DllImport("msvcrt.dll")]
        protected static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);

        protected virtual bool CompareResponseByte(byte[] eventData)
        {
            bool bEqual = false;
            if (eventData.Length <= m_FreshCmdCheckByte.Length && eventData.Length != 9)
                bEqual = false;
            else
            {
                IntPtr retval = memcmp(m_FreshCmdCheckByte, eventData, new IntPtr(m_FreshCmdCheckByte.Length));
                if (retval.ToInt32() != 0)
                    bEqual = false;
                else
                {
                    if (eventData[7] > 0x03)
                        bEqual = false;
                    else
                    {
                        ushort sum = 0;
                        for(int iLoop=0;iLoop<eventData.Length-1;iLoop++)
                        {
                            sum += eventData[iLoop];
                        }
                        byte checkCode = (byte)(sum ^ 0xFF);
                        if (checkCode==eventData[eventData.Length-1])
                            bEqual = true;
                        else
                            bEqual = false;
                    }
                }
            }
            return bEqual;
        }

        public virtual bool Open()
        {
            return m_SerialBase.Open();
        }

        public virtual void Close()
        {
            m_SerialBase.Close();
        }

        public bool IsOpen()
        {
            return m_SerialBase.IsOpen();
        }

        /// <summary>
        /// 刷新已连接串口，并返回该串口号
        /// </summary>
        /// <returns>串口号</returns>
        public virtual string FreshCom()
        {
            m_FreshEvent.Reset();
            string connectedCom = string.Empty;
            string[] portNames = SerialPort.GetPortNames();
            List<Thread> threadPool = new List<Thread>();
            List<SerialBase> serialPortPool = new List<SerialBase>();
            bufferByCom.Clear();
            foreach (string port in portNames)
            {
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
            if (m_FreshEvent.WaitOne(WAITFOREVENTTIMEOUT))
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

        protected virtual void CheckPlugged(object com)
        {
            SerialBase serialPort = com as SerialBase;
            bool bOpen = serialPort.Open();
            if (!bOpen)
                return;
            SendFreshCmd(serialPort);
        }

        protected virtual void SendFreshCmd(SerialBase serialPort)
        {
            serialPort.SendData(m_FreshCmd);
        }

    }
}
