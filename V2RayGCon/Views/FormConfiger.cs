using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormConfiger : Form
    {
        #region Sigleton
        static FormConfiger _instant;
        public static FormConfiger GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormConfiger();
            }
            return _instant;
        }
        #endregion

        // DataBinding
        Controller.Configer.Configer configer;

        Service.Setting setting;
        Scintilla scintilla;

        delegate void UpdateServerListDelegate(int index);

        FormConfiger()
        {
            setting = Service.Setting.Instance;

            InitializeComponent();
            InitComboBox();
            InitScintilla();
            InitDataBinding();
            UpdateServerList(setting.curEditingIndex);

            this.FormClosed += (s, a) =>
            {
                setting.OnSettingChange -= SettingChange;
            };

            this.Show();

            setting.OnSettingChange += SettingChange;
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

            FillComboBox(cboxConfigSection, Model.Table.configSections);
            FillComboBox(cboxSSCMethod, Model.Table.ssMethods);
            FillComboBox(cboxSSSMethod, Model.Table.ssMethods);
            FillComboBox(cboxSSSNetwork, Model.Table.ssNetworks);
        }

        void SettingChange(object sender, EventArgs args)
        {
            UpdateServerListDelegate updater =
                new UpdateServerListDelegate(UpdateServerList);
            cboxServList?.Invoke(updater, -1);
        }

        #region DataBinding
        void InitDataBinding()
        {
            configer = new Controller.Configer.Configer();
            BindDataVmessClient();
            BindDataSSRClient();
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

            cboxConfigSection.DataBindings.Add(
                nameof(cboxConfigSection.SelectedIndex),
                bs,
                nameof(editor.curSection),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataStreamSettings()
        {
            var streamClient = configer.streamClient;
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

            chkStreamIsInbound.DataBindings.Add(
                nameof(chkStreamIsInbound.Checked),
                bs,
                nameof(streamClient.isInbound),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        void BindDataSSServer()
        {
            var server = configer.ssServer;
            var bs = new BindingSource();
            bs.DataSource = server;

            tboxSSSEmail.DataBindings.Add("Text", bs, nameof(server.email));
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

        void BindDataSSRClient()
        {
            var ssrClient = configer.ssClient;

            var bs = new BindingSource();
            bs.DataSource = ssrClient;

            tboxSSCEmail.DataBindings.Add("Text", bs, nameof(ssrClient.email));
            tboxSSCAddr.DataBindings.Add("Text", bs, nameof(ssrClient.addr));
            tboxSSCPass.DataBindings.Add("Text", bs, nameof(ssrClient.pass));

            cboxSSCMethod.DataBindings.Add(
                nameof(cboxSSCMethod.SelectedIndex),
                bs,
                nameof(ssrClient.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            chkSSCOTA.DataBindings.Add(
                nameof(chkSSCOTA.Checked),
                bs,
                nameof(ssrClient.OTA),
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSaveChanges_Click(object sender, EventArgs e)
        {
            configer.SaveChanges();
        }


        private void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            configer.DiscardChanges();
        }

        private void btnOverwriteServConfig_Click(object sender, EventArgs e)
        {
            configer.OverwriteServerConfig(cboxServList.SelectedIndex);
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

        private void btnInsertNewServ_Click(object sender, EventArgs e)
        {
            configer.InsertNewServer();
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

        void UpdateServerList(int index = -1)
        {
            var oldIndex = index >= 0 ? index : cboxServList.SelectedIndex;
            cboxServList.Items.Clear();

            var aliases = setting.GetAllAliases();

            if (aliases.Count <= 0)
            {
                return;
            }

            foreach (var alias in aliases)
            {
                cboxServList.Items.Add(alias);
            }

            cboxServList.SelectedIndex = Lib.Utils.Clamp(oldIndex, 0, aliases.Count - 1);
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
    }
}
