using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

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
