using System.Windows.Forms;

namespace V2RayGCon.Views.UserControls
{
    public partial class PluginInfoUI : UserControl
    {
        public PluginInfoUI(Model.Data.PluginInfoItem pluginInfo)
        {
            InitializeComponent();

            lbFilename.Text = pluginInfo.filename;
            lbName.Text = pluginInfo.name;
            lbVersion.Text = pluginInfo.version;
            lbDescription.Text = pluginInfo.description;
            chkIsUse.Checked = pluginInfo.isUse;
        }

        public Model.Data.PluginInfoItem GetValue()
        {
            return new Model.Data.PluginInfoItem
            {
                isUse = chkIsUse.Checked,
                name = lbName.Text,
                filename = lbFilename.Text,
                version = lbVersion.Text,
                description = lbDescription.Text,
            };
        }

    }
}
