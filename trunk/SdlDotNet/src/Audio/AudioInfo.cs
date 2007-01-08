#region LICENSE
/*
 * Copyright (C) 2006 Stuart Carnie (stuart.carnie@gmail.com)
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
#endregion LICENSE

using System;
using Tao.Sdl;

namespace SdlDotNet.Audio
{
    /// <summary>
    /// Provides information about the currently open audio subsystem.
    /// </summary>
    public class AudioInfo
    {
        #region Private fields

        Sdl.SDL_AudioSpec data;
        byte bits;
        int offset;

        #endregion Private fields

        #region Constructor and Destructors

        internal AudioInfo(Sdl.SDL_AudioSpec data, byte bits, int offset)
        {
            this.data = data;
            this.bits = bits;
            this.offset = offset;
        }

        #endregion Constructor and Destructors

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public int Frequency
        {
            get
            {
                return this.data.freq;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AudioFormat Format
        {
            get
            {
                return (AudioFormat)this.data.format;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Channels
        {
            get
            {
                return this.data.channels;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public short Samples
        {
            get
            {
                return this.data.samples;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.data.size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Silence
        {
            get
            {
                return this.data.silence;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Offset
        {
            get
            {
                return this.offset;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Bits
        {
            get
            {
                return this.bits;
            }
        }

        #endregion Public Methods
    }
}
