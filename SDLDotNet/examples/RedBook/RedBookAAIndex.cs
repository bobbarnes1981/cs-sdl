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

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	/// <summary>
	///     This program draws shows how to draw anti-aliased lines in color index mode.  It
	///     draws two diagonal lines to form an X; when 'r' is typed in the window, the
	///     lines are rotated in opposite directions.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Original Author:    Silicon Graphics, Inc.
	///         http://www.opengl.org/developers/code/examples/redbook/aaindex.c
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
	public class RedBookAAIndex
	{
		#region Fields

		//Width of screen
		int width = 200;
		//Height of screen
		int height = 200;
		// Surface to render on
		Surface screen;
		private const int RAMPSIZE = 16;
		private const int RAMP1START = 32;
		private const int RAMP2START = 48;
		private float rotAngle = 0.0f;

		/// <summary>
		/// Lesson title
		/// </summary>
		public static string Title
		{
			get
			{
				return "AAIndex - AA Lines in Color Index Mode";
			}
		}

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookAAIndex()
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
//			// Sets the resize window event
//			Events.VideoResize += new VideoResizeEventHandler (this.Resize);
			// Set the Frames per second.
			Events.Fps = 60;
			// Creates SDL.NET Surface to hold an OpenGL scene
			screen = Video.SetVideoModeWindowOpenGL(width, height, true);
			// Sets Window icon and title
			this.WindowAttributes();
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
			this.Reshape(this.width, this.height);
		}

		/// <summary>
		/// Resizes window
		/// </summary>
		/// <param name="h">height of windoww</param>
		/// <param name="w">width of window</param>
		private void Reshape(int w, int h)
		{
			Gl.glViewport(0, 0, w, h);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			if(w <= h) 
			{
				Glu.gluOrtho2D(-1.0, 1.0, -1.0 * h / (float)w, 1.0 * h / (float)w);
			}
			else 
			{
				Glu.gluOrtho2D(-1.0 * w / (float)h, 1.0 * w / (float)h, -1.0, 1.0);
			}
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
		}

		/// <summary>
		/// Initializes the OpenGL system
		/// </summary>
		private void InitGL()
		{ 
			Gl.glEnable(Gl.GL_LINE_SMOOTH);
			Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
			Gl.glLineWidth(1.5f);

			Gl.glClearIndex((float) RAMP1START);
		}

		#endregion Lesson Setup

		#region void DisplayGL
		/// <summary>
		/// Renders the scene
		/// </summary>
		private void DisplayGL()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

			Gl.glIndexi(RAMP1START);
			Gl.glPushMatrix();
			Gl.glRotatef(-rotAngle, 0.0f, 0.0f, 0.1f);
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(-0.5f, 0.5f);
			Gl.glVertex2f(0.5f, -0.5f);
			Gl.glEnd();
			Gl.glPopMatrix();

			Gl.glIndexi(RAMP2START);
			Gl.glPushMatrix();
			Gl.glRotatef(rotAngle, 0.0f, 0.0f, 0.1f);
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(0.5f, 0.5f);
			Gl.glVertex2f(-0.5f, -0.5f);
			Gl.glEnd();
			Gl.glPopMatrix();

			Gl.glFlush();
		}
		#endregion void DisplayGL

		#region Event Handlers

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.Escape:
					// Will stop the app loop
					Events.QuitApplication();
					break;
				case Key.F1:
					// Toggle fullscreen
					if ((screen.FullScreen)) 
					{
						screen = Video.SetVideoModeWindowOpenGL(width, height, true);
						this.WindowAttributes();
					}
					else 
					{
						screen = Video.SetVideoModeOpenGL(width, height);
					}
					Reshape();
					break;
				case Key.R:
					rotAngle += 20.0f;
					if(rotAngle >= 360.0f) 
					{
						rotAngle = 0.0f;
					}
					break;
			}
		}

		private void Tick(object sender, TickEventArgs e)
		{
			this.DisplayGL();
			Video.GLSwapBuffers();
		}

//		private void Resize (object sender, VideoResizeEventArgs e)
//		{
//			screen = Video.SetVideoModeWindowOpenGL(e.Width, e.Height, true);
//			if (screen.Width != e.Width || screen.Height != e.Height)
//			{
//				//this.InitGL();
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
			InitGL();
			Events.Run();
		}

		#endregion Run Loop
	}
}