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

    public class VScrollBar : Widget
    {
        #region Fields

        Button btnCursor;
        Button btnDown;
        Button btnUp;
        int buttonHeight;
        int clickTickVal;
        Point cursorOffset;
        bool inverted;
        int lastTick;
        Object lockObject = new object();
        int maximum;
        int minimum;
        bool raiseVal;
        int scrollDelay;
        int scrollSpeed;
        int value;

        #endregion Fields

        #region Constructors

        public VScrollBar(string name)
            : base(name) {
            maximum = 100;
            minimum = 0;
            value = 0;
            buttonHeight = 12;
            btnDown = new Button("btnDown");
            btnUp = new Button("btnUp");
            btnCursor = new Button("btnScrollBar");
            btnDown.Parent = this;
            btnUp.Parent = this;
            btnCursor.Parent = this;

            btnDown.BackColor = Color.Transparent;
            btnDown.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/VScrollBar/button_down.png");
            btnDown.HighlightType = HighlightType.None;
            btnDown.BorderStyle = BorderStyle.None;
            btnDown.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnUp.BackColor = Color.Transparent;
            btnUp.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/VScrollBar/button_up.png");
            btnUp.HighlightType = HighlightType.None;
            btnUp.BorderStyle = BorderStyle.None;
            btnUp.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnCursor.BackColor = Color.Transparent;
            btnCursor.BackgroundImage = new Surface(Widgets.ResourceDirectory + "/VScrollBar/cursor.png");
            btnCursor.HighlightType = HighlightType.None;
            btnCursor.BorderStyle = BorderStyle.None;
            btnCursor.BackgroundImageSizeMode = ImageSizeMode.StretchImage;

            btnDown.Redraw += new EventHandler(Button_Redraw);
            btnUp.Redraw += new EventHandler(Button_Redraw);
            btnCursor.Redraw += new EventHandler(Button_Redraw);

            btnUp.Click += new EventHandler<MouseButtonEventArgs>(btnUp_Click);
            btnDown.Click += new EventHandler<MouseButtonEventArgs>(btnDown_Click);
            clickTickVal = -1;
            scrollDelay = 200;
            scrollSpeed = 50;

            base.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion Events

        #region Properties

        public int ButtonHeight {
            get { return buttonHeight; }
            set { buttonHeight = value; }
        }

        public bool Inverted {
            get { return inverted; }
            set { inverted = value; }
        }

        public int Maximum {
            get { return maximum; }
            set {
                if (this.maximum != value) {
                    maximum = value;
                    if (maximum < 0)
                        maximum = 0;
                    if (this.value > maximum)
                        this.value = maximum;
                    RecalculateScrollButton();
                    RequestRedraw();
                }
            }
        }

        public int Minimum {
            get { return minimum; }
            set {
                if (this.minimum != value) {
                    minimum = value;
                    if (this.value < minimum)
                        this.value = minimum;
                    if (minimum > maximum)
                        minimum = maximum;
                    RecalculateScrollButton();
                    RequestRedraw();
                }
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
                if (base.Size != value) {
                    base.Size = value;
                    RecalculateButtonPositions();
                    RecalculateScrollButton();
                    RequestRedraw();
                }
            }
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
                    RecalculateScrollButton();
                    RequestRedraw();
                    if (ValueChanged != null)
                        ValueChanged(this, new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            lock (lockObject) {
                base.FreeResources();
                if (btnUp != null) {
                    btnUp.FreeResources();
                }
                if (btnDown != null) {
                    btnDown.FreeResources();
                }
                if (btnCursor != null) {
                    btnCursor.FreeResources();
                }
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            lock (lockObject) {
                base.OnMouseDown(e);
                //Point location = this.ScreenLocation;
                //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
                if (btnUp != null && DrawingSupport.PointInBounds(e.RelativePosition, btnUp.Bounds)) {
                    btnUp.OnMouseDown(new MouseButtonEventArgs(e));
                    clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                    raiseVal = false;
                }
                if (btnDown != null && DrawingSupport.PointInBounds(e.RelativePosition, btnDown.Bounds)) {
                    btnDown.OnMouseDown(new MouseButtonEventArgs(e));
                    clickTickVal = SdlDotNet.Core.Timer.TicksElapsed;
                    raiseVal = true;
                }
                if (btnCursor != null && DrawingSupport.PointInBounds(e.RelativePosition, btnCursor.Bounds)) {
                    btnCursor.OnMouseDown(new MouseButtonEventArgs(e));
                    cursorOffset = new Point(e.RelativePosition.X - btnCursor.X, e.RelativePosition.Y - btnCursor.Y);
                    WindowManager.heldScroller = this;
                }
            }
        }

        public override void OnMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            lock (lockObject) {
                base.OnMouseMotion(e);
                Point location = this.ScreenLocation;
                Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
                if (btnUp != null && DrawingSupport.PointInBounds(relPoint, btnUp.Bounds)) {
                    btnUp.OnMouseMotion(e);
                }
                if (btnDown != null && DrawingSupport.PointInBounds(relPoint, btnDown.Bounds)) {
                    btnDown.OnMouseMotion(e);
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
                        btnCursor.Y = relPoint.Y - cursorOffset.Y;

                        if (btnCursor.Y < btnCursor.Y + buttonHeight)
                            btnCursor.Y = btnCursor.Y + buttonHeight;
                        else if (btnCursor.Y > btnCursor.Y + Height - btnCursor.Height - 9)
                            btnCursor.Y = btnCursor.Y + Height - btnCursor.Height - 9;

                        float y = btnCursor.Y - (buttonHeight * 2);
                        int value = 0;

                        if (!inverted)
                            value = (int)System.Math.Round(y / ((Height - (buttonHeight * 2)) - btnCursor.Height) * (maximum - minimum));
                        else
                            value = (maximum - minimum) - (int)System.Math.Round(y / ((Height - (buttonHeight * 2)) - btnCursor.Height) * (maximum - minimum));

                        value += minimum;
                        if (value < minimum) {
                            value = minimum;
                            //RecalculateScrollButton();
                            //DrawBuffer();
                        } else if (value > maximum) {
                            value = maximum;
                            //RecalculateScrollButton();
                            //DrawBuffer();
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
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            lock (lockObject) {
                base.OnMouseUp(e);
                //Point location = this.ScreenLocation;
                //Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
                if (btnUp != null && DrawingSupport.PointInBounds(e.RelativePosition, btnUp.Bounds)) {
                    btnUp.OnMouseUp(new MouseButtonEventArgs(e));
                }
                if (btnDown != null && DrawingSupport.PointInBounds(e.RelativePosition, btnDown.Bounds)) {
                    btnDown.OnMouseUp(new MouseButtonEventArgs(e));
                }
                if (btnCursor != null && DrawingSupport.PointInBounds(e.RelativePosition, btnCursor.Bounds)) {
                    btnCursor.OnMouseUp(new MouseButtonEventArgs(e));
                }
            }
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            lock (lockObject) {
                base.OnTick(e);
                if (btnUp != null) {
                    btnUp.OnTick(e);
                }
                if (btnDown != null) {
                    btnDown.OnTick(e);
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
        }

        protected override void DrawBuffer() {
            lock (lockObject) {
                base.DrawBuffer();
                if (btnUp != null) {
                    btnUp.BlitToScreen(base.Buffer);
                }
                if (btnDown != null) {
                    btnDown.BlitToScreen(base.Buffer);
                }
                if (btnCursor != null) {
                    btnCursor.BlitToScreen(base.Buffer);
                }
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
            lock (lockObject) {
                btnUp.Location = new Point(0, 0);
                btnUp.Size = new Size(this.Width, buttonHeight);

                btnDown.Location = new Point(0, this.Height - buttonHeight);
                btnDown.Size = new Size(this.Width, buttonHeight);

                RequestRedraw();
            }
        }

        private void RecalculateScrollButton() {
            lock (lockObject) {
                if ((maximum - minimum) != 0) {
                    btnCursor.Height = System.Math.Max(10, ((Height / 2) - (buttonHeight * 2)) - System.Math.Max(10, (maximum - minimum) / 4));
                    btnCursor.Width = this.Width;

                    btnCursor.X = 0;
                    if (!((maximum - minimum) == 0)) {
                        if (!inverted)
                            btnCursor.Y = (int)(buttonHeight + (Height - (buttonHeight + 1) - btnCursor.Height - buttonHeight) * ((float)(value - minimum) / (float)(maximum - minimum)));
                        else
                            btnCursor.Y = (int)(buttonHeight + (Height - (buttonHeight + 1) - btnCursor.Height - buttonHeight) * ((float)((maximum - minimum) - (value - minimum)) / (float)(maximum - minimum)));
                    } else {
                        btnCursor.Y = buttonHeight;
                    }
                } else {
                    btnCursor.Size = new Size(this.Width, this.Height - (buttonHeight * 2));
                    btnCursor.Location = new Point(0, buttonHeight);
                }
            }
        }

        #endregion Methods
    }
}