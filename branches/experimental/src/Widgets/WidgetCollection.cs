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

    public class WidgetCollection
    {
        #region Fields

        List<string> widgetNames;
        List<Widget> widgets;

        #endregion Fields

        #region Constructors

        public WidgetCollection() {
            widgetNames = new List<string>();
            widgets = new List<Widget>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the number of widgets in this collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get { return widgetNames.Count; }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the <see cref="SdlDotNet.Widgets.Widget"/> with the specified name.
        /// </summary>
        /// <value></value>
        public Widget this[string name] {
            get {
                return widgets[widgetNames.IndexOf(name)];
            }
        }

        /// <summary>
        /// Gets the <see cref="SdlDotNet.Widgets.Widget"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Widget this[int index] {
            get {
                return widgets[index];
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds the widget.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="widget">The widget.</param>
        public void AddWidget(string name, Widget widget) {
            if (!widgetNames.Contains(name)) {
                widgetNames.Add(name);
                widgets.Add(widget);
            }
        }

        /// <summary>
        /// Adds the widget.
        /// </summary>
        /// <param name="widget">The widget.</param>
        public void AddWidget(Widget widget) {
            if (!string.IsNullOrEmpty(widget.Name)) {
                if (!widgetNames.Contains(widget.Name)) {
                    widgetNames.Add(widget.Name);
                    widgets.Add(widget);
                }
            }
        }

        /// <summary>
        /// Finds the widget.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int FindWidget(string name) {
            return widgetNames.IndexOf(name);
        }

        /// <summary>
        /// Handles the mouse down event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Widgets.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="location">The location.</param>
        public void HandleMouseDown(MouseButtonEventArgs e, Point location) {
            e.Position = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = widgets.Count - 1; i >= 0; i--) {
                if (widgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(e.Position, widgets[i].Bounds)) {
                        widgets[i].OnMouseDown(e);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the mouse motion event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.MouseMotionEventArgs"/> instance containing the event data.</param>
        /// <param name="location">The location.</param>
        public void HandleMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e, Point location) {
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = widgets.Count - 1; i >= 0; i--) {
                if (widgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(relPoint, widgets[i].Bounds)) {
                        widgets[i].OnMouseMotion(e);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the mouse up event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Widgets.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <param name="location">The location.</param>
        public void HandleMouseUp(MouseButtonEventArgs e, Point location) {
            e.Position = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = widgets.Count - 1; i >= 0; i--) {
                if (widgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(e.Position, widgets[i].Bounds)) {
                        widgets[i].OnMouseUp(e);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the tick event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Core.TickEventArgs"/> instance containing the event data.</param>
        public void HandleTick(SdlDotNet.Core.TickEventArgs e) {
            if (widgets != null) {
                for (int i = 0; i < widgets.Count; i++) {
                    widgets[i].OnTick(e);
                }
            }
        }

        /// <summary>
        /// Removes the widget at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveWidget(int index) {
            if (index > -1) {
                widgetNames.RemoveAt(index);
                widgets.RemoveAt(index);
            }
        }

        public IEnumerable<Widget> EnumerateWidgets() {
            for (int i = 0; i < widgets.Count; i++) {
                yield return widgets[i];
                if (widgets[i] is IContainer) {
                    IContainer container = widgets[i] as IContainer;
                    foreach (Widget childWidget in container.ChildWidgets.EnumerateWidgets()) {
                        yield return childWidget;
                    }
                }
            }
        }

        #endregion Methods
    }
}