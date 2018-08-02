using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller
{
    class Tester
    {
        public event EventHandler<Model.Data.StrEvent> OnLog;

        Process v2rayCore;
        Service.Setting setting;

        public Tester()
        {
            setting = Service.Setting.Instance;
        }

        #region public method

        public void RestartCore(int index)
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
                setting.SendLog(I18N("DecodeImportFail"));
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
                v2rayCore = null;
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
        bool IsExecutableExist()
        {
            return File.Exists(StrConst("Executable"));
        }

        void RestartCore(string config)
        {
            StopCoreThen(() =>
            {
                if (IsExecutableExist())
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
                return;
            }

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
                SendLog(I18N("CoreExit"));
                v2rayCore = null;
            };
            v2rayCore.ErrorDataReceived += (s, e) => SendLog(e.Data);
            v2rayCore.OutputDataReceived += (s, e) => SendLog(e.Data);

            try
            {
                v2rayCore.Start();
                v2rayCore.StandardInput.WriteLine(config);
                v2rayCore.StandardInput.Close();

                // Add to JOB object support win8+ only
                Lib.ChildProcessTracker.AddProcess(v2rayCore);
            }
            catch
            {
                StopCoreThen(() => MessageBox.Show(I18N("CantLauchCore")));
                return;
            }

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();
        }

        void SendLog(string log)
        {
            var arg = new Model.Data.StrEvent(log);
            OnLog?.Invoke(this, arg);
        }

        #endregion

    }
}
