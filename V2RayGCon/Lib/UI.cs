using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Lib
{
    class UI
    {
        public static Model.Data.Enum.SaveFileErrorCode ShowSaveFileDialog(string extension, string content, out string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = extension,
                RestoreDirectory = true,
                Title = I18N("SaveAs"),
                ShowHelp = true,
            };

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return Model.Data.Enum.SaveFileErrorCode.Cancel;
            }

            try
            {
                File.WriteAllText(fileName, content);
                return Model.Data.Enum.SaveFileErrorCode.Success;
            }
            catch { }
            return Model.Data.Enum.SaveFileErrorCode.Fail;
        }

        public static string ShowReadFileDialog(string extension, out string fileName)
        {
            OpenFileDialog readFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = extension,
                RestoreDirectory = true,
                CheckFileExists = true,
                CheckPathExists = true,
                ShowHelp = true,
            };

            fileName = string.Empty;

            if (readFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = readFileDialog.FileName;
                try
                {
                    return File.ReadAllText(fileName);
                }
                catch { }
            }

            return string.Empty;
        }

        public static bool Confirm(string content)
        {
            var confirm = MessageBox.Show(
                content,
                I18N("Confirm"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            return confirm == DialogResult.Yes;
        }

        public static void VisitUrl(string msg,string url)
        {
            var text = string.Format("{0}\n{1}",msg,url);
            if (Confirm(text))
            {
                Task.Factory.StartNew(() => System.Diagnostics.Process.Start(url));
            }
        }

        public static void FillComboBox(ComboBox element, List<string> itemList)
        {
            element.Items.Clear();

            if (itemList == null || itemList.Count <= 0)
            {
                element.SelectedIndex = -1;
                return;
            }

            foreach (var item in itemList)
            {
                element.Items.Add(item);
            }
            element.SelectedIndex = 0;
        }

        [Conditional("DEBUG")]
        public static void SetFormLocation<T>(T form, Model.Data.Enum.FormLocations location) where T : Form
        {
            var width = Screen.PrimaryScreen.WorkingArea.Width;
            var height = Screen.PrimaryScreen.WorkingArea.Height;

            form.StartPosition = FormStartPosition.Manual;
            form.Size = new Size(width / 2, height / 2);
            form.Left = 0;
            form.Top = 0;

            switch (location)
            {
                case Model.Data.Enum.FormLocations.TopRight:
                    form.Left = width / 2;
                    break;
                case Model.Data.Enum.FormLocations.BottomLeft:
                    form.Top = height / 2;
                    break;
                case Model.Data.Enum.FormLocations.BottomRight:
                    form.Top = height / 2;
                    form.Left = width / 2;
                    break;
            }
        }
    }
}
