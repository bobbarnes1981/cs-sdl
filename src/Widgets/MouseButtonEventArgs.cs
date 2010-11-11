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

    public class MouseButtonEventArgs : EventArgs
    {
        #region Fields

        SdlDotNet.Input.MouseButtonEventArgs mouseEventArgs;
        Point position;
        Point relativePosition;
        Point screenPosition;

        #endregion Fields

        #region Constructors

        public MouseButtonEventArgs(SdlDotNet.Input.MouseButtonEventArgs e, Point position) {
            screenPosition = e.Position;
            mouseEventArgs = e;
            this.position = position;
            this.relativePosition = position;
        }

        public MouseButtonEventArgs(MouseButtonEventArgs e) {
            screenPosition = e.ScreenPosition;
            mouseEventArgs = e.MouseEventArgs;
            this.position = e.Position;
            this.relativePosition = e.Position;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the mouse event args passed by SDL.
        /// </summary>
        /// <value>The mouse event args.</value>
        public SdlDotNet.Input.MouseButtonEventArgs MouseEventArgs {
            get { return mouseEventArgs; }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point Position {
            get { return position; }
            set {
                position = value;
            }
        }

        /// <summary>
        /// Gets or sets the relative position.
        /// </summary>
        /// <value>The relative position.</value>
        public Point RelativePosition {
            get { return relativePosition; }
            set {
                relativePosition = value;
            }
        }

        /// <summary>
        /// Gets the screen position.
        /// </summary>
        /// <value>The screen position.</value>
        public Point ScreenPosition {
            get { return screenPosition; }
        }

        #endregion Properties
    }
}