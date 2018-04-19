using System.Collections.Generic;

namespace V2RayGCon.Model
{
    class Table
    {
        public static Dictionary<int, string> configSections => _configSections;
        static Dictionary<int, string> _configSections = new Dictionary<int, string>
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
        public static int sectionSeparator => _sectionSeparator;
        static int _sectionSeparator = 11;

        public static Dictionary<int, string> ssrMethods => _ssrMethods;
        static Dictionary<int, string> _ssrMethods = new Dictionary<int, string>
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

        public static Dictionary<int, string> streamSecurity => _streamSecurity;
        static Dictionary<int, string> _streamSecurity = new Dictionary<int, string>
        {
            { 0, "none" },
            { 1, "tls" },
        };
    }
}
