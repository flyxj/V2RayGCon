using ScintillaNET;
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
        public static Scintilla CreateScintilla(Panel container, bool readOnlyMode = false)
        {
            var scintilla = new Scintilla();

            container.Controls.Add(scintilla);

            // scintilla.Dock = DockStyle.Fill;
            scintilla.Dock = DockStyle.Fill;
            scintilla.WrapMode = WrapMode.None;
            scintilla.IndentationGuides = IndentView.LookBoth;

            // Configure the JSON lexer styles
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
            if (readOnlyMode)
            {
                var bgColor = Color.FromArgb(240, 240, 240);
                scintilla.Styles[Style.Default].BackColor = bgColor;
                scintilla.Styles[Style.Json.BlockComment].BackColor = bgColor;
                scintilla.Styles[Style.Json.Default].BackColor = bgColor;
                scintilla.Styles[Style.Json.Error].BackColor = bgColor;
                scintilla.Styles[Style.Json.EscapeSequence].BackColor = bgColor;
                scintilla.Styles[Style.Json.Keyword].BackColor = bgColor;
                scintilla.Styles[Style.Json.LineComment].BackColor = bgColor;
                scintilla.Styles[Style.Json.Number].BackColor = bgColor;
                scintilla.Styles[Style.Json.Operator].BackColor = bgColor;
                scintilla.Styles[Style.Json.PropertyName].BackColor = bgColor;
                scintilla.Styles[Style.Json.String].BackColor = bgColor;
                scintilla.Styles[Style.Json.CompactIRI].BackColor = bgColor;
                scintilla.ReadOnly = true;
            }

            scintilla.Styles[Style.Json.Default].ForeColor = Color.Silver;
            scintilla.Styles[Style.Json.BlockComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.LineComment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Json.Number].ForeColor = Color.Olive;
            scintilla.Styles[Style.Json.PropertyName].ForeColor = Color.Blue;
            scintilla.Styles[Style.Json.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            scintilla.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            scintilla.Styles[Style.Json.Operator].ForeColor = Color.Purple;
            scintilla.Lexer = Lexer.Json;

            // folding
            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            // key binding

            // clear default keyboard shortcut
            scintilla.ClearCmdKey(Keys.Control | Keys.P);
            scintilla.ClearCmdKey(Keys.Control | Keys.S);
            scintilla.ClearCmdKey(Keys.Control | Keys.F);

            return scintilla;
        }

        public static void FillComboBox(ComboBox cbox, Dictionary<int, string> table)
        {
            cbox.Items.Clear();
            foreach (var item in table)
            {
                cbox.Items.Add(item.Value);
            }
            cbox.SelectedIndex = table.Count > 0 ? 0 : -1;
        }

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

        public static void VisitUrl(string msg, string url)
        {
            var text = string.Format("{0}\n{1}", msg, url);
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
