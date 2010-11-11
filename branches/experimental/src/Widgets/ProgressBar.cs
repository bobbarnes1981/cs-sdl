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

    public class ProgressBar : Widget
    {
        #region Fields

        Color barColor;
        SdlDotNet.Graphics.Font font;
        int max;
        int min;
        int percent;
        int step;
        string text;
        ProgressBarTextStyle textStyle;
        int val;

        #endregion Fields

        #region Constructors

        public ProgressBar(string name)
            : base(name) {
            font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            barColor = Color.Green;
            max = 100;
            min = 0;
            val = 0;
            percent = 0;
            step = 1;
            textStyle = ProgressBarTextStyle.Custom;
            base.BorderStyle = BorderStyle.Fixed3D;
        }

        #endregion Constructors

        #region Events

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion Events

        #region Properties

        public Color BarColor {
            get { return barColor; }
            set {
                barColor = value;
                RequestRedraw();
            }
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
            get { return max; }
            set {
                if (val < min) {
                    val = min;
                }

                if (value <= 0) {
                    value = 1;
                }

                max = value;

                if (val > max) {
                    val = max;
                }
                UpdateText();
                RequestRedraw();
            }
        }

        public int Minimum {
            get { return min; }
            set {
                if (value < 0) {
                    value = 0;
                }

                if (value > max) {
                    value = max;
                }

                if (val < value) {
                    val = value;
                }

                min = value;
                UpdateText();
                RequestRedraw();
            }
        }

        public int Percent {
            get { return percent; }
        }

        public int Step {
            get { return step; }
            set { step = value; }
        }

        public string Text {
            get { return text; }
            set {
                text = value;
                RequestRedraw();
            }
        }

        public ProgressBarTextStyle TextStyle {
            get { return textStyle; }
            set {
                textStyle = value;
                UpdateText();
                RequestRedraw();
            }
        }

        public int Value {
            get { return val; }
            set {
                int oldVal = val;
                if (value < min) {
                    value = min;
                } else if (value > max) {
                    value = max;
                }

                val = value;
                percent = (int)(val * 100 / max);

                if (ValueChanged != null)
                    ValueChanged(this, new ValueChangedEventArgs(oldVal, val));
                UpdateText();
                RequestRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (font != null) {
                font.Close();
                font = null;
            }
        }

        public void PerformReverseStep() {
            Value -= step;
        }

        /// <summary>
        /// Advances the current position of the progress bar by the amount of the Step property.
        /// </summary>
        public void PerformStep() {
            Value += step;
        }

        protected override void DrawBuffer() {
            base.DrawBuffer();
            if (percent > 0) {
                decimal newWidth = (decimal)base.Width * ((decimal)percent / (decimal)100);
                Size barSize = new Size(System.Math.Max(0, (int)newWidth), System.Math.Max(0, base.Height));
                SdlDotNet.Graphics.Primitives.Box bar = new SdlDotNet.Graphics.Primitives.Box(new Point(0, 0), barSize);
                base.Buffer.Draw(bar, barColor, false, true);
            }
            if (!string.IsNullOrEmpty(text) && font != null) {
                SdlDotNet.Graphics.Surface fontSurf = font.Render(text, base.ForeColor, true);
                base.Buffer.Blit(fontSurf, DrawingSupport.GetCenter(base.Buffer, fontSurf.Size), new Rectangle(0, 0, this.Width, this.Height));
                fontSurf.Close();
                fontSurf = null;
            }
            base.DrawBorder();
        }

        private void UpdateText() {
            switch (textStyle) {
                case ProgressBarTextStyle.Percent: {
                        this.text = percent.ToString() + "%";
                        break;
                    }
                case ProgressBarTextStyle.Value: {
                        this.text = val.ToString();
                        break;
                    }
                case ProgressBarTextStyle.ValueAndMaximum: {
                        this.text = val.ToString() + "/" + max.ToString();
                        break;
                    }
            }
        }

        #endregion Methods
    }
}