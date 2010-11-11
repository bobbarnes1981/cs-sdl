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

    using SdlDotNet.Graphics;

    public class WidgetRenderer
    {
        #region Methods

        public static void ClearRegion(Surface destinationBuffer, Rectangle bounds, Widget widgetToSkip, WidgetCollection childWidgets, Color backgroundColor) {
            destinationBuffer.Fill(bounds, backgroundColor);

            for (int i = 0; i < childWidgets.Count; i++) {
                if (childWidgets[i].Visible && (widgetToSkip != null && childWidgets[i] != widgetToSkip)) {
                    if (childWidgets[i].Bounds.IntersectsWith(bounds)) {
                        Rectangle region = CalculateRegion(bounds, childWidgets[i].Bounds);//new Rectangle(widgetToSkip.X - childWidgets[i].X, widgetToSkip.Y - childWidgets[i].Y, System.Math.Min((childWidgets[i].Width + childWidgets[i].X) - widgetToSkip.X, widgetToSkip.Width), System.Math.Min((childWidgets[i].Height + childWidgets[i].Y) - widgetToSkip.Y, widgetToSkip.Height));
                        childWidgets[i].BlitToScreen(destinationBuffer, region, new Point(childWidgets[i].X + region.X, childWidgets[i].Y + region.Y));
                    }
                }
            }
        }

        public static void UpdateWidget(Surface destinationBuffer, Widget widgetToUpdate, WidgetCollection childWidgets) {
            for (int i = 0; i < childWidgets.Count; i++) {
                if (childWidgets[i].Visible) {
                    if (childWidgets[i] == widgetToUpdate) {
                        childWidgets[i].BlitToScreen(destinationBuffer);
                    } else if (childWidgets[i].Bounds.IntersectsWith(widgetToUpdate.Bounds)) {
                        Rectangle region = CalculateRegion(widgetToUpdate.Bounds, childWidgets[i].Bounds);//new Rectangle(widget.X, widget.Y, System.Math.Min((childWidgets[i].Width + childWidgets[i].X) - widget.X, widget.Width), System.Math.Min((childWidgets[i].Height + childWidgets[i].Y) - widget.Y, widget.Height));
                        childWidgets[i].BlitToScreen(destinationBuffer, region, new Point(childWidgets[i].X + region.X, childWidgets[i].Y + region.Y));
                    }
                }
            }
        }

        private static Rectangle CalculateRegion(Rectangle parentBounds, Rectangle childBounds) {
            int width = 0;
            int height = 0;
            int x = 0;
            int y = 0;
            if (childBounds.X < parentBounds.X) {
                if (childBounds.X + childBounds.Width > parentBounds.X + parentBounds.Width) {
                    width = parentBounds.Width;
                    x = parentBounds.X - childBounds.X;
                } else {
                    width = childBounds.Width - (parentBounds.X - childBounds.X);
                    x = childBounds.Width - width;
                }
            }
            if (childBounds.X >= parentBounds.X) {
                width = (parentBounds.X + parentBounds.Width) - childBounds.X;
                x = 0;
            }
            if (childBounds.Y < parentBounds.Y) {
                if (childBounds.Y + childBounds.Height > parentBounds.Y + parentBounds.Height) {
                    height = parentBounds.Height;
                    y = parentBounds.Y - childBounds.Y;
                } else {
                    height = childBounds.Height - (parentBounds.Y - childBounds.Y);
                    y = childBounds.Height - height;
                }
            }
            if (childBounds.Y >= parentBounds.Y) {
                height = (parentBounds.Y + parentBounds.Height) - childBounds.Y;
                y = 0;
            }

            return new Rectangle(x, y, width, height);
        }

        #endregion Methods
    }
}