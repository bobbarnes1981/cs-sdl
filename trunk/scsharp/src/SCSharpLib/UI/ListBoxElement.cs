#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
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

using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class ListBoxElement : UIElement
    {
        List<string> items;
        int cursor = -1;
        bool selectable = true;
        int numVisible;
        int firstVisible;

        /// <summary>
        ///
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="el"></param>
        /// <param name="palette"></param>
        public ListBoxElement(UIScreen screen, BinElement el, byte[] palette)
            : base(screen, el, palette)
        {
            items = new List<string>();

            numVisible = Height / Font.LineSize;
            firstVisible = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void KeyboardDown(KeyboardEventArgs args)
        {
            bool selection_changed = false;

            /* navigation keys */
            if (args.Key == Key.UpArrow)
            {
                if (cursor > 0)
                {
                    cursor--;
                    selection_changed = true;

                    if (cursor < firstVisible)
                    {
                        firstVisible = cursor;
                    }
                }
            }
            else if (args.Key == Key.DownArrow)
            {
                if (cursor < items.Count - 1)
                {
                    cursor++;
                    selection_changed = true;

                    if (cursor >= firstVisible + numVisible)
                    {
                        firstVisible = cursor - numVisible + 1;
                    }
                }
            }

            if (selection_changed)
            {
                ClearSurface();
                if (SelectionChanged != null)
                {
                    SelectionChanged(cursor);
                }
            }
        }

        bool selecting;
        int selectionIndex;

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void MouseWheel(MouseButtonEventArgs args)
        {
            bool needRedraw = false;

            if (args.Button == MouseButton.WheelUp)
            {
                if (firstVisible > 0)
                {
                    firstVisible--;
                    needRedraw = true;
                }
            }
            else
            {
                if (firstVisible + numVisible < items.Count - 1)
                {
                    firstVisible++;
                    needRedraw = true;
                }
            }

            if (needRedraw)
            {
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonDown(MouseButtonEventArgs args)
        {
            bool needRedraw = false;
            /* if we're over the scrollbar handle that here */

            /* otherwise start our selection */
            selecting = true;

            /* otherwise, select the clicked-on item (if
            * there is one) */
            int index = (args.Y - Y1) / Font.LineSize + firstVisible;
            if (index < items.Count)
            {
                selectionIndex = index;
                needRedraw = true;
            }

            if (needRedraw)
            {
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void PointerMotion(MouseMotionEventArgs args)
        {
            if (!selecting)
            {
                return;
            }

            int index = (args.Y - Y1) / Font.LineSize + firstVisible;
            if (index < items.Count)
            {
                selectionIndex = index;
                ClearSurface();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonUp(MouseButtonEventArgs args)
        {
            if (!selecting)
            {
                return;
            }

            selecting = false;
            if (selectionIndex != cursor)
            {
                cursor = selectionIndex;
                if (SelectionChanged != null)
                {
                    SelectionChanged(cursor);
                }
            }

            ClearSurface();
        }

        /// <summary>
        ///
        /// </summary>
        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
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
            items.Add(item);
            if (cursor == -1)
            {
                cursor = 0;
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
        /// <returns></returns>
        protected override Surface CreateSurface()
        {
            Surface surf = new Surface(Width, Height);

            int y = 0;
            for (int i = firstVisible; i < firstVisible + numVisible; i++)
            {
                if (i >= items.Count)
                {
                    break;
                }
                Surface item_surface = GuiUtil.ComposeText(items[i], Font, Palette,
                (!selectable ||
                (!selecting && cursor == i) ||
                (selecting && selectionIndex == i)) ? 4 : 24);

                surf.Blit(item_surface, new Point(0, y));
                y += item_surface.Height;
            }

            surf.TransparentColor = Color.Black; /* XXX */

            return surf;
        }

        /// <summary>
        ///
        /// </summary>
        public event ListBoxSelectionChanged SelectionChanged;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="selectedIndex"></param>
    public delegate void ListBoxSelectionChanged(int selectedIndex);
}
