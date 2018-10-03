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
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.flyServerListContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSelectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectInverse = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCollapseSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExpanSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRestartSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStopSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonModifySelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortSelectedBySummary = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSortSelectedBySpeedTestResult = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonFormOption = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxMarkFilter = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxFlySearcher = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.operationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemSimAddVmessServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemImportLinkFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolMenuItemExportAllServerToFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemImportFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectInvertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAutorunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRunningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTimeoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoSpeedTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMenuItemServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemModifySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortBySpeedTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortBySummary = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemModifySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemCollapsePanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExpansePanel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsV2rayLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsVmessLink = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyAsSubscription = new System.Windows.Forms.ToolStripMenuItem();
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer2
            // 
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.flyServerListContainer);
            resources.ApplyResources(this.toolStripContainer2.ContentPanel, "toolStripContainer2.ContentPanel");
            resources.ApplyResources(this.toolStripContainer2, "toolStripContainer2");
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // flyServerListContainer
            // 
            this.flyServerListContainer.AllowDrop = true;
            resources.ApplyResources(this.flyServerListContainer, "flyServerListContainer");
            this.flyServerListContainer.BackColor = System.Drawing.Color.White;
            this.flyServerListContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flyServerListContainer.Name = "flyServerListContainer";
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSelectAll,
            this.toolStripButtonSelectInverse,
            this.toolStripButtonSelectNone,
            this.toolStripSeparator2,
            this.toolStripButtonCollapseSelected,
            this.toolStripButtonExpanSelected,
            this.toolStripSeparator6,
            this.toolStripButtonRestartSelected,
            this.toolStripButtonStopSelected,
            this.toolStripSeparator7,
            this.toolStripButtonModifySelected,
            this.toolStripButtonSortSelectedBySummary,
            this.toolStripButtonSortSelectedBySpeedTestResult,
            this.toolStripSeparator9,
            this.toolStripButtonFormOption,
            this.toolStripButtonHelp});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButtonSelectAll
            // 
            this.toolStripButtonSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSelectAll, "toolStripButtonSelectAll");
            this.toolStripButtonSelectAll.Name = "toolStripButtonSelectAll";
            // 
            // toolStripButtonSelectInverse
            // 
            this.toolStripButtonSelectInverse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSelectInverse, "toolStripButtonSelectInverse");
            this.toolStripButtonSelectInverse.Name = "toolStripButtonSelectInverse";
            // 
            // toolStripButtonSelectNone
            // 
            this.toolStripButtonSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSelectNone, "toolStripButtonSelectNone");
            this.toolStripButtonSelectNone.Name = "toolStripButtonSelectNone";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // toolStripButtonCollapseSelected
            // 
            this.toolStripButtonCollapseSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonCollapseSelected, "toolStripButtonCollapseSelected");
            this.toolStripButtonCollapseSelected.Name = "toolStripButtonCollapseSelected";
            // 
            // toolStripButtonExpanSelected
            // 
            this.toolStripButtonExpanSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonExpanSelected, "toolStripButtonExpanSelected");
            this.toolStripButtonExpanSelected.Name = "toolStripButtonExpanSelected";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // toolStripButtonRestartSelected
            // 
            this.toolStripButtonRestartSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonRestartSelected, "toolStripButtonRestartSelected");
            this.toolStripButtonRestartSelected.Name = "toolStripButtonRestartSelected";
            // 
            // toolStripButtonStopSelected
            // 
            this.toolStripButtonStopSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonStopSelected, "toolStripButtonStopSelected");
            this.toolStripButtonStopSelected.Name = "toolStripButtonStopSelected";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // toolStripButtonModifySelected
            // 
            this.toolStripButtonModifySelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonModifySelected, "toolStripButtonModifySelected");
            this.toolStripButtonModifySelected.Name = "toolStripButtonModifySelected";
            // 
            // toolStripButtonSortSelectedBySummary
            // 
            this.toolStripButtonSortSelectedBySummary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSortSelectedBySummary, "toolStripButtonSortSelectedBySummary");
            this.toolStripButtonSortSelectedBySummary.Name = "toolStripButtonSortSelectedBySummary";
            // 
            // toolStripButtonSortSelectedBySpeedTestResult
            // 
            this.toolStripButtonSortSelectedBySpeedTestResult.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonSortSelectedBySpeedTestResult, "toolStripButtonSortSelectedBySpeedTestResult");
            this.toolStripButtonSortSelectedBySpeedTestResult.Name = "toolStripButtonSortSelectedBySpeedTestResult";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // toolStripButtonFormOption
            // 
            this.toolStripButtonFormOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonFormOption, "toolStripButtonFormOption");
            this.toolStripButtonFormOption.Name = "toolStripButtonFormOption";
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButtonHelp, "toolStripButtonHelp");
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripComboBoxMarkFilter,
            this.toolStripLabel2,
            this.toolStripTextBoxFlySearcher});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // toolStripComboBoxMarkFilter
            // 
            this.toolStripComboBoxMarkFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxMarkFilter.Name = "toolStripComboBoxMarkFilter";
            resources.ApplyResources(this.toolStripComboBoxMarkFilter, "toolStripComboBoxMarkFilter");
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            // 
            // toolStripTextBoxFlySearcher
            // 
            this.toolStripTextBoxFlySearcher.Name = "toolStripTextBoxFlySearcher";
            resources.ApplyResources(this.toolStripTextBoxFlySearcher, "toolStripTextBoxFlySearcher");
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operationToolStripMenuItem,
            this.selectToolStripMenuItem,
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
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.selectInvertToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectAutorunToolStripMenuItem,
            this.selectRunningToolStripMenuItem,
            this.selectTimeoutToolStripMenuItem,
            this.selectNoSpeedTestToolStripMenuItem});
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            resources.ApplyResources(this.selectToolStripMenuItem, "selectToolStripMenuItem");
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            resources.ApplyResources(this.selectNoneToolStripMenuItem, "selectNoneToolStripMenuItem");
            // 
            // selectInvertToolStripMenuItem
            // 
            this.selectInvertToolStripMenuItem.Name = "selectInvertToolStripMenuItem";
            resources.ApplyResources(this.selectInvertToolStripMenuItem, "selectInvertToolStripMenuItem");
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // selectAutorunToolStripMenuItem
            // 
            this.selectAutorunToolStripMenuItem.Name = "selectAutorunToolStripMenuItem";
            resources.ApplyResources(this.selectAutorunToolStripMenuItem, "selectAutorunToolStripMenuItem");
            // 
            // selectRunningToolStripMenuItem
            // 
            this.selectRunningToolStripMenuItem.Name = "selectRunningToolStripMenuItem";
            resources.ApplyResources(this.selectRunningToolStripMenuItem, "selectRunningToolStripMenuItem");
            // 
            // selectTimeoutToolStripMenuItem
            // 
            this.selectTimeoutToolStripMenuItem.Name = "selectTimeoutToolStripMenuItem";
            resources.ApplyResources(this.selectTimeoutToolStripMenuItem, "selectTimeoutToolStripMenuItem");
            // 
            // selectNoSpeedTestToolStripMenuItem
            // 
            this.selectNoSpeedTestToolStripMenuItem.Name = "selectNoSpeedTestToolStripMenuItem";
            resources.ApplyResources(this.selectNoSpeedTestToolStripMenuItem, "selectNoSpeedTestToolStripMenuItem");
            // 
            // toolMenuItemServer
            // 
            this.toolMenuItemServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemModifySelected,
            this.toolStripMenuItem1,
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
            // toolStripMenuItemModifySelected
            // 
            this.toolStripMenuItemModifySelected.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMoveToTop,
            this.toolStripMenuItemMoveToBottom,
            this.toolStripMenuItemSortBySpeedTest,
            this.toolStripMenuItemSortBySummary,
            this.toolStripSeparator3,
            this.toolStripMenuItemModifySettings,
            this.toolStripSeparator4,
            this.toolStripMenuItemCollapsePanel,
            this.toolStripMenuItemExpansePanel});
            this.toolStripMenuItemModifySelected.Name = "toolStripMenuItemModifySelected";
            resources.ApplyResources(this.toolStripMenuItemModifySelected, "toolStripMenuItemModifySelected");
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
            // toolStripMenuItemSortBySpeedTest
            // 
            this.toolStripMenuItemSortBySpeedTest.Name = "toolStripMenuItemSortBySpeedTest";
            resources.ApplyResources(this.toolStripMenuItemSortBySpeedTest, "toolStripMenuItemSortBySpeedTest");
            // 
            // toolStripMenuItemSortBySummary
            // 
            this.toolStripMenuItemSortBySummary.Name = "toolStripMenuItemSortBySummary";
            resources.ApplyResources(this.toolStripMenuItemSortBySummary, "toolStripMenuItemSortBySummary");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItemModifySettings
            // 
            this.toolStripMenuItemModifySettings.Name = "toolStripMenuItemModifySettings";
            resources.ApplyResources(this.toolStripMenuItemModifySettings, "toolStripMenuItemModifySettings");
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTotal});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabelTotal
            // 
            this.toolStripStatusLabelTotal.Name = "toolStripStatusLabelTotal";
            resources.ApplyResources(this.toolStripStatusLabelTotal, "toolStripStatusLabelTotal");
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripContainer2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private ToolStripMenuItem toolStripMenuItemModifySelected;
        private ToolStripMenuItem toolStripMenuItemModifySettings;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemMoveToTop;
        private ToolStripMenuItem toolStripMenuItemMoveToBottom;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem toolStripMenuItemCollapsePanel;
        private ToolStripMenuItem toolStripMenuItemExpansePanel;
        private ToolStripMenuItem toolStripMenuItemSortBySpeedTest;
        private ToolStripMenuItem selectToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem selectNoneToolStripMenuItem;
        private ToolStripMenuItem selectInvertToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem selectAutorunToolStripMenuItem;
        private ToolStripMenuItem selectRunningToolStripMenuItem;
        private ToolStripMenuItem selectTimeoutToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemSortBySummary;
        private ToolStripMenuItem selectNoSpeedTestToolStripMenuItem;
        private ToolStrip toolStrip2;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox toolStripComboBoxMarkFilter;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox toolStripTextBoxFlySearcher;
        private ToolStripContainer toolStripContainer2;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonSelectAll;
        private ToolStripButton toolStripButtonSelectInverse;
        private ToolStripButton toolStripButtonSelectNone;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButtonCollapseSelected;
        private ToolStripButton toolStripButtonExpanSelected;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton toolStripButtonRestartSelected;
        private ToolStripButton toolStripButtonStopSelected;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripButton toolStripButtonModifySelected;
        private ToolStripButton toolStripButtonSortSelectedBySummary;
        private ToolStripButton toolStripButtonSortSelectedBySpeedTestResult;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton toolStripButtonFormOption;
        private ToolStripButton toolStripButtonHelp;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelTotal;
    }
}
