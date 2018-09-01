using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.BaseClass
{
    public class CoreServer
    {
        #region support ctrl+c
        // https://stackoverflow.com/questions/283128/how-do-i-send-ctrlc-to-a-process-in-c
        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);
        #endregion

        public event EventHandler<Model.Data.StrEvent> OnLog;

        Process v2rayCore;
        bool _isRunning;

        public CoreServer()
        {
            _isRunning = false;
            v2rayCore = null;
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
            get => _isRunning;
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

        public void StopCoreThen(Action lambda)
        {
            if (!_isRunning || v2rayCore == null)
            {
                lambda?.Invoke();
                return;
            }

            try
            {
                if (AttachConsole((uint)v2rayCore.Id))
                {
                    v2rayCore.Exited += (s, a) =>
                    {
                        FreeConsole();
                        SetConsoleCtrlHandler(null, false);
                        v2rayCore.Close();
                        lambda?.Invoke();
                    };

                    SetConsoleCtrlHandler(null, true);
                    GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0);
                    return;
                }
            }
            catch { }

            SendLog(I18N("AttachToV2rayCoreProcessFail"));

            // kill if not able to attach to process
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
            if (_isRunning)
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

                var err = v2rayCore.ExitCode;
                if (err != 0)
                {
                    v2rayCore.Close();
                    Task.Factory.StartNew(() =>
                    {
                        MessageBox.Show(I18N("V2rayCoreExitAbnormally"));
                    });
                }

                // SendLog("Exit code: " + err);
                _isRunning = false;
                OnStateChanged?.Invoke();
            };

            v2rayCore.ErrorDataReceived += (s, e) => SendLog(e.Data);
            v2rayCore.OutputDataReceived += (s, e) => SendLog(e.Data);

            v2rayCore.Start();
            // Add to JOB object support win8+ 
            Lib.ChildProcessTracker.AddProcess(v2rayCore);

            v2rayCore.StandardInput.WriteLine(config);
            v2rayCore.StandardInput.Close();

            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();

            _isRunning = true;
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
