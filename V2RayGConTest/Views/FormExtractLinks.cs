using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace V2RayGConTest.Views
{
    public partial class FormExtractLinks : Form
    {
        public FormExtractLinks()
        {
            InitializeComponent();
        }

        private void btnSS_Click(object sender, EventArgs e)
        {
            var content = rTboxInput.Text;
            var output = new List<string>();

            var pattern = V2RayGCon.Lib.Utils.GenPattern(
                V2RayGCon.Model.Data.Enum.LinkTypes.ss);
            foreach (Match m in Regex.Matches(content, pattern, RegexOptions.IgnoreCase))
            {
                string link = m.Value.Substring(1);
                output.Add(link);
            }

            labelTotal.Text = output.Count.ToString();

            rTboxOutput.Text = string.Join("\r\n", output);

        }
    }
}
