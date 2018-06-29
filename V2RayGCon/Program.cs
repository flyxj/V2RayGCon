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

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        // PROCESS_DPI_AWARENESS = 0/1/2 None/SystemAware/PerMonitorAware
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // load Shcore.dll and get high resolution support
            IntPtr pDll = Lib.DllLoader.LoadLibrary(@"Shcore.DLL");
            Lib.DllLoader.CallMethod(
                pDll,
                @"SetProcessDpiAwareness",
                typeof(SetProcessDpiAwareness),
                (method) => ((SetProcessDpiAwareness)method).Invoke(2));

            Service.Notifier noty = Service.Notifier.Instance;

            Application.Run();

            Lib.DllLoader.FreeLibrary(pDll);
        }
    }
}
