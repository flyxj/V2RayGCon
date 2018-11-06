namespace V2RayGCon.Model.Plugin
{
    // https://code.msdn.microsoft.com/windowsdesktop/Creating-a-simple-plugin-b6174b62
    public class PluginContracts
    {
        public interface IPlugin
        {
            string Name { get; }
            string Version { get; }
            string Description { get; }

            string[] GetFuncsName();
            void Do(string funcName);
        }

        public interface IApi
        {
            void MessageBox(string content);
            void Log(string content);
        }
    }
}
