using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class Import : OptionComponentController
    {
        FlowLayoutPanel flyPanel;
        Button btnAdd;

        Service.Setting setting;
        string oldOptions;

        public Import(
            FlowLayoutPanel flyPanel,
            Button btnAdd)
        {
            this.setting = Service.Setting.Instance;

            this.flyPanel = flyPanel;
            this.btnAdd = btnAdd;

            InitPanel();
            BindEvent();
        }

        #region public method
        public override bool SaveOptions()
        {
            string curOptions = GetCurOptions();

            if (curOptions != oldOptions)
            {
                setting.SaveGlobalImportItems(curOptions);
                oldOptions = curOptions;
                return true;
            }
            return false;
        }

        public override bool IsOptionsChanged()
        {
            return GetCurOptions() != oldOptions;
        }

        public void Reload(string rawSetting)
        {
            Properties.Settings.Default.ImportUrls = rawSetting;
            Properties.Settings.Default.Save();

            Lib.UI.ClearFlowLayoutPanel(this.flyPanel);
            InitPanel();
        }
        #endregion

        #region private method
        string GetCurOptions()
        {
            return JsonConvert.SerializeObject(CollectImportItems());
        }

        List<Model.Data.UrlItem> CollectImportItems()
        {
            var itemList = new List<Model.Data.UrlItem>();
            foreach (Model.UserControls.UrlListItem item in this.flyPanel.Controls)
            {
                var v = item.GetValue();
                if (!string.IsNullOrEmpty(v.alias)
                    || !string.IsNullOrEmpty(v.url))
                {
                    itemList.Add(v);
                }
            }
            return itemList;
        }

        void InitPanel()
        {
            var importUrlItemList = setting.GetGlobalImportItems();

            this.oldOptions = JsonConvert.SerializeObject(importUrlItemList);

            if (importUrlItemList.Count <= 0)
            {
                importUrlItemList.Add(new Model.Data.UrlItem());
            }

            foreach (var item in importUrlItemList)
            {
                this.flyPanel.Controls.Add(new Model.UserControls.UrlListItem(item, UpdatePanelItemsIndex));
            }

            UpdatePanelItemsIndex();
        }

        void BindEventBtnAddClick()
        {
            this.btnAdd.Click += (s, a) =>
            {
                this.flyPanel.Controls.Add(
                    new Model.UserControls.UrlListItem(
                        new Model.Data.UrlItem(),
                        UpdatePanelItemsIndex));
                UpdatePanelItemsIndex();
            };
        }

        void BindEventFlyPanelDragDrop()
        {
            this.flyPanel.DragDrop += (s, a) =>
            {
                // https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

                var data = a.Data.GetData(typeof(Model.UserControls.UrlListItem))
                    as Model.UserControls.UrlListItem;

                var _destination = s as FlowLayoutPanel;
                Point p = _destination.PointToClient(new Point(a.X, a.Y));
                var item = _destination.GetChildAtPoint(p);
                int index = _destination.Controls.GetChildIndex(item, false);
                _destination.Controls.SetChildIndex(data, index);
                _destination.Invalidate();
            };
        }

        void BindEvent()
        {
            BindEventBtnAddClick();
            BindEventFlyPanelDragDrop();

            this.flyPanel.DragEnter += (s, a) =>
            {
                a.Effect = DragDropEffects.Move;
            };
        }

        void UpdatePanelItemsIndex()
        {
            var index = 1;
            foreach (Model.UserControls.UrlListItem item in this.flyPanel.Controls)
            {
                item.SetIndex(index++);
            }
        }
        #endregion
    }
}
