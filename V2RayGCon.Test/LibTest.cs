using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;
using static V2RayGCon.Test.Resource.StringResource;


namespace V2RayGCon.Test
{
    [TestClass]
    public class LibTest
    {
        [TestMethod]
        public void GetValue_GetBoolFromString_ThrowException()
        {
            var json = JObject.Parse(resData("config_min"));
            Assert.ThrowsException<FormatException>(
                () => Lib.Utils.GetValue<bool>(json, "log.loglevel"));
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
        public void GetLatestVersion()
        {
            string version = Lib.Utils.GetLatestVersion();
            Assert.AreNotEqual(string.Empty, version);
        }

    }
}
