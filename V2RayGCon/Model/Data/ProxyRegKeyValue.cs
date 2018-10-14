namespace V2RayGCon.Model.Data
{
    public class ProxyRegKeyValue
    {
        // ProxyServer ProxyEnable AutoConfigURL ProxyOverride
        public string proxyServer, autoConfigUrl;
        public bool proxyEnable;  // 0 close 1 enable

        public ProxyRegKeyValue()
        {
            proxyEnable = false;
            proxyServer = string.Empty;
            autoConfigUrl = string.Empty;
            // proxyOverride = string.Empty; // do not touch this
        }
    }
}
