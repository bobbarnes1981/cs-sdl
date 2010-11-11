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

    public class Timer : Component
    {
        #region Fields

        int interval;
        int lastTick;
        bool running;

        #endregion Fields

        #region Constructors

        public Timer(string name)
            : base(name) {
            running = false;
        }

        #endregion Constructors

        #region Events

        public event EventHandler Elapsed;

        #endregion Events

        #region Properties

        public int Interval {
            get { return interval; }
            set { interval = value; }
        }

        public int LastTick {
            get { return lastTick; }
        }

        #endregion Properties

        #region Methods

        public override void FreeResources() {
            // There are no resources to free
        }

        public override void OnTick(SdlDotNet.Core.TickEventArgs e) {
            if (running) {
                if (e.Tick > lastTick + interval) {
                    if (Elapsed != null)
                        Elapsed(this, null);
                    lastTick = e.Tick;
                }
            }
            base.OnTick(e);
        }

        public void Start() {
            running = true;
            lastTick = SdlDotNet.Core.Timer.TicksElapsed;
        }

        public void Stop() {
            running = false;
        }

        #endregion Methods
    }
}