using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static V2RayGCon.Lib.StringResource;


namespace V2RayGCon.Controller.Configer
{
    class Import:
        Model.BaseClass.NotifyComponent,
        Model.BaseClass.IConfigerComponent
    {
        #region properties
        private string _content;

        public string content
        {
            get { return _content; }
            set { SetField(ref _content, value); }
        }
        #endregion

        #region public method
        Control element;
        public Import(Control elementForInvoke)
        {
            element = elementForInvoke;
        }
        public JToken GetSettings()
        {
            return JToken.Parse(@"{}");
        }

        public void UpdateData(JObject config)
        {
            // todo
            Task.Factory.StartNew(() => {
                var cfg = "{}";
                try
                {
                    cfg = Lib.ImportParser.ParseImport(config).ToString();
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
                catch
                {
                    cfg = I18N("DecodeImportFail");
                }

                element.Invoke((MethodInvoker)delegate{
                    content = cfg;
                });
            });
        }
        #endregion
    }
}
