/*
 * $RCSfile: SpriteGuiDemosMain.cs,v $
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Globalization;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SpriteDemos
{
    /// <summary>
    /// The SpriteGuiDemosMain is a general testbed and display of various features
    /// in the MFGames.Sdl library. It includes animated sprites and
    /// movement. To run, it currently assumes that the current
    /// directory has a "test/" directory underneath it containing
    /// various images.
    /// </summary>
    public class SpriteDemosMain : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        public static void Run()
        {
            // Create the demo application
            SpriteDemosMain demo = new SpriteDemosMain();
            demo.Go();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            // Create the screen
            int width = 800;
            int height = 600;

            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Sprite Demos";
            screen = Video.SetVideoMode(width, height);

            // Set up the master sprite container
            SetupGui();

            // Load demos
            LoadDemos();

            // Loop until the system indicates it should stop
            Console.WriteLine("Welcome to the SDL.NET Demo!");

            // Start up the ticker (and animation)
            SwitchDemo(0);
            Events.Fps = 100;
            Events.Run();
        }

        #region GUI

        private int[] fpsSpeeds =
            new int[] { 1, 5, 10, 15, 20, 30, 40, 50, 60, 100 };
        //string data_directory = @"Data/";
        //string filepath = @"../../";
        private static void SetupGui()
        {
            // Set up the demo sprite containers
            master.EnableMouseButtonEvent();
            master.EnableMouseMotionEvent();
            master.EnableTickEvent();

            //if (File.Exists(data_directory + "comic.ttf"))
            //{
            //    filepath = "";
            //}
        }

        #endregion

        #region Demos
        private ArrayList demos = new ArrayList();

        private static DemoMode currentDemo;

        /// <summary>
        /// 
        /// </summary>
        public static DemoMode CurrentDemo
        {
            get
            {
                return currentDemo;
            }
        }

        private void LoadDemo(DemoMode mode)
        {
            // Add to the array list
            demos.Add(mode);
        }

        private void LoadDemos()
        {
            // Add the sprite manager to the master
            master.Add(manager);

            // Load the actual demos
            LoadDemo(new BounceMode());
            LoadDemo(new FontMode());
            LoadDemo(new DragMode());
            LoadDemo(new ViewportMode());
            LoadDemo(new MultipleMode());
        }

        private static void StopDemo()
        {
            // Stop the demo, if any
            if (currentDemo != null)
            {
                currentDemo.Stop(manager);
                currentDemo = null;
            }
        }

        private void SwitchDemo(int demo)
        {
            // Stop the demo, if any
            StopDemo();

            // Ignore if the demo request is too high
            if (demo < 0 || demo + 1 > demos.Count)
            {
                return;
            }

            // Start it
            currentDemo = (DemoMode)demos[demo];
            currentDemo.Start(manager);
            Console.WriteLine("Switched to " + currentDemo + " mode");
        }
        #endregion

        #region Events

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                case Key.Q:
                    Events.QuitApplication();
                    break;
                case Key.C:
                    StopDemo();
                    break;
                case Key.One:
                    SwitchDemo(0);
                    break;
                case Key.Two:
                    SwitchDemo(1);
                    break;
                case Key.Three:
                    SwitchDemo(2);
                    break;
                case Key.Four:
                    SwitchDemo(3);
                    break;
                case Key.Five:
                    SwitchDemo(4);
                    break;
                case Key.M:
                    Video.IconifyWindow();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Tick(object sender, TickEventArgs args)
        {
            screen.Fill(Color.Black);
            if (currentDemo != null)
            {
                screen.Blit(currentDemo.RenderSurface());
            }
            screen.Blit(master);
            screen.Update();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        #endregion

        #region Properties
        private static SpriteDictionary master = new SpriteDictionary();
        private static SpriteDictionary manager = new SpriteDictionary();
        private Surface screen;

        /// <summary>
        /// 
        /// </summary>
        public static Size Size
        {
            get
            {
                return new Size(800, 600);
            }
        }
        #endregion

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "SpriteDemos: Several demos showing sprites.";
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
            Dispose(true);
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
                    this.screen.Dispose();
                    foreach (Sprite s in SpriteDemosMain.manager.Keys)
                    {
                        IDisposable disposableObj = s as IDisposable;
                        if (disposableObj != null)
                        {
                            disposableObj.Dispose();
                        }
                    }
                    foreach (Sprite s in SpriteDemosMain.master.Keys)
                    {
                        IDisposable disposableObj = s as IDisposable;
                        if (disposableObj != null)
                        {
                            disposableObj.Dispose();
                        }
                    }
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
