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

using SdlDotNet.Core;using SdlDotNet.Graphics;using SdlDotNet.Input;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace SdlDotNet.Examples.RedBook
{
	/// <summary>
	///     This program draws a wireframe model, which uses intensity (brightness) to give
	///     clues to distance.  Fog is used to achieve this effect.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Original Author:    Mark J. Kilgard
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
	public class RedBookDepthCue
	{
		#region Fields

		//Width of screen
		int width = 250;
		//Height of screen
		int height = 250;
		
		
		
		

		/// <summary>
		/// Lesson title
		/// </summary>
		public static string Title
		{
			get
			{
				return "DepthCue - Wireframe Icosohedron using fog";
			}
		}

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookDepthCue()
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
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);
			// Sets the ticker to update OpenGL Context
			Events.Tick += new TickEventHandler(this.Tick); 
			Events.Quit += new QuitEventHandler(this.Quit);
			//			// Sets the resize window event
			//			Events.VideoResize += new VideoResizeEventHandler (this.Resize);
			// Set the Frames per second.
			Events.Fps = 60;
			// Sets Window icon and title
			this.WindowAttributes();
			// Creates SDL.NET Surface to hold an OpenGL scene
			Video.SetVideoModeWindowOpenGL(width, height, true);
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
			Glu.gluPerspective(45.0, (float) w / (float) h, 3.0, 5.0);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			Gl.glTranslatef(0.0f, 0.0f, -4.0f);  //move object into view
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
			
			float[] fogColor = {0.0f, 0.0f, 0.0f, 1.0f};

			Gl.glEnable(Gl.GL_FOG);
			Gl.glFogi(Gl.GL_FOG_MODE, Gl.GL_LINEAR);
			Gl.glHint(Gl.GL_FOG_HINT, Gl.GL_NICEST);  // per pixel
			Gl.glFogf(Gl.GL_FOG_START, 3.0f);
			Gl.glFogf(Gl.GL_FOG_END, 5.0f);
			Gl.glFogfv(Gl.GL_FOG_COLOR, fogColor);
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

			Gl.glDepthFunc(Gl.GL_LESS);
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glShadeModel(Gl.GL_FLAT);
		}

		#endregion Lesson Setup

		#region void Display
		/// <summary>
		/// Renders the scene
		/// </summary>
		private static void Display()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
			Glut.glutWireIcosahedron();
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
		//			Video.SetVideoModeWindowOpenGL(e.Width, e.Height, true);
		//			if (screen.Width != e.Width || screen.Height != e.Height)
		//			{
		//				//this.Init();
		//				this.Reshape();
		//			}
		//		}

		#endregion Event Handlers

		#region Run Loop
		/// <summary>
		/// Starts demo
		/// </summary>
		public void Run()
		{
			Reshape();
			Init();
			Events.Run();
		}

		#endregion Run Loop
	}
}