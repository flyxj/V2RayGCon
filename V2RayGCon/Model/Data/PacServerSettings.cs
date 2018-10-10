namespace V2RayGCon.Model.Data
{
    public class PacServerSettings
    {
        public string customWhiteList, customBlackList;
        public int port;
        public bool isAutorun;

        public PacServerSettings()
        {
            customBlackList = string.Empty;
            customBlackList = string.Empty;
            port = 3000;
            isAutorun = false;
        }
    }
}
