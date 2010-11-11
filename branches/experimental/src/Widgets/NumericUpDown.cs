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

    public class NumericUpDown : Widget
    {
        #region Fields

        Button btnDown;
        Button btnUp;
        Size buttonSize;
        int clickTickVal;
        SdlDotNet.Graphics.Font font;
        string inputString = "";
        int lastTick;
        int maximum;
        int minimum;
        bool raiseVal;
        int scrollDelay;
        int scrollSpeed;
        int value;

        #endregion Fields

        #region Constructors

        public NumericUpDown(string name)
            : base(name) {
            minimum = 0;
            maximum = 100;
            value = 0;
            buttonSize = new Size(30, 20);

            font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);

            btnUp = new Button("btnUp");
            btnUp.Size = buttonSize;
            btnUp.Parent = this;

            btnDown = new Button("btnDown");
            btnDown.Size = buttonSize;
            btnDown.Parent = this;

            btnUp.Redraw += new EventHandler(Button_Redraw);
            btnDown.Redraw += new EventHandler(Button_Redraw);
            btnUp.Click += new EventHandler<MouseButtonEventArgs>(btnUp_Click);
            btnDown.Click += new EventHandler<MouseButtonEventArgs>(btnDown_Click);

            base.KeyDown += new EventHandler<Input.KeyboardEventArgs>(NumericUpDown_KeyDown);
            base.Click += new EventHandler<MouseButtonEventArgs>(NumericUpDown_Click);

            clickTickVal = -1;
            scrollDelay = 200;
            scrollSpeed = 50;

            RecalculateButtonPositions();
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion Events

        #region Properties

        public Button DownButton {
            get { return btnDown; }
        }

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

        public int Maximum {
            get { return maximum; }
            set {
                maximum = value;
                if (maximum < 0)
                    maximum = 0;
                if (this.value > maximum)
                    this.value = maximum;
                RequestRedraw();
            }
        }

        public int Minimum {
            get { return minimum; }
            set {
                minimum = value;
                if (this.value < minimum)
                    this.value = minimum;
                if (minimum > maximum)
                    minimum = maximum;
                RequestRedraw();
            }
        }

        public int ScrollDelay {
            get { return scrollDelay; }
            set { scrollDelay = value; }
        }

        public int ScrollSpeed {
            get { return scrollSpeed; }
            set { scrollSpeed = value; }
        }

        public new Size Size {
            get { return base.Size; }
            set {
                base.Size = value;
                buttonSize = new Size(20, value.Height / 2);
                RecalculateButtonPositions();
            }
        }

        public Button UpButton {
            get { return btnUp; }
        }

        public int Value {
            get { return value; }
            set {
                if (this.value != value) {
                    int oldValue = this.value;
                    this.value = value;
                    if (this.value < minimum)
                        this.value = minimum;
                    else if (this.value > maximum)
                        this.value = maximum;
                    RequestRedraw();
                    if (ValueChanged != null)
                        ValueChanged(this, new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (btnUp != null) {
                btnUp.FreeResources();
            }
            if (btnDown != null) {
                btnDown.FreeResources();
            }
            if (font != null) {
                font.Close();
                font = null;
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            if (btnUp != null && DrawingSupport.PointInBounds(e.RelativePosition, btnUp.Bounds)) {
                btnUp.OnMouseDown(new MouseButtonEventArgs(e));
                clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                raiseVal = true;
                if (btnUp.RedrawRequested) {
                    RequestRedraw();
                }
            }
            if (btnDown != null && DrawingSupport.PointInBounds(e.RelativePosition, btnDown.Bounds)) {
                btnDown.OnMouseDown(new MouseButtonEventArgs(e));
                clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                raiseVal = false;
                if (btnDown.RedrawRequested) {
                    RequestRedraw();
                }
            }
        }

        public override void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            Point location = this.ScreenLocation;
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            if (btnUp != null && DrawingSupport.PointInBounds(relPoint, btnUp.Bounds)) {
                btnUp.OnMouseMotion(e);
                if (btnUp.RedrawRequested) {
                    RequestRedraw();
                }
            }
            if (btnDown != null && DrawingSupport.PointInBounds(relPoint, btnDown.Bounds)) {
                btnDown.OnMouseMotion(e);
                if (btnDown.RedrawRequested) {
                    RequestRedraw();
                }
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            if (btnUp != null && DrawingSupport.PointInBounds(e.RelativePosition, btnUp.Bounds)) {
                btnUp.OnMouseUp(new MouseButtonEventArgs(e));
                if (btnUp.RedrawRequested) {
                    RequestRedraw();
                }
            }
            if (btnDown != null && DrawingSupport.PointInBounds(e.RelativePosition, btnDown.Bounds)) {
                btnDown.OnMouseUp(new MouseButtonEventArgs(e));
                if (btnDown.RedrawRequested) {
                    RequestRedraw();
                }
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            base.OnTick(e);
            if (btnUp != null) {
                btnUp.OnTick(e);
            }
            if (btnDown != null) {
                btnDown.OnTick(e);
            }
            if (clickTickVal != -1 && e.Tick > clickTickVal + scrollDelay) {
                if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton)) {
                    if (e.Tick > lastTick + scrollSpeed) {
                        if (raiseVal) {
                            Value++;
                        } else {
                            Value--;
                        }
                        lastTick = e.Tick;
                    }
                } else {
                    clickTickVal = -1;
                }
            }
        }

        protected override void DrawBuffer() {
            base.DrawBuffer();
            if (btnUp != null) {
                btnUp.BlitToScreen(base.Buffer);
            }
            if (btnDown != null) {
                btnDown.BlitToScreen(base.Buffer);
            }
            if (font != null) {
                TextRenderer.RenderText(base.Buffer, this.font, value.ToString(), base.ForeColor, true, this.Width - buttonSize.Width, this.Height / font.Height, 5, ((this.Height - font.Height) / 2));
            }
            for (int i = 0; i < 1; i++) {
                SdlDotNet.Graphics.IPrimitive border = new SdlDotNet.Graphics.Primitives.Box((short)(i), (short)(i), (short)(((this.Width - buttonSize.Width)) - (1 + i)), (short)((this.Height - (1 + i))));
                base.Buffer.Draw(border, base.BorderColor);
            }
        }

        void btnDown_Click(object sender, MouseButtonEventArgs e) {
            Value--;
        }

        void btnUp_Click(object sender, MouseButtonEventArgs e) {
            Value++;
        }

        void Button_Redraw(object sender, EventArgs e) {
            RequestRedraw();
        }

        void NumericUpDown_Click(object sender, MouseButtonEventArgs e) {
            if (DrawingSupport.PointInBounds(e.RelativePosition, btnUp.Bounds) == false &&
                DrawingSupport.PointInBounds(e.RelativePosition, btnDown.Bounds) == false) {
                inputString = "";
                Value = Minimum;
            }
        }

        void NumericUpDown_KeyDown(object sender, Input.KeyboardEventArgs e) {
            if (e.Key != Input.Key.Backspace) {
                string charString = Keyboard.GetCharString(e);
                int result;
                if (Int32.TryParse(charString, out result)) {
                    inputString += result.ToString();
                }
                if (Int32.TryParse(inputString, out result)) {
                    this.Value = result;
                }
            } else {
                if (inputString.Length > 0) {
                    inputString.Remove(inputString.Length - 1, 1);

                    int result;
                    if (Int32.TryParse(inputString, out result)) {
                        this.Value = result;
                    }
                }
            }
        }

        private void RecalculateButtonPositions() {
            btnUp.Size = buttonSize;
            btnDown.Size = buttonSize;
            btnUp.Location = new Point(this.Width - btnUp.Width, 0);
            btnDown.Location = new Point(this.Width - btnUp.Width, this.Height - buttonSize.Height);

            RequestRedraw();
        }

        #endregion Methods
    }
}