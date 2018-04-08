namespace testV2RayGCon
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
            ((System.ComponentModel.ISupportInitialize)(this.picDrawArea)).BeginInit();
            this.SuspendLayout();
            // 
            // picDrawArea
            // 
            this.picDrawArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDrawArea.Image = ((System.Drawing.Image)(resources.GetObject("picDrawArea.Image")));
            this.picDrawArea.Location = new System.Drawing.Point(12, 48);
            this.picDrawArea.Name = "picDrawArea";
            this.picDrawArea.Size = new System.Drawing.Size(400, 300);
            this.picDrawArea.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDrawArea.TabIndex = 0;
            this.picDrawArea.TabStop = false;
            // 
            // btnScanWinTest
            // 
            this.btnScanWinTest.Location = new System.Drawing.Point(332, 12);
            this.btnScanWinTest.Name = "btnScanWinTest";
            this.btnScanWinTest.Size = new System.Drawing.Size(75, 23);
            this.btnScanWinTest.TabIndex = 1;
            this.btnScanWinTest.Text = "SquareWin";
            this.btnScanWinTest.UseVisualStyleBackColor = true;
            this.btnScanWinTest.Click += new System.EventHandler(this.btnScanWinTest_Click);
            // 
            // btnZoomTest
            // 
            this.btnZoomTest.Location = new System.Drawing.Point(251, 12);
            this.btnZoomTest.Name = "btnZoomTest";
            this.btnZoomTest.Size = new System.Drawing.Size(75, 23);
            this.btnZoomTest.TabIndex = 2;
            this.btnZoomTest.Text = "ZoomWin";
            this.btnZoomTest.UseVisualStyleBackColor = true;
            this.btnZoomTest.Click += new System.EventHandler(this.btnZoomTest_Click);
            // 
            // btnScanQRCode
            // 
            this.btnScanQRCode.Location = new System.Drawing.Point(170, 12);
            this.btnScanQRCode.Name = "btnScanQRCode";
            this.btnScanQRCode.Size = new System.Drawing.Size(75, 23);
            this.btnScanQRCode.TabIndex = 3;
            this.btnScanQRCode.Text = "ScanQRCode";
            this.btnScanQRCode.UseVisualStyleBackColor = true;
            this.btnScanQRCode.Click += new System.EventHandler(this.btnScanQRCode_Click);
            // 
            // btnCpoyScreen
            // 
            this.btnCpoyScreen.Location = new System.Drawing.Point(89, 12);
            this.btnCpoyScreen.Name = "btnCpoyScreen";
            this.btnCpoyScreen.Size = new System.Drawing.Size(75, 23);
            this.btnCpoyScreen.TabIndex = 4;
            this.btnCpoyScreen.Text = "CopyScreen";
            this.btnCpoyScreen.UseVisualStyleBackColor = true;
            this.btnCpoyScreen.Click += new System.EventHandler(this.btnCpoyScreen_Click);
            // 
            // testQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 358);
            this.Controls.Add(this.btnCpoyScreen);
            this.Controls.Add(this.btnScanQRCode);
            this.Controls.Add(this.btnZoomTest);
            this.Controls.Add(this.btnScanWinTest);
            this.Controls.Add(this.picDrawArea);
            this.Name = "testQRCode";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picDrawArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picDrawArea;
        private System.Windows.Forms.Button btnScanWinTest;
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Button btnZoomTest;
        private System.Windows.Forms.Button btnScanQRCode;
        private System.Windows.Forms.Button btnCpoyScreen;
    }
}

