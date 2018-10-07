namespace V2RayGCon.Views.UserControls
{
    partial class ServerUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerUI));
            this.lbServerTitle = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.cboxInbound = new System.Windows.Forms.ComboBox();
            this.tboxInboundAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMenu = new System.Windows.Forms.Button();
            this.lbRunning = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.cboxMark = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnIsCollapse = new System.Windows.Forms.Button();
            this.imageListCollapse = new System.Windows.Forms.ImageList(this.components);
            this.lbIsAutorun = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMultiboxing = new System.Windows.Forms.Button();
            this.ctxMenuStripMore = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemStart = new System.Windows.Forms.ToolStripMenuItem();
            this.multiboxingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemIsAutorun = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemIsInjectImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSkipCNSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vmessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.v2rayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.speedTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOfThisServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsSystemProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuStripMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbServerTitle
            // 
            this.lbServerTitle.AutoEllipsis = true;
            this.lbServerTitle.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbServerTitle, "lbServerTitle");
            this.lbServerTitle.Name = "lbServerTitle";
            this.toolTip1.SetToolTip(this.lbServerTitle, resources.GetString("lbServerTitle.ToolTip"));
            this.lbServerTitle.UseCompatibleTextRendering = true;
            this.lbServerTitle.Click += new System.EventHandler(this.lbSummary_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.Cursor = System.Windows.Forms.Cursors.SizeAll;
            resources.ApplyResources(this.lbStatus, "lbStatus");
            this.lbStatus.Name = "lbStatus";
            this.toolTip1.SetToolTip(this.lbStatus, resources.GetString("lbStatus.ToolTip"));
            this.lbStatus.UseCompatibleTextRendering = true;
            this.lbStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbStatus_MouseDown);
            // 
            // cboxInbound
            // 
            this.cboxInbound.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxInbound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxInbound.FormattingEnabled = true;
            this.cboxInbound.Items.AddRange(new object[] {
            resources.GetString("cboxInbound.Items"),
            resources.GetString("cboxInbound.Items1"),
            resources.GetString("cboxInbound.Items2")});
            resources.ApplyResources(this.cboxInbound, "cboxInbound");
            this.cboxInbound.Name = "cboxInbound";
            this.toolTip1.SetToolTip(this.cboxInbound, resources.GetString("cboxInbound.ToolTip"));
            this.cboxInbound.SelectedIndexChanged += new System.EventHandler(this.cboxInbound_SelectedIndexChanged);
            // 
            // tboxInboundAddr
            // 
            this.tboxInboundAddr.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tboxInboundAddr, "tboxInboundAddr");
            this.tboxInboundAddr.Name = "tboxInboundAddr";
            this.toolTip1.SetToolTip(this.tboxInboundAddr, resources.GetString("tboxInboundAddr.ToolTip"));
            this.tboxInboundAddr.TextChanged += new System.EventHandler(this.tboxInboundAddr_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Name = "label2";
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // btnMenu
            // 
            this.btnMenu.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnMenu, "btnMenu");
            this.btnMenu.Name = "btnMenu";
            this.toolTip1.SetToolTip(this.btnMenu, resources.GetString("btnMenu.ToolTip"));
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // lbRunning
            // 
            this.lbRunning.Cursor = System.Windows.Forms.Cursors.SizeAll;
            resources.ApplyResources(this.lbRunning, "lbRunning");
            this.lbRunning.ForeColor = System.Drawing.Color.Green;
            this.lbRunning.Name = "lbRunning";
            this.toolTip1.SetToolTip(this.lbRunning, resources.GetString("lbRunning.ToolTip"));
            this.lbRunning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbRunning_MouseDown);
            // 
            // chkSelected
            // 
            resources.ApplyResources(this.chkSelected, "chkSelected");
            this.chkSelected.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkSelected.Name = "chkSelected";
            this.toolTip1.SetToolTip(this.chkSelected, resources.GetString("chkSelected.ToolTip"));
            this.chkSelected.UseVisualStyleBackColor = true;
            this.chkSelected.CheckedChanged += new System.EventHandler(this.chkSelected_CheckedChanged);
            // 
            // btnStart
            // 
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.Name = "btnStart";
            this.toolTip1.SetToolTip(this.btnStart, resources.GetString("btnStart.ToolTip"));
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cboxMark
            // 
            this.cboxMark.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxMark.FormattingEnabled = true;
            resources.ApplyResources(this.cboxMark, "cboxMark");
            this.cboxMark.Name = "cboxMark";
            this.toolTip1.SetToolTip(this.cboxMark, resources.GetString("cboxMark.ToolTip"));
            this.cboxMark.DropDown += new System.EventHandler(this.cboxMark_DropDown);
            this.cboxMark.TextChanged += new System.EventHandler(this.cboxMark_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label4_MouseDown);
            // 
            // btnIsCollapse
            // 
            this.btnIsCollapse.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnIsCollapse, "btnIsCollapse");
            this.btnIsCollapse.ImageList = this.imageListCollapse;
            this.btnIsCollapse.Name = "btnIsCollapse";
            this.toolTip1.SetToolTip(this.btnIsCollapse, resources.GetString("btnIsCollapse.ToolTip"));
            this.btnIsCollapse.UseVisualStyleBackColor = true;
            this.btnIsCollapse.Click += new System.EventHandler(this.btnIsCollapse_Click);
            // 
            // imageListCollapse
            // 
            this.imageListCollapse.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCollapse.ImageStream")));
            this.imageListCollapse.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListCollapse.Images.SetKeyName(0, "StepBackArrow_16x.png");
            this.imageListCollapse.Images.SetKeyName(1, "GlyphUp_16x.png");
            this.imageListCollapse.Images.SetKeyName(2, "StepOverArrow_16x.png");
            // 
            // lbIsAutorun
            // 
            this.lbIsAutorun.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbIsAutorun, "lbIsAutorun");
            this.lbIsAutorun.Name = "lbIsAutorun";
            this.toolTip1.SetToolTip(this.lbIsAutorun, resources.GetString("lbIsAutorun.ToolTip"));
            this.lbIsAutorun.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbIsAutorun_MouseDown);
            // 
            // label1
            // 
            this.label1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.Name = "btnStop";
            this.toolTip1.SetToolTip(this.btnStop, resources.GetString("btnStop.ToolTip"));
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnMultiboxing
            // 
            this.btnMultiboxing.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnMultiboxing, "btnMultiboxing");
            this.btnMultiboxing.Name = "btnMultiboxing";
            this.toolTip1.SetToolTip(this.btnMultiboxing, resources.GetString("btnMultiboxing.ToolTip"));
            this.btnMultiboxing.UseVisualStyleBackColor = true;
            this.btnMultiboxing.Click += new System.EventHandler(this.btnMultiboxing_Click);
            // 
            // ctxMenuStripMore
            // 
            this.ctxMenuStripMore.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctxMenuStripMore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemStart,
            this.multiboxingToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemIsAutorun,
            this.toolStripMenuItemIsInjectImport,
            this.toolStripMenuItemSkipCNSite,
            this.toolStripMenuItem1,
            this.copyToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.speedTestToolStripMenuItem,
            this.logOfThisServerToolStripMenuItem,
            this.setAsSystemProxyToolStripMenuItem});
            this.ctxMenuStripMore.Name = "ctxMenuStripMore";
            resources.ApplyResources(this.ctxMenuStripMore, "ctxMenuStripMore");
            // 
            // toolStripMenuItemStart
            // 
            this.toolStripMenuItemStart.Name = "toolStripMenuItemStart";
            resources.ApplyResources(this.toolStripMenuItemStart, "toolStripMenuItemStart");
            this.toolStripMenuItemStart.Click += new System.EventHandler(this.toolStripMenuItemStart_Click);
            // 
            // multiboxingToolStripMenuItem
            // 
            this.multiboxingToolStripMenuItem.Name = "multiboxingToolStripMenuItem";
            resources.ApplyResources(this.multiboxingToolStripMenuItem, "multiboxingToolStripMenuItem");
            this.multiboxingToolStripMenuItem.Click += new System.EventHandler(this.multiboxingToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            resources.ApplyResources(this.stopToolStripMenuItem, "stopToolStripMenuItem");
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripMenuItemIsAutorun
            // 
            this.toolStripMenuItemIsAutorun.Name = "toolStripMenuItemIsAutorun";
            resources.ApplyResources(this.toolStripMenuItemIsAutorun, "toolStripMenuItemIsAutorun");
            this.toolStripMenuItemIsAutorun.Click += new System.EventHandler(this.toolStripMenuItemIsAutorun_Click);
            // 
            // toolStripMenuItemIsInjectImport
            // 
            this.toolStripMenuItemIsInjectImport.Name = "toolStripMenuItemIsInjectImport";
            resources.ApplyResources(this.toolStripMenuItemIsInjectImport, "toolStripMenuItemIsInjectImport");
            this.toolStripMenuItemIsInjectImport.Click += new System.EventHandler(this.toolStripMenuItemIsInjectImport_Click);
            // 
            // toolStripMenuItemSkipCNSite
            // 
            this.toolStripMenuItemSkipCNSite.Name = "toolStripMenuItemSkipCNSite";
            resources.ApplyResources(this.toolStripMenuItemSkipCNSite, "toolStripMenuItemSkipCNSite");
            this.toolStripMenuItemSkipCNSite.Click += new System.EventHandler(this.toolStripMenuItemSkipCNSite_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vmessToolStripMenuItem,
            this.v2rayToolStripMenuItem});
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // vmessToolStripMenuItem
            // 
            this.vmessToolStripMenuItem.Name = "vmessToolStripMenuItem";
            resources.ApplyResources(this.vmessToolStripMenuItem, "vmessToolStripMenuItem");
            this.vmessToolStripMenuItem.Click += new System.EventHandler(this.vmessToolStripMenuItem_Click);
            // 
            // v2rayToolStripMenuItem
            // 
            this.v2rayToolStripMenuItem.Name = "v2rayToolStripMenuItem";
            resources.ApplyResources(this.v2rayToolStripMenuItem, "v2rayToolStripMenuItem");
            this.v2rayToolStripMenuItem.Click += new System.EventHandler(this.v2rayToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // speedTestToolStripMenuItem
            // 
            this.speedTestToolStripMenuItem.Name = "speedTestToolStripMenuItem";
            resources.ApplyResources(this.speedTestToolStripMenuItem, "speedTestToolStripMenuItem");
            this.speedTestToolStripMenuItem.Click += new System.EventHandler(this.speedTestToolStripMenuItem_Click);
            // 
            // logOfThisServerToolStripMenuItem
            // 
            this.logOfThisServerToolStripMenuItem.Name = "logOfThisServerToolStripMenuItem";
            resources.ApplyResources(this.logOfThisServerToolStripMenuItem, "logOfThisServerToolStripMenuItem");
            this.logOfThisServerToolStripMenuItem.Click += new System.EventHandler(this.logOfThisServerToolStripMenuItem_Click);
            // 
            // setAsSystemProxyToolStripMenuItem
            // 
            this.setAsSystemProxyToolStripMenuItem.Name = "setAsSystemProxyToolStripMenuItem";
            resources.ApplyResources(this.setAsSystemProxyToolStripMenuItem, "setAsSystemProxyToolStripMenuItem");
            this.setAsSystemProxyToolStripMenuItem.Click += new System.EventHandler(this.setAsSystemProxyToolStripMenuItem_Click);
            // 
            // ServerUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnMultiboxing);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lbIsAutorun);
            this.Controls.Add(this.btnIsCollapse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxMark);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chkSelected);
            this.Controls.Add(this.tboxInboundAddr);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.cboxInbound);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbRunning);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbServerTitle);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.Name = "ServerUI";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.ServerListItem_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ServerListItem_MouseDown);
            this.ctxMenuStripMore.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbServerTitle;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ComboBox cboxInbound;
        private System.Windows.Forms.TextBox tboxInboundAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Label lbRunning;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkSelected;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ContextMenuStrip ctxMenuStripMore;
        private System.Windows.Forms.ToolStripMenuItem multiboxingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vmessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem v2rayToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem speedTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logOfThisServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsSystemProxyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboxMark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStart;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemIsAutorun;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemIsInjectImport;
        private System.Windows.Forms.Button btnIsCollapse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lbIsAutorun;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSkipCNSite;
        private System.Windows.Forms.ImageList imageListCollapse;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnMultiboxing;
    }
}
