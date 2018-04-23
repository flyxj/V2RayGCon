namespace V2RayGCon.Model.Data
{
    class Enum
    {
        public enum LinkTypes
        {
            vmess,
            v2ray,
            ss,
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
    }
}
