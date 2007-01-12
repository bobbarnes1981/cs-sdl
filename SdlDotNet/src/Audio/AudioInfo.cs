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
        /// Audio frequency in samples per second
        /// </summary>
        /// <remarks>
        /// The number of samples sent to the sound device every second.  
        /// Common values are 11025, 22050 and 44100. The higher the better.
        /// </remarks>
        public int Frequency
        {
            get
            {
                return this.data.freq;
            }
        }

        /// <summary>
        /// Audio data format.
        /// </summary>
        /// <remarks>
        /// Specifies the size and type of each sample element.
        /// </remarks>
        public AudioFormat Format
        {
            get
            {
                return (AudioFormat)this.data.format;
            }
        }

        /// <summary>
        /// Number of channels: 1 mono, 2 stereo.
        /// </summary>
        /// <remarks>
        /// The number of seperate sound channels. 
        /// 1 is mono (single channel), 2 is stereo (dual channel).
        /// </remarks>
        public byte Channels
        {
            get
            {
                return this.data.channels;
            }
        }

        /// <summary>
        /// Audio buffer size in samples.
        /// </summary>
        /// <remarks>
        /// When used with <see cref="AudioBasic.OpenAudio"/> this refers 
        /// to the size of the 
        /// audio buffer in samples. A sample a chunk of audio data
        ///  of the size specified in format mulitplied by the number
        ///   of channels.
        /// </remarks>
        public short Samples
        {
            get
            {
                return this.data.samples;
            }
        }

        /// <summary>
        /// Audio buffer size in bytes (calculated)
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this.data.size;
            }
        }

        /// <summary>
        /// Audio buffer silence value (calculated).
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
