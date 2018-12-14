using System;

namespace Lunar.Models.Apis
{
    public class LuaApis : VgcApis.ILuaApis
    {
        Services.Settings settings;
        VgcApis.Models.IServices.IServersService vgcServers;
        Action<string> redirectLogWorker;

        public LuaApis() { }

        #region ILuaApis
        public void Sleep(int milliseconds) =>
            System.Threading.Thread.Sleep(milliseconds);

        public void Log(string content) =>
            redirectLogWorker(content);
        #endregion

        #region public methods
        public void SetRedirectLogWorker(Action<string> worker)
        {
            if (worker != null)
            {
                redirectLogWorker = worker;
            }
        }

        public void Run(
            Services.Settings settings,
            VgcApis.Models.IServices.IServersService vgcServers)
        {
            this.settings = settings;
            this.vgcServers = vgcServers;
            this.redirectLogWorker = settings.SendLog;
        }
        #endregion

        #region private methods
        #endregion
    }
}
