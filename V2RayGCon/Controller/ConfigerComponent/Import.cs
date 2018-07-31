using Newtonsoft.Json.Linq;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.ConfigerComponet
{
    class Import :
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
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

        #endregion

        #region public method
        Scintilla editor;

        public void Bind(List<Control> controls)
        {
            if (controls.Count != 1)
            {
                throw new ArgumentException();
            }

            var el = controls[0];

            if (!(el is Scintilla))
            {
                throw new ArgumentException();
            }

            editor = el as Scintilla;


            var bs = new BindingSource();
            bs.DataSource = this;

            el.DataBindings.Add(
                "Text",
                bs,
                nameof(this.content),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
        }

        public JObject Inject(JObject config)
        {
            return config;
        }

        public void Update(JObject config)
        {
            content = I18N("AnalysingImport");
            // todo
            Task.Factory.StartNew(() =>
            {
                var cfg = "{}";
                try
                {
                    cfg = Lib.ImportParser.ParseImport(config).ToString();
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
