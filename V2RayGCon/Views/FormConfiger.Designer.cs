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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiger));
            this.btnInsertNewServ = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOverwriteConfig = new System.Windows.Forms.Button();
            this.cboxConfigSection = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnInsertVServ = new System.Windows.Forms.Button();
            this.btnGenVServID = new System.Windows.Forms.Button();
            this.tboxVServPort = new System.Windows.Forms.TextBox();
            this.tboxVServAID = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tboxVServLevel = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tboxVServID = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSSInsertServer = new System.Windows.Forms.Button();
            this.chkSSSShowPass = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tboxSSSPass = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cboxSSSMethod = new System.Windows.Forms.ComboBox();
            this.tboxSSSPort = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tboxSSSEmail = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.chkSSSOTA = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboxSSSNetwork = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkSSCOTA = new System.Windows.Forms.CheckBox();
            this.btnSSRInsertClient = new System.Windows.Forms.Button();
            this.cboxSSCMethod = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkSSCShowPass = new System.Windows.Forms.CheckBox();
            this.tboxSSCPass = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tboxSSCAddr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tboxSSCEmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkStreamIsInbound = new System.Windows.Forms.CheckBox();
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInsertNewServ
            // 
            resources.ApplyResources(this.btnInsertNewServ, "btnInsertNewServ");
            this.btnInsertNewServ.Name = "btnInsertNewServ";
            this.toolTip1.SetToolTip(this.btnInsertNewServ, resources.GetString("btnInsertNewServ.ToolTip"));
            this.btnInsertNewServ.UseVisualStyleBackColor = true;
            this.btnInsertNewServ.Click += new System.EventHandler(this.btnInsertNewServ_Click);
            // 
            // btnExit
            // 
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.toolTip1.SetToolTip(this.btnExit, resources.GetString("btnExit.ToolTip"));
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOverwriteConfig
            // 
            resources.ApplyResources(this.btnOverwriteConfig, "btnOverwriteConfig");
            this.btnOverwriteConfig.Name = "btnOverwriteConfig";
            this.toolTip1.SetToolTip(this.btnOverwriteConfig, resources.GetString("btnOverwriteConfig.ToolTip"));
            this.btnOverwriteConfig.UseVisualStyleBackColor = true;
            this.btnOverwriteConfig.Click += new System.EventHandler(this.btnOverwriteServConfig_Click);
            // 
            // cboxConfigSection
            // 
            resources.ApplyResources(this.cboxConfigSection, "cboxConfigSection");
            this.cboxConfigSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConfigSection.FormattingEnabled = true;
            this.cboxConfigSection.Items.AddRange(new object[] {
            resources.GetString("cboxConfigSection.Items"),
            resources.GetString("cboxConfigSection.Items1")});
            this.cboxConfigSection.Name = "cboxConfigSection";
            this.toolTip1.SetToolTip(this.cboxConfigSection, resources.GetString("cboxConfigSection.ToolTip"));
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.toolTip1.SetToolTip(this.tabControl1, resources.GetString("tabControl1.ToolTip"));
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Name = "tabPage1";
            this.toolTip1.SetToolTip(this.tabPage1, resources.GetString("tabPage1.ToolTip"));
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.btnInsertVServ);
            this.groupBox5.Controls.Add(this.btnGenVServID);
            this.groupBox5.Controls.Add(this.tboxVServPort);
            this.groupBox5.Controls.Add(this.tboxVServAID);
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.label22);
            this.groupBox5.Controls.Add(this.tboxVServLevel);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.tboxVServID);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox5, resources.GetString("groupBox5.ToolTip"));
            // 
            // btnInsertVServ
            // 
            resources.ApplyResources(this.btnInsertVServ, "btnInsertVServ");
            this.btnInsertVServ.Name = "btnInsertVServ";
            this.toolTip1.SetToolTip(this.btnInsertVServ, resources.GetString("btnInsertVServ.ToolTip"));
            this.btnInsertVServ.UseVisualStyleBackColor = true;
            this.btnInsertVServ.Click += new System.EventHandler(this.btnInsertVServ_Click);
            // 
            // btnGenVServID
            // 
            resources.ApplyResources(this.btnGenVServID, "btnGenVServID");
            this.btnGenVServID.Name = "btnGenVServID";
            this.toolTip1.SetToolTip(this.btnGenVServID, resources.GetString("btnGenVServID.ToolTip"));
            this.btnGenVServID.UseVisualStyleBackColor = true;
            this.btnGenVServID.Click += new System.EventHandler(this.btnGenVServID_Click);
            // 
            // tboxVServPort
            // 
            resources.ApplyResources(this.tboxVServPort, "tboxVServPort");
            this.tboxVServPort.Name = "tboxVServPort";
            this.toolTip1.SetToolTip(this.tboxVServPort, resources.GetString("tboxVServPort.ToolTip"));
            // 
            // tboxVServAID
            // 
            resources.ApplyResources(this.tboxVServAID, "tboxVServAID");
            this.tboxVServAID.Name = "tboxVServAID";
            this.toolTip1.SetToolTip(this.tboxVServAID, resources.GetString("tboxVServAID.ToolTip"));
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            this.toolTip1.SetToolTip(this.label21, resources.GetString("label21.ToolTip"));
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            this.toolTip1.SetToolTip(this.label22, resources.GetString("label22.ToolTip"));
            // 
            // tboxVServLevel
            // 
            resources.ApplyResources(this.tboxVServLevel, "tboxVServLevel");
            this.tboxVServLevel.Name = "tboxVServLevel";
            this.toolTip1.SetToolTip(this.tboxVServLevel, resources.GetString("tboxVServLevel.ToolTip"));
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            this.toolTip1.SetToolTip(this.label23, resources.GetString("label23.ToolTip"));
            // 
            // tboxVServID
            // 
            resources.ApplyResources(this.tboxVServID, "tboxVServID");
            this.tboxVServID.Name = "tboxVServID";
            this.toolTip1.SetToolTip(this.tboxVServID, resources.GetString("tboxVServID.ToolTip"));
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            this.toolTip1.SetToolTip(this.label24, resources.GetString("label24.ToolTip"));
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
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // btnVMessInsertClient
            // 
            resources.ApplyResources(this.btnVMessInsertClient, "btnVMessInsertClient");
            this.btnVMessInsertClient.Name = "btnVMessInsertClient";
            this.toolTip1.SetToolTip(this.btnVMessInsertClient, resources.GetString("btnVMessInsertClient.ToolTip"));
            this.btnVMessInsertClient.UseVisualStyleBackColor = true;
            this.btnVMessInsertClient.Click += new System.EventHandler(this.btnVMessInsertClient_Click);
            // 
            // btnVMessGenUUID
            // 
            resources.ApplyResources(this.btnVMessGenUUID, "btnVMessGenUUID");
            this.btnVMessGenUUID.Name = "btnVMessGenUUID";
            this.toolTip1.SetToolTip(this.btnVMessGenUUID, resources.GetString("btnVMessGenUUID.ToolTip"));
            this.btnVMessGenUUID.UseVisualStyleBackColor = true;
            this.btnVMessGenUUID.Click += new System.EventHandler(this.btnVMessGenUUID_Click);
            // 
            // tboxVMessIPaddr
            // 
            resources.ApplyResources(this.tboxVMessIPaddr, "tboxVMessIPaddr");
            this.tboxVMessIPaddr.Name = "tboxVMessIPaddr";
            this.toolTip1.SetToolTip(this.tboxVMessIPaddr, resources.GetString("tboxVMessIPaddr.ToolTip"));
            // 
            // tboxVMessAid
            // 
            resources.ApplyResources(this.tboxVMessAid, "tboxVMessAid");
            this.tboxVMessAid.Name = "tboxVMessAid";
            this.toolTip1.SetToolTip(this.tboxVMessAid, resources.GetString("tboxVMessAid.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.toolTip1.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // tboxVMessLevel
            // 
            resources.ApplyResources(this.tboxVMessLevel, "tboxVMessLevel");
            this.tboxVMessLevel.Name = "tboxVMessLevel";
            this.toolTip1.SetToolTip(this.tboxVMessLevel, resources.GetString("tboxVMessLevel.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // tboxVMessID
            // 
            resources.ApplyResources(this.tboxVMessID, "tboxVMessID");
            this.tboxVMessID.Name = "tboxVMessID";
            this.toolTip1.SetToolTip(this.tboxVMessID, resources.GetString("tboxVMessID.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Name = "tabPage2";
            this.toolTip1.SetToolTip(this.tabPage2, resources.GetString("tabPage2.ToolTip"));
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.btnSSInsertServer);
            this.groupBox4.Controls.Add(this.chkSSSShowPass);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.tboxSSSPass);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.cboxSSSMethod);
            this.groupBox4.Controls.Add(this.tboxSSSPort);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.tboxSSSEmail);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.chkSSSOTA);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.cboxSSSNetwork);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
            // 
            // btnSSInsertServer
            // 
            resources.ApplyResources(this.btnSSInsertServer, "btnSSInsertServer");
            this.btnSSInsertServer.Name = "btnSSInsertServer";
            this.toolTip1.SetToolTip(this.btnSSInsertServer, resources.GetString("btnSSInsertServer.ToolTip"));
            this.btnSSInsertServer.UseVisualStyleBackColor = true;
            this.btnSSInsertServer.Click += new System.EventHandler(this.btnSSInsertServer_Click);
            // 
            // chkSSSShowPass
            // 
            resources.ApplyResources(this.chkSSSShowPass, "chkSSSShowPass");
            this.chkSSSShowPass.Name = "chkSSSShowPass";
            this.toolTip1.SetToolTip(this.chkSSSShowPass, resources.GetString("chkSSSShowPass.ToolTip"));
            this.chkSSSShowPass.UseVisualStyleBackColor = true;
            this.chkSSSShowPass.CheckedChanged += new System.EventHandler(this.chkSSSShowPass_CheckedChanged);
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            this.toolTip1.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
            // 
            // tboxSSSPass
            // 
            resources.ApplyResources(this.tboxSSSPass, "tboxSSSPass");
            this.tboxSSSPass.Name = "tboxSSSPass";
            this.toolTip1.SetToolTip(this.tboxSSSPass, resources.GetString("tboxSSSPass.ToolTip"));
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            this.toolTip1.SetToolTip(this.label19, resources.GetString("label19.ToolTip"));
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            this.toolTip1.SetToolTip(this.label18, resources.GetString("label18.ToolTip"));
            // 
            // cboxSSSMethod
            // 
            resources.ApplyResources(this.cboxSSSMethod, "cboxSSSMethod");
            this.cboxSSSMethod.FormattingEnabled = true;
            this.cboxSSSMethod.Name = "cboxSSSMethod";
            this.toolTip1.SetToolTip(this.cboxSSSMethod, resources.GetString("cboxSSSMethod.ToolTip"));
            // 
            // tboxSSSPort
            // 
            resources.ApplyResources(this.tboxSSSPort, "tboxSSSPort");
            this.tboxSSSPort.Name = "tboxSSSPort";
            this.toolTip1.SetToolTip(this.tboxSSSPort, resources.GetString("tboxSSSPort.ToolTip"));
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            this.toolTip1.SetToolTip(this.label17, resources.GetString("label17.ToolTip"));
            // 
            // tboxSSSEmail
            // 
            resources.ApplyResources(this.tboxSSSEmail, "tboxSSSEmail");
            this.tboxSSSEmail.Name = "tboxSSSEmail";
            this.toolTip1.SetToolTip(this.tboxSSSEmail, resources.GetString("tboxSSSEmail.ToolTip"));
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            this.toolTip1.SetToolTip(this.label16, resources.GetString("label16.ToolTip"));
            // 
            // chkSSSOTA
            // 
            resources.ApplyResources(this.chkSSSOTA, "chkSSSOTA");
            this.chkSSSOTA.Name = "chkSSSOTA";
            this.toolTip1.SetToolTip(this.chkSSSOTA, resources.GetString("chkSSSOTA.ToolTip"));
            this.chkSSSOTA.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            this.toolTip1.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
            // 
            // cboxSSSNetwork
            // 
            resources.ApplyResources(this.cboxSSSNetwork, "cboxSSSNetwork");
            this.cboxSSSNetwork.FormattingEnabled = true;
            this.cboxSSSNetwork.Name = "cboxSSSNetwork";
            this.toolTip1.SetToolTip(this.cboxSSSNetwork, resources.GetString("cboxSSSNetwork.ToolTip"));
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.chkSSCOTA);
            this.groupBox3.Controls.Add(this.btnSSRInsertClient);
            this.groupBox3.Controls.Add(this.cboxSSCMethod);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chkSSCShowPass);
            this.groupBox3.Controls.Add(this.tboxSSCPass);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tboxSSCAddr);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tboxSSCEmail);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
            // 
            // chkSSCOTA
            // 
            resources.ApplyResources(this.chkSSCOTA, "chkSSCOTA");
            this.chkSSCOTA.Name = "chkSSCOTA";
            this.toolTip1.SetToolTip(this.chkSSCOTA, resources.GetString("chkSSCOTA.ToolTip"));
            this.chkSSCOTA.UseVisualStyleBackColor = true;
            // 
            // btnSSRInsertClient
            // 
            resources.ApplyResources(this.btnSSRInsertClient, "btnSSRInsertClient");
            this.btnSSRInsertClient.Name = "btnSSRInsertClient";
            this.toolTip1.SetToolTip(this.btnSSRInsertClient, resources.GetString("btnSSRInsertClient.ToolTip"));
            this.btnSSRInsertClient.UseVisualStyleBackColor = true;
            this.btnSSRInsertClient.Click += new System.EventHandler(this.btnSSRInsertClient_Click);
            // 
            // cboxSSCMethod
            // 
            resources.ApplyResources(this.cboxSSCMethod, "cboxSSCMethod");
            this.cboxSSCMethod.FormattingEnabled = true;
            this.cboxSSCMethod.Items.AddRange(new object[] {
            resources.GetString("cboxSSCMethod.Items")});
            this.cboxSSCMethod.Name = "cboxSSCMethod";
            this.toolTip1.SetToolTip(this.cboxSSCMethod, resources.GetString("cboxSSCMethod.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            this.toolTip1.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // chkSSCShowPass
            // 
            resources.ApplyResources(this.chkSSCShowPass, "chkSSCShowPass");
            this.chkSSCShowPass.Name = "chkSSCShowPass";
            this.toolTip1.SetToolTip(this.chkSSCShowPass, resources.GetString("chkSSCShowPass.ToolTip"));
            this.chkSSCShowPass.UseVisualStyleBackColor = true;
            this.chkSSCShowPass.CheckedChanged += new System.EventHandler(this.cboxShowPassWord_CheckedChanged);
            // 
            // tboxSSCPass
            // 
            resources.ApplyResources(this.tboxSSCPass, "tboxSSCPass");
            this.tboxSSCPass.Name = "tboxSSCPass";
            this.toolTip1.SetToolTip(this.tboxSSCPass, resources.GetString("tboxSSCPass.ToolTip"));
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            this.toolTip1.SetToolTip(this.label8, resources.GetString("label8.ToolTip"));
            // 
            // tboxSSCAddr
            // 
            resources.ApplyResources(this.tboxSSCAddr, "tboxSSCAddr");
            this.tboxSSCAddr.Name = "tboxSSCAddr";
            this.toolTip1.SetToolTip(this.tboxSSCAddr, resources.GetString("tboxSSCAddr.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            this.toolTip1.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // tboxSSCEmail
            // 
            resources.ApplyResources(this.tboxSSCEmail, "tboxSSCEmail");
            this.tboxSSCEmail.Name = "tboxSSCEmail";
            this.toolTip1.SetToolTip(this.tboxSSCEmail, resources.GetString("tboxSSCEmail.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Name = "tabPage3";
            this.toolTip1.SetToolTip(this.tabPage3, resources.GetString("tabPage3.ToolTip"));
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.chkStreamIsInbound);
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
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // chkStreamIsInbound
            // 
            resources.ApplyResources(this.chkStreamIsInbound, "chkStreamIsInbound");
            this.chkStreamIsInbound.Name = "chkStreamIsInbound";
            this.toolTip1.SetToolTip(this.chkStreamIsInbound, resources.GetString("chkStreamIsInbound.ToolTip"));
            this.chkStreamIsInbound.UseVisualStyleBackColor = true;
            // 
            // cboxStreamSecurity
            // 
            resources.ApplyResources(this.cboxStreamSecurity, "cboxStreamSecurity");
            this.cboxStreamSecurity.FormattingEnabled = true;
            this.cboxStreamSecurity.Items.AddRange(new object[] {
            resources.GetString("cboxStreamSecurity.Items"),
            resources.GetString("cboxStreamSecurity.Items1")});
            this.cboxStreamSecurity.Name = "cboxStreamSecurity";
            this.toolTip1.SetToolTip(this.cboxStreamSecurity, resources.GetString("cboxStreamSecurity.ToolTip"));
            // 
            // btnStreamInsertTCP
            // 
            resources.ApplyResources(this.btnStreamInsertTCP, "btnStreamInsertTCP");
            this.btnStreamInsertTCP.Name = "btnStreamInsertTCP";
            this.toolTip1.SetToolTip(this.btnStreamInsertTCP, resources.GetString("btnStreamInsertTCP.ToolTip"));
            this.btnStreamInsertTCP.UseVisualStyleBackColor = true;
            this.btnStreamInsertTCP.Click += new System.EventHandler(this.btnStreamInsertTCP_Click);
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.toolTip1.SetToolTip(this.textBox3, resources.GetString("textBox3.ToolTip"));
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            this.toolTip1.SetToolTip(this.label14, resources.GetString("label14.ToolTip"));
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            this.toolTip1.SetToolTip(this.label13, resources.GetString("label13.ToolTip"));
            // 
            // btnStreamInsertWS
            // 
            resources.ApplyResources(this.btnStreamInsertWS, "btnStreamInsertWS");
            this.btnStreamInsertWS.Name = "btnStreamInsertWS";
            this.toolTip1.SetToolTip(this.btnStreamInsertWS, resources.GetString("btnStreamInsertWS.ToolTip"));
            this.btnStreamInsertWS.UseVisualStyleBackColor = true;
            this.btnStreamInsertWS.Click += new System.EventHandler(this.btnStreamInsertWS_Click);
            // 
            // tboxWSPath
            // 
            resources.ApplyResources(this.tboxWSPath, "tboxWSPath");
            this.tboxWSPath.Name = "tboxWSPath";
            this.toolTip1.SetToolTip(this.tboxWSPath, resources.GetString("tboxWSPath.ToolTip"));
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            this.toolTip1.SetToolTip(this.label12, resources.GetString("label12.ToolTip"));
            // 
            // btnStreamInsertKCP
            // 
            resources.ApplyResources(this.btnStreamInsertKCP, "btnStreamInsertKCP");
            this.btnStreamInsertKCP.Name = "btnStreamInsertKCP";
            this.toolTip1.SetToolTip(this.btnStreamInsertKCP, resources.GetString("btnStreamInsertKCP.ToolTip"));
            this.btnStreamInsertKCP.UseVisualStyleBackColor = true;
            this.btnStreamInsertKCP.Click += new System.EventHandler(this.btnStreamInsertKCP_Click);
            // 
            // tboxKCPType
            // 
            resources.ApplyResources(this.tboxKCPType, "tboxKCPType");
            this.tboxKCPType.Name = "tboxKCPType";
            this.toolTip1.SetToolTip(this.tboxKCPType, resources.GetString("tboxKCPType.ToolTip"));
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            this.toolTip1.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
            // 
            // btnSaveModify
            // 
            resources.ApplyResources(this.btnSaveModify, "btnSaveModify");
            this.btnSaveModify.Name = "btnSaveModify";
            this.toolTip1.SetToolTip(this.btnSaveModify, resources.GetString("btnSaveModify.ToolTip"));
            this.btnSaveModify.UseVisualStyleBackColor = true;
            this.btnSaveModify.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // btnClearModify
            // 
            resources.ApplyResources(this.btnClearModify, "btnClearModify");
            this.btnClearModify.Name = "btnClearModify";
            this.toolTip1.SetToolTip(this.btnClearModify, resources.GetString("btnClearModify.ToolTip"));
            this.btnClearModify.UseVisualStyleBackColor = true;
            this.btnClearModify.Click += new System.EventHandler(this.btnDiscardChanges_Click);
            // 
            // btnLoadDefault
            // 
            resources.ApplyResources(this.btnLoadDefault, "btnLoadDefault");
            this.btnLoadDefault.Name = "btnLoadDefault";
            this.toolTip1.SetToolTip(this.btnLoadDefault, resources.GetString("btnLoadDefault.ToolTip"));
            this.btnLoadDefault.UseVisualStyleBackColor = true;
            this.btnLoadDefault.Click += new System.EventHandler(this.btnLoadExample_Click);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            this.toolTip1.SetToolTip(this.label15, resources.GetString("label15.ToolTip"));
            // 
            // cboxServList
            // 
            resources.ApplyResources(this.cboxServList, "cboxServList");
            this.cboxServList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxServList.FormattingEnabled = true;
            this.cboxServList.Name = "cboxServList";
            this.toolTip1.SetToolTip(this.cboxServList, resources.GetString("cboxServList.ToolTip"));
            // 
            // panelScintilla
            // 
            resources.ApplyResources(this.panelScintilla, "panelScintilla");
            this.panelScintilla.Name = "panelScintilla";
            this.toolTip1.SetToolTip(this.panelScintilla, resources.GetString("panelScintilla.ToolTip"));
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
            this.Controls.Add(this.cboxConfigSection);
            this.Controls.Add(this.btnOverwriteConfig);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnInsertNewServ);
            this.Name = "FormConfiger";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.ComboBox cboxConfigSection;
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
        private System.Windows.Forms.ComboBox cboxSSCMethod;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkSSCShowPass;
        private System.Windows.Forms.TextBox tboxSSCPass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tboxSSCAddr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tboxSSCEmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStreamInsertTCP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnStreamInsertWS;
        private System.Windows.Forms.TextBox tboxWSPath;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStreamInsertKCP;
        private System.Windows.Forms.TextBox tboxKCPType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cboxServList;
        private System.Windows.Forms.Panel panelScintilla;
        private System.Windows.Forms.CheckBox chkSSCOTA;
        private System.Windows.Forms.ComboBox cboxStreamSecurity;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tboxSSSEmail;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox chkSSSOTA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboxSSSNetwork;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cboxSSSMethod;
        private System.Windows.Forms.TextBox tboxSSSPort;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnSSInsertServer;
        private System.Windows.Forms.CheckBox chkSSSShowPass;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tboxSSSPass;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox chkStreamIsInbound;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnInsertVServ;
        private System.Windows.Forms.Button btnGenVServID;
        private System.Windows.Forms.TextBox tboxVServPort;
        private System.Windows.Forms.TextBox tboxVServAID;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tboxVServLevel;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tboxVServID;
        private System.Windows.Forms.Label label24;
    }
}