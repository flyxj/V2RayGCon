namespace V2RayGCon.Model.Data
{
    class LockStrPair
    {
        public object rwLock;
        public string content;

        public LockStrPair()
        {
            rwLock = new object();
            content = null;
        }

    }
}
