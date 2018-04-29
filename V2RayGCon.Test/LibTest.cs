using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;


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
    }
}
