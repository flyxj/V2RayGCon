namespace V2RayGCon.Views
{
    partial class FormConfigTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigTester));
            this.rtboxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxServList = new System.Windows.Forms.ComboBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtboxLog
            // 
            this.rtboxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtboxLog.Location = new System.Drawing.Point(0, 32);
            this.rtboxLog.Name = "rtboxLog";
            this.rtboxLog.ReadOnly = true;
            this.rtboxLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtboxLog.Size = new System.Drawing.Size(619, 290);
            this.rtboxLog.TabIndex = 0;
            this.rtboxLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server";
            // 
            // cboxServList
            // 
            this.cboxServList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxServList.FormattingEnabled = true;
            this.cboxServList.Location = new System.Drawing.Point(59, 6);
            this.cboxServList.Name = "cboxServList";
            this.cboxServList.Size = new System.Drawing.Size(121, 20);
            this.cboxServList.TabIndex = 2;
            this.cboxServList.SelectedIndexChanged += new System.EventHandler(this.cboxServList_SelectedIndexChanged);
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(186, 6);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 20);
            this.btnRestart.TabIndex = 3;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(267, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 20);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // FormConfigTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 322);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.cboxServList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtboxLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfigTester";
            this.Text = "Config tester";
            this.Shown += new System.EventHandler(this.FormConfigTester_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtboxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxServList;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnStop;
    }
}