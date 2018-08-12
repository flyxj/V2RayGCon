using System;

namespace V2RayGCon.Model.Data
{
    public class StrEvent : EventArgs
    {
        public string Data;
        public StrEvent(string data)
        {
            Data = data;
        }

    }

    public class IntEvent : EventArgs
    {
        public int Data;
        public IntEvent(int data)
        {
            Data = data;
        }

    }
}
