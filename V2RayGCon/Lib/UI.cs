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
        public static bool DlgWriteFile(string extension, string content, out string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = extension;
            saveFileDialog.Title = I18N("SaveAs");
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

        public static string DlgReadFile(string extension, out string fileName)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = "c:\\";
            dlg.Filter = extension;
            dlg.RestoreDirectory = true;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            fileName = string.Empty;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                try
                {

                    return File.ReadAllText(fileName);
                }
                catch { }
            }

            return string.Empty;
        }

        public static void ShowMsgboxSuccFail(bool success, string msgSuccess, string msgFail)
        {
            if (success)
            {
                MessageBox.Show(msgSuccess);
            }
            else
            {
                MessageBox.Show(msgFail);
            };
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

#if DEBUG
        public static MenuItem FindSubMenuItemByText(MenuItem parent, string text)
        {
            for (int a = 0; a < parent.MenuItems.Count; a++)
            {

                MenuItem item = parent.MenuItems[a];
                if (item != null)
                {
                    // Debug.WriteLine("FSM: " + a + " name:" +item.Text);
                    if (item.Text == text)
                    {
                        return item;
                    }
                    else
                    {
                        // running reursively
                        if (item.MenuItems.Count > 0)
                        {
                            item = FindSubMenuItemByText(item, text);
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            // nothing found
            return null;
        }

        public static MenuItem FindMenuItemByText(ContextMenu parent, string text)
        {
            for (int a = 0; a < parent.MenuItems.Count; a++)
            {
                MenuItem item = parent.MenuItems[a];
                // Debug.WriteLine("FM: " + a + " name:" + item.Text);
                if (item != null)
                {
                    if (item.Text == text)
                    {
                        return item;
                    }
                    else
                    {
                        // running reursively
                        if (item.MenuItems.Count > 0)
                        {
                            item = FindSubMenuItemByText(item, text);
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                }
            }
            // nothing found
            return null;
        }
#endif
    }
}
