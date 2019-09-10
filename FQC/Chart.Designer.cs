namespace FQC
{
    partial class Chart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpChannel = new System.Windows.Forms.TableLayoutPanel();
            this.picChannel = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbToolingPort = new System.Windows.Forms.ComboBox();
            this.cbPumpPort = new System.Windows.Forms.ComboBox();
            this.tbRate = new System.Windows.Forms.TextBox();
            this.cmbSetBrand = new System.Windows.Forms.ComboBox();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.picPumpPortStatus = new System.Windows.Forms.PictureBox();
            this.picGaugePortStatus = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbPattern = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbOprator = new System.Windows.Forms.TextBox();
            this.btnStopAlarm = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.picStart = new System.Windows.Forms.PictureBox();
            this.picStop = new System.Windows.Forms.PictureBox();
            this.picDetail = new System.Windows.Forms.PictureBox();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.WavelinePanel = new System.Windows.Forms.Panel();
            this.detail = new FQC.Detail();
            this.tlpMain.SuspendLayout();
            this.tlpChannel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChannel)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPumpPortStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGaugePortStatus)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDetail)).BeginInit();
            this.pnlChart.SuspendLayout();
            this.WavelinePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84F));
            this.tlpMain.Controls.Add(this.tlpChannel, 0, 0);
            this.tlpMain.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tlpMain.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tlpMain.Controls.Add(this.pnlChart, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpMain.Size = new System.Drawing.Size(600, 595);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpChannel
            // 
            this.tlpChannel.ColumnCount = 2;
            this.tlpChannel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChannel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChannel.Controls.Add(this.picChannel, 0, 0);
            this.tlpChannel.Controls.Add(this.label1, 1, 0);
            this.tlpChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChannel.Location = new System.Drawing.Point(0, 0);
            this.tlpChannel.Margin = new System.Windows.Forms.Padding(0);
            this.tlpChannel.Name = "tlpChannel";
            this.tlpChannel.RowCount = 2;
            this.tlpChannel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChannel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChannel.Size = new System.Drawing.Size(96, 178);
            this.tlpChannel.TabIndex = 0;
            // 
            // picChannel
            // 
            this.picChannel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picChannel.Image = global::FQC.Properties.Resources.icon_1;
            this.picChannel.Location = new System.Drawing.Point(7, 64);
            this.picChannel.Name = "picChannel";
            this.tlpChannel.SetRowSpan(this.picChannel, 2);
            this.picChannel.Size = new System.Drawing.Size(34, 50);
            this.picChannel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picChannel.TabIndex = 0;
            this.picChannel.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(51, 69);
            this.label1.Name = "label1";
            this.tlpChannel.SetRowSpan(this.label1, 2);
            this.label1.Size = new System.Drawing.Size(25, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "道\r\n泵";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbToolingPort, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbPumpPort, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbRate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbSetBrand, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbLevel, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.picPumpPortStatus, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.picGaugePortStatus, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbPattern, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbOprator, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnStopAlarm, 4, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(96, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 178);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(0, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "工装串口";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(332, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "泵串口";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(0, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "品牌";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label7.Location = new System.Drawing.Point(0, 100);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 1;
            this.label7.Text = "速率";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label8.Location = new System.Drawing.Point(284, 100);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "mL/h";
            // 
            // cbToolingPort
            // 
            this.cbToolingPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbToolingPort.BackColor = System.Drawing.Color.White;
            this.cbToolingPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.cbToolingPort.FormattingEnabled = true;
            this.cbToolingPort.Location = new System.Drawing.Point(75, 10);
            this.cbToolingPort.Margin = new System.Windows.Forms.Padding(0);
            this.cbToolingPort.Name = "cbToolingPort";
            this.cbToolingPort.Size = new System.Drawing.Size(209, 24);
            this.cbToolingPort.TabIndex = 2;
            this.cbToolingPort.Tag = "";
            this.cbToolingPort.SelectedIndexChanged += new System.EventHandler(this.cbToolingPort_SelectedIndexChanged);
            // 
            // cbPumpPort
            // 
            this.cbPumpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPumpPort.BackColor = System.Drawing.Color.White;
            this.cbPumpPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.cbPumpPort.FormattingEnabled = true;
            this.cbPumpPort.Location = new System.Drawing.Point(394, 10);
            this.cbPumpPort.Margin = new System.Windows.Forms.Padding(0);
            this.cbPumpPort.Name = "cbPumpPort";
            this.cbPumpPort.Size = new System.Drawing.Size(80, 24);
            this.cbPumpPort.TabIndex = 2;
            this.cbPumpPort.SelectedIndexChanged += new System.EventHandler(this.cbPumpPort_SelectedIndexChanged);
            // 
            // tbRate
            // 
            this.tbRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRate.BackColor = System.Drawing.Color.White;
            this.tbRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.tbRate.Location = new System.Drawing.Point(75, 97);
            this.tbRate.Margin = new System.Windows.Forms.Padding(0);
            this.tbRate.Name = "tbRate";
            this.tbRate.Size = new System.Drawing.Size(209, 26);
            this.tbRate.TabIndex = 3;
            this.tbRate.Text = "300";
            // 
            // cmbSetBrand
            // 
            this.cmbSetBrand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSetBrand.BackColor = System.Drawing.Color.White;
            this.cmbSetBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.cmbSetBrand.FormattingEnabled = true;
            this.cmbSetBrand.Location = new System.Drawing.Point(75, 54);
            this.cmbSetBrand.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSetBrand.Name = "cmbSetBrand";
            this.cmbSetBrand.Size = new System.Drawing.Size(209, 24);
            this.cmbSetBrand.TabIndex = 2;
            this.cmbSetBrand.SelectedIndexChanged += new System.EventHandler(this.cmbSetBrand_SelectedIndexChanged);
            // 
            // cmbLevel
            // 
            this.cmbLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLevel.BackColor = System.Drawing.Color.White;
            this.cmbLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(394, 54);
            this.cmbLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(80, 24);
            this.cmbLevel.TabIndex = 2;
            this.cmbLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLevel_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(332, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "压力";
            // 
            // picPumpPortStatus
            // 
            this.picPumpPortStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.picPumpPortStatus.Location = new System.Drawing.Point(474, 0);
            this.picPumpPortStatus.Margin = new System.Windows.Forms.Padding(0);
            this.picPumpPortStatus.Name = "picPumpPortStatus";
            this.picPumpPortStatus.Size = new System.Drawing.Size(30, 44);
            this.picPumpPortStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPumpPortStatus.TabIndex = 4;
            this.picPumpPortStatus.TabStop = false;
            this.picPumpPortStatus.Visible = false;
            // 
            // picGaugePortStatus
            // 
            this.picGaugePortStatus.Dock = System.Windows.Forms.DockStyle.Left;
            this.picGaugePortStatus.Image = global::FQC.Properties.Resources.error;
            this.picGaugePortStatus.InitialImage = global::FQC.Properties.Resources.error;
            this.picGaugePortStatus.Location = new System.Drawing.Point(284, 0);
            this.picGaugePortStatus.Margin = new System.Windows.Forms.Padding(0);
            this.picGaugePortStatus.Name = "picGaugePortStatus";
            this.picGaugePortStatus.Size = new System.Drawing.Size(30, 44);
            this.picGaugePortStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picGaugePortStatus.TabIndex = 4;
            this.picGaugePortStatus.TabStop = false;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label6.Location = new System.Drawing.Point(332, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "模式";
            // 
            // cmbPattern
            // 
            this.cmbPattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPattern.BackColor = System.Drawing.Color.White;
            this.cmbPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.cmbPattern.FormattingEnabled = true;
            this.cmbPattern.Items.AddRange(new object[] {
            "自动",
            "手动"});
            this.cmbPattern.Location = new System.Drawing.Point(394, 98);
            this.cmbPattern.Margin = new System.Windows.Forms.Padding(0);
            this.cmbPattern.Name = "cmbPattern";
            this.cmbPattern.Size = new System.Drawing.Size(80, 24);
            this.cmbPattern.TabIndex = 2;
            this.cmbPattern.SelectedIndexChanged += new System.EventHandler(this.cmbPattern_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label9.Location = new System.Drawing.Point(0, 145);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "操作员";
            // 
            // tbOprator
            // 
            this.tbOprator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOprator.BackColor = System.Drawing.Color.White;
            this.tbOprator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.tbOprator.Location = new System.Drawing.Point(75, 142);
            this.tbOprator.Margin = new System.Windows.Forms.Padding(0);
            this.tbOprator.Name = "tbOprator";
            this.tbOprator.Size = new System.Drawing.Size(209, 26);
            this.tbOprator.TabIndex = 3;
            this.tbOprator.Text = "12345678";
            this.tbOprator.TextChanged += new System.EventHandler(this.tbOprator_TextChanged);
            this.tbOprator.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbOprator_KeyPress);
            // 
            // btnStopAlarm
            // 
            this.btnStopAlarm.Location = new System.Drawing.Point(397, 135);
            this.btnStopAlarm.Name = "btnStopAlarm";
            this.btnStopAlarm.Size = new System.Drawing.Size(74, 23);
            this.btnStopAlarm.TabIndex = 5;
            this.btnStopAlarm.UseVisualStyleBackColor = true;
            this.btnStopAlarm.Visible = false;
            this.btnStopAlarm.Click += new System.EventHandler(this.btnStopAlarm_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tlpMain.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.picStart, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.picStop, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.picDetail, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 535);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(600, 60);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // picStart
            // 
            this.picStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picStart.Image = global::FQC.Properties.Resources.icon_start_Blue;
            this.picStart.Location = new System.Drawing.Point(74, 5);
            this.picStart.Name = "picStart";
            this.picStart.Size = new System.Drawing.Size(51, 50);
            this.picStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStart.TabIndex = 0;
            this.picStart.TabStop = false;
            this.picStart.Click += new System.EventHandler(this.picStart_Click);
            // 
            // picStop
            // 
            this.picStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picStop.Enabled = false;
            this.picStop.Image = global::FQC.Properties.Resources.icon_stop_gray;
            this.picStop.Location = new System.Drawing.Point(274, 5);
            this.picStop.Name = "picStop";
            this.picStop.Size = new System.Drawing.Size(51, 50);
            this.picStop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStop.TabIndex = 0;
            this.picStop.TabStop = false;
            this.picStop.Click += new System.EventHandler(this.picStop_Click);
            // 
            // picDetail
            // 
            this.picDetail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picDetail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picDetail.Image = global::FQC.Properties.Resources.icon_tablelist_blue;
            this.picDetail.Location = new System.Drawing.Point(474, 5);
            this.picDetail.Name = "picDetail";
            this.picDetail.Size = new System.Drawing.Size(51, 50);
            this.picDetail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDetail.TabIndex = 0;
            this.picDetail.TabStop = false;
            this.picDetail.Click += new System.EventHandler(this.picDetail_Click);
            // 
            // pnlChart
            // 
            this.pnlChart.BackColor = System.Drawing.Color.White;
            this.tlpMain.SetColumnSpan(this.pnlChart, 2);
            this.pnlChart.Controls.Add(this.WavelinePanel);
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.pnlChart.ForeColor = System.Drawing.Color.Black;
            this.pnlChart.Location = new System.Drawing.Point(0, 178);
            this.pnlChart.Margin = new System.Windows.Forms.Padding(0);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(600, 357);
            this.pnlChart.TabIndex = 5;
            // 
            // WavelinePanel
            // 
            this.WavelinePanel.BackColor = System.Drawing.Color.White;
            this.WavelinePanel.Controls.Add(this.detail);
            this.WavelinePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WavelinePanel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.WavelinePanel.ForeColor = System.Drawing.Color.Black;
            this.WavelinePanel.Location = new System.Drawing.Point(0, 0);
            this.WavelinePanel.Margin = new System.Windows.Forms.Padding(0);
            this.WavelinePanel.Name = "WavelinePanel";
            this.WavelinePanel.Size = new System.Drawing.Size(600, 357);
            this.WavelinePanel.TabIndex = 5;
            this.WavelinePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.WavelinePanel_Paint);
            // 
            // detail
            // 
            this.detail.BackColor = System.Drawing.Color.Purple;
            this.detail.Channel = 1;
            this.detail.Location = new System.Drawing.Point(74, 154);
            this.detail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.detail.Name = "detail";
            this.detail.Pid = FQC.PumpID.GrasebyC6;
            this.detail.Size = new System.Drawing.Size(460, 143);
            this.detail.SyrangeSize = 50;
            this.detail.TabIndex = 0;
            this.detail.Visible = false;
            // 
            // Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpMain);
            this.Name = "Chart";
            this.Size = new System.Drawing.Size(600, 595);
            this.Load += new System.EventHandler(this.Chart_Load);
            this.EnabledChanged += new System.EventHandler(this.Chart_EnabledChanged);
            this.tlpMain.ResumeLayout(false);
            this.tlpChannel.ResumeLayout(false);
            this.tlpChannel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picChannel)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPumpPortStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGaugePortStatus)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDetail)).EndInit();
            this.pnlChart.ResumeLayout(false);
            this.WavelinePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpChannel;
        private System.Windows.Forms.PictureBox picChannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbToolingPort;
        private System.Windows.Forms.ComboBox cbPumpPort;
        private System.Windows.Forms.TextBox tbRate;
        private System.Windows.Forms.Panel WavelinePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox picStart;
        private System.Windows.Forms.PictureBox picStop;
        private System.Windows.Forms.PictureBox picDetail;
        private System.Windows.Forms.Panel pnlChart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.PictureBox picPumpPortStatus;
        private System.Windows.Forms.PictureBox picGaugePortStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbPattern;
        private Detail detail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbOprator;
        public System.Windows.Forms.ComboBox cmbSetBrand;
        private System.Windows.Forms.Button btnStopAlarm;
    }
}
