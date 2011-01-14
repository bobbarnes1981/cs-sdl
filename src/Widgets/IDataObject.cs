using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    interface IDataObject
    {
        object GetData(Type format);
        bool GetDataPresent(Type format);
        void SetData(object data);
    }
}
