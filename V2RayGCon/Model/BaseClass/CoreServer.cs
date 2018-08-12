using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.BaseClass
{
    public class CoreServer
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;

        Process v2rayCore;
        Service.Setting setting;

        public CoreServer()
        {
            setting = Service.Setting.Instance;
        }

        #region public method
        public string GetCoreVersion()
        {
            var ver = string.Empty;
            if (!IsExecutableExist())
            {
                return ver;
            }

            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetExecutablePath(),
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

        public bool IsExecutableExist()
        {
            return !string.IsNullOrEmpty(GetExecutablePath());
        }

        public string GetExecutablePath()
        {
            var folders = new string[]{
                Lib.Utils.GetAppDataFolder(), //piror
                Lib.Utils.GetAppDir(),  // fallback
            };

            for (var i = 0; i < folders.Length; i++)
            {
                var file = Path.Combine(folders[i], StrConst("Executable"));
                if (File.Exists(file))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        public bool isRunning
        {
            get => v2rayCore != null;
        }

        public void RestartCore(string config, Action OnStateChanged = null)
        {
            StopCoreThen(() =>
            {
                if (IsExecutableExist())
                {
                    StartCore(config, OnStateChanged);
                }
                else
                {
                    MessageBox.Show(I18N("ExeNotFound"));
                }
            });
        }

        public virtual void RestartCore(int index)
        {
            if (index < 0)
            {
                StopCoreThen(null);
                return;
            }

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
            }
            catch
            {
                SendLog(I18N("DecodeImportFail"));
                StopCoreThen(null);
                return;
            }

            RestartCore(config.ToString());
        }

        public void StopCoreThen(Action lambda)
        {
            if (v2rayCore == null)
            {
                lambda?.Invoke();
                return;
            }

            v2rayCore.Exited += (s, a) =>
            {
                lambda?.Invoke();
            };

            try
            {
                Lib.Utils.KillProcessAndChildrens(v2rayCore.Id);
            }
            catch { }
        }
        #endregion

        #region private method

        void StartCore(string config, Action OnStateChanged = null)
        {
            if (v2rayCore != null)
            {
                return;
            }

            v2rayCore = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = GetExecutablePath(),
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
                SendLog(I18N("CoreExit"));

                // isRunning() need this.v2rayCore to be set to null 
                // before invoking OnStateChanged
                v2rayCore = null;
                OnStateChanged?.Invoke();
            };

            v2rayCore.ErrorDataReceived += (s, e) => SendLog(e.Data);
            v2rayCore.OutputDataReceived += (s, e) => SendLog(e.Data);

            try
            {
                v2rayCore.Start();
                v2rayCore.StandardInput.WriteLine(config);
                v2rayCore.StandardInput.Close();

                // Add to JOB object support win8+ 
                Lib.ChildProcessTracker.AddProcess(v2rayCore);
            }
            catch
            {
                StopCoreThen(() => MessageBox.Show(I18N("CantLauchCore")));
                return;
            }

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();
            OnStateChanged?.Invoke();
        }

        void SendLog(string log)
        {
            var arg = new Model.Data.StrEvent(log);
            OnLog?.Invoke(this, arg);
        }

        #endregion
    }
}
