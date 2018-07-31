using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Views
{
    #region auto hide tools panel
    struct ToolsPanelHandler
    {
        public Rectangle toolsPanel;
        public Rectangle editor;
        public int span;
        public int tabWidth;
        public Rectangle pageRect;


        Timer hideToolsPanelTimer;
        EventHandler onTick;

        public void InitTimer()
        {
            if (hideToolsPanelTimer == null)
            {
                hideToolsPanelTimer = new Timer();
            }
            ClearTimer();
        }

        public void Dispose()
        {
            if (hideToolsPanelTimer == null)
            {
                return;
            }
            ClearTimer();
            hideToolsPanelTimer.Dispose();
            hideToolsPanelTimer = null;
        }

        public void SetTimer(Action lambda, int milliSecond = 500)
        {
            if (hideToolsPanelTimer.Enabled)
            {
                return;
            }

            var that = this;
            onTick = (s, e) =>
            {
                that.ClearTimer();
                lambda();
            };

            hideToolsPanelTimer.Interval = milliSecond;
            hideToolsPanelTimer.Tick += onTick;
            hideToolsPanelTimer.Start();
        }

        public void ClearTimer()
        {
            if (!hideToolsPanelTimer.Enabled)
            {
                return;
            }

            hideToolsPanelTimer.Tick -= onTick;
            hideToolsPanelTimer.Stop();
        }
    };
    #endregion

    public partial class FormConfiger : Form
    {
        Controller.Configer configer;
        Service.Setting setting;
        Scintilla scintillaMain, scintillaImport;
        FormSearch formSearch;
        ToolsPanelHandler toolsPanelHandler;

        int _serverIndex;
        string formTitle;

        public FormConfiger(int serverIndex = -1)
        {
            setting = Service.Setting.Instance;
            _serverIndex = serverIndex;
            formSearch = null;
            InitializeComponent();
            formTitle = this.Text;
            this.Show();
        }

        private void FormConfiger_Shown(object sender, EventArgs e)
        {
            InitToolsPanel();

            scintillaMain = new Scintilla();
            InitScintilla(scintillaMain, panelScintilla);
            scintillaMain.MouseClick += OnMouseLeaveToolsPanel;

            scintillaImport = new Scintilla();
            InitScintilla(scintillaImport, panelExpandConfig, true);

            InitConfiger();
            InitComboBox();

            UpdateServerMenu();
            SetTitle(configer.GetAlias());
            ToggleToolsPanel(setting.isShowConfigerToolsPanel);

            cboxConfigSection.SelectedIndex = 0;

            this.FormClosing += (s, a) =>
            {
                a.Cancel = !Lib.UI.Confirm(I18N("ConfirmCloseWindow"));
            };

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
                toolsPanelHandler.Dispose();
            };

            setting.OnSettingChange += SettingChange;
        }

        #region UI event handler
        private void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            cboxExamples.SelectedIndex = 0;
            configer.DiscardChanges();
        }

        private void btnInsertVmess_Click(object sender, EventArgs e)
        {
            configer.Inject("vmess");
        }

        private void btnSSRInsertClient_Click(object sender, EventArgs e)
        {
            configer.Inject("ssClient");
        }

        private void btnVMessGenUUID_Click(object sender, EventArgs e)
        {
            var vmess = configer.GetComponent("vmess") as Controller.ConfigerComponet.Vmess;
            vmess.ID = Guid.NewGuid().ToString();
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
            configer.Inject("ssServer");
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
                StrConst("ExtJson"),
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

            string json = Lib.UI.ShowReadFileDialog(StrConst("ExtJson"), out string filename);
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
            // todo
            var vmess = configer.GetComponent("vmess") as Controller.ConfigerComponet.Vmess;
            vmess.serverMode = rbtnVmessIServerMode.Checked;
            configer.Update();
        }

        private void rbtnStreamInbound_CheckedChanged(object sender, EventArgs e)
        {
            var stream = configer.GetComponent("stream") as Controller.ConfigerComponet.StreamSettings;
            stream.isServer = rbtnStreamInbound.Checked;
            configer.Update();
        }

        private void btnInsertStreamSetting(object sender, EventArgs e)
        {
            configer.Inject("stream");
        }

        private void btnVGC_Click(object sender, EventArgs e)
        {
            configer.Inject("vgc");
        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            configer.FormatCurrentContent();
        }

        private void btnQConSkipCN_Click(object sender, EventArgs e)
        {
            configer.InsertSkipCNSite();
        }

        private void saveConfigStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("ConfirmSaveCurConfig")))
            {
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
                    var visible = !setting.isShowConfigerToolsPanel;
                    setting.isShowConfigerToolsPanel = visible;
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
                cbox.SelectedIndex = table.Count > 0 ? 0 : -1;
            }

            FillComboBox(cboxConfigSection, Model.Data.Table.configSections);
            FillComboBox(cboxSSCMethod, Model.Data.Table.ssMethods);
            FillComboBox(cboxSSSMethod, Model.Data.Table.ssMethods);
            FillComboBox(cboxSSSNetwork, Model.Data.Table.ssNetworks);
            FillComboBox(cboxStreamTLS, Model.Data.Table.streamTLS);

            var streamType = new Dictionary<int, string>();
            foreach (var type in Model.Data.Table.streamSettings)
            {
                streamType.Add(type.Key, type.Value.name);
            }
            FillComboBox(cboxStreamType, streamType);
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
            if (readOnlyMode)
            {
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
        #endregion

        #region private method
        void InitConfiger()
        {
            configer = new Controller.Configer(scintillaMain, _serverIndex);

            configer.AddComponent(
                "vmess",
                new Controller.ConfigerComponet.Vmess(),
                new List<Control> {
                    tboxVMessID,
                    tboxVMessLevel,
                    tboxVMessAid,
                    tboxVMessIPaddr,
             });

            configer.AddComponent(
                "vgc",
                new Controller.ConfigerComponet.VGC(),
                new List<Control> {
                    tboxVGCAlias,
                    tboxVGCDesc,
                });

            configer.AddComponent(
                "stream",
                new Controller.ConfigerComponet.StreamSettings(),
                new List<Control> {
                    cboxStreamType,
                    cboxStreamParam,
                    cboxStreamTLS,
                });

            configer.AddComponent(
                "ssClient",
                new Controller.ConfigerComponet.SSClient(),
                new List<Control> {
                    tboxSSCAddr,
                    tboxSSCPass,
                    cboxSSCMethod,
                    chkSSCOTA,
                });

            configer.AddComponent(
            "ssServer",
            new Controller.ConfigerComponet.SSServer(),
            new List<Control> {
                    tboxSSSPass,
                    tboxSSSPort,
                    cboxSSSNetwork,
                    cboxSSSMethod,
                    chkSSSOTA,
            });

            configer.AddComponent(
                "import",
                new Controller.ConfigerComponet.Import(),
                new List<Control> { scintillaImport });
        }

        private void InitToolsPanel()
        {
            toolsPanelHandler.InitTimer();
            toolsPanelHandler.editor = new Rectangle(pnlEditor.Location, pnlEditor.Size);
            toolsPanelHandler.toolsPanel = new Rectangle(pnlTools.Location, pnlTools.Size);
            toolsPanelHandler.span = (this.ClientRectangle.Width - toolsPanelHandler.toolsPanel.Width - toolsPanelHandler.editor.Width) / 3;
            toolsPanelHandler.tabWidth = tabCtrlToolPanel.Left + tabCtrlToolPanel.ItemSize.Width;

            var page = tabCtrlToolPanel.TabPages[0];
            toolsPanelHandler.pageRect = new Rectangle(
                pnlTools.Location.X + page.Left,
                pnlTools.Location.Y + page.Top,
                page.Width,
                page.Height);

            for (int i = 0; i < tabCtrlToolPanel.TabCount; i++)
            {
                tabCtrlToolPanel.TabPages[i].MouseEnter += OnMouseEnterToolsPanel;
                tabCtrlToolPanel.TabPages[i].MouseLeave += (s, e) =>
                {
                    var rect = toolsPanelHandler.pageRect;
                    rect.Height = tabCtrlToolPanel.TabPages[0].Height;
                    if (!rect.Contains(this.PointToClient(Cursor.Position)))
                    {
                        OnMouseLeaveToolsPanel(s, e);
                    }
                };
            }

            tabCtrlToolPanel.MouseLeave += OnMouseLeaveToolsPanel;
            tabCtrlToolPanel.MouseEnter += OnMouseEnterToolsPanel;
        }

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
                int maxWidth = 0, temp = 0;
                var font = cboxExamples.Font;
                cboxExamples.Enabled = true;
                foreach (var description in descriptions)
                {
                    cboxExamples.Items.Add(description);
                    temp = TextRenderer.MeasureText(description, font).Width;
                    if (temp > maxWidth)
                    {
                        maxWidth = temp;
                    }
                }
                cboxExamples.DropDownWidth = Math.Max(
                    cboxExamples.Width,
                    maxWidth + SystemInformation.VerticalScrollBarWidth);
            }
            cboxExamples.SelectedIndex = 0;
        }

        void SetTitle(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.Text = formTitle;
            }
            else
            {
                this.Text = string.Format("{0} - {1}", formTitle, name);
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
            var formSize = this.ClientSize;
            var editorSize = pnlEditor.Size;

            pnlTools.Visible = false;
            pnlEditor.Visible = false;

            if (visible)
            {
                pnlTools.Width = toolsPanelHandler.toolsPanel.Width;
                pnlEditor.Left = pnlTools.Left + pnlTools.Width + toolsPanelHandler.span;
                pnlEditor.Width = this.ClientSize.Width - pnlEditor.Left - toolsPanelHandler.span;
            }
            else
            {
                pnlTools.Width = toolsPanelHandler.tabWidth;
                pnlEditor.Left = pnlTools.Left + pnlTools.Width;
                pnlEditor.Width = this.ClientSize.Width - pnlEditor.Left - toolsPanelHandler.span;
            }

            pnlTools.Visible = true;
            pnlEditor.Visible = true;

            showLeftPanelToolStripMenuItem.Checked = visible;
            hideLeftPanelToolStripMenuItem.Checked = !visible;
            setting.isShowConfigerToolsPanel = visible;
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

        private void btnImportClearCache_Click(object sender, EventArgs e)
        {
            configer.ClearHTMLCache();
        }

        private void cboxStreamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = cboxStreamType.SelectedIndex;
            if (index < 0)
            {
                cboxStreamParam.SelectedIndex = -1;
                cboxStreamParam.Items.Clear();
                return;
            }

            var s = Model.Data.Table.streamSettings[index];

            cboxStreamParam.Items.Clear();

            if (!s.dropDownStyle)
            {
                cboxStreamParam.DropDownStyle = ComboBoxStyle.Simple;
                return;
            }

            cboxStreamParam.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var option in s.options)
            {
                cboxStreamParam.Items.Add(option.Key);
            }
        }

        private void OnMouseEnterToolsPanel(object sender, EventArgs e)
        {
            toolsPanelHandler.ClearTimer();

            var width = toolsPanelHandler.toolsPanel.Width;
            if (pnlTools.Width != width)
            {
                pnlTools.Width = width;
            }
        }

        private void ResetToolsPanelWidth()
        {
            var visible = setting.isShowConfigerToolsPanel;
            var width = toolsPanelHandler.toolsPanel.Width;

            if (!visible)
            {
                width = toolsPanelHandler.tabWidth;
            }

            if (pnlTools.Width != width)
            {
                pnlTools.Width = width;
            }
        }

        private void tabCtrlToolPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (setting.isShowConfigerToolsPanel)
            {
                return;
            }

            for (int i = 0; i < tabCtrlToolPanel.TabCount; i++)
            {
                if (tabCtrlToolPanel.GetTabRect(i).Contains(e.Location))
                {
                    if (tabCtrlToolPanel.SelectedIndex != i)
                        tabCtrlToolPanel.SelectTab(i);
                    break;
                }
            }
        }

        private void OnMouseLeaveToolsPanel(object sender, EventArgs e)
        {
            toolsPanelHandler.SetTimer(ResetToolsPanelWidth);
        }

        private void cboxSSCMethod_SelectedIndexChanged(object sender, EventArgs e)
        {

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
