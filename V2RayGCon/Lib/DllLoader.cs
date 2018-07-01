using System;
using System.Runtime.InteropServices;

namespace V2RayGCon.Lib
{
    static class DllLoader
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        /*
         * Suppose we call SetProcessDpiAwareness from Shcore.dll.
         * 
         * Define a delegate first.
         * [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
         * private delegate int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);
         * 
         * Then parameter lamda should look like this:
         * (method) => ((SetProcessDpiAwareness)method).Invoke(0);  // PROCESS_DPI_AWARENESS=0
         */
        public static bool CallMethod(IntPtr pDll, string methodName, Type methodType, Action<Delegate> lamda)
        {
            var method = GetMethod(pDll, methodName, methodType);
            if (method != null)
            {
                lamda(method);
                return true;
            }
            return false;
        }

        public static Delegate GetMethod(IntPtr pDll, string methodName, Type methodType)
        {
            if (pDll == IntPtr.Zero)
            {
                return null;
            }

            IntPtr pMethod = GetProcAddress(pDll, methodName);
            if (pMethod != IntPtr.Zero)
            {
                return Marshal.GetDelegateForFunctionPointer(pMethod, methodType);
            }

            return null;
        }
    }
}
