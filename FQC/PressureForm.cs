using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using ClosedXML.Excel;
using System.Drawing.Drawing2D;
using ApplicationClient;
using Misc = ComunicationProtocol.Misc;
using PCTool.CommonProcess;
using PCTool.CommonProcess.UserControls;
using SerialDevice;

namespace FQC
{
    public partial class PressureForm : Form
    {
        private PumpID m_LocalPid = PumpID.GrasebyF8;                 //默认显示的是F8,这个是本地自定义F8
        private ProductModel m_ProductModel = ProductModel.GrasebyF8;
        private Misc.ProductID m_ProductID = Misc.ProductID.GrasebyF8;//默认显示的是F8,这个是真正的用于通信的F8

        private bool moving = false;
        private Point oldMousePosition;
        private int m_SampleInterval = 500;//采样频率：毫秒
        private List<List<SampleData>> m_SampleDataList = new List<List<SampleData>>();//存放双道泵上传的数据，等第二道泵结束后，一起存在一张表中

        private List<Misc.SyringeBrand> m_LevelNBrands = new List<Misc.SyringeBrand>();             //不同品牌压力档位不同

        public static Dictionary<Misc.ProductID, SerialPortParameter> m_DicPumpPortParameter = new Dictionary<Misc.ProductID, SerialPortParameter>();//从config文件读泵的串口参数

        public static SerialPortParameter m_ACDPortParameter = null;                                 //从config文件读压力表的串口参数

        private Dictionary<Misc.SyringeBrand, String> m_SyringeBrands = new Dictionary<Misc.SyringeBrand, string>();



        public PressureForm()
        {
            InitializeComponent();
            InitUI();
        }

        private void PressureForm_Load(object sender, EventArgs e)
        {
            InitPumpType();
            LoadSettings();
            LoadConfig();
        }

        /// <summary>
        /// 加载配置文件中的参数
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string strInterval = ConfigurationManager.AppSettings.Get("SampleInterval");
                if (!int.TryParse(strInterval, out m_SampleInterval))
                    m_SampleInterval = 500;
                string strTool1 = ConfigurationManager.AppSettings.Get("Tool1");
                string strTool2 = ConfigurationManager.AppSettings.Get("Tool2");
                tbToolingNo.Text = strTool1;
                tbToolingNo2.Text = strTool2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("FQC.config文件参数配置错误，请先检查该文件后再重新启动程序!" + ex.Message);
            }

            string brandNameOcclusion = string.Empty;
            m_LevelNBrands.Clear();

            #region 加载压力档位，不同品牌档位不同，主要是N档
            try
            {
                //if key not exist
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("SyringeBrandLevel");
                int index = Array.FindIndex<string>(config.AllKeys, (x) => { return x == this.m_ProductID.ToString(); });
                if (index >= 0)
                {
                    brandNameOcclusion = config[index];
                    string[] arrOcclusion = brandNameOcclusion.Split(',');
                    foreach (string s in arrOcclusion)
                    {
                        if (Enum.IsDefined(typeof(Misc.SyringeBrand), s))
                        {
                            m_LevelNBrands.Add((Misc.SyringeBrand)Enum.Parse(typeof(Misc.SyringeBrand), s, false));
                        }
                    }
                }
            }
            catch
            {
                brandNameOcclusion = string.Empty;
            }
            #endregion

            #region 加载C8串口参数，泵端代表了115200系列
            try
            {
                //if key not exist
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("GrasebyC8SerialSet");
                int baudRate = 0;
                if (!Int32.TryParse(config["BaudRate"], out baudRate))
                {
                    MessageBox.Show("GrasebyC8波特率参数非法，请检查config文件");
                    return;
                }
                int dataBits = 0;
                if (!Int32.TryParse(config["DataBits"], out dataBits))
                {
                    MessageBox.Show("GrasebyC8数据位参数非法，请检查config文件");
                    return;
                }
                StopBits stopBits = StopBits.None;
                if (!Enum.IsDefined(typeof(StopBits), config["StopBits"]))
                {
                    MessageBox.Show("GrasebyC8停止位参数非法，请检查config文件");
                    return;
                }
                else
                {
                    stopBits = (StopBits)Enum.Parse(typeof(StopBits), config["StopBits"]);
                }
                Parity parity = Parity.None;
                if (!Enum.IsDefined(typeof(Parity), config["Parity"]))
                {
                    MessageBox.Show("GrasebyC8奇偶校验参数非法，请检查config文件");
                    return;
                }
                else
                {
                    parity = (Parity)Enum.Parse(typeof(Parity), config["Parity"]);
                }
                m_DicPumpPortParameter.Add(Misc.ProductID.GrasebyC8, new SerialPortParameter(baudRate, dataBits, stopBits, parity));
            }
            catch
            {
            }
            #endregion 

