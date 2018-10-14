namespace V2RayGCon.Model.Data
{
    public class PacUrlParams
    {
        public string ip, mime;
        public int port;
        public bool isSocks, isWhiteList, isDebug;

        // (string ip, int port, bool isSocks, bool isWhiteList)
        public PacUrlParams()
        {
            mime = "html";  // html,js,pac default pac
            ip = string.Empty;
            isSocks = false;
            isWhiteList = true;
            isDebug = false;
            port = 0;
        }
        #region public method
        // override operators is too complex for one simple purpose
        //public bool IsSameAs(PacUrlParams b)
        //{
        //    if (ip != b.ip
        //        || port != b.port
        //        || isSocks != b.isSocks
        //        || isWhiteList != b.isWhiteList
        //        || isDebug != b.isDebug)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        #endregion
    }
}
