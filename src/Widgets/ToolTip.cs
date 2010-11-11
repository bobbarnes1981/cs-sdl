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

    public class ToolTip : Label
    {
        #region Fields

        int lastTick;
        bool vanishing;

        #endregion Fields

        #region Constructors

        public ToolTip(string name)
            : base(name) {
            base.AlphaBlending = true;
            base.Alpha = 255;

            base.BackColor = Color.Yellow;
        }

        #endregion Constructors

        #region Methods

        public override void OnMouseMotion(Input.MouseMotionEventArgs e) {
            base.OnMouseMotion(e);
            //vanishing = false;
            //lastTick = SdlDotNet.Core.Timer.TicksElapsed;
            //base.Alpha = 255;
        }

        public override void OnTick(Core.TickEventArgs e) {
            base.OnTick(e);
            if (lastTick == 0) {
                lastTick = e.Tick;
            }
            if (vanishing == false) {
                //if (base.MouseInBounds == false) {
                //    if (lastTick + 5000 > e.Tick) {
                //        vanishing = true;
                //        lastTick = e.Tick;
                //    }
                //}
            } else {
                if (e.Tick < lastTick + 100) {
                    int newValue = base.Alpha - 10;
                    if (newValue < 0) {
                        newValue = 0;
                    } else if (newValue > 255) {
                        newValue = 255;
                    }
                    base.Alpha = (byte)newValue;
                    lastTick = e.Tick;
                    if (base.Alpha == 0) {
                        WindowManager.RemoveFromOverlayCollection(this);
                    }
                }
            }
        }

        public void ResetVanish() {
            vanishing = false;
            base.Alpha = 255;
        }

        public void ShowTooltip() {
        }

        public void StartVanish() {
            vanishing = true;
            base.Alpha = 255;
            lastTick = 0;
        }

        #endregion Methods
    }
}