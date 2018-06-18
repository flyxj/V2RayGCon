using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V2RayGCon.Model.Data
{
    class StrEvent : EventArgs
    {
        public string Data;
        public StrEvent(string data)
        {
            Data = data;
        }

    }

    class IntEvent : EventArgs
    {
        public int Data;
        public IntEvent(int data)
        {
            Data = data;
        }

    }
}