using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Model.Data
{
    class Table
    {
        public static Dictionary<int, string> configSections = new Dictionary<int, string>
        {
                { 0, "config.json"},
                { 1, "log"},
                { 2, "api"},
                { 3, "dns"},
                { 4, "stats"},
                { 5, "routing"},
                { 6, "policy"},
                { 7, "inbound"},
                { 8, "outbound"},
                { 9, "transport"},
                { 10,"v2raygcon" },
                { 11,"inboundDetour"},
                { 12,"outboundDetour"},
        };

        // separate between dictionary or array
        public const int sectionSeparator = 11;
        public const int inboundIndex = 7;
        public const int outboundIndex = 8;
        public static string[] proxyTypesString = new string[] { "config", "http", "socks" };


        public static Dictionary<int, string> linkPrefix = new Dictionary<int, string>
        {
            {0,"vmess://" },
            {1,"v2ray://" },
            {2,"ss://" },
        };

        public static Dictionary<int, string> ssMethods = new Dictionary<int, string>
        {
            { 0,"aes-128-cfb"},
            { 1,"aes-128-gcm"},
            { 2,"aes-256-cfb"},
            { 3,"aes-256-gcm"},
            { 4,"chacha20"},
            { 5,"chacha20-ietf"},
            { 6,"chacha20-poly1305"},
            { 7,"chacha20-ietf-poly1305"},
        };

        public static Dictionary<int, string> streamTLS = new Dictionary<int, string>
        {
            { 0, "none" },
            { 1, "tls" },
        };

        public static Dictionary<int, string> ssNetworks = new Dictionary<int, string>
        {
            { 0, "tcp" },
            { 1, "udp" },
            { 2, "tcp,udp" },
        };

        public static Dictionary<int, string> kcpTypes = new Dictionary<int, string> {
            {0, "none" },
            {1, "srtp" },
            {2, "utp" },
            {3, "wechat-video" },
            {4, "dtls" },
        };

        public static Dictionary<int, StreamComponent> streamSettings = new Dictionary<int, Model.Data.StreamComponent>
        {
            //public bool dropDownStyle;
            //public string name;
            //public string network;
            //public string optionPath;
            //public Dictionary<string, string> options;

            // kcp
            { 0, new StreamComponent{
                dropDownStyle=true,
                name="mKCP",
                network="kcp",
                optionPath="kcpSettings.header.type",
                options=new Dictionary<string,string>{
                    { "none", "kcp"},
                    { "srtp", "kcp_srtp" },
                    { "utp", "kcp_utp"},
                    { "wechat-video", "kcp_wechat-video" },
                    { "dtls", "kcp_dtls"},
                },
            } },

            // tcp
            { 1, new StreamComponent{
                dropDownStyle=true,
                name="TCP",
                network="tcp",
                optionPath="tcpSettings.header.type",
                options=new Dictionary<string, string>{
                    { "none","tcp" },
                    { "http","tcp_http" },
                },
            } },

            // h2 ws dsock
            { 2, new StreamComponent{
                dropDownStyle=false,
                name="HTTP/2",
                network="h2",
                optionPath="httpSettings.path",
                options=new Dictionary<string, string>{
                    { "none","h2" },
                },
            } },
            { 3, new StreamComponent{
                dropDownStyle=false,
                name="WebSocket",
                network="ws",
                optionPath="wsSettings.path",
                options=new Dictionary<string, string>{
                    { "none","ws" },
                },
            } },
            { 4, new StreamComponent{
                dropDownStyle=false,
                name="DomainSocket",
                network="domainsocket",
                optionPath="dsSettings.path",
                options=new Dictionary<string, string>{
                    { "none","dsock" },
                },
            } },
        };

        // editor examples
        public static Dictionary<int, List<string[]>> examples = ExampleHelper();
        static Dictionary<int, List<string[]>> ExampleHelper()
        {
            string[] SS(string description, string key)
            {
                return new string[] { description, key };
            }

            string[] SSS(string description, string key, string protocol)
            {
                return new string[] { description, key, protocol };
            }

            List<string[]> NewList()
            {
                return new List<string[]>();
            }

            var d = new Dictionary<int, List<string[]>>();

            List<string[]> list;

            // { 0, "config.json"},
            list = NewList();
            list.Add(SS(I18N("Default"), "cfgMin"));
            d.Add(0, list);

            //{ 1, "log"},
            list = NewList();
            list.Add(SS(I18N("Default"), "logFile"));
            list.Add(SS("None", "logNone"));
            list.Add(SS("Error", "logError"));
            list.Add(SS("Warning", "logWarning"));
            list.Add(SS("Info", "logInfo"));
            list.Add(SS("Debug", "logDebug"));
            d.Add(1, list);

            //{ 2, "api"},
            list = NewList();
            list.Add(SS(I18N("Default"), "apiDefault"));
            d.Add(2, list);

            //{ 3, "dns"},
            list = NewList();
            list.Add(SS(I18N("Default"), "dnsDefault"));
            list.Add(SS(I18N("CFnGoogle"), "dnsCFnGoogle"));
            d.Add(3, list);

            //{ 4, "stats"},

            //{ 5, "routing"},
            list = NewList();
            list.Add(SS(I18N("Default"), "routeAll"));
            list.Add(SS("skip CN web site", "routeCNIP"));
            list.Add(SS("Inbound to Outbound", "routeIn2Out"));
            d.Add(5, list);

            //{ 6, "policy"},
            list = NewList();
            list.Add(SS(I18N("Default"), "policyDefault"));
            d.Add(6, list);

            //{ 7, "inbound"},
            list = NewList();
            list.Add(SSS("HTTP", "inHTTP", "http"));
            list.Add(SSS("SOCKS", "inSocks", "socks"));
            list.Add(SSS("Shadowsocks", "inSS", "shadowsocks"));
            list.Add(SSS("VMess", "inVmess", "vmess"));
            list.Add(SSS("Dokodemo-door", "inDoko", "dokodemo-door"));
            d.Add(7, list);

            //{ 8, "outbound"},
            list = NewList();
            list.Add(SSS("VMess", "outVmess", "vmess"));
            list.Add(SSS("Shadowsocks", "outSS", "shadowsocks"));
            list.Add(SSS("SOCKS", "outSocks", "socks"));
            list.Add(SSS("Freedom", "outFree", "freedom"));
            list.Add(SSS("Black hole", "outBlackHole", "blackhole"));
            d.Add(8, list);

            //{ 9, "transport"},
            list = NewList();
            list.Add(SS(I18N("Default"), "transDefault"));
            d.Add(9, list);

            //{ 10,"v2raygcon" },
            list = NewList();
            list.Add(SS(I18N("Default"), "v2raygcon"));
            list.Add(SS(I18N("Import"), "vgcImport"));
            d.Add(10, list);

            //{ 11,"inboundDetour"}, 
            list = NewList();
            list.Add(SS(I18N("Default"), "inDtrDefault"));
            d.Add(11, list);

            //{ 12,"outboundDetour"}, outDtrDefault
            list = NewList();
            list.Add(SS(I18N("Default"), "outDtrDefault"));
            list.Add(SS("Freedom", "outDtrFreedom"));
            d.Add(12, list);

            return d;
        }

        // servers summary
        public static Dictionary<string, string[]> servInfoKeys = ServInfoKeysHelper();
        static Dictionary<string, string[]> ServInfoKeysHelper()
        {
            var streamType = "outbound.streamSettings.network";
            var type = "outbound.streamSettings.kcpSettings.header.type";
            var path = "outbound.streamSettings.wsSettings.path";
            var tls = "outbound.streamSettings.security";

            var ssIP = "outbound.settings.servers.0.address";
            var ssPort = "outbound.settings.servers.0.port";

            var vmessIP = "outbound.settings.vnext.0.address";
            var vmessPort = "outbound.settings.vnext.0.port";

            var keys = new Dictionary<string, string[]>();

            // ip,port,tls, path,streamType,type
            keys.Add("shadowsocks", new string[] {
                ssIP,ssPort,tls,path,streamType,type});

            keys.Add("vmess", new string[] {
                vmessIP,vmessPort,tls,path,streamType,type});

            return keys;
        }
    }
}
