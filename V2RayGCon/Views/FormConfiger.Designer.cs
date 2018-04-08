namespace V2RayGCon.Views
{
    partial class FormConfiger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiger));
            this.btnInsertNewServ = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOverwriteConfig = new System.Windows.Forms.Button();
            this.cboxConfigPart = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnVMessInsertClient = new System.Windows.Forms.Button();
            this.btnVMessGenUUID = new System.Windows.Forms.Button();
            this.tboxVMessIPaddr = new System.Windows.Forms.TextBox();
            this.tboxVMessAid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tboxVMessLevel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tboxVMessID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSSRInsertClient = new System.Windows.Forms.Button();
            this.cboxSSROTA = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboxSSRMethod = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboxShowPassWord = new System.Windows.Forms.CheckBox();
            this.tboxSSRPass = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tboxSSRAddr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tboxSSREmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboxStreamSecurity = new System.Windows.Forms.ComboBox();
            this.btnStreamInsertTCP = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnStreamInsertWS = new System.Windows.Forms.Button();
            this.tboxWSPath = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnStreamInsertKCP = new System.Windows.Forms.Button();
            this.tboxKCPType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSaveModify = new System.Windows.Forms.Button();
            this.btnClearModify = new System.Windows.Forms.Button();
            this.btnLoadDefault = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.cboxServList = new System.Windows.Forms.ComboBox();
            this.panelScintilla = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInsertNewServ
            // 
            resources.ApplyResources(this.btnInsertNewServ, "btnInsertNewServ");
            this.btnInsertNewServ.Name = "btnInsertNewServ";
            this.btnInsertNewServ.UseVisualStyleBackColor = true;
            this.btnInsertNewServ.Click += new System.EventHandler(this.btnInsertNewServ_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOverwriteConfig
            // 
            resources.ApplyResources(this.btnOverwriteConfig, "btnOverwriteConfig");
            this.btnOverwriteConfig.Name = "btnOverwriteConfig";
            this.btnOverwriteConfig.UseVisualStyleBackColor = true;
            this.btnOverwriteConfig.Click += new System.EventHandler(this.btnOverwriteServConfig_Click);
            // 
            // cboxConfigPart
            // 
            resources.ApplyResources(this.cboxConfigPart, "cboxConfigPart");
            this.cboxConfigPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConfigPart.FormattingEnabled = true;
            this.cboxConfigPart.Items.AddRange(new object[] {
            resources.GetString("cboxConfigPart.Items"),
            resources.GetString("cboxConfigPart.Items1"),
            resources.GetString("cboxConfigPart.Items2"),
            resources.GetString("cboxConfigPart.Items3"),
            resources.GetString("cboxConfigPart.Items4"),
            resources.GetString("cboxConfigPart.Items5"),
            resources.GetString("cboxConfigPart.Items6"),
            resources.GetString("cboxConfigPart.Items7"),
            resources.GetString("cboxConfigPart.Items8"),
            resources.GetString("cboxConfigPart.Items9"),
            resources.GetString("cboxConfigPart.Items10"),
            resources.GetString("cboxConfigPart.Items11")});
            this.cboxConfigPart.Name = "cboxConfigPart";
            this.cboxConfigPart.SelectedIndexChanged += new System.EventHandler(this.cboxConfigPart_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.btnVMessInsertClient);
            this.groupBox1.Controls.Add(this.btnVMessGenUUID);
            this.groupBox1.Controls.Add(this.tboxVMessIPaddr);
            this.groupBox1.Controls.Add(this.tboxVMessAid);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tboxVMessLevel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tboxVMessID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnVMessInsertClient
            // 
            resources.ApplyResources(this.btnVMessInsertClient, "btnVMessInsertClient");
            this.btnVMessInsertClient.Name = "btnVMessInsertClient";
            this.btnVMessInsertClient.UseVisualStyleBackColor = true;
            this.btnVMessInsertClient.Click += new System.EventHandler(this.btnVMessInsertClient_Click);
            // 
            // btnVMessGenUUID
            // 
            resources.ApplyResources(this.btnVMessGenUUID, "btnVMessGenUUID");
            this.btnVMessGenUUID.Name = "btnVMessGenUUID";
            this.btnVMessGenUUID.UseVisualStyleBackColor = true;
            this.btnVMessGenUUID.Click += new System.EventHandler(this.btnVMessGenUUID_Click);
            // 
            // tboxVMessIPaddr
            // 
            resources.ApplyResources(this.tboxVMessIPaddr, "tboxVMessIPaddr");
            this.tboxVMessIPaddr.Name = "tboxVMessIPaddr";
            // 
            // tboxVMessAid
            // 
            resources.ApplyResources(this.tboxVMessAid, "tboxVMessAid");
            this.tboxVMessAid.Name = "tboxVMessAid";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tboxVMessLevel
            // 
            resources.ApplyResources(this.tboxVMessLevel, "tboxVMessLevel");
            this.tboxVMessLevel.Name = "tboxVMessLevel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tboxVMessID
            // 
            resources.ApplyResources(this.tboxVMessID, "tboxVMessID");
            this.tboxVMessID.Name = "tboxVMessID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.btnSSRInsertClient);
            this.groupBox3.Controls.Add(this.cboxSSROTA);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cboxSSRMethod);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cboxShowPassWord);
            this.groupBox3.Controls.Add(this.tboxSSRPass);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tboxSSRAddr);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tboxSSREmail);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // btnSSRInsertClient
            // 
            resources.ApplyResources(this.btnSSRInsertClient, "btnSSRInsertClient");
            this.btnSSRInsertClient.Name = "btnSSRInsertClient";
            this.btnSSRInsertClient.UseVisualStyleBackColor = true;
            this.btnSSRInsertClient.Click += new System.EventHandler(this.btnSSRInsertClient_Click);
            // 
            // cboxSSROTA
            // 
            resources.ApplyResources(this.cboxSSROTA, "cboxSSROTA");
            this.cboxSSROTA.FormattingEnabled = true;
            this.cboxSSROTA.Items.AddRange(new object[] {
            resources.GetString("cboxSSROTA.Items"),
            resources.GetString("cboxSSROTA.Items1")});
            this.cboxSSROTA.Name = "cboxSSROTA";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cboxSSRMethod
            // 
            resources.ApplyResources(this.cboxSSRMethod, "cboxSSRMethod");
            this.cboxSSRMethod.FormattingEnabled = true;
            this.cboxSSRMethod.Items.AddRange(new object[] {
            resources.GetString("cboxSSRMethod.Items"),
            resources.GetString("cboxSSRMethod.Items1"),
            resources.GetString("cboxSSRMethod.Items2"),
            resources.GetString("cboxSSRMethod.Items3"),
            resources.GetString("cboxSSRMethod.Items4"),
            resources.GetString("cboxSSRMethod.Items5"),
            resources.GetString("cboxSSRMethod.Items6"),
            resources.GetString("cboxSSRMethod.Items7")});
            this.cboxSSRMethod.Name = "cboxSSRMethod";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cboxShowPassWord
            // 
            resources.ApplyResources(this.cboxShowPassWord, "cboxShowPassWord");
            this.cboxShowPassWord.Name = "cboxShowPassWord";
            this.cboxShowPassWord.UseVisualStyleBackColor = true;
            this.cboxShowPassWord.CheckedChanged += new System.EventHandler(this.cboxShowPassWord_CheckedChanged);
            // 
            // tboxSSRPass
            // 
            resources.ApplyResources(this.tboxSSRPass, "tboxSSRPass");
            this.tboxSSRPass.Name = "tboxSSRPass";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // tboxSSRAddr
            // 
            resources.ApplyResources(this.tboxSSRAddr, "tboxSSRAddr");
            this.tboxSSRAddr.Name = "tboxSSRAddr";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // tboxSSREmail
            // 
            resources.ApplyResources(this.tboxSSREmail, "tboxSSREmail");
            this.tboxSSREmail.Name = "tboxSSREmail";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.cboxStreamSecurity);
            this.groupBox2.Controls.Add(this.btnStreamInsertTCP);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btnStreamInsertWS);
            this.groupBox2.Controls.Add(this.tboxWSPath);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnStreamInsertKCP);
            this.groupBox2.Controls.Add(this.tboxKCPType);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // cboxStreamSecurity
            // 
            resources.ApplyResources(this.cboxStreamSecurity, "cboxStreamSecurity");
            this.cboxStreamSecurity.FormattingEnabled = true;
            this.cboxStreamSecurity.Items.AddRange(new object[] {
            resources.GetString("cboxStreamSecurity.Items"),
            resources.GetString("cboxStreamSecurity.Items1")});
            this.cboxStreamSecurity.Name = "cboxStreamSecurity";
            // 
            // btnStreamInsertTCP
            // 
            resources.ApplyResources(this.btnStreamInsertTCP, "btnStreamInsertTCP");
            this.btnStreamInsertTCP.Name = "btnStreamInsertTCP";
            this.btnStreamInsertTCP.UseVisualStyleBackColor = true;
            this.btnStreamInsertTCP.Click += new System.EventHandler(this.btnStreamInsertTCP_Click);
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // btnStreamInsertWS
            // 
            resources.ApplyResources(this.btnStreamInsertWS, "btnStreamInsertWS");
            this.btnStreamInsertWS.Name = "btnStreamInsertWS";
            this.btnStreamInsertWS.UseVisualStyleBackColor = true;
            this.btnStreamInsertWS.Click += new System.EventHandler(this.btnStreamInsertWS_Click);
            // 
            // tboxWSPath
            // 
            resources.ApplyResources(this.tboxWSPath, "tboxWSPath");
            this.tboxWSPath.Name = "tboxWSPath";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // btnStreamInsertKCP
            // 
            resources.ApplyResources(this.btnStreamInsertKCP, "btnStreamInsertKCP");
            this.btnStreamInsertKCP.Name = "btnStreamInsertKCP";
            this.btnStreamInsertKCP.UseVisualStyleBackColor = true;
            this.btnStreamInsertKCP.Click += new System.EventHandler(this.btnStreamInsertKCP_Click);
            // 
            // tboxKCPType
            // 
            resources.ApplyResources(this.tboxKCPType, "tboxKCPType");
            this.tboxKCPType.Name = "tboxKCPType";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // btnSaveModify
            // 
            resources.ApplyResources(this.btnSaveModify, "btnSaveModify");
            this.btnSaveModify.Name = "btnSaveModify";
            this.btnSaveModify.UseVisualStyleBackColor = true;
            this.btnSaveModify.Click += new System.EventHandler(this.btnSaveModify_Click);
            // 
            // btnClearModify
            // 
            resources.ApplyResources(this.btnClearModify, "btnClearModify");
            this.btnClearModify.Name = "btnClearModify";
            this.btnClearModify.UseVisualStyleBackColor = true;
            this.btnClearModify.Click += new System.EventHandler(this.btnClearModify_Click);
            // 
            // btnLoadDefault
            // 
            resources.ApplyResources(this.btnLoadDefault, "btnLoadDefault");
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.btnLoadDefault.UseVisualStyleBackColor = true;
            this.btnLoadDefault.Click += new System.EventHandler(this.btnLoadDefault_Click);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // cboxServList
            // 
            resources.ApplyResources(this.cboxServList, "cboxServList");
            this.cboxServList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxServList.FormattingEnabled = true;
            this.cboxServList.Name = "cboxServList";
            // 
            // panelScintilla
            // 
            resources.ApplyResources(this.panelScintilla, "panelScintilla");
            this.panelScintilla.Name = "panelScintilla";
            // 
            // FormConfiger
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelScintilla);
            this.Controls.Add(this.cboxServList);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnLoadDefault);
            this.Controls.Add(this.btnClearModify);
            this.Controls.Add(this.btnSaveModify);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cboxConfigPart);
            this.Controls.Add(this.btnOverwriteConfig);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInsertNewServ);
            this.Name = "FormConfiger";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInsertNewServ;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOverwriteConfig;
        private System.Windows.Forms.ComboBox cboxConfigPart;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSaveModify;
        private System.Windows.Forms.Button btnClearModify;
        private System.Windows.Forms.Button btnLoadDefault;
        private System.Windows.Forms.Button btnVMessInsertClient;
        private System.Windows.Forms.TextBox tboxVMessIPaddr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tboxVMessAid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxVMessLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tboxVMessID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVMessGenUUID;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSSRInsertClient;
        private System.Windows.Forms.ComboBox cboxSSROTA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboxSSRMethod;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cboxShowPassWord;
        private System.Windows.Forms.TextBox tboxSSRPass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tboxSSRAddr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tboxSSREmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboxStreamSecurity;
        private System.Windows.Forms.Button btnStreamInsertTCP;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnStreamInsertWS;
        private System.Windows.Forms.TextBox tboxWSPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStreamInsertKCP;
        private System.Windows.Forms.TextBox tboxKCPType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cboxServList;
        private System.Windows.Forms.Panel panelScintilla;
    }
}