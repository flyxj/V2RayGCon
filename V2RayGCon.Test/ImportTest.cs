using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;
using static V2RayGCon.Test.Resource.StringResource;


namespace V2RayGCon.Test
{
    [TestClass]
    public class ImportTest
    {
        [TestMethod]
        public void DetectJArrayTest()
        {
            var config = JObject.Parse(resData("config_def"));
            var domainOverride = Lib.Utils.GetKey(config, "inTpl.domainOverride");
            var isArray = domainOverride is JArray;
            var isObject = domainOverride is JObject;

            string[] list =null;
            if (isArray)
            {
                list = domainOverride.ToObject<string[]>();
            }

            Assert.AreEqual(true, isArray);
            Assert.AreEqual(false, isObject);
            Assert.AreNotEqual(null, list);
        }

        [DataTestMethod]
        [DataRow(@"{}", @"{}", @"{}")]
        [DataRow(@"{a:'123',b:null}", @"{a:null,b:'123'}", @"{a:null,b:'123'}")]
        [DataRow(@"{a:[1,2],b:{}}", @"{a:[3],b:{a:[1,2,3]}}", @"{a:[3,2],b:{a:[1,2,3]}}")]
        public void MergeJson(string first, string second, string expect)
        {
            var v = Lib.Utils.MergeJson(JObject.Parse(first), JObject.Parse(second));
            var e = JObject.Parse(expect);

            Assert.AreEqual(true, JObject.DeepEquals(v, e));
        }

    }
}
