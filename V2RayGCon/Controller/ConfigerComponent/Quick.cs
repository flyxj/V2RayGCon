using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace V2RayGCon.Controller.ConfigerComponet
{
    class Quick : Model.BaseClass.ConfigerComponent
    {
        Service.Cache cache;

        public Quick(Button skipCN, Button mtProto)
        {
            cache = Service.Cache.Instance;

            skipCN.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    InsertSkipCNSite();
                });
            };

            mtProto.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    InsertMTProto();
                });
            };
        }

        #region public method
        public override void Update(JObject config)
        {
        }
        #endregion

        #region private method
        void InsertMTProto()
        {
            var mtproto = cache.tpl.LoadTemplate("dtrMTProto") as JObject;

            foreach (string key in new string[] {
                    "inboundDetour",
                    "outboundDetour",
                    "routing",
                })
            {
                var part = Lib.Utils.ExtractJObjectPart(mtproto, key);
                if (Lib.Utils.Contain(container.config, part))
                {
                    try
                    {
                        Lib.Utils.RemoveKeyFromJObject(mtproto, key);
                    }
                    catch (KeyNotFoundException) { }
                }
            }
            var user0 = Lib.Utils.GetKey(mtproto, "inboundDetour.0.settings.users.0");
            if (user0 != null && user0 is JObject)
            {
                user0["secret"] = Lib.Utils.RandomHex(32);
            }

            Lib.Utils.CombineConfig(ref container.config, mtproto);
        }

        void InsertSkipCNSite()
        {
            var c = JObject.Parse(@"{}");

            var dict = new Dictionary<string, string> {
                    { "dns","dnsCFnGoogle" },
                    { "routing","routeCNIP" },
                    { "outboundDetour","outDtrFreedom" },
                };

            foreach (var item in dict)
            {
                var tpl = Lib.Utils.CreateJObject(item.Key);
                var value = cache.tpl.LoadExample(item.Value);
                tpl[item.Key] = value;

                if (!Lib.Utils.Contain(container.config, tpl))
                {
                    c[item.Key] = value;
                }
            }

            Lib.Utils.CombineConfig(ref container.config, c);
        }
        #endregion
    }
}
