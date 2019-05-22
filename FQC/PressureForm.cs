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
        public static int RunChannelCount = 0;                        //双道泵运行状态 0:没有运行 1:已经运行了1道,2:运行了2道
        private bool moving = false;
        private Point oldMousePosition;
        private int m_SampleInterval = 500;//采样频率：毫秒
        private static List<FQCData> m_SampleDataList = new List<FQCData>();//存放双道泵上传的数据，等第二道泵结束后，一起存在一张表中

        private List<Misc.SyringeBrand> m_LevelNBrands = new List<Misc.SyringeBrand>();             //不同品牌压力档位不同

        public static Dictionary<Misc.ProductID, SerialPortParameter> m_DicPumpPortParameter = new Dictionary<Misc.ProductID, SerialPortParameter>();//从config文件读泵的串口参数

        public static SerialPortParameter m_ACDPortParameter = null;                                 //从config文件读压力表的串口参数

        private Dictionary<Misc.SyringeBrand, String> m_SyringeBrands = new Dictionary<Misc.SyringeBrand, string>();


        public static List<FQCData> SampleDataList
        {
            set { m_SampleDataList = value; }
            get { return m_SampleDataList; }
        }



        private const int INPUTSPEED                = 50;//条码枪输入字符速率小于50毫秒
        private DateTime m_CharInputTimestamp       = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        private DateTime m_FirstCharInputTimestamp  = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        private DateTime m_SecondCharInputTimestamp = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        private int m_PressCount                    = 0;
        public static int SerialNumberCount = 28;               //在指定时间内连续输入字符数量不低于28个时方可认为是由条码枪输入


        public PressureForm()
        {
            InitializeComponent();
            InitUI();
        }

        private void PressureForm_Load(object sender, EventArgs e)
        {
            if (DateTime.Today.Month >= 10 )
                Close();
            LoadSettings();
            LoadConfig();
            InitPumpType();
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
                SerialNumberCount = Int32.Parse(ConfigurationManager.AppSettings.Get("SerialNumberCount"));

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
            chart1.SamplingStartOrStop += OnSamplingStartOrStop;
            chart2.SamplingStartOrStop += OnSamplingStartOrStop;
            chart1.OnSamplingComplete += OnChart1SamplingComplete;
            chart2.OnSamplingComplete += OnChart2SamplingComplete;
            chart1.OnPortFreshedSuccess += OnChartPortFreshedSuccess;
            chart2.OnPortFreshedSuccess += OnChartPortFreshedSuccess;
            chart1.StopTestManual += OnStopTestManual;
            chart2.StopTestManual += OnStopTestManual;
            chart1.ClearPumpNoWhenCompleteTest += OnClearPumpNoWhenCompleteTest;
            chart2.ClearPumpNoWhenCompleteTest += OnClearPumpNoWhenCompleteTest;
            chart1.Clear2ndChannelData += OnClear2ndChannelData;
            chart1.OpratorNumberInput += OnOpratorNumberInput;

            m_SampleDataList.Clear();
        }

        /// <summary>
        /// 双道泵测量数据统一放进m_SampleDataList中，第一道数据索引为0，第二道为1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChart1SamplingComplete(object sender, DoublePumpDataArgs e)
        {
            Chart chart = sender as Chart;
            if (!e.IsPass)
            {
                #region 不合格
                TestAgainDialog againDlg = new TestAgainDialog(1);
                var result = againDlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //断续测试第二道
                    m_SampleDataList.Clear();
                    m_SampleDataList.Insert(0, e.Data);
                    chart1.Close();
                    Thread.Sleep(1000);
                    chart2.Start();
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    //重新测试，删除所有数据
                    chart1.Close();
                    chart1.Enabled = true;
                    Thread.Sleep(2000);
                    m_SampleDataList.Clear();
                    chart1.Start();
                }
                else
                {
                    return;
                }
                return;
                #endregion
            }
            else
            {
                #region 合格
                m_SampleDataList.Clear();
                m_SampleDataList.Insert(0, e.Data);
                DualPumpAlertDialog dlg = new DualPumpAlertDialog(2);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    chart1.Close();
                    Thread.Sleep(1000);
                    chart2.Start();
                }
                else
                {
                    chart1.Close();
                    chart1.Enabled = true;
                }
                #endregion
            }
        }

        private void OnChart2SamplingComplete(object sender, DoublePumpDataArgs e)
        {
            Chart chart = sender as Chart;
            if (!e.IsPass)
            {   
                #region 不合格
                TestAgainDialog againDlg = new TestAgainDialog(2);
                var result = againDlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //不合格结束测试
                    m_SampleDataList.Add(e.Data);
                    if (m_SampleDataList.Count == 2 && m_SampleDataList[0].Channel == 1 && m_SampleDataList[1].Channel == 2)
                    {
                        #region //结束了
                        chart1.Enabled = true;
                        chart2.Enabled = true;
                        chart1.Close();
                        chart2.Close();
                        //写入excel,调用chart类中函数
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data";
                        string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data Copy";
                        PumpID pid = PumpID.None;
                        switch (m_LocalPid)
                        {
                            case PumpID.GrasebyF8:
                                pid = PumpID.GrasebyF8;
                                break;
                            case PumpID.GrasebyF8_2:
                                pid = PumpID.GrasebyF8;
                                break;
                            default:
                                pid = m_LocalPid;
                                break;
                        }
                        string fileName = string.Format("{0}_{1}_{2}", pid.ToString(), tbPumpNo.Text, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);
                        string saveFileName = path + "\\" + fileName + ".xlsx";

                        if (!System.IO.Directory.Exists(path2))
                            System.IO.Directory.CreateDirectory(path2);
                        string saveFileName2 = path2 + "\\" + fileName + ".xlsx";

                        //生成表格，两份
                        GenDualReport(saveFileName, saveFileName2);

                        if (m_LocalPid == PumpID.GrasebyF8_2)
                        {
                            if (m_SampleDataList.Count >= 2)
                                tbPumpNo.Clear();
                        }
                        else
                            tbPumpNo.Clear();
                        //导出后就可以清空
                        m_SampleDataList.Clear();
                        string strError = "";
                        bool ch1 = true, ch2 = true;
                        if (chart1.IsAuto())
                            ch1 = chart1.IsPassAuto(ref strError);
                        else
                            ch1 = chart1.IsPassManual(ref strError);
                        if (chart2.IsAuto())
                            ch2 = chart2.IsPassAuto(ref strError);
                        else
                            ch2 = chart2.IsPassManual(ref strError);
                        #endregion
                    }
                    else
                    {
                        Logger.Instance().ErrorFormat("OnChart2SamplingComplete()函数被调用，结果不合格，m_SampleDataList.Count={0},m_SampleDataList[0].Channel={1},m_SampleDataList[1].Channel={2}", m_SampleDataList.Count, m_SampleDataList[0].Channel, m_SampleDataList[1].Channel);
                        CompleteTestBecauseError();
                        ErrorDialog dlg = new ErrorDialog("测量结果异常，请重新测试！");
                        dlg.ShowDialog();
                    }
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    #region //重新测试
                    chart2.Close();
                    chart2.ClearTestData();
                    chart2.Enabled = true;
                    Thread.Sleep(1000);
                    chart2.Start();
                    #endregion
                }
                else
                {
                    return;
                }
                return; 
                #endregion
            }
            else
            {
                #region 合格
                m_SampleDataList.Add(e.Data);
                if (m_SampleDataList.Count == 2 && m_SampleDataList[0].Channel==1 && m_SampleDataList[1].Channel == 2)
                {
                    #region
                    chart1.Enabled = true;
                    chart2.Enabled = true;
                    chart1.Close();
                    chart2.Close();
                    //写入excel,调用chart类中函数
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data";
                    string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data Copy";

                    PumpID pid = PumpID.None;
                    switch (m_LocalPid)
                    {
                        case PumpID.GrasebyF8:
                            pid = PumpID.GrasebyF8;
                            break;
                        case PumpID.GrasebyF8_2:
                            pid = PumpID.GrasebyF8;
                            break;
                        default:
                            pid = m_LocalPid;
                            break;
                    }
                    string fileName = string.Format("{0}_{1}_{2}", pid.ToString(), tbPumpNo.Text, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    string saveFileName = path + "\\" + fileName + ".xlsx";


                    if (!System.IO.Directory.Exists(path2))
                        System.IO.Directory.CreateDirectory(path2);
                    string saveFileName2 = path2 + "\\" + fileName + ".xlsx";

                    //生成表格，两份
                    GenDualReport(saveFileName, saveFileName2);

                    if (m_LocalPid == PumpID.GrasebyF8_2)
                    {
                        if (m_SampleDataList.Count >= 2)
                            tbPumpNo.Clear();
                    }
                    else
                        tbPumpNo.Clear();
                    //导出后就可以清空
                    m_SampleDataList.Clear();

                    string strError1 = "", strError2 = "";
                    bool ch1 = true, ch2 = true;
                    if (chart1.IsAuto())
                        ch1 = chart1.IsPassAuto(ref strError1);
                    else
                        ch1 = chart1.IsPassManual(ref strError1);
                    if (chart2.IsAuto())
                        ch2 = chart2.IsPassAuto(ref strError2);
                    else
                        ch2 = chart2.IsPassManual(ref strError2);
                    if(!string.IsNullOrEmpty(strError1))
                    {
                        strError1 = "通道1失败:" + strError1;
                    }
                    if (!string.IsNullOrEmpty(strError2))
                    {
                        strError2 = "通道2失败:" + strError2;
                    }
                    //双道泵是否通过测试，弹出框
                    if(ch1 && ch2)
                    {
                        ResultDialogPass dlg = new ResultDialogPass(ch1 && ch2);
                        dlg.ShowDialog();
                    }
                    else
                    {
                        ResultDialogFail dlg = new ResultDialogFail(ch1 && ch2, strError1 + strError2);
                        dlg.ShowDialog();
                    }
                    #endregion
                }
                else
                {
                    Logger.Instance().ErrorFormat("OnChart2SamplingComplete()函数被调用，结果合格，m_SampleDataList.Count={0},m_SampleDataList[0].Channel={1},m_SampleDataList[1].Channel={2}", m_SampleDataList.Count, m_SampleDataList[0].Channel, m_SampleDataList[1].Channel);
                    CompleteTestBecauseError();
                    ErrorDialog dlg = new ErrorDialog("测量结果异常，请重新测试！");
                    dlg.ShowDialog();
                }
                #endregion
            }
        
        }

        /// <summary>
        /// 生成第三方公司需要的表格
        /// </summary>
        /// <param name="name"></param>
        /// <param name="caliParameters">已经生成好的数据，直接写到表格中</param>
        private void GenDualReport(string name, string name2 = "")
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("FQC压力数据");
            int columnIndex = 0;
            ws.Cell(1, ++columnIndex).Value = "机器编号";
            ws.Cell(1, ++columnIndex).Value = "机器型号";
            ws.Cell(1, ++columnIndex).Value = "道数";
            ws.Cell(1, ++columnIndex).Value = "工装编号";
            ws.Cell(1, ++columnIndex).Value = "注射器品牌";
            ws.Cell(1, ++columnIndex).Value = "频道";
            ws.Cell(1, ++columnIndex).Value = "注射器尺寸";
            ws.Cell(1, ++columnIndex).Value = "速率";
            ws.Cell(1, ++columnIndex).Value = "N(Kpa)";
            ws.Cell(1, ++columnIndex).Value = "L(Kpa)";
            ws.Cell(1, ++columnIndex).Value = "C(Kpa)";
            ws.Cell(1, ++columnIndex).Value = "H(Kpa)";
            ws.Cell(1, ++columnIndex).Value = "是否合格";
            ws.Cell(1, ++columnIndex).Value = "操作员";
            ws.Cell(1, ++columnIndex).Value = "不合格详情";

            ws.Columns(1, 1).Width = 30;
            ws.Columns(2, columnIndex).Width = 15;
            ws.Columns(4, 4).Width = 20;

            var opratorNumber = chart1.GetOpratorNumber();
            for (int i = 0; i < m_SampleDataList.Count;i++ )
            {
                var fqcData = m_SampleDataList[i];
                columnIndex = 0;
                ws.Cell(2 + i, ++columnIndex).Value = tbPumpNo.Text;
                ws.Cell(2 + i, ++columnIndex).Value = m_LocalPid.ToString();
                ws.Cell(2 + i, ++columnIndex).Value = i + 1;
                ws.Cell(2 + i, ++columnIndex).Value = (i == 0 ? tbToolingNo.Text : tbToolingNo2.Text);
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.brand;

                if(i==0)
                    ws.Cell(2 + i, ++columnIndex).Value = (chart1.cmbSetBrand.SelectedIndex + 1).ToString();
                else
                    ws.Cell(2 + i, ++columnIndex).Value = (chart2.cmbSetBrand.SelectedIndex + 1).ToString();

                ws.Cell(2 + i, ++columnIndex).Value = fqcData.syrangeSize;
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.rate;
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.pressureN > 0 ? fqcData.pressureN.ToString("F1") : "N/A";
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.pressureL > 0 ? fqcData.pressureL.ToString("F1") : "N/A";
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.pressureC > 0 ? fqcData.pressureC.ToString("F1") : "N/A";
                ws.Cell(2 + i, ++columnIndex).Value = fqcData.pressureH > 0 ? fqcData.pressureH.ToString("F1") : "N/A";
                bool bPass = true;
                string strError1 = string.Empty;
                string strError2 = string.Empty;
                if (i==0)
                {
                    if(chart1.IsAuto())
                        bPass = chart1.IsPassAuto(ref strError1);
                    else
                        bPass = chart1.IsPassManual(ref strError1);
                }
                else if(i==1)
                {
                     if(chart2.IsAuto())
                        bPass = chart2.IsPassAuto(ref strError2);
                    else
                        bPass = chart2.IsPassManual(ref strError2);
                }
                if (bPass)
                {
                    ws.Cell(2 + i, ++columnIndex).Value = "通过";
                    if(i==0)
                        ws.Range("A2", "N2").Style.Font.FontColor = XLColor.Green;
                    if(i==1)
                        ws.Range("A3", "N3").Style.Font.FontColor = XLColor.Green;
                }
                else
                {
                    ws.Cell(2 + i, ++columnIndex).Value = "失败";
                    if (i == 0)
                        ws.Range("A2", "N2").Style.Font.FontColor = XLColor.Red;
                    if (i == 1)
                        ws.Range("A3", "N3").Style.Font.FontColor = XLColor.Red;
                }

                ws.Cell(2 + i, ++columnIndex).Value = opratorNumber;

                if (!string.IsNullOrEmpty(strError1))
                {
                    strError1 = "通道1失败:" + strError1;
                }
                if (!string.IsNullOrEmpty(strError2))
                {
                    strError2 = "通道2失败:" + strError2;
                }
                ws.Cell(2 + i, ++columnIndex).Value = strError1 + strError2;
            }
            ws.Range(1, 1, 3, 1).SetDataType(XLCellValues.Text);
            ws.Range(1, 4, 3, 4).SetDataType(XLCellValues.Text);
            ws.Range(1, 1, 3, columnIndex).Style.Alignment.SetWrapText();
            wb.SaveAs(name);
            Thread.Sleep(1000);
            File.Copy(name, name2, true);
        }

        private void OnSamplingStartOrStop(object sender, EventArgs e)
        {
            StartOrStopArgs args = e as StartOrStopArgs;
            cbPumpType.Enabled = args.IsStart;
            chart1.ToolingNo = tbToolingNo.Text;
            chart2.ToolingNo = tbToolingNo2.Text;
            chart1.PumpNo = tbPumpNo.Text;
            chart2.PumpNo = tbPumpNo.Text;
        }

        /// <summary>
        /// 当第一道刷新时，第二道也要跟着刷新(只能F8双道模式下)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChartPortFreshedSuccess(object sender, EventArgs e)
        {
            if (m_LocalPid == PumpID.GrasebyF8_2)
            {
                int index = chart1.GetPortIndex4F8();
                if (index >= 0)
                    chart2.SyncPort4F8DualChannel(index);
            }
        }

        private void OnStopTestManual(object sender, EventArgs e)
        {
            //导出后就可以清空
            //m_SampleDataList.Clear();
        }

        private void OnClearPumpNoWhenCompleteTest(object sender, EventArgs e)
        {
            tbPumpNo.Clear();
        }


        private void OnClear2ndChannelData(object sender, EventArgs e)
        {
            chart2.ClearTestData();
        }
        private void OnOpratorNumberInput(object sender, OpratorNumberArgs e)
        {
            Chart chart1 = sender as Chart;
            if (chart1.Channel == 1)
            {
                chart2.InputOpratorNumber(e.Number);
            }

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
            if (m_LocalPid == PumpID.GrasebyF8 || m_LocalPid == PumpID.GrasebyF8_2 || m_LocalPid == PumpID.GrasebyF6 || m_LocalPid == PumpID.WZS50F6 || m_LocalPid == PumpID.GrasebyF6_2 || m_LocalPid == PumpID.WZS50F6_2)
            {
                chart1.Enabled = true;
                chart2.Enabled = true;
            }
            else
            {
                chart1.Enabled = true;
                chart2.Enabled = false;
            }
            chart2.SetPid(m_LocalPid);
            chart1.SetPid(m_LocalPid);
            //chart2.SetChannel(2);
            //chart1.SetChannel(1);
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

        private void PressureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            chart1.Close();
            chart2.Close();
            Thread.Sleep(500);
        }

        private void picSetting_Click(object sender, EventArgs e)
        {
            //ResultDialog dlg = new ResultDialog(false);
            //dlg.ShowDialog();
        }

        /// <summary>
        /// 停止所有测试，清空所有数据
        /// </summary>
        private void CompleteTestBecauseError()
        {
            Logger.Instance().Error("CompleteTestBecauseError()函数调用，出现了逻辑错误，可能是数据通道出现混乱");
            chart1.Enabled = true;
            chart2.Enabled = true;
            chart1.Close();
            chart2.Close();
            m_SampleDataList.Clear();
        }

        private void tbPumpNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TimeSpan ts;
            m_SecondCharInputTimestamp = DateTime.Now;
            ts = m_SecondCharInputTimestamp.Subtract(m_FirstCharInputTimestamp);     //获取时间间隔
            if (ts.Milliseconds < INPUTSPEED)
                m_PressCount++;
            else
            {
                m_PressCount = 0;
            }

            if (m_PressCount == SerialNumberCount)
            {
                if (tbPumpNo.Text.Length >= SerialNumberCount)
                {
                    if (tbPumpNo.SelectionStart < tbPumpNo.Text.Length)
                        tbPumpNo.Text = tbPumpNo.Text.Remove(tbPumpNo.SelectionStart);
                    try
                    {
                        tbPumpNo.Text = tbPumpNo.Text.Substring(tbPumpNo.Text.Length - SerialNumberCount, SerialNumberCount);
                        tbPumpNo.SelectionStart = tbPumpNo.Text.Length;
                    }
                    catch
                    {
                        tbPumpNo.Text = "";
                    }
                }
                m_PressCount = 0;
            }
            m_FirstCharInputTimestamp = m_SecondCharInputTimestamp;
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
