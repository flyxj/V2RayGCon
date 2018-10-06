using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class TabSetting : OptionComponentController
    {
        Service.Setting setting;

        ComboBox cboxLanguage = null, cboxPageSize = null;

        public TabSetting(
            ComboBox cboxLanguage,
            ComboBox cboxPageSize)
        {
            this.setting = Service.Setting.Instance;
            this.cboxLanguage = cboxLanguage;
            this.cboxPageSize = cboxPageSize;

            InitElement(cboxLanguage, cboxPageSize);
        }

        private void InitElement(ComboBox cboxLanguage, ComboBox cboxPageSize)
        {
            cboxLanguage.SelectedIndex = (int)setting.culture;
            cboxPageSize.Text = setting.serverPanelPageSize.ToString();
        }

        #region public method
        public override bool SaveOptions()
        {
            if (!IsOptionsChanged())
            {
                return false;
            }

            var pageSize = Lib.Utils.Str2Int(cboxPageSize.Text);
            if (pageSize != setting.serverPanelPageSize)
            {
                setting.serverPanelPageSize = pageSize;
                Service.Servers.Instance.InvokeEventOnRequireFlyPanelUpdate(
                    this, System.EventArgs.Empty);
            }


            var index = cboxLanguage.SelectedIndex;
            if (IsIndexValide(index) && ((int)setting.culture != index))
            {
                setting.culture = (Model.Data.Enum.Cultures)index;
                MessageBox.Show("Language change has not yet taken effect.\n"
                    + "Please restart this application.");
            }
            return true;
        }

        public override bool IsOptionsChanged()
        {
            if (Lib.Utils.Str2Int(cboxPageSize.Text) != setting.serverPanelPageSize)
            {
                return true;
            }

            var index = cboxLanguage.SelectedIndex;
            if (IsIndexValide(index) && ((int)setting.culture != index))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region private method
        bool IsIndexValide(int index)
        {
            if (index < 0 || index > 2)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
