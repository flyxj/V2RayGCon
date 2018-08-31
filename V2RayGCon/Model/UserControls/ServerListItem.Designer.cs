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
            this.tboxAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lbSummary
            // 
            this.lbSummary.AutoSize = true;
            this.lbSummary.Location = new System.Drawing.Point(38, 11);
            this.lbSummary.Name = "lbSummary";
            this.lbSummary.Size = new System.Drawing.Size(149, 12);
            this.lbSummary.TabIndex = 0;
            this.lbSummary.Text = "[lovely] vmess@127.0.0.1";
            // 
            // lbIndex
            // 
            this.lbIndex.AutoSize = true;
            this.lbIndex.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbIndex.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.lbIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbIndex.Location = new System.Drawing.Point(6, 7);
            this.lbIndex.Name = "lbIndex";
            this.lbIndex.Size = new System.Drawing.Size(26, 16);
            this.lbIndex.TabIndex = 1;
            this.lbIndex.Text = "78";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
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
            this.cboxInbound.Location = new System.Drawing.Point(91, 56);
            this.cboxInbound.Name = "cboxInbound";
            this.cboxInbound.Size = new System.Drawing.Size(82, 20);
            this.cboxInbound.TabIndex = 3;
            // 
            // chkImport
            // 
            this.chkImport.AutoSize = true;
            this.chkImport.Location = new System.Drawing.Point(227, 58);
            this.chkImport.Name = "chkImport";
            this.chkImport.Size = new System.Drawing.Size(60, 16);
            this.chkImport.TabIndex = 4;
            this.chkImport.Text = "Import";
            this.chkImport.UseVisualStyleBackColor = true;
            // 
            // chkEnv
            // 
            this.chkEnv.AutoSize = true;
            this.chkEnv.Location = new System.Drawing.Point(181, 58);
            this.chkEnv.Name = "chkEnv";
            this.chkEnv.Size = new System.Drawing.Size(42, 16);
            this.chkEnv.TabIndex = 4;
            this.chkEnv.Text = "Env";
            this.chkEnv.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(293, 29);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 21);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // tboxAddress
            // 
            this.tboxAddress.Location = new System.Drawing.Point(91, 29);
            this.tboxAddress.Name = "tboxAddress";
            this.tboxAddress.Size = new System.Drawing.Size(196, 21);
            this.tboxAddress.TabIndex = 6;
            this.tboxAddress.Text = "127.0.0.1:1080";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP:Port";
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(293, 56);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(60, 21);
            this.btnAction.TabIndex = 5;
            this.btnAction.Text = "Action";
            this.btnAction.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(310, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "OFF";
            // 
            // ServerListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tboxAddress);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.chkEnv);
            this.Controls.Add(this.chkImport);
            this.Controls.Add(this.cboxInbound);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbIndex);
            this.Controls.Add(this.lbSummary);
            this.Name = "ServerListItem";
            this.Size = new System.Drawing.Size(360, 84);
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
        private System.Windows.Forms.TextBox tboxAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
