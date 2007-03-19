#region LICENSE
/*
 * $RCSfile: DemoMode.cs,v $
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

using System.Collections;
using System.Drawing;
using System;
using System.IO;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.SpriteDemos
{
    /// <summary>
    /// An abstract page to encapsulates the common functionality of all
    /// demo pages.
    /// </summary>
    public abstract class DemoMode : IDisposable
    {
        private static Hashtable marbles = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        private Surface surf = new Surface(Video.Screen.Width, Video.Screen.Height);

        /// <summary>
        /// 
        /// </summary>
        private SpriteDictionary sprites = new SpriteDictionary();
        static Random rand = new Random();
        static string dataDirectory = "Data";
        static string filePath = Path.Combine("..", "..");

        #region Drawables
        /// <summary>
        /// Loads a floor title into memory.
        /// </summary>
        protected static SurfaceCollection LoadFloor()
        {
            if (File.Exists(Path.Combine(dataDirectory, "floor-00.png")))
            {
                filePath = "";
            }
            SurfaceCollection id = new SurfaceCollection();
            id.Add(Path.Combine(filePath, Path.Combine(dataDirectory, "floor")), ".png");
            return id;
        }

        /// <summary>
        /// Loads the marble series into memory and returns the
        /// collection.
        /// </summary>
        protected static SurfaceCollection LoadMarble(string name)
        {
            if (File.Exists(Path.Combine(dataDirectory, Path.Combine(name, ".png"))))
            {
                filePath = "";
            }

            // Load the marble and cache it before returning
            SurfaceCollection icd = new SurfaceCollection();
            icd.Add(Path.Combine(filePath, Path.Combine(dataDirectory, name + ".png")), new Size(50, 50));
            marbles["icd:" + name] = icd;
            return icd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected static SurfaceCollection LoadRandomMarble()
        {
            return LoadMarble("marble" + (rand.Next() % 6 + 1));
        }

        /// <summary>
        /// Loads a marble from a single image and tiles it.
        /// </summary>
        protected static SurfaceCollection LoadTiledMarble(string name)
        {
            if (File.Exists(Path.Combine(dataDirectory, Path.Combine(name, ".png"))))
            {
                filePath = "";
            }
            // Load the marble
            SurfaceCollection td = new SurfaceCollection();
            td.Add(new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, Path.Combine(name, ".png")))), new Size(50, 50));
            return td;
        }
        #endregion

        #region Mode Switching
        /// <summary>
        /// Indicates to the demo page that it should start displaying its
        /// data in the given sprite manager.
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Indicates to the demo page that it should stop displaying its
        /// data in the given sprite manager.
        /// </summary>
        public virtual void Stop()
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Surface Surface
        {
            get
            {
                return surf;
            }
            set
            {
                surf = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Surface RenderSurface()
        {
            surf.Fill(Color.Black);
            foreach (Sprite s in Sprites.Keys)
            {
                surf.Blit(s.Surface, s.Rectangle);
            }
            return surf;
        }

        /// <summary>
        /// 
        /// </summary>
        public SpriteDictionary Sprites
        {
            get
            {
                return sprites;
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
                    if (this.surf != null)
                    {
                        this.surf.Dispose();
                        this.surf = null;
                    }
                    foreach (Sprite s in this.sprites.Keys)
                    {
                        if (s != null)
                        {
                            this.sprites.Remove(s);
                            s.Dispose();
                        }
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
        ~DemoMode()
        {
            Dispose(false);
        }

        #endregion
    }
}
