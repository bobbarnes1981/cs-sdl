using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace SdlDotNet.Widgets
{
    public class FileBrowserDialog : Window
    {
        Button btnConfirm;
        Button btnCancel;
        ListBox lbxFileList;
        TextBox txtCurrentPath;
        TextBox txtSelectedItem;
        ComboBox cmbFilter;

        Button btnUpOneLevel;

        string currentPath;
        string filter;
        int filterIndex;
        string selectedFile;

        bool checkFileExists;

        public int FilterIndex {
            get { return filterIndex; }
            set {
                filterIndex = value;
                if (cmbFilter.Items.Count > filterIndex) {
                    cmbFilter.SelectItem(filterIndex);
                }
            }
        }

        public string SelectedFile {
            get { return selectedFile; }
        }

        public bool CheckFileExists {
            get { return checkFileExists; }
            set { checkFileExists = value; }
        }

        public string ConfirmButtonText {
            get { return btnConfirm.Text; }
            set { btnConfirm.Text = value; }
        }

        public FileBrowserDialog(string name)
            : base(name) {

            this.Windowed = true;
            this.Text = "File Browser Dialog";

            btnConfirm = new Button("btnConfirm");
            btnConfirm.Size = new Size(100, 15);
            btnConfirm.Text = "Confirm";
            btnConfirm.Click += new EventHandler<MouseButtonEventArgs>(btnConfirm_Click);

            btnCancel = new Button("btnCancel");
            btnCancel.Size = new Size(100, 15);
            btnCancel.Text = "Cancel";
            btnCancel.Click += new EventHandler<MouseButtonEventArgs>(btnCancel_Click);

            btnUpOneLevel = new Button("btnUpOneLevel");
            btnUpOneLevel.Text = "Up";
            btnUpOneLevel.Size = new System.Drawing.Size(40, 15);
            btnUpOneLevel.Click += new EventHandler<MouseButtonEventArgs>(btnUpOneLevel_Click);

            txtCurrentPath = new TextBox("txtCurrentPath");
            txtCurrentPath.Size = new System.Drawing.Size(300, 15);

            txtSelectedItem = new TextBox("txtSelectedItem");
            txtSelectedItem.Size = new System.Drawing.Size(350, 20);
            txtSelectedItem.BackColor = Color.WhiteSmoke;

            cmbFilter = new ComboBox("cmbFilter");
            cmbFilter.Size = new System.Drawing.Size(350, 20);
            cmbFilter.BackColor = Color.WhiteSmoke;
            cmbFilter.ItemSelected += new EventHandler(cmbFilter_ItemSelected);

            lbxFileList = new ListBox("lbxFileList");
            lbxFileList.Size = new Size(400, 200);
            lbxFileList.DoubleClick += new EventHandler<MouseButtonEventArgs>(lbxFileList_DoubleClick);
            lbxFileList.ItemSelected += new EventHandler(lbxFileList_ItemSelected);

            this.AddWidget(btnConfirm);
            this.AddWidget(btnCancel);
            this.AddWidget(btnUpOneLevel);
            this.AddWidget(lbxFileList);
            this.AddWidget(txtSelectedItem);
            this.AddWidget(cmbFilter);
            this.AddWidget(txtCurrentPath);

            base.Resized += new EventHandler(FileBrowserDialog_Resized);

            this.Size = new Size(400, 310);
            this.Location = DrawingSupport.GetCenter(Screen.Size, this.Size);

            DisplayFiles(Environment.CurrentDirectory);
        }

        void btnCancel_Click(object sender, MouseButtonEventArgs e) {
            this.selectedFile = null;
            this.DialogResult = SdlDotNet.Widgets.DialogResult.Cancel;
            this.Close();
        }

        void btnConfirm_Click(object sender, MouseButtonEventArgs e) {
            IListBoxItem item = lbxFileList.SelectedItem;
            string pathAddition = null;
            if (item != null) {
                ListBoxTextItem textItem = item as ListBoxTextItem;
                pathAddition = textItem.Text;
            }
            if (checkFileExists) {
                if (pathAddition != null && File.Exists(currentPath + pathAddition)) {
                    this.selectedFile = currentPath + pathAddition;
                } else {
                    this.selectedFile = null;
                }
            } else {
                this.selectedFile = currentPath + txtSelectedItem.Text;
            }

            this.DialogResult = SdlDotNet.Widgets.DialogResult.OK;
            this.Close();
        }

        void cmbFilter_ItemSelected(object sender, EventArgs e) {
            DisplayFiles(currentPath);
        }

        public string Filter {
            get {
                return filter;
            }
            set {
                if (value != filter) {
                    cmbFilter.Items.Clear();
                    filter = value;
                    if (!string.IsNullOrEmpty(filter)) {
                        string[] filters = filter.Split('|');
                        for (int i = 0; i < filters.Length; i++) {
                            ListBoxTextItem item = new ListBoxTextItem(new Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize), filters[i] + " (" + filters[i +1].Replace(";", ", ") + ")");
                            item.Tag = filters[i + 1];
                            cmbFilter.Items.Add(item);

                            i += 1;
                        }
                    }
                }
            }
        }

        public string SelectedFilterType {
            get {
                if (cmbFilter.SelectedItem != null) {
                    return cmbFilter.SelectedItem.Tag as string;
                } else {
                    return null;
                }
            }
        }

        public new DialogResult ShowDialog() {
            if (cmbFilter.Items.Count > filterIndex) {
                cmbFilter.SelectItem(filterIndex);
            }
            return base.ShowDialog();
        }

        void lbxFileList_ItemSelected(object sender, EventArgs e) {
            IListBoxItem item = lbxFileList.SelectedItem;
            if (item != null) {
                ListBoxTextItem textItem = item as ListBoxTextItem;
                string pathAddition = textItem.Text;
                if (File.Exists(currentPath + pathAddition)) {
                    txtSelectedItem.Text = pathAddition;
                } else {
                    txtSelectedItem.Text = "";
                }
            }
        }

        void btnUpOneLevel_Click(object sender, MouseButtonEventArgs e) {
            DirectoryInfo dirInfo = new DirectoryInfo(currentPath);
            DisplayFiles(dirInfo.Parent.FullName);
        }

        void lbxFileList_DoubleClick(object sender, MouseButtonEventArgs e) {
            IListBoxItem item = lbxFileList.SelectedItem;
            if (item != null) {
                ListBoxTextItem textItem = item as ListBoxTextItem;
                string pathAddition = textItem.Text;
                if (Directory.Exists(currentPath + pathAddition)) {
                    DisplayFiles(currentPath + pathAddition);
                }
            }
        }

        void FileBrowserDialog_Resized(object sender, EventArgs e) {
            RelocateWidgets();
        }

        private void RelocateWidgets() {
            btnConfirm.Location = new Point(this.Width - btnConfirm.Width, this.Height - btnConfirm.Height - 5);
            btnCancel.Location = new Point(this.Width - btnCancel.Width - btnCancel.Width, this.Height - btnCancel.Height - 5);

            cmbFilter.Location = new Point(25, btnCancel.Y - cmbFilter.Height - 5);
            txtSelectedItem.Location = new Point(25, cmbFilter.Y - txtSelectedItem.Height - 5);
            lbxFileList.Location = new Point(this.Width - lbxFileList.Width, txtSelectedItem.Y - lbxFileList.Height - 5);

            txtCurrentPath.Location = new Point(0, 0);
            btnUpOneLevel.Location = new Point(txtCurrentPath.X + txtCurrentPath.Width + 5, 0);
        }

        private void DisplayFiles(string directory) {
            lbxFileList.Items.Clear();
            if (directory.EndsWith(Path.DirectorySeparatorChar.ToString()) == false && directory.EndsWith(Path.AltDirectorySeparatorChar.ToString()) == false) {
                directory += Path.DirectorySeparatorChar;
            }
            txtCurrentPath.Text = directory;
            this.currentPath = directory;
            SdlDotNet.Graphics.Font font = new Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            string[] directories = Directory.GetDirectories(directory);
            foreach (string dir in directories) {
                ListBoxTextItem item = new ListBoxTextItem(font, Path.GetFileName(dir));
                lbxFileList.Items.Add(item);
            }
            string[] files = GetFiles(directory, GetCurrentFilter(), SearchOption.TopDirectoryOnly);
            foreach (string file in files) {
                ListBoxTextItem item = new ListBoxTextItem(font, Path.GetFileName(file));
                lbxFileList.Items.Add(item);
            }
        }

        private string GetCurrentFilter() {
            IListBoxItem item = cmbFilter.SelectedItem;
            if (item != null) {
                string filterExtensions = item.Tag as string;
                return filterExtensions;
            } else {
                return "*";
            }
        }

        public string[] GetFiles(string path, string searchPattern, SearchOption searchOption) {
            string[] searchPatterns = searchPattern.Split(';');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
            files.Sort();
            return files.ToArray();
        }

    }
}
