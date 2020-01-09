namespace FQC
{
    partial class PressureForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PressureForm));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.picSetting = new System.Windows.Forms.PictureBox();
            this.picCloseWindow = new System.Windows.Forms.PictureBox();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.tbPumpNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPumpType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbOprator = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tlpChart = new System.Windows.Forms.TableLayoutPanel();
            this.picSplit = new System.Windows.Forms.PictureBox();
            this.chart2 = new FQC.Chart();
            this.chart1 = new FQC.Chart();
            this.tlpMain.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow)).BeginInit();
            this.tlpParameter.SuspendLayout();
            this.tlpChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSplit)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpTitle, 0, 0);
            this.tlpMain.Controls.Add(this.tlpParameter, 0, 1);
            this.tlpMain.Controls.Add(this.tlpChart, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84F));
            this.tlpMain.Size = new System.Drawing.Size(1024, 730);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpTitle
            // 
            this.tlpTitle.ColumnCount = 5;
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpTitle.Controls.Add(this.picLogo, 0, 0);
            this.tlpTitle.Controls.Add(this.lbTitle, 1, 0);
            this.tlpTitle.Controls.Add(this.picSetting, 3, 0);
            this.tlpTitle.Controls.Add(this.picCloseWindow, 4, 0);
            this.tlpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitle.Location = new System.Drawing.Point(0, 0);
            this.tlpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitle.Name = "tlpTitle";
            this.tlpTitle.RowCount = 1;
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.Size = new System.Drawing.Size(1024, 43);
            this.tlpTitle.TabIndex = 0;
            this.tlpTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tlpTitle_MouseDown);
            this.tlpTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tlpTitle_MouseMove);
            this.tlpTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tlpTitle_MouseUp);
            // 
            // picLogo
            // 
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Image = global::FQC.Properties.Resources.fqc;
            this.picLogo.Location = new System.Drawing.Point(5, 5);
            this.picLogo.Margin = new System.Windows.Forms.Padding(5);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(40, 33);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(53, 12);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(130, 18);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "FQC压力检验1.0";
            // 
            // picSetting
            // 
            this.picSetting.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.picSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSetting.Image = global::FQC.Properties.Resources.icon_setting;
            this.picSetting.Location = new System.Drawing.Point(943, 9);
            this.picSetting.Margin = new System.Windows.Forms.Padding(9);
            this.picSetting.Name = "picSetting";
            this.picSetting.Size = new System.Drawing.Size(27, 25);
            this.picSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSetting.TabIndex = 2;
            this.picSetting.TabStop = false;
            this.picSetting.Visible = false;
            this.picSetting.Click += new System.EventHandler(this.picSetting_Click);
            // 
            // picCloseWindow
            // 
            this.picCloseWindow.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.picCloseWindow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCloseWindow.Image = global::FQC.Properties.Resources.close;
            this.picCloseWindow.Location = new System.Drawing.Point(990, 11);
            this.picCloseWindow.Margin = new System.Windows.Forms.Padding(11);
            this.picCloseWindow.Name = "picCloseWindow";
            this.picCloseWindow.Size = new System.Drawing.Size(23, 21);
            this.picCloseWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCloseWindow.TabIndex = 3;
            this.picCloseWindow.TabStop = false;
            this.picCloseWindow.Click += new System.EventHandler(this.picCloseWindow_Click);
            // 
            // tlpParameter
            // 
            this.tlpParameter.BackColor = System.Drawing.Color.Purple;
            this.tlpParameter.ColumnCount = 7;
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.48193F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.73494F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.04819F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpParameter.Controls.Add(this.tbPumpNo, 4, 0);
            this.tlpParameter.Controls.Add(this.label2, 3, 0);
            this.tlpParameter.Controls.Add(this.cbPumpType, 2, 0);
            this.tlpParameter.Controls.Add(this.label1, 1, 0);
            this.tlpParameter.Controls.Add(this.label5, 0, 0);
            this.tlpParameter.Controls.Add(this.tbOprator, 6, 0);
            this.tlpParameter.Controls.Add(this.label3, 5, 0);
            this.tlpParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpParameter.Location = new System.Drawing.Point(0, 43);
            this.tlpParameter.Margin = new System.Windows.Forms.Padding(0);
            this.tlpParameter.Name = "tlpParameter";
            this.tlpParameter.RowCount = 1;
            this.tlpParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpParameter.Size = new System.Drawing.Size(1024, 73);
            this.tlpParameter.TabIndex = 1;
            // 
            // tbPumpNo
            // 
            this.tbPumpNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPumpNo.BackColor = System.Drawing.Color.Purple;
            this.tbPumpNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.tbPumpNo.ForeColor = System.Drawing.Color.White;
            this.tbPumpNo.Location = new System.Drawing.Point(467, 24);
            this.tbPumpNo.Margin = new System.Windows.Forms.Padding(0);
            this.tbPumpNo.Name = "tbPumpNo";
            this.tbPumpNo.Size = new System.Drawing.Size(345, 24);
            this.tbPumpNo.TabIndex = 3;
            this.tbPumpNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPumpNo_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(390, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "产品序号";
            // 
            // cbPumpType
            // 
            this.cbPumpType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPumpType.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.cbPumpType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPumpType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.cbPumpType.ForeColor = System.Drawing.Color.White;
            this.cbPumpType.FormattingEnabled = true;
            this.cbPumpType.Location = new System.Drawing.Point(172, 23);
            this.cbPumpType.Margin = new System.Windows.Forms.Padding(0);
            this.cbPumpType.Name = "cbPumpType";
            this.cbPumpType.Size = new System.Drawing.Size(209, 26);
            this.cbPumpType.TabIndex = 2;
            this.cbPumpType.SelectedIndexChanged += new System.EventHandler(this.cbPumpType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(95, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "机器型号";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(9, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 18);
            this.label5.TabIndex = 1;
            this.label5.Text = "参数设置";
            // 
            // tbOprator
            // 
            this.tbOprator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOprator.BackColor = System.Drawing.Color.White;
            this.tbOprator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.tbOprator.Location = new System.Drawing.Point(898, 23);
            this.tbOprator.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.tbOprator.Name = "tbOprator";
            this.tbOprator.Size = new System.Drawing.Size(116, 26);
            this.tbOprator.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(828, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "操作员";
            // 
            // tlpChart
            // 
            this.tlpChart.ColumnCount = 3;
            this.tlpChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpChart.Controls.Add(this.picSplit, 1, 0);
            this.tlpChart.Controls.Add(this.chart2, 2, 0);
            this.tlpChart.Controls.Add(this.chart1, 0, 0);
            this.tlpChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChart.Location = new System.Drawing.Point(0, 116);
            this.tlpChart.Margin = new System.Windows.Forms.Padding(0);
            this.tlpChart.Name = "tlpChart";
            this.tlpChart.RowCount = 1;
            this.tlpChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpChart.Size = new System.Drawing.Size(1024, 614);
            this.tlpChart.TabIndex = 2;
            // 
            // picSplit
            // 
            this.picSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.picSplit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picSplit.Location = new System.Drawing.Point(510, 0);
            this.picSplit.Margin = new System.Windows.Forms.Padding(0);
            this.picSplit.Name = "picSplit";
            this.picSplit.Size = new System.Drawing.Size(3, 614);
            this.picSplit.TabIndex = 2;
            this.picSplit.TabStop = false;
            // 
            // chart2
            // 
            this.chart2.BackColor = System.Drawing.Color.White;
            this.chart2.Channel = 1;
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart2.Enabled = false;
            this.chart2.Location = new System.Drawing.Point(516, 3);
            this.chart2.Name = "chart2";
            this.chart2.Operator = "";
            this.chart2.PumpNo = "";
            this.chart2.SampleInterval = 500;
            this.chart2.Size = new System.Drawing.Size(505, 608);
            this.chart2.TabIndex = 3;
            this.chart2.ToolingNo = "";
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.White;
            this.chart1.Channel = 1;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Enabled = false;
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            this.chart1.Operator = "";
            this.chart1.PumpNo = "";
            this.chart1.SampleInterval = 500;
            this.chart1.Size = new System.Drawing.Size(504, 608);
            this.chart1.TabIndex = 3;
            this.chart1.ToolingNo = "";
            // 
            // PressureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 730);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PressureForm";
            this.Text = "FQC压力检验工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PressureForm_FormClosing);
            this.Load += new System.EventHandler(this.PressureForm_Load);
            this.tlpMain.ResumeLayout(false);
            this.tlpTitle.ResumeLayout(false);
            this.tlpTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseWindow)).EndInit();
            this.tlpParameter.ResumeLayout(false);
            this.tlpParameter.PerformLayout();
            this.tlpChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSplit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpTitle;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox picSetting;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPumpType;
        private System.Windows.Forms.TextBox tbPumpNo;
        private System.Windows.Forms.TableLayoutPanel tlpChart;
        private System.Windows.Forms.PictureBox picSplit;
        private System.Windows.Forms.PictureBox picCloseWindow;
        private System.Windows.Forms.Label label5;
        private Chart chart2;
        private Chart chart1;
        private System.Windows.Forms.TextBox tbOprator;
        private System.Windows.Forms.Label label3;
    }
}