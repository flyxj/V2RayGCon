using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{

    public class Core : Model.BaseClass.SingletonService<Core>
    {
        Process v2rayCore;
        Setting setting;
        bool _isRunning;

        public event EventHandler OnCoreStatChange;

        Core()
        {
            v2rayCore = null;
            setting = Setting.Instance;
            _isRunning = false;
            setting.OnRequireCoreRestart += (s, a) => RestartCore();
        }

        #region properties
        public bool isRunning
        {
            get => _isRunning;
        }
        #endregion

        #region private method
        void OverwriteInboundSettings(JObject config)
        {
            var type = setting.proxyType;

            if (!(type == (int)Model.Data.Enum.ProxyTypes.http
                || type == (int)Model.Data.Enum.ProxyTypes.socks))
            {
                return;
            }

            var protocol = Model.Data.Table.proxyTypesString[type];

            Lib.Utils.TryParseIPAddr(setting.proxyAddr, out string ip, out int port);
            var tpl = JObject.Parse(resData("config_tpl"));
            var part = protocol + "In";
            try
            {
                config["inbound"]["protocol"] = protocol;
                config["inbound"]["listen"] = ip;
                config["inbound"]["port"] = port;
                config["inbound"]["settings"] = tpl[part];
                if (type == (int)Model.Data.Enum.ProxyTypes.socks)
                {
                    config["inbound"]["settings"]["ip"] = ip;
                }
            }
            catch
            {
                Debug.WriteLine("Core: Can not set local proxy address");
                MessageBox.Show(I18N("CoreCantSetLocalAddr"));
            }

        }

        void RestartCore()
        {
            var index = setting.GetCurServIndex();
            var b64Config = setting.GetServer(index);

            if (string.IsNullOrEmpty(b64Config))
            {
                return;
            }

            string plainText = Lib.Utils.Base64Decode(b64Config);
            JObject config = JObject.Parse(plainText);
            OverwriteInboundSettings(config);
            RestartCore(config.ToString());
        }

        void StartCore(string config)
        {
            if (v2rayCore != null)
            {
                Debug.WriteLine("Error: v2ray core is running!");
                return;
            }

            Debug.WriteLine("start v2ray core");

            v2rayCore = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = resData("Executable"),
                    Arguments = "-config=stdin: -format=json",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                }
            };

            v2rayCore.EnableRaisingEvents = true;
            v2rayCore.Exited += (s, e) =>
            {
                setting.SendLog(I18N("CoreExit"));
            };
            v2rayCore.ErrorDataReceived += (s, e) => setting.SendLog(e.Data);
            v2rayCore.OutputDataReceived += (s, e) => setting.SendLog(e.Data);

            try
            {
                v2rayCore.Start();
                v2rayCore.StandardInput.WriteLine(config);
                v2rayCore.StandardInput.Close();

                // Add to JOB object support win8+ only
                Lib.ChildProcessTracker.AddProcess(v2rayCore);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Excep: {0}", e);
                StopCore();
                MessageBox.Show(I18N("CantLauchCore"));
                return;
            }

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();

            _isRunning = true;
            OnCoreStatChange?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region public method
        public void RestartCore(string config)
        {
            StopCore();

            if (File.Exists(resData("Executable")))
            {
                StartCore(config);
            }
            else
            {
                MessageBox.Show(I18N("ExeNotFound"));
            }
        }

        public void StopCore()
        {
            if (v2rayCore == null)
            {
                Debug.WriteLine("v2ray-core is not runnig!");
            }
            else
            {
                Debug.WriteLine("kill v2ray core");
                try
                {
                    Lib.Utils.KillProcessAndChildrens(v2rayCore.Id);
                }
                catch { }
                v2rayCore = null;
            }

            _isRunning = false;
            OnCoreStatChange?.Invoke(this, EventArgs.Empty);
        }

        public string GetCoreVersion()
        {
            var ver = string.Empty;
            if (!File.Exists(resData("Executable")))
            {
                return ver;
            }

            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = resData("Executable"),
                    Arguments = "-version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }

            };

            Regex pattern = new Regex(@"v(?<version>[\d\.]+)");
            try
            {
                p.Start();
                while (!p.StandardOutput.EndOfStream)
                {
                    var output = p.StandardOutput.ReadLine();
                    Match match = pattern.Match(output);
                    if (match.Success)
                    {
                        ver = match.Groups["version"].Value;
                    }
                }
                p.WaitForExit();
            }
            catch { }

            return ver;
        }
        #endregion
    }
}
