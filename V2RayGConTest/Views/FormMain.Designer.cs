namespace V2RayGConTest.Views

{
    partial class FormMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.base64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patternTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getLinkBodyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(389, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 窗口ToolStripMenuItem
            // 
            this.窗口ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.base64ToolStripMenuItem,
            this.patternTestToolStripMenuItem,
            this.getLinkBodyToolStripMenuItem});
            this.窗口ToolStripMenuItem.Name = "窗口ToolStripMenuItem";
            this.窗口ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.窗口ToolStripMenuItem.Text = "Test";
            // 
            // base64ToolStripMenuItem
            // 
            this.base64ToolStripMenuItem.Name = "base64ToolStripMenuItem";
            this.base64ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.base64ToolStripMenuItem.Text = "Base64 decode";
            this.base64ToolStripMenuItem.Click += new System.EventHandler(this.base64ToolStripMenuItem_Click);
            // 
            // patternTestToolStripMenuItem
            // 
            this.patternTestToolStripMenuItem.Name = "patternTestToolStripMenuItem";
            this.patternTestToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.patternTestToolStripMenuItem.Text = "Pattern test";
            this.patternTestToolStripMenuItem.Click += new System.EventHandler(this.patternTestToolStripMenuItem_Click);
            // 
            // getLinkBodyToolStripMenuItem
            // 
            this.getLinkBodyToolStripMenuItem.Name = "getLinkBodyToolStripMenuItem";
            this.getLinkBodyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.getLinkBodyToolStripMenuItem.Text = "GetLinkBody";
            this.getLinkBodyToolStripMenuItem.Click += new System.EventHandler(this.getLinkBodyToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 244);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "V2RayGConTest";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem base64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patternTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getLinkBodyToolStripMenuItem;
    }
}

