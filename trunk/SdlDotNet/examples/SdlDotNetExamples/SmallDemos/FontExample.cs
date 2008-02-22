#region LICENSE

/*
 * $RCSfile: FontExample.cs,v $
 * Copyright (C) 2004-2006 David Hudson (jendave@yahoo.com)
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
using System.Threading;
using System.IO;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class FontExample : IDisposable
    {
        Surface text;
        Surface screen;
        int size = 12;
        int width = 640;
        int height = 480;
        SdlDotNet.Graphics.Font font;
        string filePath = Path.Combine("..", "..");
        string fileDirectory = "Data";
        string fileName = "FreeSans.ttf";
        Random rand = new Random();

        string[] textArray = { "Hello World!", "This is a test", "FontExample", "SDL.NET" };
        int[] styleArray = { 0, 1, 2, 4 };

        [STAThread]
        public static void Run()
        {
            FontExample t = new FontExample();
            t.Go();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
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

            Events.Tick +=
                new EventHandler<TickEventArgs>(Events_TickEvent);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            font = new SdlDotNet.Graphics.Font(file, size);
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Font Example";
            screen = Video.SetVideoMode(width, height, true);

            Surface surf = screen.CreateCompatibleSurface(width, height, true);
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);
            Events.Run();
        }

        private void Events_TickEvent(object sender, TickEventArgs e)
        {
            try
            {
                font.Style = (Styles)styleArray[rand.Next(styleArray.Length)];
                text = font.Render(
                    textArray[rand.Next(textArray.Length)],
                    Color.FromArgb(0, (byte)rand.Next(255),
                    (byte)rand.Next(255), (byte)rand.Next(255)));

                switch (rand.Next(4))
                {
                    case 1:
                        text = text.CreateFlippedVerticalSurface();
                        break;
                    case 2:
                        text = text.CreateFlippedHorizontalSurface();
                        break;
                    case 3:
                        text = text.CreateRotatedSurface(rand.Next(360));
                        break;
                    default:
                        break;
                }

                screen.Blit(
                    text,
                    new Rectangle(new Point(rand.Next(width - 100), rand.Next(height - 100)),
                    text.Size));
                screen.Update();
                Thread.Sleep(500);
            }
            catch
            {
                //sdl.Dispose();
                throw;
            }
        }

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            // Check if the key pressed was a Q or Escape
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "FontExample: Display font in multiple styles";
            }
        }

        #region IDisposable Members

        private bool disposed;

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
                    if (this.font != null)
                    {
                        this.font.Dispose();
                        this.font = null;
                    }
                }
                this.disposed = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        ~FontExample()
        {
            Dispose(false);
        }

        #endregion
    }
}
