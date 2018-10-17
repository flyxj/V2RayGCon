using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace V2RayGCon.Service
{
    class respData
    {
        public bool success;
        public string data;
    }

    class servInfo
    {
        public bool isRunning;
        public string sha, title;

        public servInfo(Controller.CoreServerCtrl servCtrl)
        {
            isRunning = servCtrl.isServerOn;
            title = servCtrl.GetTitle();
            sha = Lib.Utils.SHA256(servCtrl.config);
        }
    }

    public class Cmder : Model.BaseClass.SingletonService<Cmder>
    {
        Setting setting;
        Servers servers;

        Dictionary<string, Func<string, string>> cmds =
            new Dictionary<string, Func<string, string>>();

        Cmder() { }

        #region properties

        #endregion

        #region public method
        public void Run(
            Setting setting,
            Servers servers,
            PacServer pacServer)
        {
            this.servers = servers;
            this.setting = setting;
            pacServer.RegistPostRequestHandler(RequestDispatcher);
            InitCmdHandler();
        }

        public void Cleanup()
        {

        }
        #endregion

        #region commands
        void InitCmdHandler()
        {
            RegistCmd("echo", s => s);
            RegistCmd("getServersInfo", GetServersInfo);
        }

        string GetServersInfo(string param)
        {
            var list = servers.GetServerList()
                .Select(s => new servInfo(s))
                .ToList();

            return JsonConvert.SerializeObject(list);
        }
        #endregion

        #region private method
        void RegistCmd(string name, Func<string, string> func)
        {
            if (string.IsNullOrEmpty(name) || cmds.ContainsKey(name))
            {
                throw new ArgumentException($"Command already existed: {name}");
            }

            cmds[name] = func;
        }

        bool TryParseJObject(string jsonString, out JObject json)
        {
            try
            {
                json = JObject.Parse(jsonString);
                return true;
            }
            catch { }
            json = null;
            return false;
        }

        JObject GetPostData(HttpListenerRequest request)
        {
            string text;
            using (var reader = new StreamReader(
                request.InputStream,
                request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }

            try
            {
                return JObject.Parse(text);
            }
            catch { }
            return null;
        }

        string GenRespose(bool success, string data)
        {
            var response = new respData
            {
                success = success,
                data = data ?? "",
            };
            return JsonConvert.SerializeObject(response);
        }

        string RequestDispatcher(HttpListenerRequest request)
        {
            var post = GetPostData(request);
            var cmd = Lib.Utils.GetValue<string>(post, "cmd");
            var param = Lib.Utils.GetValue<string>(post, "param");

            if (post == null
                || string.IsNullOrEmpty(cmd)
                || !cmds.ContainsKey(cmd)
                || param == null)
            {
                return GenRespose(false, "Bad post request params!");
            }

            var result = cmds[cmd](param);
            return GenRespose(result != null, result ?? "");
        }
        #endregion
    }
}
