using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using static V2RayGCon.Lib.StringResource;
using static V2RayGCon.Lib.Utils;
using static V2RayGCon.Test.Resource.StringResource;


namespace V2RayGCon.Test
{
    [TestClass]
    public class VLink
    {
        [DataTestMethod]
        [DataRow(@"{}", @"{}",@"{}")]
        [DataRow(@"{a:'123',b:null}", @"{a:null,b:'123'}", @"{a:null,b:'123'}")]
        [DataRow(@"{a:[1,2],b:{}}", @"{a:[3],b:{a:[1,2,3]}}", @"{a:[1,2,3],b:{a:[1,2,3]}}")]
        public void MergeJson(string first,string second, string expect)
        {
            var v = Lib.Utils.MergeJson(JObject.Parse(first), JObject.Parse(second));
            var e = JObject.Parse(expect);
            
            Assert.AreEqual(true, JObject.DeepEquals(v,e));
        }

     

    }
}
