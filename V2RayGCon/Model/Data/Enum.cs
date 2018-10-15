namespace V2RayGCon.Model.Data
{
    public class Enum
    {
        /// <summary>
        /// None,PAC,Global
        /// </summary>
        public enum SystemProxyMode
        {
            None,
            PAC,
            Global,
        }

        public enum Cultures
        {
            auto = 0,
            enUS = 1,
            zhCN = 2,
        }

        public enum LinkTypes
        {
            vmess = 0,
            v2ray = 1,
            ss = 2,
        }

        /// <summary>
        /// Inbound types
        /// </summary>
        public enum ProxyTypes
        {
            Config = 0,
            HTTP = 1,
            SOCKS = 2,
        }

        public enum FormLocations
        {
            TopLeft,
            BottomLeft,
            TopRight,
            BottomRight,
        }

        public enum SaveFileErrorCode
        {
            Fail,
            Cancel,
            Success,
        }
    }
}
