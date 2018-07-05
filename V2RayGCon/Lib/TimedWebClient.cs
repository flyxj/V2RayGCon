using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace V2RayGCon.Lib
{
    // use string s = new TimedWebClient { Timeout = 500 }.DownloadString(URL);
    
    public class TimedWebClient : WebClient
    {
        // https://stackoverflow.com/questions/12878857/how-to-limit-the-time-downloadstringurl-allowed-by-500-milliseconds
        // Timeout in milliseconds, default = 600,000 msec
        public int Timeout { get; set; }

        public TimedWebClient()
        {
            this.Timeout = 100000;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest = base.GetWebRequest(address);
            objWebRequest.Timeout = this.Timeout;
            return objWebRequest;
        }
    }

    
}
