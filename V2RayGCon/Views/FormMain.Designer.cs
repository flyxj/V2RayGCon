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
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxMarkFilter = new System.Windows.Forms.ToolStripComboBox();
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
            this.SelectNoMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer2
            // 
            resources.ApplyResources(this.toolStripContainer2, "toolStripContainer2");
            // 
            // toolStripContainer2.BottomToolStripPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.BottomToolStripPanel, "toolStripContainer2.BottomToolStripPanel");
            this.toolTip1.SetToolTip(this.toolStripContainer2.BottomToolStripPanel, resources.GetString("toolStripContainer2.BottomToolStripPanel.ToolTip"));
            this.toolStripContainer2.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer2.ContentPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.ContentPanel, "toolStripContainer2.ContentPanel");
            this.toolStripContainer2.ContentPanel.Controls.Add(this.flyServerListContainer);
            this.toolTip1.SetToolTip(this.toolStripContainer2.ContentPanel, resources.GetString("toolStripContainer2.ContentPanel.ToolTip"));
            // 
            // toolStripContainer2.LeftToolStripPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.LeftToolStripPanel, "toolStripContainer2.LeftToolStripPanel");
            this.toolTip1.SetToolTip(this.toolStripContainer2.LeftToolStripPanel, resources.GetString("toolStripContainer2.LeftToolStripPanel.ToolTip"));
            this.toolStripContainer2.LeftToolStripPanelVisible = false;
            this.toolStripContainer2.Name = "toolStripContainer2";
            // 
            // toolStripContainer2.RightToolStripPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.RightToolStripPanel, "toolStripContainer2.RightToolStripPanel");
            this.toolTip1.SetToolTip(this.toolStripContainer2.RightToolStripPanel, resources.GetString("toolStripContainer2.RightToolStripPanel.ToolTip"));
            this.toolStripContainer2.RightToolStripPanelVisible = false;
            this.toolTip1.SetToolTip(this.toolStripContainer2, resources.GetString("toolStripContainer2.ToolTip"));
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            resources.ApplyResources(this.toolStripContainer2.TopToolStripPanel, "toolStripContainer2.TopToolStripPanel");
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolTip1.SetToolTip(this.toolStripContainer2.TopToolStripPanel, resources.GetString("toolStripContainer2.TopToolStripPanel.ToolTip"));
            // 
            // flyServerListContainer
            // 
            resources.ApplyResources(this.flyServerListContainer, "flyServerListContainer");
            this.flyServerListContainer.AllowDrop = true;
            this.flyServerListContainer.BackColor = System.Drawing.Color.White;
            this.flyServerListContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flyServerListContainer.Name = "flyServerListContainer";
            this.toolTip1.SetToolTip(this.flyServerListContainer, resources.GetString("flyServerListContainer.ToolTip"));
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
            this.toolStripSeparator10,
            this.toolStripLabel2,
            this.toolStripComboBoxMarkFilter});
            this.toolStrip1.Name = "toolStrip1";
            this.toolTip1.SetToolTip(this.toolStrip1, resources.GetString("toolStrip1.ToolTip"));
            // 
            // toolStripButtonSelectAll
            // 
            resources.ApplyResources(this.toolStripButtonSelectAll, "toolStripButtonSelectAll");
            this.toolStripButtonSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectAll.Name = "toolStripButtonSelectAll";
            // 
            // toolStripButtonSelectInverse
            // 
            resources.ApplyResources(this.toolStripButtonSelectInverse, "toolStripButtonSelectInverse");
            this.toolStripButtonSelectInverse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectInverse.Name = "toolStripButtonSelectInverse";
            // 
            // toolStripButtonSelectNone
            // 
            resources.ApplyResources(this.toolStripButtonSelectNone, "toolStripButtonSelectNone");
            this.toolStripButtonSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSelectNone.Name = "toolStripButtonSelectNone";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButtonCollapseSelected
            // 
            resources.ApplyResources(this.toolStripButtonCollapseSelected, "toolStripButtonCollapseSelected");
            this.toolStripButtonCollapseSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCollapseSelected.Name = "toolStripButtonCollapseSelected";
            // 
            // toolStripButtonExpanSelected
            // 
            resources.ApplyResources(this.toolStripButtonExpanSelected, "toolStripButtonExpanSelected");
            this.toolStripButtonExpanSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExpanSelected.Name = "toolStripButtonExpanSelected";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // toolStripButtonRestartSelected
            // 
            resources.ApplyResources(this.toolStripButtonRestartSelected, "toolStripButtonRestartSelected");
            this.toolStripButtonRestartSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRestartSelected.Name = "toolStripButtonRestartSelected";
            // 
            // toolStripButtonStopSelected
            // 
            resources.ApplyResources(this.toolStripButtonStopSelected, "toolStripButtonStopSelected");
            this.toolStripButtonStopSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStopSelected.Name = "toolStripButtonStopSelected";
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // toolStripButtonModifySelected
            // 
            resources.ApplyResources(this.toolStripButtonModifySelected, "toolStripButtonModifySelected");
            this.toolStripButtonModifySelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonModifySelected.Name = "toolStripButtonModifySelected";
            // 
            // toolStripButtonSortSelectedBySummary
            // 
            resources.ApplyResources(this.toolStripButtonSortSelectedBySummary, "toolStripButtonSortSelectedBySummary");
            this.toolStripButtonSortSelectedBySummary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortSelectedBySummary.Name = "toolStripButtonSortSelectedBySummary";
            // 
            // toolStripButtonSortSelectedBySpeedTestResult
            // 
            resources.ApplyResources(this.toolStripButtonSortSelectedBySpeedTestResult, "toolStripButtonSortSelectedBySpeedTestResult");
            this.toolStripButtonSortSelectedBySpeedTestResult.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSortSelectedBySpeedTestResult.Name = "toolStripButtonSortSelectedBySpeedTestResult";
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // toolStripButtonFormOption
            // 
            resources.ApplyResources(this.toolStripButtonFormOption, "toolStripButtonFormOption");
            this.toolStripButtonFormOption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFormOption.Name = "toolStripButtonFormOption";
            // 
            // toolStripSeparator10
            // 
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // toolStripLabel2
            // 
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.toolStripLabel2.Name = "toolStripLabel2";
            // 
            // toolStripComboBoxMarkFilter
            // 
            resources.ApplyResources(this.toolStripComboBoxMarkFilter, "toolStripComboBoxMarkFilter");
            this.toolStripComboBoxMarkFilter.Name = "toolStripComboBoxMarkFilter";
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
            this.toolTip1.SetToolTip(this.menuStrip1, resources.GetString("menuStrip1.ToolTip"));
            // 
            // operationToolStripMenuItem
            // 
            resources.ApplyResources(this.operationToolStripMenuItem, "operationToolStripMenuItem");
            this.operationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemSimAddVmessServer,
            this.toolMenuItemImportLinkFromClipboard,
            this.toolStripSeparator5,
            this.toolMenuItemExportAllServerToFile,
            this.toolMenuItemImportFromFile,
            this.toolStripSeparator8,
            this.exitToolStripMenuItem});
            this.operationToolStripMenuItem.Name = "operationToolStripMenuItem";
            // 
            // toolMenuItemSimAddVmessServer
            // 
            resources.ApplyResources(this.toolMenuItemSimAddVmessServer, "toolMenuItemSimAddVmessServer");
            this.toolMenuItemSimAddVmessServer.Name = "toolMenuItemSimAddVmessServer";
            // 
            // toolMenuItemImportLinkFromClipboard
            // 
            resources.ApplyResources(this.toolMenuItemImportLinkFromClipboard, "toolMenuItemImportLinkFromClipboard");
            this.toolMenuItemImportLinkFromClipboard.Name = "toolMenuItemImportLinkFromClipboard";
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // toolMenuItemExportAllServerToFile
            // 
            resources.ApplyResources(this.toolMenuItemExportAllServerToFile, "toolMenuItemExportAllServerToFile");
            this.toolMenuItemExportAllServerToFile.Name = "toolMenuItemExportAllServerToFile";
            // 
            // toolMenuItemImportFromFile
            // 
            resources.ApplyResources(this.toolMenuItemImportFromFile, "toolMenuItemImportFromFile");
            this.toolMenuItemImportFromFile.Name = "toolMenuItemImportFromFile";
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // selectToolStripMenuItem
            // 
            resources.ApplyResources(this.selectToolStripMenuItem, "selectToolStripMenuItem");
            this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.selectInvertToolStripMenuItem,
            this.toolStripMenuItem2,
            this.selectAutorunToolStripMenuItem,
            this.selectRunningToolStripMenuItem,
            this.selectTimeoutToolStripMenuItem,
            this.selectNoSpeedTestToolStripMenuItem,
            this.SelectNoMarkToolStripMenuItem});
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            // 
            // selectAllToolStripMenuItem
            // 
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            // 
            // selectNoneToolStripMenuItem
            // 
            resources.ApplyResources(this.selectNoneToolStripMenuItem, "selectNoneToolStripMenuItem");
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            // 
            // selectInvertToolStripMenuItem
            // 
            resources.ApplyResources(this.selectInvertToolStripMenuItem, "selectInvertToolStripMenuItem");
            this.selectInvertToolStripMenuItem.Name = "selectInvertToolStripMenuItem";
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // selectAutorunToolStripMenuItem
            // 
            resources.ApplyResources(this.selectAutorunToolStripMenuItem, "selectAutorunToolStripMenuItem");
            this.selectAutorunToolStripMenuItem.Name = "selectAutorunToolStripMenuItem";
            // 
            // selectRunningToolStripMenuItem
            // 
            resources.ApplyResources(this.selectRunningToolStripMenuItem, "selectRunningToolStripMenuItem");
            this.selectRunningToolStripMenuItem.Name = "selectRunningToolStripMenuItem";
            // 
            // selectTimeoutToolStripMenuItem
            // 
            resources.ApplyResources(this.selectTimeoutToolStripMenuItem, "selectTimeoutToolStripMenuItem");
            this.selectTimeoutToolStripMenuItem.Name = "selectTimeoutToolStripMenuItem";
            // 
            // selectNoSpeedTestToolStripMenuItem
            // 
            resources.ApplyResources(this.selectNoSpeedTestToolStripMenuItem, "selectNoSpeedTestToolStripMenuItem");
            this.selectNoSpeedTestToolStripMenuItem.Name = "selectNoSpeedTestToolStripMenuItem";
            // 
            // SelectNoMarkToolStripMenuItem
            // 
            resources.ApplyResources(this.SelectNoMarkToolStripMenuItem, "SelectNoMarkToolStripMenuItem");
            this.SelectNoMarkToolStripMenuItem.Name = "SelectNoMarkToolStripMenuItem";
            // 
            // toolMenuItemServer
            // 
            resources.ApplyResources(this.toolMenuItemServer, "toolMenuItemServer");
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
            // 
            // toolStripMenuItemModifySelected
            // 
            resources.ApplyResources(this.toolStripMenuItemModifySelected, "toolStripMenuItemModifySelected");
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
            // 
            // toolStripMenuItemMoveToTop
            // 
            resources.ApplyResources(this.toolStripMenuItemMoveToTop, "toolStripMenuItemMoveToTop");
            this.toolStripMenuItemMoveToTop.Name = "toolStripMenuItemMoveToTop";
            // 
            // toolStripMenuItemMoveToBottom
            // 
            resources.ApplyResources(this.toolStripMenuItemMoveToBottom, "toolStripMenuItemMoveToBottom");
            this.toolStripMenuItemMoveToBottom.Name = "toolStripMenuItemMoveToBottom";
            // 
            // toolStripMenuItemSortBySpeedTest
            // 
            resources.ApplyResources(this.toolStripMenuItemSortBySpeedTest, "toolStripMenuItemSortBySpeedTest");
            this.toolStripMenuItemSortBySpeedTest.Name = "toolStripMenuItemSortBySpeedTest";
            // 
            // toolStripMenuItemSortBySummary
            // 
            resources.ApplyResources(this.toolStripMenuItemSortBySummary, "toolStripMenuItemSortBySummary");
            this.toolStripMenuItemSortBySummary.Name = "toolStripMenuItemSortBySummary";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripMenuItemModifySettings
            // 
            resources.ApplyResources(this.toolStripMenuItemModifySettings, "toolStripMenuItemModifySettings");
            this.toolStripMenuItemModifySettings.Name = "toolStripMenuItemModifySettings";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // toolStripMenuItemCollapsePanel
            // 
            resources.ApplyResources(this.toolStripMenuItemCollapsePanel, "toolStripMenuItemCollapsePanel");
            this.toolStripMenuItemCollapsePanel.Name = "toolStripMenuItemCollapsePanel";
            // 
            // toolStripMenuItemExpansePanel
            // 
            resources.ApplyResources(this.toolStripMenuItemExpansePanel, "toolStripMenuItemExpansePanel");
            this.toolStripMenuItemExpansePanel.Name = "toolStripMenuItemExpansePanel";
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopyAsV2rayLink,
            this.toolStripMenuItemCopyAsVmessLink,
            this.toolStripMenuItemCopyAsSubscription});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // toolStripMenuItemCopyAsV2rayLink
            // 
            resources.ApplyResources(this.toolStripMenuItemCopyAsV2rayLink, "toolStripMenuItemCopyAsV2rayLink");
            this.toolStripMenuItemCopyAsV2rayLink.Name = "toolStripMenuItemCopyAsV2rayLink";
            // 
            // toolStripMenuItemCopyAsVmessLink
            // 
            resources.ApplyResources(this.toolStripMenuItemCopyAsVmessLink, "toolStripMenuItemCopyAsVmessLink");
            this.toolStripMenuItemCopyAsVmessLink.Name = "toolStripMenuItemCopyAsVmessLink";
            // 
            // toolStripMenuItemCopyAsSubscription
            // 
            resources.ApplyResources(this.toolStripMenuItemCopyAsSubscription, "toolStripMenuItemCopyAsSubscription");
            this.toolStripMenuItemCopyAsSubscription.Name = "toolStripMenuItemCopyAsSubscription";
            // 
            // toolStripMenuItemRestartSelected
            // 
            resources.ApplyResources(this.toolStripMenuItemRestartSelected, "toolStripMenuItemRestartSelected");
            this.toolStripMenuItemRestartSelected.Name = "toolStripMenuItemRestartSelected";
            // 
            // toolStripMenuItemStopSelected
            // 
            resources.ApplyResources(this.toolStripMenuItemStopSelected, "toolStripMenuItemStopSelected");
            this.toolStripMenuItemStopSelected.Name = "toolStripMenuItemStopSelected";
            // 
            // toolStripMenuItemSpeedTestOnSelected
            // 
            resources.ApplyResources(this.toolStripMenuItemSpeedTestOnSelected, "toolStripMenuItemSpeedTestOnSelected");
            this.toolStripMenuItemSpeedTestOnSelected.Name = "toolStripMenuItemSpeedTestOnSelected";
            // 
            // toolStripMenuItemPackSelectedServers
            // 
            resources.ApplyResources(this.toolStripMenuItemPackSelectedServers, "toolStripMenuItemPackSelectedServers");
            this.toolStripMenuItemPackSelectedServers.Name = "toolStripMenuItemPackSelectedServers";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolMenuItemRefreshSummary
            // 
            resources.ApplyResources(this.toolMenuItemRefreshSummary, "toolMenuItemRefreshSummary");
            this.toolMenuItemRefreshSummary.Name = "toolMenuItemRefreshSummary";
            // 
            // toolStripMenuItemDeleteServers
            // 
            resources.ApplyResources(this.toolStripMenuItemDeleteServers, "toolStripMenuItemDeleteServers");
            this.toolStripMenuItemDeleteServers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDeleteAllServer,
            this.toolStripMenuItemDeleteSelectedServers});
            this.toolStripMenuItemDeleteServers.Name = "toolStripMenuItemDeleteServers";
            // 
            // toolStripMenuItemDeleteAllServer
            // 
            resources.ApplyResources(this.toolStripMenuItemDeleteAllServer, "toolStripMenuItemDeleteAllServer");
            this.toolStripMenuItemDeleteAllServer.Name = "toolStripMenuItemDeleteAllServer";
            // 
            // toolStripMenuItemDeleteSelectedServers
            // 
            resources.ApplyResources(this.toolStripMenuItemDeleteSelectedServers, "toolStripMenuItemDeleteSelectedServers");
            this.toolStripMenuItemDeleteSelectedServers.Name = "toolStripMenuItemDeleteSelectedServers";
            // 
            // systemProxyToolStripMenuItem
            // 
            resources.ApplyResources(this.systemProxyToolStripMenuItem, "systemProxyToolStripMenuItem");
            this.systemProxyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemCurrentSysProxy,
            this.toolMenuItemClearSysProxy});
            this.systemProxyToolStripMenuItem.Name = "systemProxyToolStripMenuItem";
            // 
            // toolMenuItemCurrentSysProxy
            // 
            resources.ApplyResources(this.toolMenuItemCurrentSysProxy, "toolMenuItemCurrentSysProxy");
            this.toolMenuItemCurrentSysProxy.Name = "toolMenuItemCurrentSysProxy";
            // 
            // toolMenuItemClearSysProxy
            // 
            resources.ApplyResources(this.toolMenuItemClearSysProxy, "toolMenuItemClearSysProxy");
            this.toolMenuItemClearSysProxy.Name = "toolMenuItemClearSysProxy";
            // 
            // windowToolStripMenuItem
            // 
            resources.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMenuItemConfigEditor,
            this.toolMenuItemQRCode,
            this.toolMenuItemLog,
            this.toolMenuItemOptions});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            // 
            // toolMenuItemConfigEditor
            // 
            resources.ApplyResources(this.toolMenuItemConfigEditor, "toolMenuItemConfigEditor");
            this.toolMenuItemConfigEditor.Name = "toolMenuItemConfigEditor";
            // 
            // toolMenuItemQRCode
            // 
            resources.ApplyResources(this.toolMenuItemQRCode, "toolMenuItemQRCode");
            this.toolMenuItemQRCode.Name = "toolMenuItemQRCode";
            // 
            // toolMenuItemLog
            // 
            resources.ApplyResources(this.toolMenuItemLog, "toolMenuItemLog");
            this.toolMenuItemLog.Name = "toolMenuItemLog";
            // 
            // toolMenuItemOptions
            // 
            resources.ApplyResources(this.toolMenuItemOptions, "toolMenuItemOptions");
            this.toolMenuItemOptions.Name = "toolMenuItemOptions";
            // 
            // aboutToolStripMenuItem1
            // 
            resources.ApplyResources(this.aboutToolStripMenuItem1, "aboutToolStripMenuItem1");
            this.aboutToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDownLoadV2rayCore,
            this.toolStripMenuItemRemoveV2rayCore,
            this.toolMenuItemCheckUpdate,
            this.toolMenuItemAbout,
            this.toolMenuItemHelp});
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            // 
            // toolStripMenuItemDownLoadV2rayCore
            // 
            resources.ApplyResources(this.toolStripMenuItemDownLoadV2rayCore, "toolStripMenuItemDownLoadV2rayCore");
            this.toolStripMenuItemDownLoadV2rayCore.Name = "toolStripMenuItemDownLoadV2rayCore";
            // 
            // toolStripMenuItemRemoveV2rayCore
            // 
            resources.ApplyResources(this.toolStripMenuItemRemoveV2rayCore, "toolStripMenuItemRemoveV2rayCore");
            this.toolStripMenuItemRemoveV2rayCore.Name = "toolStripMenuItemRemoveV2rayCore";
            // 
            // toolMenuItemCheckUpdate
            // 
            resources.ApplyResources(this.toolMenuItemCheckUpdate, "toolMenuItemCheckUpdate");
            this.toolMenuItemCheckUpdate.Name = "toolMenuItemCheckUpdate";
            // 
            // toolMenuItemAbout
            // 
            resources.ApplyResources(this.toolMenuItemAbout, "toolMenuItemAbout");
            this.toolMenuItemAbout.Name = "toolMenuItemAbout";
            // 
            // toolMenuItemHelp
            // 
            resources.ApplyResources(this.toolMenuItemHelp, "toolMenuItemHelp");
            this.toolMenuItemHelp.Name = "toolMenuItemHelp";
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTotal});
            this.statusStrip1.Name = "statusStrip1";
            this.toolTip1.SetToolTip(this.statusStrip1, resources.GetString("statusStrip1.ToolTip"));
            // 
            // toolStripStatusLabelTotal
            // 
            resources.ApplyResources(this.toolStripStatusLabelTotal, "toolStripStatusLabelTotal");
            this.toolStripStatusLabelTotal.Name = "toolStripStatusLabelTotal";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.toolStripContainer2);
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelTotal;
        private Panel panel1;
        private ToolStripMenuItem SelectNoMarkToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripLabel toolStripLabel2;
        private ToolStripComboBox toolStripComboBoxMarkFilter;
    }
}
