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

    public class WindowCollection
    {
        #region Fields

        List<string> windowNames;
        List<Window> windows;

        #endregion Fields

        #region Constructors

        public WindowCollection()
        {
            windowNames = new List<string>();
            windows = new List<Window>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Counts the number of windows that have been added.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return windows.Count; }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the <see cref="Window"/> with the specified name.
        /// </summary>
        /// <value></value>
        public Window this[string name]
        {
            get { return windows[windowNames.IndexOf(name)]; }
        }

        /// <summary>
        /// Gets the <see cref="Window"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Window this[int index]
        {
            get { return windows[index]; }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="window">The window.</param>
        public void AddWindow(string name, Window window)
        {
            windowNames.Add(name);
            windows.Add(window);
        }

        /// <summary>
        /// Determines whether this collection contains a window with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> if this collection contains a window with the specified name; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string name)
        {
            return windowNames.Contains(name);
        }

        /// <summary>
        /// Determines whether this collection contains the specified window.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns>
        /// 	<c>true</c> if this collection contains the specified window; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Window window)
        {
            return windows.Contains(window);
        }

        /// <summary>
        /// Returns the index of the window with the specified name. -1 is returned if the window
        /// is not in the collection
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int IndexOf(string name)
        {
            return windowNames.IndexOf(name);
        }

        /// <summary>
        /// Returns the index of the specified window. -1 is returned if the window
        /// is not in the collection
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns></returns>
        public int IndexOf(Window window)
        {
            return windows.IndexOf(window);
        }

        /// <summary>
        /// Removes the window based on its name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveWindow(string name)
        {
            int index = windowNames.IndexOf(name);
            windowNames.RemoveAt(index);
            windows.RemoveAt(index);
        }

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="window">The window.</param>
        public void RemoveWindow(Window window)
        {
            int index = windows.IndexOf(window);
            windowNames.RemoveAt(index);
            windows.RemoveAt(index);
        }

        /// <summary>
        /// Switches the windows.
        /// </summary>
        /// <param name="oldSlot">The old slot.</param>
        /// <param name="newSlot">The new slot.</param>
        public void SwitchWindows(int oldSlot, int newSlot)
        {
            if (windows.Count > oldSlot && windows.Count > newSlot) {
                string tempName = windowNames[oldSlot];
                Window temp = windows[oldSlot];
                windows[oldSlot] = windows[newSlot];
                windowNames[oldSlot] = windowNames[newSlot];
                windows[newSlot] = temp;
                windowNames[newSlot] = tempName;
            }
        }

        #endregion Methods
    }
}