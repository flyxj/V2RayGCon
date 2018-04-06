using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V2RayGCon.Service
{

    class Core
    {
        #region Singleton
        static readonly Core instV2RCore = new Core();
        public static Core Instance
        {
            get
            {
                return instV2RCore;
            }
        }
        #endregion

        // Class begin
        Process v2rayCore = null;

        Func<string, string> resData, I18N;
        Setting setting;

        public event EventHandler<Model.DataEvent> OnLog;
        public event EventHandler OnCoreStatChange;

        Core()
        {
            resData = Lib.Utils.resData;
            I18N = Lib.Utils.I18N;
            setting = Setting.Instance;

            setting.OnRequireCoreRestart += ChangeConfigFile;
        }

        void ChangeConfigFile(object sender, EventArgs ev)
        {
            var index = setting.GetSelectedServerIndex();
            var b64Config = setting.GetServer(index);
            if (string.IsNullOrEmpty(b64Config))
            {
                return;
            }

            string plainText = Lib.Utils.Base64Decode(b64Config);
            JObject config = JObject.Parse(plainText);

            Lib.Utils.TryParseIPAddr(setting.proxyAddr, out string ip, out int port);
            try
            {
                config["inbound"]["protocol"] = setting.proxyType;
                config["inbound"]["listen"] = ip;
                config["inbound"]["port"] = port;
            }
            catch
            {
                Debug.WriteLine("Core: Can not insert local proxy address");
                MessageBox.Show(I18N("CoreCantSetLocalAddr"));
            }
            var fileName = resData("ConfigFileName");
            File.WriteAllText(fileName, config.ToString());

            LogMsg(string.Format("\r\n\r\nLocal proxy {0}://{1}:{2}", setting.proxyType, ip, port));
            RestartCore();
        }

        public bool IsRunning()
        {
            return v2rayCore != null;
        }

        bool IsExeExist()
        {
            var fileName = resData("Executable");
            if (!File.Exists(fileName))
            {
                return false;
            }
            return true;
        }

        public void RestartCore()
        {
            StopCore();
            if (IsExeExist())
            {
                StartCore();
            }
            else
            {
                MessageBox.Show(I18N("ExeNotFound"));
            }
        }

        void StartCore()
        {
            if (v2rayCore != null)
            {
                Debug.WriteLine("v2ray core is running, skip.");
                return;
            }

            Debug.WriteLine("start v2ray core");

            var fileName = resData("Executable");

            v2rayCore = new Process();
            v2rayCore.StartInfo.FileName = fileName;
            v2rayCore.EnableRaisingEvents = true;

            // set up output redirection
            v2rayCore.StartInfo.CreateNoWindow = true;
            v2rayCore.StartInfo.RedirectStandardOutput = true;
            v2rayCore.StartInfo.RedirectStandardError = true;
            v2rayCore.StartInfo.UseShellExecute = false;

            // see below for output handler
            v2rayCore.ErrorDataReceived += LogDeliver;
            v2rayCore.OutputDataReceived += LogDeliver;

            v2rayCore.Exited += (s, e) =>
            {
                LogMsg(I18N("CoreExit"));
            };

            try
            {
                v2rayCore.Start();
                Lib.ChildProcessTracker.AddProcess(v2rayCore);
            }
            catch
            {
                MessageBox.Show(I18N("CantLauchCore"));
                StopCore();
                return;
            }

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();
            OnCoreStatChange?.Invoke(this, null);

        }

        void LogMsg(string msg)
        {
            Debug.WriteLine(msg);
            var arg = new Model.DataEvent(msg);
            OnLog?.Invoke(this, arg);
        }

        void LogDeliver(object sender, DataReceivedEventArgs e)
        {
            var arg = new Model.DataEvent(e.Data);
            OnLog?.Invoke(this, arg);
        }

        public void StopCore()
        {
            if (v2rayCore != null)
            {
                Debug.WriteLine("kill v2ray core");
                Lib.Utils.KillProcessAndChildrens(v2rayCore.Id);
                if (!v2rayCore.HasExited)
                {
                    v2rayCore.WaitForExit(3000);
                }
            }
            else
            {
                Debug.WriteLine("v2ray-core is not runnig!");
            }
            v2rayCore = null;
            OnCoreStatChange?.Invoke(this, null);
        }
    }
}
