namespace V2RayGCon.Model.Data
{
    public class Enum
    {
        public enum LinkTypes
        {
            vmess = 0,
            v2ray = 1,
            ss = 2,
        }

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
