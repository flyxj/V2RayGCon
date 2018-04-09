using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V2RayGCon.Model
{
    class Enum
    {
        public enum LinkTypes
        {
            vmess,
            v2ray
        }
        public enum ProxyModes {
            ProxyNone,
            ProxyAll,
            ProxyPAC,
        }
    }
}
