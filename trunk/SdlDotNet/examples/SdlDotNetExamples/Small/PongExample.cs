/*
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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
using System.IO;
using System.Drawing;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Core;
using SdlDotNet.Input;

namespace SdlDotNetExamples.SmallDemos
{
    public class Pong
    {
        Sprite ball;
        int ballSpeedX = 1;
        int ballSpeedY = 1;

        public Pong()
        {
            string filePath = Path.Combine("..", "..");
            string fileDirectory = "Data";
            string ballFileName = "ball.png";
            //string paddle1FileName = "paddle1.png";
            //string paddle2FileName = "paddle2.png";

            if (File.Exists(ballFileName))
            {
                filePath = "";
                fileDirectory = "";
            }
            else if (File.Exists(Path.Combine(fileDirectory, ballFileName)))
            {
                filePath = "";
            }

            //string file = Path.Combine(Path.Combine(filePath, fileDirectory), fileName);

            ball = new Sprite(new Surface(Path.Combine(Path.Combine(filePath, fileDirectory), ballFileName)));
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Pong Example";
            Video.SetVideoMode(300, 200);
            Events.Tick += new TickEventHandler(Events_Tick);

            ball.Surface.TransparentColor = System.Drawing.Color.Magenta;
        }

        private void Events_Tick(object sender, TickEventArgs e)
        {
            // Update location of the ball
            ball.X += ballSpeedX;
            ball.Y += ballSpeedY;

            // Bounce the ball
            if (ball.Right > Video.Screen.Width)
                ballSpeedX *= -1;
            if (ball.Left < 0)
                ballSpeedX *= -1;
            if (ball.Top < 0)
                ballSpeedY *= -1;
            if (ball.Bottom > Video.Screen.Height)
                ballSpeedY *= -1;

            // Clear the screen
            Video.Screen.Fill(Color.Black);

            // Draw the ball
            ball.Render(Video.Screen);

            // Update the screen
            Video.Screen.Update();
        }

        public void Go()
        {
            Events.Run();
        }

        [STAThread]
        public static void Run()
        {
            Pong pong = new Pong();
            pong.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "PongExample: Simple game of Pong";
            }
        }
    }
}
