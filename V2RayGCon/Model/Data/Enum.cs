namespace V2RayGCon.Model.Data
{
    public class Enum
    {
        public enum LinkTypes
        {
            vmess = 0,
            v2ray = 1,
            ss = 2,
            v = 3,
        }


        public enum ProxyTypes
        {
            config = 0,
            http = 1,
            socks = 2,
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
