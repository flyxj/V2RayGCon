using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace V2RayGCon.Test
{
    [TestClass]
    public class CoreServerTest
    {
        // download v2ray-core into test folder first
        V2RayGCon.Model.BaseClass.CoreServer core;

        public CoreServerTest()
        {
            core = new Model.BaseClass.CoreServer();
        }

        [TestMethod]
        public void TestGetExecutablePath()
        {
            var exe = core.GetExecutablePath();
            Assert.AreEqual(false, string.IsNullOrEmpty(exe));
        }

        [TestMethod]
        public void TestIsExecutableExist()
        {
            var exist = core.IsExecutableExist();
            Assert.AreEqual(true, exist);
        }

        [TestMethod]
        public void TestGetCoreVersion()
        {
            var ver = core.GetCoreVersion();
            Assert.AreEqual(false, string.IsNullOrEmpty(ver));
        }


    }
}
