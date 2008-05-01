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

using SdlDotNet.Core;using SdlDotNet.Graphics;using SdlDotNet.Input;
using Tao.OpenGl;

namespace SdlDotNetExamples.RedBook
{
	/// <summary>
	///     This program draws shows how to draw anti-aliased lines.  It draws two diagonal
	///     lines to form an X; when 'r' is typed in the window, the lines are rotated in
	///     opposite directions.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Original Author:    Silicon Graphics, Inc.
	///         http://www.opengl.org/developers/code/examples/redbook/aargb.c
	///     </para>
	///     <para>
	///         C# Implementation:  Randy Ridge
	///         http://www.taoframework.com
	///     </para>	
	///		<para>
	///			SDL.NET implementation: David Hudson
	///			http://cs-sdl.sourceforge.net
	///     </para>
	/// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Correct Spelling")]
	public class RedBookAARgb
	{
		#region Fields

		//Width of screen
		int width = 200;
		//Height of screen
		int height = 200;
		
		
		
		
		private static float rotAngle;

		/// <summary>
		/// Lesson title
		/// </summary>
		public static string Title
		{
			get
			{
				return "AARgb - AA Lines for RGBA Mode";
			}
		}

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookAARgb()
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
			Keyboard.EnableKeyRepeat(150,50);
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
		private static void Init()
		{ 
			float[] values = new float[2];

			Gl.glGetFloatv(Gl.GL_LINE_WIDTH_GRANULARITY, values);
			Console.WriteLine("GL_LINE_WIDTH_GRANULARITY value is {0:F1}", values[0]);

			Gl.glGetFloatv(Gl.GL_LINE_WIDTH_RANGE, values);
			Console.WriteLine("GL_LINE_WIDTH_RANGE values are {0:F1} {1:F1}", values[0], values[1]);

			Gl.glEnable(Gl.GL_LINE_SMOOTH);
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
			Gl.glLineWidth(1.5f);

			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		}

		#endregion Lesson Setup

		#region void Display
		/// <summary>
		/// Renders the scene
		/// </summary>
		private static void Display()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

			Gl.glColor3f(0.0f, 1.0f, 0.0f);
			Gl.glPushMatrix();
			Gl.glRotatef(-rotAngle, 0.0f, 0.0f, 0.1f);
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(-0.5f, 0.5f);
			Gl.glVertex2f(0.5f, -0.5f);
			Gl.glEnd();
			Gl.glPopMatrix();

			Gl.glColor3f(0.0f, 0.0f, 1.0f);
			Gl.glPushMatrix();
			Gl.glRotatef(rotAngle, 0.0f, 0.0f, 0.1f);
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(0.5f, 0.5f);
			Gl.glVertex2f(-0.5f, -0.5f);
			Gl.glEnd();
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
			RedBookAARgb t = new RedBookAARgb(); 
            t.Reshape();
			Init();
			Events.Run();
		}

		#endregion Run Loop
	}
}