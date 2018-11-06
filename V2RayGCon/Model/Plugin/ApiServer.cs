using System.Diagnostics;
using System.Threading.Tasks;

namespace V2RayGCon.Model.Plugin
{
    public class ApiServer : PluginContracts.IApi
    {
        public void Log(string content)
        {
            Debug.WriteLine("Plugin api server: ", content);
        }

        public void MessageBox(string content)
        {
            Task.Factory.StartNew(() =>
            {
                System.Windows.Forms.MessageBox.Show(
                    content,
                    "Plugin api server");
            });
        }
    }
}
