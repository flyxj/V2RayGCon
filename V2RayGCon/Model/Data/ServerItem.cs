using Newtonsoft.Json;
using System;

namespace V2RayGCon.Model.Data
{
    public class ServerItem
    {
        public bool isOn, isInjectEnv, isInjectImport;
        public string summary, inboundIP, config;
        public int inboundOverwriteType, inboundPort;

        [JsonIgnore]
        public Model.BaseClass.CoreServer server;

        public ServerItem()
        {
            isOn = false;
            isInjectEnv = false;
            isInjectImport = false;

            summary = string.Empty;
            config = string.Empty;
            inboundOverwriteType = 0;
            this.SetInboundAddr("127.0.0.1:1080");

            server = null;
        }

        #region public method

        public string GetInboundAddr()
        {
            return string.Format("{0}:{1}", this.inboundIP, this.inboundPort);
        }

        public void SetInboundAddr(string ipAndPort)
        {
            Lib.Utils.TryParseIPAddr(ipAndPort, out string ip, out int port);
            this.inboundIP = ip;
            this.inboundPort = port;
        }

        public string GetInboundOverwriteTypeNameByIndex(int index)
        {
            var types = Model.Data.Table.inboundOverwriteTypes;
            if (index > 0 && index < 3)
            {
                return types[index];
            }
            return types[0];
        }

        public int GetInboundOverwriteTypeIndexByName(string name)
        {
            var types = Model.Data.Table.inboundOverwriteTypes;
            return Math.Max(0, Lib.Utils.GetIndexIgnoreCase(types, name));
        }
        #endregion
    }
}
