using Luna.Resources.Langs;
using System.Windows.Forms;

namespace Luna.Views.WinForms
{
    public partial class FormMain : Form
    {
        Controllers.TabEditorCtrl editorCtrl;
        Controllers.TabGeneralCtrl genCtrl;

        Services.LuaServer luaServer;
        Services.Settings settings;
        VgcApis.Models.IServices.IServersService vgcServers;

        public FormMain(
            VgcApis.Models.IServices.IServersService vgcServers,
            Services.Settings settings,
            Services.LuaServer luaServer)
        {
            this.vgcServers = vgcServers;
            this.settings = settings;
            this.luaServer = luaServer;
            InitializeComponent();
            VgcApis.Libs.UI.AutoSetFormIcon(this);
            this.Text = Properties.Resources.Name + " v" + Properties.Resources.Version;
        }

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            FixSplitPanelWidth();
            lbStatusBarMsg.Text = "";

            // TabGeneral should initialize before TabEditor.
            genCtrl = new Controllers.TabGeneralCtrl(
                flyScriptUIContainer,
                btnStopAllScript,
                btnKillAllScript);

            genCtrl.Run(luaServer);

            editorCtrl = new Controllers.TabEditorCtrl(
                cboxScriptName,
                btnSaveScript,
                btnRemoveScript,
                btnRunScript,
                btnStopScript,
                btnKillScript,
                btnClearOutput,
                rtBoxOutput,
                pnlScriptEditor);

            editorCtrl.Run(vgcServers, settings, luaServer);

            this.FormClosing += FormClosingHandler;
            this.FormClosed += (s, a) =>
            {
                // reverse order 
                editorCtrl.Cleanup();
                genCtrl.Cleanup();
            };
        }

        void FixSplitPanelWidth()
        {
            splitContainerTabEditor.SplitterDistance = this.Width * 6 / 10;
        }

        private void FormClosingHandler(object sender, FormClosingEventArgs e)
        {
            if (settings.IsShutdown())
            {
                return;
            }

            if (editorCtrl.IsChanged())
            {
                e.Cancel = !VgcApis.Libs.UI.Confirm(I18N.DiscardUnsavedChanges);
            }
        }

    }
}
