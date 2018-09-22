namespace V2RayGCon.Views
{
    partial class FormBatchModifyServerSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBatchModifyServerSetting));
            this.chkInIP = new System.Windows.Forms.CheckBox();
            this.tboxInPort = new System.Windows.Forms.TextBox();
            this.chkInPort = new System.Windows.Forms.CheckBox();
            this.tboxInIP = new System.Windows.Forms.TextBox();
            this.chkMark = new System.Windows.Forms.CheckBox();
            this.chkInMode = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboxInMode = new System.Windows.Forms.ComboBox();
            this.cboxMark = new System.Windows.Forms.ComboBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkInIP
            // 
            resources.ApplyResources(this.chkInIP, "chkInIP");
            this.chkInIP.Name = "chkInIP";
            this.chkInIP.UseVisualStyleBackColor = true;
            // 
            // tboxInPort
            // 
            resources.ApplyResources(this.tboxInPort, "tboxInPort");
            this.tboxInPort.Name = "tboxInPort";
            // 
            // chkInPort
            // 
            resources.ApplyResources(this.chkInPort, "chkInPort");
            this.chkInPort.Name = "chkInPort";
            this.chkInPort.UseVisualStyleBackColor = true;
            // 
            // tboxInIP
            // 
            resources.ApplyResources(this.tboxInIP, "tboxInIP");
            this.tboxInIP.Name = "tboxInIP";
            // 
            // chkMark
            // 
            resources.ApplyResources(this.chkMark, "chkMark");
            this.chkMark.Name = "chkMark";
            this.chkMark.UseVisualStyleBackColor = true;
            // 
            // chkInMode
            // 
            resources.ApplyResources(this.chkInMode, "chkInMode");
            this.chkInMode.Name = "chkInMode";
            this.chkInMode.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cboxInMode);
            this.groupBox1.Controls.Add(this.chkInMode);
            this.groupBox1.Controls.Add(this.tboxInPort);
            this.groupBox1.Controls.Add(this.tboxInIP);
            this.groupBox1.Controls.Add(this.chkInPort);
            this.groupBox1.Controls.Add(this.chkInIP);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cboxInMode
            // 
            resources.ApplyResources(this.cboxInMode, "cboxInMode");
            this.cboxInMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxInMode.FormattingEnabled = true;
            this.cboxInMode.Items.AddRange(new object[] {
            resources.GetString("cboxInMode.Items"),
            resources.GetString("cboxInMode.Items1"),
            resources.GetString("cboxInMode.Items2")});
            this.cboxInMode.Name = "cboxInMode";
            // 
            // cboxMark
            // 
            resources.ApplyResources(this.cboxMark, "cboxMark");
            this.cboxMark.FormattingEnabled = true;
            this.cboxMark.Name = "cboxMark";
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormBatchModifyServerSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.cboxMark);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkMark);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBatchModifyServerSetting";
            this.Shown += new System.EventHandler(this.FormBatchModifyServerInfo_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkInIP;
        private System.Windows.Forms.TextBox tboxInPort;
        private System.Windows.Forms.CheckBox chkInPort;
        private System.Windows.Forms.TextBox tboxInIP;
        private System.Windows.Forms.CheckBox chkMark;
        private System.Windows.Forms.CheckBox chkInMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboxInMode;
        private System.Windows.Forms.ComboBox cboxMark;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnCancel;
    }
}
