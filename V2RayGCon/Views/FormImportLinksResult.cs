using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Views
{
    public partial class FormImportLinksResult : Form
    {
        List<string[]> results;
        List<string> linksCache;
        public FormImportLinksResult(List<string[]> importResults)
        {
            InitializeComponent();
            results = importResults;
            linksCache = new List<string>();
            this.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormImportLinksResult_Shown(object sender, EventArgs e)
        {
            lvResult.Items.Clear();
            int count = 1;
            foreach(var result in results)
            {
                result[0] = count.ToString();
                lvResult.Items.Add(new ListViewItem(result));
                count++;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Lib.Utils.RunAsSTAThread(()=> {
                MessageBox.Show(
                Lib.Utils.CopyToClipboard(string.Join(Environment.NewLine, linksCache)) ?
                I18N("CopySuccess") :
                I18N("CopyFail"));
            });
        }

        private void lvResult_Click(object sender, EventArgs e)
        {
            linksCache = new List<string>();
            var items = lvResult.SelectedItems;
            foreach (ListViewItem item in items)
            {
                linksCache.Add(string.Format(
                    "{0}.{1}",item.SubItems[0].Text,
                    item.SubItems[1].Text));
            }

            Debug.WriteLine("selected:{0} ", linksCache.Count);
        }
    }
}
