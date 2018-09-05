using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class ServerMenuItems : FormMainComponentController
    {
        Service.Setting setting;
        Service.Cache cache;

        public ServerMenuItems(
            ToolStripMenuItem stopAllServers,
            ToolStripMenuItem restartAllServers,
            ToolStripMenuItem clearSysProxy,
            ToolStripMenuItem refreshSummary,
            ToolStripMenuItem restartAllAutorun)
        {
            setting = Service.Setting.Instance;
            cache = Service.Cache.Instance;

            stopAllServers.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmStopAllServer")))
                {
                    setting.StopAllServersThen();
                }
            };

            restartAllServers.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmRestartAllServer")))
                {
                    setting.RestartAllServers();
                }
            };

            clearSysProxy.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmClearSysProxy")))
                {
                    setting.ClearSystemProxy();
                }
            };

            restartAllAutorun.Click += (s, a) =>
            {
                if (Lib.UI.Confirm(I18N("ConfirmRestartAllAutorunServer")))
                {
                    setting.WakeupAutorunServers();
                }
            };

            refreshSummary.Click += (s, a) =>
            {
                cache.html.Clear();
                setting.UpdateAllServersSummary();
            };
        }


        #region public method
        public override bool RefreshUI()
        {
            return false;
        }

        public override void Cleanup()
        {
        }
        #endregion

        #region private method

        #endregion
    }
}
