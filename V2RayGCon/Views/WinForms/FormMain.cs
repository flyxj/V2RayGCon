using System;
using System.Windows.Forms;

namespace V2RayGCon.Views.WinForms
{
    public partial class FormMain : Form
    {
        #region Sigleton
        static FormMain _instant;
        public static FormMain GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormMain();
            }
            _instant.Activate();
            return _instant;
        }
        #endregion

        Controller.FormMainCtrl formMainCtrl;
        Service.Setting setting;
        Service.Servers servers;

        FormMain()
        {
            setting = Service.Setting.Instance;
            servers = Service.Servers.Instance;

            InitializeComponent();

#if DEBUG
            this.Icon = Properties.Resources.icon_light;
#endif

            this.Show();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            setting.RestoreFormRect(this);
            Lib.UI.AutoScaleToolSripControls(this, 16);

            // https://alexpkent.wordpress.com/2011/05/11/25/
            // 添加新控件的时候会有bug,不显示新控件
            // ToolStripManager.LoadSettings(this); 

            this.FormClosed += (s, a) =>
            {
                setting.SaveFormRect(this);
                // ToolStripManager.SaveSettings(this);
                formMainCtrl.Cleanup();
                servers.LazyGC();
            };

            this.Text = string.Format(
                "{0} v{1}",
                Properties.Resources.AppName,
                Properties.Resources.Version);

            formMainCtrl = InitFormMainCtrl();
            BindToolStripButtonToMenuItem();
        }

        #region private method

        void BindToolStripButtonToMenuItem()
        {
            void bind(ToolStripButton button, ToolStripMenuItem menu)
            {
                button.Click += (s, a) => menu.PerformClick();
            }

            bind(toolStripButtonSelectAllCurPage, selectAllCurPageToolStripMenuItem);
            bind(toolStripButtonInverseSelectionCurPage, invertSelectionCurPageToolStripMenuItem);
            bind(toolStripButtonSelectNoneCurPage, selectNoneCurPageToolStripMenuItem1);

            bind(toolStripButtonCollapseSelected, toolStripMenuItemFoldingPanel);
            bind(toolStripButtonExpanSelected, toolStripMenuItemExpansePanel);

            bind(toolStripButtonRestartSelected, toolStripMenuItemRestartSelected);
            bind(toolStripButtonStopSelected, toolStripMenuItemStopSelected);

            bind(toolStripButtonModifySelected, toolStripMenuItemModifySettings);
            bind(toolStripButtonSortSelectedBySummary, toolStripMenuItemSortBySummary);
            bind(toolStripButtonSortSelectedBySpeedTestResult, toolStripMenuItemSortBySpeedTest);

            bind(toolStripButtonFormOption, toolMenuItemOptions);
        }

        private Controller.FormMainCtrl InitFormMainCtrl()
        {
            var ctrl = new Controller.FormMainCtrl();

            ctrl.Plug(new Controller.FormMainComponent.FlyServer(
                flyServerListContainer,
                toolStripLabelMarkFilter,
                toolStripComboBoxMarkFilter,
                toolStripStatusLabelTotal,
                toolStripDropDownButtonPager,
                toolStripStatusLabelPrePage,
                toolStripStatusLabelNextPage));

            ctrl.Plug(new Controller.FormMainComponent.MenuItemsBasic(
                toolMenuItemSimAddVmessServer,
                toolMenuItemImportLinkFromClipboard,
                toolMenuItemExportAllServerToFile,
                toolMenuItemImportFromFile,
                toolMenuItemCheckUpdate,
                toolMenuItemAbout,
                toolMenuItemHelp,
                toolMenuItemConfigEditor,
                toolMenuItemQRCode,
                toolMenuItemLog,
                toolMenuItemOptions,
                toolStripMenuItemDownLoadV2rayCore,
                toolStripMenuItemRemoveV2rayCore));

            ctrl.Plug(new Controller.FormMainComponent.MenuItemsSelect(
                /*
                ToolStripMenuItem selectAllCurPage,
                ToolStripMenuItem invertSelectionCurPage,
                ToolStripMenuItem selectNoneCurPage,
                */
                selectAllCurPageToolStripMenuItem,
                invertSelectionCurPageToolStripMenuItem,
                selectNoneCurPageToolStripMenuItem1,

                /*
                ToolStripMenuItem selectAllAllPages,
                ToolStripMenuItem invertSelectionAllPages,
                ToolStripMenuItem selectNoneAllPages,       
                */
                selectAllAllPagesToolStripMenuItem,
                invertSelectionAllPagesToolStripMenuItem,
                selectNoneAllPagesToolStripMenuItem,

                /*
                ToolStripMenuItem selectAutorunAllPages,
                ToolStripMenuItem selectNoMarkAllPages,
                ToolStripMenuItem selectNoSpeedTestAllPages,
                ToolStripMenuItem selectRunningAllPages,
                ToolStripMenuItem selectTimeoutAllPages,          
                */
                selectAutorunAllPagesToolStripMenuItem,
                selectNoMarkAllPagesToolStripMenuItem,
                selectNoSpeedTestAllPagesToolStripMenuItem,
                selectRunningAllPagesToolStripMenuItem,
                selectTimeoutAllPagesToolStripMenuItem,

                /*
                ToolStripMenuItem selectAllAllServers,
                ToolStripMenuItem invertSelectionAllServers,
                ToolStripMenuItem selectNoneAllServers,
                */
                selectAllAllServersToolStripMenuItem,
                invertSelectionAllServersToolStripMenuItem,
                selectNoneAllServersToolStripMenuItem,

                /*
                ToolStripMenuItem selectAutorunAllServers,
                ToolStripMenuItem selectNoMarkAllServers,
                ToolStripMenuItem selectNoSpeedTestAllServers,
                ToolStripMenuItem selectRunningAllServers,
                ToolStripMenuItem selectTimeoutAllServers
                */
                selectAutorunAllServersToolStripMenuItem,
                selectNoMarkAllServersToolStripMenuItem,
                selectNoSpeedTestAllServersToolStripMenuItem,
                selectRunningAllServersToolStripMenuItem,
                selectTimeoutAllServersToolStripMenuItem));

            ctrl.Plug(new Controller.FormMainComponent.MenuItemsServer(
                // for invoke ui refresh
                //MenuStrip menuContainer,
                mainMneuStrip,

                //// system proxy
                //ToolStripMenuItem curSysProxySummary,
                //ToolStripMenuItem copyCurPacUrl,
                //ToolStripMenuItem clearSysProxy,
                //ToolStripMenuItem restartPACServer,
                //ToolStripMenuItem stopPACServer,
                toolMenuItemCurrentSysProxy,
                copyPACURLToolStripMenuItem,
                copyPACDebuggerURLToolStripMenuItem,
                toolMenuItemClearSysProxy,
                startPACServerToolStripMenuItem,
                stopPACServerToolStripMenuItem,

                //// misc
                //ToolStripMenuItem refreshSummary,
                //ToolStripMenuItem deleteAllServers,
                //ToolStripMenuItem deleteSelected,
                refreshSummaryToolStripMenuItem,
                toolStripMenuItemDeleteAllServer,
                toolStripMenuItemDeleteSelectedServers,

                //// copy
                //ToolStripMenuItem copyAsV2rayLinks,
                //ToolStripMenuItem copyAsVmessLinks,
                //ToolStripMenuItem copyAsSubscriptions,
                toolStripMenuItemCopyAsV2rayLink,
                toolStripMenuItemCopyAsVmessLink,
                toolStripMenuItemCopyAsSubscription,

                //// batch op
                //ToolStripMenuItem speedTestOnSelected,
                //ToolStripMenuItem modifySelected,
                //ToolStripMenuItem packSelected,
                //ToolStripMenuItem stopSelected,
                //ToolStripMenuItem restartSelected,
                toolStripMenuItemSpeedTestOnSelected,
                toolStripMenuItemModifySettings,
                toolStripMenuItemPackSelectedServers,
                toolStripMenuItemStopSelected,
                toolStripMenuItemRestartSelected,

                //// view
                //ToolStripMenuItem moveToTop,
                //ToolStripMenuItem moveToBottom,
                //ToolStripMenuItem foldPanel,
                //ToolStripMenuItem expansePanel,
                //ToolStripMenuItem sortBySpeed,
                //ToolStripMenuItem sortBySummary)
                toolStripMenuItemMoveToTop,
                toolStripMenuItemMoveToBottom,
                toolStripMenuItemFoldingPanel,
                toolStripMenuItemExpansePanel,
                toolStripMenuItemSortBySpeedTest,
                toolStripMenuItemSortBySummary));

            return ctrl;
        }

        #endregion

        #region UI event handler
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
