using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;

namespace V2RayGCon.Lib
{
    class UI
    {
        public static bool ShowSaveFileDialog(string extension, string content, out string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = extension,
                Title = I18N("SaveAs"),
            };

            saveFileDialog.ShowDialog();

            fileName = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            try
            {
                File.WriteAllText(fileName, content);
                return true;
            }
            catch { }
            return false;
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
