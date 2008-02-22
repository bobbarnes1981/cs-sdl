#region LICENSE
/*
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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
using System.Runtime.InteropServices;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Audio;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SmallDemos
{
    public class JoystickExample : IDisposable
    {
        Point position = new Point(100, 100);
        int width = 640;
        int height = 480;
        Joystick joystick;
        Surface screen;
        Surface cursor;

        /// <summary>
        /// 
        /// </summary>
        public JoystickExample()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            string filePath = Path.Combine("..", "..");
            string fileDirectory = "Data";
            string fileName = "cursor.png";
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

            Events.Tick += new EventHandler<TickEventArgs>(Tick);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.JoystickAxisMotion +=
                             new EventHandler<JoystickAxisEventArgs>(this.JoystickAxisChanged);
            Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(this.JoystickButtonDown);
            joystick = Joysticks.OpenJoystick(0);

            try
            {
                Video.WindowIcon();
                Video.WindowCaption = "SdlDotNet - Joystick Example";
                screen = Video.SetVideoMode(width, height, true);
                Mouse.ShowCursor = false; // hide the cursor

                Surface surf =
                    screen.CreateCompatibleSurface(width, height, true);
                surf.Fill(new Rectangle(new Point(0, 0),
                    surf.Size), System.Drawing.Color.Black);

                cursor = new Surface(file);

                Events.Run();
            }
            catch
            {
                throw;
            }
        }

        private void Tick(object sender, TickEventArgs e)
        {
            screen.Fill(Color.Black);
            screen.Blit(cursor, new Rectangle(position, screen.Size));
            screen.Update();
        }

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape ||
                e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void JoystickAxisChanged(object sender, JoystickAxisEventArgs e)
        {
            if (e.AxisIndex == 0)
            {
                position.X = (int)(joystick.GetAxisPosition(JoystickAxis.Horizontal) * width);
            }
            else if (e.AxisIndex == 1)
            {
                position.Y = (int)(joystick.GetAxisPosition(JoystickAxis.Vertical) * height);
            }
        }

        private void JoystickButtonDown(object sender, JoystickButtonEventArgs e)
        {
            Console.WriteLine("Joystick button was pressed");
        }

        /// <summary>
        /// Application EntryPoint.
        /// </summary>
        [STAThread]
        public static void Run()
        {
            JoystickExample joystickExample = new JoystickExample();
            joystickExample.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "JoystickExample: Move the cursor with a joystick";
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
                    if (this.screen != null)
                    {
                        this.screen.Dispose();
                        this.screen = null;
                    }
                    if (this.cursor != null)
                    {
                        this.cursor.Dispose();
                        this.cursor = null;
                    }
                    if (this.joystick != null)
                    {
                        this.joystick.Dispose();
                        this.joystick = null;
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
        ~JoystickExample()
        {
            Dispose(false);
        }

        #endregion
    }
}
