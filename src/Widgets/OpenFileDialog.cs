using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    public sealed class OpenFileDialog : FileDialog
    {
         public OpenFileDialog()
            : base("Open") {
            this.CheckFileExists = true;
            this.Title = "Open...";
        }

        public override DialogResult ShowDialog() {
            DialogResult browserResult = base.ShowDialog();
            return browserResult;
        }
    }
}
