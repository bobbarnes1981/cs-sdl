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

using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace SdlDotNet.Examples
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

            Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
            Events.Quit += new QuitEventHandler(this.Quit);
            Events.Tick += new TickEventHandler(this.FrameTick);
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

        public void FrameTick(object sender, TickEventArgs e)
        {
            screen.Fill(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
            screen.Flip();
        }

        public void Go()
        {
            Events.Run();
        }

        [STAThread]
        public static void Run()
        {
            SimpleExample t = new SimpleExample();
            t.Go();
        }
    }
}
