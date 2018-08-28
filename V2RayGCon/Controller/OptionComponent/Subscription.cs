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
        Button btnAdd, btnSave, btnUpdate;
        Service.Setting setting;

        public Subscription(
            FlowLayoutPanel flyPanel,
            Button btnAdd,
            Button btnSave,
            Button btnUpdate)
        {
            this.setting = Service.Setting.Instance;

            this.flyPanel = flyPanel;
            this.btnAdd = btnAdd;
            this.btnSave = btnSave;
            this.btnUpdate = btnUpdate;

            InitPanel();
            BindEvent();
        }

        #region public method

        #endregion

        #region private method
        void InitPanel()
        {
            var subItemList = setting.GetSubscribeItems();
            if (subItemList.Count <= 0)
            {
                subItemList.Add(new Model.Data.SubscribeItem());
            }

            foreach (var item in subItemList)
            {
                this.flyPanel.Controls.Add(new Model.UserControls.UrlListItem(item, UpdatePanelItemsIndex));
            }

            UpdatePanelItemsIndex();
        }

        void BindEvent()
        {
            this.btnAdd.Click += (s, a) =>
            {
                this.flyPanel.Controls.Add(
                    new Model.UserControls.UrlListItem(
                        new Model.Data.SubscribeItem(),
                        UpdatePanelItemsIndex));
                UpdatePanelItemsIndex();
            };

            this.btnSave.Click += (s, a) =>
            {
                var itemList = new List<Model.Data.SubscribeItem>();
                foreach (Model.UserControls.UrlListItem item in this.flyPanel.Controls)
                {
                    itemList.Add(item.GetValue());
                }
                setting.SaveSubscribeItems(itemList);

                MessageBox.Show(I18N("Done"));
            };

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

            this.flyPanel.DragEnter += (s, a) =>
            {
                a.Effect = DragDropEffects.Move;
            };

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
