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
            foreach (var result in results)
            {
                result[0] = count.ToString();
                lvResult.Items.Add(new ListViewItem(result));
                count++;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            CopyToClipboard(linksCache);
        }

        string ConvertListViewItemToString(ListViewItem item)
        {
            var count = item.SubItems.Count;
            var list = new List<string>();
            for (var i = 0; i < count; i++)
            {
                list.Add(item.SubItems[i].Text);
            }
            return string.Join(",", list);
        }

        private void lvResult_Click(object sender, EventArgs e)
        {
            linksCache = new List<string>();

            foreach (ListViewItem item in lvResult.SelectedItems)
            {
                linksCache.Add(ConvertListViewItemToString(item));
            }
        }

        void CopyToClipboard(List<string> links)
        {
            Lib.Utils.RunAsSTAThread(() =>
            {
                MessageBox.Show(
                Lib.Utils.CopyToClipboard(string.Join(Environment.NewLine, links)) ?
                I18N("CopySuccess") :
                I18N("CopyFail"));
            });
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            var links = new List<string>();
            foreach (ListViewItem item in lvResult.Items)
            {
                links.Add(ConvertListViewItemToString(item));
            }

            CopyToClipboard(links);
        }
    }
}
