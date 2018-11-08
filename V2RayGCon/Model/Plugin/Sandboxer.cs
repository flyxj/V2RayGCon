using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

// https://docs.microsoft.com/en-us/dotnet/framework/misc/how-to-run-partially-trusted-code-in-a-sandbox

namespace V2RayGCon.Model.Plugin
{
    //The Sandboxer class needs to derive from MarshalByRefObject so that we can create it in another   
    // AppDomain and refer to it from the default AppDomain.  
    class Sandboxer : MarshalByRefObject, PluginContracts.IPlugin
    {
        PluginContracts.IPlugin auxPlugin = null;

        public Sandboxer() { }

        #region properties
        public string Name => auxPlugin.Name;
        public string Version => auxPlugin.Version;
        public string Description => auxPlugin.Description;
        #endregion

        #region public method
        public string[] GetFuncsName() => auxPlugin.GetFuncsName();

        public void Do(string funcName) => auxPlugin.Do(funcName);

        public bool CreatePartiallyTrustedPluginInstance(Type pluginType)
        {

            auxPlugin = CreatePluginInstance(pluginType);
            return auxPlugin != null;
        }

        public void ExecuteUntrustedCode(string assemblyName, string typeName, string entryPoint, Object[] parameters)
        {
            //Load the MethodInfo for a method in the new Assembly. This might be a method you know, or   
            //you can use Assembly.EntryPoint to get to the main function in an executable.  
            MethodInfo target = Assembly.Load(assemblyName).GetType(typeName).GetMethod(entryPoint);
            try
            {
                //Now invoke the method.  
                bool retVal = (bool)target.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                // When we print informations from a SecurityException extra information can be printed if we are   
                //calling it with a full-trust stack.  
                (new PermissionSet(PermissionState.Unrestricted)).Assert();
                Console.WriteLine("SecurityException caught:\n{0}", ex.ToString());
                CodeAccessPermission.RevertAssert();
                Console.ReadLine();
            }
        }
        #endregion

        #region static method
        /// <summary>
        /// return null if fail or param is null
        /// </summary>
        /// <param name="pluginType"></param>
        /// <returns></returns>
        public static Model.Plugin.PluginContracts.IPlugin CreatePluginInstance(
            Type pluginType)
        {
            if (pluginType == null)
            {
                return null;
            }

            try
            {
                return (Model.Plugin.PluginContracts.IPlugin)
                    Activator.CreateInstance(pluginType);
            }
            catch
            {
                Lib.Sys.Log.Error("Load plugin fail.");
            }

            return null;
        }

        /// <summary>
        /// return null if fail to load
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Type LoadPluginTypeFromFile(string fileName)
        {
            return LoadPluginTypeFromAssembly(
                         LoadAssemblyFromFile(fileName));
        }
        #endregion

        #region private method
        #endregion

        #region static private method

        /// <summary>
        /// return null if fail or param is null
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        static Type LoadPluginTypeFromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            var pluginType = typeof(Model.Plugin.PluginContracts.IPlugin);
            Type[] types = null;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exceptions)
            {
                foreach (var e in exceptions.LoaderExceptions)
                {
                    var info = e;
                }
            }

            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsInterface
                    && !type.IsAbstract
                    && type.GetInterface(pluginType.Name) != null)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// return null if fail, return null if param is null
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static Assembly LoadAssemblyFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            try
            {
                AssemblyName an = AssemblyName.GetAssemblyName(fileName);
                return Assembly.Load(an);
            }
            catch { }
            return null;
        }
        #endregion
    }
}