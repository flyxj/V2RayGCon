namespace V2RayGCon.Views
{
    partial class FormOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOption));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSubscribe = new System.Windows.Forms.TabPage();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddSubUrl = new System.Windows.Forms.Button();
            this.flySubUrlContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageSubscribe.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPageSubscribe);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPageSubscribe
            // 
            this.tabPageSubscribe.Controls.Add(this.btnDownload);
            this.tabPageSubscribe.Controls.Add(this.btnSave);
            this.tabPageSubscribe.Controls.Add(this.btnAddSubUrl);
            this.tabPageSubscribe.Controls.Add(this.flySubUrlContainer);
            resources.ApplyResources(this.tabPageSubscribe, "tabPageSubscribe");
            this.tabPageSubscribe.Name = "tabPageSubscribe";
            this.tabPageSubscribe.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            resources.ApplyResources(this.btnDownload, "btnDownload");
            this.btnDownload.Name = "btnDownload";
            this.toolTip1.SetToolTip(this.btnDownload, resources.GetString("btnDownload.ToolTip"));
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddSubUrl
            // 
            resources.ApplyResources(this.btnAddSubUrl, "btnAddSubUrl");
            this.btnAddSubUrl.Name = "btnAddSubUrl";
            this.toolTip1.SetToolTip(this.btnAddSubUrl, resources.GetString("btnAddSubUrl.ToolTip"));
            this.btnAddSubUrl.UseVisualStyleBackColor = true;
            this.btnAddSubUrl.Click += new System.EventHandler(this.btnAddSubUrl_Click);
            // 
            // flySubUrlContainer
            // 
            this.flySubUrlContainer.AllowDrop = true;
            resources.ApplyResources(this.flySubUrlContainer, "flySubUrlContainer");
            this.flySubUrlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flySubUrlContainer.Name = "flySubUrlContainer";
            this.flySubUrlContainer.DragDrop += new System.Windows.Forms.DragEventHandler(this.flySubUrlContainer_DragDrop);
            this.flySubUrlContainer.DragEnter += new System.Windows.Forms.DragEventHandler(this.flySubUrlContainer_DragEnter);
            // 
            // FormOption
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "FormOption";
            this.Shown += new System.EventHandler(this.FormOption_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSubscribe.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSubscribe;
        private System.Windows.Forms.FlowLayoutPanel flySubUrlContainer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddSubUrl;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
