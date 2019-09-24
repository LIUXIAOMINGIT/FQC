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
using PCTool.CommonProcess;
using ApplicationClient;
using Misc = ComunicationProtocol.Misc;
using PCTool.CommonProcess.UserControls;
using SerialDevice;


namespace FQC
{
    public partial class Chart : UserControl
    {
        private const string DoublePumpLevelTips = "{0}道{1}档压力{2}Kpa,合格范围{3}~{4}Kpa;";
        private const string SinglePumpLevelTips = "{0}档压力{1}Kpa,合格范围{2}~{3}Kpa;";
        private const double MAXKPALIMIT                                      = 150;
        private const string VOL                                              = "Kpa";
        private const int LEFTBORDEROFFSET                                    = 30;
        private const int RIGHTBORDEROFFSET                                   = 10;
        private const int BOTTOMBORDEROFFSET                                  = 30;                                        //X坐标与下边距，一般是绘图区域的一半高度
        private const int TOPBOTTOMFFSET                                      = 5;                                         //坐标上下边距
        private const int CIRCLEDIAMETER                                      = 5;                                         //曲线图上的圆点直径0
        private const int TRYCOUNTSAMPLINGTIMEOUT                             = 5;                                         //采样超时次数为5.超时5次就停止 
        private Graphics                              m_gh                    = null;
        private System.Drawing.Rectangle              m_Rect;
        private Pen                                   m_WaveLinePen           = new Pen(Color.FromArgb(128, 0, 128));
        private SolidBrush                            m_WaveLineBrush         = new SolidBrush(Color.FromArgb(128, 0, 128));
        private float                                 m_XCoordinateMaxValue   = 120;                                       //X轴最大长度：120秒
        private int                                   m_YCoordinateMaxValue   = 150;                                       //y轴最大Kpa
        private int                                   m_XSectionCount         = 20;
        private int                                   m_YSectionCount         = 15;
        private float                                 m_CoordinateIntervalX   = 0;                                         //X轴上的区间实际长度，单位为像素
        private float                                 m_CoordinateIntervalY   = 0;                                         //Y轴上的区间实际长度，单位为像素
        private float                                 m_ValueInervalX         = 0;                                         //X轴上的坐标值，根据实际放大倍数和量程决定
        private float                                 m_ValueInervalY         = 0;
        private List<SampleData>                      m_Ch1SampleDataList     = new List<SampleData>();
        protected GlobalResponse                      m_ConnResponse          = null;
        private SerialPressureGauge                   m_GaugeTool             = null;
        private SerialPressureGauge                   m_DetectPTool           = null;
        private Graseby9600                           m_GrasebyDevice         = new Graseby9600();                         //只用于串口刷新
        private DeviceBase                            _GrasebyDevice4FindPort = new DeviceBase();                          //只用于泵串口刷新
        private PumpID                                m_LocalPid              = PumpID.GrasebyF8;                          //默认显示的是
        public ProductModel                           m_ProductModel          = ProductModel.GrasebyF8;
        private System.Timers.Timer                   m_Ch1Timer              = new System.Timers.Timer();
        private System.Timers.Timer                   m_GaugeTimer            = new System.Timers.Timer();
        private int                                   m_SampleInterval        = 600;                                       //采样频率：毫秒
        private int                                   m_GaugeSampleInterval   = 350;                                       //压力表采样频率：350毫秒
        private int                                   m_Channel               = 1;                                         //1号通道，默认值
        private string                                m_PumpNo                = string.Empty;                              //产品序号
        private string                                m_Operator               = string.Empty;                              //操作员
        private Misc.AlarmInfo                        m_AlarmInfo             = new Misc.AlarmInfo();
        private int                                   m_TryCount              = 0;                                         //当命令没有响应超过3次，就完全停止测试
        private Dictionary<Misc.SyringeBrand, String> m_SyringeBrands         = new Dictionary<Misc.SyringeBrand, string>();
        private List<Misc.OcclusionLevel>             m_OcclusionLevelOfBrand = new List<Misc.OcclusionLevel>();           //不同品牌有不同的压力等级，默认为三级，但有的会有4级
        private Misc.SyringeBrand                     m_CurrentBrand          = Misc.SyringeBrand.None;
        private List<Misc.SyringeBrand>               m_LevelNBrands          = new List<Misc.SyringeBrand>();             //不同品牌压力档位不同
        private Misc.OcclusionLevel                   m_CurrentLevel          = Misc.OcclusionLevel.None;                  //当前测试的压力
        //private Misc.OcclusionLevel                   m_PreLevel              = Misc.OcclusionLevel.None;                  //上一步测试的压力
        private Queue<Misc.ApplicationRequestCommand> m_RequestCommands       = new Queue<Misc.ApplicationRequestCommand>();
        protected DateTime                            m_StopTime              = DateTime.Now;                              //停止时间
        protected DateTime                            m_StartTime             = DateTime.Now;                              //开始时间
        protected AutoResetEvent                      m_FreshPumpPortEvent    = new AutoResetEvent(false);                 //用于泵串口刷新
        protected AutoResetEvent                      m_StopPumpEvent         = new AutoResetEvent(false);                 //用于泵停止事件
        protected AutoResetEvent                      m_StopPumpPumpStatusEvent         = new AutoResetEvent(false);       //用于泵停止事件，泵停止时，查询状态，是否真正停止
        protected bool                                m_bTestOverFlag         = false;                                     //是否是调用了StopTest函数
        protected bool                                m_bMaxKpaFlag           = false;                                     //是否是超过了200Kpa并停止泵
        private FQCData mFQCData                                              = new FQCData();
        private int                                   m_LevelTryCount         = 0;                                         //当压力档命令没有响应超过2次，就完全停止测试

        private int m_CuurentPatterIndex = 0;

        private DateTime m_OcclusionTimeStamp = DateTime.Now;//换档成功并且泵停止后立即读报警信息，可能会有一条阻塞报警，导致直接从L档升到H档,所以过滤前10秒的报警信息

        private bool m_bStartTimer = true;
        private bool m_bStartPumpStatusThread = true;


        #region 委托+事件
        public delegate void DelegateSetWeightValue(float weight, bool isDetect);
        public delegate void DelegateSetPValue(float p);
        public delegate void DelegateEnableContols(bool bEnabled);
        public delegate void DelegateAlertTestResult();
        public delegate void DelegateContinueStopTest();
        public delegate void DelegateInputOpratorNumber(string number);
        public delegate void DelegatePauseTest();
        public delegate void DelegateFuctionNoParamater();

        /// <summary>
        /// 当启动或停止时通知主界面
        /// </summary>
        public event EventHandler<EventArgs> SamplingStartOrStop;
        public event EventHandler<EventArgs> ClearPumpNoWhenCompleteTest;
        public event EventHandler<EventArgs> StopTestManual;//人工干预停止时，所有数据都清零
        /// <summary>
        /// 当双道泵，测量结束后通知主界面，把数据传入
        /// </summary>
        public event EventHandler<DoublePumpDataArgs> OnSamplingComplete;
        public event EventHandler<EventArgs> OnPortFreshedSuccess;  //F8双通道模式下，第一道泵成功刷新后，第二道泵串口一起刷新
        public event EventHandler<EventArgs> Clear2ndChannelData;  //F8双通道模式下，第一道泵成功启动，清空第二道数据
        public event EventHandler<OpratorNumberArgs> OpratorNumberInput;

        #endregion

        #region 属性
        /// <summary>
        /// 采样间隔
        /// </summary>
        public int SampleInterval
        {
            get { return m_SampleInterval; }
            set { m_SampleInterval = value; }
        }

        /// <summary>
        /// 设置通道号1 or 2
        /// </summary>
        public int Channel
        {
            get { return m_Channel; }
            set
            {
                m_Channel = value;

            }
        }

        /// <summary>
        /// 产品序号
        /// </summary>
        public string PumpNo
        {
            get { return m_PumpNo; }
            set { m_PumpNo = value; }
        }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        {
            get { return m_Operator; }
            set { m_Operator = value; }
        }

        /// <summary>
        /// 是否在运行中,当这个控件锁住了，表示在运行
        /// </summary>
        public bool IsRunning
        {
            get { return !tbRate.Enabled; }
        }

        /// <summary>
        /// 工装编号
        /// </summary>
        public string ToolingNo
        {
            get { return tbToolingNo.Text; }
            set { tbToolingNo.Text = value; }
        }
        #endregion

        #region 构造函数
        public Chart()
        {
            InitializeComponent();
            picGaugePortStatus.Tag = false;
            if (cmbSetBrand.Items.Count > 4)
            {
                switch (m_ProductModel)
                {
                    case ProductModel.GrasebyC8:
                    case ProductModel.GrasebyF8:
                        cmbSetBrand.SelectedIndex = 3;
                        break;
                    case ProductModel.GrasebyC6:
                    case ProductModel.GrasebyF6:
                    case ProductModel.Graseby1200:
                    case ProductModel.GrasebyC6T:
                    case ProductModel.Graseby2000:
                    case ProductModel.Graseby2100:
                    case ProductModel.WZ50C6:
                    case ProductModel.WZS50F6:
                    case ProductModel.WZ50C6T:
                        cmbSetBrand.SelectedIndex = 2;
                        break;
                    default:
                        cmbSetBrand.SelectedIndex = 3;
                        break;
                }
            }
            cmbPattern.SelectedIndex = 0;
            m_Channel = 1;
            m_gh = WavelinePanel.CreateGraphics();
            m_Rect = WavelinePanel.ClientRectangle;
        }

        public Chart(int channel = 1)
        {
            InitializeComponent();
            picGaugePortStatus.Tag = false;
            if (cmbSetBrand.Items.Count > 4)
                cmbSetBrand.SelectedIndex = 3;
            cmbPattern.SelectedIndex = 0;
            m_Channel = channel;
            m_gh = WavelinePanel.CreateGraphics();
            m_Rect = WavelinePanel.ClientRectangle;
        }
        #endregion

        public void SetPid(PumpID pid)
        {
            m_LocalPid = pid;
            if (_GrasebyDevice4FindPort != null)
            {
                _GrasebyDevice4FindPort.DeviceDataRecerived -= OnGrasebyDeviceDataRecerived;
                _GrasebyDevice4FindPort = null;
                SetPumpPortStatus(false);
            }
        }
              
        public void SetConfig(List<Misc.SyringeBrand> levelNBrands)
        {
            m_LevelNBrands = levelNBrands;
        }

        private void InitAllParameters()
        {
            m_XCoordinateMaxValue = 120;//秒
            m_FreshPumpPortEvent.Reset();
        }

        public void InitBrandList(Dictionary<Misc.SyringeBrand, String> syringeBrands)
        {
            m_SyringeBrands = syringeBrands;
            UpdateBrandList();
        }

