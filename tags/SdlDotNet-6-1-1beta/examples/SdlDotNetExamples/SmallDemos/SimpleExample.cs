#region LICENSE
/*
 * Copyright (C) 2006 David Hudson (jendave@yahoo.com)
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
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SmallDemos
{
    public class SimpleExample
    {
        #region Variables

        private const int width = 640;
        private const int height = 480;
        private Random rand = new Random();
        private Surface screen;

        #endregion

        public SimpleExample()
        {
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Simple Example";
            screen = Video.SetVideoMode(width, height);
        }

        private void KeyDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void Tick(object sender, TickEventArgs e)
        {
            screen.Fill(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            screen.Update();
        }

        public void Go()
        {
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Fps = 5;
            Events.Run();
        }

        [STAThread]
        public static void Run()
        {
            SimpleExample t = new SimpleExample();
            t.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "SimpleExample: Displays random colors to the screen";
            }
        }
    }
}
