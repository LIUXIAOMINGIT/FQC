namespace FQC
{
    partial class ResultDialogFail
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
            this.lbResult = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tlpmain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbDetail = new System.Windows.Forms.Label();
            this.tlpmain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbResult
            // 
            this.lbResult.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbResult.AutoSize = true;
            this.lbResult.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbResult.ForeColor = System.Drawing.Color.Red;
            this.lbResult.Location = new System.Drawing.Point(112, 37);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(69, 36);
            this.lbResult.TabIndex = 0;
            this.lbResult.Text = "通过";
            this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(109, 141);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tlpmain
            // 
            this.tlpmain.BackColor = System.Drawing.Color.White;
            this.tlpmain.ColumnCount = 1;
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpmain.Controls.Add(this.lbResult, 0, 1);
            this.tlpmain.Controls.Add(this.btnClose, 0, 3);
            this.tlpmain.Controls.Add(this.panel1, 0, 0);
            this.tlpmain.Controls.Add(this.lbDetail, 0, 2);
            this.tlpmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpmain.Location = new System.Drawing.Point(0, 0);
            this.tlpmain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpmain.Name = "tlpmain";
            this.tlpmain.RowCount = 4;
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpmain.Size = new System.Drawing.Size(293, 183);
            this.tlpmain.TabIndex = 2;
            this.tlpmain.Paint += new System.Windows.Forms.PaintEventHandler(this.tlpmain_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 30);
            this.panel1.TabIndex = 2;
            // 
            // lbDetail
            // 
            this.lbDetail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbDetail.AutoSize = true;
            this.lbDetail.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDetail.ForeColor = System.Drawing.Color.Red;
            this.lbDetail.Location = new System.Drawing.Point(9, 80);
            this.lbDetail.Name = "lbDetail";
            this.lbDetail.Size = new System.Drawing.Size(275, 50);
            this.lbDetail.TabIndex = 0;
            this.lbDetail.Text = "通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通" +
    "过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过通过";
            this.lbDetail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResultDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(293, 183);
            this.Controls.Add(this.tlpmain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ResultDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResultDialog";
            this.tlpmain.ResumeLayout(false);
            this.tlpmain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tlpmain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbDetail;
    }
}