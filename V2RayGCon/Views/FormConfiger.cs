using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormConfiger : Form
    {
        Controller.Configer.Configer configer;
        Service.Setting setting;
        Scintilla scintilla;
        string formTitle;

        delegate void UpdateServerMenuDelegate();


        public FormConfiger()
        {
            setting = Service.Setting.Instance;
            configer = new Controller.Configer.Configer();

            InitializeComponent();

            formTitle = this.Text;
            InitComboBox();
            InitScintilla();
            InitDataBinding();
            UpdateServerMenu();
            ShowToolsPanel(setting.isShowConfigerLeftPanel);

            cboxConfigSection.SelectedIndex = 0;

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            this.Show();
            setting.OnSettingChange += SettingChange;

            SetTitle(configer.GetAlias());
        }

        void ShowToolsPanel(bool visible)
        {
            var margin = 4;
            var formSize = this.ClientSize;
            var editorSize = pnlEditor.Size;

            if (visible)
            {
                showLeftPanelToolStripMenuItem.Checked = true;
                hideLeftPanelToolStripMenuItem.Checked = false;
                pnlTools.Visible = true;
                pnlEditor.Left = pnlTools.Left + pnlTools.Width + margin;
                editorSize.Width = formSize.Width - pnlTools.Width - margin * 3;
            }
            else
            {
                showLeftPanelToolStripMenuItem.Checked = false;
                hideLeftPanelToolStripMenuItem.Checked = true;
                pnlTools.Visible = false;
                pnlEditor.Left = margin;
                editorSize.Width = formSize.Width - margin * 2;
            }

            pnlEditor.Size = editorSize;
            setting.isShowConfigerLeftPanel = visible;
        }

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
        }

        void SettingChange(object sender, EventArgs args)
        {
            UpdateServerMenuDelegate updater =
                new UpdateServerMenuDelegate(UpdateServerMenu);
            mainMenu?.Invoke(updater);
        }

        #region DataBinding
        void InitDataBinding()
        {
            BindDataVmessClient();
            BindDataSSClient();
            BindDataStreamSettings();
            BindDataEditor();
            BindDataSSServer();
            BindDataVmessServer();
        }

        void BindDataEditor()
        {
            // bind scintilla
            var editor = configer.editor;
            var bs = new BindingSource();
            bs.DataSource = editor;
            scintilla.DataBindings.Add("Text", bs, nameof(editor.content));
        }

        void BindDataStreamSettings()
        {
            var streamClient = configer.streamSettings;
            var bs = new BindingSource();
            bs.DataSource = streamClient;
            tboxKCPType.DataBindings.Add("Text", bs, nameof(streamClient.kcpType));
            tboxWSPath.DataBindings.Add("Text", bs, nameof(streamClient.wsPath));
            cboxStreamSecurity.DataBindings.Add(
                nameof(cboxStreamSecurity.SelectedIndex),
                bs,
                nameof(streamClient.tls),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
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

        void BindDataVmessServer()
        {
            var server = configer.vmessServer;
            var bs = new BindingSource();
            bs.DataSource = server;

            tboxVServID.DataBindings.Add("Text", bs, nameof(server.ID));
            tboxVServLevel.DataBindings.Add("Text", bs, nameof(server.level));
            tboxVServAID.DataBindings.Add("Text", bs, nameof(server.altID));
            tboxVServPort.DataBindings.Add("Text", bs, nameof(server.port));
        }

        void BindDataVmessClient()
        {
            var vmessClient = configer.vmessClient;
            var bsVmessClient = new BindingSource();
            bsVmessClient.DataSource = vmessClient;

            tboxVMessID.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.ID));
            tboxVMessLevel.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.level));
            tboxVMessAid.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.altID));
            tboxVMessIPaddr.DataBindings.Add("Text", bsVmessClient, nameof(vmessClient.addr));
        }

        #endregion

        void InitScintilla()
        {
            scintilla = new Scintilla();
            panelScintilla.Controls.Add(scintilla);

            // scintilla.Dock = DockStyle.Fill;
            scintilla.Dock = DockStyle.Fill;
            scintilla.WrapMode = WrapMode.None;
            scintilla.IndentationGuides = IndentView.LookBoth;

            // Configure the JSON lexer styles
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
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
                var _i = i;
                menuRepalceServer.Add(new ToolStripMenuItem(aliases[i], null, (s, a) =>
                {
                    ScintillaLostFocus();
                    if (Lib.UI.Confirm(I18N("ReplaceServer")))
                    {
                        configer.ReplaceServer(_i);
                        SetTitle(configer.GetAlias());
                    }
                }));

                menuLoadServer.Add(new ToolStripMenuItem(aliases[i], null, (s, a) =>
                {
                    if (!Lib.UI.Confirm(I18N("ConfirmLoadNewServer")))
                    {
                        return;
                    }
                    configer.LoadServer(_i);
                    cboxConfigSection.SelectedIndex = 0;
                    SetTitle(configer.GetAlias());
                }));
            }
        }

        void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (configer.IsValid())
            {
                configer.SaveChanges();
            }
            else
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
            }
        }

        private void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            configer.DiscardChanges();
        }

        private void btnLoadExample_Click(object sender, EventArgs e)
        {
            configer.LoadExample();
        }

        private void btnVMessInsertClient_Click(object sender, EventArgs e)
        {
            configer.InsertVmessClient();
        }

        private void btnSSRInsertClient_Click(object sender, EventArgs e)
        {
            configer.InsertSSClient();
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

        private void btnVMessGenUUID_Click(object sender, EventArgs e)
        {
            configer.vmessClient.ID = Guid.NewGuid().ToString();
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

        private void btnGenVServID_Click(object sender, EventArgs e)
        {
            configer.vmessServer.ID = Guid.NewGuid().ToString();
        }

        private void btnInsertVServ_Click(object sender, EventArgs e)
        {
            configer.InsertVmessServer();
        }

        private void chkStreamSettingsIsServer_CheckedChanged(object sender, EventArgs e)
        {
            configer.StreamSettingsIsServerChange(chkStreamIsServer.Checked);
        }

        private void cboxConfigSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (configer.SectionChanged(cboxConfigSection.SelectedIndex))
            {
                cboxConfigSection.SelectedIndex = configer.perSection;
            }
        }

        private void showLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowToolsPanel(true);
        }

        private void hideLeftPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowToolsPanel(false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScintillaLostFocus();

            if (!configer.IsValid())
            {
                MessageBox.Show(I18N("PleaseCheckConfig"));
                return;
            }
            configer.SaveChanges();

            if (Lib.UI.DlgWriteFile(resData("ExtJson"),
                configer.GetConfigFormated(),
                out string filename))
            {
                SetTitle(filename);
                MessageBox.Show(I18N("Done"));
            }
            else
            {
                MessageBox.Show(I18N("WriteFileFail"));
            }
        }

        void ScintillaLostFocus()
        {
            tboxVMessID.Focus();
        }

        private void addNewServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScintillaLostFocus();
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
            string json = Lib.UI.DlgReadFile(resData("ExtJson"), out string filename);
            if (configer.SetConfig(json))
            {
                cboxConfigSection.SelectedIndex = 0;
                SetTitle(filename);
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
    }
}
