namespace V2RayGCon.Views
{
    partial class FormDownloadCore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDownloadCore));
            this.cboxVer = new System.Windows.Forms.ComboBox();
            this.btnRefreshVer = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.pgBarDownload = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxArch = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboxVer
            // 
            resources.ApplyResources(this.cboxVer, "cboxVer");
            this.cboxVer.FormattingEnabled = true;
            this.cboxVer.Name = "cboxVer";
            this.toolTip1.SetToolTip(this.cboxVer, resources.GetString("cboxVer.ToolTip"));
            // 
            // btnRefreshVer
            // 
            resources.ApplyResources(this.btnRefreshVer, "btnRefreshVer");
            this.btnRefreshVer.Name = "btnRefreshVer";
            this.toolTip1.SetToolTip(this.btnRefreshVer, resources.GetString("btnRefreshVer.ToolTip"));
            this.btnRefreshVer.UseVisualStyleBackColor = true;
            this.btnRefreshVer.Click += new System.EventHandler(this.btnRefreshVer_Click);
            // 
            // btnDownload
            // 
            resources.ApplyResources(this.btnDownload, "btnDownload");
            this.btnDownload.Name = "btnDownload";
            this.toolTip1.SetToolTip(this.btnDownload, resources.GetString("btnDownload.ToolTip"));
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.toolTip1.SetToolTip(this.btnCancel, resources.GetString("btnCancel.ToolTip"));
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pgBarDownload
            // 
            resources.ApplyResources(this.pgBarDownload, "pgBarDownload");
            this.pgBarDownload.Name = "pgBarDownload";
            this.toolTip1.SetToolTip(this.pgBarDownload, resources.GetString("pgBarDownload.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // cboxArch
            // 
            resources.ApplyResources(this.cboxArch, "cboxArch");
            this.cboxArch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxArch.FormattingEnabled = true;
            this.cboxArch.Items.AddRange(new object[] {
            resources.GetString("cboxArch.Items"),
            resources.GetString("cboxArch.Items1")});
            this.cboxArch.Name = "cboxArch";
            this.toolTip1.SetToolTip(this.cboxArch, resources.GetString("cboxArch.ToolTip"));
            // 
            // FormDownloadCore
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxArch);
            this.Controls.Add(this.pgBarDownload);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnRefreshVer);
            this.Controls.Add(this.cboxVer);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDownloadCore";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboxVer;
        private System.Windows.Forms.Button btnRefreshVer;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ProgressBar pgBarDownload;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxArch;
    }
}