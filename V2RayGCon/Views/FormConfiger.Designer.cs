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
            this.cboxConfigSection = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.vmess = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnStreamOutbound = new System.Windows.Forms.RadioButton();
            this.rbtnStreamInbound = new System.Windows.Forms.RadioButton();
            this.tboxWSPath = new System.Windows.Forms.TextBox();
            this.cboxKCPType = new System.Windows.Forms.ComboBox();
            this.cboxTCPType = new System.Windows.Forms.ComboBox();
            this.cboxStreamSecurity = new System.Windows.Forms.ComboBox();
            this.btnStreamInsertTCP = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnStreamInsertWS = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnStreamInsertKCP = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbtnVmessIServerMode = new System.Windows.Forms.RadioButton();
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
            this.ss = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnSSInsertServer = new System.Windows.Forms.Button();
            this.chkSSSShowPass = new System.Windows.Forms.CheckBox();
            this.tboxSSSPass = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cboxSSSMethod = new System.Windows.Forms.ComboBox();
            this.tboxSSSPort = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
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
            this.label6 = new System.Windows.Forms.Label();
            this.misc = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnQConSkipCN = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tboxVGCDesc = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnVGC = new System.Windows.Forms.Button();
            this.tboxVGCAlias = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabExpanseImport = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnExpanseImport = new System.Windows.Forms.Button();
            this.btnCopyExpansedConfig = new System.Windows.Forms.Button();
            this.panelExpandConfig = new System.Windows.Forms.Panel();
            this.btnClearModify = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWinToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.loadJsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveConfigStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.loadServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceExistServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.showLeftPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideLeftPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlTools = new System.Windows.Forms.Panel();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelScintilla = new System.Windows.Forms.Panel();
            this.btnFormat = new System.Windows.Forms.Button();
            this.cboxExamples = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.vmess.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ss.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.misc.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabExpanseImport.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.pnlTools.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboxConfigSection
            // 
            this.cboxConfigSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cboxConfigSection, "cboxConfigSection");
            this.cboxConfigSection.FormattingEnabled = true;
            this.cboxConfigSection.Items.AddRange(new object[] {
            resources.GetString("cboxConfigSection.Items"),
            resources.GetString("cboxConfigSection.Items1")});
            this.cboxConfigSection.Name = "cboxConfigSection";
            this.cboxConfigSection.SelectedIndexChanged += new System.EventHandler(this.cboxConfigSection_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.vmess);
            this.tabControl1.Controls.Add(this.ss);
            this.tabControl1.Controls.Add(this.misc);
            this.tabControl1.Controls.Add(this.tabExpanseImport);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // vmess
            // 
            this.vmess.Controls.Add(this.groupBox2);
            this.vmess.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.vmess, "vmess");
            this.vmess.Name = "vmess";
            this.vmess.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnStreamOutbound);
            this.groupBox2.Controls.Add(this.rbtnStreamInbound);
            this.groupBox2.Controls.Add(this.tboxWSPath);
            this.groupBox2.Controls.Add(this.cboxKCPType);
            this.groupBox2.Controls.Add(this.cboxTCPType);
            this.groupBox2.Controls.Add(this.cboxStreamSecurity);
            this.groupBox2.Controls.Add(this.btnStreamInsertTCP);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btnStreamInsertWS);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnStreamInsertKCP);
            this.groupBox2.Controls.Add(this.label11);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // rbtnStreamOutbound
            // 
            resources.ApplyResources(this.rbtnStreamOutbound, "rbtnStreamOutbound");
            this.rbtnStreamOutbound.Checked = true;
            this.rbtnStreamOutbound.Name = "rbtnStreamOutbound";
            this.rbtnStreamOutbound.TabStop = true;
            this.rbtnStreamOutbound.UseVisualStyleBackColor = true;
            // 
            // rbtnStreamInbound
            // 
            resources.ApplyResources(this.rbtnStreamInbound, "rbtnStreamInbound");
            this.rbtnStreamInbound.Name = "rbtnStreamInbound";
            this.rbtnStreamInbound.UseVisualStyleBackColor = true;
            this.rbtnStreamInbound.CheckedChanged += new System.EventHandler(this.rbtnStreamInbound_CheckedChanged);
            // 
            // tboxWSPath
            // 
            resources.ApplyResources(this.tboxWSPath, "tboxWSPath");
            this.tboxWSPath.Name = "tboxWSPath";
            this.toolTip1.SetToolTip(this.tboxWSPath, resources.GetString("tboxWSPath.ToolTip"));
            // 
            // cboxKCPType
            // 
            this.cboxKCPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxKCPType.FormattingEnabled = true;
            this.cboxKCPType.Items.AddRange(new object[] {
            resources.GetString("cboxKCPType.Items")});
            resources.ApplyResources(this.cboxKCPType, "cboxKCPType");
            this.cboxKCPType.Name = "cboxKCPType";
            // 
            // cboxTCPType
            // 
            this.cboxTCPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxTCPType.FormattingEnabled = true;
            this.cboxTCPType.Items.AddRange(new object[] {
            resources.GetString("cboxTCPType.Items")});
            resources.ApplyResources(this.cboxTCPType, "cboxTCPType");
            this.cboxTCPType.Name = "cboxTCPType";
            // 
            // cboxStreamSecurity
            // 
            this.cboxStreamSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxStreamSecurity.FormattingEnabled = true;
            this.cboxStreamSecurity.Items.AddRange(new object[] {
            resources.GetString("cboxStreamSecurity.Items"),
            resources.GetString("cboxStreamSecurity.Items1")});
            resources.ApplyResources(this.cboxStreamSecurity, "cboxStreamSecurity");
            this.cboxStreamSecurity.Name = "cboxStreamSecurity";
            // 
            // btnStreamInsertTCP
            // 
            resources.ApplyResources(this.btnStreamInsertTCP, "btnStreamInsertTCP");
            this.btnStreamInsertTCP.Name = "btnStreamInsertTCP";
            this.btnStreamInsertTCP.UseVisualStyleBackColor = true;
            this.btnStreamInsertTCP.Click += new System.EventHandler(this.btnStreamInsertTCP_Click);
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
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.rbtnVmessIServerMode);
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
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbtnVmessIServerMode
            // 
            resources.ApplyResources(this.rbtnVmessIServerMode, "rbtnVmessIServerMode");
            this.rbtnVmessIServerMode.Name = "rbtnVmessIServerMode";
            this.rbtnVmessIServerMode.UseVisualStyleBackColor = true;
            this.rbtnVmessIServerMode.CheckedChanged += new System.EventHandler(this.rbtnVmessIServerMode_CheckedChanged);
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
            this.toolTip1.SetToolTip(this.tboxVMessLevel, resources.GetString("tboxVMessLevel.ToolTip"));
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
            this.toolTip1.SetToolTip(this.tboxVMessID, resources.GetString("tboxVMessID.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ss
            // 
            this.ss.Controls.Add(this.groupBox4);
            this.ss.Controls.Add(this.groupBox3);
            resources.ApplyResources(this.ss, "ss");
            this.ss.Name = "ss";
            this.ss.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSSInsertServer);
            this.groupBox4.Controls.Add(this.chkSSSShowPass);
            this.groupBox4.Controls.Add(this.tboxSSSPass);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.cboxSSSMethod);
            this.groupBox4.Controls.Add(this.tboxSSSPort);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.chkSSSOTA);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.cboxSSSNetwork);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // btnSSInsertServer
            // 
            resources.ApplyResources(this.btnSSInsertServer, "btnSSInsertServer");
            this.btnSSInsertServer.Name = "btnSSInsertServer";
            this.btnSSInsertServer.UseVisualStyleBackColor = true;
            this.btnSSInsertServer.Click += new System.EventHandler(this.btnSSInsertServer_Click);
            // 
            // chkSSSShowPass
            // 
            resources.ApplyResources(this.chkSSSShowPass, "chkSSSShowPass");
            this.chkSSSShowPass.Name = "chkSSSShowPass";
            this.chkSSSShowPass.UseVisualStyleBackColor = true;
            this.chkSSSShowPass.CheckedChanged += new System.EventHandler(this.chkSSSShowPass_CheckedChanged);
            // 
            // tboxSSSPass
            // 
            resources.ApplyResources(this.tboxSSSPass, "tboxSSSPass");
            this.tboxSSSPass.Name = "tboxSSSPass";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // cboxSSSMethod
            // 
            this.cboxSSSMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSSSMethod.FormattingEnabled = true;
            resources.ApplyResources(this.cboxSSSMethod, "cboxSSSMethod");
            this.cboxSSSMethod.Name = "cboxSSSMethod";
            // 
            // tboxSSSPort
            // 
            resources.ApplyResources(this.tboxSSSPort, "tboxSSSPort");
            this.tboxSSSPort.Name = "tboxSSSPort";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // chkSSSOTA
            // 
            resources.ApplyResources(this.chkSSSOTA, "chkSSSOTA");
            this.chkSSSOTA.Name = "chkSSSOTA";
            this.chkSSSOTA.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cboxSSSNetwork
            // 
            this.cboxSSSNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSSSNetwork.FormattingEnabled = true;
            resources.ApplyResources(this.cboxSSSNetwork, "cboxSSSNetwork");
            this.cboxSSSNetwork.Name = "cboxSSSNetwork";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkSSCOTA);
            this.groupBox3.Controls.Add(this.btnSSRInsertClient);
            this.groupBox3.Controls.Add(this.cboxSSCMethod);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chkSSCShowPass);
            this.groupBox3.Controls.Add(this.tboxSSCPass);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tboxSSCAddr);
            this.groupBox3.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkSSCOTA
            // 
            resources.ApplyResources(this.chkSSCOTA, "chkSSCOTA");
            this.chkSSCOTA.Name = "chkSSCOTA";
            this.chkSSCOTA.UseVisualStyleBackColor = true;
            // 
            // btnSSRInsertClient
            // 
            resources.ApplyResources(this.btnSSRInsertClient, "btnSSRInsertClient");
            this.btnSSRInsertClient.Name = "btnSSRInsertClient";
            this.btnSSRInsertClient.UseVisualStyleBackColor = true;
            this.btnSSRInsertClient.Click += new System.EventHandler(this.btnSSRInsertClient_Click);
            // 
            // cboxSSCMethod
            // 
            this.cboxSSCMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSSCMethod.FormattingEnabled = true;
            this.cboxSSCMethod.Items.AddRange(new object[] {
            resources.GetString("cboxSSCMethod.Items")});
            resources.ApplyResources(this.cboxSSCMethod, "cboxSSCMethod");
            this.cboxSSCMethod.Name = "cboxSSCMethod";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkSSCShowPass
            // 
            resources.ApplyResources(this.chkSSCShowPass, "chkSSCShowPass");
            this.chkSSCShowPass.Name = "chkSSCShowPass";
            this.chkSSCShowPass.UseVisualStyleBackColor = true;
            this.chkSSCShowPass.CheckedChanged += new System.EventHandler(this.cboxShowPassWord_CheckedChanged);
            // 
            // tboxSSCPass
            // 
            resources.ApplyResources(this.tboxSSCPass, "tboxSSCPass");
            this.tboxSSCPass.Name = "tboxSSCPass";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // tboxSSCAddr
            // 
            resources.ApplyResources(this.tboxSSCAddr, "tboxSSCAddr");
            this.tboxSSCAddr.Name = "tboxSSCAddr";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // misc
            // 
            this.misc.Controls.Add(this.groupBox5);
            this.misc.Controls.Add(this.groupBox6);
            resources.ApplyResources(this.misc, "misc");
            this.misc.Name = "misc";
            this.misc.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnQConSkipCN);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // btnQConSkipCN
            // 
            resources.ApplyResources(this.btnQConSkipCN, "btnQConSkipCN");
            this.btnQConSkipCN.Name = "btnQConSkipCN";
            this.toolTip1.SetToolTip(this.btnQConSkipCN, resources.GetString("btnQConSkipCN.ToolTip"));
            this.btnQConSkipCN.UseVisualStyleBackColor = true;
            this.btnQConSkipCN.Click += new System.EventHandler(this.btnQConSkipCN_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tboxVGCDesc);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.btnVGC);
            this.groupBox6.Controls.Add(this.tboxVGCAlias);
            this.groupBox6.Controls.Add(this.label15);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // tboxVGCDesc
            // 
            resources.ApplyResources(this.tboxVGCDesc, "tboxVGCDesc");
            this.tboxVGCDesc.Name = "tboxVGCDesc";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // btnVGC
            // 
            resources.ApplyResources(this.btnVGC, "btnVGC");
            this.btnVGC.Name = "btnVGC";
            this.btnVGC.UseVisualStyleBackColor = true;
            this.btnVGC.Click += new System.EventHandler(this.btnVGC_Click);
            // 
            // tboxVGCAlias
            // 
            resources.ApplyResources(this.tboxVGCAlias, "tboxVGCAlias");
            this.tboxVGCAlias.Name = "tboxVGCAlias";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // tabExpanseImport
            // 
            this.tabExpanseImport.Controls.Add(this.groupBox9);
            resources.ApplyResources(this.tabExpanseImport, "tabExpanseImport");
            this.tabExpanseImport.Name = "tabExpanseImport";
            this.tabExpanseImport.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Controls.Add(this.btnExpanseImport);
            this.groupBox9.Controls.Add(this.btnCopyExpansedConfig);
            this.groupBox9.Controls.Add(this.panelExpandConfig);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // btnExpanseImport
            // 
            resources.ApplyResources(this.btnExpanseImport, "btnExpanseImport");
            this.btnExpanseImport.Name = "btnExpanseImport";
            this.btnExpanseImport.UseVisualStyleBackColor = true;
            this.btnExpanseImport.Click += new System.EventHandler(this.btnExpanseImport_Click);
            // 
            // btnCopyExpansedConfig
            // 
            resources.ApplyResources(this.btnCopyExpansedConfig, "btnCopyExpansedConfig");
            this.btnCopyExpansedConfig.Name = "btnCopyExpansedConfig";
            this.btnCopyExpansedConfig.UseVisualStyleBackColor = true;
            this.btnCopyExpansedConfig.Click += new System.EventHandler(this.btnCopyExpansedConfig_Click);
            // 
            // panelExpandConfig
            // 
            resources.ApplyResources(this.panelExpandConfig, "panelExpandConfig");
            this.panelExpandConfig.Name = "panelExpandConfig";
            // 
            // btnClearModify
            // 
            resources.ApplyResources(this.btnClearModify, "btnClearModify");
            this.btnClearModify.Name = "btnClearModify";
            this.btnClearModify.UseVisualStyleBackColor = true;
            this.btnClearModify.Click += new System.EventHandler(this.btnDiscardChanges_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configToolStripMenuItem,
            this.viewToolStripMenuItem});
            resources.ApplyResources(this.mainMenu, "mainMenu");
            this.mainMenu.Name = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWinToolStripMenuItem1,
            this.toolStripSeparator4,
            this.loadJsonToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // newWinToolStripMenuItem1
            // 
            this.newWinToolStripMenuItem1.Name = "newWinToolStripMenuItem1";
            resources.ApplyResources(this.newWinToolStripMenuItem1, "newWinToolStripMenuItem1");
            this.newWinToolStripMenuItem1.Click += new System.EventHandler(this.newWinToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // loadJsonToolStripMenuItem
            // 
            this.loadJsonToolStripMenuItem.Name = "loadJsonToolStripMenuItem";
            resources.ApplyResources(this.loadJsonToolStripMenuItem, "loadJsonToolStripMenuItem");
            this.loadJsonToolStripMenuItem.Click += new System.EventHandler(this.loadJsonToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfigStripMenuItem,
            this.addNewServerToolStripMenuItem,
            this.toolStripSeparator2,
            this.loadServerToolStripMenuItem,
            this.replaceExistServerToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            resources.ApplyResources(this.configToolStripMenuItem, "configToolStripMenuItem");
            // 
            // saveConfigStripMenuItem
            // 
            this.saveConfigStripMenuItem.Name = "saveConfigStripMenuItem";
            resources.ApplyResources(this.saveConfigStripMenuItem, "saveConfigStripMenuItem");
            this.saveConfigStripMenuItem.Click += new System.EventHandler(this.saveConfigStripMenuItem_Click);
            // 
            // addNewServerToolStripMenuItem
            // 
            this.addNewServerToolStripMenuItem.Name = "addNewServerToolStripMenuItem";
            resources.ApplyResources(this.addNewServerToolStripMenuItem, "addNewServerToolStripMenuItem");
            this.addNewServerToolStripMenuItem.Click += new System.EventHandler(this.addNewServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // loadServerToolStripMenuItem
            // 
            this.loadServerToolStripMenuItem.Name = "loadServerToolStripMenuItem";
            resources.ApplyResources(this.loadServerToolStripMenuItem, "loadServerToolStripMenuItem");
            // 
            // replaceExistServerToolStripMenuItem
            // 
            this.replaceExistServerToolStripMenuItem.Name = "replaceExistServerToolStripMenuItem";
            resources.ApplyResources(this.replaceExistServerToolStripMenuItem, "replaceExistServerToolStripMenuItem");
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchBoxToolStripMenuItem,
            this.toolStripSeparator3,
            this.showLeftPanelToolStripMenuItem,
            this.hideLeftPanelToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // searchBoxToolStripMenuItem
            // 
            this.searchBoxToolStripMenuItem.Name = "searchBoxToolStripMenuItem";
            resources.ApplyResources(this.searchBoxToolStripMenuItem, "searchBoxToolStripMenuItem");
            this.searchBoxToolStripMenuItem.Click += new System.EventHandler(this.searchBoxToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // showLeftPanelToolStripMenuItem
            // 
            this.showLeftPanelToolStripMenuItem.Checked = true;
            this.showLeftPanelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLeftPanelToolStripMenuItem.Name = "showLeftPanelToolStripMenuItem";
            resources.ApplyResources(this.showLeftPanelToolStripMenuItem, "showLeftPanelToolStripMenuItem");
            this.showLeftPanelToolStripMenuItem.Click += new System.EventHandler(this.showLeftPanelToolStripMenuItem_Click);
            // 
            // hideLeftPanelToolStripMenuItem
            // 
            this.hideLeftPanelToolStripMenuItem.Name = "hideLeftPanelToolStripMenuItem";
            resources.ApplyResources(this.hideLeftPanelToolStripMenuItem, "hideLeftPanelToolStripMenuItem");
            this.hideLeftPanelToolStripMenuItem.Click += new System.EventHandler(this.hideLeftPanelToolStripMenuItem_Click);
            // 
            // pnlTools
            // 
            resources.ApplyResources(this.pnlTools, "pnlTools");
            this.pnlTools.Controls.Add(this.tabControl1);
            this.pnlTools.Name = "pnlTools";
            // 
            // pnlEditor
            // 
            resources.ApplyResources(this.pnlEditor, "pnlEditor");
            this.pnlEditor.Controls.Add(this.label7);
            this.pnlEditor.Controls.Add(this.label5);
            this.pnlEditor.Controls.Add(this.panel1);
            this.pnlEditor.Controls.Add(this.cboxConfigSection);
            this.pnlEditor.Controls.Add(this.btnFormat);
            this.pnlEditor.Controls.Add(this.btnClearModify);
            this.pnlEditor.Controls.Add(this.cboxExamples);
            this.pnlEditor.Name = "pnlEditor";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panelScintilla);
            this.panel1.Name = "panel1";
            // 
            // panelScintilla
            // 
            resources.ApplyResources(this.panelScintilla, "panelScintilla");
            this.panelScintilla.Name = "panelScintilla";
            // 
            // btnFormat
            // 
            resources.ApplyResources(this.btnFormat, "btnFormat");
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // cboxExamples
            // 
            this.cboxExamples.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxExamples.FormattingEnabled = true;
            resources.ApplyResources(this.cboxExamples, "cboxExamples");
            this.cboxExamples.Name = "cboxExamples";
            this.cboxExamples.SelectedIndexChanged += new System.EventHandler(this.cboxExamples_SelectedIndexChanged);
            // 
            // FormConfiger
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlTools);
            this.Controls.Add(this.mainMenu);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "FormConfiger";
            this.Shown += new System.EventHandler(this.FormConfiger_Shown);
            this.tabControl1.ResumeLayout(false);
            this.vmess.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ss.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.misc.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabExpanseImport.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.pnlTools.ResumeLayout(false);
            this.pnlEditor.ResumeLayout(false);
            this.pnlEditor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cboxConfigSection;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage vmess;
        private System.Windows.Forms.TabPage ss;
        private System.Windows.Forms.Button btnClearModify;
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage misc;
        private System.Windows.Forms.CheckBox chkSSCOTA;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkSSSOTA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboxSSSNetwork;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cboxSSSMethod;
        private System.Windows.Forms.TextBox tboxSSSPort;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnSSInsertServer;
        private System.Windows.Forms.CheckBox chkSSSShowPass;
        private System.Windows.Forms.TextBox tboxSSSPass;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadJsonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLeftPanelToolStripMenuItem;
        private System.Windows.Forms.Panel pnlTools;
        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.Panel panelScintilla;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem hideLeftPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceExistServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem newWinToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ComboBox cboxExamples;
        private System.Windows.Forms.ToolStripMenuItem searchBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton rbtnVmessIServerMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnStreamOutbound;
        private System.Windows.Forms.RadioButton rbtnStreamInbound;
        private System.Windows.Forms.TextBox tboxWSPath;
        private System.Windows.Forms.ComboBox cboxKCPType;
        private System.Windows.Forms.ComboBox cboxTCPType;
        private System.Windows.Forms.ComboBox cboxStreamSecurity;
        private System.Windows.Forms.Button btnStreamInsertTCP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnStreamInsertWS;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStreamInsertKCP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tboxVGCDesc;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnVGC;
        private System.Windows.Forms.TextBox tboxVGCAlias;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnQConSkipCN;
        private System.Windows.Forms.ToolStripMenuItem saveConfigStripMenuItem;
        private System.Windows.Forms.TabPage tabExpanseImport;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Panel panelExpandConfig;
        private System.Windows.Forms.Button btnExpanseImport;
        private System.Windows.Forms.Button btnCopyExpansedConfig;
    }
}