        /// <summary>
        /// Update Brand list using Dictionary
        /// </summary>
        public void UpdateBrandList()
        {
            int selIndex = cmbSetBrand.SelectedIndex;
            cmbSetBrand.Items.Clear();
            foreach (var brand in m_SyringeBrands)
            {
                cmbSetBrand.Items.Add(brand.Value);
            }
            if (selIndex >= 0 && selIndex < cmbSetBrand.Items.Count)
            {
                cmbSetBrand.SelectedIndex = selIndex;
            }
            else if (cmbSetBrand.Items.Count > 4)
            {
                cmbSetBrand.SelectedIndex = 3;
            }
            else
            {
                cmbSetBrand.SelectedIndex = -1;
            }
        }
      
        public void AddHandler()
        {
            try
            {
                m_ConnResponse.SetSyringeBrandResponse += new EventHandler<ResponseEventArgs<String>>(SetSyringeBrand);
                m_ConnResponse.SetVTBIParameterResponse += new EventHandler<ResponseEventArgs<String>>(SetInfusionParas);
                m_ConnResponse.SetOcclusionLevelResponse += new EventHandler<ResponseEventArgs<String>>(SetOcclusionLevel);
                m_ConnResponse.SetStartControlResponse += new EventHandler<ResponseEventArgs<String>>(SetStartControl);
                m_ConnResponse.SetStopControlResponse += new EventHandler<ResponseEventArgs<String>>(SetStopControl);
                m_ConnResponse.GetSyringSizeResponse += new EventHandler<ResponseEventArgs<Misc.SyringeSizeInfo>>(GetSyringSize);
                m_ConnResponse.GetPumpAlarmsResponse += new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms);
                m_ConnResponse.GetPumpStatusResponse += new EventHandler<ResponseEventArgs<Misc.PumpStatusInfo>>(GetPumpStatus);
                m_ConnResponse.GetInfusionParameterResponse += new EventHandler<ResponseEventArgs<Misc.InfusionParamemter>>(GetInfusionParameter);
            }
            catch 
            { 
            }
        }

