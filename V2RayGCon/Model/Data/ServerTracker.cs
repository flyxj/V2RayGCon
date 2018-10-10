using System.Collections.Generic;

namespace V2RayGCon.Model.Data
{
    public class ServerTracker
    {
        public bool isTrackerOn;
        public List<string> serverList;

        public ServerTracker()
        {
            isTrackerOn = false;
            serverList = new List<string>();
        }
    }
}
