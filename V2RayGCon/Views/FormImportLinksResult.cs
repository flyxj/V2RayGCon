using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V2RayGCon.Views
{
    public partial class FormImportLinksResult : Form
    {
        List<string[]> results;
        public FormImportLinksResult(List<string[]> importResults)
        {
            InitializeComponent();
            this.results = importResults;
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
    }
}
