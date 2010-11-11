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

    public class TitleBar : ContainerWidget
    {
        #region Fields

        Button btnClose;
        bool drag;
        Point dragStart;
        Color fillColor;
        SdlDotNet.Graphics.Font font;
        Object lockObject = new object();
        Window ownerWindow;
        PictureBox picIcon;
        string text;
        short textStartX = 25;

        #endregion Fields

        #region Constructors

        public TitleBar(Window ownerWindow, string name, int width)
            : base(name) {
            this.ownerWindow = ownerWindow;

            this.Size = new System.Drawing.Size(width, 20);
            // Initialize default titlebar widgets
            // Initialize close button
            btnClose = new Button("btnClose");
            btnClose.BackColor = Color.Transparent;
            btnClose.BackgroundImage = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/Windows/closebutton.png");
            btnClose.Size = btnClose.BackgroundImage.Size;
            btnClose.BorderStyle = BorderStyle.None;
            btnClose.HighlightType = HighlightType.Image;
            btnClose.HighlightSurface = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/Windows/closebutton-hover.png");
            btnClose.Click += new EventHandler<MouseButtonEventArgs>(btnClose_Click);
            this.AddWidget(btnClose);
            // Initialize window icon
            picIcon = new PictureBox("picIcon");
            picIcon.SizeMode = ImageSizeMode.StretchImage;
            picIcon.Size = new Size(16, 16);
            picIcon.Location = new Point(2, 2);
            picIcon.Visible = false;

            this.AddWidget(picIcon);
        }

        #endregion Constructors

        #region Properties

        public Button CloseButton {
            get { return btnClose; }
            set {
                if (btnClose != null) {
                    btnClose.FreeResources();
                }
                btnClose = value;
            }
        }

        public Color FillColor {
            get { return fillColor; }
            set {
                fillColor = value;
                RequestRedraw();
            }
        }

        /// <summary>
        /// Gets Or Sets The Font Of The Titlebar Text Using The SdlDotNet.Graphics.Font Class
        /// </summary>
        public SdlDotNet.Graphics.Font Font {
            get {
                lock (lockObject) {
                    CheckFont();
                    return font;
                }
            }
            set {
                lock (lockObject) {
                    if (font != null) {
                        font.Close();
                    }
                    font = value;
                    RequestRedraw();
                }
            }
        }

        public PictureBox Icon {
            get { return picIcon; }
        }

        public new Size Size {
            get { return base.Size; }
            set {
                lock (lockObject) {
                    base.Size = value;
                    if (btnClose != null) {
                        btnClose.Location = new Point(this.Width - btnClose.Width - 10, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets Or Sets Title Of The TitleBar.
        /// </summary>
        public string Text {
            get { return text; }
            set {
                lock (lockObject) {
                    text = value;
                    if (ownerWindow != null) {
                        ownerWindow.WindowSwitcherText = value;
                    }
                    RequestRedraw();
                }
            }
        }

        public short TextStartX {
            get { return textStartX; }
            set {
                textStartX = value;
                RequestRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            lock (lockObject) {
                base.FreeResources();
                if (font != null) {
                    font.Close();
                    font = null;
                }
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Point relPoint = new Point(e.ScreenPosition.X - this.Location.X, e.ScreenPosition.Y - this.Location.Y);
            bool clickedOnWidget = false;
            for (int i = 0; i < base.ChildWidgets.Count; i++) {
                if (base.ChildWidgets[i].Visible && DrawingSupport.PointInBounds(relPoint, base.ChildWidgets[i].Bounds)) {
                    clickedOnWidget = true;
                    break;
                }
            }
            if (clickedOnWidget == false) {
                drag = true;
                dragStart = relPoint;
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            drag = false;
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            base.OnTick(e);
            if (picIcon != null) {
                picIcon.Visible = (picIcon.Image != null);
            }
            if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton)) {
                if (drag) {
                    this.Location = new Point(SdlDotNet.Input.Mouse.MousePosition.X - dragStart.X, SdlDotNet.Input.Mouse.MousePosition.Y - dragStart.Y);
                    ownerWindow.Location = this.Location;
                }
            } else {
                drag = false;
            }
        }

        protected override void DrawBackgroundRegion(Rectangle region) {
            base.Buffer.Fill(region, GetTitlebarColor());
        }

        protected override void DrawBuffer() {
            lock (lockObject) {
                if (!disposed && ownerWindow != null) {
                    base.Buffer.Fill(GetTitlebarColor());
                    base.DrawBackgroundImage();
                    RedrawTitleBar();
                    base.UpdateBuffer(false);
                }
            }
        }

        void btnClose_Click(object sender, EventArgs e) {
            if (ownerWindow != null) {
                ownerWindow.Close();
            }
        }

        private void CheckFont() {
            if (font == null) {
                font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            }
        }

        private Color GetTitlebarColor() {
            Color newColor;
            if (ownerWindow.TopMost) {
                newColor = Color.FromArgb(base.BackColor.A,
                                          (int)(base.BackColor.R * 0.5),
                                          (int)(base.BackColor.G * 0.5),
                                          (int)(base.BackColor.B * 0.5));
            } else {
                newColor = Color.FromArgb(base.BackColor.A,
                                          System.Math.Min((int)(base.BackColor.R * 1.25), 255),
                                          System.Math.Min((int)(base.BackColor.G * 1.25), 255),
                                          System.Math.Min((int)(base.BackColor.B * 1.25), 255));
            }
            return newColor;
        }

        private void RedrawTitleBar() {
            CheckFont();
            if (!string.IsNullOrEmpty(text)) {
                Size textSize = font.SizeText(text);
                SdlDotNet.Graphics.Primitives.Polygon rect = new SdlDotNet.Graphics.Primitives.Polygon(new short[] { (short)(textStartX + 6), (short)(textStartX + 4), textStartX, (short)(textStartX + 4), (short)(textStartX + 6), (short)(textStartX + textSize.Width + 24), (short)(textStartX + textSize.Width + 26), (short)(textStartX + textSize.Width + 28), (short)(textStartX + textSize.Width + 26), (short)(textStartX + textSize.Width + 24) }, new short[] { 0, 5, 10, 15, 20, 20, 15, 10, 5, 0 });//new SdlDotNet.Graphics.Primitives.Box(20, 2, (short)(font.SizeText(text).Width + 40), 19);
                base.Buffer.Draw(rect, fillColor, false, true);
                SdlDotNet.Graphics.Surface textSurf = font.Render(text, base.ForeColor, true);
                base.Buffer.Blit(textSurf, new Point(textStartX + 6, 5));
                textSurf.Close();
            }
        }

        #endregion Methods
    }
}