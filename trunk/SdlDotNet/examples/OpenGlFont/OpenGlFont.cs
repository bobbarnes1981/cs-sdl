/*
 * $RCSfile$
 * Copyright (C) 2006 David Hudson (jendave@yahoo.com)
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

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SdlDotNet;
using SdlDotNet.OpenGl;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	public class OpenGlFontExample
	{
		int width = 640;
		int height = 480;
		string dataDirectory = @"Data/";
		// Path to Data directory
		string filePath = @"../../";
		string fontName = "FreeSans.ttf";
		string phrase = "Hello world! ";
		Surface screen;
		FontGl font;

		#region Run Loop
		/// <summary>
		/// Starts lesson
		/// </summary>
		public void Run()
		{
			Initialize();
			InitGL();
			Events.Run();
		}

		#endregion Run Loop

		public void Initialize()
		{
			if (File.Exists(dataDirectory + "FreeSans.ttf"))
			{
				filePath = "";
			}
			Video.WindowIcon();
			Video.WindowCaption = "SDL.NET - OpenGlFont Example";
			screen = Video.SetVideoModeWindowOpenGL(this.width, this.height);
			Events.Quit += new QuitEventHandler(this.Quit);
			Events.Tick += new TickEventHandler(this.Tick);
			font = new FontGl(filePath + dataDirectory + fontName, 20);
		}

		[STAThread]
		static void Main()
		{
			OpenGlFontExample openGlFontExample = new OpenGlFontExample();
			openGlFontExample.Run();
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
			// Calculate The Aspect Ratio Of The Window
			Glu.gluPerspective(45.0F, (width / (float)height), 0.1F, 100.0F);
			// Select The Modelview Matrix
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			// Reset The Modelview Matrix
			Gl.glLoadIdentity();
			// Enable Texture Mapping ( NEW )
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			// Enable Smooth Shading
			Gl.glShadeModel(Gl.GL_SMOOTH);
			// Black Background
			Gl.glClearColor(0.0F, 0.0F, 0.0F, 0.5F);
			// Depth Buffer Setup
			Gl.glClearDepth(1.0F);
			// The Type Of Depth Testing To Do
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			// Really Nice Perspective Calculations
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
		}

		int i;
		
		private void Tick(object sender, TickEventArgs e)
		{
			font.Render(phrase + i++, Color.White, new Point(0,0) );
			Video.GLSwapBuffers();
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			Events.QuitApplication();
		}
	}
}
