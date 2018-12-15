using System.Windows.Forms;

namespace Luna.Views.WinForms
{
    public partial class FormMain : Form
    {
        Controllers.EditorCtrl editorCtrl;
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
            editorCtrl = new Controllers.EditorCtrl(
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

            this.FormClosed += (s, a) => editorCtrl.Cleanup();
        }
    }
}
