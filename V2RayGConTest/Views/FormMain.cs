using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace V2RayGConTest.Views
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void base64ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Func<string, string, string[]> ss = Lib.Utils.SS;

            List<string[]> a = new List<string[]> {
                ss( "", ""),
                ss( "a", "YQ=="),
                ss( "ab", "YWI="),
                ss( "abc", "YWJj"),
                ss( "abcd", "YWJjZA=="),
                ss( "abcde", "YWJjZGU="),
                ss( "abcdef", "YWJjZGVm"),
                ss( "abcdefg", "YWJjZGVmZw=="),
                ss( "abcdefgh", "YWJjZGVmZ2g="),
                ss( "abcdefghi", "YWJjZGVmZ2hp"),
            };

            for (var i = 0; i < a.Count; i++)
            {
                var b64 = a[i][1];
                var cut = b64.Replace("=", "");
                var expected = a[i][0];
                var db64 = V2RayGCon.Lib.Utils.Base64Decode(b64);
                var dcut = V2RayGCon.Lib.Utils.Base64Decode(cut);
                var encode = V2RayGCon.Lib.Utils.Base64Encode(expected);

                Console.Write("Test {0} ", i);

                if (db64.Equals(dcut) && expected.Equals(db64))
                {
                    Console.Write("b64decode pass ");
                }
                else
                {
                    Console.Write("b64decode fail ");
                }

                if (encode.Equals(b64))
                {
                    Console.WriteLine("b64encode pass");
                }
                else
                {
                    Console.WriteLine("b64encode fail");
                }
            }
        }

        private void patternTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormExtractLinks();
            f.Show();
        }

        private void getLinkBodyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] links = new string[] {
                "v2ray://abcde",
                "vmess://abcde",
                "ss://abcde",
                "htp://aaaaa",
            };

            string[] linkBodys = new string[] {
                "abcde",
                "abcde",
                "abcde",
                "aaaaa",
            };

            for (int i = 0; i < links.Length; i++)
            {
                var linkBody = V2RayGCon.Lib.Utils.GetLinkBody(links[i]);
                if (linkBody.Equals(linkBodys[i]))
                {
                    Debug.WriteLine("test {0} pass", i);
                }
                else
                {
                    Debug.WriteLine("test {0} fail", i);
                }

            }
        }
    }
}
