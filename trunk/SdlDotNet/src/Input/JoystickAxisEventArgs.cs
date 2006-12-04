/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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

using System;

using SdlDotNet.Core;
using Tao.Sdl;

namespace SdlDotNet.Input
{
    /// <summary>
    /// Summary description for JoystickAxisEventArgs.
    /// </summary>
    public class JoystickAxisEventArgs : SdlEventArgs
    {
        private const float JOYSTICK_ADJUSTMENT = 32768;
        private const float JOYSTICK_SCALE = 65535;
        private const short JOYSTICK_THRESHHOLD = 3277;

        /// <summary>
        /// Create Event args
        /// </summary>
        /// <param name="device">The joystick index</param>
        /// <param name="axisIndex">The axis index</param>
        /// <param name="axisValue">The new axis value</param>
        public JoystickAxisEventArgs(byte device, byte axisIndex, float axisValue)
        {
            Sdl.SDL_Event evt = new Sdl.SDL_Event();
            evt.jaxis.which = device;
            evt.jaxis.axis = axisIndex;
            evt.jaxis.val =
                (short)((axisValue * JOYSTICK_SCALE) - JOYSTICK_ADJUSTMENT);
            evt.type = (byte)EventTypes.JoystickAxisMotion;
            this.EventStruct = evt;
        }

        internal JoystickAxisEventArgs(Sdl.SDL_Event evt)
            : base(evt)
        {
        }

        /// <summary>
        /// Return device
        /// </summary>
        public byte Device
        {
            get
            {
                return this.EventStruct.jaxis.which;
            }
        }

        /// <summary>
        /// Axis Index
        /// </summary>
        public byte AxisIndex
        {
            get
            {
                return this.EventStruct.jaxis.axis;
            }
        }

        /// <summary>
        /// AxisValue
        /// </summary>
        public float AxisValue
        {
            get
            {
                float jaxisValue =
                    ((float)this.EventStruct.jaxis.val + JOYSTICK_ADJUSTMENT) / JOYSTICK_SCALE;
                if (jaxisValue < 0)
                {
                    jaxisValue = 0;
                }
                return jaxisValue;
            }
        }

        /// <summary>
        /// Returns axis value generated by SDL.
        /// </summary>
        public float RawAxisValue
        {
            get
            {
                return this.EventStruct.jaxis.val;
            }
        }

        /// <summary>
        /// Joystick jitter threshhold
        /// </summary>
        /// <remarks>
        /// The joystick has to return a value that is 
        /// higher than this before firing an event. 
        /// This is used to reduce joystick jitter.
        /// </remarks>
        public static short JoystickThreshold
        {
            get
            {
                return JOYSTICK_THRESHHOLD;
            }
        }
    }
}