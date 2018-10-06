using System;
using System.Linq;
using System.Windows.Forms;

namespace V2RayGCon.Controller.FormMainComponent
{
    class MenuItemsSelect : FormMainComponentController
    {
        Service.Servers servers;
        public MenuItemsSelect(
            ToolStripMenuItem selectAll,
            ToolStripMenuItem selectNone,
            ToolStripMenuItem selectInvert,
            ToolStripMenuItem selectAutorun,
            ToolStripMenuItem selectRunning,
            ToolStripMenuItem selectTimeout,
            ToolStripMenuItem selectNoSpeedTest,
            ToolStripMenuItem selectNoMark,
            ToolStripMenuItem clearAllSelection,
            ToolStripMenuItem selectAllIgnorePage)
        {
            servers = Service.Servers.Instance;

            selectAllIgnorePage.Click += (s, a) =>
            {
                servers.SetAllServerIsSelected(true);
            };

            clearAllSelection.Click += (s, a) =>
            {
                servers.SetAllServerIsSelected(false);
            };

            selectNoMark.Click +=
                (s, a) => SelectAllServerIF(
                    el => string.IsNullOrEmpty(el.mark));

            selectNoSpeedTest.Click +=
                (s, a) => SelectAllServerIF(
                    el => el.speedTestResult < 0);

            selectTimeout.Click +=
                (s, a) => SelectAllServerIF(
                    el => el.speedTestResult == long.MaxValue);


            selectRunning.Click +=
                (s, a) => SelectAllServerIF(
                    el => el.isServerOn);

            selectAutorun.Click +=
                (s, a) => SelectAllServerIF(
                    el => el.isAutoRun);

            // current page only
            // fly panel may not ready while this init

            selectAll.Click += (s, a) =>
            {
                GetFlyPanel().SelectAll();
            };

            selectNone.Click += (s, a) =>
            {
                GetFlyPanel().SelectNone();
            };

            selectInvert.Click += (s, a) =>
            {
                GetFlyPanel().SelectInvert();
            };
        }

        #region public method
        public override bool RefreshUI()
        {
            return false;
        }

        public override void Cleanup()
        {
        }
        #endregion

        #region private method
        void SelectAllServerIF(Func<Model.Data.ServerItem, bool> condiction)
        {
            servers.GetServerList()
                .Select(s =>
                {
                    s.SetIsSelected(condiction(s));
                    return true;
                })
                .ToList();
        }

        Controller.FormMainComponent.FlyServer GetFlyPanel()
        {
            return this.GetContainer()
                .GetComponent<Controller.FormMainComponent.FlyServer>();
        }
        #endregion
    }
}
