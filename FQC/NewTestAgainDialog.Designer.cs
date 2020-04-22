namespace FQC
{
    partial class NewTestAgainDialog
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
            this.lbLevel5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tlpmain = new System.Windows.Forms.TableLayoutPanel();
            this.btnTestAgain = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbLevel1 = new System.Windows.Forms.Label();
            this.lbLevel2 = new System.Windows.Forms.Label();
            this.lbLevel3 = new System.Windows.Forms.Label();
            this.lbLevel4 = new System.Windows.Forms.Label();
            this.tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.tlpmain.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLevel5
            // 
            this.lbLevel5.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbLevel5, 3);
            this.lbLevel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLevel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLevel5.ForeColor = System.Drawing.Color.Black;
            this.lbLevel5.Location = new System.Drawing.Point(3, 162);
            this.lbLevel5.Name = "lbLevel5";
            this.lbLevel5.Size = new System.Drawing.Size(333, 28);
            this.lbLevel5.TabIndex = 4;
            this.lbLevel5.Text = "1道H档压力103Kpa，合格范围95~126Kpa；";
            this.lbLevel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLevel5.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(20, 195);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tlpmain
            // 
            this.tlpmain.BackColor = System.Drawing.Color.White;
            this.tlpmain.ColumnCount = 3;
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpmain.Controls.Add(this.btnOK, 0, 6);
            this.tlpmain.Controls.Add(this.lbLevel5, 0, 5);
            this.tlpmain.Controls.Add(this.btnTestAgain, 2, 6);
            this.tlpmain.Controls.Add(this.btnCancel, 1, 6);
            this.tlpmain.Controls.Add(this.lbLevel1, 0, 1);
            this.tlpmain.Controls.Add(this.lbLevel2, 0, 2);
            this.tlpmain.Controls.Add(this.lbLevel3, 0, 3);
            this.tlpmain.Controls.Add(this.lbLevel4, 0, 4);
            this.tlpmain.Controls.Add(this.tlpTitle, 0, 0);
            this.tlpmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpmain.Location = new System.Drawing.Point(0, 0);
            this.tlpmain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpmain.Name = "tlpmain";
            this.tlpmain.RowCount = 7;
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpmain.Size = new System.Drawing.Size(350, 230);
            this.tlpmain.TabIndex = 3;
            this.tlpmain.Paint += new System.Windows.Forms.PaintEventHandler(this.tlpmain_Paint);
            // 
            // btnTestAgain
            // 
            this.btnTestAgain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTestAgain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnTestAgain.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTestAgain.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTestAgain.ForeColor = System.Drawing.Color.White;
            this.btnTestAgain.Location = new System.Drawing.Point(253, 195);
            this.btnTestAgain.Name = "btnTestAgain";
            this.btnTestAgain.Size = new System.Drawing.Size(75, 30);
            this.btnTestAgain.TabIndex = 1;
            this.btnTestAgain.Text = "复测";
            this.btnTestAgain.UseVisualStyleBackColor = false;
            this.btnTestAgain.Click += new System.EventHandler(this.btnTestAgain_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(136, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbLevel1
            // 
            this.lbLevel1.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbLevel1, 3);
            this.lbLevel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLevel1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLevel1.ForeColor = System.Drawing.Color.Black;
            this.lbLevel1.Location = new System.Drawing.Point(3, 50);
            this.lbLevel1.Name = "lbLevel1";
            this.lbLevel1.Size = new System.Drawing.Size(321, 28);
            this.lbLevel1.TabIndex = 4;
            this.lbLevel1.Text = "1道N档压力18.0Kpa，合格范围20-33.4Kpa";
            this.lbLevel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLevel1.Visible = false;
            // 
            // lbLevel2
            // 
            this.lbLevel2.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbLevel2, 3);
            this.lbLevel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLevel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLevel2.ForeColor = System.Drawing.Color.Black;
            this.lbLevel2.Location = new System.Drawing.Point(3, 78);
            this.lbLevel2.Name = "lbLevel2";
            this.lbLevel2.Size = new System.Drawing.Size(337, 28);
            this.lbLevel2.TabIndex = 4;
            this.lbLevel2.Text = "1道L档压力32.4Kpa，合格范围35~53.3Kpa；";
            this.lbLevel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLevel2.Visible = false;
            // 
            // lbLevel3
            // 
            this.lbLevel3.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbLevel3, 3);
            this.lbLevel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLevel3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLevel3.ForeColor = System.Drawing.Color.Black;
            this.lbLevel3.Location = new System.Drawing.Point(3, 106);
            this.lbLevel3.Name = "lbLevel3";
            this.lbLevel3.Size = new System.Drawing.Size(331, 28);
            this.lbLevel3.TabIndex = 4;
            this.lbLevel3.Text = "4档压力103Kpa，合格范围73.4~113.3Kpa；";
            this.lbLevel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLevel3.Visible = false;
            // 
            // lbLevel4
            // 
            this.lbLevel4.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbLevel4, 3);
            this.lbLevel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbLevel4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLevel4.ForeColor = System.Drawing.Color.Black;
            this.lbLevel4.Location = new System.Drawing.Point(3, 134);
            this.lbLevel4.Name = "lbLevel4";
            this.lbLevel4.Size = new System.Drawing.Size(333, 28);
            this.lbLevel4.TabIndex = 4;
            this.lbLevel4.Text = "1道H档压力103Kpa，合格范围95~126Kpa；";
            this.lbLevel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbLevel4.Visible = false;
            // 
            // tlpTitle
            // 
            this.tlpTitle.BackColor = System.Drawing.Color.Red;
            this.tlpTitle.ColumnCount = 1;
            this.tlpmain.SetColumnSpan(this.tlpTitle, 3);
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTitle.Controls.Add(this.lbTitle, 0, 0);
            this.tlpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitle.Location = new System.Drawing.Point(0, 0);
            this.tlpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTitle.Name = "tlpTitle";
            this.tlpTitle.RowCount = 1;
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTitle.Size = new System.Drawing.Size(350, 50);
            this.tlpTitle.TabIndex = 5;
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("宋体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.ForeColor = System.Drawing.Color.White;
            this.lbTitle.Location = new System.Drawing.Point(131, 7);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(87, 35);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "失败";
            // 
            // NewTestAgainDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 230);
            this.Controls.Add(this.tlpmain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NewTestAgainDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResultDialog";
            this.tlpmain.ResumeLayout(false);
            this.tlpmain.PerformLayout();
            this.tlpTitle.ResumeLayout(false);
            this.tlpTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbLevel5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tlpmain;
        private System.Windows.Forms.Button btnTestAgain;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbLevel1;
        private System.Windows.Forms.Label lbLevel2;
        private System.Windows.Forms.Label lbLevel3;
        private System.Windows.Forms.Label lbLevel4;
        private System.Windows.Forms.TableLayoutPanel tlpTitle;
        private System.Windows.Forms.Label lbTitle;
    }
}