#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class SmackerPlayer
    {
        enum State
        {
            STOPPED,
            PLAYING,
            PAUSED
        }

        string filename;
        byte[] buf;
        Surface surface;
        State state;
        Thread decoderThread;
        FFmpeg decoder;
        int width;
        int height;

        static object sync = new object();

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="smkStream"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SmackerPlayer(string fileName,
        Stream smkStream,
        int width, int height)
        {
            this.filename = fileName;
            this.width = width;
            this.height = height;

            this.buf = GuiUtility.ReadStream(smkStream);
        }

        /// <summary>
        ///
        /// </summary>
        public void Play()
        {
            if (state == State.PAUSED)
            {
                state = State.PLAYING;
                // TODO This is obselete
                //decoderThread.Resume();
            }
            else if (state == State.STOPPED)
            {
                state = State.PLAYING;
                decoderThread = new Thread(DecoderThread);
                decoderThread.IsBackground = true;
                decoderThread.Start();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Stop()
        {
            if (state == State.STOPPED)
            {
                return;
            }

            state = State.STOPPED;
            decoderThread.Abort();
            decoder.Stop();
            decoderThread = null;
            decoder = null;
            surface = null;
        }

        /// <summary>
        ///
        /// </summary>
        public void Pause()
        {
            if (state == State.PAUSED || state == State.STOPPED)
            {
                return;
            }

            state = State.PAUSED;
            //TODO this is obselete
            //decoderThread.Suspend();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="destination"></param>
        public void BlitSurface(Surface destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            lock (sync)
            {
                if (surface != null)
                {
                    destination.Blit(surface,
                    new Point((destination.Width - surface.Width) / 2,
                    (destination.Height - surface.Height) / 2));
                }
            }
        }

        Surface ScaleSurface(Surface surf)
        {
            double horizZoom = (double)width / surf.Width;
            double vertZoom = (double)height / surf.Height;
            double zoom;

            if (horizZoom < vertZoom)
            {
                zoom = horizZoom;
            }
            else
            {
                zoom = vertZoom;
            }

            if (zoom != 1.0)
            {
                surf = surf.CreateScaledSurface(zoom);
            }

            return surf;
        }

        void DecoderThread()
        {
            try
            {
                decoder = new FFmpeg(filename, buf);

                decoder.Start();

                Console.WriteLine("animation is {0}x{1}, we're displaying at {2}x{3}",
                decoder.Width, decoder.Height,
                width, height);
                byte[] frameBuf = new byte[decoder.Width * decoder.Height * 3];
                while (decoder.GetNextFrame(frameBuf))
                {
                    lock (sync)
                    {
                        if (surface != null)
                        {
                            surface.Dispose();
                        }
                        surface = ScaleSurface(GuiUtility.CreateSurface(frameBuf,
                        (ushort)decoder.Width,
                        (ushort)decoder.Height,
                        24, decoder.Width * 3,
                        (int)0x000000ff,
                        (int)0x0000ff00,
                        (int)0x00ff0000,
                        (int)0x00000000));
                    }
                    Thread.Sleep(100);
                }

                decoder.Stop();
            }
            finally
            {
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(EmitFinished)));
            }

        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> Finished;

        void EmitFinished(object sender, EventArgs e)
        {
            if (Finished != null)
            {
                Finished(this, new SCEventArgs());
            }
        }
    }

    ///// <summary>
    /////
    ///// </summary>
    //public delegate void PlayerEventHandler(object sender, EventArgs e);
}
