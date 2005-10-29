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
using System.IO;
using System.Drawing;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe011 : NeHeBase
	{
		public float[][][] points;				
		// The Array For The Points On The Grid Of Our "Wave"
		public int wiggle_count = 0;			
		// Counter Used To Control How Fast Flag Waves

		public float xrot = 0.0f;				
		// X Rotation ( NEW )
		public float yrot = 0.0f;				
		// Y Rotation ( NEW )
		public float zrot = 0.0f;				
		// Z Rotation ( NEW )
		
		public uint[] texture = new uint[1];	
		// Texture array

		public NeHe011()
		{
		}

		public override void InitGL()
		{
			LoadTextures();

			Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);									
			// Enable Smooth Shading
			Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.5f);						
			// Black Background
			Gl.glClearDepth(1.0f);											
			// Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);									
			// Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);									
			// The Type Of Depth Testing To Do
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);		
			// Really Nice Perspective Calculations

			Gl.glPolygonMode(Gl.GL_BACK, Gl.GL_FILL);						
			// Back Face Is Solid
			Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_LINE);						
			// Front Face Is Made Of Lines
			
			this.points = new float[45][][];
			for (int i=0; i < this.points.Length; i++)
			{
				this.points[i] = new float[45][];
				for (int j=0; j < this.points[i].Length; j++)
				{
					this.points[i][j] = new float[3];
					this.points[i][j][0] = (float)((i / 5.0f) - 4.5f);
					this.points[i][j][1] = (float)((j / 5.0f) - 4.5f);
					this.points[i][j][2] = (float)(Math.Sin((((i / 5.0f) * 40.0f) / 360.0f) * Math.PI * 2.0f));
				}
			}
		}

		private bool LoadTextures()
		{
			// Load The Bitmap
			string file1 = "NeHe011.bmp";
			string file2 = "Data" + Path.DirectorySeparatorChar + file1;
			string file3 = ".." + Path.DirectorySeparatorChar + ".."  + Path.DirectorySeparatorChar + file2;
			string file = "";
			if(File.Exists(file1))
				file = file1;
			else if(File.Exists(file2))
				file = file2;
			else if(File.Exists(file3))
				file = file3;
			else
				throw new FileNotFoundException(file1);

			using(Bitmap image = new Bitmap(file))
			{
				image.RotateFlip(RotateFlipType.RotateNoneFlipY);
				System.Drawing.Imaging.BitmapData bitmapdata;
				Rectangle rect = new Rectangle(Point.Empty, image.Size);

				bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				Gl.glGenTextures(1, this.texture);
			
				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, (int)Gl.GL_RGB, image.Width, image.Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, bitmapdata.Scan0);

				image.UnlockBits(bitmapdata);
			}
			return true;
		}

		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
			Gl.glLoadIdentity();
			
			float float_x, float_y, float_xb, float_yb;

			Gl.glTranslatef(0.0f, 0.0f, -12.0f);
	  
			Gl.glRotatef(this.xrot, 1.0f, 0.0f, 0.0f);
			Gl.glRotatef(this.yrot, 0.0f, 1.0f, 0.0f);  
			Gl.glRotatef(this.zrot, 0.0f, 0.0f, 1.0f);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.texture[0]);

			Gl.glBegin(Gl.GL_QUADS);
			for (int i=0; i < 44; i++ )
			{
				for (int j=0; j < 44; j++ )
				{
					float_x = (float)i/44.0f;
					float_y = (float)j/44.0f;
					float_xb = (float)(i+1)/44.0f;
					float_yb = (float)(j+1)/44.0f;

					Gl.glTexCoord2f(float_x, float_y);
					Gl.glVertex3f(this.points[i][j][0], this.points[i][j][1], this.points[i][j][2]);

					Gl.glTexCoord2f(float_x, float_yb);
					Gl.glVertex3f(this.points[i][j+1][0], this.points[i][j+1][1], this.points[i][j+1][2]);

					Gl.glTexCoord2f(float_xb, float_yb);
					Gl.glVertex3f(this.points[i+1][j+1][0], this.points[i+1][j+1][1], this.points[i+1][j+1][2]);

					Gl.glTexCoord2f(float_xb, float_y);
					Gl.glVertex3f(this.points[i+1][j][0], this.points[i+1][j][1], this.points[i+1][j][2]);
				}
			}
			Gl.glEnd();				
			// Done Drawing Our Quads
			Video.GLSwapBuffers();	
			// Flip the SDL OpenGL buffers with SDL.NET

			float hold = 0.0f;
			if (this.wiggle_count == 2)
			{
				for (int j=0; j < this.points[0].Length; j++ )
				{
					hold = this.points[0][j][2];
					for (int i=0; i < this.points.Length - 1; i++)
					{
						this.points[i][j][2] = this.points[i+1][j][2];
					}
					this.points[this.points.Length - 1][j][2] = hold;
				}
				this.wiggle_count = 0;
			}

			this.wiggle_count++;

			this.xrot += 0.3f;
			this.yrot += 0.2f;
			this.zrot += 0.4f;
		}
	}
}