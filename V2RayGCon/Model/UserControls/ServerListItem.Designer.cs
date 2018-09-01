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
            this.lbSummary = new System.Windows.Forms.Label();
            this.lbIndex = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxInbound = new System.Windows.Forms.ComboBox();
            this.chkImport = new System.Windows.Forms.CheckBox();
            this.chkEnv = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tboxInboundIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.lbRunning = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.tboxInboundPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbSummary
            // 
            this.lbSummary.AutoSize = true;
            this.lbSummary.Location = new System.Drawing.Point(57, 16);
            this.lbSummary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSummary.Name = "lbSummary";
            this.lbSummary.Size = new System.Drawing.Size(224, 18);
            this.lbSummary.TabIndex = 0;
            this.lbSummary.Text = "[lovely] vmess@127.0.0.1";
            // 
            // lbIndex
            // 
            this.lbIndex.AutoSize = true;
            this.lbIndex.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbIndex.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.lbIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbIndex.Location = new System.Drawing.Point(9, 10);
            this.lbIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbIndex.Name = "lbIndex";
            this.lbIndex.Size = new System.Drawing.Size(36, 24);
            this.lbIndex.TabIndex = 1;
            this.lbIndex.Text = "78";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 88);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inbound";
            // 
            // cboxInbound
            // 
            this.cboxInbound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxInbound.FormattingEnabled = true;
            this.cboxInbound.Items.AddRange(new object[] {
            "Config",
            "HTTP",
            "SOCKS"});
            this.cboxInbound.Location = new System.Drawing.Point(136, 84);
            this.cboxInbound.Margin = new System.Windows.Forms.Padding(4);
            this.cboxInbound.Name = "cboxInbound";
            this.cboxInbound.Size = new System.Drawing.Size(121, 26);
            this.cboxInbound.TabIndex = 3;
            this.cboxInbound.SelectedIndexChanged += new System.EventHandler(this.cboxInbound_SelectedIndexChanged);
            // 
            // chkImport
            // 
            this.chkImport.AutoSize = true;
            this.chkImport.Location = new System.Drawing.Point(271, 86);
            this.chkImport.Margin = new System.Windows.Forms.Padding(4);
            this.chkImport.Name = "chkImport";
            this.chkImport.Size = new System.Drawing.Size(88, 22);
            this.chkImport.TabIndex = 4;
            this.chkImport.Text = "Import";
            this.chkImport.UseVisualStyleBackColor = true;
            this.chkImport.CheckedChanged += new System.EventHandler(this.chkImport_CheckedChanged);
            // 
            // chkEnv
            // 
            this.chkEnv.AutoSize = true;
            this.chkEnv.Location = new System.Drawing.Point(367, 86);
            this.chkEnv.Margin = new System.Windows.Forms.Padding(4);
            this.chkEnv.Name = "chkEnv";
            this.chkEnv.Size = new System.Drawing.Size(61, 22);
            this.chkEnv.TabIndex = 4;
            this.chkEnv.Text = "Env";
            this.chkEnv.UseVisualStyleBackColor = true;
            this.chkEnv.CheckedChanged += new System.EventHandler(this.chkEnv_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(440, 42);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 32);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tboxInboundIP
            // 
            this.tboxInboundIP.Location = new System.Drawing.Point(136, 44);
            this.tboxInboundIP.Margin = new System.Windows.Forms.Padding(4);
            this.tboxInboundIP.Name = "tboxInboundIP";
            this.tboxInboundIP.Size = new System.Drawing.Size(160, 28);
            this.tboxInboundIP.TabIndex = 6;
            this.tboxInboundIP.Text = "127.0.0.1";
            this.tboxInboundIP.TextChanged += new System.EventHandler(this.tboxInboundIP_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP";
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(440, 81);
            this.btnAction.Margin = new System.Windows.Forms.Padding(4);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(90, 32);
            this.btnAction.TabIndex = 5;
            this.btnAction.Text = "Action";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // lbRunning
            // 
            this.lbRunning.AutoSize = true;
            this.lbRunning.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRunning.ForeColor = System.Drawing.Color.DarkOrange;
            this.lbRunning.Location = new System.Drawing.Point(465, 16);
            this.lbRunning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRunning.Name = "lbRunning";
            this.lbRunning.Size = new System.Drawing.Size(38, 18);
            this.lbRunning.TabIndex = 2;
            this.lbRunning.Text = "OFF";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port";
            // 
            // tboxInboundPort
            // 
            this.tboxInboundPort.Location = new System.Drawing.Point(355, 44);
            this.tboxInboundPort.Name = "tboxInboundPort";
            this.tboxInboundPort.Size = new System.Drawing.Size(73, 28);
            this.tboxInboundPort.TabIndex = 7;
            this.tboxInboundPort.TextChanged += new System.EventHandler(this.tboxInboundPort_TextChanged);
            // 
            // ServerListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tboxInboundPort);
            this.Controls.Add(this.tboxInboundIP);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.chkEnv);
            this.Controls.Add(this.chkImport);
            this.Controls.Add(this.cboxInbound);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbRunning);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbIndex);
            this.Controls.Add(this.lbSummary);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServerListItem";
            this.Size = new System.Drawing.Size(540, 126);
            this.Load += new System.EventHandler(this.ServerListItem_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ServerListItem_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbSummary;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxInbound;
        private System.Windows.Forms.CheckBox chkImport;
        private System.Windows.Forms.CheckBox chkEnv;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox tboxInboundIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label lbRunning;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxInboundPort;
    }
}
