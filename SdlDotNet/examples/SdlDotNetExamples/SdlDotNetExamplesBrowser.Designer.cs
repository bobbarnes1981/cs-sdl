#region LICENSE
/*
 * Copyright (C) 2004 - 2006 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System.Globalization;

namespace SdlDotNetExamples
{
    partial class SdlDotNetExamplesBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdlDotNetExamplesBrowser));
            this.btnRun = new System.Windows.Forms.Button();
            this.comboBoxNamespaces = new System.Windows.Forms.ComboBox();
            this.listBoxDemos = new System.Windows.Forms.ListBox();
            this.demoCategory = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRun.Location = new System.Drawing.Point(171, 482);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(59, 24);
            this.btnRun.TabIndex = 1;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // comboBoxNamespaces
            // 
            this.comboBoxNamespaces.FormattingEnabled = true;
            this.comboBoxNamespaces.Location = new System.Drawing.Point(140, 8);
            this.comboBoxNamespaces.Name = "comboBoxNamespaces";
            this.comboBoxNamespaces.Size = new System.Drawing.Size(168, 21);
            this.comboBoxNamespaces.Sorted = true;
            this.comboBoxNamespaces.TabIndex = 3;
            this.comboBoxNamespaces.SelectedIndexChanged += new System.EventHandler(this.comboBoxNamespaces_SelectedIndexChanged);
            // 
            // listBoxDemos
            // 
            this.listBoxDemos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxDemos.FormattingEnabled = true;
            this.listBoxDemos.Location = new System.Drawing.Point(13, 39);
            this.listBoxDemos.Name = "listBoxDemos";
            this.listBoxDemos.Size = new System.Drawing.Size(380, 433);
            this.listBoxDemos.Sorted = true;
            this.listBoxDemos.TabIndex = 4;
            this.listBoxDemos.DoubleClick += new System.EventHandler(this.listBoxDemos_DoubleClick);
            // 
            // demoCategory
            // 
            this.demoCategory.AutoSize = true;
            this.demoCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.demoCategory.Location = new System.Drawing.Point(12, 11);
            this.demoCategory.Name = "demoCategory";
            this.demoCategory.Size = new System.Drawing.Size(0, 13);
            this.demoCategory.TabIndex = 5;
            // 
            // SdlDotNetExamplesBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 519);
            this.Controls.Add(this.demoCategory);
            this.Controls.Add(this.listBoxDemos);
            this.Controls.Add(this.comboBoxNamespaces);
            this.Controls.Add(this.btnRun);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SdlDotNetExamplesBrowser";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox comboBoxNamespaces;
        private System.Windows.Forms.ListBox listBoxDemos;
        private System.Windows.Forms.Label demoCategory;
    }
}

