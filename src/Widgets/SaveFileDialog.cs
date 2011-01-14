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
            this.AddExtension = true;
            this.Title = "Save...";
        }

        public override DialogResult ShowDialog() {
            DialogResult browserResult = base.ShowDialog();
            for (int i = 0; i < base.FileNames.Length; i++) {
                string fileName = base.FileNames[i];
                if (base.AddExtension && !string.IsNullOrEmpty(fileName)) {
                    string extension = null;
                    string fullFilter = base.SelectedFilterType;
                    if (!string.IsNullOrEmpty(fullFilter)) {
                        if (fullFilter.Contains(";")) {
                            extension = fullFilter.Split(';')[0];
                        } else {
                            extension = fullFilter;
                        }
                        extension = extension.Substring(extension.IndexOf('.'));
                    }
                    if (!string.IsNullOrEmpty(extension)) {
                        if (fileName.Contains(".") == false) {
                            fileName += extension;
                        }
                    }
                    base.FileNames[i] = fileName;
                    if (i == 0) {
                        base.FileName = base.FileNames[0];
                    }
                }
            }
            return browserResult;
        }
    }
}
