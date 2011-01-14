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

    public class CheckBox : Widget
    {
        #region Fields

        public const int CHECKBOX_SIZE = 16;

        bool @checked;
        SdlDotNet.Graphics.Surface IsCheckedIsOver;
        SdlDotNet.Graphics.Surface IsCheckedNotOver;
        Label lblText;
        SdlDotNet.Graphics.Surface NotCheckedIsOver;
        SdlDotNet.Graphics.Surface NotCheckedNotOver;

        #endregion Fields

        #region Constructors

        public CheckBox(string name)
            : base(name, true) {
            lblText = new Label("lblText");
            lblText.AutoSize = true;
            lblText.BackColor = Color.Transparent;

            base.InitializeDefaultWidget();

            this.BackColor = Color.Transparent;

            NotCheckedNotOver = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/CheckBox/unchecked.png");
            NotCheckedIsOver = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/CheckBox/unchecked-hover.png");
            IsCheckedNotOver = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/CheckBox/checked.png");
            IsCheckedIsOver = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/CheckBox/checked-hover.png");

            base.MouseEnter += new EventHandler(CheckBox_MouseEnter);
            base.MouseLeave += new EventHandler(Checkbox_MouseLeave);

            base.Paint += new EventHandler(CheckBox_Paint);
        }



        #endregion Constructors

        #region Events

        public event EventHandler CheckChanged;

        #endregion Events

        #region Properties

        public new Color BackColor {
            get { return base.BackColor; }
            set {
                base.BackColor = value;
            }
        }

        public bool Checked {
            get { return @checked; }
            set {
                @checked = value;
                RequestRedraw();
                if (CheckChanged != null)
                    CheckChanged(this, null);
            }
        }

        public SdlDotNet.Graphics.Font Font {
            get { return lblText.Font; }
            set {
                lblText.Font = value;
            }
        }

        public new Size Size {
            get { return base.Size; }
            set {
                //lblText.Size = new Size(value.Width - (4 + CHECKBOX_SIZE + 10), value.Height);
                base.Size = value;
            }
        }

        public string Text {
            get { return lblText.Text; }
            set {
                lblText.Text = value;
                RequestRedraw();
            }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            lblText.FreeResources();
            if (NotCheckedIsOver != null) {
                NotCheckedIsOver.Close();
            }
            if (NotCheckedNotOver != null) {
                NotCheckedNotOver.Close();
            }
            if (IsCheckedIsOver != null) {
                IsCheckedIsOver.Close();
            }
            if (IsCheckedNotOver != null) {
                IsCheckedNotOver.Close();
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Checked = !@checked;
        }

        void CheckBox_Paint(object sender, EventArgs e) {
            Size checkBoxSize = new Size(CHECKBOX_SIZE, CHECKBOX_SIZE);
            //SdlDotNet.Graphics.Surface textSurf = lblText.Render();
            //Point centerPoint = DrawingSupport.GetCenter(base.Buffer, textSurf.Size);
            if (@checked) {
                if (base.MouseInBounds) {
                    base.Buffer.Blit(IsCheckedIsOver);
                } else {
                    base.Buffer.Blit(IsCheckedNotOver);
                }
            } else {
                if (base.MouseInBounds) {
                    base.Buffer.Blit(NotCheckedIsOver);
                } else {
                    base.Buffer.Blit(NotCheckedNotOver);
                }
            }
            lblText.BlitToScreen(base.Buffer, new Point(2 + CHECKBOX_SIZE + 10, 0));
            //base.Buffer.Blit(textSurf, new Point(2 + CHECKBOX_SIZE + 10, centerPoint.Y));

            base.DrawBorder();
        }

        void CheckBox_MouseEnter(object sender, EventArgs e) {
            RequestRedraw();
        }

        void Checkbox_MouseLeave(object sender, EventArgs e) {
            RequestRedraw();
        }

        #endregion Methods
    }
}