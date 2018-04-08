
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


namespace V2RayGCon.Service
{
    class Notifier
    {
        NotifyIcon ni;
        Func<string, string> I18N, resData;

        Views.FormLog formLog = null;
        Views.FormMain formMain = null;
        Views.FormConfiger formConfiger = null;
        Views.FormQRCode formQRCode = null;

        Core core;
        Setting setting;
        Download downloader = null;

        public Notifier()
        {
            I18N = Lib.Utils.I18N;
            resData = Lib.Utils.resData;

            setting = Setting.Instance;
            core = Core.Instance;
            CreateNotifyIcon();

#if !DEBUG
            if (setting.GetServeNum() > 0)
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
            Lib.Utils.FindMenuItemByText(ctxm, I18N("ShowConfigWin")).PerformClick();
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

        void ShowFormMain()
        {
            if (formMain == null)
            {
                formMain = new Views.FormMain();
                formMain.FormClosed += (fms, fma) =>
                {
                    formMain = null;
                };

                formMain.OpenEditor += (fms, fma) =>
                {
                    if (formConfiger == null)
                    {
                        formConfiger = new Views.FormConfiger();
                        formConfiger.FormClosed += (fcs, fca) =>
                        {
                            formConfiger = null;
                        };
                    }
                };

                formMain.ShowQRCodeForm += (fms, fma) =>
                {
                    if (formQRCode == null)
                    {
                        formQRCode = new Views.FormQRCode();
                        formQRCode.FormClosed += (fqrcs, fqrca) =>
                        {
                            formQRCode = null;
                        };
                    }
                };

                formMain.ShowLogForm += (fms, fma) =>
                {
                    if (formLog == null)
                    {
                        formLog = new Views.FormLog();
                        formLog.FormClosed += (fls, fla) =>
                        {
                            formLog = null;
                        };
                    }
                };
            }
        }

        void CreateNotifyIcon()
        {
            ni = new NotifyIcon();
            ni.Text = I18N("Description");
            ni.Icon = Properties.Resources.icon_light;
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

        void DownloadCore(bool win64)
        {
            if (downloader == null)
            {
                downloader = new Download();

                if (win64)
                {
                    downloader.SetPackageName(resData("PkgWin64"));
                }

                downloader.OnDownloadCompleted += (dls, dla) =>
                {
                    var isCoreRunning = core.IsRunning();

                    if (isCoreRunning)
                    {
                        core.StopCore();
                    }

                    try
                    {
                        string fileName = resData("PkgWin32");
                        if (win64)
                        {
                            fileName = resData("PkgWin64");
                        }
                        Lib.Utils.ExtractZipFile(fileName);
                        System.IO.File.Delete(fileName);
                        // ni.BalloonTipText = I18N("DLComplete");
                        MessageBox.Show(I18N("DLComplete"));
                    }
                    catch
                    {
                        // ni.BalloonTipText = I18N("DLFail");
                        MessageBox.Show(I18N("DLFail"));
                    }
                    

                    if (isCoreRunning)
                    {
                        // because config file has been replaced
                        setting.ActivateServer(setting.GetSelectedServerIndex());
                    }

                    downloader = null;
                };

                downloader.GetV2RayWindowsPackage();
            }
            else
            {
                MessageBox.Show(I18N("Downloading"));
            }
        }


        ContextMenu CreateMenu()
        {
            return new ContextMenu(new MenuItem[] {

                new MenuItem(I18N("ShowMainWin"),(s,a)=>ShowFormMain()),

                new MenuItem(I18N("ShowLogWin"),(s,a)=>{
                    if(formLog == null)
                    {
                        formLog = new Views.FormLog();
                        formLog.FormClosed += (fms, fma) =>
                        {
                            formLog = null;
                        };
                    }
                }),

                new MenuItem(I18N("ScanQRCode"),(s,a)=>{
                    void success(string link)
                    {
                        if(!setting.ImportLinks(link))
                        {
                            MessageBox.Show(I18N("NotSupportLinkType"));
                        }

                     
                    }

                    void fail()
                    {
                        MessageBox.Show(I18N("NoQRCode"));
                    }

                    Lib.QRCode.QRCode.ScanQRCode(success,fail);

                }),

                new MenuItem(I18N("ImportLink"),(s,a)=>{
                    string links = Lib.Utils.GetClipboardText();
                    Lib.Utils.ShowMsgboxSuccFail(
                        setting.ImportLinks(links),
                        I18N("ImportLinkSuccess"),
                        I18N("ImportLinkFail"));

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

                new MenuItem(I18N("Exit"),(sender,args)=>{
                    if(Lib.Utils.Confirm(I18N("Confirm"),I18N("ConfirmExitApp"))){
                        ni.Visible = false;
                        core.StopCore();
                        Application.Exit();
                    }
                })
            });
        }
    }
}
