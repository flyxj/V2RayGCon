using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace V2RayGCon.Test
{
    [TestClass]
    public class CacheTest
    {
        V2RayGCon.Service.Cache cache;

        public CacheTest()
        {
            cache = V2RayGCon.Service.Cache.Instance;
        }

        [TestMethod]
        public void HTMLFailTest()
        {
            Assert.ThrowsException<WebException>(() =>
            {
                cache.html.GetCache("");
            });
        }

        [DataTestMethod]
        [DataRow("http://suo.im/4CTgF5")]
        [DataRow("http://suo.im/4CTgF5,http://suo.im/5lp5PJ")]
        [DataRow("http://suo.im/4CTgF5,http://suo.im/5lp5PJ,http://suo.im/4JJwhs")]
        public void HTMLNormalTest(string rawData)
        {
            var data = rawData.Split(',');
            var urls = new List<string>();
            var len = data.Length;
            for (var i = 0; i < 1000; i++)
            {
                urls.Add(data[i % len]);
            }
            var html = cache.html;

            try
            {
                Lib.Utils.ExecuteInParallel<string, string>(urls, (url) =>
                {
                    return html.GetCache(url);
                });
                html.RemoveAllCache();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [DataTestMethod]
        [DataRow(@"inTpl.domainOverride", @"['http','tls']")]
        public void LoadExampleTest(string key, string expect)
        {
            var v = cache.LoadExample(key);
            var e = JToken.Parse(expect);
            Assert.AreEqual(true, JToken.DeepEquals(v, e));

        }

        [DataTestMethod]
        [DataRow(@"vgc", @"{'alias': '','description': ''}")]
        public void LoadTplTest(string key, string expect)
        {
            var v = cache.LoadTemplate(key);
            var e = JObject.Parse(expect);
            Assert.AreEqual(true, JObject.DeepEquals(v, e));
        }

        [TestMethod]
        public void LoadMinConfigTest()
        {
            var min = cache.LoadMinConfig();
            var v = Lib.Utils.GetValue<string>(min, "log.loglevel");
            Assert.AreEqual<string>("warning", v);
        }
    }
}
