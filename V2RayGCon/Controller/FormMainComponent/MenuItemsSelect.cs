using System.Windows.Forms;

namespace V2RayGCon.Controller.FormMainComponent
{
    class MenuItemsSelect : FormMainComponentController
    {


        public MenuItemsSelect(
            ToolStripMenuItem selectAll,
            ToolStripMenuItem selectNone,
            ToolStripMenuItem selectInvert,
            ToolStripMenuItem selectAutorun,
            ToolStripMenuItem selectRunning,
            ToolStripMenuItem selectTimeout)
        {
            

            selectTimeout.Click += (s, a) =>
            {
                // fly panel may not ready while this init
                var panel = GetFlyPanel();
                panel.SelectTimeout();
            };

            selectRunning.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectRunning();
            };

            selectAutorun.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectAutorun();
            };

            selectAll.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectAll();
            };

            selectNone.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectNone();
            };

            selectInvert.Click += (s, a) =>
            {
                var panel = GetFlyPanel();
                panel.SelectInvert();
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

        Controller.FormMainComponent.FlyServer GetFlyPanel()
        {
            return this.GetContainer()
                .GetComponent<Controller.FormMainComponent.FlyServer>();
        }
        #endregion
    }
}
