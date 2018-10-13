namespace V2RayGCon.Model.Data
{

    public class PacCustomHeader
    {
        public string protocol, mode, customWhite, customBlack, ip;
        public int port;

        public PacCustomHeader(
            PacUrlParams urlParam,
            string customWhiteList,
            string customBlackList)

            : this(urlParam.isSocks,
                urlParam.isWhiteList,
                urlParam.ip,
                urlParam.port,
                customWhiteList,
                customBlackList)
        { }

        public PacCustomHeader(
            bool isSocks,
            bool isWhiteList,
            string ip,
            int port,
            string customWhiteList,
            string customBlackList)
        {
            this.protocol = isSocks ? "socks" : "http";
            this.mode = isWhiteList ? "white" : "black";
            this.customWhite = customWhiteList;
            this.customBlack = customBlackList;
            this.ip = ip;
            this.port = port;
        }

    }
}
