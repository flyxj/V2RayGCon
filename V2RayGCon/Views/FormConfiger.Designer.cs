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
            this.btnStreamInsertH2 = new System.Windows.Forms.Button();
            this.tboxH2Path = new System.Windows.Forms.TextBox();
            this.rbtnStreamOutbound = new System.Windows.Forms.RadioButton();
            this.rbtnStreamInbound = new System.Windows.Forms.RadioButton();
            this.tboxWSPath = new System.Windows.Forms.TextBox();
            this.cboxKCPType = new System.Windows.Forms.ComboBox();
            this.cboxTCPType = new System.Windows.Forms.ComboBox();
            this.cboxStreamSecurity = new System.Windows.Forms.ComboBox();
            this.btnStreamInsertTCP = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
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
            this.button1 = new System.Windows.Forms.Button();
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
            resources.ApplyResources(this.cboxConfigSection, "cboxConfigSection");
            this.cboxConfigSection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConfigSection.FormattingEnabled = true;
            this.cboxConfigSection.Items.AddRange(new object[] {
            resources.GetString("cboxConfigSection.Items"),
            resources.GetString("cboxConfigSection.Items1")});
            this.cboxConfigSection.Name = "cboxConfigSection";
            this.toolTip1.SetToolTip(this.cboxConfigSection, resources.GetString("cboxConfigSection.ToolTip"));
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
            this.toolTip1.SetToolTip(this.tabControl1, resources.GetString("tabControl1.ToolTip"));
            // 
            // vmess
            // 
            resources.ApplyResources(this.vmess, "vmess");
            this.vmess.Controls.Add(this.groupBox2);
            this.vmess.Controls.Add(this.groupBox1);
            this.vmess.Name = "vmess";
            this.toolTip1.SetToolTip(this.vmess, resources.GetString("vmess.ToolTip"));
            this.vmess.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnStreamInsertH2);
            this.groupBox2.Controls.Add(this.tboxH2Path);
            this.groupBox2.Controls.Add(this.rbtnStreamOutbound);
            this.groupBox2.Controls.Add(this.rbtnStreamInbound);
            this.groupBox2.Controls.Add(this.tboxWSPath);
            this.groupBox2.Controls.Add(this.cboxKCPType);
            this.groupBox2.Controls.Add(this.cboxTCPType);
            this.groupBox2.Controls.Add(this.cboxStreamSecurity);
            this.groupBox2.Controls.Add(this.btnStreamInsertTCP);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btnStreamInsertWS);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnStreamInsertKCP);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // btnStreamInsertH2
            // 
            resources.ApplyResources(this.btnStreamInsertH2, "btnStreamInsertH2");
            this.btnStreamInsertH2.Name = "btnStreamInsertH2";
            this.toolTip1.SetToolTip(this.btnStreamInsertH2, resources.GetString("btnStreamInsertH2.ToolTip"));
            this.btnStreamInsertH2.UseVisualStyleBackColor = true;
            this.btnStreamInsertH2.Click += new System.EventHandler(this.btnStreamInsertH2_Click);
            // 
            // tboxH2Path
            // 
            resources.ApplyResources(this.tboxH2Path, "tboxH2Path");
            this.tboxH2Path.Name = "tboxH2Path";
            this.toolTip1.SetToolTip(this.tboxH2Path, resources.GetString("tboxH2Path.ToolTip"));
            // 
            // rbtnStreamOutbound
            // 
            resources.ApplyResources(this.rbtnStreamOutbound, "rbtnStreamOutbound");
            this.rbtnStreamOutbound.Checked = true;
            this.rbtnStreamOutbound.Name = "rbtnStreamOutbound";
            this.rbtnStreamOutbound.TabStop = true;
            this.toolTip1.SetToolTip(this.rbtnStreamOutbound, resources.GetString("rbtnStreamOutbound.ToolTip"));
            this.rbtnStreamOutbound.UseVisualStyleBackColor = true;
            // 
            // rbtnStreamInbound
            // 
            resources.ApplyResources(this.rbtnStreamInbound, "rbtnStreamInbound");
            this.rbtnStreamInbound.Name = "rbtnStreamInbound";
            this.toolTip1.SetToolTip(this.rbtnStreamInbound, resources.GetString("rbtnStreamInbound.ToolTip"));
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
            resources.ApplyResources(this.cboxKCPType, "cboxKCPType");
            this.cboxKCPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxKCPType.FormattingEnabled = true;
            this.cboxKCPType.Items.AddRange(new object[] {
            resources.GetString("cboxKCPType.Items")});
            this.cboxKCPType.Name = "cboxKCPType";
            this.toolTip1.SetToolTip(this.cboxKCPType, resources.GetString("cboxKCPType.ToolTip"));
            // 
            // cboxTCPType
            // 
            resources.ApplyResources(this.cboxTCPType, "cboxTCPType");
            this.cboxTCPType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxTCPType.FormattingEnabled = true;
            this.cboxTCPType.Items.AddRange(new object[] {
            resources.GetString("cboxTCPType.Items")});
            this.cboxTCPType.Name = "cboxTCPType";
            this.toolTip1.SetToolTip(this.cboxTCPType, resources.GetString("cboxTCPType.ToolTip"));
            // 
            // cboxStreamSecurity
            // 
            resources.ApplyResources(this.cboxStreamSecurity, "cboxStreamSecurity");
            this.cboxStreamSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            this.toolTip1.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
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
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            this.toolTip1.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
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
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton1, resources.GetString("radioButton1.ToolTip"));
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbtnVmessIServerMode
            // 
            resources.ApplyResources(this.rbtnVmessIServerMode, "rbtnVmessIServerMode");
            this.rbtnVmessIServerMode.Name = "rbtnVmessIServerMode";
            this.toolTip1.SetToolTip(this.rbtnVmessIServerMode, resources.GetString("rbtnVmessIServerMode.ToolTip"));
            this.rbtnVmessIServerMode.UseVisualStyleBackColor = true;
            this.rbtnVmessIServerMode.CheckedChanged += new System.EventHandler(this.rbtnVmessIServerMode_CheckedChanged);
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
            // ss
            // 
            resources.ApplyResources(this.ss, "ss");
            this.ss.Controls.Add(this.groupBox4);
            this.ss.Controls.Add(this.groupBox3);
            this.ss.Name = "ss";
            this.toolTip1.SetToolTip(this.ss, resources.GetString("ss.ToolTip"));
            this.ss.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
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
            this.cboxSSSMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cboxSSSNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.groupBox3.Controls.Add(this.label6);
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
            this.cboxSSCMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // misc
            // 
            resources.ApplyResources(this.misc, "misc");
            this.misc.Controls.Add(this.groupBox5);
            this.misc.Controls.Add(this.groupBox6);
            this.misc.Name = "misc";
            this.toolTip1.SetToolTip(this.misc, resources.GetString("misc.ToolTip"));
            this.misc.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.button1);
            this.groupBox5.Controls.Add(this.btnQConSkipCN);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox5, resources.GetString("groupBox5.ToolTip"));
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.tboxVGCDesc);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.btnVGC);
            this.groupBox6.Controls.Add(this.tboxVGCAlias);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox6, resources.GetString("groupBox6.ToolTip"));
            // 
            // tboxVGCDesc
            // 
            resources.ApplyResources(this.tboxVGCDesc, "tboxVGCDesc");
            this.tboxVGCDesc.Name = "tboxVGCDesc";
            this.toolTip1.SetToolTip(this.tboxVGCDesc, resources.GetString("tboxVGCDesc.ToolTip"));
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            this.toolTip1.SetToolTip(this.label16, resources.GetString("label16.ToolTip"));
            // 
            // btnVGC
            // 
            resources.ApplyResources(this.btnVGC, "btnVGC");
            this.btnVGC.Name = "btnVGC";
            this.toolTip1.SetToolTip(this.btnVGC, resources.GetString("btnVGC.ToolTip"));
            this.btnVGC.UseVisualStyleBackColor = true;
            this.btnVGC.Click += new System.EventHandler(this.btnVGC_Click);
            // 
            // tboxVGCAlias
            // 
            resources.ApplyResources(this.tboxVGCAlias, "tboxVGCAlias");
            this.tboxVGCAlias.Name = "tboxVGCAlias";
            this.toolTip1.SetToolTip(this.tboxVGCAlias, resources.GetString("tboxVGCAlias.ToolTip"));
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            this.toolTip1.SetToolTip(this.label15, resources.GetString("label15.ToolTip"));
            // 
            // tabExpanseImport
            // 
            resources.ApplyResources(this.tabExpanseImport, "tabExpanseImport");
            this.tabExpanseImport.Controls.Add(this.groupBox9);
            this.tabExpanseImport.Name = "tabExpanseImport";
            this.toolTip1.SetToolTip(this.tabExpanseImport, resources.GetString("tabExpanseImport.ToolTip"));
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
            this.toolTip1.SetToolTip(this.groupBox9, resources.GetString("groupBox9.ToolTip"));
            // 
            // btnExpanseImport
            // 
            resources.ApplyResources(this.btnExpanseImport, "btnExpanseImport");
            this.btnExpanseImport.Name = "btnExpanseImport";
            this.toolTip1.SetToolTip(this.btnExpanseImport, resources.GetString("btnExpanseImport.ToolTip"));
            this.btnExpanseImport.UseVisualStyleBackColor = true;
            this.btnExpanseImport.Click += new System.EventHandler(this.btnExpanseImport_Click);
            // 
            // btnCopyExpansedConfig
            // 
            resources.ApplyResources(this.btnCopyExpansedConfig, "btnCopyExpansedConfig");
            this.btnCopyExpansedConfig.Name = "btnCopyExpansedConfig";
            this.toolTip1.SetToolTip(this.btnCopyExpansedConfig, resources.GetString("btnCopyExpansedConfig.ToolTip"));
            this.btnCopyExpansedConfig.UseVisualStyleBackColor = true;
            this.btnCopyExpansedConfig.Click += new System.EventHandler(this.btnCopyExpansedConfig_Click);
            // 
            // panelExpandConfig
            // 
            resources.ApplyResources(this.panelExpandConfig, "panelExpandConfig");
            this.panelExpandConfig.Name = "panelExpandConfig";
            this.toolTip1.SetToolTip(this.panelExpandConfig, resources.GetString("panelExpandConfig.ToolTip"));
            // 
            // btnClearModify
            // 
            resources.ApplyResources(this.btnClearModify, "btnClearModify");
            this.btnClearModify.Name = "btnClearModify";
            this.toolTip1.SetToolTip(this.btnClearModify, resources.GetString("btnClearModify.ToolTip"));
            this.btnClearModify.UseVisualStyleBackColor = true;
            this.btnClearModify.Click += new System.EventHandler(this.btnDiscardChanges_Click);
            // 
            // mainMenu
            // 
            resources.ApplyResources(this.mainMenu, "mainMenu");
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.configToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.mainMenu.Name = "mainMenu";
            this.toolTip1.SetToolTip(this.mainMenu, resources.GetString("mainMenu.ToolTip"));
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWinToolStripMenuItem1,
            this.toolStripSeparator4,
            this.loadJsonToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            // 
            // newWinToolStripMenuItem1
            // 
            resources.ApplyResources(this.newWinToolStripMenuItem1, "newWinToolStripMenuItem1");
            this.newWinToolStripMenuItem1.Name = "newWinToolStripMenuItem1";
            this.newWinToolStripMenuItem1.Click += new System.EventHandler(this.newWinToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // loadJsonToolStripMenuItem
            // 
            resources.ApplyResources(this.loadJsonToolStripMenuItem, "loadJsonToolStripMenuItem");
            this.loadJsonToolStripMenuItem.Name = "loadJsonToolStripMenuItem";
            this.loadJsonToolStripMenuItem.Click += new System.EventHandler(this.loadJsonToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            resources.ApplyResources(this.configToolStripMenuItem, "configToolStripMenuItem");
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConfigStripMenuItem,
            this.addNewServerToolStripMenuItem,
            this.toolStripSeparator2,
            this.loadServerToolStripMenuItem,
            this.replaceExistServerToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            // 
            // saveConfigStripMenuItem
            // 
            resources.ApplyResources(this.saveConfigStripMenuItem, "saveConfigStripMenuItem");
            this.saveConfigStripMenuItem.Name = "saveConfigStripMenuItem";
            this.saveConfigStripMenuItem.Click += new System.EventHandler(this.saveConfigStripMenuItem_Click);
            // 
            // addNewServerToolStripMenuItem
            // 
            resources.ApplyResources(this.addNewServerToolStripMenuItem, "addNewServerToolStripMenuItem");
            this.addNewServerToolStripMenuItem.Name = "addNewServerToolStripMenuItem";
            this.addNewServerToolStripMenuItem.Click += new System.EventHandler(this.addNewServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // loadServerToolStripMenuItem
            // 
            resources.ApplyResources(this.loadServerToolStripMenuItem, "loadServerToolStripMenuItem");
            this.loadServerToolStripMenuItem.Name = "loadServerToolStripMenuItem";
            // 
            // replaceExistServerToolStripMenuItem
            // 
            resources.ApplyResources(this.replaceExistServerToolStripMenuItem, "replaceExistServerToolStripMenuItem");
            this.replaceExistServerToolStripMenuItem.Name = "replaceExistServerToolStripMenuItem";
            // 
            // viewToolStripMenuItem
            // 
            resources.ApplyResources(this.viewToolStripMenuItem, "viewToolStripMenuItem");
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchBoxToolStripMenuItem,
            this.toolStripSeparator3,
            this.showLeftPanelToolStripMenuItem,
            this.hideLeftPanelToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            // 
            // searchBoxToolStripMenuItem
            // 
            resources.ApplyResources(this.searchBoxToolStripMenuItem, "searchBoxToolStripMenuItem");
            this.searchBoxToolStripMenuItem.Name = "searchBoxToolStripMenuItem";
            this.searchBoxToolStripMenuItem.Click += new System.EventHandler(this.searchBoxToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // showLeftPanelToolStripMenuItem
            // 
            resources.ApplyResources(this.showLeftPanelToolStripMenuItem, "showLeftPanelToolStripMenuItem");
            this.showLeftPanelToolStripMenuItem.Checked = true;
            this.showLeftPanelToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLeftPanelToolStripMenuItem.Name = "showLeftPanelToolStripMenuItem";
            this.showLeftPanelToolStripMenuItem.Click += new System.EventHandler(this.showLeftPanelToolStripMenuItem_Click);
            // 
            // hideLeftPanelToolStripMenuItem
            // 
            resources.ApplyResources(this.hideLeftPanelToolStripMenuItem, "hideLeftPanelToolStripMenuItem");
            this.hideLeftPanelToolStripMenuItem.Name = "hideLeftPanelToolStripMenuItem";
            this.hideLeftPanelToolStripMenuItem.Click += new System.EventHandler(this.hideLeftPanelToolStripMenuItem_Click);
            // 
            // pnlTools
            // 
            resources.ApplyResources(this.pnlTools, "pnlTools");
            this.pnlTools.Controls.Add(this.tabControl1);
            this.pnlTools.Name = "pnlTools";
            this.toolTip1.SetToolTip(this.pnlTools, resources.GetString("pnlTools.ToolTip"));
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
            this.toolTip1.SetToolTip(this.pnlEditor, resources.GetString("pnlEditor.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            this.toolTip1.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.panelScintilla);
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // panelScintilla
            // 
            resources.ApplyResources(this.panelScintilla, "panelScintilla");
            this.panelScintilla.Name = "panelScintilla";
            this.toolTip1.SetToolTip(this.panelScintilla, resources.GetString("panelScintilla.ToolTip"));
            // 
            // btnFormat
            // 
            resources.ApplyResources(this.btnFormat, "btnFormat");
            this.btnFormat.Name = "btnFormat";
            this.toolTip1.SetToolTip(this.btnFormat, resources.GetString("btnFormat.ToolTip"));
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // cboxExamples
            // 
            resources.ApplyResources(this.cboxExamples, "cboxExamples");
            this.cboxExamples.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxExamples.FormattingEnabled = true;
            this.cboxExamples.Name = "cboxExamples";
            this.toolTip1.SetToolTip(this.cboxExamples, resources.GetString("cboxExamples.ToolTip"));
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
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnStreamInsertH2;
        private System.Windows.Forms.TextBox tboxH2Path;
        private System.Windows.Forms.Label label20;
    }
}