            #region 加载F8串口参数，泵端代表了115200系列
            try
            {
                //if key not exist
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("GrasebyF8SerialSet");
                int baudRate = 0;
                if (!Int32.TryParse(config["BaudRate"], out baudRate))
                {
                    MessageBox.Show("GrasebyF8波特率参数非法，请检查config文件");
                    return;
                }
                int dataBits = 0;
                if (!Int32.TryParse(config["DataBits"], out dataBits))
                {
                    MessageBox.Show("GrasebyF8数据位参数非法，请检查config文件");
                    return;
                }
                StopBits stopBits = StopBits.None;
                if (!Enum.IsDefined(typeof(StopBits), config["StopBits"]))
                {
                    MessageBox.Show("GrasebyF8停止位参数非法，请检查config文件");
                    return;
                }
                else
                {
                    stopBits = (StopBits)Enum.Parse(typeof(StopBits), config["StopBits"]);
                }
                Parity parity = Parity.None;
                if (!Enum.IsDefined(typeof(Parity), config["Parity"]))
                {
                    MessageBox.Show("GrasebyF8奇偶校验参数非法，请检查config文件");
                    return;
                }
                else
                {
                    parity = (Parity)Enum.Parse(typeof(Parity), config["Parity"]);
                }
                m_DicPumpPortParameter.Add(Misc.ProductID.GrasebyF8, new SerialPortParameter(baudRate, dataBits, stopBits, parity));
            }
            catch
            {
            }
            #endregion 

            #region 加载C6串口参数，泵端, C6代表了9600系列
            try
            {
                //if key not exist
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("GrasebyC6SerialSet");
                int baudRate = 0;
                if (!Int32.TryParse(config["BaudRate"], out baudRate))
                {
                    MessageBox.Show("GrasebyC6波特率参数非法，请检查config文件");
                    return;
                }
                int dataBits = 0;
                if (!Int32.TryParse(config["DataBits"], out dataBits))
                {
                    MessageBox.Show("GrasebyC6数据位参数非法，请检查config文件");
                    return;
                }
                StopBits stopBits = StopBits.None;
                if (!Enum.IsDefined(typeof(StopBits), config["StopBits"]))
                {
                    MessageBox.Show("GrasebyC6停止位参数非法，请检查config文件");
                    return;
                }
                else
                {
                    stopBits = (StopBits)Enum.Parse(typeof(StopBits), config["StopBits"]);
                }
                Parity parity = Parity.None;
                if (!Enum.IsDefined(typeof(Parity), config["Parity"]))
                {
                    MessageBox.Show("GrasebyC6奇偶校验参数非法，请检查config文件");
                    return;
                }
                else
                {
                    parity = (Parity)Enum.Parse(typeof(Parity), config["Parity"]);
                }
                m_DicPumpPortParameter.Add(Misc.ProductID.GrasebyC6, new SerialPortParameter(baudRate, dataBits, stopBits, parity));
            }
            catch
            {
            }
            #endregion

            #region 加载串口参数，压力表
            try
            {
                //if key not exist
                NameValueCollection config = (NameValueCollection)ConfigurationManager.GetSection("ACDSerialSet");
                int baudRate = 0;
                if (!Int32.TryParse(config["BaudRate"], out baudRate))
                {
                    MessageBox.Show("压力表波特率参数非法，请检查config文件");
                    return;
                }
                int dataBits = 0;
                if (!Int32.TryParse(config["DataBits"], out dataBits))
                {
                    MessageBox.Show("压力表数据位参数非法，请检查config文件");
                    return;
                }
                StopBits stopBits = StopBits.None;
                if (!Enum.IsDefined(typeof(StopBits), config["StopBits"]))
                {
                    MessageBox.Show("压力表停止位参数非法，请检查config文件");
                    return;
                }
                else
                {
                    stopBits = (StopBits)Enum.Parse(typeof(StopBits), config["StopBits"]);
                }
                Parity parity = Parity.None;
                if (!Enum.IsDefined(typeof(Parity), config["Parity"]))
                {
                    MessageBox.Show("压力表奇偶校验参数非法，请检查config文件");
                    return;
                }
                else
                {
                    parity = (Parity)Enum.Parse(typeof(Parity), config["Parity"]);
                }
                m_ACDPortParameter = new SerialPortParameter(baudRate, dataBits, stopBits, parity);
            }
            catch
            {
            }
            #endregion

