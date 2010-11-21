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

    public class Label : Widget
    {
        #region Fields

        bool antiAlias = true;
        bool autoScroll = false;
        bool autoSize;
        bool centered = false;
        List<CharRenderOptions> charRenderOptions;
        SdlDotNet.Graphics.Font font;
        Color hoverColor;
        HScrollBar hScroll;
        Object lockObject = new object();
        int maxWidth;
        string text;
        VScrollBar vScroll;
        bool wordWrap;
        bool autoScrollSet = false;
        bool scrollToBottom;

        #endregion Fields

        #region Constructors

        public Label(string name)
            : base(name) {
            this.BackColor = Color.Transparent;

            charRenderOptions = new List<CharRenderOptions>();

            base.MouseEnter += new EventHandler(Label_MouseEnter);
            base.MouseLeave += new EventHandler(Label_MouseLeave);
            base.Resized += new EventHandler(Label_Resized);
        }

        #endregion Constructors

        #region Delegates

        private delegate void SetTextDelegate(string newText);

        #endregion Delegates

        #region Properties

        /// <summary>
        /// Gets Or Sets Whether The Text Of The Label Will Be Using AntiAlias.
        /// </summary>
        public bool AntiAlias {
            get { return antiAlias; }
            set {
                antiAlias = value;
                SetText(Text);
            }
        }

        /// <summary>
        /// Gets Or Sets Whether AutoScrolling Will Be Used In The Label Or Not. If True Then A HScrollBar and VScrollBar will be provided on the bounded text.
        /// </summary>
        public bool AutoScroll {
            get { return autoScroll; }
            set {
                autoScroll = value;
                SetText(Text);
            }
        }

        /// <summary>
        /// Gets Or Sets Whether The Text Of The Label Will AutoSize Or Not. If False Then The Size Property Will Need To be used.
        /// </summary>
        public bool AutoSize {
            get { return autoSize; }
            set {
                autoSize = value;
                SetText(Text);
            }
        }

        /// <summary>
        /// Gets Or Sets Whether The Text Of The Label Will Center Itself Or Not.
        /// </summary>
        public bool Centered {
            get { return centered; }
            set {
                centered = value;
                SetText(Text);
            }
        }

        public List<CharRenderOptions> CharRenderOptions {
            get { return charRenderOptions; }
        }

        /// <summary>
        /// Gets Or Sets The Font Of The Label Using The SdlDotNet.Graphics.Font Class
        /// </summary>
        public SdlDotNet.Graphics.Font Font {
            get {
                CheckFont();
                return font;
            }
            set {
                if (font != null) {
                    font.Close();
                }
                font = value;
            }
        }

        public new Color ForeColor {
            get { return base.ForeColor; }
            set {
                if (base.ForeColor != value) {
                    base.ForeColor = value;
                    RequestRedraw();
                }
            }
        }

        public Color HoverColor {
            get { return hoverColor; }
            set { hoverColor = value; }
        }

        public int MaxWidth {
            get { return maxWidth; }
            set { maxWidth = value; }
        }

        /// <summary>
        /// Gets Or Sets The Text Of The Label To Be Drawn.
        /// </summary>
        public string Text {
            get { return text; }
            set {
                if (text != value) {
                    SetText(value);
                }
            }
        }

        public bool WordWrap {
            get { return wordWrap; }
            set {
                wordWrap = value;
                SetText(text);
            }
        }

        #endregion Properties

        #region Methods

        public void AppendText(string text, CharRenderOptions[] renderOptions) {
            charRenderOptions.AddRange(renderOptions);
            SetText(this.text += System.Text.RegularExpressions.Regex.Unescape(text));
        }

        public void AppendText(string text, CharRenderOptions renderOptions) {
            for (int i = 0; i < text.Length; i++) {
                charRenderOptions.Add(renderOptions);
            }
            SetText(this.text += System.Text.RegularExpressions.Regex.Unescape(text));
        }

        public override void FreeResources() {
            lock (lockObject) {
                base.FreeResources();
                if (font != null) {
                    font.Close();
                }
                if (hScroll != null) {
                    hScroll.FreeResources();
                }
                if (vScroll != null) {
                    vScroll.FreeResources();
                }
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            if (vScroll != null && vScroll.Visible) {
                vScroll.OnMouseDown(e);
            }
        }

        public override void OnMouseMotion(Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            if (vScroll != null && vScroll.Visible) {
                vScroll.OnMouseMotion(e);
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            if (vScroll != null && vScroll.Visible) {
                vScroll.OnMouseUp(e);
            }
        }

        public int RoundToMultiple(int number, int multiple) {
            double d = number / multiple;
            d = System.Math.Round(d, 0);
            return Convert.ToInt32(d * multiple);
        }

        public void ScrollToBottom() {
            if (vScroll != null && vScroll.Visible) {
                //vScroll.Value = vScroll.Maximum;
                scrollToBottom = true;
            }
        }

        public void SetRenderOption(CharRenderOptions options, int startIndex, int length) {
            lock (lockObject) {
                for (int i = 0; i < length; i++) {
                    this.charRenderOptions[i + startIndex] = options;
                }

                base.RequestRedraw();
            }
        }

        public void SetRenderOption(CharRenderOptions[] options, int startIndex) {
            lock (lockObject) {
                for (int i = 0; i < options.Length; i++) {
                    this.charRenderOptions[i + startIndex] = options[i];
                }

                base.RequestRedraw();
            }
        }

        public void SetText(string newText, List<CharRenderOptions> renderOptions) {
            charRenderOptions = renderOptions;
            SetText(newText);
        }

        protected override void DrawBuffer() {
            lock (lockObject) {
                if (this.Updating == false) {
                    this.Updating = true;
                    if (string.IsNullOrEmpty(text)) {
                        base.DrawBuffer();
                    }
                    if (!string.IsNullOrEmpty(text)) {
                        CheckFont();

                        //if (autoSize) {
                        //    Size newSize = TextRenderer.SizeText(font, text, antiAlias, maxWidth);
                        //    if (this.Size != newSize) {
                        //        ResizeInternal(new Size(newSize.Width + 10, newSize.Height));
                        //    }
                        //}
                        base.DrawBuffer();

                        //string escapedText = System.Text.RegularExpressions.Regex.Escape(text);
                        //if (escapedText.Length != charRenderOptions.Count) {
                        //    charRenderOptions.Clear();

                        //    for (int i = 0; i < escapedText.Length; i++) {
                        //        charRenderOptions.Add(new CharRenderOptions(this.ForeColor));
                        //    }
                        //}

                        if (autoScroll && autoScrollSet) {
                            int width = this.Width;
                            //Size textSize1 = TextRenderer.SizeText2(font, text, antiAlias, width);
                            Size textSize2 = TextRenderer.SizeText2(font, text, antiAlias, width - 12);//vScroll.Width);
                            Size goodSize = new Size();

                            //if (textSize2.Height > this.Height) {
                            //    goodSize = textSize2;
                            //} else if (textSize1.Height > this.Height) {
                            //    goodSize = textSize1;
                            //}
                            goodSize = textSize2;

                            if (goodSize.Height > this.Height) {
                                CheckVScrollBar();
                                vScroll.Location = new Point(this.Width - 12, 0);
                                vScroll.Size = new Size(12, this.Height);
                                vScroll.Minimum = 0;
                                //vScroll.Value = 0;
                                vScroll.Maximum = RoundToMultiple((goodSize.Height / font.Height), 1) - (this.Height / font.Height) + 1;
                                vScroll.Visible = true;
                                if (scrollToBottom) {
                                    vScroll.Value = vScroll.Maximum;
                                    scrollToBottom = false;
                                }
                            } else {
                                if (vScroll != null && vScroll.Visible) {
                                    vScroll.Visible = false;
                                }
                            }
                            autoScrollSet = false;
                        }

                        SdlDotNet.Graphics.Surface textSurface;
                        Color textColor;
                        if (hoverColor != Color.Empty && IsMouseInBounds()) {
                            textColor = hoverColor;
                        } else {
                            textColor = this.ForeColor;
                        }
                        if (vScroll != null && vScroll.Visible && hScroll != null && hScroll.Visible) {
                            textSurface = TextRenderer.RenderTextBasic2(this.font, text, charRenderOptions.ToArray(), textColor, false, this.Width - vScroll.Width - 5, this.Height - hScroll.Height, 0, 0);
                        } else if (vScroll != null && vScroll.Visible) {
                            textSurface = TextRenderer.RenderTextBasic2(this.font, text, charRenderOptions.ToArray(), textColor, false, this.Width - vScroll.Width - 5, this.Height, 0, vScroll.Value);
                        } else if (hScroll != null && hScroll.Visible) {
                            textSurface = TextRenderer.RenderTextBasic2(this.font, text, charRenderOptions.ToArray(), textColor, false, this.Width - 5, this.Height - hScroll.Height, 0, 0);
                        } else {
                            textSurface = TextRenderer.RenderTextBasic2(this.font, text, charRenderOptions.ToArray(), textColor, false, this.Width - 5, this.Height, 0, 0);
                        }

                        if (textSurface != null) {
                            Point drawPoint;
                            if (!centered) {
                                drawPoint = new Point(5, 0);
                            } else {
                                drawPoint = DrawingSupport.GetCenter(this.Size, textSurface.Size);
                            }
                            base.Buffer.Blit(textSurface, drawPoint);
                            textSurface.Close();
                        }
                    }
                    if (vScroll != null) {
                        vScroll.BlitToScreen(base.Buffer);
                    }
                    base.DrawBorder();
                    this.Updating = false;
                }
            }
        }

        private void CheckFont() {
            if (font == null) {
                font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            }
        }

        private void CheckVScrollBar() {
            if (vScroll == null) {
                vScroll = new VScrollBar("vScroll");
                vScroll.Parent = this;
                vScroll.Visible = false;

                vScroll.ValueChanged += new EventHandler<ValueChangedEventArgs>(vScroll_ValueChanged);
            }
        }

        void ChildWidget_Redraw(object sender, EventArgs e) {
            RequestRedraw();
        }

        void Label_MouseEnter(object sender, EventArgs e) {
            if (hoverColor != Color.Empty) {
                RequestRedraw();
            }
        }

        void Label_MouseLeave(object sender, EventArgs e) {
            if (hoverColor != Color.Empty) {
                RequestRedraw();
            }
        }

        void Label_Resized(object sender, EventArgs e) {
            RecalculateScrollPositions();
            RequestRedraw();
        }

        private void RecalculateScrollPositions() {
            if (!string.IsNullOrEmpty(text) && font != null) {

                int width = this.Width;
                if (vScroll != null && vScroll.Visible) {
                    width -= vScroll.Width;
                }
                Size textSize = TextRenderer.SizeText(font, text, antiAlias, width);

                //vScroll.Value = 0;
                if (textSize.Height > this.Height) {
                    CheckVScrollBar();
                    vScroll.Location = new Point(this.Width - 12, 0);
                    vScroll.Size = new Size(12, this.Height);
                    vScroll.Minimum = 0;
                    vScroll.Maximum = RoundToMultiple((textSize.Height / font.Height), 1) - (this.Height / font.Height);
                    vScroll.Visible = true;
                } else {
                    if (vScroll != null) {
                        vScroll.Value = 0;
                        //vScroll.Maximum = 1;
                        vScroll.Visible = false;
                    }
                }
            }
        }

        private void SetText(string newText) {
            lock (lockObject) {
                if (newText == null) {
                    newText = "";
                }
                text = System.Text.RegularExpressions.Regex.Unescape(newText);
                if (newText.Length != charRenderOptions.Count) {
                    charRenderOptions.Clear();

                    for (int i = 0; i < newText.Length; i++) {
                        charRenderOptions.Add(new CharRenderOptions(this.ForeColor));
                    }
                }

                if (autoSize) {
                    CheckFont();
                    Size newSize = TextRenderer.SizeText2(font, text, antiAlias, maxWidth);
                    if (this.Size != newSize) {
                        //this.updatingText = true;
                        if (!TopLevel && ParentContainer != null) {
                            ParentContainer.ClearRegion(this.Bounds, this);
                        }
                        this.Size = new Size(newSize.Width + 10, newSize.Height);
                        //this.updatingText = false;
                        //ResizeInternal(new Size(newSize.Width + 10, newSize.Height));
                    }
                    if (vScroll != null && vScroll.Visible) {
                        vScroll.Hide();
                    }
                }
                if (autoScroll) {
                    autoScrollSet = true;
                }

                base.RequestRedraw();
            }
        }

        void vScroll_ValueChanged(object sender, ValueChangedEventArgs e) {
            RequestRedraw();
        }

        #endregion Methods
    }
}