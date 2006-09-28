/*
 * $RCSfile$
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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SdlDotNet
{
    /// <summary>
    /// An active audio stream for queueing audio data to be played asynchronously.
    /// </summary>
    public class AudioStream
    {
        internal AudioStream(int sample_frequency, short samples) 
        {
            _samples = samples;
            _queue = new Queue<short[]>(5);
            _sample_freq = sample_frequency;
        }

        internal void Close() { }

        /// <summary>
        /// Asynchronously queues audio data in <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Buffer formatted as <see cref="AudioFormat.Unsigned16Little"/> of audio data to be played</param>
        public void Write(short[] data)
        {
            Audio.Locked = true;
            short[] copy = (short[])data.Clone();
            _samples_in_queue += copy.Length;
            _queue.Enqueue(copy);
            Audio.Locked = false;
        }
        /// <summary>
        /// Size of the SDL audio sample buffer
        /// </summary>
        public short Samples
        {
            get { return _samples; }
            internal set { _samples = value; }
        }

        /// <summary>
        /// Total remaining samples queued
        /// </summary>
        public int RemainingSamples
        {
            get
            {
                return _samples_in_queue;
            }
        }

        /// <summary>
        /// Total remaining milliseconds before sample queue is emptied
        /// </summary>
        public int RemainingMilliseconds
        {
            get { return (int)((double)RemainingSamples / _sample_freq * 1000); }
        }

        /// <summary>
        /// Remaining number of buffers queued
        /// </summary>
        public int RemainingQueues
        {
            get { return _queue.Count; }
        }

        #region Audio Stream methods

        internal void Unsigned16LittleStream(IntPtr o, IntPtr stream, int len)
        {
            len /= 2;

            if (_queue.Count > 0)
            {
                short[] buf = _queue.Dequeue();
                _samples_in_queue -= buf.Length;
                Marshal.Copy(buf, 0, stream, len);
            }
        }

        #endregion

        #region private fields
        
        short _samples;

        Queue<short[]> _queue;
        int _sample_freq;
        int _samples_in_queue;

        #endregion
    }
}
