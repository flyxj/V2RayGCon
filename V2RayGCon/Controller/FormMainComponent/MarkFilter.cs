using System.Linq;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.FormMainComponent
{
    class MarkFilter : FormMainComponentController
    {
        Service.Servers servers;

        public MarkFilter(ComboBox cboxMarkeFilter)
        {
            servers = Service.Servers.Instance;
            InitMarkFilter(cboxMarkeFilter);
        }

        private void InitMarkFilter(ComboBox cboxMarkeFilter)
        {
            UpdateMarkFilterItemList(cboxMarkeFilter);
            cboxMarkeFilter.SelectedIndex = 0;
            cboxMarkeFilter.DropDown += (s, e) =>
            {
                cboxMarkeFilter.Invoke((MethodInvoker)delegate
                {
                    UpdateMarkFilterItemList(cboxMarkeFilter);
                });
            };
        }

        #region public method
        public override bool RefreshUI() { return false; }
        public override void Cleanup() { }
        #endregion

        #region private method
        void UpdateMarkFilterItemList(ComboBox marker)
        {
            var itemList = servers.GetMarkList().ToList();
            itemList.Insert(0, I18N("NoMark"));
            itemList.Insert(0, I18N("All"));

            marker.Items.Clear();
            foreach (var item in itemList)
            {
                marker.Items.Add(item);
            }
        }
        #endregion
    }
}
