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
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;

namespace SdlDotNetExamples.SmallDemos
{
    public class SpriteCollectionSort : IDisposable
    {
        Surface screen;
        SpriteCollection manager;
        Sprite s;
        Sprite s2;
        Random rand = new Random();

        public SpriteCollectionSort()
        {
            manager = new SpriteCollection();
        }

        public void Go()
        {
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Sprite Sorting Example";
            screen = Video.SetVideoMode(200, 200, false, false, false);

            s = new Sprite(new Surface(100, 100), new Point(10, 10));
            s2 = new Sprite(new Surface(100, 100), new Point(10, 10));

            s.Surface.Fill(Color.Red);
            s2.Surface.Fill(Color.Blue);

            manager.Add(s);
            manager.Add(s2);

            s2.X = 50;
            s2.Y = 50;

            //Red on top of Blue
            s.Z = 1;
            s2.Z = 0;

            manager.EnableTickEvent();
            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(Events_Tick);
            SdlDotNet.Core.Events.Quit += new EventHandler<QuitEventArgs>(Events_Quit);
            Events.Fps = 5;
            SdlDotNet.Core.Events.Run();
        }

        void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        void Events_Tick(object sender, TickEventArgs e)
        {
            s.Z = rand.Next(100);
            s2.Z = rand.Next(100);
            screen.Fill(Color.Black);
            screen.Update(screen.Blit(manager));
        }
        [STAThread]
        public static void Run()
        {
            SpriteCollectionSort p = new SpriteCollectionSort();
            p.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "SpriteCollectionSort: Sorting Sprite on the Z Axis";
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
                    if (this.s != null)
                    {
                        this.s.Dispose();
                        this.s = null;
                    }
                    if (this.s2 != null)
                    {
                        this.s2.Dispose();
                        this.s2 = null;
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
        ~SpriteCollectionSort()
        {
            Dispose(false);
        }

        #endregion
    }
}

