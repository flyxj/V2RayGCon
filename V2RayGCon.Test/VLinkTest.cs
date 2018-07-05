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
    public class VLink
    {
        [DataTestMethod]
        [DataRow(@"{}", @"{}", @"{}")]
        [DataRow(@"{a:'123',b:null}", @"{a:null,b:'123'}", @"{a:null,b:'123'}")]
        [DataRow(@"{a:[1,2],b:{}}", @"{a:[3],b:{a:[1,2,3]}}", @"{a:[1,2,3],b:{a:[1,2,3]}}")]
        public void MergeJson(string first, string second, string expect)
        {
            var v = Lib.Utils.MergeJson(JObject.Parse(first), JObject.Parse(second));
            var e = JObject.Parse(expect);

            Assert.AreEqual(true, JObject.DeepEquals(v, e));
        }

        [TestMethod]
        public void EncodeVLink()
        {
            try
            {
                Lib.VLinkCodec.EncodeLink(null, null);
            }
            catch
            {
                Assert.Fail("Encode(null,null) should success but fail");
            }
        }

        [TestMethod]
        public void DecodeVLink()
        {
            void test<T>(string vlink, int timeout = -1) where T : System.Exception
            {
                Assert.ThrowsException<T>(() => Lib.VLinkCodec.DecodeLink(vlink, timeout));
            }

            // invalid url {"u":"569GaT"}
            test<System.Net.WebException>("v://eyJ1IjoiNTY5R2FUIn0K");

            // invalid url '{"u":"569GaT,aaaa,bbbb,cccc"}'
            test<System.Net.WebException>("v://eyJ1IjoiNTY5R2FULGFhYWEsYmJiYixjY2NjIn0K");

            // base64 decode fail
            test<FormatException>("v://***");

            // timeout
            test<System.Net.WebException>("v://eyJ1IjoiaHR0cDovL3N1by5pbS81NjlHYVQifQ==", 1);

            // decode error
            test<Newtonsoft.Json.JsonReaderException>("v://aa");
        }


    }
}
