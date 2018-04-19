using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        // update has latency, so use callback
        private int _curSection;

        public int curSection
        {
            get
            {
                // Debug.WriteLine("Get curIndex:" + _curSection);
                return _curSection;
            }
            set
            {
                SetField(ref _curSection, value);
                // Debug.WriteLine("Set curIndex:" + _curSection);
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