        /// <summary>
        /// Remove handle functions  and stop responsing message from pump 
        /// </summary>
        public void RemoveHandler()
        {
            try
            {
                m_ConnResponse.SetSyringeBrandResponse -= new EventHandler<ResponseEventArgs<String>>(SetSyringeBrand);
                m_ConnResponse.SetVTBIParameterResponse -= new EventHandler<ResponseEventArgs<String>>(SetInfusionParas);
                m_ConnResponse.SetOcclusionLevelResponse -= new EventHandler<ResponseEventArgs<String>>(SetOcclusionLevel);
                m_ConnResponse.SetStartControlResponse -= new EventHandler<ResponseEventArgs<String>>(SetStartControl);
                m_ConnResponse.SetStopControlResponse -= new EventHandler<ResponseEventArgs<String>>(SetStopControl);
                m_ConnResponse.GetSyringSizeResponse -= new EventHandler<ResponseEventArgs<Misc.SyringeSizeInfo>>(GetSyringSize);
                m_ConnResponse.GetPumpAlarmsResponse -= new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms);
                m_ConnResponse.GetPumpStatusResponse -= new EventHandler<ResponseEventArgs<Misc.PumpStatusInfo>>(GetPumpStatus);
                m_ConnResponse.GetInfusionParameterResponse -= new EventHandler<ResponseEventArgs<Misc.InfusionParamemter>>(GetInfusionParameter);
            }
            catch
            { 
            }
        }

        #region 时钟
        private void StartTimer()
        {
            StopTimer();
            m_Ch1Timer.Start();
            m_Ch1Timer.Enabled = true;
            Logger.Instance().Info("StartTimer执行！");
            if (m_ConnResponse != null)
            {
                m_ConnResponse.GetPumpAlarmsResponse -= new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms);
                m_ConnResponse.GetPumpAlarmsResponse += new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms);
            }
        }

        private void StopTimer()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateFuctionNoParamater(StopTimer), null);
                return;
            }

            if (m_ConnResponse != null)
                m_ConnResponse.GetPumpAlarmsResponse -= new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms);
            m_Ch1Timer.Stop();
            m_Ch1Timer.Enabled = false;
            Logger.Instance().Info("StopTimer执行！");
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_bStartTimer && !m_bMaxKpaFlag && m_ConnResponse != null && m_ConnResponse.IsOpen())
            {
                //查询报警信息
                m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.GetPumpAlarms);
                SendNextRequest();
            }
        }

        private void StartTimerGauge()
        {
            StopTimerGauge();
            m_GaugeTimer.Start();
            m_GaugeTimer.Enabled = true;
            Logger.Instance().Info("StartTimerGauge执行！");
        }

        private void StopTimerGauge()
        {
            m_GaugeTimer.Stop();
            m_GaugeTimer.Enabled = false;
            Logger.Instance().Info("StopTimerGauge执行！");
        }

        private void OnTimerGauge(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!m_bMaxKpaFlag)
            {
                if (m_GaugeTool != null && m_GaugeTool.IsOpen())
                {
                    m_GaugeTool.SendQueryCmd(); //向压力表请求数据
                }
            }
        }
        
        #endregion
               
        /// <summary>
        /// 仅检测串口使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrasebyDeviceDataRecerived(object sender, EventArgs e)
        {
            m_FreshPumpPortEvent.Set();
        }

        private void OnGaugeDataRecerived(object sender, PressureGaugeDataEventArgs e)
        {
            if (m_bMaxKpaFlag)
                return;
            m_Ch1SampleDataList.Add(new SampleData(DateTime.Now, e.PressureValue));
            float count = m_XCoordinateMaxValue * 1000 / m_GaugeSampleInterval;
            if (m_Ch1SampleDataList.Count > count)
                ReDrawCoordinate();
            else
                DrawSingleAccuracyMap(m_XSectionCount, m_YSectionCount);
            if (e.PressureValue >= MAXKPALIMIT)
            {
                m_bMaxKpaFlag = true;
                //超过最大值，报警，并停止泵
                StopTimer();
                StopTimerGauge();
                AlertMaxKpaSub();
                this.InvokeOnClick(this.picStop, null);
                //StopTest();
            }
        }

        #region 画图
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xSectionCount">X轴的坐标数量</param>
        /// <param name="ySectionCount">Y轴坐标数量</param>
        private void DrawSingleAccuracyMap(int xSectionCount = 20, int ySectionCount = 20)
        {
            lock (m_gh)
            {
                if (m_Ch1SampleDataList.Count <= 1)
                    return;
                Rectangle rect = m_Rect;
                Font xValuefont = new Font("宋体", 7);
                Font fontTitle = new Font("宋体", 8);
                //X轴原点
                PointF xOriginalPoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                //X轴终点
                PointF xEndPoint = new PointF((float)rect.Right - RIGHTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                //Y轴最下面的点位置
                PointF yOriginalPoint = xOriginalPoint;
                //Y轴终点（由下向上）
                PointF yEndPoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, (float)rect.Top + TOPBOTTOMFFSET);
                float y0 = 0, y1 = 0, x0 = 0, x1 = 0;
                int i = m_Ch1SampleDataList.Count - 1;
                y0 = xOriginalPoint.Y - ((yOriginalPoint.Y - yEndPoint.Y) / ySectionCount * ((m_Ch1SampleDataList[i - 1].m_PressureValue / m_ValueInervalY)));
                y1 = xOriginalPoint.Y - ((yOriginalPoint.Y - yEndPoint.Y) / ySectionCount * ((m_Ch1SampleDataList[i].m_PressureValue / m_ValueInervalY)));
                x0 = (xEndPoint.X - xOriginalPoint.X) / (m_XCoordinateMaxValue * 1000 / m_GaugeSampleInterval) * (i) + xOriginalPoint.X;
                x1 = (xEndPoint.X - xOriginalPoint.X) / (m_XCoordinateMaxValue * 1000 / m_GaugeSampleInterval) * (i + 1) + xOriginalPoint.X;
                try
                {
                    m_gh.DrawLine(m_WaveLinePen, new PointF(x0, y0), new PointF(x1, y1));
                }
                catch (Exception e)
                {
                    MessageBox.Show("画单个点时报错：" + e.Message);
                }
            }
        }

        /// <summary>
        /// 界面移动或变化时需要重绘所有点
        /// </summary>
        /// <param name="xSectionCount"></param>
        /// <param name="ySectionCount"></param>
        private void DrawAccuracyMap(int xSectionCount = 20, int ySectionCount = 20)
        {
            lock (m_gh)
            {
                try
                {
                    if (m_Ch1SampleDataList.Count <= 1)
                        return;
                    Rectangle rect = m_Rect;
                    Font xValuefont = new Font("宋体", 7);
                    Font fontTitle = new Font("宋体", 8);
                    //画X轴
                    //X轴原点
                    PointF xOriginalPoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                    //X轴终点
                    PointF xEndPoint = new PointF((float)rect.Right - RIGHTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                    //Y轴最下面的点位置
                    PointF yOriginalPoint = xOriginalPoint;
                    //Y轴终点（由下向上）
                    PointF yEndPoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, (float)rect.Top + TOPBOTTOMFFSET);
                    string strMsg = string.Empty;
                    float y0 = 0, y1 = 0, x0 = 0, x1 = 0;
                    int count = m_Ch1SampleDataList.Count;
                    for (int iLoop = 1; iLoop < count; iLoop++)
                    {
                        y0 = xOriginalPoint.Y - ((yOriginalPoint.Y - yEndPoint.Y) / ySectionCount * ((m_Ch1SampleDataList[iLoop - 1].m_PressureValue / m_ValueInervalY)));
                        y1 = xOriginalPoint.Y - ((yOriginalPoint.Y - yEndPoint.Y) / ySectionCount * ((m_Ch1SampleDataList[iLoop].m_PressureValue / m_ValueInervalY)));
                        x0 = (xEndPoint.X - xOriginalPoint.X) / (m_XCoordinateMaxValue * 1000 / m_GaugeSampleInterval) * (iLoop) + xOriginalPoint.X;
                        x1 = (xEndPoint.X - xOriginalPoint.X) / (m_XCoordinateMaxValue * 1000 / m_GaugeSampleInterval) * (iLoop + 1) + xOriginalPoint.X;
                        m_gh.DrawLine(m_WaveLinePen, new PointF(x0, y0), new PointF(x1, y1));
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show("重绘曲线时报错:"+e.Message);
                }
            }
        }

        private void WavelinePanel_Paint(object sender, PaintEventArgs e)
        {
            DrawCoordinate(m_XCoordinateMaxValue, m_XSectionCount, m_YCoordinateMaxValue, m_YSectionCount);
            DrawAccuracyMap(m_XSectionCount, m_YSectionCount);
        }

        /// <summary>
        /// 画坐标轴
        /// </summary>
        /// <param name="xMax">X坐标最大值</param>
        /// <param name="xSectionCount">X坐标分成几段</param>
        /// <param name="yMax">Y坐标最大值</param>
        /// <param name="ySectionCount">Y坐标分成几段</param>
        private void DrawCoordinate(float xMax, int xSectionCount, int yMax, int ySectionCount)
        {
            lock (m_gh)
            {
                try
                {
                    Rectangle rect = m_Rect;
                    Font xValuefont = new Font("宋体", 7);
                    Font fontTitle = new Font("宋体", 8);
                    Font fontChartDes = new Font("Noto Sans CJK SC Bold", 12);
                    //画X轴
                    PointF originalpoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                    PointF xEndPoint = new PointF((float)rect.Right - RIGHTBORDEROFFSET, rect.Bottom - BOTTOMBORDEROFFSET);
                    m_gh.DrawLine(Pens.Black, originalpoint, xEndPoint);
                    //画X坐标箭头
                    PointF arrowpointUp = new PointF(xEndPoint.X - 12, xEndPoint.Y - 6);
                    PointF arrowpointDwon = new PointF(xEndPoint.X - 12, xEndPoint.Y + 6);
                    m_gh.DrawLine(Pens.Black, arrowpointUp, xEndPoint);
                    m_gh.DrawLine(Pens.Black, arrowpointDwon, xEndPoint);

                    //画X轴坐标,SECTIONCOUNT个点
                    float intervalX = (xEndPoint.X - originalpoint.X) / xSectionCount;
                    m_CoordinateIntervalX = intervalX;
                    float lineSegmentHeight = 8f;
                    for (int i = 1; i <= xSectionCount - 1; i++)
                    {
                        m_gh.DrawLine(Pens.Black, new PointF(originalpoint.X + intervalX * i, originalpoint.Y), new PointF(originalpoint.X + intervalX * i, originalpoint.Y - lineSegmentHeight));
                    }
                    //画X坐标值
                    float xValueInerval = xMax / xSectionCount;
                    m_ValueInervalX = xValueInerval;
                    //画Y轴
                    PointF yEndPoint = new PointF((float)rect.Left + LEFTBORDEROFFSET, (float)rect.Top + TOPBOTTOMFFSET);
                    //写图形描述字符
                    m_gh.DrawString("压力时间关系图", fontChartDes, m_WaveLineBrush, new PointF(yEndPoint.X + 180, yEndPoint.Y));
                    //y轴的起始点，从底部往上
                    PointF yOriginalPoint = originalpoint;//new PointF((float)rect.Left + LEFTBORDEROFFSET, rect.Bottom - TOPBOTTOMFFSET);
                    m_gh.DrawLine(Pens.Black, yOriginalPoint, yEndPoint);
                    //画Y坐标箭头
                    PointF arrowpointLeft = new PointF(yEndPoint.X - 6, yEndPoint.Y + 12);
                    PointF arrowpointRight = new PointF(yEndPoint.X + 6, yEndPoint.Y + 12);
                    m_gh.DrawLine(Pens.Black, arrowpointLeft, yEndPoint);
                    m_gh.DrawLine(Pens.Black, arrowpointRight, yEndPoint);
                    //画Y轴坐标,每个区间的实际坐标长度
                    float intervalY = Math.Abs(yEndPoint.Y - yOriginalPoint.Y) / ySectionCount;
                    m_CoordinateIntervalY = intervalY;
                    for (int i = 0; i < ySectionCount; i++)
                    {
                        m_gh.DrawLine(Pens.Black, new PointF(yOriginalPoint.X, yOriginalPoint.Y - intervalY * i), new PointF(yOriginalPoint.X + lineSegmentHeight, yOriginalPoint.Y - intervalY * i));
                    }
                    float yValueInerval = (float)yMax / ySectionCount;
                    m_ValueInervalY = yValueInerval;//Y轴上的坐标值，根据实际放大倍数和量程决定
                    for (int i = 0; i <= ySectionCount; i++)
                    {
                        m_gh.DrawString((i * 10).ToString(), xValuefont, Brushes.Black, new PointF(yOriginalPoint.X - 24, yOriginalPoint.Y - intervalY * i - 6));
                    }
                    //画legend
                    m_gh.DrawString(VOL, fontTitle, m_WaveLineBrush, new PointF(xEndPoint.X - 200, 10));
                    SizeF fontSize = m_gh.MeasureString(VOL, fontTitle);

                    m_gh.DrawLine(m_WaveLinePen, new PointF(xEndPoint.X - 180, 10 + fontSize.Height / 2), new PointF(xEndPoint.X - 160, 10 + fontSize.Height / 2));
                }
                catch (Exception e)
                {
                    //MessageBox.Show("DrawCoordinate Error:" + e.Message);
                }
            }
        }

        private void ReDrawCoordinate()
        {
            m_XCoordinateMaxValue += 60;
            this.WavelinePanel.Invalidate();
        }
        #endregion

        #region 控件事件
        private void Chart_Load(object sender, EventArgs e)
        {
            if (cmbSetBrand.Items.Count > 4)
                cmbSetBrand.SelectedIndex = 3;
            cbPumpPort.Items.AddRange(SerialPort.GetPortNames());
            cbToolingPort.Items.AddRange(SerialPort.GetPortNames());
            m_Ch1Timer.Interval = m_SampleInterval;
            m_Ch1Timer.Elapsed += OnTimer;

            m_GaugeTimer.Interval = m_GaugeSampleInterval;
            m_GaugeTimer.Elapsed += OnTimerGauge;
        }

        /// <summary>
        /// 当不可用时，将按钮图标变灰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                picStart.Image = global::FQC.Properties.Resources.icon_start_Blue;
                picStop.Image = global::FQC.Properties.Resources.icon_stop_blue;
                picDetail.Image = global::FQC.Properties.Resources.icon_tablelist_blue;
                if (m_Channel == 2)
                {
                    picChannel.Image = global::FQC.Properties.Resources.icon_2_blue;
                }
            }
            else
            {
                picStart.Image = global::FQC.Properties.Resources.icon_start_gray;
                picStop.Image = global::FQC.Properties.Resources.icon_stop_gray;
                picDetail.Image = global::FQC.Properties.Resources.icon_tablelist_gray;
                if (m_Channel == 2)
                {
                    picChannel.Image = global::FQC.Properties.Resources.icon_2_gray;
                }
            }
        }

        /// <summary>
        /// 串口选择时发送命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbToolingPort_SelectedIndexChanged(object sender, EventArgs e)
        {
        
            if (m_DetectPTool == null)
                m_DetectPTool = new SerialPressureGauge(PressureForm.m_ACDPortParameter.BaudRate,
                                                        PressureForm.m_ACDPortParameter.DataBits,
                                                        PressureForm.m_ACDPortParameter.StopBit,
                                                        PressureForm.m_ACDPortParameter.ParityType,
                                                        cbToolingPort.Items[cbToolingPort.SelectedIndex].ToString()
                                                        );

            string name = m_DetectPTool.FreshCom(cbToolingPort.Items[cbToolingPort.SelectedIndex].ToString());
            if(string.IsNullOrEmpty(name))
            {
                picGaugePortStatus.Tag = false;
                picGaugePortStatus.Visible = true;
                picGaugePortStatus.Image = global::FQC.Properties.Resources.error;
            }
            else
            {
                picGaugePortStatus.Tag = true;
                picGaugePortStatus.Visible = true;
                picGaugePortStatus.Image = global::FQC.Properties.Resources.ok;
            }
        }

        private void cbPumpPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_GrasebyDevice4FindPort == null)
            {
                switch (m_LocalPid)
                {
                    case PumpID.GrasebyC8:
                        _GrasebyDevice4FindPort = new Graseby115200();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyC8);
                        ((Graseby115200)_GrasebyDevice4FindPort).ChannelNumber = this.Channel;
                        break;
                    case PumpID.GrasebyF8:
                        _GrasebyDevice4FindPort = new Graseby115200();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyF8);
                        ((Graseby115200)_GrasebyDevice4FindPort).ChannelNumber = this.Channel;
                        break;
                    case PumpID.GrasebyF8_2:
                        _GrasebyDevice4FindPort = new Graseby115200();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyF8);
                        ((Graseby115200)_GrasebyDevice4FindPort).ChannelNumber = this.Channel;
                        break;
                    case PumpID.GrasebyC6:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyC6);
                        break;
                    case PumpID.GrasebyF6:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyF6);
                        break;
                    case PumpID.GrasebyF6_2:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyF6);
                        break;
                    case PumpID.GrasebyC6T:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.GrasebyC6T);
                        break;
                    case PumpID.Graseby2000:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.Graseby2000);
                        break;
                    case PumpID.Graseby2100:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.Graseby2100);
                        break;
                    case PumpID.WZ50C6:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.WZ50C6);
                        break;
                    case PumpID.WZS50F6:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.WZS50F6);
                        break;
                    case PumpID.WZS50F6_2:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.WZS50F6);
                        break;
                    case PumpID.WZ50C6T:
                        _GrasebyDevice4FindPort = new Graseby9600();
                        _GrasebyDevice4FindPort.SetDeviceType(DeviceType.WZ50C6T);
                        break;
                }
            }
            _GrasebyDevice4FindPort.DeviceDataRecerived += OnGrasebyDeviceDataRecerived;
            if (_GrasebyDevice4FindPort.IsOpen())
                _GrasebyDevice4FindPort.Close();

            _GrasebyDevice4FindPort.Init(cbPumpPort.Items[cbPumpPort.SelectedIndex].ToString());
            _GrasebyDevice4FindPort.Open();
            _GrasebyDevice4FindPort.Get();
            if(m_FreshPumpPortEvent.WaitOne(1000))
            {
                SetPumpPortStatus(true);
            }
            else
            {
                SetPumpPortStatus(false);
            }
            _GrasebyDevice4FindPort.DeviceDataRecerived -= OnGrasebyDeviceDataRecerived;
            _GrasebyDevice4FindPort.Close();
            _GrasebyDevice4FindPort = null;
            m_FreshPumpPortEvent.Reset();
        }

        private void picStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        /// <summary>
        /// 启动测试
        /// </summary>
        public void Start()
        {
            #region 第一道泵是否已经测试完成
            if (m_LocalPid == PumpID.GrasebyF8_2 && this.Channel == 2)// && PressureForm.SampleDataList[0].Channel!=1)
            {
                if (PressureForm.SampleDataList.Count == 0 || PressureForm.SampleDataList[0].Channel != 1)
                {
                    MessageBox.Show("请先完成第一道测试！");
                    return;
                }
            }
            #endregion

            #region 参数输入检查

            //if (string.IsNullOrEmpty(tbOprator.Text))
            //{
            //    MessageBox.Show("请输入操作员工号");
            //    return;
            //}
            //if (tbOprator.Text.Length != 8)
            //{
            //    MessageBox.Show("请输入正确操作员工号");
            //    return;
            //}

            if (SamplingStartOrStop != null)
            {
                SamplingStartOrStop(this, new StartOrStopArgs(true));
            }

            if (string.IsNullOrEmpty(PumpNo))
            {
                MessageBox.Show("请输入产品序号");
                return;
            }
            if (PumpNo.Length != PressureForm.SerialNumberCount)
            {
                MessageBox.Show("产品序号长度不正确");
                return;
            }
            float rate = 0;
            if (cbToolingPort.SelectedIndex < 0)
            {
                MessageBox.Show("请选择工装串口");
                return;
            }
            if ((bool)picGaugePortStatus.Tag == false)
            {
                MessageBox.Show("工装串口不正确");
                return;
            }
            if (cbPumpPort.SelectedIndex < 0)
            {
                MessageBox.Show("请选择泵串口");
                return;
            }
            if ((bool)picPumpPortStatus.Tag == false)
            {
                MessageBox.Show("泵串口不正确");
                return;
            }
            if (string.IsNullOrEmpty(tbRate.Text))
            {
                MessageBox.Show("请输入速率！");
                return;
            }
            if (!float.TryParse(tbRate.Text, out rate))
            {
                MessageBox.Show("请正确输入速率！");
                return;
            }
            if (cmbLevel.SelectedIndex < 0)
            {
                MessageBox.Show("请选择压力档位！");
                return;
            }
            #endregion

            #region 泵型号选择
            Misc.ProductID pid = Misc.ProductID.None;
            switch (m_LocalPid)
            {
                case PumpID.GrasebyC8:
                    pid = Misc.ProductID.GrasebyC8;
                    break;
                case PumpID.GrasebyF8:
                    pid = Misc.ProductID.GrasebyF8;
                    break;
                case PumpID.GrasebyF8_2:
                    pid = Misc.ProductID.GrasebyF8;
                    break;
                case PumpID.GrasebyC6:
                    pid = Misc.ProductID.GrasebyC6;
                    break;
                case PumpID.WZ50C6:
                    pid = Misc.ProductID.GrasebyC6;
                    break;
                case PumpID.GrasebyC6T:
                case PumpID.WZ50C6T:
                    pid = Misc.ProductID.GrasebyC6T;
                    break;
                case PumpID.Graseby2000:
                    pid = Misc.ProductID.Graseby2000;
                    break;
                case PumpID.Graseby2100:
                    pid = Misc.ProductID.Graseby2100;
                    break;
                case PumpID.WZS50F6:
                    pid = Misc.ProductID.GrasebyF6;
                    break;
                case PumpID.GrasebyF6:
                    pid = Misc.ProductID.GrasebyF6;
                    break;
                case PumpID.GrasebyF6_2:
                    pid = Misc.ProductID.GrasebyF6;
                    break;
                case PumpID.WZS50F6_2:
                    pid = Misc.ProductID.GrasebyF6;
                    break;
                default:
                    pid = Misc.ProductID.None;
                    break;
            }
            #endregion

            if (pid == Misc.ProductID.None)
            {
                MessageBox.Show("选择的泵类型错误，请联系管理员!");
                return;
            }
            if(m_ConnResponse != null)
                RemoveHandler();
            if (m_ConnResponse != null)
            {
                m_ConnResponse.CloseConnection();
                Thread.Sleep(1500);
                m_ConnResponse.Dispose();
                m_ConnResponse = null;
            }
            SerialPortParameter para = PressureForm.m_DicPumpPortParameter[pid] as SerialPortParameter;
#if DEBUG
            m_ConnResponse = new GlobalResponse(Misc.ProductID.GrasebyC6, Misc.CommunicationProtocolType.General);
#else
            m_ConnResponse = new GlobalResponse(pid, Misc.CommunicationProtocolType.General);
#endif
            m_ConnResponse.ChannelNumber = this.Channel;
            m_ConnResponse.Initialize(cbPumpPort.Items[cbPumpPort.SelectedIndex].ToString(), para.BaudRate);
            if(!m_ConnResponse.IsOpen())
            {
                MessageBox.Show("串口被占用，请关闭本软件后重新测试!");
                return;
            }
            m_ConnResponse.SetStartControl();

            RemoveHandler();
            AddHandler();
            if (m_GaugeTool != null)
            {
                if (!m_GaugeTool.IsOpen())
                {
                    m_GaugeTool.PressureGaugeDataRecerived += OnGaugeDataRecerived;
                    m_GaugeTool.Open();
                }
            }
            else
            {
                m_GaugeTool = new SerialPressureGauge(PressureForm.m_ACDPortParameter.BaudRate,
                                                       PressureForm.m_ACDPortParameter.DataBits,
                                                       PressureForm.m_ACDPortParameter.StopBit,
                                                       PressureForm.m_ACDPortParameter.ParityType,
                                                       cbToolingPort.Items[cbToolingPort.SelectedIndex].ToString());
                m_GaugeTool.PressureGaugeDataRecerived += OnGaugeDataRecerived;
                bool bOpen = m_GaugeTool.Open();
                if (!bOpen)
                {
                    m_GaugeTool.Close();
                    MessageBox.Show("压力表串口打开失败！");
                    return;
                }
            }

            #region Clear Value
            detail.ClearLabelValue();
            detail.Pid = m_LocalPid;
            mFQCData.Channel = 0;
            mFQCData.brand = string.Empty;
            mFQCData.pressureN = 0;
            mFQCData.pressureL = 0;
            mFQCData.pressureC = 0;
            mFQCData.pressureH = 0;
            mFQCData.syrangeSize = 0;
            m_Ch1SampleDataList.Clear();
            WavelinePanel.Invalidate();
            if(Channel==1)
            {
                if (m_LocalPid == PumpID.GrasebyF6_2 || m_LocalPid == PumpID.WZS50F6_2)
                {
                    ;//不要清空数据，因为它们可以双道并行
                }
                else
                {
                    PressureForm.SampleDataList.Clear();
                }
                //清空第二道泵数据
                if(this.m_LocalPid == PumpID.GrasebyF8_2 && Clear2ndChannelData!=null)
                {
                    Clear2ndChannelData(this, null);
                }
            }
            #endregion

            mFQCData.brand = cmbSetBrand.Items[cmbSetBrand.SelectedIndex].ToString().Substring(4);
            InitAllParameters();
            m_ConnResponse.ClearAllCommand();
            m_RequestCommands.Clear();
            if (pid == Misc.ProductID.GrasebyC8 || pid == Misc.ProductID.GrasebyF8)
                m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.GetSyringeSize);
            else
                m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.GetInfusionParameter);
            m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetSyringeBrand);
            m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetVTBIParameter);
            m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetOcclusionLevel);
            m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetStartControl);
            SendNextRequest();
            EnableContols(false);
        }

        private void picStop_Click(object sender, EventArgs e)
        {
            StopTimer();
            StopTimerGauge();
            BeginStopTestThread();
            m_Ch1SampleDataList.Clear();
            if (Channel == 1)
            {
                if (m_LocalPid == PumpID.GrasebyF6_2 || m_LocalPid == PumpID.WZS50F6_2)
                {
                    ;//不要清空数据，因为它们可以双道并行
                }
                else
                {
                    PressureForm.SampleDataList.Clear();
                }
            }
            else if (Channel == 2)
            {
                if (PressureForm.SampleDataList.Count >= 2)
                    PressureForm.SampleDataList.RemoveRange(1, PressureForm.SampleDataList.Count - 1);
            }
            else
                PressureForm.SampleDataList.Clear();
            EnableContols(true);
        }

        private void picDetail_Click(object sender, EventArgs e)
        {
            this.detail.Show();
        }

        /// <summary>
        /// 品牌改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSetBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_OcclusionLevelOfBrand.Clear();
            string brandName = cmbSetBrand.Items[cmbSetBrand.SelectedIndex].ToString();
            m_CurrentBrand = FindSyringeBrandByName(brandName);//由品牌名称查询枚举值
            //当前品牌,有没有N档?

            if (m_ProductModel == ProductModel.GrasebyC8 || m_ProductModel == ProductModel.GrasebyF8)
            {
                if (m_LevelNBrands.Contains(m_CurrentBrand))
                    m_OcclusionLevelOfBrand.Add(Misc.OcclusionLevel.N);
            }
            m_OcclusionLevelOfBrand.Add(Misc.OcclusionLevel.L);
            m_OcclusionLevelOfBrand.Add(Misc.OcclusionLevel.C);
            m_OcclusionLevelOfBrand.Add(Misc.OcclusionLevel.H);
            m_CurrentLevel = Misc.OcclusionLevel.None;
            cmbLevel.Items.Clear();
            foreach (Misc.OcclusionLevel level in m_OcclusionLevelOfBrand)
            {
                cmbLevel.Items.Add(level.ToString());
            }
            if (cmbLevel.Items.Count > 0)
                cmbLevel.SelectedIndex = 0;
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string level = cmbLevel.Items[cmbLevel.SelectedIndex].ToString();
            if (Enum.IsDefined(typeof(Misc.OcclusionLevel), level))
            {
                m_CurrentLevel = (Misc.OcclusionLevel)Enum.Parse(typeof(Misc.OcclusionLevel), level);
            }
        }

        /// <summary>
        /// 速率不能输入非法值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRateKeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;                         //让操作生效
                if (txt.Text.Length == 0)
                {
                    if (e.KeyChar == '0')
                        e.Handled = true;                  //让操作失效，第一个字符不能输入0
                }
                else if (txt.Text.Length >= 3)
                {

                    if (e.KeyChar == (char)Keys.Back)
                        e.Handled = false;             //让操作生效
                    else
                        e.Handled = true;              //让操作失效，如果第一个字符是2以上，不能输入其他字符
                }
                else
                {
                    e.Handled = false;                 //让操作生效
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion

        #region 命令响应
        /// <summary>
        /// this function is invoked by GlobalResponse class event, when m_ConnResponse.SetSyringeBrand() is called; 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetSyringeBrand(object sender, ResponseEventArgs<String> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<String>>(SetSyringeBrand), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'SetSyringeBrand'指令返回错误！ErrorMessage = {0}", args.ErrorMessage);
                MessageBox.Show("设置注射器品牌失败！");
                //m_CurrentLevel = m_PreLevel;
                StopTestWithoutStartPump();
            }
            else
            {
                Logger.Instance().Info("命令'SetSyringeBrand'指令成功返回！");
                SendNextRequest();
                //设置成功
            }
        }

        /// <summary>
        /// this function is invoked by GlobalResponse class event
        /// when m_ConnResponse.SetOcclusionLevel() is called; 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetInfusionParas(object sender, ResponseEventArgs<String> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<String>>(SetInfusionParas), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'SetInfusionParas'指令返回错误！ErrorMessage = {0}", args.ErrorMessage);
                MessageBox.Show("设置速率失败！");
                //m_CurrentLevel = m_PreLevel;
                StopTestWithoutStartPump();
            }
            else
            {
                Logger.Instance().Info("命令'SetInfusionParas'指令成功返回！");
                SendNextRequest();
            }
        }

        /// <summary>
        /// this function is invoked by GlobalResponse class event, when m_ConnResponse.SetOcclusionLevel() is called; 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetOcclusionLevel(object sender, ResponseEventArgs<String> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<String>>(SetOcclusionLevel), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                ++m_LevelTryCount;
                Logger.Instance().ErrorFormat("命令'SetOcclusionLevel'指令返回错误！ErrorMessage = {0}, m_LevelTryCount = {1}, m_CurrentLevel={2}", args.ErrorMessage, m_LevelTryCount, m_CurrentLevel);
                if (m_LevelTryCount < 3)
                {
                    //m_ConnResponse.SetOcclusionLevel(m_CurrentLevel);
                    //m_ConnResponse.ClearAllCommand();
                    m_RequestCommands.Clear();
                    m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetOcclusionLevel);
                    //m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetStartControl);
                    SendNextRequest();
                    //SendNextRequest();
                }
                else
                {
                    ErrorDialog dlg = new ErrorDialog("设置压力档失败，请选择最低档后重新开始测试！");
                    dlg.Show();
                    StopTestWithoutStartPump();
                }
            }
            else
            {
                m_LevelTryCount = 0;
                Logger.Instance().Info("命令'SetOcclusionLevel'指令成功返回！");
                SendNextRequest();
            }
        }

        /// <summary>
        /// this function is invoked by GlobalResponse class event
        /// this function return a SyringSize value
        /// according to this value, set infusion rate maxvalue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GetSyringSize(object sender, ResponseEventArgs<Misc.SyringeSizeInfo> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<Misc.SyringeSizeInfo>>(GetSyringSize), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'GetSyringSize'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
                StopTestWithoutStartPump();
                ErrorDialog dlg = new ErrorDialog("读注射器尺寸失败！");
                dlg.Show();
            }
            else
            {
                mFQCData.syrangeSize = args.EventData.size;
                SendNextRequest();
            }
        }

        private void GetInfusionParameter(object sender, ResponseEventArgs<Misc.InfusionParamemter> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<Misc.InfusionParamemter>>(GetInfusionParameter), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'GetInfusionParameter'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
                StopTestWithoutStartPump();
                ErrorDialog dlg = new ErrorDialog("读注射器尺寸失败！");
                dlg.Show();
            }
            else
            {
                mFQCData.syrangeSize = args.EventData.syringeSize;
                SendNextRequest();
            }
        }

        private void GetPressureCalibrationPValue(object sender, ResponseEventArgs<Misc.PValueInfo> args)
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new EventHandler<ResponseEventArgs<PValueInfo>>(GetPressureCalibrationPValue), new object[] { sender, args });
            //    return;
            //}
            //if (String.Empty != args.ErrorMessage)
            //{
            //    Logger.Instance().ErrorFormat("命令'GetPressureCalibrationPValue'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
            //    MessageBox.Show("读P值失败！");
            //}
            //else
            //{
            //    Logger.Instance().InfoFormat("命令'GetPressureCalibrationPValue'指令成功返回！P值={0}", m_PValue);
            //    m_PValue = args.EventData.PValue.ToString();//泵的P
            //    SendNextRequest();
            //}

        }

        /// <summary>
        /// this function is invoked by GlobalResponse class event, when m_ConnResponse.SetStartControl() is called; 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetStartControl(object sender, ResponseEventArgs<String> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<String>>(SetStartControl), new object[] { sender, args });
                return;
            }

            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'SetStartControl'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
                ErrorDialog dlg = new ErrorDialog("启动泵失败！");
                dlg.Show();
            }
            else
            {
                Logger.Instance().Info("命令'SetStartControl'指令成功返回！");
                StartTimer();
                StartTimerGauge();
                m_StartTime = DateTime.Now;
                //EnableAllControls(false);
                SendNextRequest();
            }
        }

        /// <summary>
        /// this function is invoked by GlobalResponse class event, when m_ConnResponse.SetStopControl() is called; 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SetStopControl(object sender, ResponseEventArgs<String> args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<ResponseEventArgs<String>>(SetStopControl), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'SetStopControl'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
            }
            else
            {
                Logger.Instance().Info("命令'SetStopControl'指令成功返回！");
                m_StopTime = DateTime.Now;
            }
            if (m_bTestOverFlag)
            {
                m_bTestOverFlag = false;
                m_ConnResponse.CloseConnection();
                m_StopPumpEvent.Set();
            }
        }

        private void GetPumpAlarms(object sender, ResponseEventArgs<Misc.AlarmInfo> args)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ResponseEventArgs<Misc.AlarmInfo>>(GetPumpAlarms), new object[] { sender, args });
                return;
            }

            System.Diagnostics.Debug.WriteLine("begin Invoke GetPumpAlarms");
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'GetPumpAlarms'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
                m_AlarmInfo = new Misc.AlarmInfo();//set false
                m_TryCount++;
                if (m_TryCount > 5)
                {
                    ShowErrorDialog("读取报警数据失败,停止测试！");
                    StopTest();
                }
            }
            else
            {
                m_TryCount = 0;
                Misc.AlarmInfo paras = args.EventData;
                if (!m_AlarmInfo.Equals(paras))
                    m_AlarmInfo = paras;
                AlarmsInformation alarmMessage = new AlarmsInformation(m_ProductModel, SalesRegion.ZH);
                List<String> alarmList = alarmMessage.GetAlarmsString(paras);

                StringBuilder sb = new StringBuilder();
                foreach (String s in alarmList)
                {
                    sb.Append(s);
                    sb.Append(";");
                }
                Logger.Instance().InfoFormat("命令'GetPumpAlarms'指令返回成功！报警={0}", sb.ToString());
                //此处要加其他的报警排除项
                foreach (String s in alarmList)
                {
                    if ("Occlusion" == s)
                    {
                        Logger.Instance().Info("命令'GetPumpAlarms'测到Occlusion报警，启动停止泵线程");
                        InvokeOnClick(btnStopAlarm, null);
                        BeginPauseTestThread();
                        break;
                    }
                    else if (s.Contains("Near Empty"))
                    {
                        SendNextRequest();
                        continue;
                    }
                    else
                    {
                        StopTimer();
                        StopTimerGauge();
                        this.InvokeOnClick(this.picStop, null);
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// 泵状态响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GetPumpStatus(object sender, ResponseEventArgs<Misc.PumpStatusInfo> args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<ResponseEventArgs<Misc.PumpStatusInfo>>(GetPumpStatus), new object[] { sender, args });
                return;
            }
            if (String.Empty != args.ErrorMessage)
            {
                Logger.Instance().ErrorFormat("命令'GetPumpStatus'指令返回错误！ErrorMessage={0}", args.ErrorMessage);
                ShowErrorDialog("读取泵状态数据失败,停止测试！");
                StopTest();
            }
            else
            {
                Misc.PumpStatus pumpStatus = args.EventData.pumpStatus;
                if(pumpStatus == Misc.PumpStatus.Stop || pumpStatus == Misc.PumpStatus.Off || pumpStatus == Misc.PumpStatus.Pause)
                {
                    m_StopPumpPumpStatusEvent.Set();
                    Logger.Instance().InfoFormat("命令'GetPumpStatus'指令返回！信号量已有信号！当前泵状态，pumpStatus={0}", pumpStatus);
                }
                else
                {
                    Logger.Instance().InfoFormat("命令'GetPumpStatus'指令返回！pumpStatus={0}", pumpStatus);
                }
            }
        }


        private void SendNextRequest()
        {
            try
            {
                lock (m_RequestCommands)
                {
                    #region
                    if (m_RequestCommands.Count <= 0)
                    {
                        return;
                    }
                    Misc.ApplicationRequestCommand cmd = m_RequestCommands.Peek();
                    switch (cmd)
                    {
                        case Misc.ApplicationRequestCommand.SetSyringeBrand:
                            {
                                m_ConnResponse.SetSyringeBrand(m_CurrentBrand);
                                Logger.Instance().Info("命令'SetSyringeBrand'指令发出！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.SetVTBIParameter:
                            {
                                float rate = 0;
                                if (float.TryParse(tbRate.Text, out rate))
                                {
                                    m_ConnResponse.SetVTBIParameter(0, rate);
                                    Logger.Instance().InfoFormat("命令'SetVTBIParameter'指令发出！速率={0}", rate);
                                }
                                else
                                    MessageBox.Show("请正确输入速率！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.SetOcclusionLevel:
                            {
                                if (m_CurrentLevel == Misc.OcclusionLevel.None)
                                {
                                    string level = cmbLevel.Items[cmbLevel.SelectedIndex].ToString();
                                    if (Enum.IsDefined(typeof(Misc.OcclusionLevel), level))
                                    {
                                        m_CurrentLevel = (Misc.OcclusionLevel)Enum.Parse(typeof(Misc.OcclusionLevel), level);
                                    }
                                }
                                m_ConnResponse.SetOcclusionLevel(m_CurrentLevel);
                                Logger.Instance().InfoFormat("命令'SetOcclusionLevel'指令发出！压力档位={0}", m_CurrentLevel);
                                break;
                            }
                        case Misc.ApplicationRequestCommand.SetStartControl:
                            {
                                m_ConnResponse.SetStartControl();
                                Logger.Instance().Info("命令'SetStartControl'指令发出！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.GetSyringeSize: //C8/F8
                            {
                                m_ConnResponse.GetSyringeSize();
                                Logger.Instance().Info("命令'GetSyringeSize'指令发出！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.GetInfusionParameter://C6/F6等其他老泵
                            {
                                m_ConnResponse.GetInfusionParamemter(); 
                                Logger.Instance().Info("命令'GetInfusionParamemter'指令发出！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.GetPressureCalibrationPValue:
                            {
                                m_ConnResponse.GetPressureCalibrationPValue();
                                Logger.Instance().Info("命令'GetPressureCalibrationPValue'指令发出！");
                                break;
                            }
                        case Misc.ApplicationRequestCommand.GetPumpAlarms:
                            {
                                m_ConnResponse.GetPumpAlarms();
                                Logger.Instance().Info("命令'GetPumpAlarms'指令发出！");
                                break;
                            }
                        default:
                            break;
                    }
                    if (m_RequestCommands.Count <= 0)
                    {
                        return;
                    }
                    m_RequestCommands.Dequeue();
                    #endregion
                }
            }
            catch(Exception ex)
            {
                Logger.Instance().Fatal("Chart::SendNextRequest() -> " + ex.Message);
            }
            
        }

        private void ShowErrorDialog(string message)
        {
            Thread th = new Thread((ParameterizedThreadStart)ProcShowErrorDialog);
            th.Start(message);
            //th.Join();
        }
        private void ProcShowErrorDialog(object message)
        {
            string msg = message as string;
            ErrorDialog dlg = new ErrorDialog(msg);
            dlg.ShowDialog();
        }

        #endregion

        private void SetPumpPortStatus(bool isOK = true)
        {
            picPumpPortStatus.Tag = isOK;
            picPumpPortStatus.Visible = true;
            if (isOK)
                picPumpPortStatus.Image = global::FQC.Properties.Resources.ok;
            else
                picPumpPortStatus.Image = global::FQC.Properties.Resources.error;
            //规定必须是第一道泵先刷新
            if(Channel==1 && isOK && OnPortFreshedSuccess!=null)
            {
                OnPortFreshedSuccess(this, null);
            }
        }

        private void EnableContols(bool bEnabled = true)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateEnableContols(EnableContols), new object[] { bEnabled });
                return;
            }
            Program.EnableContols(bEnabled);
            cbToolingPort.Enabled = bEnabled;
            cbPumpPort.Enabled = bEnabled;
            tbRate.Enabled = bEnabled;
            tbToolingNo.Enabled = bEnabled;
            picStart.Enabled = bEnabled;
            cmbSetBrand.Enabled = bEnabled;
            cmbLevel.Enabled =  bEnabled;
            cmbPattern.Enabled =  bEnabled;
            picStop.Enabled = !bEnabled;
            if (SamplingStartOrStop != null)
            {
                SamplingStartOrStop(this, new StartOrStopArgs(bEnabled));
            }
        }

        /// <summary>
        /// 生成第三方公司需要的表格
        /// </summary>
        /// <param name="name"></param>
        /// <param name="caliParameters">已经生成好的数据，直接写到表格中</param>
        private void GenReport(string name, string name2)
        {
            Logger.Instance().InfoFormat("======GenReport()函数调用 begin===== name={0},name2={1}", name, name2);

            string title = string.Empty;
            if (m_LocalPid == PumpID.GrasebyF8 || m_LocalPid == PumpID.GrasebyF8_2)
            {
                title = string.Format("泵型号:{0}{1}道 产品序号:{2} 工装编号:{3}", "GrasebyF8", m_Channel, m_PumpNo, ToolingNo);
            }
            else if (m_LocalPid == PumpID.GrasebyF6 || m_LocalPid == PumpID.GrasebyF6_2)
            {
                title = string.Format("泵型号:{0}{1}道 产品序号:{2} 工装编号:{3}", "GrasebyF6", m_Channel, m_PumpNo, ToolingNo);
            }
            else if (m_LocalPid == PumpID.WZS50F6 || m_LocalPid == PumpID.WZS50F6_2)
            {
                title = string.Format("泵型号:{0}{1}道 产品序号:{2} 工装编号:{3}", "WZS50F6", m_Channel, m_PumpNo, ToolingNo);
            }
            else
            {
                title = string.Format("泵型号：{0} 产品序号:{1} 工装编号:{2}", m_LocalPid.ToString(), m_PumpNo, ToolingNo);
            }
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("FQC压力数据");
            int columnIndex = 0;
            ws.Cell(1, ++columnIndex).Value = "机器编号";
            ws.Cell(1, ++columnIndex).Value = "机器型号";
            ws.Cell(1, ++columnIndex).Value = "道数";
            ws.Cell(1, ++columnIndex).Value = "工装编号";
            //ws.Cell(1, ++columnIndex).Value = "泵频道";//这列不要了
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
            
            columnIndex = 0;
            ws.Cell(2, ++columnIndex).Value = m_PumpNo;
            ws.Cell(2, ++columnIndex).Value = m_LocalPid.ToString();
            ws.Cell(2, ++columnIndex).Value = m_Channel;
            ws.Cell(2, ++columnIndex).Value = ToolingNo;
            //ws.Cell(2, ++columnIndex).Value = mFQCData.brand;
            ws.Cell(2, ++columnIndex).Value = (cmbSetBrand.SelectedIndex+1).ToString();
            ws.Cell(2, ++columnIndex).Value = mFQCData.syrangeSize;
            ws.Cell(2, ++columnIndex).Value = tbRate.Text;
            if(m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.N))
                ws.Cell(2, ++columnIndex).Value = mFQCData.pressureN;
            else
                ws.Cell(2, ++columnIndex).Value = "N/A";
            ws.Cell(2, ++columnIndex).Value = mFQCData.pressureL;
            ws.Cell(2, ++columnIndex).Value = mFQCData.pressureC;
            ws.Cell(2, ++columnIndex).Value = mFQCData.pressureH;

            bool bPass = true;
            string strError = string.Empty;
            List<LevelTips> strErrorList = new List<LevelTips>();
            if (IsAuto())
                bPass = IsPassAuto(strErrorList);
            else
                bPass = IsPassManual(strErrorList);

            foreach(var tips in strErrorList)
            {
                if (!tips.isPass)
                {
                    strError += tips.tips;
                }
            }

            if (bPass)
            {
                ws.Cell(2, ++columnIndex).Value = "Pass";
                ws.Range("A2", "N2").Style.Font.FontColor = XLColor.Green;
            }
            else
            {
                ws.Cell(2, ++columnIndex).Value = "Fail";
                ws.Range("A2", "N2").Style.Font.FontColor = XLColor.Red;
            }

            ws.Cell(2, ++columnIndex).Value = this.Operator;
            ws.Cell(2, ++columnIndex).Value = strError;

            ws.Range(1, 1, 2, 1).SetDataType(XLCellValues.Text);
            ws.Range(1, 4, 2, 4).SetDataType(XLCellValues.Text);
            ws.Range(1, 1, 2, columnIndex).Style.Alignment.SetWrapText();
            wb.SaveAs(name);
            Thread.Sleep(1000);
            File.Copy(name, name2, true);
            Logger.Instance().InfoFormat("======GenReport()函数调用 end=====");
        }

        /// <summary>
        /// 单泵模式下弹出
        /// </summary>
        private void AlertTestResult()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateAlertTestResult(AlertTestResult), null);
                return;
            }
            bool bPass = false;
            string strError = string.Empty;
            List<LevelTips> strErrorList = new List<LevelTips>();
            if (cmbPattern.SelectedIndex == 0)
                bPass = IsPassAuto(strErrorList);
            else
                bPass = IsPassManual(strErrorList);

            NewTestAgainDialog dlg = new NewTestAgainDialog(strErrorList);
            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Logger.Instance().Info("======AlertTestResult() 生成压力数据文件 begin=====");
                var pid = ProductIDConvertor.PumpID2ProductID(m_LocalPid);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data";
                string fileName = string.Format("{0}{1}{2}", pid.ToString(), m_PumpNo, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);
                string saveFileName = path + "\\" + fileName + ".xlsx";
                string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data Copy";
                if (!System.IO.Directory.Exists(path2))
                    System.IO.Directory.CreateDirectory(path2);
                string saveFileName2 = path2 + "\\" + fileName + ".xlsx";
                try
                {
                    GenReport(saveFileName, saveFileName2);
                }
                catch (Exception ex)
                {
                    Logger.Instance().ErrorFormat("======AlertTestResult() 生成压力数据文件出错,错误信息={0}", ex.Message);
                }
                if (ClearPumpNoWhenCompleteTest != null)
                {
                    ClearPumpNoWhenCompleteTest(this, null);
                }
                Logger.Instance().Info("======AlertTestResult() 生成压力数据文件 end=====");
            }
            else if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                #region //重新测试
                Close();
                ClearTestData();
                this.Enabled = true;
                Thread.Sleep(1000);
                Start();
                #endregion
            }
            else
            {
                //不要生成excel数据，不要清空文本框，等用户第二次测试
                return;
            }
        }

        private void AlertMaxKpa()
        {
            Thread.Sleep(1000);
            MaxKpaAlertForm dlg = new MaxKpaAlertForm();
            dlg.ShowDialog();
            m_bMaxKpaFlag = false;
        }

        #region 是否通过 
        public bool IsPassAuto(List<LevelTips> strErrorList)
        {
            strErrorList.Clear();
            bool bPass = true; //总体有没有通过，只要有一个档位没有通过，则都不能通过
            ProductID pid = ProductIDConvertor.PumpID2ProductID(m_LocalPid);
            PressureConfig cfg = PressureManager.Instance().Get(pid);

            if (cfg == null)
            {
                Logger.Instance().FatalFormat("======IsPassAuto()函数调用出错，PressureConfig is null, 可能是泵类型转换错误。m_LocalPid={0}, pid={1}=====", m_LocalPid, pid);
                return false;
            }
            //单泵与双道泵的格式化信息不一样
            bool isSinglePump = true;
            string strFormat = SinglePumpLevelTips;
            if (m_LocalPid == PumpID.GrasebyF8 
                || m_LocalPid == PumpID.GrasebyF8_2
                || m_LocalPid == PumpID.GrasebyF6
                || m_LocalPid == PumpID.GrasebyF6_2
                || m_LocalPid == PumpID.WZS50F6
                || m_LocalPid == PumpID.WZS50F6_2
                )
            {
                strFormat = DoublePumpLevelTips;
                isSinglePump = false;
            }
            else
            {
                strFormat = SinglePumpLevelTips;
                isSinglePump = true;
            }

            var parameter = cfg.Find(Misc.OcclusionLevel.N);
            if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.N) && parameter != null)
            {
                bool currentPass = true; //当前档位是否通过测试
                string strError = string.Empty;
                Logger.Instance().InfoFormat("======IsPassAuto()生成N档信息 begin=====格式化字符串:{0}", strFormat);
                if (isSinglePump)
                    strError = string.Format(strFormat, "N", mFQCData.pressureN, parameter.Item2, parameter.Item3);
                else
                    strError = string.Format(strFormat, m_Channel, "N", mFQCData.pressureN, parameter.Item2, parameter.Item3);
                if (mFQCData.pressureN >= parameter.Item2 && mFQCData.pressureN <= parameter.Item3)
                    currentPass = true;
                else
                    currentPass = false;
                bPass = bPass && currentPass;
                strErrorList.Add(new LevelTips(currentPass, strError, 1));
                Logger.Instance().InfoFormat("======IsPassAuto()生成N档信息 end {0}", strError);
            }

            //读L档的压力参数在pressure.ini中
            parameter = cfg.Find(Misc.OcclusionLevel.L);
            if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.L) && parameter != null)
            {
                bool currentPass = true; //当前档位是否通过测试
                string strError = string.Empty;
                Logger.Instance().InfoFormat("======IsPassAuto()生成L档信息 begin=====格式化字符串:{0}", strFormat);
                if (isSinglePump)
                    strError = string.Format(strFormat, "L", mFQCData.pressureL, parameter.Item2, parameter.Item3);
                else
                    strError = string.Format(strFormat, m_Channel, "L", mFQCData.pressureL, parameter.Item2, parameter.Item3);
                if (mFQCData.pressureL >= parameter.Item2 && mFQCData.pressureL <= parameter.Item3)
                    currentPass = true;
                else
                    currentPass = false;
                bPass = bPass && currentPass;
                strErrorList.Add(new LevelTips(currentPass, strError, 2));
                Logger.Instance().InfoFormat("======IsPassAuto()生成L档信息 end {0}", strError);
            }

            //读C档的压力参数在pressure.ini中
            parameter = cfg.Find(Misc.OcclusionLevel.C);
            if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.C) && parameter != null)
            {
                bool currentPass = true; //当前档位是否通过测试
                string strError = string.Empty;
                Logger.Instance().InfoFormat("======IsPassAuto()生成C档信息 begin=====格式化字符串:{0}", strFormat);
                if (isSinglePump)
                    strError = string.Format(strFormat, "C", mFQCData.pressureC, parameter.Item2, parameter.Item3);
                else
                    strError = string.Format(strFormat, m_Channel, "C", mFQCData.pressureC, parameter.Item2, parameter.Item3);
                if (mFQCData.pressureC >= parameter.Item2 && mFQCData.pressureC <= parameter.Item3)
                    currentPass = true;
                else
                    currentPass = false;
                bPass = bPass && currentPass;
                strErrorList.Add(new LevelTips(currentPass, strError, 3));
                Logger.Instance().InfoFormat("======IsPassAuto()生成C档信息 end {0}", strError);
            }

            //读H档的压力参数在pressure.ini中
            parameter = cfg.Find(Misc.OcclusionLevel.H);
            if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.H) && parameter != null)
            {
                bool currentPass = true; //当前档位是否通过测试
                string strError = string.Empty;
                Logger.Instance().InfoFormat("======IsPassAuto()生成H档信息 begin=====格式化字符串:{0}", strFormat);
                if (isSinglePump)
                    strError = string.Format(strFormat, "H", mFQCData.pressureH, parameter.Item2, parameter.Item3);
                else
                    strError = string.Format(strFormat, m_Channel, "H", mFQCData.pressureH, parameter.Item2, parameter.Item3);
                if (mFQCData.pressureH >= parameter.Item2 && mFQCData.pressureH <= parameter.Item3)
                    currentPass = true;
                else
                    currentPass = false;
                bPass = bPass && currentPass;
                strErrorList.Add(new LevelTips(currentPass, strError, 4));
                Logger.Instance().InfoFormat("======IsPassAuto()生成H档信息 end {0}", strError);
            }
            return bPass;
        }

        public bool IsAuto()
        {
            if (cmbPattern.SelectedIndex == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 手动模式下只要判断1个档位
        /// </summary>
        /// <returns></returns>
        public bool IsPassManual(List<LevelTips> strErrorList)
        {
            strErrorList.Clear();
            bool bPass = true;
            ProductID pid = ProductIDConvertor.PumpID2ProductID(m_LocalPid);
            PressureConfig cfg = PressureManager.Instance().Get(pid);
            if (cfg == null)
            {
                Logger.Instance().FatalFormat("======IsPassManual()函数调用出错，PressureConfig is null, 可能是泵类型转换错误。m_LocalPid={0}, pid={1}=====", m_LocalPid, pid);
                return false;
            }
            //单泵与双道泵的格式化信息不一样
            bool isSinglePump = true;
            string strFormat = SinglePumpLevelTips;
            if (m_LocalPid == PumpID.GrasebyF8
                || m_LocalPid == PumpID.GrasebyF8_2
                || m_LocalPid == PumpID.GrasebyF6
                || m_LocalPid == PumpID.GrasebyF6_2
                || m_LocalPid == PumpID.WZS50F6
                || m_LocalPid == PumpID.WZS50F6_2
                )
            {
                strFormat = DoublePumpLevelTips;
                isSinglePump = false;
            }
            else
            {
                strFormat = SinglePumpLevelTips;
                isSinglePump = true;
            }
          
            var parameter = cfg.Find(m_CurrentLevel);
            if (parameter != null)
            {
                bool currentPass = true; //当前档位是否通过测试
                string strError = string.Empty;
                if (m_CurrentLevel == Misc.OcclusionLevel.N && m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.N))
                {
                    Logger.Instance().InfoFormat("======IsPassManual()生成N档信息 begin=====格式化字符串:{0}", strFormat);
                    if (isSinglePump)
                        strError = string.Format(strFormat, "N", mFQCData.pressureN, parameter.Item2, parameter.Item3);
                    else
                        strError = string.Format(strFormat, m_Channel, "N", mFQCData.pressureN, parameter.Item2, parameter.Item3);
                    if (mFQCData.pressureN >= parameter.Item2 && mFQCData.pressureN <= parameter.Item3)
                        currentPass = true;
                    else
                        currentPass = false;
                    bPass = bPass && currentPass;
                    Logger.Instance().InfoFormat("======IsPassAuto()生成N档信息 end {0}", strError);
                }
                else if (m_CurrentLevel == Misc.OcclusionLevel.L && m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.L))
                {
                    Logger.Instance().InfoFormat("======IsPassManual()生成L档信息 begin=====格式化字符串:{0}", strFormat);
                    if (isSinglePump)
                        strError = string.Format(strFormat, "L", mFQCData.pressureL, parameter.Item2, parameter.Item3);
                    else
                        strError = string.Format(strFormat, m_Channel, "L", mFQCData.pressureL, parameter.Item2, parameter.Item3);
                    if (mFQCData.pressureL >= parameter.Item2 && mFQCData.pressureL <= parameter.Item3)
                        currentPass = true;
                    else
                        currentPass = false;
                    bPass = bPass && currentPass;
                    Logger.Instance().InfoFormat("======IsPassAuto()生成L档信息 end {0}", strError);
                }
                else if (m_CurrentLevel == Misc.OcclusionLevel.C && m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.C))
                {
                    Logger.Instance().InfoFormat("======IsPassManual()生成C档信息 begin=====格式化字符串:{0}", strFormat);
                    if (isSinglePump)
                        strError = string.Format(strFormat, "C", mFQCData.pressureC, parameter.Item2, parameter.Item3);
                    else
                        strError = string.Format(strFormat, m_Channel, "C", mFQCData.pressureC, parameter.Item2, parameter.Item3);
                    if (mFQCData.pressureC >= parameter.Item2 && mFQCData.pressureC <= parameter.Item3)
                        currentPass = true;
                    else
                        currentPass = false;
                    bPass = bPass && currentPass;
                    Logger.Instance().InfoFormat("======IsPassAuto()生成C档信息 end {0}", strError);
                }
                else if (m_CurrentLevel == Misc.OcclusionLevel.H && m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.H))
                {
                    Logger.Instance().InfoFormat("======IsPassManual()生成H档信息 begin=====格式化字符串:{0}", strFormat);
                    if (isSinglePump)
                        strError = string.Format(strFormat, "H", mFQCData.pressureH, parameter.Item2, parameter.Item3);
                    else
                        strError = string.Format(strFormat, m_Channel, "H", mFQCData.pressureH, parameter.Item2, parameter.Item3);
                    if (mFQCData.pressureH >= parameter.Item2 && mFQCData.pressureH <= parameter.Item3)
                        currentPass = true;
                    else
                        currentPass = false;
                    bPass = bPass && currentPass;
                    Logger.Instance().InfoFormat("======IsPassAuto()生成H档信息 end {0}", strError);
                }

                int level = 1;
                switch(m_CurrentLevel)
                {
                    case Misc.OcclusionLevel.N:
                        level = 1;
                        break;
                    case Misc.OcclusionLevel.L:
                        level = 2;
                        break;
                    case Misc.OcclusionLevel.C:
                        level = 3;
                        break;
                    case Misc.OcclusionLevel.H:
                        level = 4;
                        break;
                    default:
                        level = 5;
                        break;
                }
                strErrorList.Add(new LevelTips(currentPass, strError, level));
            }
            return bPass;
        }
        #endregion

        private Misc.SyringeBrand FindSyringeBrandByName(string name)
        {
            Misc.SyringeBrand brand = Misc.SyringeBrand.None;
            try
            {
                KeyValuePair<Misc.SyringeBrand, String> brandLevel = m_SyringeBrands.First<KeyValuePair<Misc.SyringeBrand, String>>((x) => { return x.Value == name; });
                brand = brandLevel.Key;
            }
            catch
            {
                brand = Misc.SyringeBrand.None;
            }
            return brand;
        }

        private void PumpCloseConnectionSub()
        {
            Thread.Sleep(2000);
            Thread th = new Thread(Close);
            th.Start();
        }

        private void AlertTestResultSub()
        {
            Thread th = new Thread(AlertTestResult);
            th.Start();
        }

        private void AlertMaxKpaSub()
        {
            Thread th = new Thread(AlertMaxKpa);
            th.Start();
        }

        private void StopTest()
        {
            StopTimer();
            StopTimerGauge();
            m_ConnResponse.ClearAllCommand();
            InvokeOnClick(btnStopAlarm, null);
            ContinueStopTest();
        }

        private void StopTestWithoutStartPump()
        {
            StopTimer();
            StopTimerGauge();
            m_RequestCommands.Clear();
            m_ConnResponse.ClearAllCommand();
            Close();
            EnableContols(true);
        }

        private void ContinueStopTest()
        {
            if (InvokeRequired)
            {
                Invoke(new DelegateContinueStopTest(ContinueStopTest), null);
                return;
            }
            Logger.Instance().Info("======ContinueStopTest()函数调用 begin=====");

            lock (m_RequestCommands)
            {
                m_RequestCommands.Clear();
            }
            m_GaugeTool.Close();
            EnableContols(true);
            //自动模式下，完成测试后要归到最低档
            if (cmbPattern.SelectedIndex == 0)
                cmbLevel.SelectedIndex = 0;
            if (m_LocalPid == PumpID.GrasebyF8_2 || m_LocalPid == PumpID.GrasebyF6_2 || m_LocalPid == PumpID.WZS50F6_2)
            {
                //第一道测试完成，判断是否合格，不合格要提示，是否重测
                string strError = "";
                List<LevelTips> strErrorList = new List<LevelTips>();
                if (IsAuto())
                {
                   if( IsPassAuto(strErrorList) )
                       OnSamplingComplete(this, new DoublePumpDataArgs(strErrorList, mFQCData, true));
                   else
                   {
                       OnSamplingComplete(this, new DoublePumpDataArgs(strErrorList, mFQCData, false));
                   }
                }
                else
                {
                    if (IsPassManual(strErrorList))
                       OnSamplingComplete(this, new DoublePumpDataArgs(strErrorList, mFQCData, true));
                    else
                    {
                       OnSamplingComplete(this, new DoublePumpDataArgs(strErrorList, mFQCData, false));
                    }
                }
            }
            else
            {
                //不能在这里生成excel，等弹出框弹出，由用户选择是否生成
                //Logger.Instance().Info("======ContinueStopTest() 生成压力数据文件 begin=====");

                //var pid = ProductIDConvertor.PumpID2ProductID(m_LocalPid);
                ////string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(PressureForm)).Location) + "\\数据导出";
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data";
                //string fileName = string.Format("{0}{1}{2}", pid.ToString(), m_PumpNo, DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss"));
                //if (!System.IO.Directory.Exists(path))
                //    System.IO.Directory.CreateDirectory(path);
                //string saveFileName = path + "\\" + fileName + ".xlsx";

                //string path2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\压力数据Pressure Data\\Data Copy";
                //if (!System.IO.Directory.Exists(path2))
                //    System.IO.Directory.CreateDirectory(path2);
                //string saveFileName2 = path2 + "\\" + fileName + ".xlsx";
                //try
                //{
                //    GenReport(saveFileName, saveFileName2);
                //}
                //catch(Exception ex)
                //{
                //    Logger.Instance().ErrorFormat("======ContinueStopTest() 生成压力数据文件出错,错误信息={0}", ex.Message);
                //}
                //if (ClearPumpNoWhenCompleteTest != null)
                //{
                //    ClearPumpNoWhenCompleteTest(this, null);
                //}

                //Logger.Instance().Info("======ContinueStopTest() 生成压力数据文件 end=====");

                AlertTestResultSub();
                PumpCloseConnectionSub();
            }
            Logger.Instance().Info("======ContinueStopTest()函数调用 end=====");
        }

        private void BeginStopTestThread()
        {
            Thread th = new Thread(StopTest);
            th.Start();
        }

        //private void StopPump()
        //{
        //    UserMessageHelper.SendStopPumpMessage();
        //    Thread.Sleep(500);
        //}

        //private void BeginStopPumpThread()
        //{
        //    Thread th = new Thread(StopPump);
        //    th.Start();
        //}

        /// <summary>
        /// 如果报警一直响，不能创建多个线程，只要一个线程等待，泵的状态返回就可以了
        /// </summary>
        private void BeginPauseTestThread()
        {
            if (m_bStartPumpStatusThread)
            {
                m_bStartPumpStatusThread = false;
                m_StopPumpPumpStatusEvent.Reset();
                Thread th = new Thread(PauseTest);
                th.Start();
            }
        }

        private void PauseTest()
        {
            //if (this.InvokeRequired)
            //{
            //    this.BeginInvoke(new DelegatePauseTest(PauseTest), null);
            //    return;
            //}
            
           
            bool hasStoped = false;
            int iTryStopCount = 3;
            do
            {
                m_ConnResponse.GetPumpStatus();
                if (m_StopPumpPumpStatusEvent.WaitOne(3000))
                {
                    hasStoped = true;
                    break;
                }
                else
                {
                    
                }
            } while (--iTryStopCount > 0);

            m_bStartPumpStatusThread = true;



            StopTimer();
            StopTimerGauge();

            lock (m_RequestCommands)
            {
                m_RequestCommands.Clear();
                m_ConnResponse.ClearAllCommand();
            }

           
            float max = FindMaxPressure();
            switch (m_CurrentLevel)
            {
                case Misc.OcclusionLevel.N:
                    if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.N))
                        mFQCData.pressureN = max;
                    else
                        mFQCData.pressureN = -1;
                    break;
                case Misc.OcclusionLevel.L:
                    if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.L))
                        mFQCData.pressureL = max;
                    else
                        mFQCData.pressureL = -1;
                    break;
                case Misc.OcclusionLevel.C:
                    if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.C))
                        mFQCData.pressureC = max;
                    else
                        mFQCData.pressureC = -1;
                    break;
                case Misc.OcclusionLevel.H:
                    if (m_OcclusionLevelOfBrand.Contains(Misc.OcclusionLevel.H))
                        mFQCData.pressureH = max;
                    else
                        mFQCData.pressureH = -1;
                    break;
                default: break;
            }
            mFQCData.Channel = m_Channel;
            mFQCData.rate = float.Parse(tbRate.Text);
            //如果是自动模式下
            if(m_CuurentPatterIndex == 0)
            {
                if (m_CurrentLevel == Misc.OcclusionLevel.H)
                {
                    Logger.Instance().InfoFormat("PauseTest()-> 泵即将停止,当前的压力档为={0}", m_CurrentLevel);
                    this.detail.SetFQCResult(mFQCData);
                    BeginStopTestThread();
                }
                else
                {
                    if (hasStoped)
                    {
                        Logger.Instance().InfoFormat("BeginStopPumpThread()-> 线程停止泵成功,当前的压力档为={0}", m_CurrentLevel);
                        //不管有没有停止成功，最好还是直接设置下一档位
                        //cmbLevel.SelectedIndex = cmbLevel.SelectedIndex + 1;
                        SetPumpOcclusion();
                        m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetOcclusionLevel);
                        m_RequestCommands.Enqueue(Misc.ApplicationRequestCommand.SetStartControl);
                        SendNextRequest();
                    }
                    else
                    {
                        Logger.Instance().ErrorFormat("BeginStopPumpThread()-> 线程停止泵失败,当前的压力档为={0}", m_CurrentLevel);
                    }
                }
            }
            else
            {
                this.detail.SetFQCResult(mFQCData);
                BeginStopTestThread();
            }

        }

        private float FindMaxPressure()
        {
           if (m_Ch1SampleDataList.Count == 0)
                return 0;
           float max = 0;
           lock (m_Ch1SampleDataList)
           {
               try
               {
                  max = m_Ch1SampleDataList.Max(x => { return x.m_PressureValue; });
               }
               catch(Exception e)
               {
                   Logger.Instance().Error("FindMaxPressure Error:" + e.Message);
               }
           }
           return max;
        }

        /// <summary>
        /// 第二道泵串口联动
        /// </summary>
        /// <param name="index"></param>
        public void SyncPort4F8DualChannel(int index)
        {
            this.cbPumpPort.SelectedIndex = index;
            SetPumpPortStatus(true);
        }

        public int GetPortIndex4F8()
        {
            return cbPumpPort.SelectedIndex;
        }

        public void Close()
        {
            StopTimer();
            StopTimerGauge();
            //m_Ch1SampleDataList.Clear();
            //mFQCData.Clear();
            if (m_ConnResponse != null)
                m_ConnResponse.AbortConnection();
            if (m_GaugeTool != null)
                m_GaugeTool.Close();
        }

        public void ClearTestData()
        {
            m_Ch1SampleDataList.Clear();
            mFQCData.Clear();
            WavelinePanel.Invalidate();
        }

        private void tbOprator_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;                         //让操作生效
                if (txt.Text.Length >= 8)
                {
                    if (e.KeyChar == (char)Keys.Back)
                        e.Handled = false;             //让操作生效
                    else
                        e.Handled = true;              //让操作失效，如果第一个字符是2以上，不能输入其他字符
                }
                else
                {
                    e.Handled = false;                 //让操作生效
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void tbOprator_TextChanged(object sender, EventArgs e)
        {
            if (this.Channel == 1)
            {
                if (OpratorNumberInput != null)
                {
                    //OpratorNumberInput(this, new OpratorNumberArgs("操作员编号"));
                }
            }
        }

        public void InputOpratorNumber(string number)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DelegateInputOpratorNumber(InputOpratorNumber), new object[] { number });
            }
            else
            {
                //this.tbOprator.Text = number;
            }
        }

        public string GetOpratorNumber()
        {
            return Operator;
        }

        public void SetDefaultRate()
        {
            switch(m_ProductModel)
            {
                case ProductModel.GrasebyC8:
                case ProductModel.GrasebyF8:
                    tbRate.Text = "210";
                    break;
                case ProductModel.GrasebyC6:
                case ProductModel.GrasebyF6:
                case ProductModel.Graseby1200:
                case ProductModel.GrasebyC6T:
                case ProductModel.Graseby2000:
                case ProductModel.Graseby2100:
                case ProductModel.WZ50C6:
                case ProductModel.WZS50F6:
                case ProductModel.WZ50C6T:
                    tbRate.Text = "300";
                    break;
                default:
                    tbRate.Text = "450";
                    break;
            }
        }

        public void SetDefaultBrand()
        {
            if (cmbSetBrand.Items.Count > 4)
            {
                switch (m_ProductModel)
                {
                    case ProductModel.GrasebyC8:
                    case ProductModel.GrasebyF8:
                        cmbSetBrand.SelectedIndex = 3;
                        break;
                    case ProductModel.GrasebyC6:
                    case ProductModel.GrasebyF6:
                    case ProductModel.Graseby1200:
                    case ProductModel.GrasebyC6T:
                    case ProductModel.Graseby2000:
                    case ProductModel.Graseby2100:
                    case ProductModel.WZ50C6:
                    case ProductModel.WZS50F6:
                    case ProductModel.WZ50C6T:
                        cmbSetBrand.SelectedIndex = 2;
                        break;
                    default:
                        cmbSetBrand.SelectedIndex = 3;
                        break;
                }
            }
        }

        /// <summary>
        /// 消息驱动界面线程停止泵，因为其他线程无法停止。
        /// </summary>
        public void StopPumpByOcclusionSwitch()
        {
            //if(m_ConnResponse!=null && m_ConnResponse.IsOpen())
            //    m_ConnResponse.SetStopControl(GlobalResponse.CommandPriority.High);
        }

        public void SetPumpOcclusion()
        {
            if(this.InvokeRequired)
            {
                this.Invoke(new DelegateFuctionNoParamater(SetPumpOcclusion), null);
                return;
            }
            if(cmbLevel.SelectedIndex + 1 <= cmbLevel.Items.Count-1)
                cmbLevel.SelectedIndex = cmbLevel.SelectedIndex + 1;
        }


        private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CuurentPatterIndex = cmbPattern.SelectedIndex;
        }

        private void btnStopAlarm_Click(object sender, EventArgs e)
        {
            //m_StopPumpPumpStatusEvent.Reset();
            if (m_ConnResponse != null && m_ConnResponse.IsOpen())
            {
                m_ConnResponse.SetStopControl(GlobalResponse.CommandPriority.High);
                Logger.Instance().Info("btnStopAlarm_Click()调用成功");
            }
          

        }
    }
}
