﻿namespace FQC
{
    partial class DualPumpAlertDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.tlpmain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlpmain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbResult
            // 
            this.lbResult.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbResult.AutoSize = true;
            this.tlpmain.SetColumnSpan(this.lbResult, 2);
            this.lbResult.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.lbResult.Location = new System.Drawing.Point(20, 54);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(259, 26);
            this.lbResult.TabIndex = 4;
            this.lbResult.Text = "点击确定继续测试第另一道泵";
            this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(37, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tlpmain
            // 
            this.tlpmain.BackColor = System.Drawing.Color.White;
            this.tlpmain.ColumnCount = 2;
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpmain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpmain.Controls.Add(this.btnOK, 0, 2);
            this.tlpmain.Controls.Add(this.panel1, 0, 0);
            this.tlpmain.Controls.Add(this.lbResult, 0, 1);
            this.tlpmain.Controls.Add(this.btnCancel, 1, 2);
            this.tlpmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpmain.Location = new System.Drawing.Point(0, 0);
            this.tlpmain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpmain.Name = "tlpmain";
            this.tlpmain.RowCount = 3;
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpmain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpmain.Size = new System.Drawing.Size(300, 180);
            this.tlpmain.TabIndex = 3;
            this.tlpmain.Paint += new System.Windows.Forms.PaintEventHandler(this.tlpmain_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.tlpmain.SetColumnSpan(this.panel1, 2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 30);
            this.panel1.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(113)))), ((int)(((byte)(185)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(187, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DualPumpAlertDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(300, 180);
            this.Controls.Add(this.tlpmain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DualPumpAlertDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResultDialog";
            this.tlpmain.ResumeLayout(false);
            this.tlpmain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tlpmain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
    }
}