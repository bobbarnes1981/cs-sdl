using System;
using System.Collections.Generic;
using System.Text;

namespace SdlDotNet.Widgets
{
    public class FileDialog
    {
        FileBrowserDialog fileBrowser;

        string title;
        bool addExtension;
        bool checkFileExists;
        bool checkPathExists;
        bool createPrompt;
        string defaultExt;
        string fileName;
        string[] fileNames;
        string filter;
        int filterIndex;
        string initialDirectory;
        bool overwritePrompt;

        public string InitialDirectory {
            get { return initialDirectory; }
            set { initialDirectory = value; }
        }

        public int FilterIndex {
            get { return filterIndex; }
            set { filterIndex = value; }
        }

        public string Filter {
            get { return filter; }
            set {
                filter = value;
                fileBrowser.Filter = filter;
            }
        }

        public string[] FileNames {
            get { return fileNames; }
            set { fileNames = value; }
        }

        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }

        public string DefaultExt {
            get { return defaultExt; }
            set { defaultExt = value; }
        }

        public bool CreatePrompt {
            get { return createPrompt; }
            set { createPrompt = value; }
        }

        public bool CheckPathExists {
            get { return checkPathExists; }
            set { checkPathExists = value; }
        }

        public bool CheckFileExists {
            get { return checkFileExists; }
            set { 
                checkFileExists = value;
                fileBrowser.CheckFileExists = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box automatically adds an extension to a file name if the user omits the extension.
        /// </summary>
        public bool AddExtension {
            get { return addExtension; }
            set { addExtension = value; }
        }

        /// <summary>
        /// Gets or sets the file dialog box title.
        /// </summary>
        public string Title {
            get { return title; }
            set {
                if (title != value) {
                    title = value;
                    fileBrowser.Text = title;
                }
            }
        }

        public FileDialog() {
            Initialize();
        }

        public FileDialog(string confirmButtonText) {
            Initialize();
            fileBrowser.ConfirmButtonText = confirmButtonText;
        }

        private void Initialize() {
            fileBrowser = new FileBrowserDialog("FileBrowser-FileDialog");
        }

        public DialogResult ShowDialog() {
            
            DialogResult browserResult = fileBrowser.ShowDialog();
            this.fileName = fileBrowser.SelectedFile;
            return browserResult;
        }
    }
}
