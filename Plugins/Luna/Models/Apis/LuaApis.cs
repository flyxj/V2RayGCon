using System;
using System.Collections.Generic;
using System.Linq;

namespace Luna.Models.Apis
{
    public class LuaApis : VgcApis.Models.Interfaces.ILuaApis
    {
        Services.Settings settings;
        VgcApis.Models.IServices.IServersService vgcServers;
        Action<string> redirectLogWorker;
        List<VgcApis.Models.IControllers.ICoreCtrl> serverListCache;

        public LuaApis() { }

        #region ILuaApis
        VgcApis.Models.IControllers.ICoreCtrl GetServerByUid(string uid) =>
            serverListCache.FirstOrDefault(s => s.GetUid() == uid);
        VgcApis.Models.IControllers.ICoreCtrl GetServerByName(string name) =>
            serverListCache.FirstOrDefault(s => s.GetName() == name);


        public void UpdateServerListCache()
        {
            serverListCache = vgcServers
                .GetAllServersList()
                .ToList();
        }

        public string[] GetAllServerUid() =>
            serverListCache
                .Select(s => s.GetUid())
                .ToArray();

        public string GetServerUidByName(string name) =>
            GetServerByName(name)?.GetUid() ?? "";

        public string GetServerNameByUid(string uid) =>
            GetServerByUid(uid)?.GetName() ?? "";


        public string GetServerTitleByUid(string uid) =>
            GetServerByUid(uid)?.GetTitle() ?? "";

        public void SelectServerByUid(string uid) =>
            GetServerByUid(uid)?.SetIsSelected(true);

        public void UnSelectServerByUid(string uid) =>
            GetServerByUid(uid)?.SetIsSelected(false);

        public void StartServerByUid(string uid) =>
            GetServerByUid(uid)?.RestartCoreThen();

        public void StopServerByUid(string uid) =>
            GetServerByUid(uid)?.StopCoreThen();

        public void Sleep(int milliseconds) =>
            System.Threading.Thread.Sleep(milliseconds);

        public void Print(params object[] contents)
        {
            var text = "";
            foreach (var c in contents)
            {
                text += c.ToString();
            }
            redirectLogWorker(text);
        }
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
            UpdateServerListCache();
        }
        #endregion

        #region private methods
        #endregion
    }
}
