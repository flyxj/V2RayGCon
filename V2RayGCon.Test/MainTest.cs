using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;
using static V2RayGCon.Test.Resource.StringResource;


namespace V2RayGCon.Test
{
    [TestClass]
    public class LibTest
    {
        [DataTestMethod]
        [DataRow("aaaaaa", 0, "...")]
        [DataRow("aaaaaaaaa", 5, "aa...")]
        [DataRow("aaaaaa", 3, "...")]
        [DataRow("aaaaaa", -1, "...")]
        [DataRow("", 100, "")]
        public void CutStrTest(string org, int len, string expect)
        {
            var cut = Lib.Utils.CutStr(org, len);
            Assert.AreEqual(expect, cut);
        }

        [DataTestMethod]
        [DataRow(@"{}", "")]
        [DataRow(@"{v2raygcon:{env:['1','2']}}", "")]
        [DataRow(@"{v2raygcon:{env:{a:'1',b:2}}}", "a:1,b:2")]
        [DataRow(@"{v2raygcon:{env:{a:'1',b:'2'}}}", "a:1,b:2")]
        public void GetEnvVarsFromConfigTest(string json, string expect)
        {
            var j = JObject.Parse(json);
            var env = Lib.Utils.GetEnvVarsFromConfig(j);
            var strs = env.OrderBy(p => p.Key).Select(p => p.Key + ":" + p.Value);
            var r = string.Join(",", strs);

            Assert.AreEqual(expect, r);
        }


        [TestMethod]
        public void CreateDeleteAppFolderTest()
        {
            var appFolder = Lib.Utils.GetAppDataFolder();
            Assert.AreEqual(false, string.IsNullOrEmpty(appFolder));

            // do not run these tests 
            // Lib.Utils.CreateAppDataFolder();
            // Assert.AreEqual(true, Directory.Exists(appFolder));
            // Lib.Utils.DeleteAppDataFolder();
            // Assert.AreEqual(false, Directory.Exists(appFolder));
        }

        [DataTestMethod]
        [DataRow(@"{}", "a", "abc", @"{'a':'abc'}")]
        [DataRow(@"{'a':{'b':{'c':1234}}}", "a.b.c", "abc", @"{'a':{'b':{'c':'abc'}}}")]
        public void SetValueStringTest(string json, string path, string value, string expect)
        {
            var r = JObject.Parse(json);
            var e = JObject.Parse(expect);
            Lib.Utils.SetValue<string>(r, path, value);
            Assert.AreEqual(true, JObject.DeepEquals(e, r));
        }

        [DataTestMethod]
        [DataRow(@"{}", "a", 1, @"{'a':1}")]
        [DataRow(@"{'a':{'b':{'c':1234}}}", "a.b.c", 5678, @"{'a':{'b':{'c':5678}}}")]
        public void SetValueIntTest(string json, string path, int value, string expect)
        {
            var r = JObject.Parse(json);
            var e = JObject.Parse(expect);
            Lib.Utils.SetValue<int>(r, path, value);
            Assert.AreEqual(true, JObject.DeepEquals(e, r));
        }

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

            var core = new Model.BaseClass.CoreServer();
            var version = core.GetCoreVersion();

            if (core.IsExecutableExist())
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
            var json = Service.Cache.Instance.
                tpl.LoadMinConfig();
            Assert.AreEqual(default(bool), GetValue<bool>(json, "log.loglevel"));
        }

        [TestMethod]
        public void GetValue_GetStringNotExist_ReturnNull()
        {
            var json = Service.Cache.Instance.
                tpl.LoadMinConfig();
            Assert.AreEqual(string.Empty, GetValue<string>(json, "log.keyNotExist"));
        }

        [TestMethod]
        public void GetValue_KeyNotExist_ReturnDefault()
        {
            var json = Service.Cache.Instance.
                tpl.LoadMinConfig();
            var value = Lib.Utils.GetValue<int>(json, "log.key_not_exist");
            Assert.AreEqual(default(int), value);
        }

        [DataTestMethod]
        [DataRow("config_min")]
        [DataRow("config_tpl")]
        [DataRow("config_def")]
        public void ConfigResource_Validate(string filename)
        {
            try
            {
                JObject.Parse(StrConst(filename));
            }
            catch
            {
                Assert.Fail();
            }
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
            Assert.ThrowsException<KeyNotFoundException>(() => StrConst(key));
        }

        [DataTestMethod]
        [DataRow("Executable", "v2ray.exe")]
        public void resData_Test(string key, string expect)
        {
            Assert.AreEqual<string>(expect, StrConst(key));
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
