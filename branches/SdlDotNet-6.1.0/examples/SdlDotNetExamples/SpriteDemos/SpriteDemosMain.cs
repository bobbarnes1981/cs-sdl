#region LICENSE
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
#endregion LICENSE

using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Core;
using SdlDotNetExamples.SpriteDemos;

namespace SdlDotNetExamples.LargeDemos
{
    /// <summary>
    /// The SpriteDemosMain is a general testbed and display of various features
    /// in the SDL.NET library. It includes animated sprites and
    /// movement. To run, it currently assumes that the current
    /// directory has a "Data" directory underneath it containing
    /// various images.
    /// </summary>
    public class SpriteDemosMain : IDisposable
    {
        static int width = 800;
        static int height = 600;

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
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Sprite Demos";
            screen = Video.SetVideoMode(width, height);
            Events.Fps = 100;
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            //// Load demos
            //LoadDemos();
            SwitchDemo(0);

            Events.Run();
        }

        #region Demos
        //private List<DemoMode> demos = new List<DemoMode>();

        private DemoMode currentDemo;

        

        //private void LoadDemo(DemoMode mode)
        //{
        //    // Add to the array list
        //    demos.Add(mode);
        //}

        //private void LoadDemos()
        //{
            // Load the actual demos
            //LoadDemo(new BounceMode());
            //LoadDemo(new FontMode());
            //LoadDemo(new DragMode());
            //LoadDemo(new ViewportMode());
            //LoadDemo(new MultipleMode());
        //}

        private void StopDemo()
        {
            // Stop the demo, if any
            if (currentDemo != null)
            {
                currentDemo.Stop();
                //currentDemo.Dispose();
                currentDemo = null;
            }
            //System.GC.Collect();
        }

        int demo = 1;

        private void SwitchDemo(int demo)
        {
            //// Ignore if the demo request is too high
            //if (demo < 0 || demo + 1 > demos.Count)
            //{
            //    return;
            //}
            if (demo != this.demo)
            {
                this.demo = demo;
                // Stop the demo, if any
                StopDemo();

                switch (demo)
                {
                    case 0:
                        currentDemo = new BounceMode();
                        currentDemo.Start();
                        break;
                    case 1:
                        currentDemo = new FontMode();
                        currentDemo.Start();
                        break;
                    case 2:
                        currentDemo = new DragMode();
                        currentDemo.Start();
                        break;
                    case 3:
                        currentDemo = new ViewportMode();
                        currentDemo.Start();
                        break;
                    case 4:
                        currentDemo = new MultipleMode();
                        currentDemo.Start();
                        break;
                    default:
                        currentDemo = new BounceMode();
                        currentDemo.Start();
                        break;
                }

                //// Start it
                //currentDemo = demos[demo];
                //currentDemo.Start();
            }
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
            screen.Update();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        #endregion

        #region Properties

        private Surface screen;

        /// <summary>
        /// 
        /// </summary>
        public static Size Size
        {
            get
            {
                return new Size(width, height);
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
                    if (currentDemo != null)
                    {
                        currentDemo.Dispose();
                        currentDemo = null;
                    }
                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
