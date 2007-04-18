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
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// Demo of Bouncing Balls using Sprites. 
    /// The Bouncesprites will respond to Tick Events by spinning. 
    /// You can click on each sprite and move them around the 
    /// screen as well (MouseButton and MouseMotion events).
    /// </summary>
    public class BounceSprites : IDisposable
    {
        #region Fields
        private Surface screen; //video screen
        private SpriteCollection master = new SpriteCollection(); //holds all sprites
        private int width = 640; //screen width
        private int height = 480; //screen height
        private int maxBalls = 10; //number of balls to display
        private Random rand = new Random(); //randomizer
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");
        private Surface background;
        #endregion Fields

        #region EventHandler Methods
        //Handles keyboard events. 
        // The 'Escape' and 'Q'keys will cause the app to exit
        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        Collection<Rectangle> rects = new Collection<Rectangle>();

        //A ticker is running to update the sprites constantly.
        //This method will fill the screen with black to clear it of the sprites.
        //Then it will Blit all of the sprites to the screen.
        //Then it will refresh the screen and display it.
        private void Tick(object sender, TickEventArgs args)
        {
            rects = screen.Blit(master);
            screen.Update(rects);
            screen.Erase(master, background);
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
        #endregion EventHandler Methods

        #region Methods
        //Main program loop
        private void Go()
        {
            //Set up screen
            if (File.Exists(Path.Combine(dataDirectory, "background.png")))
            {
                filePath = "";
            }
            background = new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "background.png")));
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Bounce Sprites";
            screen = Video.SetVideoMode(width, height);
            screen.Blit(background);
            screen.Update();

            //This loads the various images (provided by Moonfire) 
            // into a SurfaceCollection for animation
            SurfaceCollection marbleSurfaces = new SurfaceCollection();
            marbleSurfaces.Add(new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "marble1.png"))), new Size(50, 50));

            for (int i = 0; i < this.maxBalls; i++)
            {
                //Create a new Sprite at a random location on the screen
                master.Add(new BounceSprite(marbleSurfaces,
                    new Point(rand.Next(screen.Rectangle.Left, screen.Rectangle.Right),
                    rand.Next(screen.Rectangle.Top, screen.Rectangle.Bottom))));
            }

            //The collection will respond to mouse button clicks, mouse movement and the ticker.
            master.EnableMouseButtonEvent();
            master.EnableMouseMotionEvent();
            master.EnableTickEvent();

            //These bind the events to the above methods.
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            //Start the event ticker
            Events.Run();
        }

        /// <summary>
        /// Entry point for App.
        /// </summary>
        [STAThread]
        public static void Run()
        {
            BounceSprites bounce = new BounceSprites();
            bounce.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "BounceSprites: Bouncing balls";
            }
        }
        #endregion Methods

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
        ~BounceSprites()
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
                    if (this.background != null)
                    {
                        this.background.Dispose();
                        this.background = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}