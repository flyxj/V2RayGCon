using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace V2RayGCon.Controller.Configer
{
    class Editor : INotifyPropertyChanged
    {
        // boiler-plate
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

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


        private int _curSection;

        public int curSection
        {
            get
            {
                return _curSection;
            }
            set
            {
                // SelectedIndexChanged fire before value change, so use callback
                SetField(ref _curSection, value);
                SectionChanged();
            }
        }

        Action SectionChanged;

        public Editor(Action OnSectionChanged)
        {
            SectionChanged = OnSectionChanged;
        }
    }
}
