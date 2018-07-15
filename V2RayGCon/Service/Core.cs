using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{

    public class Core : Model.BaseClass.SingletonService<Core>
    {
        Process v2rayCore;
        Setting setting;
        Cache cache;

        public event EventHandler OnCoreStatChange;

        Core()
        {
            v2rayCore = null;
            cache = Cache.Instance;
            setting = Setting.Instance;
            setting.OnRequireCoreRestart += (s, a) =>
            {
                Task.Factory.StartNew(() => CoreRestartHandler());
            };
        }

        #region properties
        public bool isRunning
        {
            get => v2rayCore != null;
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
            var part = protocol + "In";
            try
            {
                config["inbound"]["protocol"] = protocol;
                config["inbound"]["listen"] = ip;
                config["inbound"]["port"] = port;
                config["inbound"]["settings"] = Cache.Instance.LoadTemplate(part);
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

        void CoreRestartHandler()
        {
            var index = setting.GetCurServIndex();
            var b64Config = setting.GetServer(index);

            if (string.IsNullOrEmpty(b64Config))
            {
                return;
            }

            string plainText = Lib.Utils.Base64Decode(b64Config);
            JObject config = JObject.Parse(plainText);

            try
            {
                config = Lib.ImportParser.ParseImport(config);
                cache.UpdateDecodeCache(b64Config, config.ToString(Newtonsoft.Json.Formatting.None));
            }
            catch
            {
                setting.SendLog(I18N("DecodeImportFail"));
                var cacheConfig = cache.GetDecodeCache(b64Config);
                if (string.IsNullOrEmpty(cacheConfig))
                {
                    StopCoreThen(null);
                    return;
                }
                setting.SendLog(I18N("UsingDecodeCache"));
                config = JObject.Parse(cacheConfig);
            }

            OverwriteInboundSettings(config);
            RestartCore(config.ToString());
        }

        void RestartCore(string config)
        {
            StopCoreThen(() =>
            {
                if (IsCoreExist())
                {
                    StartCore(config);
                }
                else
                {
                    MessageBox.Show(I18N("ExeNotFound"));
                }
            });
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
                    FileName = StrConst("Executable"),
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
                v2rayCore = null;
                NotifyStateChange();
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
                StopCoreThen(() => MessageBox.Show(I18N("CantLauchCore")));
                return;
            }

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();

            NotifyStateChange();
        }

        void NotifyStateChange()
        {
            try
            {
                OnCoreStatChange?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }
        #endregion

        #region public method

        public bool IsCoreExist()
        {
            return File.Exists(StrConst("Executable"));
        }

        public void StopCoreThen(Action lamda)
        {
            if (v2rayCore == null)
            {
                lamda?.Invoke();
                return;
            }
            v2rayCore.Exited += (s, a) =>
            {
                v2rayCore = null;
                lamda?.Invoke();
            };
            try
            {
                Lib.Utils.KillProcessAndChildrens(v2rayCore.Id);
            }
            catch { }
        }

        public string GetCoreVersion()
        {
            var ver = string.Empty;
            if (!IsCoreExist())
            {
                return ver;
            }

            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = StrConst("Executable"),
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
