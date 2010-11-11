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

    public class DrawingSupport
    {
        #region Methods

        public static Point GetCenter(SdlDotNet.Graphics.Surface mTexture, Size graphicSize) {
            return new Point((mTexture.Width / 2) - (graphicSize.Width / 2), (mTexture.Height / 2) - (graphicSize.Height / 2));
        }

        public static Point GetCenter(Size parentSize, Size graphicSize) {
            return new Point((parentSize.Width / 2) - (graphicSize.Width / 2), (parentSize.Height / 2) - (graphicSize.Height / 2));
        }

        public static int GetCenter(int parentSize, int childSize) {
            return (parentSize / 2) - (childSize / 2);
        }

        public static bool PointInBounds(Point pointToTest, Rectangle bounds) {
            if (pointToTest.X >= bounds.X && pointToTest.Y >= bounds.Y && pointToTest.X - bounds.Location.X <= bounds.Width && pointToTest.Y - bounds.Location.Y <= bounds.Height) {
                return true;
            } else {
                return false;
            }
        }

        #endregion Methods
    }
}