using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace SerialDevice
{
    public class SerialBase
    {
        public event EventHandler<DataTransEventArgs> DataReceived;
        private SerialPort m_SerialPort    = null;
        private byte[]     m_ConstBuffer   = null;
        private int        m_ReceivedCount = 0;
        private int        m_ReadCount     = 0;

        /// <summary>
        /// 读固定长度
        /// </summary>
        public int ReadCount
        {
            get {
                return m_ReadCount;
            }
            set
            { 
                m_ReadCount = value;
                m_ConstBuffer = new byte[m_ReadCount];
            }
        }

        public SerialBase()
        {
            m_SerialPort = new SerialPort();
        }

        public SerialBase(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
        {
            m_SerialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }

        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="portName">串口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="parity">奇偶检验</param>
        public void InitSerialPort(string portName, int baudRate, int dataBits, StopBits stopBits, Parity parity)
        {
            if (null == m_SerialPort)
                m_SerialPort = new SerialPort();

            m_SerialPort.PortName = portName;
            m_SerialPort.BaudRate = baudRate;
            m_SerialPort.DataBits = dataBits;
            m_SerialPort.StopBits = stopBits;
            m_SerialPort.Parity = parity;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns>true:打开成功或已经打开;false:对象为空或打开报错</returns>
        public bool Open()
        {
            bool bRet = true;
            if (null == m_SerialPort)
                return false;
            if (m_SerialPort.IsOpen)
                return true;
            try
            {
                // if (m_ReadCount == 0)
                m_SerialPort.DataReceived += SerialPortDataReceived;
                // else
                //     m_SerialPort.DataReceived += SerialPortDataReceivedByConst;

                m_SerialPort.Open();
                bRet = true;
            }
            catch (System.UnauthorizedAccessException e1)
            {
                //串口被其他进程占用
                bRet = false;
            }
            catch (System.ArgumentOutOfRangeException e2)
            {
                bRet = false;
                throw new Exception("Open Serial Port Error", e2);
            }
            catch (System.ArgumentException e3)
            {
                bRet = false;
                throw new Exception("Open Serial Port Error", e3);
            }
            catch (System.IO.IOException e4)
            {
                bRet = false;
                throw new Exception("Open Serial Port Error", e4);
            }
            catch (System.InvalidOperationException e5)
            {
                //串口已经打开
                bRet = true;
            }
            catch (Exception e)
            {
                bRet = false;
                throw new Exception("Open Serial Port Error", e);
            }
            finally
            {
            }
            return bRet;
        }

        /// <summary>
        /// close serial port
        /// </summary>
        public void Close()
        {
            if (null != m_SerialPort)
            {
                m_SerialPort.DataReceived -= SerialPortDataReceived;
                if (m_SerialPort.IsOpen)
                {
                    try
                    {
                        m_SerialPort.DiscardInBuffer();
                        m_SerialPort.DiscardOutBuffer();
                        m_SerialPort.Close();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Close Serial Port Error", e);
                    }
                }
            }
        }

        public bool IsOpen()
        {
            return m_SerialPort.IsOpen;
        }

        public void SendData(byte[] data)
        {
            if (null != m_SerialPort && m_SerialPort.IsOpen)
            {
                try
                {
                    m_SerialPort.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    throw new Exception("Write SerialPort Error", e);
                }
            }
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int count = m_SerialPort.BytesToRead;
                byte[] buffer = new byte[count];
                m_SerialPort.Read(buffer, 0, count);
               
                //向上层发送，将串口号也带上 
                if (this.DataReceived != null)
                {
                    this.DataReceived(this, new DataTransEventArgs(buffer, ((SerialPort)sender).PortName));
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }
    }

    /// <summary>
    /// Event arguments containing event data.
    /// </summary>
    public class DataTransEventArgs : EventArgs
    {
        protected byte[] data;

        public byte[] EventData
        {
            get { return this.data; }
        }

        private string m_PortName = string.Empty;
        /// <summary>
        /// 返回当前端口号
        /// </summary>
        public string PortName
        {
            get { return m_PortName; }
            set { m_PortName = value; }
        }

        /// <summary>
        ///Copies the data starting at the specified index and paste them to the inner array
        /// </summary>
        /// <param name="result">Data raised in the event.</param>
        /// <param name="index">the index in the sourceArray at which copying begins.</param>
        /// <param name="length"> the number of elements to copy</param>
        public DataTransEventArgs(byte[] result, int index, int length)
        {
            data = new byte[length];
            Array.Copy(result, index, data, 0, length);
        }
        /// <summary>
        /// Copies the data to the inner array 
        /// </summary>
        /// <param name="result">Data raised in the event.</param>
        public DataTransEventArgs(byte[] result, string portName = "")
        {
            data = new byte[result.Length];
            Array.Copy(result, data, result.Length);
            m_PortName = portName;
        }

        /// <summary>
        /// Override of Object.ToString
        /// </summary>
        /// <returns>String with ConnectionEventArgs parameters</returns>
        public override string ToString()
        {
            return EventData.ToString();
        }
    }
}
