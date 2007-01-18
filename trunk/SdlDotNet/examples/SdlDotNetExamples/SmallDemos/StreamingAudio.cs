#region LICENSE
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
#endregion LICENSE

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;

using SdlDotNet.Audio;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SmallDemos
{
    public class StreamingAudio : IDisposable
    {
        const int playbackFreq = 44100;
        const int samples = 2048;
        private const int width = 400;
        private const int height = 100;
        private Surface screen;
        private TextSprite textDisplay;
        string dataDirectory = @"Data/";
        string filepath = @"../../";
        AudioStream stream;

        static short[] buffer16;

        const double pi2 = Math.PI * 2;
        const double divider = (double)playbackFreq / pi2;

        static double time;
        static double step = 1.0 / playbackFreq * pi2;
        static double freq = 224.0;         //Hz
        static double freq2 = 224.0;        //Hz
        static double volume = 0.9;
        static int offset;
        static Oscillator osc = new Oscillator(playbackFreq);
        static Oscillator osc2 = new Oscillator(playbackFreq);

        public StreamingAudio()
        {
            // Setup events
            Events.Tick +=
                new EventHandler<TickEventArgs>(Events_TickEvent);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(Events_KeyboardDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
        }

        [STAThread]
        public static void Run()
        {
            StreamingAudio t = new StreamingAudio();
            t.Go();
        }

        private void Go()
        {
            //string me = "Hello World";
            textDisplay = new TextSprite(" ", new SdlDotNet.Graphics.Font(Path.Combine(filepath, Path.Combine(dataDirectory, "FreeSans.ttf")), 20), Color.Red);
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - StreamingAudio";
            screen = Video.SetVideoMode(width, height);

            //AudioFormat fmt = AudioFormat.Signed16Little;
            //AudioBasic.OpenAudio(playbackFreq, fmt, SoundChannel.Mono, samples, new AudioCallback(Unsigned16LittleCallback), me);
            AudioFormat audioFormat = AudioFormat.Unsigned16Little;
            stream = new AudioStream(playbackFreq, audioFormat, SoundChannel.Mono, samples);
            Mixer.OpenAudio(stream);
            //stream = AudioBasic.OpenAudioStream(playbackFreq, audioFormat, SoundChannel.Mono, samples);
            offset = stream.Offset; // 2 << (16 - 2);
            volume = 0.9 * 32768;

            buffer16 = new short[samples];

            osc.Rate = 20;
            osc.Amplitude = 1;

            osc2.Rate = 3;
            osc2.Amplitude = 10;
            //AudioBasic.OpenAudio(stream);
            Mixer.Paused = false;

            textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "StreamingAudioDirections", CultureInfo.CurrentUICulture);
            textDisplay.TextWidth = 350;
            Events.Run();
        }

        //        static byte[] buffer8 = { };
        //static short[] buffer16;

        //const double pi2 = Math.PI * 2;
        //const double divider = (double)playbackFreq / pi2;

        //static double time;
        //static double step = 1.0 / playbackFreq * pi2;
        //static double freq = 224.0;         //Hz
        //static double freq2 = 224.0;        //Hz
        //static double volume = 0.9;
        //static int offset;
        //static Oscillator osc = new Oscillator(playbackFreq);
        //static Oscillator osc2 = new Oscillator(playbackFreq);

        //static void Unsigned8Callback(IntPtr userData, IntPtr stream, int len)
        //{
        //    int buf_pos = 0;

        //    while (buf_pos < len)
        //    {
        //        buffer8[buf_pos++] = (byte)((Math.Sin(time / pi2 * freq) * 128) * volume);

        //        time += step;
        //    }

        //    Marshal.Copy(buffer8, 0, stream, len);
        //    len = 0;
        //}

        //static void Unsigned16LittleCallback(IntPtr userData, IntPtr stream, int len)
        //{
        //    len /= 2;
        //    int bufPos = 0;

        //    while (bufPos < len)
        //    {
        //        double oscPoint = osc.ValueY(time);
        //        double osc2Point = osc2.ValueY(time);
        //        double sound = ((Math.Sin(time * freq + oscPoint) * volume) + offset) + ((Math.Sin(time * freq2 + osc2Point) * volume) + offset);
        //        sound /= 2.0;
        //        buffer16[bufPos++] = (short)sound;
        //        time += step;
        //    }

        //    Marshal.Copy(buffer16, 0, stream, len);
        //    len = 0;
        //}

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "StreamingAudio: Uses the Audio callback feature to stream audio";
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Mixer.CloseAudio();
            Events.QuitApplication();
        }

        private void Events_TickEvent(object sender, TickEventArgs e)
        {
            int bufPos = 0;
            while (bufPos < 2048)
            {
                double oscPoint = osc.ValueY(time);
                double osc2Point = osc2.ValueY(time);
                double sound = ((Math.Sin(time * freq + oscPoint) * volume) + offset) + ((Math.Sin(time * freq2 + osc2Point) * volume) + offset);
                sound /= 2.0;
                buffer16[bufPos++] = (short)sound;
                time += step;
            }
            stream.Write(buffer16);
            screen.Fill(Color.Black);
            screen.Blit(textDisplay);
            screen.Update();
        }

        private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                case Key.Escape:
                    // Quit the example
                    Mixer.CloseAudio();
                    Events.QuitApplication();
                    break;
            }
        }
        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~StreamingAudio()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.textDisplay != null)
                    {
                        this.textDisplay.Dispose();
                        this.textDisplay = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }

    public class Oscillator
    {
        const double pi2 = Math.PI * 2;
        double amplitude;
        double rate;

        public Oscillator(int sampleRate)
        {
            this.rate = (double)sampleRate;
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

        public double ValueY(double sample)
        {
            return Math.Sin(sample * rate) * amplitude;
        }
    }
}
