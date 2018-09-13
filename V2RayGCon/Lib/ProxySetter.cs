using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace V2RayGCon.Lib
{
    // https://stackoverflow.com/questions/197725/programmatically-set-browser-proxy-settings-in-c-sharp
    class ProxySetter
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;


        const string userRoot = "HKEY_CURRENT_USER";
        const string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
        const string keyName = userRoot + "\\" + subkey;

        public static void setProxy(string proxyhost, bool proxyEnabled)
        {

            Registry.SetValue(keyName, "ProxyServer", proxyhost);
            Registry.SetValue(keyName, "ProxyEnable", proxyEnabled ? 1 : 0);

            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing IP to realy update
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public static string getProxyUrl()
        {
            return Registry.GetValue(keyName, "ProxyServer", "").ToString();
        }

        public static bool getProxyState()
        {
            var state = Registry.GetValue(keyName, "ProxyEnable", 0);
            return Lib.Utils.Str2Int(state.ToString()) == 1;
        }
    }
}
