﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Statistics.Services
{
    public class Settings
    {
        VgcApis.Models.IServices.ISettingService vgcSetting;
        VgcApis.Models.IServices.IServersService vgcServers;

        Models.UserSettings userSettins;
        VgcApis.Libs.Sys.LazyGuy bookKeeper;
        Timer bgStatsDataUpdateTimer = null;

        #region properties
        public int statsDataUpdateInterval { get; } = 2000;
        public bool isRequireClearStatsData { get; set; } = false;
        const int bgStatsDataUpdateInterval = 5 * 60 * 1000;
        #endregion

        #region public method
        public bool IsShutdown() => vgcSetting.IsShutdown();

        public void RequireHistoryStatsDataUpdate()
        {
            if (isUpdating)
            {
                return;
            }
            UpdateHistoryStatsDataWorker();
        }

        void ClearStatsDataOnDemand()
        {
            if (!isRequireClearStatsData)
            {
                return;
            }

            userSettins.statsData =
                new Dictionary<string, Models.StatsResult>();
            isRequireClearStatsData = false;
        }

        public Dictionary<string, Models.StatsResult> GetAllStatsData()
        {
            return userSettins.statsData;
        }

        public void Run(
            VgcApis.Models.IServices.ISettingService vgcSetting,
            VgcApis.Models.IServices.IServersService vgcServers)
        {
            this.vgcSetting = vgcSetting;
            this.vgcServers = vgcServers;

            userSettins = LoadUserSetting();
            bookKeeper = new VgcApis.Libs.Sys.LazyGuy(
                SaveUserSetting, 1000 * 60 * 5);
            StartBgStatsDataUpdateTimer();
            vgcServers.OnCoreClosing += OnCoreClosingHandler;
        }

        public void Cleanup()
        {
            vgcServers.OnCoreClosing -= OnCoreClosingHandler;
            ReleaseBgStatsDataUpdateTimer();

            // Calling v2ctl.exe at shutdown can cause problems.
            // So losing 5 minutes of statistics data is an acceptable loss.
            if (!IsShutdown())
            {
                VgcApis.Libs.Sys.FileLog.Info("Statistics: save data");
                UpdateHistoryStatsDataWorker();
            }

            bookKeeper.DoItNow();
            bookKeeper.Quit();
            VgcApis.Libs.Sys.FileLog.Info("Statistics: done!");
        }
        #endregion

        #region private method
        void OnCoreClosingHandler(
            object sender,
            VgcApis.Models.Datas.StrEvent args)
        {
            var uid = args.Data;
            var coreCtrl = vgcServers
                .GetAllServersList()
                .FirstOrDefault(s => s.GetUid() == uid);
            if (coreCtrl == null)
            {
                return;
            }
            var sample = coreCtrl.TakeStatisticsSample();
            var title = coreCtrl.GetTitle();
            Task.Factory.StartNew(
                () => AddToHistoryStatsData(uid, title, sample));

        }

        void AddToHistoryStatsData(
            string uid,
            string title,
            VgcApis.Models.Datas.StatsSample sample)
        {
            if (sample == null)
            {
                return;
            }

            var datas = userSettins.statsData;
            if (datas.ContainsKey(uid))
            {
                datas[uid].totalDown += sample.statsDownlink;
                datas[uid].totalUp += sample.statsUplink;
                return;
            }
            datas[uid] = new Models.StatsResult
            {
                uid = uid,
                title = title,
                totalDown = sample.statsDownlink,
                totalUp = sample.statsUplink,
            };
        }

        void BgStatsDataUpdateHandler(object sender, EventArgs args)
        {
            if (isUpdating)
            {
                return;
            }
            RequireHistoryStatsDataUpdate();
        }

        void StartBgStatsDataUpdateTimer()
        {
            bgStatsDataUpdateTimer = new Timer
            {
                Interval = bgStatsDataUpdateInterval,
            };
            bgStatsDataUpdateTimer.Start();
        }

        void ReleaseBgStatsDataUpdateTimer()
        {
            if (bgStatsDataUpdateTimer == null)
            {
                return;
            }
            bgStatsDataUpdateTimer.Stop();
            bgStatsDataUpdateTimer.Elapsed -= BgStatsDataUpdateHandler;
            bgStatsDataUpdateTimer.Dispose();
        }

        bool isUpdating = false;
        readonly object updateHistoryStatsDataLocker = new object();
        void UpdateHistoryStatsDataWorker()
        {
            lock (updateHistoryStatsDataLocker)
            {
                isUpdating = true;
                var newDatas = vgcServers
                    .GetAllServersList()
                    .Where(s => s.IsCoreRunning())
                    .OrderBy(s => s.GetIndex())
                    .Select(s => GetterCoreInfo(s))
                    .ToList();

                ClearStatsDataOnDemand();

                var historyDatas = userSettins.statsData;
                ResetCurSpeed(historyDatas);

                foreach (var d in newDatas)
                {
                    var uid = d.uid;
                    if (!historyDatas.ContainsKey(uid))
                    {
                        historyDatas[uid] = d;
                        continue;
                    }
                    MergeNewDataIntoHistoryData(historyDatas, d, uid);
                }

                bookKeeper.DoItLater();
                isUpdating = false;
            }
        }

        void MergeNewDataIntoHistoryData(
            Dictionary<string, Models.StatsResult> datas,
            Models.StatsResult statsResult,
            string uid)
        {
            var p = datas[uid];

            var elapse = 1.0 * (statsResult.stamp - p.stamp) / TimeSpan.TicksPerSecond;
            if (elapse <= 1)
            {
                elapse = statsDataUpdateInterval / 1000.0;
            }

            var downSpeed = (statsResult.totalDown / elapse) / 1024.0;
            var upSpeed = (statsResult.totalUp / elapse) / 1024.0;
            p.curDownSpeed = Math.Max(0, (int)downSpeed);
            p.curUpSpeed = Math.Max(0, (int)upSpeed);
            p.stamp = statsResult.stamp;
            p.totalDown = p.totalDown + statsResult.totalDown;
            p.totalUp = p.totalUp + statsResult.totalUp;
        }

        void ResetCurSpeed(Dictionary<string, Models.StatsResult> datas)
        {
            foreach (var data in datas)
            {
                data.Value.curDownSpeed = 0;
                data.Value.curUpSpeed = 0;
            }
        }

        Models.StatsResult GetterCoreInfo(VgcApis.Models.IControllers.ICoreCtrl coreCtrl)
        {
            var result = new Models.StatsResult();
            result.title = coreCtrl.GetTitle();
            result.uid = coreCtrl.GetUid();

            var curData = coreCtrl.TakeStatisticsSample();
            if (curData != null)
            {
                result.stamp = curData.stamp;
                result.totalUp = curData.statsUplink;
                result.totalDown = curData.statsDownlink;
            }
            return result;
        }

        void SaveUserSetting()
        {
            vgcSetting.SavePluginsSetting(
                Properties.Resources.Name,
                VgcApis.Libs.Utils.SerializeObject(userSettins));
        }

        Models.UserSettings LoadUserSetting()
        {
            string uss = vgcSetting.GetPluginsSetting(
                Properties.Resources.Name);
            try
            {
                var us = VgcApis.Libs.Utils
                    .DeserializeObject<Models.UserSettings>(uss);
                if (us != null)
                {
                    return us;
                }
            }
            catch { }
            return new Models.UserSettings();
        }
        #endregion
    }
}
