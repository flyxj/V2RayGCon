using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.ConfigerComponet
{
    class Import : Model.BaseClass.ConfigerComponent
    {
        Scintilla editor;

        public Import(
            Panel container,
            Button expand,
            Button clearCache,
            Button copy)
        {
            this.editor = Lib.UI.CreateScintilla(container, true);
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
                    Service.Cache.Instance.html.Remove(
                        Lib.ImportParser.GetImportUrls(
                            container.config));
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
            content = I18N("AnalysingImport");
            // todo
            Task.Factory.StartNew(() =>
            {
                var cfg = "{}";
                try
                {
                    cfg = Lib.ImportParser.ParseImport(config.ToString(), true).ToString();
                }
                catch (FileNotFoundException)
                {
                    // webclient 竟然可以读本地文件！

                    cfg = string.Format(
                            "{0}{1}{2}",
                            I18N("DecodeImportFail"),
                            Environment.NewLine,
                            I18N("FileNotFound"));
                }
                catch (FormatException)
                {
                    cfg = I18N("DecodeImportFail");
                }
                catch (System.Net.WebException)
                {
                    cfg = string.Format(
                            "{0}{1}{2}",
                            I18N("DecodeImportFail"),
                            Environment.NewLine,
                            I18N("NetworkTimeout"));
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    cfg = I18N("DecodeImportFail");
                }

                editor.Invoke((MethodInvoker)delegate
                {
                    content = cfg;
                });
            });
        }
        #endregion
    }
}
