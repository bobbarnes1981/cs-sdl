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

    public class ContainerWidget : ScrollableWidget, IContainer
    {
        #region Fields

        Widget activeWidget;
        bool autoScroll;
        WidgetCollection childWidgets;
        ComponentCollection components;
        HScrollBar hScrollBar;
        bool loaded;
        Object lockObject = new object();
        Size totalSize;
        bool updateParentContainer;
        VScrollBar vScrollBar;

        #endregion Fields

        #region Constructors

        public ContainerWidget(string name)
            : base(name)
        {
            childWidgets = new WidgetCollection();
            components = new ComponentCollection();

            updateParentContainer = true;

            loaded = false;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<WidgetAddedEventArgs> WidgetAdded;

        #endregion Events

        #region Properties

        public Widget ActiveWidget
        {
            get { return activeWidget; }
            set {
                SetActiveWidget(value);
            }
        }

        public WidgetCollection ChildWidgets
        {
            get { return childWidgets; }
        }

        public ComponentCollection Components
        {
            get { return components; }
        }

        public bool UpdateParentContainer
        {
            get {
                return updateParentContainer;
            }
            set {
                updateParentContainer = value;
            }
        }

        #endregion Properties

        #region Methods

        public void AddWidget(string name, Widget widget)
        {
            if (widget != null && !string.IsNullOrEmpty(name)) {
                widget.Parent = this;
                widget.ParentContainer = this;
                childWidgets.AddWidget(name, widget);
                if (WidgetAdded != null)
                    WidgetAdded(this, new WidgetAddedEventArgs(widget));
                IContainer container = widget as IContainer;
                if (container != null) {
                    container.LoadComplete();
                }
                if (loaded || this.Parent == null) {
                    widget.RequestRedraw();
                    UpdateWidget(widget);
                }
            }
        }

        public void AddWidget(Widget widget)
        {
            if (widget != null) {
                AddWidget(widget.Name, widget);
            }
        }

        public void AddWidget(string name, Component component)
        {
            if (component != null && !string.IsNullOrEmpty(name)) {
                components.AddComponent(name, component);
            }
        }

        public void AddWidget(Component component)
        {
            if (component != null) {
                AddWidget(component.Name, component);
            }
        }

        public override void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface)
        {
            base.BlitToScreen(destinationSurface);
        }

        public void ClearRegion(Rectangle bounds, Widget widgetToSkip)
        {
            ClearRegion(bounds, widgetToSkip, true);
        }

        public void ClearRegion(Rectangle bounds, Widget widgetToSkip, bool updateParent)
        {
            lock (lockObject) {
                DrawBackgroundRegion(bounds);
                base.DrawBackgroundImageRegion(bounds);

                for (int i = 0; i < childWidgets.Count; i++) {
                    if (childWidgets[i].Visible && (widgetToSkip != null && childWidgets[i] != widgetToSkip)) {
                        if (childWidgets[i].Bounds.IntersectsWith(bounds)) {
                            Rectangle region = CalculateRegion(bounds, childWidgets[i].Bounds);//new Rectangle(widgetToSkip.X - childWidgets[i].X, widgetToSkip.Y - childWidgets[i].Y, System.Math.Min((childWidgets[i].Width + childWidgets[i].X) - widgetToSkip.X, widgetToSkip.Width), System.Math.Min((childWidgets[i].Height + childWidgets[i].Y) - widgetToSkip.Y, widgetToSkip.Height));
                            childWidgets[i].BlitToScreen(base.Buffer, region, new Point(childWidgets[i].X + region.X, childWidgets[i].Y + region.Y));
                        }
                    }
                }
                // TriggerRedrawEvent();
                if (updateParentContainer && updateParent) {
                    if (ParentContainer != null && ParentContainer != this) {
                        ParentContainer.UpdateWidget(this);
                    }
                }
            }
        }

        public bool ContainsWidget(string name)
        {
            return (childWidgets.FindWidget(name) > -1);
        }

        public override void FreeResources()
        {
            base.FreeResources();
            if (childWidgets != null) {
                for (int i = 0; i < childWidgets.Count; i++) {
                    childWidgets[i].FreeResources();
                }
            }
            if (components != null) {
                for (int i = 0; i < components.Count; i++) {
                    components[i].FreeResources();
                }
            }
        }

        public Widget GetWidget(string name)
        {
            int index = childWidgets.FindWidget(name);
            if (index > -1) {
                return childWidgets[index];
            } else {
                return null;
            }
        }

        public void LoadComplete()
        {
            loaded = true;
            for (int i = 0; i < childWidgets.Count; i++) {
                //childWidgets[i].SelectiveDrawBuffer();
                childWidgets[i].RequestRedraw();
            }
            RequestRedraw();
            //DrawBuffer();
        }

        public override void OnKeyboardDown(SdlDotNet.Input.KeyboardEventArgs e)
        {
            if (activeWidget != null) {
                activeWidget.OnKeyboardDown(e);
            } else {
                base.OnKeyboardDown(e);
            }
        }

        public override void OnKeyboardUp(SdlDotNet.Input.KeyboardEventArgs e)
        {
            if (activeWidget != null) {
                activeWidget.OnKeyboardUp(e);
            } else {
                base.OnKeyboardUp(e);
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            e.Position = new Point(e.Position.X - this.Location.X, e.Position.Y - this.Location.Y);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = childWidgets.Count - 1; i >= 0; i--) {
                if (childWidgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(e.Position, childWidgets[i].Bounds)) {
                        if (!childWidgets[i].PreventFocus) {
                            activeWidget = childWidgets[i];
                        }
                        childWidgets[i].OnMouseDown(e);
                        return;
                    }
                }
            }
            activeWidget = null;
        }

        public override void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e)
        {
            base.OnMouseMotion(e);
            Point location = this.ScreenLocation;
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = childWidgets.Count - 1; i >= 0; i--) {
                if (childWidgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(relPoint, childWidgets[i].Bounds)) {
                        childWidgets[i].OnMouseMotion(e);
                        return;
                    }
                }
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            e.Position = new Point(e.Position.X - this.Location.X, e.Position.Y - this.Location.Y);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            for (int i = childWidgets.Count - 1; i >= 0; i--) {
                if (childWidgets[i].Visible) {
                    if (DrawingSupport.PointInBounds(e.Position, childWidgets[i].Bounds)) {
                        childWidgets[i].OnMouseUp(e);
                        return;
                    }
                }
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e)
        {
            base.OnTick(e);
            if (components != null) {
                for (int i = 0; i < components.Count; i++) {
                    components[i].OnTick(e);
                }
            }
            if (childWidgets != null) {
                for (int i = 0; i < childWidgets.Count; i++) {
                    childWidgets[i].OnTick(e);
                }
            }
            for (int i = 0; i < childWidgets.Count; i++) {
                if (childWidgets[i].Visible && childWidgets[i].RedrawRequested) {
                    ClearRegion(childWidgets[i].Bounds, childWidgets[i]);
                    UpdateWidget(childWidgets[i]);
                }
            }
        }

        public void RemoveWidget(string name)
        {
            lock (lockObject) {
                int index = childWidgets.FindWidget(name);
                if (index > -1) {
                    ClearRegion(childWidgets[index].Bounds, childWidgets[index]);
                    childWidgets[index].FreeResources();
                    childWidgets.RemoveWidget(index);
                } else {
                    index = components.FindComponent(name);
                    if (index > -1) {
                        components[index].FreeResources();
                        components.RemoveComponent(index);
                    }
                }
            }
        }

        public void SetActiveWidget(Widget widget)
        {
            int index = childWidgets.FindWidget(widget.Name);
            if (index > -1) {
                activeWidget = widget;
            }
        }

        public void UpdateBuffer()
        {
            UpdateBuffer(true);
        }

        public void UpdateBuffer(bool resetBackground)
        {
            lock (lockObject) {
                if (!base.Updating) {
                    if (resetBackground) {
                        base.Buffer.Fill(this.BackColor);
                        DrawBackgroundImage();
                    }
                    if (childWidgets != null) {
                        for (int i = 0; i < childWidgets.Count; i++) {
                            if (childWidgets[i].Visible) {
                                childWidgets[i].BlitToScreen(base.Buffer);
                            }
                        }
                    }
                    TriggerRedrawEvent();
                }
            }
        }

        public void UpdateWidget(Widget widget)
        {
            lock (lockObject) {
                if (!base.Updating) {
                    if (!string.IsNullOrEmpty(widget.Name)) {
                        if (widget.BackColor == Color.Transparent) {
                            DrawBackgroundRegion(widget.Bounds);
                            base.DrawBackgroundImageRegion(widget.Bounds);
                        }
                        WidgetRenderer.UpdateWidget(base.Buffer, widget, childWidgets);
                        //for (int i = 0; i < childWidgets.Count; i++) {
                        //    if (childWidgets[i].Visible) {
                        //        if (childWidgets[i] == widget) {
                        //            childWidgets[i].BlitToScreen(base.Buffer);
                        //        } else if (childWidgets[i].Bounds.IntersectsWith(widget.Bounds)) {
                        //            Rectangle region = CalculateRegion(widget.Bounds, childWidgets[i].Bounds);//new Rectangle(widget.X, widget.Y, System.Math.Min((childWidgets[i].Width + childWidgets[i].X) - widget.X, widget.Width), System.Math.Min((childWidgets[i].Height + childWidgets[i].Y) - widget.Y, widget.Height));
                        //            childWidgets[i].BlitToScreen(base.Buffer, region, new Point(childWidgets[i].X + region.X, childWidgets[i].Y + region.Y));
                        //        }
                        //    }
                        //}
                        TriggerRedrawEvent();
                        if (updateParentContainer) {
                            if (ParentContainer != null && ParentContainer != this) {
                                base.RequestRedraw();
                                ParentContainer.UpdateWidget(this);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void DrawBackgroundRegion(Rectangle region)
        {
            lock (lockObject) {
                base.Buffer.Fill(region, this.BackColor);
            }
        }

        private Rectangle CalculateRegion(Rectangle parentBounds, Rectangle childBounds)
        {
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