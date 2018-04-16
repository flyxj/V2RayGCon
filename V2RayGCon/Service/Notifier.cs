
// #define DBG_FORM_CONFIG

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Service
{
    class Notifier
    {
        NotifyIcon ni;

        Views.FormLog formLog = null;
        Views.FormMain formMain = null;
        Views.FormConfiger formConfiger = null;
        Views.FormQRCode formQRCode = null;

        Core core;
        Setting setting;
        Download downloader = null;

        public Notifier()
        {
            setting = Setting.Instance;
            core = Core.Instance;
            CreateNotifyIcon();

            core.OnCoreStatChange += (s, a) => UpdateNotifyText();

#if !DEBUG
            if (setting.GetServerCount() > 0)
            {
                setting.ActivateServer(setting.GetSelectedServerIndex());
            }
            else
            {
                ShowFormMain();
            }
#endif

#if DEBUG
            InsertDebugMenu();
            This_function_do_some_tedious_stuff();
#endif
        }

        #region DEBUG code TL;DR
#if DEBUG

        void This_function_do_some_tedious_stuff()
        {
            ni.DoubleClick += (s, a) =>
            {
                Debug.WriteLine("Test JObject:");
                Debug.WriteLine("Done!");
            };

            ContextMenu ctxm = ni.ContextMenu;
#if !DBG_FORM_CONFIG
            Lib.Utils.FindMenuItemByText(ctxm, I18N("ShowMainWin")).PerformClick();
            Lib.Utils.FindMenuItemByText(ctxm, I18N("ShowLogWin")).PerformClick();
            // Lib.Utils.FindMenuItemByText(ctxm, "Add dummy server1").PerformClick();
#else
            setting.curEditingIndex = 0;
            ShowFormConfigEditor();
#endif
        }

        void InsertDebugMenu()
        {
            var mis = ni.ContextMenu.MenuItems;
            mis.Add(0, new MenuItem("-"));
            mis.Add(0, CreateDebugMenu());
        }

        MenuItem CreateDebugMenu()
        {
            return new MenuItem("Debug", new MenuItem[]{
                new MenuItem("QRCode",new MenuItem[]{
                    new MenuItem("test ScanRect", (s,a)=>{
                        void success(string link)
                        {
                            Debug.WriteLine("Got Link: {0}",link);
                        }

                        void fail()
                        {
                            Debug.WriteLine("no link found!");
                        }

                        Lib.QRCode.QRCode.ScanQRCode(success,fail);

                    }),
                }),

                new MenuItem("Show download fail!",(s,a)=>{
                    MessageBox.Show( I18N("DLFail"));
                }),

                new MenuItem("Add dummy server1",(s,a)=>{
                    setting.ImportLinks(resData("DummyServ1"));
                    Debug.WriteLine("Done!");
                }),

                new MenuItem("Add dummy server2",(s,a)=>{
                    setting.ImportLinks(resData("DummyServ2"));
                    Debug.WriteLine("Done!");
                }),
                new MenuItem("Show all servers",(s,a)=>{
                    Debug.WriteLine("servers: ",Properties.Settings.Default.Servers);
                    Debug.WriteLine("Done!");
                }),
                new MenuItem("Test [en/de]code vmess",(s,a)=>{
                    string encode_vmess = resData("DummyServ1");

                    string plain_vmess = Lib.Utils.Base64Decode(
                        Lib.Utils.LinkBody(
                            encode_vmess,
                            Model.Enum.LinkTypes.vmess));

                    Model.Vmess vmess = JsonConvert.DeserializeObject<Model.Vmess>(plain_vmess);
                    string duplicated_vmess = Lib.Utils.Vmess2VmessLink(vmess);
;
                    Debug.WriteLine("org: " + encode_vmess);
                    Debug.WriteLine("new: " + duplicated_vmess);
                    Debug.WriteLine("Done!");
                }),
            });
        }

#endif
        #endregion

        void ShowFormConfigEditor()
        {
            if (formConfiger != null)
            {
                return;
            }

            formConfiger = new Views.FormConfiger();
            formConfiger.FormClosed += (s, a) => formConfiger = null;
        }

        void ShowFormMain()
        {
            if (formMain != null)
            {
                return;
            }

            formMain = new Views.FormMain();
            formMain.FormClosed += (s, a) => formMain = null;
            formMain.ShowFormConfiger += (s, a) => ShowFormConfigEditor();
            formMain.ShowFormQRCode += (s, a) =>
            {
                if (formQRCode != null)
                {
                    return;
                }

                formQRCode = new Views.FormQRCode();
                formQRCode.FormClosed += (fqrcs, fqrca) => formQRCode = null;

            };

            formMain.ShowFormLog += (s, a) =>
            {
                if (formLog != null)
                {
                    return;
                }

                formLog = new Views.FormLog();
                formLog.FormClosed += (fls, fla) => formLog = null;
            };

        }

        void UpdateNotifyText()
        {

            var proxy = string.Format("{0}://{1}",
                setting.proxyType,
                setting.proxyAddr);

            ni.Text = core.IsRunning() ? proxy : I18N("Description");
        }

        void CreateNotifyIcon()
        {
            ni = new NotifyIcon();
            ni.Text = I18N("Description");
            ni.Icon = Properties.Resources.icon_dark;
            ni.BalloonTipTitle = I18N("AppName");
            ni.ContextMenu = CreateMenu();
            ni.Visible = true;

#if !DEBUG
            ni.MouseClick += (s, a) =>
            {
                if (a.Button == MouseButtons.Left)
                {
                    ShowFormMain();
                }
            };
#endif

        }

        void DownloadCore(bool win64 = false)
        {
            if (downloader != null)
            {
                MessageBox.Show(I18N("Downloading"));
                return;
            }

            string coreName = win64 ? resData("PkgWin64") : resData("PkgWin32");

            downloader = new Download();
            downloader.SetPackageName(coreName);

            downloader.OnDownloadCompleted += (dls, dla) =>
            {
                // do not have to stop core any more
                string msg = I18N("DLComplete");

                try
                {
                    Lib.Utils.ZipFileDecompress(coreName);
                    System.IO.File.Delete(coreName);
                }
                catch
                {
                    msg = I18N("DLFail");
                }

                downloader = null;
                MessageBox.Show(msg);
            };

            downloader.GetV2RayCore();
        }

        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {

                new MenuItem(I18N("ShowMainWin"),(s,a)=>ShowFormMain()),

                new MenuItem(I18N("ShowLogWin"),(s,a)=>{

                    if(formLog != null){
                        return;
                    }

                    formLog = new Views.FormLog();
                    formLog.FormClosed += (fms, fma) => formLog = null;
                }),

                new MenuItem(I18N("ScanQRCode"),(s,a)=>{
                    Lib.QRCode.QRCode.ScanQRCode(

                        // success
                        (link)=>{
                            if (!setting.ImportLinks(link))
                            {
                                MessageBox.Show(I18N("NotSupportLinkType"));
                            }
                        },

                        // fail
                        ()=>MessageBox.Show(I18N("NoQRCode"))
                        );
                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    Lib.Utils.ShowMsgboxSuccFail(
                        setting.ImportLinks(links),
                        I18N("ImportLinkSuccess"),
                        I18N("ImportLinkFail"));

                }),

                new MenuItem("-"),

                new MenuItem(I18N("CopyPacUrl"),(s,a)=>{
                    if (Lib.Utils.CopyToClipboard(setting.GetPacUrl()))
                    {
                        MessageBox.Show(
                            I18N("WarnIENotSupportPac"),
                            I18N("CopySuccess"));
                    } else{
                        MessageBox.Show(I18N("CopyFail"));
                    }
                }),

                new MenuItem(I18N("DLv2rayCore"),new MenuItem[]{
                    new MenuItem(I18N("Win32"),(s,a)=>{
                        DownloadCore(false);
                    }),
                    new MenuItem(I18N("Win64"),(s,a)=>{
                        DownloadCore(true);
                    }),
                }),

                new MenuItem("-"),

                new MenuItem(I18N("About"),(s,a)=>MessageBox.Show(
                    Properties.Resources.ProjectLink
                    )),

                new MenuItem(I18N("Exit"),(s,a)=>{
                    if(Lib.Utils.Confirm(I18N("ConfirmExitApp"))){
                        ni.Visible = false;
                        core.StopCore();
                        Application.Exit();
                    }
                })
            });
        }
    }
}
