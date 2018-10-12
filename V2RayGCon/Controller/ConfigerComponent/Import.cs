using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using V2RayGCon.Resource.Resx;


namespace V2RayGCon.Controller.ConfigerComponet
{
    class Import : ConfigerComponentController
    {
        Scintilla editor;
        CheckBox cboxGlobalImport;

        public Import(
            Panel container,
            CheckBox globalImport,
            Button expand,
            Button clearCache,
            Button copy)
        {
            this.editor = Lib.UI.CreateScintilla(container, true);
            this.cboxGlobalImport = globalImport;
            DataBinding();
            AttachEvent(expand, clearCache, copy);
        }

        #region properties
        private string _content;

        public string content
        {
            get { return _content; }
            set
            {
                editor.ReadOnly = false;
                SetField(ref _content, value);
                editor.ReadOnly = true;
            }
        }
        #endregion

        #region private method
        void AttachEvent(
            Button expand,
            Button clearCache,
            Button copy)
        {
            expand.Click += (s, a) =>
            {
                container.InjectConfigHelper(null);
            };

            clearCache.Click += (s, a) =>
            {
                container.InjectConfigHelper(() =>
                {
                    Service.Cache.Instance.html.Clear();
                });
            };

            copy.Click += (s, a) =>
            {
                Lib.Utils.CopyToClipboardAndPrompt(editor.Text);
            };
        }

        void DataBinding()
        {
            var bs = new BindingSource();
            bs.DataSource = this;

            editor.DataBindings.Add(
                "Text",
                bs,
                nameof(this.content),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion

        #region public method
        public override void Update(JObject config)
        {
            content = I18N.AnalysingImport;
            var plainText = config.ToString();
            // todo
            Task.Factory.StartNew(() =>
            {
                var result = "{}";
                try
                {
                    result = Lib.ImportParser.Parse(
                        cboxGlobalImport.Checked ?
                        Lib.Utils.InjectGlobalImport(plainText, false, true) :
                            plainText)
                            .ToString();

                    Service.Servers.Instance.LazyGC();
                }
                catch (FileNotFoundException)
                {
                    result = string.Format(
                            "{0}{1}{2}",
                            I18N.DecodeImportFail,
                            Environment.NewLine,
                            I18N.FileNotFound);
                }
                catch (FormatException)
                {
                    result = I18N.DecodeImportFail;
                }
                catch (System.Net.WebException)
                {
                    result = string.Format(
                            "{0}{1}{2}",
                            I18N.DecodeImportFail,
                            Environment.NewLine,
                            I18N.NetworkTimeout);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    result = I18N.DecodeImportFail;
                }

                editor.Invoke((MethodInvoker)delegate
                {
                    content = result;
                });
            });
        }
        #endregion
    }
}