            chart1.SetConfig(m_LevelNBrands);
            chart2.SetConfig(m_LevelNBrands);

        }

        private void LoadSettings()
        {
            string currentPath = Assembly.GetExecutingAssembly().Location;
            currentPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'));  //删除文件名
            string iniPath = currentPath + "\\pressure.ini";
            IniReader reader = new IniReader(iniPath);
            reader.ReadSettings();
        }

        private void SaveLastToolingNo()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["Tool1"].Value = tbToolingNo.Text;
            cfa.AppSettings.Settings["Tool2"].Value = tbToolingNo2.Text;
            cfa.Save();
        }

        private void InitPumpType()
        {
            cbPumpType.Items.Clear();
            cbPumpType.Items.AddRange(ProductIDConvertor.GetAllPumpIDString().ToArray());
            cbPumpType.SelectedIndex = 0;
            m_LocalPid = ProductIDConvertor.String2PumpID(cbPumpType.Items[cbPumpType.SelectedIndex].ToString());
            SyncProductID();
        }

        /// <summary>
        /// 当本地泵ID改变时，一定要同步到通信ProductID, 和 ProductModel
        /// </summary>
        private void SyncProductID()
        {
            #region 泵型号选择
            switch (m_LocalPid)
            {
                case PumpID.GrasebyF8:
                    m_ProductID = Misc.ProductID.GrasebyF8;
                    m_ProductModel = ProductModel.GrasebyF8;
                    break;
                case PumpID.GrasebyF8_2:
                    m_ProductID = Misc.ProductID.GrasebyF8;
                    m_ProductModel = ProductModel.GrasebyF8;
                    break;
                case PumpID.GrasebyC8:
                    m_ProductID = Misc.ProductID.GrasebyC8;
                    m_ProductModel = ProductModel.GrasebyC8;
                    break;
                case PumpID.GrasebyC6:
                    m_ProductID = Misc.ProductID.GrasebyC6;
                    m_ProductModel = ProductModel.GrasebyC6;
                    break;
                case PumpID.WZ50C6:
                    m_ProductID = Misc.ProductID.GrasebyC6;
                    m_ProductModel = ProductModel.GrasebyC6;
                    break;
                case PumpID.GrasebyC6T:
                case PumpID.WZ50C6T:
                    m_ProductID = Misc.ProductID.GrasebyC6T;
                    m_ProductModel = ProductModel.GrasebyC6T;
                    break;
                case PumpID.Graseby2000:
                    m_ProductID = Misc.ProductID.Graseby2000;
                    m_ProductModel = ProductModel.Graseby2000;
                    break;
                case PumpID.Graseby2100:
                    m_ProductID = Misc.ProductID.Graseby2100;
                    m_ProductModel = ProductModel.Graseby2100;
                    break;
                case PumpID.WZS50F6:
                    m_ProductID = Misc.ProductID.GrasebyF6;
                    m_ProductModel = ProductModel.GrasebyF6;
                    break;
                case PumpID.GrasebyF6:
                    m_ProductID = Misc.ProductID.GrasebyF6;
                    m_ProductModel = ProductModel.GrasebyF6;
                    break;
                case PumpID.GrasebyF6_2:
                    m_ProductID = Misc.ProductID.GrasebyF6;
                    m_ProductModel = ProductModel.GrasebyF6;
                    break;
                case PumpID.WZS50F6_2:
                    m_ProductID = Misc.ProductID.GrasebyF6;
                    m_ProductModel = ProductModel.GrasebyF6;
                    break;
                default:
                    m_ProductID = Misc.ProductID.None;
                    m_ProductModel = ProductModel.None;
                    break;
            }
            #endregion
        }


        private void InitUI()
        {
            lbTitle.ForeColor = Color.FromArgb(3, 116, 214);
            tlpParameter.BackColor = Color.FromArgb(19, 113, 185);
            cbPumpType.BackColor = Color.FromArgb(19, 113, 185);
            tbPumpNo.BackColor = Color.FromArgb(19, 113, 185);
            tbToolingNo.BackColor = Color.FromArgb(19, 113, 185);
            tbToolingNo2.BackColor = Color.FromArgb(19, 113, 185);
            chart1.Channel = 1;
            chart2.Channel = 2;
            chart2.Enabled = false;
            chart2.SamplingStartOrStop += OnSamplingStartOrStop;
            chart1.SamplingStartOrStop += OnSamplingStartOrStop;
            chart2.OnSamplingComplete += OnChartSamplingComplete;
            chart1.OnSamplingComplete += OnChartSamplingComplete;
            m_SampleDataList.Clear();
        }

        /// <summary>
        /// 双道泵测量数据统一放进m_SampleDataList中，第一道数据索引为0，第二道为1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChartSamplingComplete(object sender, DoublePumpDataArgs e)
        {
            Chart chart = sender as Chart;
            if (e.SampleDataList != null)
            {
                if (chart.Name == "chart1")
                    m_SampleDataList.Insert(0, e.SampleDataList);
                else
                    m_SampleDataList.Add(e.SampleDataList);
            }
            if(m_SampleDataList.Count>=2)
            {
                //写入excel,调用chart类中函数
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(PressureForm)).Location) + "\\数据导出";
                PumpID pid = PumpID.None;
                switch (m_LocalPid)
                {
                    case PumpID.GrasebyF6_2:
                        pid = PumpID.GrasebyF6;
                        break;
                    case PumpID.WZS50F6_2:
                        pid = PumpID.WZS50F6;
                        break;
                    default:
                        pid = m_LocalPid;
                        break;
                }
                string fileName = string.Format("{0}_{1}_{2}", pid.ToString(), tbPumpNo.Text, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                string saveFileName = path + "\\" + fileName + ".xlsx";
            }
        }

        private void OnSamplingStartOrStop(object sender, EventArgs e)
        {
            StartOrStopArgs args = e as StartOrStopArgs;
            cbPumpType.Enabled = args.IsStart;
        }

        private void tlpTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return;
            }
            oldMousePosition = e.Location;
            moving = true; 
        }

        private void tlpTitle_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }

        private void tlpTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && moving)
            {
                Point newPosition = new Point(e.Location.X - oldMousePosition.X, e.Location.Y - oldMousePosition.Y);
                this.Location += new Size(newPosition);
            }
        }

        private void cbPumpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_LocalPid = ProductIDConvertor.String2PumpID(cbPumpType.Items[cbPumpType.SelectedIndex].ToString());
            SyncProductID();

