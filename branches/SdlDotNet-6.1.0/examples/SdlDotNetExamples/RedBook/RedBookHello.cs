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
	///     This is a simple, introductory OpenGL program.
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
	public class RedBookHello
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
				return "Hello - Simple, introductory OpenGL program.";
			}
		}

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookHello()
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
			Gl.glOrtho(0.0, (double) w, 0.0, (double) h, -1.0, 1.0);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);

		}

		/// <summary>
		/// Initializes the OpenGL system
		/// </summary>
		private static void Init()
		{
			// Select clearing color
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

			// Initialize viewing values
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
		}

		#endregion Lesson Setup

		#region void Display
		/// <summary>
		/// Renders the scene
		/// </summary>
		private static void Display()
		{
			// Clear all pixels.
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

			// Draw white polygon (rectangle) with corners at (0.25, 0.25, 0.0) and
			// (0.75, 0.75, 0.0).
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
			Gl.glBegin(Gl.GL_POLYGON);
			Gl.glVertex3f(0.25f, 0.25f, 0.0f);
			Gl.glVertex3f(0.75f, 0.25f, 0.0f);
			Gl.glVertex3f(0.75f, 0.75f, 0.0f);
			Gl.glVertex3f(0.25f, 0.75f, 0.0f);
			Gl.glEnd();

			// Don't wait!  Start processing buffered OpenGL routines.
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
			RedBookHello t = new RedBookHello(); t.Reshape();
			Init();
			Events.Run();
		}

		#endregion Run Loop
	}
}