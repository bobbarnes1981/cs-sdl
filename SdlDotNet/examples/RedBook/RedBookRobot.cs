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

namespace SdlDotNetExamples.RedBook
{
	/// <summary>
	///     This program shows how to composite modeling transformations to draw translated
	///     and rotated hierarchical models.  Interaction:  pressing the s and e keys
	///     (shoulder and elbow) alters the rotation of the robot arm.
	/// </summary>
	/// <remarks>
	///     <para>
	///         Original Author:    Silicon Graphics, Inc.
	///         http://www.opengl.org/developers/code/examples/redbook/planet.c
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
	public class RedBookRobot
	{
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
				return "Robot - Rotated models";
			}
		}

		#region Private Fields
		private static int shoulder = 0;
		private static int elbow = 0;
		#endregion Private Fields

		#region Constructors

		/// <summary>
		/// Basic constructor
		/// </summary>
		public RedBookRobot()
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

		#endregion Lesson Setup
		/// <summary>
		/// Resizes window
		/// </summary>
		private void Reshape()
		{
			Reshape(this.width, this.height);
		}

		// --- Application Methods ---
		#region Init()
		private static void Init() 
		{
			
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			Gl.glShadeModel(Gl.GL_FLAT);
		}
		#endregion Init()

		// --- Callbacks ---
		#region Display()
		private static void Display() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			Gl.glPushMatrix();
			Gl.glTranslatef(-1.0f, 0.0f, 0.0f);
			Gl.glRotatef((float) shoulder, 0.0f, 0.0f, 1.0f);
			Gl.glTranslatef(1.0f, 0.0f, 0.0f);
			Gl.glPushMatrix();
			Gl.glScalef(2.0f, 0.4f, 1.0f);
			Glut.glutWireCube(1.0);
			Gl.glPopMatrix();

			Gl.glTranslatef(1.0f, 0.0f, 0.0f);
			Gl.glRotatef((float) elbow, 0.0f, 0.0f, 1.0f);
			Gl.glTranslatef(1.0f, 0.0f, 0.0f);
			Gl.glPushMatrix();
			Gl.glScalef(2.0f, 0.4f, 1.0f);
			Glut.glutWireCube(1.0);
			Gl.glPopMatrix();
			Gl.glPopMatrix();
		}
		#endregion Display()

		#region Reshape(int w, int h)
		private static void Reshape(int w, int h) 
		{
			Gl.glViewport(0, 0, w, h);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Glu.gluPerspective(65.0, (float) w / (float) h, 1.0, 20.0);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			Gl.glTranslatef(0.0f, 0.0f, -5.0f);
		}
		#endregion Reshape(int w, int h)
		#region Event Handlers

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.Escape:
					// Will stop the app loop
					Events.QuitApplication();
					break;
				case Key.A:
					shoulder = (shoulder + 5) % 360;
					break;
				case Key.S:
					shoulder = (shoulder - 5) % 360;
					break;
				case Key.W:
					elbow = (elbow + 5) % 360;
					break;
				case Key.E:
					elbow = (elbow - 5) % 360;
					break;
				default:
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