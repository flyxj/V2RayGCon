namespace V2RayGCon.Model.Data
{
    public class PacUrlParams
    {
        public string ip;
        public int port;
        public bool isSocks, isWhiteList;

        // (string ip, int port, bool isSocks, bool isWhiteList)
        public PacUrlParams()
        {
            ip = string.Empty;
            isSocks = false;
            isWhiteList = true;
            port = 0;
        }

        // override operators is too complex for one simple purpose
        public bool IsSameAs(PacUrlParams b)
        {
            if (ip != b.ip
                || port != b.port
                || isSocks != b.isSocks
                || isWhiteList != b.isWhiteList)
            {
                return false;
            }
            return true;
        }
    }
}
