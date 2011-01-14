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

    public class ListBox : Widget
    {
        #region Fields

        ListBoxItemCollection items;
        bool multiSelect;
        SelectedItemCollection<IListBoxItem> selectedItems;
        VScrollBar vScroll;

        #endregion Fields

        #region Constructors

        public ListBox(String name)
            : base(name, true) {
            base.InitializeDefaultWidget();

            vScroll = new VScrollBar("vScroll");
            vScroll.Parent = this;
            vScroll.ValueChanged += new EventHandler<ValueChangedEventArgs>(vScroll_ValueChanged);
            items = new ListBoxItemCollection(this);
            selectedItems = new SelectedItemCollection<IListBoxItem>();

            base.BorderStyle = BorderStyle.FixedSingle;
            base.BorderWidth = 2;
            base.BorderColor = Color.Black;

            base.Click += new EventHandler<MouseButtonEventArgs>(ListBox_Click);
            base.Resized += new EventHandler(ListBox_Resized);

            base.Paint += new EventHandler(ListBox_Paint);
        }

        #endregion Constructors

        #region Events

        public event EventHandler ItemSelected;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets the items of the SdlDotNet.Widgets.ListBox.
        /// </summary>
        public ListBoxItemCollection Items {
            get { return items; }
        }

        public bool MultiSelect {
            get { return multiSelect; }
            set { multiSelect = value; }
        }

        public int SelectedIndex {
            get {
                if (selectedItems.Count > 0) {
                    for (int i = 0; i < items.Count; i++) {
                        if (items[i].Selected) {
                            return i;
                        }
                    }
                    return -1;
                } else {
                    return -1;
                }
            }
            set {
                SelectItem(value);
            }
        }

        public IListBoxItem SelectedItem {
            get {
                if (selectedItems.Count > 0) {
                    return selectedItems[0];
                } else {
                    return null;
                }
            }
        }

        public SelectedItemCollection<IListBoxItem> SelectedItems {
            get { return selectedItems; }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (vScroll != null) {
                vScroll.FreeResources();
            }
            for (int i = 0; i < items.Count; i++) {
                items[i].FreeResources();
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            if (vScroll.Visible && DrawingSupport.PointInBounds(e.RelativePosition, vScroll.Bounds)) {
                vScroll.OnMouseDown(new MouseButtonEventArgs(e.MouseEventArgs, e.RelativePosition));
            }
        }

        public override void OnMouseMotion(Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            Point location = this.ScreenLocation;
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            if (vScroll.Visible && DrawingSupport.PointInBounds(relPoint, vScroll.Bounds)) {
                vScroll.OnMouseMotion(e);
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            if (vScroll.Visible && DrawingSupport.PointInBounds(e.RelativePosition, vScroll.Bounds)) {
                vScroll.OnMouseUp(new MouseButtonEventArgs(e.MouseEventArgs, e.RelativePosition));
            }
        }

        public void SelectItem(int index) {
            if (items[index].Selected == false) {
                items[index].Selected = true;
                selectedItems.Add(items[index]);
            }
            HandleMultiSelect(index);
            RequestRedraw();
            if (ItemSelected != null)
                ItemSelected(this, EventArgs.Empty);
        }

        public void SelectItem(string textIdentifier, bool caseSensitive) {
            for (int i = 0; i < items.Count; i++) {
                if ((caseSensitive ? items[i].TextIdentifier.ToLower() : items[i].TextIdentifier) == (caseSensitive ? textIdentifier.ToLower() : textIdentifier)) {
                    SelectItem(i);
                    if (!multiSelect) {
                        break;
                    }
                }
            }
        }

        public void SelectItem(string textIdentifier) {
            SelectItem(textIdentifier, false);
        }

        public void ToggleItem(int index) {
            if (items[index].Selected) {
                items[index].Selected = false;
                selectedItems.Remove(items[index]);
            } else {
                items[index].Selected = true;
                selectedItems.Add(items[index]);
            }
            HandleMultiSelect(index);
            RequestRedraw();
            if (ItemSelected != null)
                ItemSelected(this, EventArgs.Empty);
        }

        internal void ItemCountChanged() {
            if (items.Count > 0) {
                for (int i = selectedItems.Count - 1; i >= 0; i--) {
                    if (items.Contains(selectedItems[i]) == false) {
                        selectedItems.RemoveAt(i);
                    }
                }
            } else {
                selectedItems.Clear();
            }
            RequestRedraw();
        }

        void ListBox_Paint(object sender, EventArgs e) {
            int vScrollMax = CalculateVScrollMax();
            if (vScroll.Maximum != vScrollMax) {
                if (vScrollMax > 0) {
                    vScroll.Show();
                    vScroll.Maximum = vScrollMax;
                } else {
                    vScroll.Hide();
                }
            }

            int itemStart = 0;
            int itemEnd = items.Count;
            if (vScroll.Visible) {
                itemStart = vScroll.Value;
                int tempHeight = 5;
                int counter = itemStart;
                while (tempHeight < this.Height && counter < items.Count) {
                    tempHeight += items[counter].Buffer.Height;
                    counter++;
                }
                itemEnd = counter;
            }
            int lastY = 5;
            for (int i = itemStart; i < itemEnd; i++) {
                if (items[i].Selected) {
                    base.Buffer.Fill(new Rectangle(5, lastY, this.Width - 10, items[i].Buffer.Height), Color.Blue);
                }
                items[i].AttemptRedraw();
                base.Buffer.Blit(items[i].Buffer, new Point(5, lastY));
                lastY += items[i].Buffer.Height;
            }

            if (vScroll.Visible) {
                vScroll.BlitToScreen(base.Buffer);
            }

            base.DrawBorder();
        }

        private int CalculateVScrollMax() {
            int currentHeight = 5;
            int visibleItems = 0;
            for (int i = 0; i < items.Count; i++) {
                currentHeight += items[i].Height;
                if (currentHeight <= base.Height) {
                    visibleItems++;
                } else {
                    break;
                }
            }
            if (currentHeight > base.Height) {
                return items.Count - visibleItems;
            } else {
                return 0;
            }
        }

        private void HandleMultiSelect(int itemToKeep) {
            if (!multiSelect) {
                for (int z = 0; z < items.Count; z++) {
                    if (items[z] != items[itemToKeep]) {
                        if (items[z].Selected) {
                            items[z].Selected = false;
                            selectedItems.Remove(items[z]);
                        }
                    }
                }
            }
        }

        void ListBox_Click(object sender, MouseButtonEventArgs e) {
            if (e.MouseEventArgs.Button == Input.MouseButton.PrimaryButton) {
                int itemStart = 0;
                int itemEnd = items.Count;
                if (vScroll.Visible) {
                    itemStart = vScroll.Value;
                    int tempHeight = 5;
                    int counter = itemStart;
                    while (tempHeight < this.Height && counter < items.Count) {
                        tempHeight += items[counter].Buffer.Height;
                        counter++;
                    }
                    itemEnd = counter;
                }
                int lastY = 5;
                for (int i = itemStart; i < itemEnd; i++) {
                    if (DrawingSupport.PointInBounds(e.RelativePosition, new Rectangle(5, lastY, (vScroll.Visible ? this.Width - 10 - vScroll.Width : this.Width - 10), items[i].Buffer.Height))) {
                        SelectItem(i);
                    }
                    lastY += items[i].Buffer.Height;
                }
            }
        }

        void ListBox_Resized(object sender, EventArgs e) {
            vScroll.Location = new Point(this.Width - 12, 0);
            vScroll.Size = new Size(12, this.Height);
        }

        void vScroll_ValueChanged(object sender, ValueChangedEventArgs e) {
            RequestRedraw();
        }

        #endregion Methods
    }
}