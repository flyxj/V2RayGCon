namespace V2RayGCon.Model.Data
{
    public class Enum
    {
        /// <summary>
        /// 数值需要连续,否则无法和ComboBox的selectedIndex对应
        /// </summary>
        public enum Sections
        {
            Config = 0,
            Log = 1,
            Inbound = 2,
            Outbound = 3,
            Routing = 4,
            Policy = 5,
            V2raygcon = 6,
            Api = 7,
            Dns = 8,
            Stats = 9,
            Transport = 10,
            Reverse = 11,

            Seperator = 12, // eq first array

            Inbounds = 12,
            Outbounds = 13,
            InboundDetour = 14,
            OutboundDetour = 15,
        }

        /// <summary>
        /// Determine if the two ranges overlap
        /// </summary>
        public enum Overlaps
        {
            None,
            All,
            Left,
            Middle,
            Right,
        }

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
