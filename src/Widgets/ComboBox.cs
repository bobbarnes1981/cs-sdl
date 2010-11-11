#region Header

/*
 * Copyright (C) 2010 Pikablu
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

#endregion Header

namespace SdlDotNet.Widgets
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    public class ComboBox : Widget
    {
        #region Fields

        ListBox itemListBox;
        int listBoxHeight = 100;

        #endregion Fields

        #region Constructors

        public ComboBox(string name)
            : base(name) {
            base.InitializeDefaultWidget();

            base.BackColor = Color.WhiteSmoke;

            itemListBox = new ListBox("itemListBox");
            itemListBox.BackColor = Color.WhiteSmoke;
            itemListBox.MultiSelect = false;

            itemListBox.ItemSelected += new EventHandler(itemListBox_ItemSelected);

            base.Click += new EventHandler<MouseButtonEventArgs>(ComboBox_Click);
            base.Resized += new EventHandler(ComboBox_Resized);
        }

        #endregion Constructors

        #region Events

        public event EventHandler ItemSelected;

        #endregion Events

        #region Properties

        public ListBoxItemCollection Items {
            get { return itemListBox.Items; }
        }

        public int ListBoxHeight {
            get { return listBoxHeight; }
            set {
                listBoxHeight = value;
                itemListBox.Size = new Size(this.Width, listBoxHeight);
            }
        }

        public int SelectedIndex {
            get { return itemListBox.SelectedIndex; }
        }

        public IListBoxItem SelectedItem {
            get {
                if (itemListBox.SelectedItems.Count == 1) {
                    return itemListBox.SelectedItems[0];
                } else {
                    return null;
                }
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (itemListBox != null) {
                itemListBox.FreeResources();
            }
        }

        public void SelectItem(string textIdentifier) {
            itemListBox.SelectItem(textIdentifier);
        }

        public void SelectItem(int index) {
            itemListBox.SelectItem(index);
        }

        protected override void DrawBuffer() {
            base.DrawBuffer();
            if (itemListBox.SelectedItems.Count == 1) {
                base.Buffer.Blit(itemListBox.SelectedItems[0].Buffer, new Point(5, 2), new Rectangle(0, 0, this.Width - 5, this.Height - 2));
            }
            base.DrawBorder();
        }

        void ComboBox_Click(object sender, MouseButtonEventArgs e) {
            Point screenPos = base.ScreenLocation;
            itemListBox.Location = new System.Drawing.Point(screenPos.X, screenPos.Y + base.Height);
            WindowManager.AddToOverlayCollection(itemListBox, true);
        }

        void ComboBox_Resized(object sender, EventArgs e) {
            itemListBox.Size = new System.Drawing.Size(this.Width, listBoxHeight);
        }

        void itemListBox_ItemSelected(object sender, EventArgs e) {
            RequestRedraw();
            if (ItemSelected != null)
                ItemSelected(this, EventArgs.Empty);
            WindowManager.RemoveFromOverlayCollection(itemListBox);
        }

        #endregion Methods
    }
}