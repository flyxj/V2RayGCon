using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class MenuItems : FormMainComponentController
    {
        Service.Servers servers;

        public MenuItems(
            ToolStripMenuItem simVmessServer,
            ToolStripMenuItem importLinkFromClipboard,
            ToolStripMenuItem exportAllServer,
            ToolStripMenuItem importFromFile,
            ToolStripMenuItem checkUpdate,
            ToolStripMenuItem toolMenuItemAbout,
            ToolStripMenuItem toolMenuItemHelp,
            ToolStripMenuItem configEditor,
            ToolStripMenuItem QRCode,
            ToolStripMenuItem log,
            ToolStripMenuItem options,
            ToolStripMenuItem downloadV2rayCore,
            ToolStripMenuItem removeV2rayCore)
        {
            servers = Service.Servers.Instance;

            downloadV2rayCore.Click += (s, a) => Views.FormDownloadCore.GetForm();

            removeV2rayCore.Click += (s, a) => RemoveV2RayCore();

            simVmessServer.Click +=
                (s, a) => Views.FormSimAddVmessClient.GetForm();

            importLinkFromClipboard.Click += (s, a) =>
            {
                string links = Lib.Utils.GetClipboardText();
                servers.ImportLinks(links);
            };

            exportAllServer.Click += (s, a) => ExportAllServersToTextFile();

            importFromFile.Click += (s, a) => ImportServersFromTextFile();

            checkUpdate.Click += (s, a) => CheckVGCUpdate();

            toolMenuItemAbout.Click += (s, a) =>
                Lib.UI.VisitUrl(I18N("VistPorjectPage"), Properties.Resources.ProjectLink);

            toolMenuItemHelp.Click += (s, a) =>
                Lib.UI.VisitUrl(I18N("VistWikiPage"), Properties.Resources.WikiLink);

            configEditor.Click += (s, a) => new Views.FormConfiger();

            QRCode.Click += (s, a) => Views.FormQRCode.GetForm();

            log.Click += (s, a) => Views.FormLog.GetForm();

            options.Click += (s, a) => Views.FormOption.GetForm();
        }


        #region public method
        public void ImportServersFromTextFile()
        {
            string v2rayLinks = Lib.UI.ShowReadFileDialog(StrConst("ExtText"), out string filename);

            if (v2rayLinks == null)
            {
                return;
            }

            servers.ImportLinks(v2rayLinks);
        }

        public void ExportAllServersToTextFile()
        {
            if (this.servers.IsEmpty())
            {
                MessageBox.Show(I18N("ServerListIsEmpty"));
                return;
            }

            var serverList = servers.GetServerList();
            string s = string.Empty;

            foreach (var server in serverList)
            {
                s += "v2ray://" + Lib.Utils.Base64Encode(server.config) + "\r\n\r\n";
            }

            switch (Lib.UI.ShowSaveFileDialog(
                StrConst("ExtText"),
                s,
                out string filename))
            {
                case Model.Data.Enum.SaveFileErrorCode.Success:
                    MessageBox.Show(I18N("Done"));
                    break;
                case Model.Data.Enum.SaveFileErrorCode.Fail:
                    MessageBox.Show(I18N("WriteFileFail"));
                    break;
                case Model.Data.Enum.SaveFileErrorCode.Cancel:
                    // do nothing
                    break;
            }
        }

        public void CheckVGCUpdate()
        {
            Task.Factory.StartNew(CheckUpdate);
        }

        public override bool RefreshUI() { return false; }
        public override void Cleanup()
        {
        }
        #endregion

        #region private method

        private void RemoveV2RayCore()
        {
            if (!Lib.UI.Confirm(I18N("ConfirmRemoveV2RayCore")))
            {
                return;
            }

            if (!Directory.Exists(Lib.Utils.GetAppDataFolder()))
            {
                MessageBox.Show(I18N("Done"));
                return;
            }

            servers.StopAllServersThen(() =>
            {
                try
                {
                    Lib.Utils.DeleteAppDataFolder();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show(I18N("FileInUse"));
                    return;
                }
                MessageBox.Show(I18N("Done"));
            });
        }

        private void CheckUpdate()
        {
            var version = Lib.Utils.GetLatestVGCVersion();
            if (string.IsNullOrEmpty(version))
            {
                MessageBox.Show(I18N("GetVGCVerFail"));
                return;
            }

            var verNew = new Version(version);
            var verCur = new Version(Properties.Resources.Version);

            var result = verCur.CompareTo(verNew);
            if (result >= 0)
            {
                MessageBox.Show(I18N("NoNewVGC"));
                return;
            }

            var confirmTpl = I18N("ConfirmDownloadNewVGC");
            var msg = string.Format(confirmTpl, version);
            if (Lib.UI.Confirm(msg))
            {
                var tpl = StrConst("TplUrlVGCRelease");
                var url = string.Format(tpl, version);
                System.Diagnostics.Process.Start(url);
            }
        }
        #endregion
    }
}
