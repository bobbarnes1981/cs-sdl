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

namespace SdlDotNet.Audio
{
    /// <summary>
    /// An active audio stream for queueing audio data to be played asynchronously.
    /// </summary>
    public class AudioStream
    {
        internal AudioStream(int sampleFrequency, short samples) 
        {
            this.samples = samples;
            this.queue = new Queue<short[]>(5);
            this.sampleFrequency = sampleFrequency;
        }

        internal void Close() 
        { 
        }

        /// <summary>
        /// Asynchronously queues audio data in <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Buffer formatted as <see cref="AudioFormat.Unsigned16Little"/> of audio data to be played</param>
        public void Write(short[] data)
        {
            AudioBasic.Locked = true;
            short[] copy = (short[])data.Clone();
            samplesInQueue += copy.Length;
            queue.Enqueue(copy);
            AudioBasic.Locked = false;
        }
        /// <summary>
        /// Size of the SDL audio sample buffer
        /// </summary>
        public short Samples
        {
            get 
            { 
                return samples; 
            }
            internal set 
            { 
                samples = value; 
            }
        }

        /// <summary>
        /// Total remaining samples queued
        /// </summary>
        public int RemainingSamples
        {
            get
            {
                return samplesInQueue;
            }
        }

        /// <summary>
        /// Total remaining milliseconds before sample queue is emptied
        /// </summary>
        public int RemainingMilliseconds
        {
            get 
            { 
                return (int)((double)RemainingSamples / sampleFrequency * 1000); 
            }
        }

        /// <summary>
        /// Remaining number of buffers queued
        /// </summary>
        public int RemainingQueues
        {
            get 
            { 
                return queue.Count; 
            }
        }

        #region Audio Stream methods

        internal void Unsigned16LittleStream(IntPtr userData, IntPtr stream, int len)
        {
            len /= 2;

            if (queue.Count > 0)
            {
                short[] buf = queue.Dequeue();
                samplesInQueue -= buf.Length;
                Marshal.Copy(buf, 0, stream, len);
            }
        }

        #endregion

        #region private fields
        
        short samples;

        Queue<short[]> queue;
        int sampleFrequency;
        int samplesInQueue;

        #endregion
    }
}
