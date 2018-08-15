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
        public int span;
        public int tabWidth;

        public Rectangle panel;
        public Rectangle editor;
        public Rectangle page;

        public Model.BaseClass.CancelableTimeout timer;

        public void Dispose()
        {
            timer?.Dispose();
        }
    };
    #endregion

    public partial class FormConfiger : Form
    {
        Controller.Configer configer;
        Service.Setting setting;
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
            InitConfiger();

            UpdateServerMenu();
            SetTitle(configer.GetAlias());
            ToggleToolsPanel(setting.isShowConfigerToolsPanel);

            this.FormClosing += (s, a) =>
            {
                if (!configer.IsConfigSaved())
                {
                    a.Cancel = !Lib.UI.Confirm(I18N("ConfirmCloseWinWithoutSave"));
                }
            };

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= OnSettingChange;
                toolsPanelHandler.Dispose();
            };

            var editor = configer.GetComponent<Controller.ConfigerComponet.Editor>();
            editor.GetEditor().Click += OnMouseLeaveToolsPanel;

            setting.OnSettingChange += OnSettingChange;
        }

        #region UI event handler
        private void ShowLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolsPanel(true);
        }

        private void HideLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleToolsPanel(false);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configer.InjectConfigHelper(null);

            switch (Lib.UI.ShowSaveFileDialog(
                StrConst("ExtJson"),
                configer.GetConfigFormated(),
                out string filename))
            {
                case Model.Data.Enum.SaveFileErrorCode.Success:
                    SetTitle(filename);
                    configer.MarkOriginalFile();
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

        private void AddNewServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("AddNewServer")))
            {
                configer.AddNewServer();
                SetTitle(configer.GetAlias());
            }
        }

        private void LoadJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!configer.IsConfigSaved()
                && !Lib.UI.Confirm(I18N("ConfirmLoadNewServer")))
            {
                return;
            }

            string json = Lib.UI.ShowReadFileDialog(StrConst("ExtJson"), out string filename);

            // user cancelled.
            if (json == null)
            {
                return;
            }

            if (configer.LoadJsonFromFile(json))
            {
                cboxConfigSection.SelectedIndex = 0;
                SetTitle(filename);

                // SetTitle is enough.
                // MessageBox.Show(I18N("Done"));
            }
            else
            {
                MessageBox.Show(I18N("LoadJsonFail"));
            }
        }

        private void NewWinToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new FormConfiger();
        }

        private void SearchBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSearchBox();
        }

        private void SaveConfigStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Lib.UI.Confirm(I18N("ConfirmSaveCurConfig")))
            {
                if (configer.ReplaceOriginalServer())
                {
                    SetTitle(configer.GetAlias());
                }
            }
        }

        private void TabCtrlToolPanel_MouseMove(object sender, MouseEventArgs e)
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
                    configer.InjectConfigHelper(null);
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyCode);
        }
        #endregion

        #region init
        void InitConfiger()
        {
            var components = new List<Model.BaseClass.ConfigerComponent> {

                new Controller.ConfigerComponet.Editor(
                    panelScintilla,
                    cboxConfigSection,
                    cboxExamples,
                    btnFormat,
                    btnClearModify),

                new Controller.ConfigerComponet.Vmess(
                    tboxVMessID,
                    tboxVMessLevel,
                    tboxVMessAid,
                    tboxVMessIPaddr,
                    rbtnVmessIServerMode,
                    btnVMessGenUUID,
                    btnVMessInsertClient),

                new Controller.ConfigerComponet.VGC(
                    tboxVGCAlias,
                    tboxVGCDesc,
                    btnInsertVGC),

                new Controller.ConfigerComponet.StreamSettings(
                    cboxStreamType,
                    cboxStreamParam,
                    cboxStreamTLS,
                    rbtnStreamInbound,
                    btnInsertStream),

                new Controller.ConfigerComponet.SSClient(
                    tboxSSCAddr,
                    tboxSSCPass,
                    cboxSSCMethod,
                    chkSSCOTA,
                    chkSSCShowPass,
                    btnSSRInsertClient),

                new Controller.ConfigerComponet.SSServer(
                    tboxSSSPass,
                    tboxSSSPort,
                    cboxSSSNetwork,
                    cboxSSSMethod,
                    chkSSSOTA,
                    chkSSSShowPass,
                    btnSSInsertServer),

                new Controller.ConfigerComponet.Import(
                    panelExpandConfig,
                    btnExpandImport,
                    btnImportClearCache,
                    btnCopyExpansedConfig),

                new Controller.ConfigerComponet.Quick(
                    btnQConSkipCN,
                    btnQConMTProto),

            };

            configer = new Controller.Configer(_serverIndex);
            configer.Plug(components);
            configer.Prepare();
        }

        void InitToolsPanel()
        {
            toolsPanelHandler.editor = new Rectangle(pnlEditor.Location, pnlEditor.Size);
            toolsPanelHandler.panel = new Rectangle(pnlTools.Location, pnlTools.Size);

            toolsPanelHandler.span = (ClientRectangle.Width - toolsPanelHandler.panel.Width - toolsPanelHandler.editor.Width) / 3;
            toolsPanelHandler.tabWidth = tabCtrlToolPanel.Left + tabCtrlToolPanel.ItemSize.Width;

            toolsPanelHandler.timer = new Model.BaseClass.CancelableTimeout(ResetToolsPanelWidth, 800);

            var page = tabCtrlToolPanel.TabPages[0];
            toolsPanelHandler.page = new Rectangle(
                pnlTools.Location.X + page.Left,
                pnlTools.Location.Y + page.Top,
                page.Width,
                page.Height);

            for (int i = 0; i < tabCtrlToolPanel.TabCount; i++)
            {
                tabCtrlToolPanel.TabPages[i].MouseEnter += OnMouseEnterToolsPanel;
                tabCtrlToolPanel.TabPages[i].MouseLeave += (s, e) =>
                {
                    var rect = toolsPanelHandler.page;
                    rect.Height = tabCtrlToolPanel.TabPages[0].Height;
                    if (!rect.Contains(PointToClient(Cursor.Position)))
                    {
                        OnMouseLeaveToolsPanel(s, e);
                    }
                };
            }

            tabCtrlToolPanel.MouseLeave += OnMouseLeaveToolsPanel;
            tabCtrlToolPanel.MouseEnter += OnMouseEnterToolsPanel;
        }
        #endregion

        #region private method

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
            var menuReplaceServer = replaceExistServerToolStripMenuItem.DropDownItems;
            var menuLoadServer = loadServerToolStripMenuItem.DropDownItems;

            menuReplaceServer.Clear();
            menuLoadServer.Clear();

            var aliases = setting.GetAllAliases();

            var enable = aliases.Count > 0;
            replaceExistServerToolStripMenuItem.Enabled = enable;
            loadServerToolStripMenuItem.Enabled = enable;

            for (int i = 0; i < aliases.Count; i++)
            {
                var index = i;
                menuReplaceServer.Add(new ToolStripMenuItem(aliases[i], null, (s, a) =>
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
                    if (!configer.IsConfigSaved()
                    && !Lib.UI.Confirm(I18N("ConfirmLoadNewServer")))
                    {
                        return;
                    }

                    configer.LoadServer(index);
                    SetTitle(configer.GetAlias());
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
                pnlTools.Width = toolsPanelHandler.panel.Width;
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

        void OnSettingChange(object sender, EventArgs args)
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

        void OnMouseEnterToolsPanel(object sender, EventArgs e)
        {
            toolsPanelHandler.timer.Cancel();

            var width = toolsPanelHandler.panel.Width;
            if (pnlTools.Width != width)
            {
                pnlTools.Width = width;
            }
        }

        void ResetToolsPanelWidth()
        {
            var visible = setting.isShowConfigerToolsPanel;
            var width = toolsPanelHandler.panel.Width;

            if (!visible)
            {
                width = toolsPanelHandler.tabWidth;
            }

            if (pnlTools.Width != width)
            {
                pnlTools.Width = width;
            }
        }

        void OnMouseLeaveToolsPanel(object sender, EventArgs e)
        {
            toolsPanelHandler.timer.Start();
        }

        void ShowSearchBox()
        {
            if (formSearch != null)
            {
                return;
            }
            var editor = configer.GetComponent<Controller.ConfigerComponet.Editor>();
            formSearch = new FormSearch(editor.GetEditor());
            formSearch.FormClosed += (s, a) => formSearch = null;
        }
        #endregion
    }
}
