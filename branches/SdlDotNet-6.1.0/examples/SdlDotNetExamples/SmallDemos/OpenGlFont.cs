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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics.CodeAnalysis;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using Tao.OpenGl;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Correct Spelling")]
    public class OpenGlFont : IDisposable
    {
        int width = 640;
        int height = 480;
        string dataDirectory = "Data";
        // Path to Data directory
        string filePath = Path.Combine("..", "..");
        string fontName = "FreeSans.ttf";
        string phrase1 = "Hello world! ";
        string phrase2 = "This is a Truetype font ";
        string phrase3 = "On an OpenGl Surface ";
        SdlDotNet.Graphics.Font font;
        // Angle For The Triangle ( NEW )
        float rtri;
        // Angle For The Quad ( NEW ) 
        float rquad;

        #region Run Loop
        /// <summary>
        /// Starts lesson
        /// </summary>
        public void Go()
        {
            Initialize();
            InitGL();
            Events.Run();
        }

        #endregion Run Loop

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            if (File.Exists(Path.Combine(dataDirectory, "FreeSans.ttf")))
            {
                filePath = "";
            }
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - OpenGlFont Example";
            Video.SetVideoMode(this.width, this.height, true, true);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            font = new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, fontName)), 18);
            Video.GLDoubleBufferEnabled = true;

        }

        [STAThread]
        public static void Run()
        {
            OpenGlFont openGlFont = new OpenGlFont();
            openGlFont.Go();
        }

        /// <summary>
        /// Initializes the OpenGL system
        /// </summary>
        protected void InitGL()
        {
            // Reset The Current Viewport
            Gl.glViewport(0, 0, width, height);
            // Select The Projection Matrix
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // Reset The Projection Matrix
            Gl.glLoadIdentity();
            Gl.glOrtho(-2.0, 2.0, -2.0, 2.0, -20.0, 20.0);
            // Select The Modelview Matrix
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            // Reset The Modelview Matrix
            Gl.glLoadIdentity();
            // Enable Smooth Shading
            Gl.glShadeModel(Gl.GL_SMOOTH);
            // Enables Depth Testing
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            // The Type Of Depth Testing To Do
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            // Really Nice Perspective Calculations
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            surfaceGl1 = new SurfaceGl(font.Render(phrase1, Color.White, Color.Black));
            surfaceGl2 = new SurfaceGl(font.Render(phrase2, Color.White, Color.Black));
            surfaceGl3 = new SurfaceGl(font.Render(phrase3, Color.White, Color.Black));
        }
        SurfaceGl surfaceGl1;
        SurfaceGl surfaceGl2;
        SurfaceGl surfaceGl3;
        int i;

        #region void DrawGLScene

        /// <summary>
        /// Renders the scene
        /// </summary>
        protected void DrawGLScene()
        {
            // Rotate The Quad On The X, Y, and Z Axis ( NEW )
            Gl.glRotatef(rquad, 1, 1, 1);
            // Set The Color To Blue One Time Only
            Gl.glColor3f(0.5f, 0.5f, 1);
            // Draw A Quad
            Gl.glBegin(Gl.GL_QUADS);
            // Set The Color To Green
            Gl.glColor3f(0, 1, 0);
            // Top Right Of The Quad (Top)
            Gl.glVertex3f(1, 1, -1);
            // Top Left Of The Quad (Top)
            Gl.glVertex3f(-1, 1, -1);
            // Bottom Left Of The Quad (Top)
            Gl.glVertex3f(-1, 1, 1);
            // Bottom Right Of The Quad (Top)
            Gl.glVertex3f(1, 1, 1);
            // Set The Color To Orange
            Gl.glColor3f(1, 0.5f, 0);
            // Top Right Of The Quad (Bottom)
            Gl.glVertex3f(1, -1, 1);
            // Top Left Of The Quad (Bottom)
            Gl.glVertex3f(-1, -1, 1);
            // Bottom Left Of The Quad (Bottom)
            Gl.glVertex3f(-1, -1, -1);
            // Bottom Right Of The Quad (Bottom)
            Gl.glVertex3f(1, -1, -1);
            // Set The Color To Red
            Gl.glColor3f(1, 0, 0);
            // Top Right Of The Quad (Front)
            Gl.glVertex3f(1, 1, 1);
            // Top Left Of The Quad (Front)
            Gl.glVertex3f(-1, 1, 1);
            // Bottom Left Of The Quad (Front)
            Gl.glVertex3f(-1, -1, 1);
            // Bottom Right Of The Quad (Front)
            Gl.glVertex3f(1, -1, 1);
            // Set The Color To Yellow
            Gl.glColor3f(1, 1, 0);
            // Top Right Of The Quad (Back)
            Gl.glVertex3f(1, -1, -1);
            // Top Left Of The Quad (Back)
            Gl.glVertex3f(-1, -1, -1);
            // Bottom Left Of The Quad (Back)
            Gl.glVertex3f(-1, 1, -1);
            // Bottom Right Of The Quad (Back)
            Gl.glVertex3f(1, 1, -1);
            // Set The Color To Blue
            Gl.glColor3f(0, 0, 1);
            // Top Right Of The Quad (Left)
            Gl.glVertex3f(-1, 1, 1);
            // Top Left Of The Quad (Left)
            Gl.glVertex3f(-1, 1, -1);
            // Bottom Left Of The Quad (Left)
            Gl.glVertex3f(-1, -1, -1);
            // Bottom Right Of The Quad (Left)
            Gl.glVertex3f(-1, -1, 1);

            // Set The Color To Violet
            Gl.glColor3f(1, 0, 1);
            // Top Right Of The Quad (Right)
            Gl.glVertex3f(1, 1, -1);
            // Top Left Of The Quad (Right)
            Gl.glVertex3f(1, 1, 1);
            // Bottom Left Of The Quad (Right)
            Gl.glVertex3f(1, -1, 1);
            // Bottom Right Of The Quad (Right)
            Gl.glVertex3f(1, -1, -1);
            Gl.glEnd();
            // Done Drawing The Quad

            // Increase The Rotation Variable For The Triangle ( NEW )
            rtri += 0.2f;
            // Decrease The Rotation Variable For The Quad ( NEW )
            rquad -= 0.15f;
        }

        #endregion void DrawGLScene

        private void Tick(object sender, TickEventArgs e)
        {
            Gl.glClearColor(0.0F, 0.0F, 0.0F, 1.0F);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            DrawGLScene();
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glColor3f(0, 0, 0);
            Gl.glEnd();
            SurfaceGl.Mode2D = true;
            surfaceGl1.Surface = font.Render(phrase1 + i++, Color.White, Color.Black);
            //surfaceGl1.Load(font.Render(phrase1 + i++, Color.White, Color.Black));
            surfaceGl1.Draw(new Point(0, 0));
            surfaceGl2.Surface = font.Render(phrase2 + i++, Color.White, Color.Black);
            //surfaceGl2.Load(font.Render(phrase2 + i++, Color.White, Color.Black));
            surfaceGl2.Draw(new Point(100, 100));
            surfaceGl3.Surface = font.Render(phrase3 + i++, Color.White, Color.Black);
            //surfaceGl3.Load(font.Render(phrase3 + i++, Color.White, Color.Black));
            surfaceGl3.Draw(new Point(200, 200));
            SurfaceGl.Mode2D = false;

            Video.GLSwapBuffers();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            // Check if the key pressed was a Q or Escape
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "OpenGLFont: 2D Surfaces on an OpenGL surface";
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
        ~OpenGlFont()
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
                    if (this.font != null)
                    {
                        this.font.Dispose();
                        this.font = null;
                    }
                    if (this.surfaceGl1 != null)
                    {
                        this.surfaceGl1.Dispose();
                        this.surfaceGl1 = null;
                    }
                    if (this.surfaceGl2 != null)
                    {
                        this.surfaceGl2.Dispose();
                        this.surfaceGl2 = null;
                    }
                    if (this.surfaceGl3 != null)
                    {
                        this.surfaceGl3.Dispose();
                        this.surfaceGl3 = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}
