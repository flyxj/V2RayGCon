using System;
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
        [STAThread]
        static void Main()
        {
            // 这个逻辑有点问题，暂时不开启
            //var mutex = new System.Threading.Mutex(true, "UniqueAppId", out bool result);
            //if (!result)
            //{
            //    if (!Lib.Utils.Confirm(I18N("WarnMultipleInstance")))
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    GC.KeepAlive(mutex);
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Service.Notifier noty = new Service.Notifier();

            Application.Run();
        }
    }
}
