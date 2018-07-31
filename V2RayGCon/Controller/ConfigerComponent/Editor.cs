namespace V2RayGCon.Controller.ConfigerComponet
{
    class Editor : Model.BaseClass.NotifyComponent
    {
        private string _content;

        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                SetField(ref _content, value);
            }
        }

    }
}
