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

    public class Screen
    {
        #region Fields

        internal static Widget activeWidget;

        static WidgetCollection widgets;

        #endregion Fields

        #region Properties

        public static int Height {
            get { return WindowManager.destSurf.Height; }
        }

        public static Size Size {
            get { return WindowManager.destSurf.Size; }
        }

        public static int Width {
            get { return WindowManager.destSurf.Width; }
        }

        #endregion Properties

        #region Methods

        public static void AddWidget(Widget widget) {
            widget.TopLevel = true;
            widgets.AddWidget(widget.Name, widget);
        }

        public static void ClearWidgets() {
            for (int i = widgets.Count - 1; i >= 0; i--) {
                widgets[i].FreeResources();
                widgets.RemoveWidget(i);
            }
        }

        public static bool ContainsWidget(string name) {
            return widgets.FindWidget(name) != -1;
        }

        public static void DrawWidgets(SdlDotNet.Core.TickEventArgs e) {
            if (widgets != null) {
                for (int i = 0; i < widgets.Count; i++) {
                    Widget widget = null;
                    try {
                        widget = widgets[i];
                    } catch (IndexOutOfRangeException) {
                    }
                    if (widget != null) {
                        widget.OnTick(e);
                        widget.BlitToScreen(WindowManager.destSurf);
                    }
                }
            }
        }

        public static Widget GetWidget(string name) {
            int index = widgets.FindWidget(name);
            if (index > -1) {
                return widgets[index];
            } else {
                return null;
            }
        }

        internal static bool HandleKeyboardDown(SdlDotNet.Input.KeyboardEventArgs e) {
            if (widgets != null) {
                if (activeWidget != null) {
                    activeWidget.OnKeyboardDown(e);
                    return true;
                }
            }
            return false;
        }

        internal static bool HandleKeyboardUp(SdlDotNet.Input.KeyboardEventArgs e) {
            if (widgets != null) {
                if (activeWidget != null) {
                    activeWidget.OnKeyboardUp(e);
                    return true;
                }
            }
            return false;
        }

        internal static bool HandleMouseButtonDown(MouseButtonEventArgs e) {
            if (widgets != null) {
                for (int n = widgets.Count - 1; n >= 0; n--) {
                    if (widgets[n].Visible) {
                        if (DrawingSupport.PointInBounds(e.Position, widgets[n].Bounds)) {
                            activeWidget = widgets[n];
                            widgets[n].OnMouseDown(new MouseButtonEventArgs(e));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static bool HandleMouseButtonUp(MouseButtonEventArgs e) {
            if (widgets != null) {
                for (int n = widgets.Count - 1; n >= 0; n--) {
                    if (widgets[n].Visible) {
                        if (DrawingSupport.PointInBounds(e.Position, widgets[n].Bounds)) {
                            widgets[n].OnMouseUp(new MouseButtonEventArgs(e));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static bool HandleMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            if (widgets != null) {
                for (int n = widgets.Count - 1; n >= 0; n--) {
                    if (widgets[n].Visible) {
                        if (DrawingSupport.PointInBounds(e.Position, widgets[n].Bounds)) {
                            widgets[n].OnMouseMotion(e);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static void Initialize() {
            widgets = new WidgetCollection();
        }

        #endregion Methods
    }
}