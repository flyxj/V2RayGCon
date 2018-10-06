using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Controller.OptionComponent
{
    class Subscription : OptionComponentController
    {
        FlowLayoutPanel flyPanel;
        Button btnAdd, btnUpdate;

        Service.Setting setting;
        Service.Servers servers;

        string oldOptions;

        public Subscription(
            FlowLayoutPanel flyPanel,
            Button btnAdd,
            Button btnUpdate)
        {
            this.setting = Service.Setting.Instance;
            this.servers = Service.Servers.Instance;

            this.flyPanel = flyPanel;
            this.btnAdd = btnAdd;
            this.btnUpdate = btnUpdate;

            InitPanel();
            BindEvent();
        }

        #region public method
        public override bool SaveOptions()
        {
            string curOptions = GetCurOptions();

            if (curOptions != oldOptions)
            {
                setting.SaveSubscriptionItems(curOptions);
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
            Properties.Settings.Default.SubscribeUrls = rawSetting;
            Properties.Settings.Default.Save();

            Lib.UI.ClearFlowLayoutPanel(this.flyPanel);
            InitPanel();
        }
        #endregion

        #region private method
        string GetCurOptions()
        {
            return JsonConvert.SerializeObject(CollectSubscriptionItems());
        }

        List<Model.Data.SubscriptionItem> CollectSubscriptionItems()
        {
            var itemList = new List<Model.Data.SubscriptionItem>();
            foreach (Model.UserControls.SubscriptionUI item in this.flyPanel.Controls)
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
            var subItemList = setting.GetSubscriptionItems();

            this.oldOptions = JsonConvert.SerializeObject(subItemList);

            if (subItemList.Count <= 0)
            {
                subItemList.Add(new Model.Data.SubscriptionItem());
            }

            foreach (var item in subItemList)
            {
                this.flyPanel.Controls.Add(new Model.UserControls.SubscriptionUI(item, UpdatePanelItemsIndex));
            }

            UpdatePanelItemsIndex();
        }

        void BindEventBtnAddClick()
        {
            this.btnAdd.Click += (s, a) =>
            {
                this.flyPanel.Controls.Add(
                    new Model.UserControls.SubscriptionUI(
                        new Model.Data.SubscriptionItem(),
                        UpdatePanelItemsIndex));
                UpdatePanelItemsIndex();
            };
        }

        void BindEventBtnUpdateClick()
        {
            this.btnUpdate.Click += (s, a) =>
            {
                this.btnUpdate.Enabled = false;

                var subs = new Dictionary<string, string>();
                foreach (Model.UserControls.SubscriptionUI item in this.flyPanel.Controls)
                {
                    var value = item.GetValue();
                    if (value.isUse
                        && !string.IsNullOrEmpty(value.url)
                        && !subs.ContainsKey(value.url))
                    {
                        subs[value.url] = value.isSetMark ? value.alias : null;
                    }
                }

                if (subs.Count <= 0)
                {
                    this.btnUpdate.Enabled = true;
                    MessageBox.Show(I18N("NoSubsUrlAvailable"));
                    return;
                }

                ImportFromSubscriptionUrls(subs);
            };
        }

        void BindEventFlyPanelDragDrop()
        {
            this.flyPanel.DragDrop += (s, a) =>
            {
                // https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

                var data = a.Data.GetData(typeof(Model.UserControls.SubscriptionUI))
                    as Model.UserControls.SubscriptionUI;

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
            BindEventBtnUpdateClick();
            BindEventFlyPanelDragDrop();

            this.flyPanel.DragEnter += (s, a) =>
            {
                a.Effect = DragDropEffects.Move;
            };
        }

        void UpdatePanelItemsIndex()
        {
            var index = 1;
            foreach (Model.UserControls.SubscriptionUI item in this.flyPanel.Controls)
            {
                item.SetIndex(index++);
            }
        }

        private void ImportFromSubscriptionUrls(Dictionary<string, string> subscriptions)
        {
            Task.Factory.StartNew(() =>
            {
                // dict( [url]=>mark ) to list(url,mark) mark maybe null
                var list = subscriptions.Select(s => s).ToList();

                var timeout = Lib.Utils.Str2Int(StrConst("ParseImportTimeOut"));
                var contents = Lib.Utils.ExecuteInParallel<
                    KeyValuePair<string, string>,
                    string[]>(list, (item) =>
                 {
                     // item[url]=mark
                     var subsString = Lib.Utils.Fetch(item.Key, timeout * 1000);
                     if (string.IsNullOrEmpty(subsString))
                     {
                         setting.SendLog(I18N("DownloadFail") + "\n" + item.Key);
                         return new string[] { string.Empty, item.Value };
                     }

                     var links = new List<string>();
                     var matches = Regex.Matches(subsString, StrConst("PatternBase64NonStandard"));
                     foreach (Match match in matches)
                     {
                         try
                         {
                             links.Add(Lib.Utils.Base64Decode(match.Value));
                         }
                         catch { }
                     }

                     return new string[] { string.Join("\n", links), item.Value };
                 });

                servers.ImportLinks(contents);

                try
                {
                    this.btnUpdate.Invoke((MethodInvoker)delegate
                    {
                        this.btnUpdate.Enabled = true;
                    });
                }
                catch { }

            });
        }
        #endregion
    }
}
