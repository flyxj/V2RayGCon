using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;
using static V2RayGCon.Test.Resource.StringResource;


namespace V2RayGCon.Test
{
    [TestClass]
    public class LibTest
    {
        [DataTestMethod]
        [DataRow(@"{'a':{'c':null},'b':1}", "a.b.c")]
        [DataRow(@"{'a':[0,1,2],'b':1}", "a.0")]
        [DataRow(@"{}", "")]
        public void RemoveKeyFromJsonFailTest(string json, string key)
        {
            // outboundDetour inboundDetour
            var j = JObject.Parse(json);
            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                RemoveKeyFromJObject(j, key);
            });
        }

        [DataTestMethod]
        [DataRow(@"{'a':{'c':null,'a':2},'b':1}", "a.c", @"{'a':{'a':2},'b':1}")]
        [DataRow(@"{'a':{'c':1},'b':1}", "a.c", @"{'a':{},'b':1}")]
        [DataRow(@"{'a':{'c':1},'b':1}", "a.b", @"{'a':{'c':1},'b':1}")]
        [DataRow(@"{'a':1,'b':1}", "c", @"{'a':1,'b':1}")]
        [DataRow(@"{'a':1,'b':1}", "a", @"{'b':1}")]
        public void RemoveKeyFromJsonNormalTest(string json, string key, string expect)
        {
            // outboundDetour inboundDetour
            var j = JObject.Parse(json);
            RemoveKeyFromJObject(j, key);
            var e = JObject.Parse(expect);
            Assert.AreEqual(true, JObject.DeepEquals(e, j));
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("1", "1")]
        [DataRow("1 , 2", "1,2")]
        [DataRow(",  ,  ,", "")]
        [DataRow(",,,  ,1  ,  ,2,  ,3,,,", "1,2,3")]
        public void Str2JArray2Str(string value, string expect)
        {
            var array = Lib.Utils.Str2JArray(value);
            var str = Lib.Utils.JArray2Str(array);
            Assert.AreEqual(expect, str);
        }

        [DataTestMethod]
        [DataRow("0", 0)]
        [DataRow("-1", -1)]
        [DataRow("str-1.234", 0)]
        [DataRow("-1.234str", 0)]
        [DataRow("-1.234", -1)]
        [DataRow("1.432", 1)]
        [DataRow("1.678", 2)]
        [DataRow("-1.678", -2)]
        public void Str2Int(string value, int expect)
        {
            Assert.AreEqual(expect, Lib.Utils.Str2Int(value));
        }

        [TestMethod]
        public void GetLocalCoreVersion()
        {

            var core = Service.Core.Instance;
            var version = core.GetCoreVersion();

            if (core.IsCoreExist())
            {
                Assert.AreNotEqual(string.Empty, version);
            }
            else
            {
                Assert.AreEqual(string.Empty, version);
            }
        }

        [TestMethod]
        public void GetValue_GetBoolFromString_ReturnDefault()
        {
            var json = JObject.Parse(resData("config_min"));
            Assert.AreEqual(default(bool), GetValue<bool>(json, "log.loglevel"));
        }

        [TestMethod]
        public void GetValue_GetStringNotExist_ReturnNull()
        {
            var json = JObject.Parse(resData("config_min"));
            Assert.AreEqual(string.Empty, GetValue<string>(json, "log.keyNotExist"));
        }

        [TestMethod]
        public void GetValue_KeyNotExist_ReturnDefault()
        {
            var json = JObject.Parse(resData("config_min"));
            var value = Lib.Utils.GetValue<int>(json, "log.key_not_exist");
            Assert.AreEqual(default(int), value);
        }

        [DataTestMethod]
        [DataRow("config_min")]
        [DataRow("config_tpl")]
        [DataRow("config_def")]
        public void ConfigResource_Validate(string filename)
        {
            var configMin = resData(filename);
            var json = Parse<JObject>(configMin);
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void Str2ListStr()
        {
            var testData = new Dictionary<string, int> {
                // string serial, int expectLength
                {"",0 },
                {",,,,",0 },
                {"1.1,2.2,,3.3,,,4.4.4,", 4},
            };

            foreach (var item in testData)
            {
                var len = Lib.Utils.Str2ListStr(item.Key).Count;
                Assert.AreEqual(item.Value, len);
            }

        }

        [DataTestMethod]
        [DataRow("this_resource_key_not_exist")]
        public void resData_ThrowExceptionWhenKeyNotExist(string key)
        {
            Assert.ThrowsException<KeyNotFoundException>(() => resData(key));
        }

        [DataTestMethod]
        [DataRow("Executable", "v2ray.exe")]
        public void resData_Test(string key, string expect)
        {
            Assert.AreEqual<string>(expect, resData(key));
        }

        [TestMethod]
        public void JsonParser_EmptyStringReturnNull()
        {
            var json = Parse<JToken>(string.Empty);
            Assert.IsNull(json);
        }

        [TestMethod]
        public void ExtractLinks_FromString()
        {
            // var content = testData("links");
            var content = "ss://ZHVtbXkwMA==";
            var links = Lib.Utils.ExtractLinks(content, Model.Data.Enum.LinkTypes.ss);
            var expact = "ss://ZHVtbXkwMA==";
            Assert.AreEqual(links.Count, 1);
            Assert.AreEqual(expact, links[0]);
        }

        [TestMethod]
        public void ExtractLinks_FromLinksTxt()
        {
            var content = testData("links");
            var links = Lib.Utils.ExtractLinks(content, Model.Data.Enum.LinkTypes.vmess);
            Assert.AreEqual(2, links.Count);
        }

        [TestMethod]
        public void ExtractLink_FromEmptyString_Return_EmptyList()
        {
            var content = "";
            var links = Lib.Utils.ExtractLinks(content, Model.Data.Enum.LinkTypes.vmess);
            Assert.AreEqual(0, links.Count);
        }

        [TestMethod]
        public void GetRemoteCoreVersions()
        {
            List<string> versions = Lib.Utils.GetCoreVersions();
            // Assert.AreNotEqual(versions, null);
            Assert.AreEqual(true, versions.Count > 0);
        }

        [TestMethod]
        public void GetVGCVersions()
        {
            var version = Lib.Utils.GetLatestVGCVersion();
            Assert.AreNotEqual(string.Empty, version);

        }

    }
}
