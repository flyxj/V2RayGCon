namespace V2RayGCon.Model.Data
{
    public class PacServerSettings
    {
        public string customWhiteList, customBlackList, customPacFilePath;
        public int port;
        public bool isAutorun, isUseCustomPac;

        public PacServerSettings()
        {
            isUseCustomPac = false;
            customPacFilePath = string.Empty;
            customBlackList = string.Empty;
            customBlackList = string.Empty;
            port = 3000;
            isAutorun = false;
        }
    }
}
