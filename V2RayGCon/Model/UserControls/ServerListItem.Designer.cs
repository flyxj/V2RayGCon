namespace V2RayGCon.Model.UserControls
{
    partial class ServerListItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerListItem));
            this.lbSummary = new System.Windows.Forms.Label();
            this.lbIndex = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.cboxInbound = new System.Windows.Forms.ComboBox();
            this.chkImport = new System.Windows.Forms.CheckBox();
            this.tboxInboundIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMore = new System.Windows.Forms.Button();
            this.lbRunning = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tboxInboundPort = new System.Windows.Forms.TextBox();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.cboxMark = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ctxMenuStripMore = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.multiboxingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vmessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.v2rayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.speedTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logOfThisServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsSystemProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.ctxMenuStripMore.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbSummary
            // 
            this.lbSummary.AutoEllipsis = true;
            this.lbSummary.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbSummary, "lbSummary");
            this.lbSummary.Name = "lbSummary";
            this.toolTip1.SetToolTip(this.lbSummary, resources.GetString("lbSummary.ToolTip"));
            this.lbSummary.Click += new System.EventHandler(this.lbSummary_Click);
            // 
            // lbIndex
            // 
            this.lbIndex.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbIndex, "lbIndex");
            this.lbIndex.Name = "lbIndex";
            this.lbIndex.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbIndex_MouseDown);
            // 
            // lbStatus
            // 
            resources.ApplyResources(this.lbStatus, "lbStatus");
            this.lbStatus.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.lbStatus.Name = "lbStatus";
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
            // chkImport
            // 
            resources.ApplyResources(this.chkImport, "chkImport");
            this.chkImport.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkImport.Name = "chkImport";
            this.toolTip1.SetToolTip(this.chkImport, resources.GetString("chkImport.ToolTip"));
            this.chkImport.UseVisualStyleBackColor = true;
            this.chkImport.CheckedChanged += new System.EventHandler(this.chkImport_CheckedChanged);
            // 
            // tboxInboundIP
            // 
            this.tboxInboundIP.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tboxInboundIP, "tboxInboundIP");
            this.tboxInboundIP.Name = "tboxInboundIP";
            this.toolTip1.SetToolTip(this.tboxInboundIP, resources.GetString("tboxInboundIP.ToolTip"));
            this.tboxInboundIP.TextChanged += new System.EventHandler(this.tboxInboundIP_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.label2.Name = "label2";
            // 
            // btnMore
            // 
            this.btnMore.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnMore, "btnMore");
            this.btnMore.Name = "btnMore";
            this.btnMore.UseVisualStyleBackColor = true;
            this.btnMore.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // lbRunning
            // 
            this.lbRunning.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbRunning, "lbRunning");
            this.lbRunning.ForeColor = System.Drawing.Color.Green;
            this.lbRunning.Name = "lbRunning";
            this.lbRunning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbRunning_MouseDown);
            // 
            // tboxInboundPort
            // 
            this.tboxInboundPort.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.tboxInboundPort, "tboxInboundPort");
            this.tboxInboundPort.Name = "tboxInboundPort";
            this.toolTip1.SetToolTip(this.tboxInboundPort, resources.GetString("tboxInboundPort.ToolTip"));
            this.tboxInboundPort.TextChanged += new System.EventHandler(this.tboxInboundPort_TextChanged);
            // 
            // chkAutoRun
            // 
            resources.ApplyResources(this.chkAutoRun, "chkAutoRun");
            this.chkAutoRun.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkAutoRun.Name = "chkAutoRun";
            this.toolTip1.SetToolTip(this.chkAutoRun, resources.GetString("chkAutoRun.ToolTip"));
            this.chkAutoRun.UseVisualStyleBackColor = true;
            this.chkAutoRun.CheckedChanged += new System.EventHandler(this.chkAutoRun_CheckedChanged);
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Name = "label3";
            // 
            // ctxMenuStripMore
            // 
            this.ctxMenuStripMore.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ctxMenuStripMore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.multiboxingToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.editToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.speedTestToolStripMenuItem,
            this.logOfThisServerToolStripMenuItem,
            this.setAsSystemProxyToolStripMenuItem});
            this.ctxMenuStripMore.Name = "ctxMenuStripMore";
            resources.ApplyResources(this.ctxMenuStripMore, "ctxMenuStripMore");
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
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
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
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
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
            // label1
            // 
            this.label1.Cursor = System.Windows.Forms.Cursors.SizeAll;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // ServerListItem
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxMark);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chkSelected);
            this.Controls.Add(this.chkAutoRun);
            this.Controls.Add(this.tboxInboundPort);
            this.Controls.Add(this.tboxInboundIP);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.chkImport);
            this.Controls.Add(this.cboxInbound);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbRunning);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbIndex);
            this.Controls.Add(this.lbSummary);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.Name = "ServerListItem";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Load += new System.EventHandler(this.ServerListItem_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ServerListItem_MouseDown);
            this.ctxMenuStripMore.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbSummary;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ComboBox cboxInbound;
        private System.Windows.Forms.CheckBox chkImport;
        private System.Windows.Forms.TextBox tboxInboundIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMore;
        private System.Windows.Forms.Label lbRunning;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxInboundPort;
        private System.Windows.Forms.CheckBox chkAutoRun;
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
    }
}
