using ScintillaNET;
using System;
using System.Drawing;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

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
            InitCboxConfigSections();
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

        void InitCboxConfigSections() {
            cboxConfigSection.Items.Clear();

            var sections = Model.Table.configSections;
            foreach(var section in sections)
            {
                cboxConfigSection.Items.Add(section.Value);
            }
            cboxConfigSection.SelectedIndex = 0;

        }

        void SettingChange(object sender, EventArgs args)
        {
            UpdateServerListDelegate updater =
                new UpdateServerListDelegate(UpdateServerList);
            cboxServList.Invoke(updater, -1);
        }

        #region DataBinding
        void InitDataBinding()
        {
            configer = new Controller.Configer.Configer();
            BindDataVmessClient();
            BindDataSSRClient();
            BindDataStreamClient();
            BindDataEditor();
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

        void BindDataStreamClient()
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
        }

        void BindDataSSRClient()
        {
            var ssrClient = configer.ssrClient;

            var bs = new BindingSource();
            bs.DataSource = ssrClient;

            tboxSSREmail.DataBindings.Add("Text", bs, nameof(ssrClient.email));
            tboxSSRAddr.DataBindings.Add("Text", bs, nameof(ssrClient.addr));
            tboxSSRPass.DataBindings.Add("Text", bs, nameof(ssrClient.pass));

            cboxSSRMethod.DataBindings.Add(
                nameof(cboxSSRMethod.SelectedIndex),
                bs,
                nameof(ssrClient.method),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

            cboxSSRClientOTA.DataBindings.Add(
                nameof(cboxSSRClientOTA.Checked),
                bs,
                nameof(ssrClient.OTA),
                true,
                DataSourceUpdateMode.OnPropertyChanged);

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
            configer.InsertSSRClient();
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
            if (cboxShowPassWord.Checked == true)
            {
                tboxSSRPass.PasswordChar = '\0';
            }
            else
            {
                tboxSSRPass.PasswordChar = '*';
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
    }
}
