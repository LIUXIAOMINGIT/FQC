namespace FQC
{
    partial class Detail
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
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbBrandValue = new System.Windows.Forms.Label();
            this.lbSizeValue = new System.Windows.Forms.Label();
            this.lbNValue = new System.Windows.Forms.Label();
            this.lbLValue = new System.Windows.Forms.Label();
            this.lbCValue = new System.Windows.Forms.Label();
            this.lbHValue = new System.Windows.Forms.Label();
            this.lbH = new System.Windows.Forms.Label();
            this.lbC = new System.Windows.Forms.Label();
            this.lbL = new System.Windows.Forms.Label();
            this.lbN = new System.Windows.Forms.Label();
            this.lbSize = new System.Windows.Forms.Label();
            this.lbBrand = new System.Windows.Forms.Label();
            this.tlpMain.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tlpMain.ColumnCount = 6;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlpMain.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpMain.Controls.Add(this.lbBrandValue, 0, 2);
            this.tlpMain.Controls.Add(this.lbSizeValue, 1, 2);
            this.tlpMain.Controls.Add(this.lbNValue, 2, 2);
            this.tlpMain.Controls.Add(this.lbLValue, 3, 2);
            this.tlpMain.Controls.Add(this.lbCValue, 4, 2);
            this.tlpMain.Controls.Add(this.lbHValue, 5, 2);
            this.tlpMain.Controls.Add(this.lbH, 5, 1);
            this.tlpMain.Controls.Add(this.lbC, 4, 1);
            this.tlpMain.Controls.Add(this.lbL, 3, 1);
            this.tlpMain.Controls.Add(this.lbN, 2, 1);
            this.tlpMain.Controls.Add(this.lbSize, 1, 1);
            this.tlpMain.Controls.Add(this.lbBrand, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(460, 143);
            this.tlpMain.TabIndex = 0;
            // 
            // pnlTitle
            // 
            this.tlpMain.SetColumnSpan(this.pnlTitle, 6);
            this.pnlTitle.Controls.Add(this.picClose);
            this.pnlTitle.Controls.Add(this.lbTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(2, 2);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(456, 25);
            this.pnlTitle.TabIndex = 0;
            // 
            // picClose
            // 
            this.picClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = global::FQC.Properties.Resources.close_blue;
            this.picClose.Location = new System.Drawing.Point(436, 0);
            this.picClose.Margin = new System.Windows.Forms.Padding(10);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(20, 25);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picClose.TabIndex = 1;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTitle.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Location = new System.Drawing.Point(0, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(70, 22);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "压力峰值";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbBrandValue
            // 
            this.lbBrandValue.AutoSize = true;
            this.lbBrandValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBrandValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbBrandValue.ForeColor = System.Drawing.Color.White;
            this.lbBrandValue.Location = new System.Drawing.Point(5, 86);
            this.lbBrandValue.Name = "lbBrandValue";
            this.lbBrandValue.Size = new System.Drawing.Size(68, 55);
            this.lbBrandValue.TabIndex = 0;
            this.lbBrandValue.Text = "10mL";
            this.lbBrandValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbSizeValue
            // 
            this.lbSizeValue.AutoSize = true;
            this.lbSizeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSizeValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbSizeValue.Location = new System.Drawing.Point(81, 86);
            this.lbSizeValue.Name = "lbSizeValue";
            this.lbSizeValue.Size = new System.Drawing.Size(68, 55);
            this.lbSizeValue.TabIndex = 0;
            this.lbSizeValue.Text = "30";
            this.lbSizeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbNValue
            // 
            this.lbNValue.AutoSize = true;
            this.lbNValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbNValue.ForeColor = System.Drawing.Color.White;
            this.lbNValue.Location = new System.Drawing.Point(157, 86);
            this.lbNValue.Name = "lbNValue";
            this.lbNValue.Size = new System.Drawing.Size(68, 55);
            this.lbNValue.TabIndex = 0;
            this.lbNValue.Text = "30.3";
            this.lbNValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLValue
            // 
            this.lbLValue.AutoSize = true;
            this.lbLValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbLValue.ForeColor = System.Drawing.Color.White;
            this.lbLValue.Location = new System.Drawing.Point(233, 86);
            this.lbLValue.Name = "lbLValue";
            this.lbLValue.Size = new System.Drawing.Size(68, 55);
            this.lbLValue.TabIndex = 0;
            this.lbLValue.Text = "40.3";
            this.lbLValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbCValue
            // 
            this.lbCValue.AutoSize = true;
            this.lbCValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbCValue.ForeColor = System.Drawing.Color.White;
            this.lbCValue.Location = new System.Drawing.Point(309, 86);
            this.lbCValue.Name = "lbCValue";
            this.lbCValue.Size = new System.Drawing.Size(68, 55);
            this.lbCValue.TabIndex = 0;
            this.lbCValue.Text = "50.6";
            this.lbCValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbHValue
            // 
            this.lbHValue.AutoSize = true;
            this.lbHValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbHValue.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 10F);
            this.lbHValue.ForeColor = System.Drawing.Color.White;
            this.lbHValue.Location = new System.Drawing.Point(385, 86);
            this.lbHValue.Name = "lbHValue";
            this.lbHValue.Size = new System.Drawing.Size(70, 55);
            this.lbHValue.TabIndex = 0;
            this.lbHValue.Text = "112.3";
            this.lbHValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbH
            // 
            this.lbH.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbH.AutoSize = true;
            this.lbH.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbH.ForeColor = System.Drawing.Color.White;
            this.lbH.Location = new System.Drawing.Point(409, 45);
            this.lbH.Name = "lbH";
            this.lbH.Size = new System.Drawing.Size(21, 22);
            this.lbH.TabIndex = 0;
            this.lbH.Text = "H";
            // 
            // lbC
            // 
            this.lbC.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbC.AutoSize = true;
            this.lbC.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbC.ForeColor = System.Drawing.Color.White;
            this.lbC.Location = new System.Drawing.Point(333, 45);
            this.lbC.Name = "lbC";
            this.lbC.Size = new System.Drawing.Size(20, 22);
            this.lbC.TabIndex = 0;
            this.lbC.Text = "C";
            // 
            // lbL
            // 
            this.lbL.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbL.AutoSize = true;
            this.lbL.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbL.ForeColor = System.Drawing.Color.White;
            this.lbL.Location = new System.Drawing.Point(258, 45);
            this.lbL.Name = "lbL";
            this.lbL.Size = new System.Drawing.Size(18, 22);
            this.lbL.TabIndex = 0;
            this.lbL.Text = "L";
            // 
            // lbN
            // 
            this.lbN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbN.AutoSize = true;
            this.lbN.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbN.ForeColor = System.Drawing.Color.White;
            this.lbN.Location = new System.Drawing.Point(180, 45);
            this.lbN.Name = "lbN";
            this.lbN.Size = new System.Drawing.Size(21, 22);
            this.lbN.TabIndex = 0;
            this.lbN.Text = "N";
            // 
            // lbSize
            // 
            this.lbSize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbSize.AutoSize = true;
            this.lbSize.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbSize.ForeColor = System.Drawing.Color.White;
            this.lbSize.Location = new System.Drawing.Point(95, 45);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(40, 22);
            this.lbSize.TabIndex = 0;
            this.lbSize.Text = "尺寸";
            // 
            // lbBrand
            // 
            this.lbBrand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbBrand.AutoSize = true;
            this.lbBrand.Font = new System.Drawing.Font("Noto Sans CJK SC Regular", 11F);
            this.lbBrand.ForeColor = System.Drawing.Color.White;
            this.lbBrand.Location = new System.Drawing.Point(19, 45);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(40, 22);
            this.lbBrand.TabIndex = 0;
            this.lbBrand.Text = "品牌";
            // 
            // Detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.Controls.Add(this.tlpMain);
            this.Name = "Detail";
            this.Size = new System.Drawing.Size(460, 143);
            this.VisibleChanged += new System.EventHandler(this.Detail_VisibleChanged);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.Label lbL;
        private System.Windows.Forms.Label lbC;
        private System.Windows.Forms.Label lbH;
        private System.Windows.Forms.Label lbSizeValue;
        private System.Windows.Forms.Label lbNValue;
        private System.Windows.Forms.Label lbLValue;
        private System.Windows.Forms.Label lbCValue;
        private System.Windows.Forms.Label lbHValue;
        private System.Windows.Forms.Label lbBrandValue;
        private System.Windows.Forms.Label lbN;
        private System.Windows.Forms.Label lbSize;
        private System.Windows.Forms.Label lbBrand;
    }
}
