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

    public class WindowSwitcher : Widget
    {
        #region Fields

        Button btnLeft;
        Button btnRight;
        List<WindowSwitcherButton> buttons;
        bool loaded = false;
        SdlDotNet.Graphics.Surface menuSurface;
        int visibleX;

        #endregion Fields

        #region Constructors

        public WindowSwitcher(int width)
            : base("") {
            buttons = new List<WindowSwitcherButton>();
            menuSurface = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/Window Switcher/menu.png");
            menuSurface.Transparent = true;
            menuSurface.TransparentColor = Color.Transparent;
            loaded = true;

            btnLeft = new Button("btnLeft");
            btnLeft.Parent = this;
            btnLeft.Visible = false;

            btnRight = new Button("btnRight");
            btnRight.Parent = this;
            btnRight.Visible = false;

            base.Size = new Size(width, 20);
            base.BackColor = Color.Transparent;
            base.BorderStyle = BorderStyle.None;

            base.Paint += new EventHandler(WindowSwitcher_Paint);
        }

      

        #endregion Constructors

        #region Properties

        public new Point Location {
            get { return base.Location; }
            set {
                base.Location = value;
                RecalculateButtons();
            }
        }

        public new Size Size {
            get { return base.Size; }
            set {
                base.Size = value;
                RecalculateButtons();
            }
        }

        #endregion Properties

        #region Methods

        public void AddButton(WindowSwitcherButton buttonToAdd) {
            buttonToAdd.Parent = this;
            buttonToAdd.Visible = true;
            buttonToAdd.Redraw += new EventHandler(WindowButton_Redraw);
            buttons.Add(buttonToAdd);
            int width = CalculateTotalButtonWidth();
            if (width + menuSurface.Width > CalculateWidthMax()) {
                visibleX++;
            }
            RequestRedraw();
        }

        public void ClearButtons() {
            for (int i = buttons.Count - 1; i >= 0; i--) {
                buttons[i].FreeResources();
                buttons.RemoveAt(i);
            }
        }

        public WindowSwitcherButton FindWindowButton(Window windowToFind) {
            for (int i = 0; i < buttons.Count; i++) {
                if (buttons[i].AttachedWindow == windowToFind) {
                    return buttons[i];
                }
            }
            return null;
        }

        public override void FreeResources() {
            base.FreeResources();
            if (menuSurface != null) {
                menuSurface.Close();
                menuSurface = null;
            }
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].FreeResources();
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Point location = this.ScreenLocation;
            Point relPoint = new Point(e.Position.X - location.X, e.Position.Y - location.Y);
            //			for (int i = 0; i < buttons.Count; i++) {
            //				if (DrawingSupport.PointInBounds(relPoint, buttons[i].Bounds)) {
            //					buttons[i].OnMouseDown(e);
            //					break;
            //				}
            //			}
            int totalWidth = menuSurface.Width;
            for (int i = visibleX; i < buttons.Count; i++) {
                if (totalWidth + buttons[i].Width < CalculateWidthMax()) {
                    if (DrawingSupport.PointInBounds(relPoint, buttons[i].Bounds)) {
                        buttons[i].OnMouseDown(e);
                        break;
                    }
                    totalWidth += buttons[i].Width;
                } else {
                    break;
                }
            }
        }

        public void RemoveButton(Window windowToRemove) {
            WindowSwitcherButton button = FindWindowButton(windowToRemove);
            if (button != null) {
                button.FreeResources();
                buttons.Remove(button);
            }
            RequestRedraw();
        }

        void WindowSwitcher_Paint(object sender, EventArgs e) {
            if (loaded) {
                base.Buffer.Blit(menuSurface, new Point(0, 0));
                //				int lastX = menuSurface.Width;
                //				for (int i = 0; i < buttons.Count; i++) {
                //					buttons[i].Location = new Point(lastX, base.Location.Y + 3);
                //					buttons[i].Visible = true;
                //					lastX += buttons[i].Size.Width;
                //				}
                if (btnLeft != null) {
                    btnLeft.BlitToScreen(base.Buffer);
                }
                if (btnRight != null) {
                    btnRight.BlitToScreen(base.Buffer);
                }
                int totalWidth = menuSurface.Width;
                for (int i = visibleX; i < buttons.Count; i++) {
                    if (totalWidth + buttons[i].Width < CalculateWidthMax()) {
                        buttons[i].Location = new Point(totalWidth, 3);
                        buttons[i].BlitToScreen(base.Buffer);
                        totalWidth += buttons[i].Width;
                    } else {
                        break;
                    }
                }
                base.DrawBorder();
            }
        }

        private int CalculateTotalButtonWidth() {
            int width = 0;
            for (int i = 0; i < buttons.Count; i++) {
                width += buttons[i].Width;
            }
            return width;
        }

        private int CalculateWidthMax() {
            int width = this.Width;
            if (btnLeft != null && btnLeft.Visible) {
                width -= btnLeft.Width;
            }
            if (btnRight != null && btnRight.Visible) {
                width -= btnRight.Width;
            }
            return width;
        }

        private void RecalculateButtons() {
            btnLeft.Size = new Size(20, this.Height);
            btnRight.Size = new Size(20, this.Height);

            btnLeft.Location = new Point(this.Width - btnLeft.Width - btnRight.Width, 0);
            btnRight.Location = new Point(this.Width - btnRight.Width, 0);
            RequestRedraw();
        }

        void WindowButton_Redraw(object sender, EventArgs e) {
            RequestRedraw();
        }

        #endregion Methods
    }
}