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

namespace SdlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    using SdlDotNet.Widgets;

    public class Resolution
    {
        #region Fields

        public static int StandardResolutionHeight = 600;
        public static int StandardResolutionWidth = 800;

        static int resolutionHeight = StandardResolutionHeight;
        static decimal resolutionHeightRatio;
        static int resolutionWidth = StandardResolutionWidth;
        static decimal resolutionWidthRatio;

        #endregion Fields

        #region Properties

        public static int ResolutionHeight {
            get { return resolutionHeight; }
        }

        public static int ResolutionWidth {
            get { return resolutionWidth; }
        }

        #endregion Properties

        #region Methods

        public static int ConvertHeight(int height) {
            return (int)(height * resolutionHeightRatio);
        }

        public static int ConvertWidth(int width) {
            return (int)(width * resolutionWidthRatio);
        }

        public static Point ConvertPoint(int x, int y) {
            return new Point((int)(x * resolutionWidthRatio), (int)(y * resolutionHeightRatio));
        }

        public static Size ConvertSize(int width, int height) {
            return new Size((int)(width * resolutionWidthRatio), (int)(height * resolutionHeightRatio));
        }

        public static Size DeconvertSize(int width, int height) {
            return new Size((int)(width / resolutionWidthRatio), (int)(height / resolutionHeightRatio));
        }

        public static Point GetCenter(Size childSize) {
            return GetCenter(childSize, 0, 0);
        }

        public static Point GetCenter(Size childSize, int xOffset, int yOffset) {
            return new Point((StandardResolutionWidth / 2) - (childSize.Width / 2) + xOffset, (StandardResolutionHeight / 2) - (childSize.Height / 2) + yOffset);
        }

        public static Point GetCenter(Widget widget, int xOffset, int yOffset) {
            return GetCenter(widget.UnscaledSize, xOffset, yOffset);
        }

        public static Point GetCenter(Widget widget) {
            return GetCenter(widget.UnscaledSize);
        }

        public static int GetCenterX(Widget widget) {
            return (StandardResolutionWidth / 2) - (widget.UnscaledSize.Width / 2);
        }

        public static void SetStandardResolution(int width, int height) {
            StandardResolutionWidth = width;
            StandardResolutionHeight = height;
        }

        public static void SetResolution(int width, int height) {
            resolutionWidth = width;
            resolutionHeight = height;

            resolutionWidthRatio = resolutionWidth / (decimal)StandardResolutionWidth;
            resolutionHeightRatio = resolutionHeight / (decimal)StandardResolutionHeight;

            foreach (Widget childWidget in Widgets.EnumerateActiveWidgets()) {
                childWidget.HandleResolutionChanged();
            }
        }

        #endregion Methods
    }
}