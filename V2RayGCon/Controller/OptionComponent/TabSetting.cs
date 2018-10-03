using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class TabSetting : OptionComponentController
    {
        Service.Setting setting;

        ComboBox cboxLanguage = null;

        public TabSetting(
            ComboBox cboxLanguage)
        {
            this.setting = Service.Setting.Instance;
            this.cboxLanguage = cboxLanguage;
            InitElement(cboxLanguage);
        }

        private void InitElement(ComboBox cboxLanguage)
        {
            cboxLanguage.SelectedIndex = (int)setting.culture;
        }

        #region public method
        public override bool SaveOptions()
        {
            if (!IsOptionsChanged())
            {
                return false;
            }

            var index = cboxLanguage.SelectedIndex;
            setting.culture = (Model.Data.Enum.Cultures)index;
            MessageBox.Show("Language change has not yet taken effect.\n"
                + "Please restart this application.");
            return true;
        }

        public override bool IsOptionsChanged()
        {
            var index = cboxLanguage.SelectedIndex;
            return IsIndexValide(index) && ((int)setting.culture != index);
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
