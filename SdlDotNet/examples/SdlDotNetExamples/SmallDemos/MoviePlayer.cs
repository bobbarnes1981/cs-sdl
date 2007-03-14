#region LICENSE
/*
 * Copyright (C) 2004 - 2006 David Hudson (jendave@yahoo.com)
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
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Audio;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SmallDemos
{
    #region Class Documentation
    /// <summary>
    /// Simple Tao.Sdl Example
    /// </summary>
    /// <remarks>
    /// Just plays a short movie.
    /// To quit, you can close the window, 
    /// press the Escape key or press the 'q' key
    /// <p>Written by David Hudson (jendave@yahoo.com)</p>
    /// </remarks>
    #endregion Class Documentation
    public class MoviePlayer : IDisposable
    {
        Movie movie;
        Surface screen;

        #region Go()
        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            string filePath = Path.Combine("..", "..");
            string fileDirectory = "Data";
            string fileName = "test.mpg";
            if (File.Exists(fileName))
            {
                filePath = "";
                fileDirectory = "";
            }
            else if (File.Exists(Path.Combine(fileDirectory, fileName)))
            {
                filePath = "";
            }

            string file = Path.Combine(Path.Combine(filePath, fileDirectory), fileName);

            int width = 352;
            int height = 240;

            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Movie Player";
            screen = Video.SetVideoMode(width, height);
            Mixer.Close();
            movie = new Movie(file);
            Console.WriteLine("Time: " + movie.Length);
            Console.WriteLine("Width: " + movie.Size.Width);
            Console.WriteLine("Height: " + movie.Size.Height);
            Console.WriteLine("HasAudio: " + movie.HasAudio);
            Console.WriteLine("HasVideo: " + movie.HasVideo);
            movie.Display(screen);
            movie.Play();
            Events.Run();
        }
        #endregion Go()

        #region Run()
        [STAThread]
        public static void Run()
        {
            MoviePlayer player = new MoviePlayer();
            player.Go();
        }
        #endregion Run()

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            // Check if the key pressed was a Q or Escape
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                movie.Stop();
                movie.Close();
                Events.QuitApplication();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            movie.Stop();
            movie.Close();
            Events.QuitApplication();
        }

        private void Tick(object sender, TickEventArgs e)
        {
            if (movie.IsPlaying)
            {
                return;
            }
            else
            {
                movie.Stop();
                movie.Close();
                Events.QuitApplication();
            }
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "MoviePlayer: MPEG-1 movie player demo";
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Closes and destroys this object
        /// </summary>
        /// <remarks>Destroys managed and unmanaged objects</remarks>
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
        ~MoviePlayer()
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
                    if (this.movie != null)
                    {
                        this.movie.Dispose();
                        this.movie = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}
