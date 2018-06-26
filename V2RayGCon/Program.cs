using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#region Support CallerMemberName on .net 4.0
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerMemberNameAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerFilePathAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class CallerLineNumberAttribute : Attribute
    {
    }
}
#endregion


namespace V2RayGCon

{
    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // load dll and call shcore.dll (if exist)
            IntPtr pDll = NativeMethods.LoadLibrary(@"Shcore.DLL");
            if (pDll != IntPtr.Zero)
            {
                IntPtr pSetProcessDpiAwareness = NativeMethods.GetProcAddress(pDll, "SetProcessDpiAwareness");
                if(pSetProcessDpiAwareness != IntPtr.Zero)
                {
                    SetProcessDpiAwareness setProcessDpiAwareness = 
                        (SetProcessDpiAwareness)Marshal.GetDelegateForFunctionPointer(
                            pSetProcessDpiAwareness,
                            typeof(SetProcessDpiAwareness));
                    setProcessDpiAwareness((int)DpiAwareness.PerMonitorAware);
                }
            }

            Service.Notifier noty = Service.Notifier.Instance;

            Application.Run();

            if (pDll != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(pDll);
            }
        }
    }
}
