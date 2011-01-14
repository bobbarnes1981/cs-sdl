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

    public class HScrollBar : Widget
    {
        #region Fields

        Button btnCursor;
        Button btnLeft;
        Button btnRight;
        int buttonWidth;
        int clickTickVal;
        Point cursorOffset;
        bool inverted;
        int lastTick;
        int maximum;
        int minimum;
        bool raiseVal;
        int scrollDelay;
        int scrollSpeed;
        int value;

        #endregion Fields

        #region Constructors

        public HScrollBar(string name)
            : base(name) {
            maximum = 100;
            minimum = 0;
            value = 0;
            buttonWidth = 12;
            btnRight = new Button("btnDown");
            btnLeft = new Button("btnUp");
            btnCursor = new Button("btnScrollBar");
            btnRight.Parent = this;
            btnLeft.Parent = this;
            btnCursor.Parent = this;

            btnRight.BackColor = Color.Transparent;
            btnRight.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/HScrollBar/button_right.png");
            btnRight.HighlightType = HighlightType.None;
            btnRight.BorderStyle = BorderStyle.None;
            btnRight.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnLeft.BackColor = Color.Transparent;
            btnLeft.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/HScrollBar/button_left.png");
            btnLeft.HighlightType = HighlightType.None;
            btnLeft.BorderStyle = BorderStyle.None;
            btnLeft.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnCursor.BackColor = Color.Transparent;
            btnCursor.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/HScrollBar/cursor.png");
            btnCursor.HighlightType = HighlightType.None;
            btnCursor.BorderStyle = BorderStyle.None;
            btnCursor.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnRight.Redraw += new EventHandler(Button_Redraw);
            btnLeft.Redraw += new EventHandler(Button_Redraw);
            btnCursor.Redraw += new EventHandler(Button_Redraw);

            btnLeft.Click += new EventHandler<MouseButtonEventArgs>(btnUp_Click);
            btnRight.Click += new EventHandler<MouseButtonEventArgs>(btnDown_Click);
            clickTickVal = -1;
            scrollDelay = 200;
            scrollSpeed = 50;

            base.Paint += new EventHandler(HScrollBar_Paint);
        }

        

        #endregion Constructors

        #region Events

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion Events

        #region Properties

        public int ButtonWidth {
            get { return buttonWidth; }
            set { buttonWidth = value; }
        }

        public bool Inverted {
            get { return inverted; }
            set { inverted = value; }
        }

        public int Maximum {
            get { return maximum; }
            set {
                maximum = value;
                if (maximum < 0)
                    maximum = 0;
                if (this.value > maximum)
                    this.value = maximum;
                RecalculateScrollButton();
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
                RecalculateScrollButton();
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
                RecalculateButtonPositions();
                RecalculateScrollButton();
                RequestRedraw();
            }
        }

        public int Value {
            get { return value; }
            set {
                //				if (this.value != value) {
                int oldValue = this.value;
                this.value = value;
                if (this.value < minimum)
                    this.value = minimum;
                else if (this.value > maximum)
                    this.value = maximum;
                RecalculateScrollButton();
                RequestRedraw();
                if (ValueChanged != null)
                    ValueChanged(this, new ValueChangedEventArgs(oldValue, this.value));
                //				}
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (btnLeft != null) {
                btnLeft.FreeResources();
            }
            if (btnRight != null) {
                btnRight.FreeResources();
            }
            if (btnCursor != null) {
                btnCursor.FreeResources();
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            if (btnLeft != null && DrawingSupport.PointInBounds(e.RelativePosition, btnLeft.Bounds)) {
                btnLeft.OnMouseDown(new MouseButtonEventArgs(e));
                clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                raiseVal = false;
            }
            if (btnRight != null && DrawingSupport.PointInBounds(e.RelativePosition, btnRight.Bounds)) {
                btnRight.OnMouseDown(new MouseButtonEventArgs(e));
                clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                raiseVal = true;
            }
            if (btnCursor != null && DrawingSupport.PointInBounds(e.RelativePosition, btnCursor.Bounds)) {
                btnCursor.OnMouseDown(new MouseButtonEventArgs(e));
                cursorOffset = new Point(e.RelativePosition.X - btnCursor.X, e.RelativePosition.Y - btnCursor.Y);
                WindowManager.heldScroller = this;
            }
        }

        public override void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            Point location = this.ScreenLocation;
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            if (btnLeft != null && DrawingSupport.PointInBounds(relPoint, btnLeft.Bounds)) {
                btnLeft.OnMouseMotion(e);
            }
            if (btnRight != null && DrawingSupport.PointInBounds(relPoint, btnRight.Bounds)) {
                btnRight.OnMouseMotion(e);
            }
            if (btnCursor != null && DrawingSupport.PointInBounds(relPoint, btnCursor.Bounds)) {
                btnCursor.OnMouseMotion(e);
            }

            bool validate = true;
            if (DrawingSupport.PointInBounds(relPoint, base.Bounds)) {
                if (btnCursor.MouseInBounds == false) {
                    validate = false;
                }
            }

            if (SdlDotNet.Input.Mouse.IsButtonPressed(SdlDotNet.Input.MouseButton.PrimaryButton) && validate == true) {
                if (maximum - minimum != 0) {
                    btnCursor.X = relPoint.X - cursorOffset.X;

                    if (btnCursor.X < btnCursor.X + buttonWidth)
                        btnCursor.X = btnCursor.X + buttonWidth;
                    else if (btnCursor.X > btnCursor.X + Width - btnCursor.Width - 9)
                        btnCursor.X = btnCursor.X + Width - btnCursor.Width - 9;

                    float x = btnCursor.X - (buttonWidth * 2);
                    int value = 0;

                    if (!inverted)
                        value = (int)System.Math.Round(x / ((Width - (buttonWidth * 2)) - btnCursor.Width) * (maximum - minimum));
                    else
                        value = (maximum - minimum) - (int)System.Math.Round(x / ((Width - (buttonWidth * 2)) - btnCursor.Width) * (maximum - minimum));

                    value += minimum;
                    if (value < minimum) {
                        value = minimum;
                        //						RecalculateScrollButton();
                        //						DrawBuffer();
                    } else if (value > maximum) {
                        value = maximum;
                        //						RecalculateScrollButton();
                        //						DrawBuffer();
                    }

                    if (this.Value != value) {
                        Value = value;
                    } else {
                        RecalculateScrollButton();
                        RequestRedraw();
                    }
                }
            }
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            //Point location = this.ScreenLocation;
            //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            if (btnLeft != null && DrawingSupport.PointInBounds(e.RelativePosition, btnLeft.Bounds)) {
                btnLeft.OnMouseUp(new MouseButtonEventArgs(e));
            }
            if (btnRight != null && DrawingSupport.PointInBounds(e.RelativePosition, btnRight.Bounds)) {
                btnRight.OnMouseUp(new MouseButtonEventArgs(e));
            }
            if (btnCursor != null && DrawingSupport.PointInBounds(e.RelativePosition, btnCursor.Bounds)) {
                btnCursor.OnMouseUp(new MouseButtonEventArgs(e));
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            base.OnTick(e);
            if (btnLeft != null) {
                btnLeft.OnTick(e);
            }
            if (btnRight != null) {
                btnRight.OnTick(e);
            }
            if (btnCursor != null) {
                btnCursor.OnTick(e);
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

        void HScrollBar_Paint(object sender, EventArgs e) {
            if (btnLeft != null) {
                btnLeft.BlitToScreen(base.Buffer);
            }
            if (btnRight != null) {
                btnRight.BlitToScreen(base.Buffer);
            }
            if (btnCursor != null) {
                btnCursor.BlitToScreen(base.Buffer);
            }
        }

        void btnDown_Click(object sender, EventArgs e) {
            Value++;
        }

        void btnUp_Click(object sender, EventArgs e) {
            Value--;
        }

        void Button_Redraw(object sender, EventArgs e) {
            RequestRedraw();
        }

        private void RecalculateButtonPositions() {
            btnLeft.Location = new Point(0, 0);
            btnLeft.Size = new Size(buttonWidth, this.Height);

            btnRight.Location = new Point(this.Width - buttonWidth, 0);
            btnRight.Size = new Size(buttonWidth, this.Height);

            RequestRedraw();
        }

        private void RecalculateScrollButton() {
            if ((maximum - minimum) != 0) {
                btnCursor.Width = System.Math.Max(10, ((Width / 2) - (buttonWidth * 2)) - System.Math.Max(10, (maximum - minimum) / 4));
                btnCursor.Height = this.Height;

                btnCursor.Y = 0;
                if (!inverted)
                    btnCursor.X = (int)(buttonWidth + (Width - (buttonWidth + 1) - btnCursor.Width - buttonWidth) * ((float)(value - minimum) / (float)(maximum - minimum)));
                else
                    btnCursor.X = (int)(buttonWidth + (Width - (buttonWidth + 1) - btnCursor.Width - buttonWidth) * ((float)((maximum - minimum) - (value - minimum)) / (float)(maximum - minimum)));
            } else {
                btnCursor.Size = new Size(this.Width - (buttonWidth * 2), this.Height);
                btnCursor.Location = new Point(buttonWidth, 0);
            }
        }

        #endregion Methods
    }
}