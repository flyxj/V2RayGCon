using System.Windows.Forms;

namespace Lunar.Views.WinForms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            VgcApis.Libs.UI.AutoSetFormIcon(this);
            this.Text = Properties.Resources.Name + " v" + Properties.Resources.Version;
        }
    }
}
