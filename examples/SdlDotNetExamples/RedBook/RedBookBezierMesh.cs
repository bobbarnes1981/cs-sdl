#region License
/*
MIT License
Copyright �2003-2005 Tao Framework Team
http://www.taoframework.com
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License

using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using Tao.OpenGl;

namespace SdlDotNetExamples.RedBook
{
    /// <summary>
    ///     This program uses evaluators to draw a Bezier curve.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Original Author:    Silicon Graphics, Inc.
    ///         http://www.opengl.org/developers/code/examples/redbook/bezcurve.c
    ///     </para>
    ///     <para>
    ///         C# Implementation:  Randy Ridge
    ///         http://www.taoframework.com
    ///     </para>
    ///     <para>
    ///			SDL.NET implementation: David Hudson
    ///			http://cs-sdl.sourceforge.net
    ///     </para>
    /// </remarks>
    public class RedBookBezierMesh
    {
        #region Fields

        //Width of screen
        int width = 500;
        //Height of screen
        int height = 500;

        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", Justification = "Jagged Arrays are not CLS-compliant")]
        private static float[/* 4*4*3 */] controlPoints = {
            -1.5f, -1.5f,  4.0f,
            -0.5f, -1.5f,  2.0f,
             0.5f, -1.5f, -1.0f,
             1.5f, -1.5f,  2.0f,
        
            -1.5f, -0.5f,  1.0f,
            -0.5f, -0.5f,  3.0f,
             0.5f, -0.5f,  0.0f,
             1.5f, -0.5f, -1.0f,
        
            -1.5f, 0.5f, 4.0f,
            -0.5f, 0.5f, 0.0f,
             0.5f, 0.5f, 3.0f,
             1.5f, 0.5f, 4.0f,
        
            -1.5f, 1.5f, -2.0f,
            -0.5f, 1.5f, -2.0f,
             0.5f, 1.5f,  0.0f,
             1.5f, 1.5f, -1.0f
         };

        /// <summary>
        /// Lesson title
        /// </summary>
        public static string Title
        {
            get
            {
                return "BezierMesh - Bezier mesh";
            }
        }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Basic constructor
        /// </summary>
        public RedBookBezierMesh()
        {
            Initialize();
        }

        #endregion Constructors

        #region Lesson Setup
        /// <summary>
        /// Initializes methods common to all RedBook lessons
        /// </summary>
        private void Initialize()
        {
            // Sets keyboard events
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyDown);
            Keyboard.EnableKeyRepeat(150, 50);
            // Sets the ticker to update OpenGL Context
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            //			// Sets the resize window event
            //			Events.VideoResize += new EventHandler<VideoResizeEventArgs> (this.Resize);
            // Set the Frames per second.
            Events.Fps = 60;
            // Sets Window icon and title
            this.WindowAttributes();
            // Creates SDL.NET Surface to hold an OpenGL scene
            Video.SetVideoMode(width, height, true, true);
        }

        /// <summary>
        /// Sets Window icon and caption
        /// </summary>
        private void WindowAttributes()
        {
            Video.WindowIcon();
            Video.WindowCaption =
                "SDL.NET - RedBook " +
                this.GetType().ToString().Substring(26);
        }

        /// <summary>
        /// Resizes window
        /// </summary>
        private void Reshape()
        {
            Reshape(this.width, this.height);
        }

        /// <summary>
        /// Resizes window
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        private static void Reshape(int w, int h)
        {
            Gl.glViewport(0, 0, w, h);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            if (w <= h)
            {
                Gl.glOrtho(-4.0, 4.0, -4.0 * h / (float)w, 4.0 * h / (float)w, -4.0, 4.0);
            }
            else
            {
                Gl.glOrtho(-4.0 * w / (float)h, 4.0 * w / (float)h, -4.0, 4.0, -4.0, 4.0);
            }
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        /// <summary>
        ///     <para>
        ///         Initialize antialiasing for RGBA mode, including alpha blending, hint, and
        ///         line width.  Print out implementation specific info on line width granularity
        ///         and width.
        ///     </para>
        /// </summary>
        private static void Init()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glMap2f(Gl.GL_MAP2_VERTEX_3, 0.0f, 1.0f, 3, 4, 0.0f, 1.0f, 12, 4, controlPoints);
            Gl.glEnable(Gl.GL_MAP2_VERTEX_3);
            Gl.glEnable(Gl.GL_AUTO_NORMAL);
            Gl.glMapGrid2f(20, 0.0f, 1.0f, 20, 0.0f, 1.0f);
            // For lighted version only
            InitLights();
        }

        #region InitLights()
        private static void InitLights()
        {
            float[] ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] position = { 0.0f, 0.0f, 2.0f, 1.0f };
            float[] materialDiffuse = { 0.6f, 0.6f, 0.6f, 1.0f };
            float[] materialSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] materialShininess = { 50.0f };

            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambient);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, position);

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, materialDiffuse);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, materialSpecular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, materialShininess);
        }
        #endregion InitLights()

        #endregion Lesson Setup

        #region void Display
        /// <summary>
        /// Renders the scene
        /// </summary>
        private static void Display()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glPushMatrix();
            Gl.glRotatef(85.0f, 1.0f, 1.0f, 1.0f);
            Gl.glEvalMesh2(Gl.GL_FILL, 0, 20, 0, 20);
            Gl.glPopMatrix();
            Gl.glFlush();
        }
        #endregion void Display

        #region Event Handlers

        private void KeyDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    // Will stop the app loop
                    Events.QuitApplication();
                    break;
            }
        }

        private void Tick(object sender, TickEventArgs e)
        {
            Display();
            Video.GLSwapBuffers();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        //		private void Resize (object sender, VideoResizeEventArgs e)
        //		{
        //			Video.SetVideoMode(e.Width, e.Height, true);
        //			if (screen.Width != e.Width || screen.Height != e.Height)
        //			{
        //				//this.Init();
        //				this.RedBook t = new RedBook(); t.Reshape();
        //			}
        //		}

        #endregion Event Handlers

        #region Run Loop
        /// <summary>
        /// Starts demo
        /// </summary>
        public static void Run()
        {
            RedBookBezierMesh t = new RedBookBezierMesh(); t.Reshape();
            Init();
            Events.Run();
        }

        #endregion Run Loop
    }
}