using Lunar.Resources.Langs;

namespace Lunar
{
    // Using lunar not lua to void naming conflicts.
    public class Lunar : VgcApis.Models.BaseClasses.Plugin
    {
        VgcApis.IService api;
        VgcApis.Models.IServices.IServersService vgcServers;
        VgcApis.Models.IServices.ISettingService vgcSettings;

        Views.WinForms.FormMain formMain = null;

        #region properties
        public override string Name => Properties.Resources.Name;
        public override string Version => Properties.Resources.Version;
        public override string Description => I18N.Description;
        #endregion

        #region protected overrides
        protected override void Popup()
        {
            if (formMain != null)
            {
                formMain.Activate();
                return;
            }

            formMain = new Views.WinForms.FormMain();
            formMain.FormClosed += (s, a) => formMain = null;
            formMain.Show();
        }

        protected override void Start(VgcApis.IService api)
        {
            this.api = api;
            vgcServers = api.GetVgcServersService();
            vgcSettings = api.GetVgcSettingService();
        }

        protected override void Stop()
        {
            if (formMain != null)
            {
                formMain.Close();
            }
        }
        #endregion
    }
}
