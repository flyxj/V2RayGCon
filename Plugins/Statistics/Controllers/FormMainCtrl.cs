using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Statistics.Controllers
{
    public class FormMainCtrl
    {
        Services.Settings settings;
        VgcApis.Models.IServersService vgcServers;

        ListView dataView;
        ToolStripMenuItem miReset, miResizeByTitle, miResizeByContent;

        // const int updateInterval = 5000; // debug
        const int updateInterval = 2000;
        bool requireReset = false;
        bool[] sortFlags = new bool[] { false, false, false, false, false };

        public FormMainCtrl(
            Services.Settings settings,
            VgcApis.Models.IServersService vgcServers,

            ListView dataView,

            ToolStripMenuItem miReset,
            ToolStripMenuItem miResizeByTitle,
            ToolStripMenuItem miResizeByContent)
        {
            this.settings = settings;
            this.vgcServers = vgcServers;

            this.dataView = dataView;

            this.miReset = miReset;
            this.miResizeByContent = miResizeByContent;
            this.miResizeByTitle = miResizeByTitle;
        }

        #region public methods
        public void Cleanup()
        {
            ReleaseUpdateTimer();
        }

        public void Run()
        {
            ResizeDataViewByTitle();
            updateDataViewTimer.Tick += UpdateDataViewHandler;
            updateDataViewTimer.Start();
            miReset.Click += (s, a) => requireReset = true;
            miResizeByContent.Click += (s, a) => ResizeDataViewByContent();
            miResizeByTitle.Click += (s, a) => ResizeDataViewByTitle();
            dataView.ColumnClick += ColumnClickHandler;
        }
        #endregion

        #region copy from form main 
        void ColumnClickHandler(object sender, ColumnClickEventArgs args)
        {
            var column = args.Column;
            if (column < 1 || column > 4)
            {
                return;
            }

            // evil to-do
        }

        void UpdateDataViewHandler(object sender, EventArgs args)
        {
            Task.Factory.StartNew(UpdateDataViewWorker);
        }

        Models.StatsResult GetterCoreInfo(VgcApis.Models.ICoreCtrl coreCtrl)
        {
            var result = new Models.StatsResult();
            result.title = coreCtrl.GetTitle();
            result.uid = coreCtrl.GetUid();

            var curData = coreCtrl.Peek();
            if (curData != null)
            {
                result.stamp = curData.stamp;
                result.totalUp = curData.statsUplink;
                result.totalDown = curData.statsDownlink;
            }
            return result;
        }

        Timer updateDataViewTimer = new Timer
        {
            Interval = updateInterval,
        };

        void ReleaseUpdateTimer()
        {
            updateDataViewTimer.Stop();
            updateDataViewTimer.Tick -= UpdateDataViewHandler;
            updateDataViewTimer.Dispose();
        }

        readonly object updateDataViewLocker = new object();
        bool isUpdating = false;
        void UpdateDataViewWorker()
        {
            lock (updateDataViewLocker)
            {
                if (isUpdating)
                {
                    return;
                }
                isUpdating = true;
            }

            if (requireReset)
            {
                settings.ClearStatsData();
                requireReset = false;
            }

            var datas = vgcServers
                .GetAllServersList()
                .Where(s => s.IsCoreRunning())
                .OrderBy(s => s.GetIndex())
                .Select(s => GetterCoreInfo(s))
                .ToList();

            MergeStatsResultsIntoStatsDatas(datas);
            settings.SaveAllStatsData();

            ShowStatsDataOnDataView();
            isUpdating = false;
        }

        int dataViewKeyIndex = 0;
        void ShowStatsDataOnDataView()
        {
            const int MiB = 1024 * 1024;
            var contents = settings.GetAllStatsData()
                .Select(d =>
                {
                    var v = d.Value;
                    return new string[] {
                        v.title,
                        v.curDownSpeed.ToString(),
                        v.curUpSpeed.ToString(),
                        (v.totalDown/MiB).ToString(),
                        (v.totalUp/MiB).ToString(),
                    };
                });

            IEnumerable<string[]> sortedContent = null;
            var index = dataViewKeyIndex;
            switch (Math.Sign(index))
            {
                case 1:
                    sortedContent = contents.OrderBy(e => e[index]);
                    break;
                case -1:
                    sortedContent = contents.OrderByDescending(e => e[-index]);
                    break;
                default:
                    sortedContent = contents;
                    break;
            }

            var lvContent = sortedContent
                .Select(e => new ListViewItem(e))
                .ToArray();

            BatchUpdateDataView(() =>
            {
                dataView.Items.Clear();
                dataView.Items.AddRange(lvContent);
            });
        }

        void MergeStatsResultsIntoStatsDatas(
            List<Models.StatsResult> statsResults)
        {
            var datas = settings.GetAllStatsData();

            foreach (var s in statsResults)
            {
                var uid = s.uid;
                if (!datas.ContainsKey(uid))
                {
                    datas[uid] = s;
                    return;
                }
                UpdateStatsData(datas, s, uid);
            }
        }

        private static void UpdateStatsData(
            Dictionary<string, Models.StatsResult> datas,
            Models.StatsResult statsResult,
            string uid)
        {
            var p = datas[uid];
            var elapse = 1.0 * (statsResult.stamp - p.stamp) / TimeSpan.TicksPerSecond;
            if (elapse <= 1)
            {
                elapse = updateInterval / 1000.0;
            }

            var downSpeed = (statsResult.totalDown / elapse) / 1024.0;
            var upSpeed = (statsResult.totalUp / elapse) / 1024.0;
            p.curDownSpeed = downSpeed <= 0 ? 0 : (int)downSpeed;
            p.curUpSpeed = upSpeed <= 0 ? 0 : (int)upSpeed;
            p.stamp = statsResult.stamp;
            p.totalDown = p.totalDown + statsResult.totalDown;
            p.totalUp = p.totalUp + statsResult.totalUp;
        }

        #endregion

        #region private methods
        void BatchUpdateDataView(Action action)
        {
            dataView.Invoke((MethodInvoker)delegate
            {
                dataView.BeginUpdate();
                try
                {
                    action?.Invoke();
                }
                catch { }
                finally
                {
                    dataView.EndUpdate();
                }
            });
        }

        void ResizeDataViewColumn(bool byTitle)
        {
            var mode = byTitle ?
                ColumnHeaderAutoResizeStyle.HeaderSize :
                ColumnHeaderAutoResizeStyle.ColumnContent;

            var count = dataView.Columns.Count;
            for (int i = 1; i < count; i++)
            {
                dataView.Columns[i].AutoResize(mode);
            }
        }

        void ResizeDataViewByContent()
        {
            BatchUpdateDataView(
                () => ResizeDataViewColumn(false));
        }

        void ResizeDataViewByTitle()
        {
            BatchUpdateDataView(
                () => ResizeDataViewColumn(true));
        }

        #endregion
    }
}
