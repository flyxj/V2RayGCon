using System.Collections.Generic;

namespace Statistics.Services
{
    public class Settings
    {
        VgcApis.Models.ISettingService vgcSetting;
        Models.UserSettings userSettins;
        VgcApis.Libs.Sys.LazyGuy bookKeeper;

        #region public method
        public void SaveAllStatsData()
        {
            bookKeeper.DoItLater();
        }

        public void ClearStatsData()
        {
            userSettins.statsData =
                new Dictionary<string, Models.StatsResult>();
            bookKeeper.DoItLater();
        }

        public Dictionary<string, Models.StatsResult> GetAllStatsData()
        {
            return userSettins.statsData;
        }

        public void Run(
            VgcApis.Models.ISettingService vgcSetting)
        {
            this.vgcSetting = vgcSetting;
            userSettins = LoadUserSetting();
            bookKeeper = new VgcApis.Libs.Sys.LazyGuy(
                SaveUserSetting, 1000 * 60 * 5);
        }

        public void Cleanup()
        {
            bookKeeper.DoItNow();
            bookKeeper.DoneWithYou();
        }
        #endregion

        #region private method
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
