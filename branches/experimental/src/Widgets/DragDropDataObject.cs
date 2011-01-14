using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    class DragDropDataObject : IDataObject
    {
        public object GetData(Type format) {
            return null;
        }

        public bool GetDataPresent(Type format) {
            return false;
        }

        public void SetData(object data) {
            
        }
    }
}
