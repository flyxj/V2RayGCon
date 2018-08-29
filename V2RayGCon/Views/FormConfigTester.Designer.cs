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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigTester));
            this.rtboxLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxServList = new System.Windows.Forms.ComboBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboxGlobalImport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtboxLog
            // 
            resources.ApplyResources(this.rtboxLog, "rtboxLog");
            this.rtboxLog.Name = "rtboxLog";
            this.rtboxLog.ReadOnly = true;
            this.rtboxLog.TextChanged += new System.EventHandler(this.rtboxLog_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cboxServList
            // 
            this.cboxServList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxServList.FormattingEnabled = true;
            resources.ApplyResources(this.cboxServList, "cboxServList");
            this.cboxServList.Name = "cboxServList";
            this.cboxServList.SelectedIndexChanged += new System.EventHandler(this.cboxServList_SelectedIndexChanged);
            // 
            // btnRestart
            // 
            resources.ApplyResources(this.btnRestart, "btnRestart");
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Name = "btnStop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cboxGlobalImport
            // 
            resources.ApplyResources(this.cboxGlobalImport, "cboxGlobalImport");
            this.cboxGlobalImport.Checked = true;
            this.cboxGlobalImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxGlobalImport.Name = "cboxGlobalImport";
            this.toolTip1.SetToolTip(this.cboxGlobalImport, resources.GetString("cboxGlobalImport.ToolTip"));
            this.cboxGlobalImport.UseVisualStyleBackColor = true;
            // 
            // FormConfigTester
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboxGlobalImport);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.cboxServList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtboxLog);
            this.Name = "FormConfigTester";
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox cboxGlobalImport;
    }
}
