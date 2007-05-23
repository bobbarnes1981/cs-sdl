#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ComboBoxElement : UIElement, IDisposable
    {
        List<string> items;
        int cursor = -1;
        Surface dropdownSurface;
        bool dropdownVisible;
        int selectedItem;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="el"></param>
        /// <param name="palette"></param>
        public ComboBoxElement(UIScreen screen, BinElement el, byte[] palette)
            : base(screen, el, palette)
        {
            items = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedIndex
        {
            get { return cursor; }
            set
            {
                cursor = value;
                ClearSurface();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedItem
        {
            get { return items[cursor]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(string item)
        {
            AddItem(item, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="select"></param>
        public void AddItem(string item, bool select)
        {
            items.Add(item);
            if (select || cursor == -1)
            {
                cursor = items.IndexOf(item);
            }
            ClearSurface();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
            if (items.Count == 0)
            {
                cursor = -1;
            }
            ClearSurface();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            items.Clear();
            cursor = -1;
            ClearSurface();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(string item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonDown(MouseButtonEventArgs args)
        {
            ShowDropdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonUp(MouseButtonEventArgs args)
        {
            HideDropdown();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void PointerMotion(MouseMotionEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            /* if the dropdown is visible, see if we're inside it */
            if (!dropdownVisible)
            {
                return;
            }
            /*
			      starcraft doesn't include this check..  should we?
			      args.X >= X1 && args.X < X1 + dropdownSurface.Width &&
			    */
            if (args.Y >= Y1 + Height && args.Y < Y1 + Height + dropdownSurface.Height)
            {
                int new_selected_item = (args.Y - (Y1 + Height)) / Font.LineSize;

                if (selectedItem != new_selected_item)
                {
                    selectedItem = new_selected_item;
                    CreateDropdownSurface();
                }
            }
        }

        void PaintDropdown(Surface surf, DateTime dt)
        {
            surf.Blit(dropdownSurface, new Point(X1, Y1 + Height));
        }

        void ShowDropdown()
        {
            dropdownVisible = true;
            selectedItem = cursor;
            CreateDropdownSurface();
            ParentScreen.Painter.Add(Layer.Popup, PaintDropdown);
        }

        void HideDropdown()
        {
            dropdownVisible = false;
            if (cursor != selectedItem)
            {
                cursor = selectedItem;
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, new BoxSelectionChangedEventArgs(cursor));
                }
                ClearSurface();
            }
            ParentScreen.Painter.Remove(Layer.Popup, PaintDropdown);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Surface CreateSurface()
        {
            Surface surf = new Surface(Width, Height);

            /* XXX draw the arrow (and border) */

            if (cursor != -1)
            {
                Surface itemSurface = GuiUtil.ComposeText(items[cursor], Font, Palette, 4);

                itemSurface.TransparentColor = Color.Black;
                itemSurface.Transparent = true;
                surf.Blit(itemSurface, new Point(0, 0));
            }

            return surf;
        }

        void CreateDropdownSurface()
        {
            dropdownSurface = new Surface(Width, items.Count * Font.LineSize);

            int y = 0;
            for (int i = 0; i < items.Count; i++)
            {
                Surface itemSurface = GuiUtil.ComposeText(items[i], Font, Palette,
                                        i == selectedItem ? 4 : 24);

                itemSurface.TransparentColor = Color.Black;
                itemSurface.Transparent = true;

                dropdownSurface.Blit(itemSurface, new Point(0, y));
                y += itemSurface.Height;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<BoxSelectionChangedEventArgs> SelectionChanged;

        #region IDisposable Members

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            throw new SCException("The method or operation is not implemented.");
        }

        #endregion
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="e"></param>
    ///// <param name="sender"></param>
    //public delegate void ComboBoxSelectionChangedEventHandler(object sender, BoxSelectionChangedEventArgs e);
}
