using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using static V2RayGCon.Lib.UI;

namespace V2RayGCon.Test
{
    [TestClass]
    public class UITest
    {

        public UITest()
        {

        }

        [TestMethod]
        public void UpdateControlOnDemandTest()
        {
            TextBox box = new TextBox();
            box.Text = "abc";
            UpdateControlOnDemand<TextBox, string>(box, "def");
            Assert.AreEqual("def", box.Text);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                UpdateControlOnDemand<TextBox, int>(box, 123);
            });
        }

        [TestMethod]
        public void CheckControlValueTypeMatchTest()
        {
            Assert.ThrowsException<ArgumentException>(
                () => CheckControlValueTypeMatch(
                    nameof(TextBox), typeof(int)));

            Assert.ThrowsException<ArgumentException>(
                () => CheckControlValueTypeMatch(
                    nameof(Panel), typeof(int)));

            Assert.AreEqual(true, CheckControlValueTypeMatch(
                nameof(TextBox), typeof(string)));
        }
    }
}
