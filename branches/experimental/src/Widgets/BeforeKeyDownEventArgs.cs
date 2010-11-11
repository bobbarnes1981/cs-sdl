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

    public class BeforeKeyDownEventArgs : EventArgs
    {
        #region Fields

        SdlDotNet.Input.KeyboardEventArgs keyboardEventArgs;
        bool useKeyRepeat;

        #endregion Fields

        #region Constructors

        internal BeforeKeyDownEventArgs(bool useKeyRepeat, SdlDotNet.Input.KeyboardEventArgs e) {
            this.useKeyRepeat = useKeyRepeat;
            this.keyboardEventArgs = e;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the keyboard event args passed by the standard SDL KeyDown event.
        /// </summary>
        /// <value>The keyboard event args.</value>
        public SdlDotNet.Input.KeyboardEventArgs KeyboardEventArgs {
            get { return keyboardEventArgs; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use key repeat.
        /// </summary>
        /// <value><c>true</c> if key repeat is active; otherwise, <c>false</c>.</value>
        public bool UseKeyRepeat {
            get { return useKeyRepeat; }
            set {
                useKeyRepeat = value;
            }
        }

        #endregion Properties
    }
}