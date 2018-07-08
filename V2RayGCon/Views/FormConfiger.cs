using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Views
{
    public partial class FormConfiger : Form
    {
        Controller.Configer.Configer configer;
        Service.Setting setting;
        Scintilla scintillaMain, scintillaImport;
        FormSearch formSearch;

        string _Title;
        int _serverIndex;

        public FormConfiger(int serverIndex = -1)
        {
            setting = Service.Setting.Instance;
            _serverIndex = serverIndex;
            formSearch = null;
            InitializeComponent();
            _Title = this.Text;
            this.Show();
        }

        private void FormConfiger_Shown(object sender, EventArgs e)
        {
            scintillaMain = new Scintilla();
            InitScintilla(scintillaMain,panelScintilla);
            scintillaImport = new Scintilla();
            InitScintilla(scintillaImport,panelExpandConfig,true);
            
            configer = new Controller.Configer.Configer(
                scintillaImport,
                _serverIndex);

            InitComboBox();
            InitDataBinding();
            UpdateServerMenu();
            SetTitle(configer.GetAlias());
            ToggleToolsPanel(setting.isShowConfigureToolsPanel);

            cboxConfigSection.SelectedIndex = 0;

            this.FormClosing += (s, a) =>
            {
                a.Cancel = !Lib.UI.Confirm(I18N("ConfirmCloseWindow"));
            };

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            setting.OnSettingChange += SettingChange;
        }

        #region data binding


        void BindDataEditor()
        {
            // bind scintilla
            var editor = configer.editor;
            var bs = new BindingSource();

            bs.DataSource = editor;
            scintillaMain.DataBindings.Add(
                "Text",
                bs,
                nameof(editor.content),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataStreamSettings()
        {
            var streamClient = configer.streamSettings;
            var bs = new BindingSource();
            bs.DataSource = streamClient;

            tboxWSPath.DataBindings.Add("Text", bs, nameof(streamClient.wsPath));
            tboxH2Path.DataBindings.Add("Text", bs, nameof(streamClient.h2Path));

            cboxKCPType.DataBindings.Add(
                nameof(cboxKCPType.SelectedIndex),
                bs,
                nameof(streamClient.kcpType),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            cboxTCPType.DataBindings.Add(
                nameof(cboxTCPType.SelectedIndex),
                bs,
                nameof(streamClient.tcpType),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            cboxStreamSecurity.DataBindings.Add(
                nameof(cboxStreamSecurity.SelectedIndex),
                bs,
                nameof(streamClient.tls),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataImport()
        {
            var import = configer.import;
            var bs = new BindingSource();
            bs.DataSource = import;

            scintillaImport.DataBindings.Add(
                "Text",
                bs,
                nameof(import.content),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataVGC()
        {
            var vgc = configer.vgc;
            var bs = new BindingSource();
            bs.DataSource = vgc;
            tboxVGCAlias.DataBindings.Add("Text", bs, nameof(vgc.alias));
            tboxVGCDesc.DataBindings.Add("Text", bs, nameof(vgc.description));
        }

        void BindDataSSServer()
        {
            var server = configer.ssServer;
            var bs = new BindingSource();
            bs.DataSource = server;

            tboxSSSPass.DataBindings.Add("Text", bs, nameof(server.pass));
            tboxSSSPort.DataBindings.Add("Text", bs, nameof(server.port));

            cboxSSSNetwork.DataBindings.Add(
                nameof(cboxSSSNetwork.SelectedIndex),
                bs,
                nameof(server.network),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            cboxSSSMethod.DataBindings.Add(
                nameof(cboxSSSMethod.SelectedIndex),
                bs,
                nameof(server.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            chkSSSOTA.DataBindings.Add(
                nameof(chkSSSOTA.Checked),
                bs,
                nameof(server.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataSSClient()
        {
            var ssClient = configer.ssClient;

            var bs = new BindingSource();
            bs.DataSource = ssClient;

            tboxSSCAddr.DataBindings.Add("Text", bs, nameof(ssClient.addr));
            tboxSSCPass.DataBindings.Add("Text", bs, nameof(ssClient.pass));

            cboxSSCMethod.DataBindings.Add(
                nameof(cboxSSCMethod.SelectedIndex),
                bs,
                nameof(ssClient.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            chkSSCOTA.DataBindings.Add(
                nameof(chkSSCOTA.Checked),
                bs,
                nameof(ssClient.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

        }

        void BindDataVmess()
        {
            var vmessClient = configer.vmessCtrl;
            var bsVmessClient = new BindingSource();
            bsVmessClient.DataSource = vmessClient;

            tboxVMessID.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.ID));
            tboxVMessLevel.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.level));
            tboxVMessAid.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.altID));
            tboxVMessIPaddr.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.addr));
        }
        #endregion

        #region UI event handler
        private void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            cboxExamples.SelectedIndex = 0;
            configer.DiscardChanges();
        }

        private void btnVMessInsertClient_Click(object sender, EventArgs e)
        {
            configer.InsertVmess();
        }

        private void btnSSRInsertClient_Click(object sender, EventArgs e)
        {
            configer.InsertSSClient();
        }

        private void btnVMessGenUUID_Click(object sender, EventArgs e)
        {
            configer.vmessCtrl.ID = Guid.NewGuid().ToString();
        }

        private void cboxShowPassWord_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSSCShowPass.Checked == true)
            {
                tboxSSCPass.PasswordChar = '\0';
            }
            else
            {
                tboxSSCPass.PasswordChar = '*';
            }
        }

        private void chkSSSShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSSSShowPass.Checked == true)
            {
                tboxSSSPass.PasswordChar = '\0';
            }
            else
            {
                tboxSSSPass.PasswordChar = '*';
            }
        }

        private void btnSSInsertServer_Click(object sender, EventArgs e)
        {
            configer.InsertSSServer();
        }

        private void cboxConfigSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!configer.OnSectionChanged(cboxConfigSection.SelectedIndex))
            {
                cboxConfigSection.SelectedIndex = configer.preSection;
            }
            else
            {
                // update examples
                UpdateExamplesDescription();
            }
        }

        private void showLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolsPanel(true);
        }

        private void hideLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolsPanel(false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configer.InsertConfigHelper(null);

            switch (Lib.UI.ShowSaveFileDialog(
                resData("ExtJson"),
                configer.GetConfigFormated(),
                out string filename))
            {
                case Model.Data.Enum.SaveFileErrorCode.Success:
                    SetTitle(filename);
                    configer.ClearOriginalConfig();
                    MessageBox.Show(I18N("Done"));
                    break;
                case Model.Data.Enum.SaveFileErrorCode.Fail:
                    MessageBox.Show(I18N("WriteFileFail"));
                    break;
                case Model.Data.Enum.SaveFileErrorCode.Cancel:
                    // do nothing
                    break;
            }
        }

        private void addNewServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("AddNewServer")))
            {
                configer.AddNewServer();
                SetTitle(configer.GetAlias());
            }
        }

        private void loadJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Lib.UI.Confirm(I18N("ConfirmLoadNewServer")))
            {
                return;
            }

            string json = Lib.UI.ShowReadFileDialog(resData("ExtJson"), out string filename);
            if (configer.SetConfig(json))
            {
                cboxConfigSection.SelectedIndex = 0;
                SetTitle(filename);
                configer.ClearOriginalConfig();
                I18N("Done");
            }
            else
            {
                I18N("LoadJsonFail");
            }
           
        }

        private void newWinToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new FormConfiger();
        }

        private void cboxExamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            configer.LoadExample(cboxExamples.SelectedIndex - 1);
        }

        private void searchBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchBox();
        }

        private void rbtnVmessIServerMode_CheckedChanged(object sender, EventArgs e)
        {
            configer.SetVmessServerMode(rbtnVmessIServerMode.Checked);
        }

        private void rbtnStreamInbound_CheckedChanged(object sender, EventArgs e)
        {
            configer.SetStreamSettingsServerMode(rbtnStreamInbound.Checked);
        }

        private void btnStreamInsertKCP_Click(object sender, EventArgs e)
        {
            configer.InsertKCP();
        }

        private void btnStreamInsertWS_Click(object sender, EventArgs e)
        {
            configer.InsertWS();
        }

        private void btnStreamInsertTCP_Click(object sender, EventArgs e)
        {
            configer.InsertTCP();
        }

        private void btnVGC_Click(object sender, EventArgs e)
        {
            configer.InsertVGC();
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            configer.FormatCurrentContent();
        }

        private void btnQConSkipCN_Click(object sender, EventArgs e)
        {
            configer.InsertSkipCN();
        }

        private void saveConfigStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("ConfirmSaveCurConfig"))) {
                if (configer.ReplaceOriginalServer())
                {
                    SetTitle(configer.GetAlias());
                }
            }
        }
        #endregion

        #region bind hotkey
        protected override bool ProcessCmdKey(ref Message msg, Keys keyCode)
        {
            switch (keyCode)
            {
                case (Keys.Control | Keys.P):
                    var visible = !setting.isShowConfigureToolsPanel;
                    setting.isShowConfigureToolsPanel = visible;
                    ToggleToolsPanel(visible);
                    break;
                case (Keys.Control | Keys.F):
                    ShowSearchBox();
                    break;
                case (Keys.Control | Keys.S):
                    configer.InsertConfigHelper(null);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyCode);
        }
        #endregion

        #region init
        void InitComboBox()
        {
            void FillComboBox(ComboBox cbox, Dictionary<int, string> table)
            {
                cbox.Items.Clear();
                foreach (var item in table)
                {
                    cbox.Items.Add(item.Value);
                }
                cbox.SelectedIndex = 0;
            }

            FillComboBox(cboxConfigSection, Model.Data.Table.configSections);
            FillComboBox(cboxSSCMethod, Model.Data.Table.ssMethods);
            FillComboBox(cboxSSSMethod, Model.Data.Table.ssMethods);
            FillComboBox(cboxSSSNetwork, Model.Data.Table.ssNetworks);
            FillComboBox(cboxTCPType, Model.Data.Table.tcpTypes);
            FillComboBox(cboxKCPType, Model.Data.Table.kcpTypes);
            FillComboBox(cboxStreamSecurity, Model.Data.Table.streamSecurity);
        }

        void InitScintilla(Scintilla scintilla, Panel container, bool readOnlyMode = false)
        {
            container.Controls.Add(scintilla);

            // scintilla.Dock = DockStyle.Fill;
            scintilla.Dock = DockStyle.Fill;
            scintilla.WrapMode = WrapMode.None;
            scintilla.IndentationGuides = IndentView.LookBoth;

            // Configure the JSON lexer styles
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
            if (readOnlyMode) {
                var bgColor = this.BackColor;
                scintilla.Styles[Style.Default].BackColor = bgColor;
                scintilla.Styles[Style.Json.BlockComment].BackColor = bgColor;
                scintilla.Styles[Style.Json.Default].BackColor = bgColor;
                scintilla.Styles[Style.Json.Error].BackColor = bgColor;
                scintilla.Styles[Style.Json.EscapeSequence].BackColor = bgColor;
                scintilla.Styles[Style.Json.Keyword].BackColor = bgColor;
                scintilla.Styles[Style.Json.LineComment].BackColor = bgColor;
                scintilla.Styles[Style.Json.Number].BackColor = bgColor;
                scintilla.Styles[Style.Json.Operator].BackColor = bgColor;
                scintilla.Styles[Style.Json.PropertyName].BackColor = bgColor;
                scintilla.Styles[Style.Json.String].BackColor = bgColor;
                scintilla.Styles[Style.Json.CompactIRI].BackColor = bgColor;
                scintilla.ReadOnly = true;
            }
            scintilla.Styles[Style.Json.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
            scintilla.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Json.Operator].ForeColor = Color.Purple;
            scintilla.Lexer = Lexer.Json;

            // folding
            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            // key binding

            // clear default keyboard shortcut
            scintilla.ClearCmdKey(Keys.Control | Keys.P);
            scintilla.ClearCmdKey(Keys.Control | Keys.S);
            scintilla.ClearCmdKey(Keys.Control | Keys.F);
        }

        void InitDataBinding()
        {
            BindDataVmess();
            BindDataSSClient();
            BindDataStreamSettings();
            BindDataEditor();
            BindDataSSServer();
            BindDataVGC();
            BindDataImport();
        }
        #endregion

        #region private method
        void UpdateExamplesDescription()
        {
            cboxExamples.Items.Clear();

            cboxExamples.Items.Add(I18N("AvailableExamples"));
            var descriptions = configer.GetExamplesDescription();
            if (descriptions.Count < 1)
            {
                cboxExamples.Enabled = false;
            }
            else
            {
                cboxExamples.Enabled = true;
                foreach (var description in descriptions)
                {
                    cboxExamples.Items.Add(description);
                }
            }
            cboxExamples.SelectedIndex = 0;

        }

        void SetTitle(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.Text = _Title;
            }
            else
            {
                this.Text = string.Format("{0} - {1}", _Title, name);
            }
        }

        void UpdateServerMenu()
        {
            var menuRepalceServer = replaceExistServerToolStripMenuItem.DropDownItems;
            var menuLoadServer = loadServerToolStripMenuItem.DropDownItems;

            menuRepalceServer.Clear();
            menuLoadServer.Clear();

            var aliases = setting.GetAllAliases();

            var enable = aliases.Count > 0;
            replaceExistServerToolStripMenuItem.Enabled = enable;
            loadServerToolStripMenuItem.Enabled = enable;

            for (int i = 0; i < aliases.Count; i++)
            {
                var index = i;
                menuRepalceServer.Add(new ToolStripMenuItem(aliases[i], null, (s, a) =>
                {
                    if (Lib.UI.Confirm(I18N("ReplaceServer")))
                    {
                        if (configer.ReplaceServer(index))
                        {
                            SetTitle(configer.GetAlias());
                        }
                    }
                }));

                menuLoadServer.Add(new ToolStripMenuItem(aliases[i], null, (s, a) =>
                {
                    if (Lib.UI.Confirm(I18N("ConfirmLoadNewServer")))
                    {
                        configer.LoadServer(index);
                        SetTitle(configer.GetAlias());
                        cboxConfigSection.SelectedIndex = 0;
                    }
                }));
            }
        }

        void ToggleToolsPanel(bool visible)
        {
            var margin = 4;
            var formSize = this.ClientSize;
            var editorSize = pnlEditor.Size;

            pnlTools.Visible = visible;
            pnlEditor.Visible = false;
            if (visible)
            {
                pnlEditor.Left = pnlTools.Left + pnlTools.Width + margin;
                editorSize.Width = formSize.Width - pnlTools.Width - margin * 3;
            }
            else
            {
                pnlEditor.Left = margin;
                editorSize.Width = formSize.Width - margin * 2;
            }
            pnlEditor.Size = editorSize;
            pnlEditor.Visible = true;

            showLeftPanelToolStripMenuItem.Checked = visible;
            hideLeftPanelToolStripMenuItem.Checked = !visible;
            setting.isShowConfigureToolsPanel = visible;
        }

        void SettingChange(object sender, EventArgs args)
        {
            try
            {
                mainMenu.Invoke((MethodInvoker)delegate
                {
                    UpdateServerMenu();
                });
            }
            catch { }
        }

        private void btnCopyExpansedConfig_Click(object sender, EventArgs e)
        {
            Lib.Utils.CopyToClipboardAndPrompt(
                scintillaImport.Text);
        }

        private void btnExpanseImport_Click(object sender, EventArgs e)
        {
            configer.InsertConfigHelper(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            configer.InsertDtrMTProto();
        }

        private void btnStreamInsertH2_Click(object sender, EventArgs e)
        {
            configer.InsertH2();
        }

        void ShowSearchBox()
        {
            if (formSearch != null)
            {
                return;
            }
            formSearch = new FormSearch(scintillaMain);
            formSearch.FormClosed += (s, a) => formSearch = null;
        }


        #endregion

       
    }
}
