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
    using System.Text;

    public class ListBoxItemCollection
    {
        #region Fields

        List<IListBoxItem> items;
        ListBox listBox;

        #endregion Fields

        #region Constructors

        public ListBoxItemCollection(ListBox listBox) {
            this.listBox = listBox;
            items = new List<IListBoxItem>();
        }

        #endregion Constructors

        #region Properties

        public int Count {
            get { return items.Count; }
        }

        #endregion Properties

        #region Indexers

        public IListBoxItem this[int index] {
            get { return items[index]; }
        }

        #endregion Indexers

        #region Methods

        public void Add(IListBoxItem item) {
            items.Add(item);

            listBox.ItemCountChanged();
        }

        public void Add(string text) {
            ListBoxTextItem textItem = new ListBoxTextItem(new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize), text);

            Add(textItem);
        }

        public void Clear() {
            for (int i = items.Count - 1; i >= 0; i--) {
                items[i].FreeResources();
                items.RemoveAt(i);
            }

            listBox.ItemCountChanged();
        }

        public void Remove(string textIdentifier) {
            for (int i = 0; i < items.Count; i++) {
                if (items[i].TextIdentifier == textIdentifier) {
                    items[i].FreeResources();
                    items.RemoveAt(i);

                    listBox.ItemCountChanged();
                    break;
                }
            }
        }

        public void Remove(IListBoxItem item) {
            if (items.Contains(item)) {
                item.FreeResources();
                items.Remove(item);

                listBox.ItemCountChanged();
            }
        }

        public void RemoveAt(int index) {
            if (index > -1 && index < items.Count) {
                items[index].FreeResources();
                items.RemoveAt(index);

                listBox.ItemCountChanged();
            }
        }

        #endregion Methods
    }
}