using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    public sealed class SaveFileDialog : FileDialog
    {
        public SaveFileDialog()
            : base("Save") {
            this.CheckFileExists = false;
        }
    }
}
