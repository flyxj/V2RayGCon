namespace TestV2RayGCon
{
    partial class testQRCode
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(testQRCode));
            this.picDrawArea = new System.Windows.Forms.PictureBox();
            this.btnScanWinTest = new System.Windows.Forms.Button();
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.btnZoomTest = new System.Windows.Forms.Button();
            this.btnScanQRCode = new System.Windows.Forms.Button();
            this.btnCpoyScreen = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tboxOutput = new System.Windows.Forms.TextBox();
            this.btnProto = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picDrawArea)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // picDrawArea
            // 
            this.picDrawArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDrawArea.Image = ((System.Drawing.Image)(resources.GetObject("picDrawArea.Image")));
            this.picDrawArea.Location = new System.Drawing.Point(6, 35);
            this.picDrawArea.Name = "picDrawArea";
            this.picDrawArea.Size = new System.Drawing.Size(400, 300);
            this.picDrawArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDrawArea.TabIndex = 0;
            this.picDrawArea.TabStop = false;
            // 
            // btnScanWinTest
            // 
            this.btnScanWinTest.Location = new System.Drawing.Point(249, 6);
            this.btnScanWinTest.Name = "btnScanWinTest";
            this.btnScanWinTest.Size = new System.Drawing.Size(75, 23);
            this.btnScanWinTest.TabIndex = 1;
            this.btnScanWinTest.Text = "SquareWin";
            this.btnScanWinTest.UseVisualStyleBackColor = true;
            this.btnScanWinTest.Click += new System.EventHandler(this.btnScanWinTest_Click);
            // 
            // btnZoomTest
            // 
            this.btnZoomTest.Location = new System.Drawing.Point(168, 6);
            this.btnZoomTest.Name = "btnZoomTest";
            this.btnZoomTest.Size = new System.Drawing.Size(75, 23);
            this.btnZoomTest.TabIndex = 2;
            this.btnZoomTest.Text = "ZoomWin";
            this.btnZoomTest.UseVisualStyleBackColor = true;
            this.btnZoomTest.Click += new System.EventHandler(this.btnZoomTest_Click);
            // 
            // btnScanQRCode
            // 
            this.btnScanQRCode.Location = new System.Drawing.Point(87, 6);
            this.btnScanQRCode.Name = "btnScanQRCode";
            this.btnScanQRCode.Size = new System.Drawing.Size(75, 23);
            this.btnScanQRCode.TabIndex = 3;
            this.btnScanQRCode.Text = "ScanQRCode";
            this.btnScanQRCode.UseVisualStyleBackColor = true;
            this.btnScanQRCode.Click += new System.EventHandler(this.btnScanQRCode_Click);
            // 
            // btnCpoyScreen
            // 
            this.btnCpoyScreen.Location = new System.Drawing.Point(6, 6);
            this.btnCpoyScreen.Name = "btnCpoyScreen";
            this.btnCpoyScreen.Size = new System.Drawing.Size(75, 23);
            this.btnCpoyScreen.TabIndex = 4;
            this.btnCpoyScreen.Text = "CopyScreen";
            this.btnCpoyScreen.UseVisualStyleBackColor = true;
            this.btnCpoyScreen.Click += new System.EventHandler(this.btnCpoyScreen_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(9, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(420, 367);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tboxOutput);
            this.tabPage2.Controls.Add(this.btnProto);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(412, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Proto";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tboxOutput
            // 
            this.tboxOutput.Location = new System.Drawing.Point(6, 143);
            this.tboxOutput.Multiline = true;
            this.tboxOutput.Name = "tboxOutput";
            this.tboxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tboxOutput.Size = new System.Drawing.Size(400, 192);
            this.tboxOutput.TabIndex = 1;
            // 
            // btnProto
            // 
            this.btnProto.Location = new System.Drawing.Point(6, 6);
            this.btnProto.Name = "btnProto";
            this.btnProto.Size = new System.Drawing.Size(75, 23);
            this.btnProto.TabIndex = 0;
            this.btnProto.Text = "Proto";
            this.btnProto.UseVisualStyleBackColor = true;
            this.btnProto.Click += new System.EventHandler(this.btnProto_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnCpoyScreen);
            this.tabPage1.Controls.Add(this.picDrawArea);
            this.tabPage1.Controls.Add(this.btnScanWinTest);
            this.tabPage1.Controls.Add(this.btnZoomTest);
            this.tabPage1.Controls.Add(this.btnScanQRCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(412, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "QRCode";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(412, 341);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Stuff";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "MultipleLineMsgBox";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // testQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 387);
            this.Controls.Add(this.tabControl1);
            this.Name = "testQRCode";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picDrawArea)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picDrawArea;
        private System.Windows.Forms.Button btnScanWinTest;
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Button btnZoomTest;
        private System.Windows.Forms.Button btnScanQRCode;
        private System.Windows.Forms.Button btnCpoyScreen;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tboxOutput;
        private System.Windows.Forms.Button btnProto;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button1;
    }
}

