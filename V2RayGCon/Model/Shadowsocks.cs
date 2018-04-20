using System;

namespace V2RayGCon.Model
{
    class Shadowsocks
    {
        public string method, pass, addr;

        public Shadowsocks()
        {
            method = String.Empty;
            pass = String.Empty;
            addr = String.Empty;
        }
    }
}
