using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace V2RayGCon.Test
{
    [TestClass]
    public class FormMainTest
    {
        V2RayGCon.Service.Cache cache;

        public FormMainTest()
        {
            cache = V2RayGCon.Service.Cache.Instance;
        }

        [TestMethod]
        public void ServerItemListSerializeTest()
        {
            var servList = new List<Model.Data.ServerItem>();
            // servList.Add(new Model.Data.ServerItem());

            var sl = JsonConvert.SerializeObject(servList);
            var expect = "";

            var result = sl == expect;

            // developing
            // Assert.AreEqual(true, result);

        }

    }
}
