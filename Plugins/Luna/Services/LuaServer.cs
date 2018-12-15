using System.Collections.Generic;
using System.Linq;

namespace Luna.Services
{
    public class LuaServer
    {
        Settings settings;
        List<Controllers.LuaCoreCtrl> luaCoreCtrls;
        Models.Apis.LuaApis luaApis;

        public LuaServer()
        {

        }

        #region public methods
        public bool RemoveScriptByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            var coreCtrl = luaCoreCtrls.FirstOrDefault(c => c.name == name);
            if (coreCtrl == null)
            {
                return false;
            }

            coreCtrl.Kill();
            luaCoreCtrls.Remove(coreCtrl);

            settings.GetLuaCoreStates().RemoveAll(s => s.name == name);
            Save();
            return true;
        }

        public bool AddOrReplaceScript(string name, string script)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            var coreCtrl = luaCoreCtrls
                .FirstOrDefault(c => c.name == name);

            if (coreCtrl != null)
            {
                coreCtrl.ReplaceScript(script);
                return true;
            }

            var coreState = new Models.Data.LuaCoreSetting
            {
                name = name,
                script = script,
            };

            settings.GetLuaCoreStates().Add(coreState);

            coreCtrl = new Controllers.LuaCoreCtrl();
            luaCoreCtrls.Add(coreCtrl);
            coreCtrl.Run(settings, coreState, luaApis);
            Save();
            return true;
        }

        public void Run(
            Settings settings,
            VgcApis.Models.IServices.IServersService vgcServers)
        {
            this.settings = settings;
            this.luaApis = new Models.Apis.LuaApis();
            luaApis.Run(settings, vgcServers);

            luaCoreCtrls = InitLuaCores(settings, luaApis);
            WakeUpAutoRunScripts();
        }

        public void Cleanup()
        {
            if (luaCoreCtrls == null)
            {
                return;
            }

            foreach (var ctrl in luaCoreCtrls)
            {
                ctrl.Kill();
            }
        }
        #endregion

        #region private methods
        void Save() => settings.SaveSettings();

        void WakeUpAutoRunScripts()
        {
            var list = luaCoreCtrls.Where(c => c.isAutoRun).ToList();
            if (list.Count() <= 0)
            {
                return;
            }
            foreach (var core in list)
            {
                core.Start();
            }
        }

        List<Controllers.LuaCoreCtrl> InitLuaCores(
            Settings settings,
            VgcApis.ILuaApis luaApis)
        {
            var cores = new List<Controllers.LuaCoreCtrl>();
            foreach (var luaCoreState in settings.GetLuaCoreStates())
            {
                var luaCtrl = new Controllers.LuaCoreCtrl();
                luaCtrl.Run(settings, luaCoreState, luaApis);
                cores.Add(luaCtrl);
            }
            return cores;
        }
        #endregion
    }
}
