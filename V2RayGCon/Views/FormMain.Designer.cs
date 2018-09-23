using System.Windows.Forms;

namespace V2RayGCon.Views
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemSimAddVmessServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemImportLinkFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMenuItemExportAllServerToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSelectInvert = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemSelectAutorunServers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsV2rayLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsVmessLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsSubscription = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemModifySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemModifySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemCollapsePanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExpansePanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRestartSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStopSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSpeedTestOnSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPackSelectedServers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMenuItemRefreshSummary = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteServers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteAllServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteSelectedServers = new System.Windows.Forms.ToolStripMenuItem();
            this.systemProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemCurrentSysProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemClearSysProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemConfigEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDownLoadV2rayCore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveV2rayCore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemCheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboxMarkFilter = new System.Windows.Forms.ComboBox();
            this.flyServerListContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationToolStripMenuItem,
            this.toolMenuItemServer,
            this.windowToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.menuStrip1.Name = "menuStrip1";
            // 
            // operationToolStripMenuItem
            // 
            this.operationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemSimAddVmessServer,
            this.toolMenuItemImportLinkFromClipboard,
            this.toolStripSeparator5,
            this.toolMenuItemExportAllServerToFile,
            this.toolMenuItemImportFromFile,
            this.toolStripSeparator8,
            this.exitToolStripMenuItem});
            this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
            resources.ApplyResources(this.operationToolStripMenuItem, "operationToolStripMenuItem");
            // 
            // toolMenuItemSimAddVmessServer
            // 
            this.toolMenuItemSimAddVmessServer.Name = "toolMenuItemSimAddVmessServer";
            resources.ApplyResources(this.toolMenuItemSimAddVmessServer, "toolMenuItemSimAddVmessServer");
            // 
            // toolMenuItemImportLinkFromClipboard
            // 
            this.toolMenuItemImportLinkFromClipboard.Name = "toolMenuItemImportLinkFromClipboard";
            resources.ApplyResources(this.toolMenuItemImportLinkFromClipboard, "toolMenuItemImportLinkFromClipboard");
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // toolMenuItemExportAllServerToFile
            // 
            this.toolMenuItemExportAllServerToFile.Name = "toolMenuItemExportAllServerToFile";
            resources.ApplyResources(this.toolMenuItemExportAllServerToFile, "toolMenuItemExportAllServerToFile");
            // 
            // toolMenuItemImportFromFile
            // 
            this.toolMenuItemImportFromFile.Name = "toolMenuItemImportFromFile";
            resources.ApplyResources(this.toolMenuItemImportFromFile, "toolMenuItemImportFromFile");
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolMenuItemServer
            // 
            this.toolMenuItemServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectAll,
            this.toolStripMenuItemSelectNone,
            this.toolStripMenuItemSelectInvert,
            this.toolMenuItemSelectAutorunServers,
            this.toolStripSeparator2,
            this.toolStripMenuItem1,
            this.toolStripMenuItemModifySelected,
            this.toolStripMenuItemRestartSelected,
            this.toolStripMenuItemStopSelected,
            this.toolStripMenuItemSpeedTestOnSelected,
            this.toolStripMenuItemPackSelectedServers,
            this.toolStripSeparator1,
            this.toolMenuItemRefreshSummary,
            this.toolStripMenuItemDeleteServers,
            this.systemProxyToolStripMenuItem});
            this.toolMenuItemServer.Name = "toolMenuItemServer";
            resources.ApplyResources(this.toolMenuItemServer, "toolMenuItemServer");
            // 
            // toolStripMenuItemSelectAll
            // 
            this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
            resources.ApplyResources(this.toolStripMenuItemSelectAll, "toolStripMenuItemSelectAll");
            // 
            // toolStripMenuItemSelectNone
            // 
            this.toolStripMenuItemSelectNone.Name = "toolStripMenuItemSelectNone";
            resources.ApplyResources(this.toolStripMenuItemSelectNone, "toolStripMenuItemSelectNone");
            // 
            // toolStripMenuItemSelectInvert
            // 
            this.toolStripMenuItemSelectInvert.Name = "toolStripMenuItemSelectInvert";
            resources.ApplyResources(this.toolStripMenuItemSelectInvert, "toolStripMenuItemSelectInvert");
            // 
            // toolMenuItemSelectAutorunServers
            // 
            this.toolMenuItemSelectAutorunServers.Name = "toolMenuItemSelectAutorunServers";
            resources.ApplyResources(this.toolMenuItemSelectAutorunServers, "toolMenuItemSelectAutorunServers");
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopyAsV2rayLink,
            this.toolStripMenuItemCopyAsVmessLink,
            this.toolStripMenuItemCopyAsSubscription});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // toolStripMenuItemCopyAsV2rayLink
            // 
            this.toolStripMenuItemCopyAsV2rayLink.Name = "toolStripMenuItemCopyAsV2rayLink";
            resources.ApplyResources(this.toolStripMenuItemCopyAsV2rayLink, "toolStripMenuItemCopyAsV2rayLink");
            // 
            // toolStripMenuItemCopyAsVmessLink
            // 
            this.toolStripMenuItemCopyAsVmessLink.Name = "toolStripMenuItemCopyAsVmessLink";
            resources.ApplyResources(this.toolStripMenuItemCopyAsVmessLink, "toolStripMenuItemCopyAsVmessLink");
            // 
            // toolStripMenuItemCopyAsSubscription
            // 
            this.toolStripMenuItemCopyAsSubscription.Name = "toolStripMenuItemCopyAsSubscription";
            resources.ApplyResources(this.toolStripMenuItemCopyAsSubscription, "toolStripMenuItemCopyAsSubscription");
            // 
            // toolStripMenuItemModifySelected
            // 
            this.toolStripMenuItemModifySelected.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemModifySettings,
            this.toolStripSeparator3,
            this.toolStripMenuItemMoveToTop,
            this.toolStripMenuItemMoveToBottom,
            this.toolStripSeparator4,
            this.toolStripMenuItemCollapsePanel,
            this.toolStripMenuItemExpansePanel});
            this.toolStripMenuItemModifySelected.Name = "toolStripMenuItemModifySelected";
            resources.ApplyResources(this.toolStripMenuItemModifySelected, "toolStripMenuItemModifySelected");
            // 
            // toolStripMenuItemModifySettings
            // 
            this.toolStripMenuItemModifySettings.Name = "toolStripMenuItemModifySettings";
            resources.ApplyResources(this.toolStripMenuItemModifySettings, "toolStripMenuItemModifySettings");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItemMoveToTop
            // 
            this.toolStripMenuItemMoveToTop.Name = "toolStripMenuItemMoveToTop";
            resources.ApplyResources(this.toolStripMenuItemMoveToTop, "toolStripMenuItemMoveToTop");
            // 
            // toolStripMenuItemMoveToBottom
            // 
            this.toolStripMenuItemMoveToBottom.Name = "toolStripMenuItemMoveToBottom";
            resources.ApplyResources(this.toolStripMenuItemMoveToBottom, "toolStripMenuItemMoveToBottom");
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // toolStripMenuItemCollapsePanel
            // 
            this.toolStripMenuItemCollapsePanel.Name = "toolStripMenuItemCollapsePanel";
            resources.ApplyResources(this.toolStripMenuItemCollapsePanel, "toolStripMenuItemCollapsePanel");
            // 
            // toolStripMenuItemExpansePanel
            // 
            this.toolStripMenuItemExpansePanel.Name = "toolStripMenuItemExpansePanel";
            resources.ApplyResources(this.toolStripMenuItemExpansePanel, "toolStripMenuItemExpansePanel");
            // 
            // toolStripMenuItemRestartSelected
            // 
            this.toolStripMenuItemRestartSelected.Name = "toolStripMenuItemRestartSelected";
            resources.ApplyResources(this.toolStripMenuItemRestartSelected, "toolStripMenuItemRestartSelected");
            // 
            // toolStripMenuItemStopSelected
            // 
            this.toolStripMenuItemStopSelected.Name = "toolStripMenuItemStopSelected";
            resources.ApplyResources(this.toolStripMenuItemStopSelected, "toolStripMenuItemStopSelected");
            // 
            // toolStripMenuItemSpeedTestOnSelected
            // 
            this.toolStripMenuItemSpeedTestOnSelected.Name = "toolStripMenuItemSpeedTestOnSelected";
            resources.ApplyResources(this.toolStripMenuItemSpeedTestOnSelected, "toolStripMenuItemSpeedTestOnSelected");
            // 
            // toolStripMenuItemPackSelectedServers
            // 
            this.toolStripMenuItemPackSelectedServers.Name = "toolStripMenuItemPackSelectedServers";
            resources.ApplyResources(this.toolStripMenuItemPackSelectedServers, "toolStripMenuItemPackSelectedServers");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolMenuItemRefreshSummary
            // 
            this.toolMenuItemRefreshSummary.Name = "toolMenuItemRefreshSummary";
            resources.ApplyResources(this.toolMenuItemRefreshSummary, "toolMenuItemRefreshSummary");
            // 
            // toolStripMenuItemDeleteServers
            // 
            this.toolStripMenuItemDeleteServers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDeleteAllServer,
            this.toolStripMenuItemDeleteSelectedServers});
            this.toolStripMenuItemDeleteServers.Name = "toolStripMenuItemDeleteServers";
            resources.ApplyResources(this.toolStripMenuItemDeleteServers, "toolStripMenuItemDeleteServers");
            // 
            // toolStripMenuItemDeleteAllServer
            // 
            this.toolStripMenuItemDeleteAllServer.Name = "toolStripMenuItemDeleteAllServer";
            resources.ApplyResources(this.toolStripMenuItemDeleteAllServer, "toolStripMenuItemDeleteAllServer");
            // 
            // toolStripMenuItemDeleteSelectedServers
            // 
            this.toolStripMenuItemDeleteSelectedServers.Name = "toolStripMenuItemDeleteSelectedServers";
            resources.ApplyResources(this.toolStripMenuItemDeleteSelectedServers, "toolStripMenuItemDeleteSelectedServers");
            // 
            // systemProxyToolStripMenuItem
            // 
            this.systemProxyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemCurrentSysProxy,
            this.toolMenuItemClearSysProxy});
            this.systemProxyToolStripMenuItem.Name = "systemProxyToolStripMenuItem";
            resources.ApplyResources(this.systemProxyToolStripMenuItem, "systemProxyToolStripMenuItem");
            // 
            // toolMenuItemCurrentSysProxy
            // 
            resources.ApplyResources(this.toolMenuItemCurrentSysProxy, "toolMenuItemCurrentSysProxy");
            this.toolMenuItemCurrentSysProxy.Name = "toolMenuItemCurrentSysProxy";
            // 
            // toolMenuItemClearSysProxy
            // 
            this.toolMenuItemClearSysProxy.Name = "toolMenuItemClearSysProxy";
            resources.ApplyResources(this.toolMenuItemClearSysProxy, "toolMenuItemClearSysProxy");
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemConfigEditor,
            this.toolMenuItemQRCode,
            this.toolMenuItemLog,
            this.toolMenuItemOptions});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
            // 
            // toolMenuItemConfigEditor
            // 
            this.toolMenuItemConfigEditor.Name = "toolMenuItemConfigEditor";
            resources.ApplyResources(this.toolMenuItemConfigEditor, "toolMenuItemConfigEditor");
            // 
            // toolMenuItemQRCode
            // 
            this.toolMenuItemQRCode.Name = "toolMenuItemQRCode";
            resources.ApplyResources(this.toolMenuItemQRCode, "toolMenuItemQRCode");
            // 
            // toolMenuItemLog
            // 
            this.toolMenuItemLog.Name = "toolMenuItemLog";
            resources.ApplyResources(this.toolMenuItemLog, "toolMenuItemLog");
            // 
            // toolMenuItemOptions
            // 
            this.toolMenuItemOptions.Name = "toolMenuItemOptions";
            resources.ApplyResources(this.toolMenuItemOptions, "toolMenuItemOptions");
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDownLoadV2rayCore,
            this.toolStripMenuItemRemoveV2rayCore,
            this.toolMenuItemCheckUpdate,
            this.toolMenuItemAbout,
            this.toolMenuItemHelp});
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            resources.ApplyResources(this.aboutToolStripMenuItem1, "aboutToolStripMenuItem1");
            // 
            // toolStripMenuItemDownLoadV2rayCore
            // 
            this.toolStripMenuItemDownLoadV2rayCore.Name = "toolStripMenuItemDownLoadV2rayCore";
            resources.ApplyResources(this.toolStripMenuItemDownLoadV2rayCore, "toolStripMenuItemDownLoadV2rayCore");
            // 
            // toolStripMenuItemRemoveV2rayCore
            // 
            this.toolStripMenuItemRemoveV2rayCore.Name = "toolStripMenuItemRemoveV2rayCore";
            resources.ApplyResources(this.toolStripMenuItemRemoveV2rayCore, "toolStripMenuItemRemoveV2rayCore");
            // 
            // toolMenuItemCheckUpdate
            // 
            this.toolMenuItemCheckUpdate.Name = "toolMenuItemCheckUpdate";
            resources.ApplyResources(this.toolMenuItemCheckUpdate, "toolMenuItemCheckUpdate");
            // 
            // toolMenuItemAbout
            // 
            this.toolMenuItemAbout.Name = "toolMenuItemAbout";
            resources.ApplyResources(this.toolMenuItemAbout, "toolMenuItemAbout");
            // 
            // toolMenuItemHelp
            // 
            this.toolMenuItemHelp.Name = "toolMenuItemHelp";
            resources.ApplyResources(this.toolMenuItemHelp, "toolMenuItemHelp");
            // 
            // cboxMarkFilter
            // 
            resources.ApplyResources(this.cboxMarkFilter, "cboxMarkFilter");
            this.cboxMarkFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMarkFilter.FormattingEnabled = true;
            this.cboxMarkFilter.Items.AddRange(new object[] {
            resources.GetString("cboxMarkFilter.Items"),
            resources.GetString("cboxMarkFilter.Items1")});
            this.cboxMarkFilter.Name = "cboxMarkFilter";
            this.toolTip1.SetToolTip(this.cboxMarkFilter, resources.GetString("cboxMarkFilter.ToolTip"));
            // 
            // flyServerListContainer
            // 
            this.flyServerListContainer.AllowDrop = true;
            resources.ApplyResources(this.flyServerListContainer, "flyServerListContainer");
            this.flyServerListContainer.BackColor = System.Drawing.Color.White;
            this.flyServerListContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flyServerListContainer.Name = "flyServerListContainer";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxMarkFilter);
            this.Controls.Add(this.flyServerListContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem operationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemImportLinkFromClipboard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemConfigEditor;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemQRCode;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemSimAddVmessServer;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemCheckUpdate;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemExportAllServerToFile;
        private System.Windows.Forms.ToolStripMenuItem toolMenuItemImportFromFile;
        private ToolStripMenuItem toolMenuItemHelp;
        private ToolStripMenuItem toolMenuItemOptions;
        private FlowLayoutPanel flyServerListContainer;
        private ToolStripMenuItem toolMenuItemServer;
        private ToolStripMenuItem toolStripMenuItemStopSelected;
        private ToolStripMenuItem toolStripMenuItemRestartSelected;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem systemProxyToolStripMenuItem;
        private ToolStripMenuItem toolMenuItemCurrentSysProxy;
        private ToolStripMenuItem toolMenuItemClearSysProxy;
        private ToolStripMenuItem toolMenuItemRefreshSummary;
        private ToolStripMenuItem toolMenuItemSelectAutorunServers;
        private ToolStripMenuItem toolStripMenuItemSelectAll;
        private ToolStripMenuItem toolStripMenuItemSelectNone;
        private ToolStripMenuItem toolStripMenuItemSelectInvert;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripMenuItemSpeedTestOnSelected;
        private ToolStripMenuItem toolStripMenuItemDeleteServers;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItemCopyAsV2rayLink;
        private ToolStripMenuItem toolStripMenuItemCopyAsVmessLink;
        private ToolStripMenuItem toolStripMenuItemDeleteAllServer;
        private ToolStripMenuItem toolStripMenuItemDeleteSelectedServers;
        private ToolStripMenuItem toolStripMenuItemDownLoadV2rayCore;
        private ToolStripMenuItem toolStripMenuItemRemoveV2rayCore;
        private ToolStripMenuItem toolStripMenuItemCopyAsSubscription;
        private ToolStripMenuItem toolStripMenuItemPackSelectedServers;
        private ComboBox cboxMarkFilter;
        private Label label1;
        private ToolStripMenuItem toolStripMenuItemModifySelected;
        private ToolStripMenuItem toolStripMenuItemModifySettings;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemMoveToTop;
        private ToolStripMenuItem toolStripMenuItemMoveToBottom;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem toolStripMenuItemCollapsePanel;
        private ToolStripMenuItem toolStripMenuItemExpansePanel;
    }
}
