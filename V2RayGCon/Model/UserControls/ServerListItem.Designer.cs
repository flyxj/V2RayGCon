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
            this.btnAction = new System.Windows.Forms.Button();
            this.lbRunning = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tboxInboundPort = new System.Windows.Forms.TextBox();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.chkSelected = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbSummary
            // 
            resources.ApplyResources(this.lbSummary, "lbSummary");
            this.lbSummary.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbSummary.Name = "lbSummary";
            this.toolTip1.SetToolTip(this.lbSummary, resources.GetString("lbSummary.ToolTip"));
            this.lbSummary.Click += new System.EventHandler(this.lbSummary_Click);
            // 
            // lbIndex
            // 
            this.lbIndex.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbIndex, "lbIndex");
            this.lbIndex.Name = "lbIndex";
            // 
            // lbStatus
            // 
            resources.ApplyResources(this.lbStatus, "lbStatus");
            this.lbStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbStatus.Name = "lbStatus";
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
            // btnAction
            // 
            this.btnAction.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.btnAction, "btnAction");
            this.btnAction.Name = "btnAction";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // lbRunning
            // 
            this.lbRunning.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.lbRunning, "lbRunning");
            this.lbRunning.ForeColor = System.Drawing.Color.Green;
            this.lbRunning.Name = "lbRunning";
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Name = "label3";
            // 
            // ServerListItem
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.chkSelected);
            this.Controls.Add(this.chkAutoRun);
            this.Controls.Add(this.tboxInboundPort);
            this.Controls.Add(this.tboxInboundIP);
            this.Controls.Add(this.btnAction);
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
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label lbRunning;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxInboundPort;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.CheckBox chkSelected;
    }
}
