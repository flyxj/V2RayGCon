using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormOption : Form
    {
        #region Sigleton
        static FormOption _instant;
        public static FormOption GetForm()
        {
            if (_instant == null || _instant.IsDisposed)
            {
                _instant = new FormOption();
            }
            return _instant;
        }
        #endregion

        Service.Setting setting;

        FormOption()
        {
            this.setting = Service.Setting.Instance;

            InitializeComponent();

            this.Show();
        }

        private void FormOption_Shown(object sender, System.EventArgs e)
        {
            var subItemList = setting.GetSubscribeItems();
            if (subItemList.Count <= 0)
            {
                subItemList.Add(new Model.Data.SubscribeItem());
            }

            foreach (var item in subItemList)
            {
                flySubsUrlContainer.Controls.Add(new Model.UserControls.UrlListItem(item));
            }

            UpdateFlySubUrlItemIndex();
        }


        #region public method
        public void UpdateFlySubUrlItemIndex()
        {
            var index = 1;
            foreach (Model.UserControls.UrlListItem item in flySubsUrlContainer.Controls)
            {
                item.SetIndex(index++);
            }
        }
        #endregion

        #region private method
        private void btnAddSubUrl_Click(object sender, System.EventArgs e)
        {

            flySubsUrlContainer.Controls.Add(
                new Model.UserControls.UrlListItem(
                    new Model.Data.SubscribeItem()));
            UpdateFlySubUrlItemIndex();

        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            var itemList = new List<Model.Data.SubscribeItem>();
            foreach (Model.UserControls.UrlListItem item in flySubsUrlContainer.Controls)
            {
                itemList.Add(item.GetValue());
            }
            setting.SaveSubscribeItems(itemList);

            MessageBox.Show(I18N("Done"));
        }

        private void flySubUrlContainer_DragDrop(object sender, DragEventArgs e)
        {
            // https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

            var data = e.Data.GetData(typeof(Model.UserControls.UrlListItem))
                as Model.UserControls.UrlListItem;

            var _destination = sender as FlowLayoutPanel;
            Point p = _destination.PointToClient(new Point(e.X, e.Y));
            var item = _destination.GetChildAtPoint(p);
            int index = _destination.Controls.GetChildIndex(item, false);
            _destination.Controls.SetChildIndex(data, index);
            _destination.Invalidate();
        }

        private void flySubsUrlContainer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnUpdateViaSubscription_Click(object sender, System.EventArgs e)
        {
            btnUpdateViaSubscription.Enabled = false;

            var urls = new List<string>();
            foreach (Model.UserControls.UrlListItem item in flySubsUrlContainer.Controls)
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
                btnUpdateViaSubscription.Enabled = true;
                MessageBox.Show(I18N("NoSubsUrlAvailable"));
                return;
            }

            ImportFromSubscriptionUrls(urls);
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
                    btnUpdateViaSubscription.Invoke((MethodInvoker)delegate
                    {
                        btnUpdateViaSubscription.Enabled = true;
                    });
                }
                catch { }

            });
        }
        #endregion
    }
}
