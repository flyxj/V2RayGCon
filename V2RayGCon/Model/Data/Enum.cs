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
        public enum ProxyModes
        {
            ProxyNone,
            ProxyAll,
            ProxyPAC,
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
