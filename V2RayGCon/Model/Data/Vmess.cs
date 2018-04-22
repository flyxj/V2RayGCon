using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V2RayGCon.Model.Data
{
    class Vmess
    {
        public string ps, add, port, id, aid, net, type, host, tls;

        public Vmess()
        {
            ps = String.Empty;      // alias
            add = String.Empty;     // ip,hostname
            port = String.Empty;    // port
            id = String.Empty;      // user id
            aid = String.Empty;
            net = String.Empty;     // ws,tcp,kcp
            type = String.Empty;    // kcp->header
            host = String.Empty;    // ws->path
            tls = String.Empty;     // streamSettings->security
        }
    }
}
