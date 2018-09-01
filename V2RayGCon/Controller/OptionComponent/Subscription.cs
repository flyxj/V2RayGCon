using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
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
        string oldOptions;

        public Subscription(
            FlowLayoutPanel flyPanel,
            Button btnAdd,
            Button btnUpdate)
        {
            this.setting = Service.Setting.Instance;

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
                setting.SaveSubscriptionUrls(curOptions);
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

        List<Model.Data.UrlItem> CollectSubscriptionItems()
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
            var subItemList = setting.GetSubscribeItems();

            this.oldOptions = JsonConvert.SerializeObject(subItemList);

            if (subItemList.Count <= 0)
            {
                subItemList.Add(new Model.Data.UrlItem());
            }

            foreach (var item in subItemList)
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

        void BindEventBtnUpdateClick()
        {
            this.btnUpdate.Click += (s, a) =>
            {
                this.btnUpdate.Enabled = false;

                var urls = new List<string>();
                foreach (Model.UserControls.UrlListItem item in this.flyPanel.Controls)
                {
                    var value = item.GetValue();
                    if (value.inUse
                        && !string.IsNullOrEmpty(value.url)
                        && !urls.Contains(value.url))
                    {
                        urls.Add(value.url);
                    }
                }

                if (urls.Count <= 0)
                {
                    this.btnUpdate.Enabled = true;
                    MessageBox.Show(I18N("NoSubsUrlAvailable"));
                    return;
                }

                ImportFromSubscriptionUrls(urls);
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
            foreach (Model.UserControls.UrlListItem item in this.flyPanel.Controls)
            {
                item.SetIndex(index++);
            }
        }

        private void ImportFromSubscriptionUrls(List<string> urls)
        {
            Task.Factory.StartNew(() =>
            {
                var timeout = Lib.Utils.Str2Int(StrConst("ParseImportTimeOut"));

                var contents = Lib.Utils.ExecuteInParallel<string, string>(urls, (url) =>
                {
                    var subsString = Lib.Utils.Fetch(url, timeout * 1000);
                    if (string.IsNullOrEmpty(subsString))
                    {
                        setting.SendLog(I18N("DownloadFail") + "\n" + url);
                        return string.Empty;
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

                    return string.Join("\n", links);
                });

                setting.ImportLinks(string.Join("\n", contents));

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
