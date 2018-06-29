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
            Assert.AreEqual(GetValue<bool>(json, "log.loglevel"), default(bool));
        }

        [TestMethod]
        public void GetValue_KeyNotExist_ReturnDefault()
        {
            var json = JObject.Parse(resData("config_min"));
            var value = Lib.Utils.GetValue<int>(json, "log.key_not_exist");
            Assert.AreEqual(value, default(int));
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
        public void LoadString_ThrowExceptionWhenKeyNotExist(string key)
        {
            Assert.ThrowsException<KeyNotFoundException>(() => resData(key));
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
            Assert.AreEqual(links.Count, 2);
        }

        [TestMethod]
        public void ExtractLink_FromEmptyString_Return_EmptyList()
        {
            var content = "";
            var links = Lib.Utils.ExtractLinks(content, Model.Data.Enum.LinkTypes.vmess);
            Assert.AreEqual(links.Count, 0);
        }

        [TestMethod]
        public void GetRemoteCoreVersions()
        {
            List<string> versions = Lib.Utils.GetCoreVersions();
            // Assert.AreNotEqual(versions, null);
            Assert.AreEqual(versions.Count > 0, true);
        }

        [TestMethod]
        public void GetVGCVersions()
        {
            var version = Lib.Utils.GetLatestVGCVersion();
            Assert.AreNotEqual(string.Empty, version);

        }

    }
}
