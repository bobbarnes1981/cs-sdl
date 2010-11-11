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
    using System.Text;

    public class AnimatedWidget : Widget
    {
        #region Fields

        int currentTick;
        int fps;
        int lastTick;
        int targetTick;
        float ticksPerFrame;

        #endregion Fields

        #region Constructors

        public AnimatedWidget(string name)
            : base(name) {
            fps = -1;
        }

        #endregion Constructors

        #region Events

        public event EventHandler RenderFrame;

        #endregion Events

        #region Properties

        public int FPS {
            get {
                return fps;
            }
            set {
                fps = value;
                if (fps != -1) {
                    ticksPerFrame = (1000.0f / (float)fps);
                } else {
                    ticksPerFrame = 0;
                }
            }
        }

        #endregion Properties

        #region Methods

        public override void OnTick(Core.TickEventArgs e) {
            base.OnTick(e);

            if (fps != -1) {
                currentTick = SdlDotNet.Core.Timer.TicksElapsed;
                targetTick = lastTick + (int)ticksPerFrame;

                if (currentTick > targetTick) {
                    base.RequestRedraw();
                    lastTick = currentTick;
                }
            } else {
                base.RequestRedraw();
            }
        }

        protected override void DrawBuffer() {
            base.DrawBuffer();

            if (RenderFrame != null)
                RenderFrame(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}