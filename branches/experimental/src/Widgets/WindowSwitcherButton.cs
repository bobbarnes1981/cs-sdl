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

    public class WindowSwitcherButton : Widget
    {
        #region Fields

        Window attachedWindow;
        SdlDotNet.Graphics.Surface backgroundSurface;
        SdlDotNet.Graphics.Font font;
        bool loaded = false;

        #endregion Fields

        #region Constructors

        public WindowSwitcherButton(Window attachedWindow)
            : base("WSButton-" + attachedWindow.Name) {
            this.attachedWindow = attachedWindow;
            backgroundSurface = new SdlDotNet.Graphics.Surface(Widgets.ResourceDirectory + "/Window Switcher/menubutton.png");
            loaded = true;
            base.Size = backgroundSurface.Size;
        }

        #endregion Constructors

        #region Properties

        public Window AttachedWindow {
            get { return attachedWindow; }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            base.FreeResources();
            if (font != null) {
                font.Close();
                font = null;
            }
            if (backgroundSurface != null) {
                backgroundSurface.Close();
                backgroundSurface = null;
            }
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            switch (attachedWindow.WindowState) {
                case WindowState.Normal:
                    attachedWindow.WindowState = WindowState.Minimized;
                    break;
                case WindowState.Minimized:
                    attachedWindow.WindowState = WindowState.Normal;
                    break;
            }
            RequestRedraw();
        }

        protected override void DrawBuffer() {
            if (loaded) {
                base.DrawBuffer();
                base.Buffer.Blit(backgroundSurface, new Point(0, 0));
                if (!string.IsNullOrEmpty(attachedWindow.WindowSwitcherText)) {
                    CheckFont();
                    Surface textSurface = TextRenderer.RenderTextBasic2(font, attachedWindow.WindowSwitcherText, null, Color.WhiteSmoke, false, 0, 0, 0, 0);
                    base.Buffer.Blit(textSurface, DrawingSupport.GetCenter(base.Buffer, textSurface.Size));
                    textSurface.Close();
                    string stateString = "?";
                    switch (attachedWindow.WindowState) {
                        case WindowState.Normal:
                            stateString = "^";
                            break;
                        case WindowState.Minimized:
                            stateString = "v";
                            break;
                    }
                    Surface stateSurface = TextRenderer.RenderTextBasic2(font, stateString, null, Color.WhiteSmoke, false, 0, 0, 0, 0);
                    base.Buffer.Blit(stateSurface, new Point(this.Width - font.SizeText(stateString).Width - 1, 0));
                    stateSurface.Close();
                }
            }
        }

        private void CheckFont() {
            if (font == null) {
                font = new SdlDotNet.Graphics.Font(Widgets.DefaultFontPath, Widgets.DefaultFontSize);
            }
        }

        #endregion Methods
    }
}