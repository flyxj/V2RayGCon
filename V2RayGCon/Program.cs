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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Service.Notifier noty = Service.Notifier.Instance;

            Application.Run();
        }
    }
}
