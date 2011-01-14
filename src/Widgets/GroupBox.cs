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

    public class GroupBox : Widget, IContainer
    {
        #region Fields

        WidgetCollection children;
        SdlDotNet.Graphics.Font font;
        Panel pnlContainer;
        string text;

        #endregion Fields

        #region Constructors

        public GroupBox(string name)
            : base(name) {
            children = new WidgetCollection();

            pnlContainer = new Panel("pnlContainer");
            pnlContainer.BackColor = Color.Transparent;//Color.Green;

            pnlContainer.Parent = this;
            pnlContainer.Redraw += new EventHandler(pnlContainer_Redraw);

            children.AddWidget(pnlContainer);

            base.Resized += new EventHandler(GroupBox_Resized);
            base.Paint += new EventHandler(GroupBox_Paint);
        }

      

        #endregion Constructors

        #region Properties

        public SdlDotNet.Graphics.Font Font {
            get { return font; }
            set { font = value; }
        }

        public string Text {
            get { return text; }
            set { text = value; }
        }

        public WidgetCollection ChildWidgets {
            get { return pnlContainer.ChildWidgets; }
        }

        #endregion Properties

        #region Methods

        public void AddWidget(Widget widget) {
            pnlContainer.AddWidget(widget);
        }

        public void ClearRegion(Rectangle bounds, Widget widgetToSkip) {
            pnlContainer.ClearRegion(bounds, widgetToSkip);
        }

        public override void FreeResources() {
            base.FreeResources();
            if (pnlContainer != null) {
                pnlContainer.FreeResources();
            }
            if (font != null) {
                font.Close();
            }
        }

        public void LoadComplete() {
            pnlContainer.LoadComplete();
        }

        public override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            children.HandleMouseDown(e, this.Location);
        }

        public override void OnMouseMotion(Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            children.HandleMouseMotion(e, this.ScreenLocation);
        }

        public override void OnMouseUp(MouseButtonEventArgs e) {
            base.OnMouseUp(e);
            children.HandleMouseUp(e, this.Location);
        }

        public override void OnTick(Core.TickEventArgs e) {
            base.OnTick(e);
            children.HandleTick(e);
        }

        public void UpdateBuffer() {
            pnlContainer.UpdateBuffer();
        }

        public void UpdateBuffer(bool resetBackground) {
            pnlContainer.UpdateBuffer(resetBackground);
        }

        public void UpdateWidget(Widget widget) {
            pnlContainer.UpdateWidget(widget);
        }

        public override void BlitToScreen(SdlDotNet.Graphics.Surface destinationSurface) {
            pnlContainer.BlitToScreen(base.Buffer);
            base.BlitToScreen(destinationSurface);
        }

        void GroupBox_Paint(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(text) && font != null) {
                //TextRenderer.RenderText(base.Buffer, font, text, base.ForeColor, false, this.Width, 0, 5, 0);
            }
            pnlContainer.BlitToScreen(base.Buffer);
        }

        void GroupBox_Resized(object sender, EventArgs e) {
            pnlContainer.Location = new Point(10, 10);
            pnlContainer.Size = new Size(this.Width - (pnlContainer.X * 2), this.Height - (pnlContainer.Y * 2));

            RequestRedraw();
        }

        void pnlContainer_Redraw(object sender, EventArgs e) {
            //WidgetRenderer.ClearRegion(base.Buffer, pnlContainer.Bounds, pnlContainer, children, this.BackColor);
            WidgetRenderer.UpdateWidget(base.Buffer, pnlContainer, children);
        }

        #endregion Methods
    }
}