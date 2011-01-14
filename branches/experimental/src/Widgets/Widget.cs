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

    public class Widget : Component, IDisposable
    {
        #region Fields

        public const int DEFAULT_WIDGET_HEIGHT = 50;
        public const int DEFAULT_WIDGET_WIDTH = 50;

        protected bool disposed;
        protected Dictionary<SdlDotNet.Input.Key, int> keysDown = new Dictionary<SdlDotNet.Input.Key, int>();

        byte alpha;
        bool autoHide;
        bool autoHideEnabled;
        bool autoHideMouseEntered;
        Color backColor;
        SdlDotNet.Graphics.Surface backgroundImage;
        ImageSizeMode backgroundImageSizeMode;
        Color borderColor;
        BorderStyle borderStyle;
        int borderWidth;
        //Rectangle bounds;
        SdlDotNet.Graphics.Surface buffer;
        Rectangle clipRectangle;
        Color foreColor;
        Widget groupedWidget;
        bool keyDown;
        double keyRepeatInterval;
        int keyStart;
        Object lockObject = new Object();
        bool mouseDown;
        int mouseHoverDelay;
        bool mouseOver;
        int mouseOverStart;
        Widget parent;
        ContainerWidget parentContainer;
        bool preventFocus;
        bool redrawRequested;
        ToolTip toolTip;
        bool topLevel;
        bool updating;
        bool visible;
        Rectangle cachedBounds = Rectangle.Empty;
        Rectangle cachedOriginalBounds = Rectangle.Empty;
        bool resizeRequested;
        bool relocateRequested;
        Rectangle unscaledBounds = Rectangle.Empty;

        #endregion Fields

        #region Constructors

        public Widget(string name)
            : base(name) {
            topLevel = false;
            InitializeDefaultWidget();
        }

        public Widget(string name, bool requiresLoading)
            : base(name) {
            topLevel = false;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Fired before the KeyDown event is processed.
        /// </summary>
        public event EventHandler<BeforeKeyDownEventArgs> BeforeKeyDown;

        /// <summary>
        /// Fired when this widget is clicked on.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> Click;

        /// <summary>
        /// Fired when this widget is double-clicked on.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> DoubleClick;

        /// <summary>
        /// Fired when a key is pressed down while this widget is the active widget.
        /// </summary>
        public event EventHandler<SdlDotNet.Input.KeyboardEventArgs> KeyDown;

        /// <summary>
        /// Fired when a key is released while this widget is the active widget.
        /// </summary>
        public event EventHandler<SdlDotNet.Input.KeyboardEventArgs> KeyUp;

        /// <summary>
        /// Fired when the location of this widget is changed.
        /// </summary>
        public event EventHandler LocationChanged;

        /// <summary>
        /// Fired when a mouse button is pressed down on this widget.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseDown;

        /// <summary>
        /// Fired when the mouse enters the bounding rectangle of this widget.
        /// </summary>
        public event EventHandler MouseEnter;

        /// <summary>
        /// Fired when the mouse hovers over this widget for a brief time .
        /// </summary>
        public event EventHandler MouseHover;

        /// <summary>
        /// Fired when the mouse leaves the bounding rectangle of this widget.
        /// </summary>
        public event EventHandler MouseLeave;

        /// <summary>
        /// Fired when the mouse moves while inside the bounding rectangle of this widget.
        /// </summary>
        public event EventHandler<SdlDotNet.Input.MouseMotionEventArgs> MouseMotion;

        /// <summary>
        /// Fired when a mouse button is released while on this widget.
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> MouseUp;

        // TODO: XML Comment for Redraw event
        public event EventHandler Redraw;

        /// <summary>
        /// Fired when this widget is resized.
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Fired when the buffer is actually resized
        /// </summary>
        public event EventHandler BufferResized;

        public event EventHandler Paint;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public byte Alpha {
            get { return alpha; }
            set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 255) {
                    value = 255;
                }
                alpha = value;
                buffer.Alpha = alpha;
                if (alpha == 255) {
                    buffer.AlphaBlending = false;
                } else {
                    buffer.AlphaBlending = true;
                }
                RequestRedraw();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether alpha blending is enabled.
        /// </summary>
        /// <value><c>true</c> if alpha blending is enabled; otherwise, <c>false</c>.</value>
        public bool AlphaBlending {
            get { return buffer.AlphaBlending; }
            set {
                buffer.AlphaBlending = value;
                RequestRedraw();
            }
        }

        public bool AutoHide {
            get { return autoHide; }
        }

        /// <summary>
        /// Gets or sets the backcolor.
        /// </summary>
        /// <value>The backcolor of the widget.</value>
        public Color BackColor {
            get { return backColor; }
            set {
                if (value.A == 255 && value.R == 255 && value.G == 255 && value.B == 255) {
                    value = Color.FromArgb(255, 255, 255, 254);
                }
                if (backColor != value) {
                    backColor = value;
                    if (backColor.A == 0) {
                        buffer.TransparentColor = Color.Transparent;
                        buffer.Transparent = true;
                    } else {
                        buffer.Transparent = false;
                    }
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background image.
        /// </summary>
        /// <value>The background image.</value>
        public SdlDotNet.Graphics.Surface BackgroundImage {
            get { return backgroundImage; }
            set {
                if (backgroundImage != null) {
                    backgroundImage.Close();
                }
                backgroundImage = value;
                //				backgroundImage.TransparentColor = Color.Transparent;
                //				backgroundImage.Transparent = true;
                UpdateBackgroundImage();
            }
        }

        /// <summary>
        /// Gets or sets the background image size mode.
        /// </summary>
        /// <value>The background image size mode.</value>
        public ImageSizeMode BackgroundImageSizeMode {
            get { return backgroundImageSizeMode; }
            set {
                backgroundImageSizeMode = value;
                UpdateBackgroundImage();
            }
        }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        /// <value>The color of the border.</value>
        public Color BorderColor {
            get { return borderColor; }
            set {
                if (borderColor != value) {
                    borderColor = value;
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the border style.
        /// </summary>
        /// <value>The border style.</value>
        public BorderStyle BorderStyle {
            get { return borderStyle; }
            set {
                if (borderStyle != value) {
                    borderStyle = value;
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        /// <value>The width of the border.</value>
        public int BorderWidth {
            get { return borderWidth; }
            set {
                if (borderWidth != value) {
                    borderWidth = value;
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets the bounding rectangle of the widget.
        /// </summary>
        /// <value>The bounding rectangle of the widget.</value>
        public Rectangle Bounds {
            get {
                return unscaledBounds;
            }
        }

        public Rectangle ScaledBounds {
            get {
                if (cachedBounds.Location == Point.Empty && cachedBounds.Size != Size.Empty) {
                    return new Rectangle(unscaledBounds.Location, cachedBounds.Size);
                } else if (cachedBounds.Location != Point.Empty && cachedBounds.Size == Size.Empty) {
                    return new Rectangle(cachedBounds.Location, unscaledBounds.Size);
                } else {
                    return unscaledBounds;
                }
            }
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <value>The buffer.</value>
        public Surface Buffer {
            get {
                return buffer;
            }
        }

        /// <summary>
        /// Gets or sets the clip rectangle.
        /// </summary>
        /// <value>The clip rectangle.</value>
        public Rectangle ClipRectangle {
            get { return clipRectangle; }
            set { clipRectangle = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Widget"/> is focused.
        /// </summary>
        /// <value><c>true</c> if focused; otherwise, <c>false</c>.</value>
        public bool Focused {
            get {
                if (parentContainer != null) {
                    if (parentContainer.ActiveWidget != null && parentContainer.ActiveWidget == this) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the forecolor.
        /// </summary>
        /// <value>The forecolor.</value>
        public Color ForeColor {
            get { return foreColor; }
            set {
                if (foreColor != value) {
                    foreColor = value;
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the grouped widget.
        /// </summary>
        /// <value>The grouped widget.</value>
        public Widget GroupedWidget {
            get { return groupedWidget; }
            set {
                groupedWidget = value;
                groupedWidget.Name = "(grouped: " + this.Name + ") " + groupedWidget.Name;
                RequestRedraw();
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height {
            get {
                return unscaledBounds.Height;
                //if (cachedBounds.Size == Size.Empty) {
                //    return this.Size.Height;
                //} else {
                //    return cachedBounds.Size.Height;
                //}
            }
            set {
                if (unscaledBounds.Height != value) {
                    cachedOriginalBounds.Height = this.ScaledSize.Height;

                    unscaledBounds.Height = value;

                    if (Video.UseResolutionScaling) {
                        value = Core.Resolution.ConvertHeight(value);
                    }

                    cachedBounds.Height = value;
                    if (cachedBounds.Height == 0) {
                        cachedBounds.Height = unscaledBounds.Height;
                    }
                    resizeRequested = true;
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether an invoke is required.
        /// </summary>
        /// <value><c>true</c> if an invoke is required; otherwise, <c>false</c>.</value>
        public bool InvokeRequired {
            get {
                return WindowManager.InvokeRequired;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a mouse button has been pressed down on this widget.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if a mouse button has been pressed down on this widget; otherwise, <c>false</c>.
        /// </value>
        public bool IsMouseDown {
            get { return mouseDown; }
        }

        /// <summary>
        /// Gets a value indicating whether key repeat is enabled.
        /// </summary>
        /// <value><c>true</c> if key repeat is enabled; otherwise, <c>false</c>.</value>
        public bool KeyRepeat {
            get { return (keyRepeatInterval > 0); }
        }

        /// <summary>
        /// Gets or sets the key repeat interval.
        /// </summary>
        /// <value>The key repeat interval.</value>
        public double KeyRepeatInterval {
            get { return keyRepeatInterval; }
            set { keyRepeatInterval = value; }
        }

        public Point ScaledLocation {
            get {
                if (cachedBounds.Location == Point.Empty) {
                    return unscaledBounds.Location;
                } else {
                    return cachedBounds.Location;
                }
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public Point Location {
            get {
                return unscaledBounds.Location;
                //if (cachedBounds.Location == Point.Empty) {
                //    return bounds.Location;
                //} else {
                //    return cachedBounds.Location;
                //}
            }
            set {
                SetLocation(value, true);
            }
        }

        private void SetLocation(Point location, bool scale) {
            if (unscaledBounds.Location != location) {
                cachedOriginalBounds.Location = this.ScaledLocation;

                unscaledBounds.Location = location;

                if (scale) {
                    if (SdlDotNet.Graphics.Video.UseResolutionScaling) {
                        location = Core.Resolution.ConvertPoint(location.X, location.Y);
                    }
                }

                cachedBounds.Location = location;
                relocateRequested = true;
                //ClearWidget();
                //bounds.Location = value;
                //if (!topLevel && parentContainer != null) {
                //    RequestRedraw();
                //    //parentContainer.UpdateWidget(this);
                //}
                if (LocationChanged != null)
                    LocationChanged(this, EventArgs.Empty);
                RequestRedraw();
            }
        }

        public void SetLocationUnscaled(Point location) {
            //unscaledBounds.Location = location;

            SetLocation(location, false);
        }

        /// <summary>
        /// Gets or sets the mouse hover delay.
        /// </summary>
        /// <value>The mouse hover delay.</value>
        public int MouseHoverDelay {
            get { return mouseHoverDelay; }
            set { mouseHoverDelay = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is inside the bounding rectangle of this widget.
        /// </summary>
        /// <value><c>true</c> if the mouse is inside the bounding rectangle of this widget; otherwise, <c>false</c>.</value>
        public bool MouseInBounds {
            get { return mouseOver; }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public Widget Parent {
            get { return parent; }
            set {
                parent = value;
                //SelectiveDrawBuffer();
            }
        }

        /// <summary>
        /// Gets or sets the parent container.
        /// </summary>
        /// <value>The parent container.</value>
        public ContainerWidget ParentContainer {
            get { return parentContainer; }
            set {
                parentContainer = value;
                if (groupedWidget != null) {
                    parentContainer.AddWidget(groupedWidget);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether focus should always be prevented.
        /// </summary>
        /// <value><c>true</c> if focus should always be prevented; otherwise, <c>false</c>.</value>
        public bool PreventFocus {
            get { return preventFocus; }
            set { preventFocus = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a redraw has been requested.
        /// </summary>
        /// <value><c>true</c> if a redraw has been requested; otherwise, <c>false</c>.</value>
        public bool RedrawRequested {
            get { return redrawRequested; }
            internal set {
                redrawRequested = value;
            }
        }

        /// <summary>
        /// Gets the absolute location of the widget.
        /// </summary>
        /// <value>The absolute location of the widget.</value>
        public Point ScreenLocation {
            get {
                Point totalLoc = GetTotalAddLocation(unscaledBounds.Location, this);
                return totalLoc;
            }
        }

        public Size ScaledSize {
            get {
                if (cachedBounds.Size == Size.Empty) {
                    return unscaledBounds.Size;
                } else {
                    return cachedBounds.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public Size Size {
            get {
                return unscaledBounds.Size;
                //if (cachedBounds.Size == Size.Empty) {
                //    return bounds.Size;
                //} else {
                //    return cachedBounds.Size;
                //}
            }
            set {
                if (unscaledBounds.Size != value) {
                    cachedOriginalBounds.Size = this.ScaledSize;

                    unscaledBounds.Width = value.Width;
                    unscaledBounds.Height = value.Height;

                    if (SdlDotNet.Graphics.Video.UseResolutionScaling) {
                        value = Core.Resolution.ConvertSize(value.Width, value.Height);
                    }

                    cachedBounds.Size = value;
                    resizeRequested = true;
                    //ClearWidget();
                    ////if (ParentContainer != null) {
                    ////    ParentContainer.ClearRegion(this.bounds, this);
                    ////    ParentContainer.UpdateWidget(this);
                    ////}
                    //bounds.Size = value;
                    //ResizeBuffer();
                    //if (ParentContainer != null) {
                    //    //    ParentContainer.ClearRegion(this.bounds, this);
                    //    //    ParentContainer.UpdateWidget(this);
                    //    ParentContainer.RequestRedraw();
                    //}
                    TriggerResizeEvent();
                    RequestRedraw();
                }
            }
        }

        public Size UnscaledSize {
            get {
                return unscaledBounds.Size;
            }
        }

        public Point UnscaledLocation {
            get {
                return unscaledBounds.Location;
            }
        }

        /// <summary>
        /// Gets the tooltip widget used by this widget.
        /// </summary>
        /// <value>The tooltip widget used by this widget.</value>
        public ToolTip ToolTip {
            get { return toolTip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this widget is a top-level widget.
        /// </summary>
        /// <value><c>true</c> if this widget is a top-level widget; otherwise, <c>false</c>.</value>
        public bool TopLevel {
            get { return topLevel; }
            set {
                topLevel = value;
                if (topLevel) {
                    RequestRedraw();
                }
            }
        }

        [Obsolete("There isn't any point to using this as the buffer will only be redrawn at the next BlitToScreen, instead of whenever a property is changed")]
        public bool Updating {
            get { return updating; }
            protected set {
                updating = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Widget"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible {
            get { return visible; }
            set {
                if (visible != value) {
                    visible = value;
                    //if (ParentContainer != null) {
                    //    ParentContainer.ClearRegion(this.bounds, this);
                    //    ParentContainer.UpdateWidget(this);
                    //}
                    //RequestRedraw();
                    if (!visible) {
                        if (ParentContainer != null) {
                            parentContainer.clearRegionRequests.Add(new ClearRegionRequest(this.Bounds, this));
                        }
                        //ClearWidget();
                    } else {
                        RequestRedraw();
                    }
                    if (groupedWidget != null) {
                        groupedWidget.Visible = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width {
            get {
                return unscaledBounds.Width;
                //if (cachedBounds.Size == Size.Empty) {
                //    return this.Size.Width;
                //} else {
                //    return cachedBounds.Size.Width;
                //}
            }
            set {
                if (unscaledBounds.Width != value) {
                    cachedOriginalBounds.Width = this.ScaledSize.Width;

                    unscaledBounds.Width = value;

                    if (Video.UseResolutionScaling) {
                        value = Core.Resolution.ConvertWidth(value);
                    }

                    cachedBounds.Width = value;
                    if (cachedBounds.Height == 0) {
                        cachedBounds.Height = unscaledBounds.Height;
                    }
                    resizeRequested = true;
                    //ClearWidget();
                    //bounds.Width = value;
                    //ResizeBuffer();
                    RequestRedraw();
                }
            }
        }

        public int ScaledX {
            get {
                if (cachedBounds.Location == Point.Empty) {
                    return unscaledBounds.X;
                } else {
                    return cachedBounds.X;
                }
            }
        }

        /// <summary>
        /// Gets or sets the X-coordinate of this widget.
        /// </summary>
        /// <value>The X-coordinate of this widget.</value>
        public int X {
            get {
                return unscaledBounds.X;
                //if (cachedBounds.Location == Point.Empty) {
                //    return bounds.X;
                //} else {
                //    return cachedBounds.X;
                //}
            }
            set {
                if (this.unscaledBounds.X != value) {
                    this.Location = new Point(value, unscaledBounds.Y);
                }
                //ClearWidget();
                //bounds.X = value;
                //if (LocationChanged != null)
                //    LocationChanged(this, EventArgs.Empty);
            }
        }

        public int ScaledY {
            get {
                if (cachedBounds.Location == Point.Empty) {
                    return unscaledBounds.Y;
                } else {
                    return cachedBounds.Y;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of this widget.
        /// </summary>
        /// <value>The Y-coordinate of this widget.</value>
        public int Y {
            get {
                return unscaledBounds.Y;
                //if (cachedBounds.Location == Point.Empty) {
                //    return bounds.Y;
                //} else {
                //    return cachedBounds.Y;
                //}
            }
            set {
                if (this.unscaledBounds.Y != value) {
                    this.Location = new Point(unscaledBounds.X, value);
                }
                //ClearWidget();
                //bounds.Y = value;
                //if (LocationChanged != null)
                //    LocationChanged(this, EventArgs.Empty);
            }
        }

        #endregion Properties

        #region Methods

        [Obsolete("There isn't any point to using this as the buffer will only be redrawn at the next BlitToScreen, instead of whenever a property is changed")]
        public void BeginUpdate() {
            updating = true;
        }

        /// <summary>
        /// Blits to screen.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        public virtual void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface, Rectangle sourceRectangle) {
            BlitToScreen(destinationSurface, sourceRectangle, Point.Empty);
        }

        /// <summary>
        /// Blits to screen.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        public virtual void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface) {
            BlitToScreen(destinationSurface, Rectangle.Empty);
        }

        /// <summary>
        /// Blits to screen.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="location">The location.</param>
        public virtual void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface, Point location) {
            BlitToScreen(destinationSurface, Rectangle.Empty, location);
        }

        bool blitting = false;
        /// <summary>
        /// Blits to screen.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        /// <param name="location">The location.</param>
        public virtual void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface, Rectangle sourceRectangle, Point location) {
            lock (this) {
                blitting = true;
                if (!disposed) {

                    if (relocateRequested) {
                        relocateRequested = false;
                        if (parentContainer != null) {
                            parentContainer.ClearRegion(cachedOriginalBounds, this);
                        }
                        unscaledBounds.Location = cachedBounds.Location;
                        cachedBounds.Location = Point.Empty;
                        if (!topLevel && parentContainer != null) {
                            RequestRedraw();
                        }
                    }
                    if (resizeRequested) {
                        resizeRequested = false;
                        if (parentContainer != null) {
                            parentContainer.ClearRegion(cachedOriginalBounds, this);
                        }
                        ResizeBuffer();
                        cachedBounds.Size = Size.Empty;
                        if (BufferResized != null) {
                            BufferResized(this, EventArgs.Empty);
                        }
                        if (ParentContainer != null) {
                            ParentContainer.RequestRedraw();
                        }
                    }

                    if (redrawRequested) {
                        redrawRequested = false;
                        TriggerPaint();
                    }

                }
                if (visible && !disposed) {
                    //PaintEventArgs e = new PaintEventArgs(destinationSurface, false);
                    //if (BeforePaint != null)
                    //    BeforePaint(this, e);
                    //if (Paint != null)
                    //    Paint(this, e);
                    //if (!e.CancelBufferBlit) {
                    if (sourceRectangle == Rectangle.Empty) {
                        if (location == Point.Empty) {
                            destinationSurface.Blit(buffer, this.ScaledLocation);
                        } else {
                            destinationSurface.Blit(buffer, location);
                        }
                    } else {
                        if (location == Point.Empty) {
                            destinationSurface.Blit(buffer, this.ScaledLocation, sourceRectangle);
                        } else {
                            destinationSurface.Blit(buffer, location, sourceRectangle);
                        }
                    }
                    //}
                }

                //if (sourceRectangle == Rectangle.Empty && location == Point.Empty) {

                //}

                blitting = false;
            }
        }

        [Obsolete("There isn't any point to using this as the buffer will only be redrawn at the next BlitToScreen, instead of whenever a property is changed")]
        public void EndUpdate() {
            updating = false;
            RequestRedraw();
        }

        /// <summary>
        /// Focuses this instance.
        /// </summary>
        public void Focus() {
            if (parentContainer != null) {
                parentContainer.SetActiveWidget(this);
            }
            if (topLevel) {
                if (Screen.ContainsWidget(this.Name)) {
                    Screen.activeWidget = this;
                }
            }
        }

        /// <summary>
        /// Frees all resources used.
        /// </summary>
        public override void FreeResources() {
            if (buffer != null) {
                buffer.Close();
                buffer = null;
            }
            if (backgroundImage != null) {
                backgroundImage.Close();
                backgroundImage = null;
            }
            if (toolTip != null) {
                toolTip.FreeResources();
                toolTip = null;
            }
            disposed = true;
        }

        /// <summary>
        /// Hides this instance.
        /// </summary>
        public void Hide() {
            this.Visible = false;
        }

        public void HideHidden() {
            autoHide = false;
            autoHideMouseEntered = false;
            autoHideEnabled = true;
            this.Visible = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose() {
            if (!disposed) {
                FreeResources();
                disposed = true;
            }
        }

        /// <summary>
        /// Invokes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="args">The args.</param>
        public void Invoke(Delegate target, params object[] args) {
            if (InvokeRequired) {
                WindowManager.Invoke(target, args);
            }
        }

        /// <summary>
        /// Requests that the buffer be redrawn.
        /// </summary>
        public void InvokeRedraw() {
            RequestRedraw();
        }

        /// <summary>
        /// Determines whether the mouse is in the bounding rectangle of this widget.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the mouse is in the bounding rectangle of this widget; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMouseInBounds() {
            Point relPoint;
            if (parent != null) {
                Point location = GetTotalAddLocation(new Point(0, 0), this);
                Point val = SdlDotNet.Input.Mouse.MousePosition;
                relPoint = new Point(SdlDotNet.Input.Mouse.MousePosition.X - location.X, SdlDotNet.Input.Mouse.MousePosition.Y - location.Y);
            } else {
                relPoint = new Point(SdlDotNet.Input.Mouse.MousePosition.X, SdlDotNet.Input.Mouse.MousePosition.Y);
            }
            return (DrawingSupport.PointInBounds(relPoint, this.Bounds));
        }

        public virtual void OnKeyboardDown(SdlDotNet.Input.KeyboardEventArgs e) {
            BeforeKeyDownEventArgs beforeKeyDownArgs = new BeforeKeyDownEventArgs(SdlDotNet.Input.Keyboard.KeyRepeat, e);
            if (BeforeKeyDown != null) {
                BeforeKeyDown(this, beforeKeyDownArgs);
            }
            List<SdlDotNet.Input.Key> newKeysDown = new List<Input.Key>();
            foreach (SdlDotNet.Input.Key key in keysDown.Keys) {
                if (SdlDotNet.Input.Keyboard.IsKeyPressed(key) == false) {
                    newKeysDown.Add(key);
                }
            }
            if (newKeysDown.Count > 0) {
                foreach (SdlDotNet.Input.Key key in newKeysDown) {
                    keysDown.Remove(key);
                }
            }
            if (keyRepeatInterval > 0 && beforeKeyDownArgs.UseKeyRepeat) {
                if (keysDown.ContainsKey(e.Key) == false) {
                    keysDown.Add(e.Key, SdlDotNet.Core.Timer.TicksElapsed);
                }
                if (SdlDotNet.Core.Timer.TicksElapsed > keysDown[e.Key] + keyRepeatInterval) {
                    keysDown[e.Key] = SdlDotNet.Core.Timer.TicksElapsed;
                    if (KeyDown != null)
                        KeyDown(this, e);
                }
            } else {
                if (keysDown.ContainsKey(e.Key) == false) {
                    keysDown.Add(e.Key, 0);
                    if (KeyDown != null)
                        KeyDown(this, e);
                }
            }
        }

        public virtual void OnKeyboardUp(SdlDotNet.Input.KeyboardEventArgs e) {
            if (keysDown.ContainsKey(e.Key)) {
                keysDown.Remove(e.Key);
            }
            if (KeyUp != null)
                KeyUp(this, e);
        }

        public virtual void OnMouseDown(MouseButtonEventArgs e) {
            e.RelativePosition = new Point(e.Position.X - this.Location.X, e.Position.Y - this.Location.Y);
            if (MouseDown != null)
                MouseDown(this, new MouseButtonEventArgs(e.MouseEventArgs, new Point(e.Position.X + this.Location.X, e.Position.Y + this.Location.Y)));
            mouseDown = true;
        }

        public virtual void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            if (mouseOver == false) {
                mouseOver = true;
                mouseOverStart = SdlDotNet.Core.Timer.TicksElapsed;
                if (autoHideEnabled) {
                    autoHideMouseEntered = true;
                }
                if (MouseEnter != null)
                    MouseEnter(this, null);
            } else {
                mouseOver = true;
            }
            if (MouseMotion != null)
                MouseMotion(this, e);
        }

        int lastClick;
        public virtual void OnMouseUp(MouseButtonEventArgs e) {
            e.RelativePosition = new Point(e.Position.X - this.Location.X, e.Position.Y - this.Location.Y);
            if (MouseUp != null)
                MouseUp(this, e);
            if (mouseDown) {
                TriggerClickEvent(e);
                mouseDown = false;
                if ((lastClick > 0) && (SdlDotNet.Core.Timer.TicksElapsed < lastClick + 250)) {
                    if (DoubleClick != null)
                        DoubleClick(this, new MouseButtonEventArgs(e));
                    lastClick = 0;
                } else {
                    lastClick = SdlDotNet.Core.Timer.TicksElapsed;
                }
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            base.OnTick(e);
            if (mouseOver) {
                if (mouseOverStart > -1 && e.Tick > mouseOverStart + mouseHoverDelay) {
                    mouseOverStart = -1;
                    if (MouseHover != null)
                        MouseHover(this, null);
                    if (toolTip != null) {
                        toolTip.ResetVanish();
                        Point mousePos = SdlDotNet.Input.Mouse.MousePosition;
                        toolTip.Location = new Point(mousePos.X - 10, mousePos.Y + 15);
                        WindowManager.AddToOverlayCollection(toolTip, false);
                    }
                }
                if (!IsMouseInBounds()) {
                    mouseOver = false;
                    if (MouseLeave != null)
                        MouseLeave(this, null);
                    if (autoHideEnabled && autoHideMouseEntered) {
                        HideHidden();
                    }
                    if (toolTip != null) {
                        toolTip.StartVanish();
                    }
                }
            }
        }

        /// <summary>
        /// Requests a redraw.
        /// </summary>
        public void RequestRedraw() {
            this.redrawRequested = true;
        }

        public void CancelRedrawRequest() {
            this.redrawRequested = false;
        }

        public void SetAutoHide() {
            autoHideEnabled = true;
            this.Visible = false;
        }

        /// <summary>
        /// Sets the tool tip.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetToolTip(string text) {
            if (toolTip != null) {
                toolTip.FreeResources();
            }
            toolTip = new ToolTip(base.Name + "-toolTip");
            toolTip.AutoSize = true;
            toolTip.MaxWidth = 200;
            toolTip.Text = text;
        }

        /// <summary>
        /// Shows this instance.
        /// </summary>
        public void Show() {
            this.Visible = true;
        }

        public void ShowHidden() {
            autoHide = true;
            autoHideMouseEntered = false;
            this.Visible = true;
        }

        /// <summary>
        /// Draws the background image.
        /// </summary>
        protected void DrawBackgroundImage() {
            if (backgroundImage != null) {
                lock (lockObject) {
                    Point drawPoint = new Point(0, 0);
                    switch (backgroundImageSizeMode) {
                        case ImageSizeMode.CenterImage: {
                                drawPoint = DrawingSupport.GetCenter(this.Buffer, backgroundImage.Size);
                                this.buffer.Blit(backgroundImage, drawPoint);
                                break;
                            }
                        case ImageSizeMode.Normal: {
                                lock (lockObject) {
                                    this.buffer.Blit(backgroundImage, drawPoint, new Rectangle(0, 0, this.Width, this.Height));
                                }
                                break;
                            }
                        case ImageSizeMode.StretchImage: {
                                lock (lockObject) {
                                    Surface oldBackground = backgroundImage;
                                    backgroundImage = backgroundImage.CreateStretchedSurface(this.Size);
                                    oldBackground.Close();
                                    this.buffer.Blit(backgroundImage, drawPoint);
                                }
                                break;
                            }
                        default: {
                                lock (lockObject) {
                                    this.buffer.Blit(backgroundImage, drawPoint);
                                }
                                break;
                            }
                    }
                    buffer.Blit(backgroundImage, new Point(0, 0));
                }
            }
        }

        /// <summary>
        /// Draws a specified region of the background image.
        /// </summary>
        /// <param name="region">The region.</param>
        protected void DrawBackgroundImageRegion(Rectangle region) {
            if (backgroundImage != null) {
                lock (lockObject) {
                    buffer.Blit(backgroundImage, region.Location, region);
                }
            }
        }

        /// <summary>
        /// Draws the border.
        /// </summary>
        protected virtual void DrawBorder() {
            switch (borderStyle) {
                case BorderStyle.FixedSingle: {
                        lock (lockObject) {
                            for (int i = 0; i < borderWidth; i++) {
                                SdlDotNet.Graphics.IPrimitive border = new SdlDotNet.Graphics.Primitives.Box((short)(i), (short)(i), (short)((this.Width) - (1 + i)), (short)((this.Height - (1 + i))));
                                buffer.Draw(border, borderColor);
                            }
                        }
                        break;
                    }
                case BorderStyle.Fixed3D: {
                        lock (lockObject) {
                            SdlDotNet.Graphics.Primitives.Line lineToDraw;

                            lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, 0),
                                                                                new Point(this.Width - borderWidth, 0));
                            buffer.Draw(lineToDraw, Color.FromArgb(73, 0, 0, 0));
                            lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, 0),
                                                                                new Point(0, this.Height - borderWidth));
                            buffer.Draw(lineToDraw, Color.FromArgb(73, 0, 0, 0));
                            lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, this.Height - borderWidth),
                                                                                new Point(this.Width - borderWidth, this.Height - borderWidth));
                            buffer.Draw(lineToDraw, Color.FromArgb(255, 255, 255, 254));
                            lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(this.Width - borderWidth, 0),
                                                                                new Point(this.Width - borderWidth, this.Height - borderWidth));
                            buffer.Draw(lineToDraw, Color.FromArgb(255, 255, 255, 254));
                        }
                        break;
                    }
            }
        }

        internal void ForceDrawBuffer() {
            TriggerPaint();
        }

        /// <summary>
        /// Initializes the default widget.
        /// </summary>
        protected void InitializeDefaultWidget() {
            unscaledBounds = new Rectangle(0, 0, DEFAULT_WIDGET_WIDTH, DEFAULT_WIDGET_HEIGHT);
            backColor = Color.LightGray;
            borderColor = Color.Black;
            this.visible = true;
            borderWidth = 1;
            ResizeBuffer();
            //TriggerResizeEvent();
        }

        /// <summary>
        /// Resizes the buffer.
        /// </summary>
        protected virtual void ResizeBuffer() {
            if (cachedBounds.Size != Size.Empty) {
                ResizeInternal(cachedBounds.Size);
            } else {
                ResizeInternal(this.Size);
            }

            if (backgroundImageSizeMode == ImageSizeMode.StretchImage) {
                UpdateBackgroundImage();
            } else {
                RequestRedraw();
            }
        }

        /// <summary>
        /// Internal method for resizing the buffer.
        /// </summary>
        /// <param name="size">The size.</param>
        protected void ResizeInternal(Size size) {
            lock (lockObject) {
                if (buffer != null) {
                    buffer.Close();
                }
                unscaledBounds.Size = size;
                buffer = new SdlDotNet.Graphics.Surface(ScaledBounds.Size);
                if (backColor.A == 0) {
                    buffer.TransparentColor = Color.Transparent;
                    buffer.Transparent = true;
                } else {
                    buffer.Transparent = false;
                }
                buffer.Alpha = alpha;
                buffer.AlphaBlending = (alpha != 255 && alpha != 0);
            }
        }

        /// <summary>
        /// Sets the top level.
        /// </summary>
        /// <param name="topLevel">if set to <c>true</c> [top level].</param>
        protected void SetTopLevel(bool topLevel) {
            this.topLevel = topLevel;
        }

        internal void HandleResolutionChanged() {
            this.Size = unscaledBounds.Size;
            this.Location = unscaledBounds.Location;
        }

        /// <summary>
        /// Triggers the click event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        protected void TriggerClickEvent(MouseButtonEventArgs e) {
            if (Click != null) {
                Click(this, e);
            }
        }

        /// <summary>
        /// Triggers the resize event.
        /// </summary>
        protected void TriggerResizeEvent() {
            if (Resized != null)
                Resized(this, null);
        }

        /// <summary>
        /// Triggers the redraw event.
        /// </summary>
        protected void TriggerRedrawEvent() {
            if (Redraw != null)
                Redraw(this, null);
        }

        /// <summary>
        /// Clears the widget from the parent containers' buffer.
        /// </summary>
        private void ClearWidget() {
            lock (lockObject) {
                if (!topLevel && parentContainer != null) {
                    if (!autoHide) {
                        parentContainer.ClearRegion(this.ScaledBounds, this);
                    } else {
                        parentContainer.ClearRegion(this.clipRectangle, this);
                    }
                }
            }
        }

        /// <summary>
        /// Obtains the location to add on to the widgets relative location to obtains a widgets absolution location
        /// </summary>
        private Point GetTotalAddLocation(Point addLoc, Widget widget) {
            if (widget.Parent != null) {
                return GetTotalAddLocation(new Point(addLoc.X + widget.Parent.Location.X, addLoc.Y + widget.Parent.Location.Y), widget.Parent);
            } else {
                return addLoc;
            }
        }

        /// <summary>
        /// Updates the background image.
        /// </summary>
        private void UpdateBackgroundImage() {
            if (backgroundImage != null) {
                Point drawPoint = new Point(0, 0);
                switch (backgroundImageSizeMode) {
                    case ImageSizeMode.AutoSize: {
                            if (this.Size != backgroundImage.Size) {
                                this.Size = backgroundImage.Size;
                            }
                            break;
                        }
                }
                RequestRedraw();
            }
        }

        protected void TriggerPaint() {
            if (!disposed && buffer != null) {
                buffer.Fill(this.BackColor);
                DrawBackgroundImage();
                if (Paint != null) {
                    Paint(this, EventArgs.Empty);
                }
            }
        }

        #endregion Methods
    }
}