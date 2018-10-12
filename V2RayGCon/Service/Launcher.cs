using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;

namespace V2RayGCon.Service
{
    class Launcher : Model.BaseClass.SingletonService<Launcher>
    {
        Service.Setting setting;
        Service.PacServer pacServer;
        Service.Notifier notifier;
        Service.Servers servers;


        Launcher()
        {
            Lib.Utils.SupportProtocolTLS12();

            setting = Setting.Instance;
            pacServer = PacServer.Instance;
            servers = Servers.Instance;
            notifier = Notifier.Instance;

            Application.ApplicationExit += Cleanup;
            Microsoft.Win32.SystemEvents.SessionEnding += (s, a) => Application.Exit();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }
        #region debug
        void This_Function_Is_Used_For_Debugging()
        {
            notifier.InjectDebugMenuItem(new ToolStripMenuItem(
                "Debug",
                null,
                (s, a) =>
                {
                    servers.DbgFastRestartTest(100);
                }));

            // new Views.WinForms.FormConfiger(@"{}");
            // new Views.WinForms.FormConfigTester();
            // Views.WinForms.FormOption.GetForm();
            Views.WinForms.FormMain.GetForm();
            Views.WinForms.FormLog.GetForm();
            // setting.WakeupAutorunServer();
            // Views.WinForms.FormSimAddVmessClient.GetForm();
            // Views.WinForms.FormDownloadCore.GetForm();
        }
        #endregion

        #region public method
        public void run()
        {
            servers.Prepare(setting);

            if (servers.IsEmpty())
            {
                Views.WinForms.FormMain.GetForm();
            }
            else
            {
                servers.WakeupServers();
            }

#if DEBUG
            This_Function_Is_Used_For_Debugging();
#endif
        }

        #endregion

        #region unhandled exception
        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            SaveUnHandledException(e.Exception.ToString());
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            SaveUnHandledException((e.ExceptionObject as Exception).ToString());
        }

        void ShowExceptionDetails()
        {
            System.Diagnostics.Process.Start(GetBugLogFileName());
            MessageBox.Show(I18N.LooksLikeABug
                + System.Environment.NewLine
                + GetBugLogFileName());
        }

        void SaveUnHandledException(string msg)
        {
            var log = msg;
            try
            {
                log += Environment.NewLine
                    + Environment.NewLine
                    + setting.logCache;
            }
            catch { }
            SaveBugLog(log);
            ShowExceptionDetails();
            Application.Exit();
        }

        string GetBugLogFileName()
        {
            var appData = Lib.Utils.GetAppDataFolder();
            return Path.Combine(appData, StrConst.BugFileName);
        }

        void SaveBugLog(string content)
        {
            try
            {
                var bugFileName = GetBugLogFileName();
                Lib.Utils.CreateAppDataFolder();
                File.WriteAllText(bugFileName, content);
            }
            catch { }
        }
        #endregion

        #region private method
        void Cleanup(object sender, EventArgs args)
        {
            notifier.Cleanup();
            servers.Cleanup();
            pacServer.Cleanup();
        }
        #endregion
    }
}
