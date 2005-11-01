#region License
/*
MIT License
Copyright ©2003-2005 Tao Framework Team
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	public class NeHe034 : NeHe025
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 34: Beautiful Landscapes By Means Of Height Mapping";
			}
		}
		private const int MAP_SIZE = 1024;
		// Size Of Our .RAW Height Map (NEW)
		private const int STEP_SIZE = 16;
		// Width And Height Of Each Quad (NEW)
		private const float HEIGHT_RATIO = 1.5f;
		// Ratio That The Y Is Scaled According To The X And Z (NEW)
		private bool bRender = true;
		// Polygon Flag Set To TRUE By Default (NEW)
		private byte[] heightMap = new byte[MAP_SIZE * MAP_SIZE];
		// Holds The Height Map Data (NEW)
		private float scaleValue = 0.15f;

		/// <summary>
		/// 
		/// </summary>
		public NeHe034()
		{
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Events.MouseButtonDown += new MouseButtonEventHandler(this.MouseButtonDown);
			Keyboard.EnableKeyRepeat(150,50);
		}

		#region bool DrawGLScene()
		/// <summary>
		///     Draws everything.
		/// </summary>
		/// <returns>
		///     Returns <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		public override void DrawGLScene() 
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			// Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();
			// Reset The Matrix

			Glu.gluLookAt(212, 60, 194, 186, 55, 171, 0, 1, 0);
			// This Determines Where The Camera's Position And View Is
			Gl.glScalef(scaleValue, scaleValue * HEIGHT_RATIO, scaleValue);
			RenderHeightMap(heightMap);
		}
		#endregion void DrawGLScene()

		#region int GetHeight(byte[] heightMap, int x, int y)
		/// <summary>
		///     Returns the height from a height map index.
		/// </summary>
		/// <param name="heightMap">
		///     Height map data.
		/// </param>
		/// <param name="x">
		///     X coordinate value.
		/// </param>
		/// <param name="y">
		///     Y coordinate value.
		/// </param>
		/// <returns>
		///     Returns int with height data.
		/// </returns>
		private static int GetHeight(byte[] heightMap, int x, int y) 
		{          
			// This Returns The Height From A Height Map Index
			x = x % MAP_SIZE;
			// Error Check Our x Value
			y = y % MAP_SIZE;  
			// Error Check Our y Value

			return heightMap[x + (y * MAP_SIZE)];
			// Index Into Our Height Array And Return The Height
		}
		#endregion int GetHeight(byte[] heightMap, int x, int y)

		#region void InitGL()
		/// <summary>
		///     All setup for OpenGL goes here.
		/// </summary>
		/// <returns>
		///     Returns <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		public override void InitGL() 
		{                                          
			// All Setup For OpenGL Goes Here
			Gl.glShadeModel(Gl.GL_SMOOTH);
			// Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0.5f);
			// Black Background
			Gl.glClearDepth(1);
			// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			// Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);
			// The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
			// Really Nice Perspective Calculations

			LoadRawFile("NeHe034.Terrain.raw", MAP_SIZE * MAP_SIZE, 
				ref heightMap); 
		}
		#endregion void InitGL()

		#region void LoadRawFile(string name, int size, ref byte[] heightMap)
		/// <summary>
		///     Read data from file.
		/// </summary>
		/// <param name="name">
		///     Name of file where data resides.
		/// </param>
		/// <param name="size">
		///     Size of file to be read.
		/// </param>
		/// <param name="heightMap">
		///     Where data is put when read.
		/// </param>
		/// <returns>
		///     Returns <c>true</c> if success, <c>false</c> failure.
		/// </returns>
		private void LoadRawFile(string name, int size, ref byte[] heightMap) 
		{
			if(name == null || name == string.Empty) 
			{
			}

			string fileName1 = string.Format("Data{0}{1}",
				// Look For Data\Filename
				Path.DirectorySeparatorChar, name);
			string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",
				// Look For ..\..\Data\Filename
				"..", Path.DirectorySeparatorChar, name);

			// Make Sure The File Exists In One Of The Usual Directories
			if(!File.Exists(name) && !File.Exists(fileName1) && 
				!File.Exists(fileName2)) 
			{
				MessageBox.Show("Can't Find The Height Map!", 
					"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

			if(File.Exists(fileName1)) 
			{
				// Does The File Exist Here?
				name = fileName1;
				// Set To Correct File Path
			}
			else if(File.Exists(fileName2)) 
			{
				// Does The File Exist Here?
				name = fileName2;
				// Set To Correct File Path
			}

			// Open The File In Read / Binary Mode
			using(FileStream fs = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read)) 
			{
				BinaryReader r = new BinaryReader(fs);
				heightMap = r.ReadBytes(size);
			}
		}
		#endregion void LoadRawFile(string name, int size, ref byte[] heightMap)

		#region void RenderHeightMap(byte[] heightMap)
		/// <summary>
		///     This renders the height map as quads.
		/// </summary>
		/// <param name="heightMap">
		///     Height map data.
		/// </param>
		private void RenderHeightMap(byte[] heightMap) 
		{
			// This Renders The Height Map As Quads
			int X, Y;
			// Create Some Variables To Walk The Array With.
			int x, y, z;
			// Create Some Variables For Readability

			if(bRender) 
			{
				// What We Want To Render
				Gl.glBegin(Gl.GL_QUADS);
				// Render Polygons
			}
			else 
			{
				Gl.glBegin(Gl.GL_LINES);
				// Render Lines Instead
			}

			for(X = 0; X < (MAP_SIZE - STEP_SIZE); X += STEP_SIZE) 
			{
				for (Y = 0; Y < (MAP_SIZE-STEP_SIZE); Y += STEP_SIZE) 
				{
					// Get The (X, Y, Z) Value For The Bottom Left Vertex
					x = X;
					y = GetHeight(heightMap, X, Y);
					z = Y;

					SetVertexColor(heightMap, x, z);
					// Set The Color Value Of The Current Vertex
					Gl.glVertex3i(x, y, z);
					// Send This Vertex To OpenGL To Be Rendered 
					// (Integer Points Are Faster)

					// Get The (X, Y, Z) Value For The Top Left Vertex
					x = X;
					y = GetHeight(heightMap, X, Y + STEP_SIZE);
					z = Y + STEP_SIZE;

					SetVertexColor(heightMap, x, z);
					// Set The Color Value Of The Current Vertex
					Gl.glVertex3i(x, y, z);
					// Send This Vertex To OpenGL To Be Rendered

					// Get The (X, Y, Z) Value For The Top Right Vertex
					x = X + STEP_SIZE;
					y = GetHeight(heightMap, X + STEP_SIZE, Y + STEP_SIZE);
					z = Y + STEP_SIZE;

					SetVertexColor(heightMap, x, z);
					// Set The Color Value Of The Current Vertex
					Gl.glVertex3i(x, y, z);
					// Send This Vertex To OpenGL To Be Rendered

					// Get The (X, Y, Z) Value For The Bottom Right Vertex
					x = X + STEP_SIZE;
					y = GetHeight(heightMap, X + STEP_SIZE, Y);
					z = Y;

					SetVertexColor(heightMap, x, z);
					// Set The Color Value Of The Current Vertex
					Gl.glVertex3i(x, y, z);
					// Send This Vertex To OpenGL To Be Rendered
				}
			}
			Gl.glEnd();
			Gl.glColor4f(1, 1, 1, 1);
			// Reset The Color
		}
		#endregion void RenderHeightMap(byte[] heightMap)

		#region Reshape()
		/// <summary>
		///     Resizes and initializes the GL window.
		/// </summary>
		public override void Reshape() 
		{
			base.Reshape(500.0F);
		}
		#endregion ReSizeGLScene(int width, int height)

		#region SetVertexColor(byte[] heightMap, int x, int y)
		/// <summary>
		///     Sets the color value for a particular index, 
		///     depending on the height index.
		/// </summary>
		/// <param name="heightMap">
		///     Height map data.
		/// </param>
		/// <param name="x">
		///     X coordinate value.
		/// </param>
		/// <param name="y">
		///     Y coordinate value.
		/// </param>
		private void SetVertexColor(byte[] heightMap, int x, int y) 
		{
			float fColor = -0.15f + (GetHeight(heightMap, x, y ) / 256.0f);
			Gl.glColor3f(0, 0, fColor);
			// Assign This Blue Shade To The Current Vertex
		}
		#endregion SetVertexColor(byte[] heightMap, int x, int y)

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.UpArrow: 
					scaleValue += 0.001f;
					// Increase the scale value to zoom in
					break;
				case Key.DownArrow:
					scaleValue -= 0.001f;
					// Increase the scale value to zoom in
					break;				
			}
		}

		private void MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			bRender = !bRender;
		}
	}
}