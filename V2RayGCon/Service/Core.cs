using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    public class Core
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
        public event EventHandler OnCoreStatusChanged;
        event EventHandler OnCoreReady;

        Process v2rayCore;
        static object coreLock = new object();

        public Core()
        {
            isRunning = false;
            isCheckCoreReady = false;
            v2rayCore = null;
        }

        #region property
        public bool isRunning
        {
            get;
            private set;
        }

        bool isCheckCoreReady
        {
            get;
            set;
        }
        #endregion

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

            // since 3.46.* v is deleted
            Regex pattern = new Regex(@"(?<version>(\d+\.)+\d+)");
            // Regex pattern = new Regex(@"v(?<version>[\d\.]+)");

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
                var file = Path.Combine(folders[i], StrConst.Executable);
                if (File.Exists(file))
                {
                    return file;
                }
            }

            return string.Empty;
        }

        // blocking
        public void RestartCore(string config, Dictionary<string, string> env = null)
        {
            lock (coreLock)
            {
                if (isRunning)
                {
                    StopCoreWorker();
                }

                if (IsExecutableExist())
                {
                    StartCore(config, env);
                }
                else
                {
                    Task.Factory.StartNew(
                        () => MessageBox.Show(I18N.ExeNotFound));
                }
            }
            Task.Factory.StartNew(() => InvokeEventOnCoreStatusChanged());
        }

        // non-blocking 
        public void RestartCoreThen(
            string config,
            Action next = null,
            Dictionary<string, string> env = null)
        {
            Task.Factory.StartNew(() =>
            {
                RestartCore(config, env);
                InvokeActionIgnoreError(next);
            });
        }

        // blocking
        public void StopCore()
        {
            lock (coreLock)
            {
                StopCoreWorker();
            }
        }

        // non-blocking
        public void StopCoreThen(Action next = null)
        {
            Task.Factory.StartNew(() =>
            {
                StopCore();
                InvokeActionIgnoreError(next);
            });
        }
        #endregion

        #region private method

        void InvokeEventOnCoreReady()
        {
            try
            {
                OnCoreReady?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void InvokeEventOnCoreStatusChanged()
        {
            try
            {
                OnCoreStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch { }
        }

        void StopCoreWorker()
        {
            if (v2rayCore == null)
            {
                isRunning = false;
            }

            if (!isRunning)
            {
                return;
            }

            var kill = true;

            try
            {
                if (AttachConsole((uint)v2rayCore.Id))
                {
                    AutoResetEvent finished = new AutoResetEvent(false);
                    v2rayCore.Exited += (s, a) =>
                    {
                        v2rayCore.Close();
                        finished.Set();
                    };
                    SetConsoleCtrlHandler(null, true);
                    GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0);

                    if (finished.WaitOne(3000))
                    {
                        kill = false;
                    }
                    FreeConsole();
                    SetConsoleCtrlHandler(null, false);
                }
            }
            catch { }


            if (kill)
            {
                try
                {
                    // kill if not able to attach to process
                    KillCore();
                }
                catch { }
            }
            isRunning = false;
        }

        void KillCore()
        {
            AutoResetEvent finished = new AutoResetEvent(false);

            SendLog(I18N.AttachToV2rayCoreProcessFail);

            v2rayCore.Exited += (s, a) =>
            {
                finished.Set();
            };

            Lib.Utils.KillProcessAndChildrens(v2rayCore.Id);
            finished.WaitOne(2000);
        }

        void InvokeActionIgnoreError(Action lambda)
        {
            try
            {
                lambda?.Invoke();
            }
            catch { }
        }

        Process CreateProcess()
        {
            var p = new Process
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
            p.EnableRaisingEvents = true;
            return p;
        }

        void InjectEnv(Process proc, Dictionary<string, string> envs)
        {
            if (envs == null || envs.Count <= 0)
            {
                return;
            }

            var procEnv = proc.StartInfo.EnvironmentVariables;
            foreach (var env in envs)
            {
                procEnv[env.Key] = env.Value;
            }
        }

        void OnCoreExisted(object sender, EventArgs args)
        {
            SendLog(I18N.CoreExit);
            ReleaseEvents(v2rayCore);

            var err = v2rayCore.ExitCode;
            if (err != 0)
            {
                v2rayCore.Close();
                Task.Factory.StartNew(
                    () => MessageBox.Show(I18N.V2rayCoreExitAbnormally));
            }

            // SendLog("Exit code: " + err);
            isRunning = false;
            Task.Factory.StartNew(() => InvokeEventOnCoreStatusChanged());
        }

        void BindEvents(Process proc)
        {
            proc.Exited += OnCoreExisted;
            proc.ErrorDataReceived += SendLogHandler;
            proc.OutputDataReceived += SendLogHandler;
        }

        void ReleaseEvents(Process proc)
        {
            proc.Exited -= OnCoreExisted;
            proc.ErrorDataReceived -= SendLogHandler;
            proc.OutputDataReceived -= SendLogHandler;
        }

        void StartCore(string config,
            Dictionary<string, string> envs = null)
        {
            v2rayCore = CreateProcess();
            InjectEnv(v2rayCore, envs);
            BindEvents(v2rayCore);

            AutoResetEvent ready = new AutoResetEvent(false);
            EventHandler onCoreReady = (s, a) =>
            {
                try { ready.Set(); } catch { }
            };
            isCheckCoreReady = true;
            OnCoreReady += onCoreReady;

            isRunning = true;
            v2rayCore.Start();

            // Add to JOB object require win8+.
            Lib.Sys.ChildProcessTracker.AddProcess(v2rayCore);

            v2rayCore.StandardInput.WriteLine(config);
            v2rayCore.StandardInput.Close();
            v2rayCore.BeginErrorReadLine();
            v2rayCore.BeginOutputReadLine();

            // Assume core ready after 2.5 seconds, in case log set to none.
            ready.WaitOne(2500);
            OnCoreReady -= onCoreReady;
            isCheckCoreReady = false;
        }

        void SendLogHandler(object sender, DataReceivedEventArgs args)
        {
            var msg = args.Data;

            if (msg == null)
            {
                return;
            }

            if (isCheckCoreReady
                && msg.Contains("[Warning]")
                && msg.Contains("started")
                && msg.Contains("ore:")
                && msg.Contains("V2Ray"))
            {
                InvokeEventOnCoreReady();
            }

            SendLog(msg);
        }

        void SendLog(string log)
        {
            var arg = new Model.Data.StrEvent(log);
            try
            {
                OnLog?.Invoke(this, arg);
            }
            catch { }
        }

        #endregion
    }
}
