#region LICENSE
/* This file is part of SnowDemo
 * SnowDemo.cs, (c) 2005 David Hudson
 * based on code by (c) 2003 Sijmen Mulder
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
 */
#endregion LICENSE

using System;
using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class SnowDemo : IDisposable
    {
        static string[] textArray = {
										"when the cold of winter comes",
										"starless night", "will cover day",
										"in the veiling of the sun", 
										"we will walk",
										"in bitter rain"
									};
        SpriteCollection snowflakes = new SpriteCollection();
        SpriteCollection textItems = new SpriteCollection();
        Surface screen;
        Surface background;
        Surface tree;
        Surface treeStretch;
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");
        string fontName = "FreeSans.ttf";

        /// <summary>
        /// 
        /// </summary>
        public SnowDemo()
        {
        }

        void Initialize(int numberOfSnowflakes)
        {
            for (int i = 0; i < numberOfSnowflakes; i++)
            {
                snowflakes.Add(new Snowflake());
            }
            SdlDotNet.Graphics.Font font = new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, fontName)), 24);

            textItems.Add(new TextItem(textArray[0], font, 25, 0));
            for (int i = 1; i < textArray.Length; i++)
            {
                textItems.Add(
                    new TextItem(textArray[i],
                    font, i * 50,
                    i * 2));
            }
            snowflakes.EnableTickEvent();
            textItems.EnableTickEvent();
        }

        [STAThread]
        public static void Run()
        {
            SnowDemo snowDemo = new SnowDemo();
            snowDemo.Go();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            if (File.Exists(Path.Combine(dataDirectory, "snowbackground.png")))
            {
                filePath = "";
            }
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Snow Demo";
            screen = Video.SetVideoMode(640, 480, 16);
            background = new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "snowbackground.png")));
            background.Transparent = true;
            background.TransparentColor = Color.Magenta;

            tree = new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "Tree.bmp")));
            tree.TransparentColor = Color.Magenta;
            tree.Transparent = true;
            treeStretch = new Surface(tree);
            treeStretch = treeStretch.CreateStretchedSurface(new Size(100, 100));
            treeStretch.TransparentColor = Color.Magenta;
            treeStretch.Transparent = true;
            Initialize(250);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Run();
        }

        private void Tick(object sender, TickEventArgs args)
        {
            screen.Fill(Color.FromArgb(64, 175, 239));
            screen.Blit(snowflakes);
            screen.Blit(background, new Point(0, 280));
            screen.Blit(textItems);
            screen.Blit(tree, new Point(100, 300));
            screen.Blit(tree, new Point(130, 295));
            screen.Blit(tree, new Point(155, 302));
            screen.Blit(tree, new Point(230, 302));
            screen.Blit(treeStretch, new Point(180, 290));
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

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "SnowDemo: Alpha Blending";
            }
        }

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
        ~SnowDemo()
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
                    if (this.tree != null)
                    {
                        this.tree.Dispose();
                        this.tree = null;
                    }
                    if (this.treeStretch != null)
                    {
                        this.treeStretch.Dispose();
                        this.treeStretch = null;
                    }
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
