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

    using SdlDotNet.Core;

    public class Window : ContainerWidget
    {
        #region Fields

        bool alwaysOnTop;
        DialogResult dialogResult;
        Rectangle fullBounds;
        Object lockObject = new object();
        bool showInWindowSwitcher;
        TitleBar titleBar;
        bool topMost;
        bool updateLocked = false;
        bool windowed;
        WindowState windowState;
        string windowSwitcherText;

        #endregion Fields

        #region Constructors

        public Window(string name)
            : base(name) {
            this.fullBounds = new Rectangle(this.Location, this.Size);
            showInWindowSwitcher = true;
            this.updateLocked = true;
            base.ParentContainer = this;
            base.ForeColor = Color.Black;
            this.WindowState = WindowState.Normal;
            base.BorderStyle = BorderStyle.None;
            this.titleBar = new TitleBar(this, "titleBar", base.Size.Width);
            this.titleBar.BackColor = Color.Blue;
            this.titleBar.FillColor = Color.SlateGray;
            this.titleBar.Resized += new EventHandler(TitleBar_Resized);
            this.windowed = true;
            base.SetTopLevel(true);
            this.updateLocked = false;

            base.Paint += new EventHandler(Window_Paint);
        }

       

        #endregion Constructors

        #region Events

        public event EventHandler Load;

        public event EventHandler Shown;

        public event EventHandler WindowStateChanged;

        #endregion Events

        #region Properties

        public bool AlwaysOnTop {
            get { return alwaysOnTop; }
            set {
                alwaysOnTop = value;
                //				if (alwaysOnTop) {
                //					WindowManager.BringWindowToFront(this);
                //				}
            }
        }

        public DialogResult DialogResult {
            get { return dialogResult; }
            set {
                dialogResult = value;
            }
        }

        public Rectangle FullBounds {
            get { return fullBounds; }
        }

        /// <summary>
        /// Gets Or Sets The Window Location On The Main Form Using System.Drawing.Point.
        /// </summary>
        public new Point Location {
            get {
                if (windowed) {
                    return new Point(base.Location.X, base.Location.Y - titleBar.UnscaledSize.Height);
                } else {
                    return base.Location;
                }
            }
            set {
                if (windowed) {
                    base.Location = new Point(value.X, value.Y + titleBar.UnscaledSize.Height);
                    titleBar.Location = value;
                } else {
                    base.Location = value;
                }
                RecalculateFullBounds();
            }
        }

        public bool ShowInWindowSwitcher {
            get { return showInWindowSwitcher; }
            set {
                showInWindowSwitcher = value;
            }
        }

        /// <summary>
        /// Gets Or Sets The Size Of The Window Using System.Drawing.Size
        /// </summary>
        public new Size Size {
            get {
                if (windowed) {
                    return new Size(base.Size.Width, base.Size.Height + titleBar.UnscaledSize.Height);
                } else {
                    return base.Size;
                }
            }
            set {
                if (windowed) {
                    base.Size = new Size(value.Width, value.Height - titleBar.UnscaledSize.Height);
                } else {
                    base.Size = value;
                }
                RecalculateFullBounds();
                titleBar.Size = new Size(value.Width, titleBar.UnscaledSize.Height);
            }
        }

        public string Text {
            get { return titleBar.Text; }
            set { titleBar.Text = value; }
        }

        /// <summary>
        /// The TitleBar Of The Window.
        /// </summary>
        public TitleBar TitleBar {
            get { return titleBar; }
        }

        /// <summary>
        /// Gets Or Sets Whether The TitleBar Will Be Implemented On The Window. If False Then The Bar Will Be Removed.
        /// </summary>
        public bool Windowed {
            get { return windowed; }
            set {
                windowed = value;
                this.Location = base.UnscaledLocation;
                if (windowed) {
                    //base.Size = new Size(base.Size.Width, base.Size.Height - titleBar.Size.Height);
                }
                RecalculateFullBounds();
            }
        }

        public WindowState WindowState {
            get { return windowState; }
            set {
                windowState = value;

                if (WindowStateChanged != null) {
                    WindowStateChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The Window Switchers Title Shown On The Bar To The Bottom Left Corner Using The Window Switcher.
        /// </summary>
        public string WindowSwitcherText {
            get { return windowSwitcherText; }
            set {
                windowSwitcherText = value;
            }
        }

        internal bool TopMost {
            get { return topMost; }
            set {
                topMost = value;
                titleBar.InvokeRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public override void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface) {
            lock (lockObject) {
                if (!base.disposed) {
                    base.BlitToScreen(destinationSurface);
                    if (windowed) {
                        titleBar.BlitToScreen(destinationSurface);
                    }
                }
            }
        }

        public void Close() {
            WindowManager.RemoveWindow(this);
        }

        public override void FreeResources() {
            lock (lockObject) {
                if (!base.disposed) {
                    base.FreeResources();
                    if (titleBar != null) {
                        titleBar.FreeResources();
                    }
                }
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(e.MouseEventArgs, e.ScreenPosition);
            base.OnMouseDown(e);
            if (windowed && titleBar != null && DrawingSupport.PointInBounds(e.ScreenPosition, titleBar.ScaledBounds)) {
                titleBar.OnMouseDown(eventArgs);
            }
        }

        public override void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            if (DrawingSupport.PointInBounds(e.Position, this.Bounds)) {
                base.OnMouseMotion(e);
            }
            if (DrawingSupport.PointInBounds(e.Position, titleBar.Bounds)) {
                titleBar.OnMouseMotion(e);
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(e.MouseEventArgs, e.ScreenPosition);
            base.OnMouseUp(e);
            if (titleBar != null) {
                titleBar.OnMouseUp(eventArgs);
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            base.OnTick(e);
            if (windowed) {
                titleBar.OnTick(e);
            }
        }

        public void Show() {
            if (WindowManager.IsWindowOpen(this)) {
                this.Visible = true;
            } else {
                WindowManager.AddWindow(this);
            }
        }

        public DialogResult ShowDialog() {
            if (!WindowManager.IsWindowOpen(this)) {
                this.dialogResult = DialogResult.OK;
                Show();
                this.AlwaysOnTop = true;
                WindowManager.BringWindowToFront(this);

                while (WindowManager.IsWindowOpen(this)) {
                    if (WindowManager.CurrentModalWindow != this) {
                        WindowManager.CurrentModalWindow = this;
                    }
                    Events.RunThreadTickerOnce();
                }
                WindowManager.CurrentModalWindow = null;
            }
            return this.dialogResult;
        }

        internal void InvokeLoad() {
            if (Load != null)
                Load(this, EventArgs.Empty);
            base.RequestRedraw();
            //base.UpdateBuffer();
        }

        internal void InvokeShown() {
            if (Shown != null)
                Shown(this, EventArgs.Empty);
        }

        void Window_Paint(object sender, EventArgs e) {
            if (updateLocked == false) {
                if (titleBar != null) {
                    titleBar.InvokeRedraw();
                }
                base.UpdateBuffer(false);
                base.DrawBorder();
            }
        }

        internal void RecalculateFullBounds() {
            Size size = this.Size;
            if (SdlDotNet.Graphics.Video.UseResolutionScaling) {
                size = Resolution.ConvertSize(size.Width, size.Height);
            }
            fullBounds.Width = size.Width;
            fullBounds.Height = size.Height;
            Point loc = this.Location;
            if (SdlDotNet.Graphics.Video.UseResolutionScaling) {
                loc = Resolution.ConvertPoint(loc.X, loc.Y);
            }
            fullBounds.X = loc.X;
            fullBounds.Y = loc.Y;
        }

        void TitleBar_Resized(object sender, EventArgs e) {
            lock (lockObject) {
                if (windowed) {
                    this.Location = new Point(base.Location.X, base.Location.Y - titleBar.UnscaledSize.Height);
                    Size size = this.Size;
                    fullBounds.Width = size.Width;
                    fullBounds.Height = size.Height;
                    Point location = this.Location;
                    fullBounds.X = location.X;
                    fullBounds.Y = location.Y;
                }
            }
        }

        #endregion Methods
    }
}