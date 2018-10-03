using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        #region private method
        private static RegistryKey GetRegKey(bool writeable = false)
        {
            const string subkey = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            var key = Registry.CurrentUser.OpenSubKey(subkey, writeable);
            if (key == null)
            {
                throw new KeyNotFoundException("Reg key not found!");
            }
            return key;
        }

        #endregion

        #region public method
        public static void ClearProxy()
        {
            SetProxy("", false);
        }

        public static void SetProxy(string proxyhost, bool proxyEnabled = true)
        {
            using (var key = GetRegKey(true))
            {
                key.SetValue("ProxyServer", proxyhost, RegistryValueKind.String);
                key.SetValue("ProxyEnable", proxyEnabled ? 1 : 0, RegistryValueKind.DWord);
                key.Close();
            }

            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing IP to realy update
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public static string GetProxyUrl()
        {
            var value = "";
            using (var key = GetRegKey(false))
            {
                value = key.GetValue("ProxyServer", "").ToString();
                key.Close();
            }
            return value;
        }

        public static bool GetProxyState()
        {
            var value = "0";
            using (var key = GetRegKey(false))
            {
                value = key.GetValue("ProxyEnable", "0").ToString();
                key.Close();
            }
            return value == "1";
        }
        #endregion
    }
}
