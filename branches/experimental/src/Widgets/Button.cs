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

    public class Button : Widget
    {
        #region Fields

        SdlDotNet.Graphics.Font font;
        Color highlightColor;
        SdlDotNet.Graphics.Surface highlightSurface;
        HighlightType highlightType;
        Object lockObject = new object();
        bool mouseDown;
        bool selected;
        bool spaceDown;
        string text;
        bool redrawHighlightSurfaceRequested;
        bool selectedChanged;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates A New Button Widget For Usage Of The Window Class.
        /// </summary>
        /// <param name="name">The Name Of The Button(Important When You Need To Find The Control Type)</param>
        public Button(string name)
            : base(name) {
            this.highlightType = HighlightType.Color;
            this.highlightColor = Color.Silver;
            base.BackColor = SystemColors.Control;
            base.BorderStyle = BorderStyle.Fixed3D;

            base.KeyDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(Button_KeyDown);
            base.MouseEnter += new EventHandler(Button_MouseEnter);
            base.MouseLeave += new EventHandler(Button_MouseLeave);
            base.Resized += new EventHandler(Button_Resized);

            base.Paint += new EventHandler(Button_Paint);
        }



        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets Or Sets The Font Of The Button Using The SdlDotNet.Graphics.Font Class
        /// </summary>
        public SdlDotNet.Graphics.Font Font {
            get { return font; }
            set {
                if (font != null) {
                    font.Close();
                }
                font = value;
                RequestRedraw();
            }
        }

        /// <summary>
        /// Gets Or Sets The HighLight Color Of The Button When The Mouse Cursor Is Over The Control Using The System.Drawing.Color Class
        /// </summary>
        public Color HighlightColor {
            get { return highlightColor; }
            set {
                if (highlightColor != value) {
                    highlightColor = value;
                    RequestRedrawHighlightSurface();
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets Or Sets The Surface Of The Button Using The SdlDotNet.Graphics.Surface Class
        /// </summary>
        public SdlDotNet.Graphics.Surface HighlightSurface {
            get { return highlightSurface; }
            set {
                if (highlightSurface != null) {
                    highlightSurface.Close();
                }
                highlightSurface = value;
                highlightSurface.Alpha = 0;
                highlightSurface.AlphaBlending = true;
                RequestRedraw();
            }
        }

        public HighlightType HighlightType {
            get { return highlightType; }
            set {
                if (highlightType != value) {
                    highlightType = value;
                    RequestRedrawHighlightSurface();
                }
            }
        }

        /// <summary>
        /// Gets Or Sets Whether The Button Widget Has Been Selected Or Not.
        /// </summary>
        public bool Selected {
            get { return selected; }
            set {
                if (selected != value) {
                    selected = value;
                    selectedChanged = true;
                    RequestRedrawHighlightSurface();
                    RequestRedraw();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text to be rendered on the button
        /// </summary>
        public string Text {
            get { return text; }
            set {
                if (text != value) {
                    text = value;
                    RequestRedraw();
                }
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
                if (highlightSurface != null) {
                    highlightSurface.Close();
                    highlightSurface = null;
                }
            }
        }

        public override void OnKeyboardUp(SdlDotNet.Input.KeyboardEventArgs e) {
            base.OnKeyboardUp(e);
            if (e.Key == SdlDotNet.Input.Key.Space) {
                if (spaceDown) {
                    spaceDown = false;
                    //base.InvokeClick();
                    RequestRedraw();
                }
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            mouseDown = true;
            RequestRedraw();
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            mouseDown = false;
            RequestRedraw();
        }

        protected override void DrawBorder() {
            switch (base.BorderStyle) {
                case BorderStyle.FixedSingle: {
                        for (int i = 0; i < base.BorderWidth; i++) {
                            SdlDotNet.Graphics.IPrimitive border = new SdlDotNet.Graphics.Primitives.Box((short)(i + 1), (short)(i + 1), (short)((this.Width) - (2 + i)), (short)((this.Height - (2 + i))));
                            base.Buffer.Draw(border, Color.Black);
                        }
                        break;
                    }
                case BorderStyle.Fixed3D: {
                        if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton) == false || mouseDown == false) {
                            if (SdlDotNet.Input.Keyboard.IsKeyPressed(SdlDotNet.Input.Key.Space) == false) {
                                spaceDown = false;
                            }
                            if (spaceDown == false) {
                                SdlDotNet.Graphics.Primitives.Line lineToDraw;

                                lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, 0),
                                                                                    new Point(this.Width - base.BorderWidth, 0));
                                base.Buffer.Draw(lineToDraw, Color.FromArgb(255, 255, 255, 254));
                                lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, 0),
                                                                                    new Point(0, this.Height - base.BorderWidth));
                                base.Buffer.Draw(lineToDraw, Color.FromArgb(255, 255, 255, 254));
                                lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(0, this.Height - base.BorderWidth),
                                                                                    new Point(this.Width - base.BorderWidth, this.Height - base.BorderWidth));
                                base.Buffer.Draw(lineToDraw, Color.FromArgb(73, 0, 0, 0));
                                lineToDraw = new SdlDotNet.Graphics.Primitives.Line(new Point(this.Width - base.BorderWidth, 0),
                                                                                    new Point(this.Width - base.BorderWidth, this.Height - base.BorderWidth));
                                base.Buffer.Draw(lineToDraw, Color.FromArgb(73, 0, 0, 0));
                            } else {
                                base.DrawBorder();
                            }
                        } else {
                            base.DrawBorder();
                        }
                        break;
                    }
                default: {
                        base.DrawBorder();
                        break;
                    }
            }
        }

        void Button_Paint(object sender, EventArgs e) {
            CheckHighlightSurface();
            CheckFont();
            if (highlightType == HighlightType.Color) {
                base.Buffer.Blit(highlightSurface, new Point(base.BorderWidth, base.BorderWidth), new Rectangle(base.BorderWidth, base.BorderWidth, base.Width - (base.BorderWidth * 2), base.Height - (base.BorderWidth * 2)));
            } else if (highlightType == HighlightType.Image) {
                base.Buffer.Blit(highlightSurface, DrawingSupport.GetCenter(this.Size, highlightSurface.Size));
            }
            if (!string.IsNullOrEmpty(text) && font != null) {
                SdlDotNet.Graphics.Surface fontSurf = font.Render(text, base.ForeColor);
                base.Buffer.Blit(fontSurf, DrawingSupport.GetCenter(base.Buffer, fontSurf.Size));
                fontSurf.Close();
            }
            DrawBorder();
        }

        void Button_KeyDown(object sender, SdlDotNet.Input.KeyboardEventArgs e) {
            if (e.Key == SdlDotNet.Input.Key.Space) {
                spaceDown = true;
                base.TriggerClickEvent(null);
                RequestRedraw();
            }
        }

        void Button_MouseEnter(object sender, EventArgs e) {
            if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton) == false) {
                if (highlightSurface.Alpha != 150) {
                    highlightSurface.Alpha = 150;
                    RequestRedraw();
                }
                mouseDown = false;
            }
        }

        void Button_MouseLeave(object sender, EventArgs e) {
            if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton) == false) {
                if (!selected) {
                    if (highlightSurface.Alpha != 0) {
                        highlightSurface.Alpha = 0;
                        RequestRedraw();
                    }
                }
                mouseDown = false;
            }
        }

        void Button_Resized(object sender, EventArgs e) {
            lock (lockObject) {
                if (highlightType == SdlDotNet.Widgets.HighlightType.Color && highlightSurface != null) {
                    highlightSurface.Close();
                    highlightSurface = null;
                }
                CheckHighlightSurface();
            }
        }

        private void CheckFont() {
            if (font == null) {
                font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            }
        }

        private void CheckHighlightSurface() {
            if (highlightSurface == null) {
                highlightSurface = new SdlDotNet.Graphics.Surface(base.UnscaledSize);
                highlightSurface.Alpha = 0;
                highlightSurface.AlphaBlending = true;

                RequestRedrawHighlightSurface();
            }
            if (redrawHighlightSurfaceRequested) {
                RedrawHighlightSurface();
            }
        }

        private void RequestRedrawHighlightSurface() {
            redrawHighlightSurfaceRequested = true;
        }

        private void RedrawHighlightSurface() {
            switch (highlightType) {
                case HighlightType.Color: {
                        if (highlightColor != Color.Empty) {
                            highlightSurface.Fill(highlightColor);
                        }
                    }
                    break;
            }

            if (selectedChanged) {
                selectedChanged = false;
                if (selected) {
                    highlightSurface.Alpha = 150;
                } else {
                    highlightSurface.Alpha = 0;
                }
            }
        }

        #endregion Methods
    }
}