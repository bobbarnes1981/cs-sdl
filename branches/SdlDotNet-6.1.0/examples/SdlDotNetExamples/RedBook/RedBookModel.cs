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

namespace SdlDotNetExamples.RedBook
{
	/// <summary>
	///     This program demonstrates modeling transformations.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Original Author:    Silicon Graphics, Inc.
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
	public class RedBookModel
	{
		#region Fields

		//Width of screen
		int width = 500;
		//Height of screen
		int height = 500;
		
		

		/// <summary>
		/// Lesson title
		/// </summary>
		public static string Title
		{
			get
			{
				return "Model - modelling transformations";
			}
		}

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookModel()
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
		#region Reshape(int w, int h)
		private static void Reshape(int w, int h) 
		{
			Gl.glViewport(0, 0, w, h);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			if(w <= h) 
			{
				Gl.glOrtho(-50.0, 50.0, -50.0 * (float) h / (float) w, 50.0 * (float) h / (float) w, -1.0, 1.0);
			}
			else 
			{
				Gl.glOrtho(-50.0 * (float) w / (float) h, 50.0 * (float) w / (float) h, -50.0, 50.0, -1.0, 1.0);
			}
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
		}
		#endregion Reshape(int w, int h)

		/// <summary>
		/// Initializes the OpenGL system
		/// </summary>
		private static void Init()
		{
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			Gl.glShadeModel(Gl.GL_FLAT);
		}

		#endregion Lesson Setup

		#region DrawTriangle()
		private static void DrawTriangle() 
		{
			Gl.glBegin(Gl.GL_LINE_LOOP);
			Gl.glVertex2f(0.0f, 25.0f);
			Gl.glVertex2f(25.0f, -25.0f);
			Gl.glVertex2f(-25.0f, -25.0f);
			Gl.glEnd();
		}
		#endregion DrawTriangle()

		#region void Display
		/*  Draw twelve spheres in 3 rows with 4 columns.  
			*   The spheres in the first row have materials with no ambient reflection.
			*   The second row has materials with significant ambient reflection.
			*   The third row has materials with colored ambient reflection.
			*
			*   The first column has materials with blue, diffuse reflection only.
			*   The second column has blue diffuse reflection, as well as specular
			*   reflection with a low shininess exponent.
			*   The third column has blue diffuse reflection, as well as specular
			*   reflection with a high shininess exponent (a more concentrated highlight).
			*   The fourth column has materials which also include an emissive component.
			*
			*   Gl.glTranslatef() is used to move spheres to their appropriate locations.
			*/
		private static void Display()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			Gl.glColor3f(1.0f, 1.0f, 1.0f);

			Gl.glLoadIdentity();
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
			DrawTriangle();

			Gl.glEnable(Gl.GL_LINE_STIPPLE);
			Gl.glLineStipple(1, unchecked((short)0xF0F0));
			Gl.glLoadIdentity();
			Gl.glTranslatef(-20.0f, 0.0f, 0.0f);
			DrawTriangle();

			Gl.glLineStipple(1, unchecked((short)0xF00F));
			Gl.glLoadIdentity();
			Gl.glScalef(1.5f, 0.5f, 1.0f);
			DrawTriangle();

			Gl.glLineStipple(1, unchecked((short)0x8888));
			Gl.glLoadIdentity();
			Gl.glRotatef(90.0f, 0.0f, 0.0f, 1.0f);
			DrawTriangle();
			Gl.glDisable(Gl.GL_LINE_STIPPLE);

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
			RedBookModel t = new RedBookModel(); t.Reshape();
			Init();
			Events.Run();
		}

		#endregion Run Loop
	}
}