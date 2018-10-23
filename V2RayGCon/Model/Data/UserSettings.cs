namespace V2RayGCon.Model.Data
{
    class UserSettings
    {
        #region public properties
        public int MaxLogLine { get; set; }
        public int ServerPanelPageSize { get; set; }

        public bool CfgShowToolPanel { get; set; }
        public bool isPortable { get; set; }

        public string ImportUrls { get; set; }
        public string DecodeCache { get; set; }
        public string SubscribeUrls { get; set; }
        public string Culture { get; set; }
        public string ServerList { get; set; }
        public string PacServerSettings { get; set; }
        public string SysProxySetting { get; set; }
        public string ServerTracker { get; set; }
        public string WinFormPosList { get; set; }
        #endregion

        public UserSettings()
        {
            MaxLogLine = 1000;
            ServerPanelPageSize = 99;
            CfgShowToolPanel = true;
            isPortable = false;

            ImportUrls = string.Empty;
            DecodeCache = string.Empty;
            SubscribeUrls = string.Empty;
            Culture = string.Empty;
            ServerList = string.Empty;
            PacServerSettings = string.Empty;
            SysProxySetting = string.Empty;
            ServerTracker = string.Empty;
            WinFormPosList = string.Empty;
        }
    }
}
