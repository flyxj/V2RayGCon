using NLua;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Luna.Controllers
{
    class LuaCoreCtrl
    {
        Services.Settings settings;
        Models.Data.LuaCoreSetting coreSetting;
        VgcApis.ILuaApis luaApis;

        Thread luaCoreThread;
        Task luaCoreTask;

        readonly object coreStateLocker = new object();

        public LuaCoreCtrl() { }

        #region properties 
        public string name => coreSetting.name;

        public bool isAutoRun
        {
            get => coreSetting.isAutorun;
            set
            {
                coreSetting.isAutorun = value;
                settings.SaveSettings();
            }
        }

        bool _isRunning = false;
        public bool isRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                SendLog($"lua core is running: {_isRunning}");
                if (value == false)
                {

                    luaCoreTask = null;
                    luaCoreThread = null;
                }
            }
        }

        bool _signalStop = false;
        bool signalStop
        {
            get => _signalStop;
            set
            {
                _signalStop = value;
                if (value)
                {
                    SendLog($"lua stop signal: {_signalStop}");
                }
            }
        }
        #endregion

        #region public methods
        public void ReplaceScript(string script)
        {
            coreSetting.script = script;
            Save();
        }

        public void Stop()
        {
            lock (coreStateLocker)
            {
                if (!isRunning)
                {
                    return;
                }
            }

            signalStop = true;
        }

        public void Kill()
        {
            if (luaCoreTask == null)
            {
                return;
            }

            signalStop = true;
            if (luaCoreTask.Wait(2000))
            {
                return;
            }

            try
            {
                luaCoreThread?.Abort();
            }
            catch { }
            isRunning = false;
        }

        public void Start()
        {
            lock (coreStateLocker)
            {
                if (isRunning)
                {
                    return;
                }
                isRunning = true;
            }

            luaCoreTask = Task.Factory.StartNew(
                RunLuaScript,
                TaskCreationOptions.LongRunning);
        }

        public void Cleanup()
        {
            Kill();
        }

        public void Run(
            Services.Settings settings,
            Models.Data.LuaCoreSetting luaCoreState,
            VgcApis.ILuaApis luaApis)
        {
            this.settings = settings;
            this.coreSetting = luaCoreState;
            this.luaApis = luaApis;
        }

        #endregion

        #region private methods
        void SendLog(string content)
            => luaApis.Log(content);

        void RunLuaScript()
        {
            signalStop = false;
            luaCoreThread = Thread.CurrentThread;

            try
            {

                var core = CreateLuaCore();
                core.DoString(coreSetting.script);
            }
            catch (Exception e)
            {
                SendLog(e.ToString());
            }
            isRunning = false;
        }

        Lua CreateLuaCore()
        {
            var state = new Lua();
            state["api"] = luaApis;
            state["stop"] = signalStop;
            // disable import
            state.DoString(@"import = function () end");
            return state;
        }

        void Save() => settings.SaveSettings();

        #endregion
    }
}