#if DEBUG
            chart2.Enabled = true;

#else
            if (m_LocalPid == PumpID.GrasebyF8 || m_LocalPid == PumpID.GrasebyF6 || m_LocalPid == PumpID.WZS50F6 || m_LocalPid == PumpID.GrasebyF6_2 || m_LocalPid == PumpID.WZS50F6_2)
            {
                chart2.Enabled = true;
            }
            else
            {
                chart2.Enabled = false;
            }
#endif
            chart2.SetPid(m_LocalPid);
            chart1.SetPid(m_LocalPid);
            chart2.SetChannel(1);
            chart1.SetChannel(2);
            SyringBrandProcess.InitializeBrands(m_SyringeBrands, m_ProductModel, "zh");
            SyringBrandProcess.GetBrandNames(m_SyringeBrands, m_ProductModel, "zh");
            chart2.InitBrandList(m_SyringeBrands);
            chart1.InitBrandList(m_SyringeBrands);
        }

        private void picCloseWindow_Click(object sender, EventArgs e)
        {
            SaveLastToolingNo();
            this.Close();
        }
       
    }//end class

    public class SampleData
    {
        public DateTime m_SampleTime = DateTime.Now;
        public float m_PressureValue;

        public SampleData()
        {
        }

        public SampleData(DateTime sampleTime, float pressureVale)
        {
            m_SampleTime = sampleTime;
            m_PressureValue = pressureVale;
        }

        public void Copy(SampleData other)
        {
            this.m_SampleTime = other.m_SampleTime;
            this.m_PressureValue = other.m_PressureValue;
        }
    }
}
