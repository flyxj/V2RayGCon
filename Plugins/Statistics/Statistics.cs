using Statistics.Resources.Langs;

namespace Statistics
{
    public class Statistics : VgcApis.IPlugin
    {
        VgcApis.IServices api;
        VgcApis.Models.IServersService vgcServers;
        VgcApis.Models.ISettingService vgcSetting;
        Views.WinForms.FormMain formMain = null;
        Services.Settings settings;

        #region properties
        public string Name => Properties.Resources.Name;
        public string Version => Properties.Resources.Version;
        public string Description => I18N.Description;
        #endregion

        #region public methods
        public void Run(VgcApis.IServices api)
        {
            this.api = api;
            vgcSetting = api.GetVgcSettingService();
            vgcServers = api.GetVgcServersService();

            settings = new Services.Settings();
            settings.Run(vgcSetting);
        }

        public void Show()
        {
            if (formMain != null)
            {
                formMain.Activate();
                return;
            }

            formMain = new Views.WinForms.FormMain(
                settings,
                vgcServers);
            formMain.FormClosed += (s, a) => formMain = null;
            formMain.Show();
        }

        public void Cleanup()
        {
            if (formMain != null)
            {
                formMain.Close();
            }

            settings.Cleanup();
        }
        #endregion

    }
}
