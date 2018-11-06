namespace V2RayGCon.Views.UserControls
{
    partial class PluginInfoUI
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginInfoUI));
            this.chkIsUse = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbFilename = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkIsUse
            // 
            resources.ApplyResources(this.chkIsUse, "chkIsUse");
            this.chkIsUse.Name = "chkIsUse";
            this.chkIsUse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lbDescription
            // 
            resources.ApplyResources(this.lbDescription, "lbDescription");
            this.lbDescription.AutoEllipsis = true;
            this.lbDescription.Name = "lbDescription";
            // 
            // lbName
            // 
            resources.ApplyResources(this.lbName, "lbName");
            this.lbName.AutoEllipsis = true;
            this.lbName.Name = "lbName";
            // 
            // lbVersion
            // 
            resources.ApplyResources(this.lbVersion, "lbVersion");
            this.lbVersion.AutoEllipsis = true;
            this.lbVersion.Name = "lbVersion";
            // 
            // lbFilename
            // 
            resources.ApplyResources(this.lbFilename, "lbFilename");
            this.lbFilename.AutoEllipsis = true;
            this.lbFilename.Name = "lbFilename";
            // 
            // PluginInfoUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbFilename);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkIsUse);
            this.Name = "PluginInfoUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsUse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbFilename;
    }
}
