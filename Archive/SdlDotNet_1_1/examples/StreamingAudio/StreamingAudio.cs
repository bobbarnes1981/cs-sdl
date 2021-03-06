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
using System.Runtime.InteropServices;

using SdlDotNet;

namespace SdlDotNet.Examples.StreamingAudio
{
    class StreamingAudio
    {
        const int playback_freq = 44100;
        const int samples = 2048;

        static void Main(string[] args)
        {
            TestOpenAudio();
        }

        private static void TestOpenAudio()
        {
            string me = "Hello World";

            AudioFormat fmt = AudioFormat.Signed16Little;

            Audio.OpenAudio(playback_freq, fmt, SoundChannel.Mono, samples, new AudioCallbackDelegate(Unsigned16LittleCallback), me);

            offset = Audio.AudioInfo.Offset;
            volume = 0.9 * 32768;


            buffer16 = new short[samples];

            Console.WriteLine("Status: {0}", Audio.AudioStatus.ToString());

            osc.Rate = 20;
            osc.Amplitude = 1;

            osc2.Rate = 3;
            osc2.Amplitude = 10;

            Audio.Paused = false;

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();

            Audio.CloseAudio();
        }

        static byte[] buffer8 = { };
        static short[] buffer16;

        const double pi2 = Math.PI * 2;
        const double divider = (double)playback_freq / pi2;

        static double time = 0.0;
        static double step = 1.0 / playback_freq * pi2;
        static double freq = 224.0;         //Hz
        static double freq2 = 224.0;        //Hz
        static double volume = 0.9;
        static int offset = 0;
        static Oscillator osc = new Oscillator(playback_freq);
        static Oscillator osc2 = new Oscillator(playback_freq);

        static void Unsigned8Callback(IntPtr userData, IntPtr stream, int len)
        {
            int buf_pos = 0;

            while (buf_pos < len)
            {
                buffer8[buf_pos++] = (byte)((Math.Sin(time / pi2 * freq) * 128) * volume);

                time += step;
            }

            Marshal.Copy(buffer8, 0, stream, len);
            len = 0;
        }

        static void Unsigned16LittleCallback(IntPtr userData, IntPtr stream, int len)
        {
            len /= 2;
            int buf_pos = 0;

            while (buf_pos < len)
            {
                double oscPoint = osc[time];
                double osc2Point = osc2[time];
                double sound = ((Math.Sin(time * freq + oscPoint) * volume) + offset) + ((Math.Sin(time * freq2 + osc2Point) * volume) + offset);
                sound /= 2.0;
                buffer16[buf_pos++] = (short)sound;
                time += step;
            }

            Marshal.Copy(buffer16, 0, stream, len);
            len = 0;
        }
    }

    public class Oscillator
    {
        const double pi2 = Math.PI * 2;
        double amplitude;
        double rate;

        public Oscillator(int sampleRate)
        {
        }

        public double Rate
        {
            get 
            { 
                return rate; 
            }
            set 
            { 
                rate = value; 
            }
        }

        public double Amplitude
        {
            get 
            { 
                return amplitude; 
            }
            set 
            { 
                amplitude = value; 
            }
        }

        public double this[double sample]
        {
            get
            {
                return Math.Sin(sample * rate) * amplitude;
            }
        }
    }
}
