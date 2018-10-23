using System.Windows.Forms;

namespace V2RayGCon.Controller.OptionComponent
{
    class TabSetting : OptionComponentController
    {
        Service.Setting setting;

        ComboBox cboxLanguage = null, cboxPageSize = null;
        CheckBox chkServAutoTrack = null, chkPortableMode = null;

        public TabSetting(
            ComboBox cboxLanguage,
            ComboBox cboxPageSize,
            CheckBox chkServAutoTrack,
            CheckBox chkPortableMode)
        {
            this.setting = Service.Setting.Instance;

            // Do not put these lines of code into InitElement.
            this.cboxLanguage = cboxLanguage;
            this.cboxPageSize = cboxPageSize;
            this.chkServAutoTrack = chkServAutoTrack;
            this.chkPortableMode = chkPortableMode;

            InitElement(
                cboxLanguage,
                cboxPageSize,
                chkServAutoTrack,
                chkPortableMode);
        }

        private void InitElement(
            ComboBox cboxLanguage,
            ComboBox cboxPageSize,
            CheckBox chkServAutoTrack,
            CheckBox chkPortableMode)
        {
            chkPortableMode.Checked = setting.isPortable;
            cboxLanguage.SelectedIndex = (int)setting.culture;
            cboxPageSize.Text = setting.serverPanelPageSize.ToString();
            var tracker = setting.GetServerTrackerSetting();
            chkServAutoTrack.Checked = tracker.isTrackerOn;
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

            var trackerSetting = setting.GetServerTrackerSetting();
            trackerSetting.isTrackerOn = chkServAutoTrack.Checked;
            setting.SaveServerTrackerSetting(trackerSetting);
            setting.isServerTrackerOn = trackerSetting.isTrackerOn;
            setting.isPortable = chkPortableMode.Checked;

            return true;
        }

        public override bool IsOptionsChanged()
        {
            if (setting.isPortable != chkPortableMode.Checked)
            {
                return true;
            }

            if (Lib.Utils.Str2Int(cboxPageSize.Text) != setting.serverPanelPageSize)
            {
                return true;
            }

            var index = cboxLanguage.SelectedIndex;
            if (IsIndexValide(index) && ((int)setting.culture != index))
            {
                return true;
            }

            var tracker = setting.GetServerTrackerSetting();
            if (tracker.isTrackerOn != chkServAutoTrack.Checked)
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
