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
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class NeHe009 : NeHe006
	{    
		public new static string Title
		{
			get
			{
				return "Lesson 9: Moving Bitmaps in 3D Space";
			}
		}

		private static Random rand = new Random();                              
		// Random Number Generator
		private static bool twinkle;                                            
		// Twinkling Stars
		private static bool tp;                                                 
		// 'T' Key Pressed?
		private const int num = 50;                                             
		// Number Of Stars To Draw
		private struct star 
		{                                                   
			// Create A Structure For Star
			public byte r, g, b;                                                
			// Stars Color
			public float dist;                                                  
			// Stars Distance From Center
			public float angle;                                                 
			// Stars Current Angle
		}
		private static star[] stars = new star[num];                            
		// Need To Keep Track Of 'num' Stars
		private static float zoom = -15;                                        
		// Distance Away From Stars
		private static float tilt = 90;                                         
		// Tilt The View
		private static float spin;                                              
		// Spin Stars
		private static int loop; 

		public NeHe009()
		{
			this.TextureName = "NeHe009.bmp";
			this.Texture = new int[1];
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Keyboard.EnableKeyRepeat(150,50);
		}

		public override void InitGL()
		{
			this.LoadGLTextures();
			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      
			// Enable Texture Mapping
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      
			// Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0.5f);                                     
			// Black Background
			Gl.glClearDepth(1);                                                 
			// Depth Buffer Setup
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         
			// Really Nice Perspective Calculations
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);                         
			// Set The Blending Function For Translucency
			Gl.glEnable(Gl.GL_BLEND);

			for(loop = 0; loop < num; loop++) 
			{
				stars[loop].angle = 0;
				stars[loop].dist = ((float) loop / num) * 5;
				stars[loop].r = (byte) (rand.Next() % 256);
				stars[loop].g = (byte) (rand.Next() % 256);
				stars[loop].b = (byte) (rand.Next() % 256);
			}
		}


		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear The Screen And The Depth Buffer
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);                     
			// Select Our Texture

			for(loop = 0; loop < num; loop++) 
			{                                 
				// Loop Through All The Stars
				Gl.glLoadIdentity();                                            
				// Reset The View Before We Draw Each Star
				Gl.glTranslatef(0, 0, zoom);                                    
				// Zoom Into The Screen (Using The Value In 'zoom')
				Gl.glRotatef(tilt, 1, 0, 0);                                    
				// Tilt The View (Using The Value In 'tilt')
				Gl.glRotatef(stars[loop].angle, 0, 1, 0);                       
				// Rotate To The Current Stars Angle
				Gl.glTranslatef(stars[loop].dist, 0, 0);                        
				// Move Forward On The X Plane
				Gl.glRotatef(-stars[loop].angle, 0, 1, 0);                      
				// Cancel The Current Stars Angle
				Gl.glRotatef(-tilt, 1, 0, 0);                                   
				// Cancel The Screen Tilt
				if(twinkle) 
				{
					Gl.glColor4ub(stars[(num - loop) - 1].r, stars[(num - loop) - 1].g, stars[(num - loop) - 1].b, 255);
					Gl.glBegin(Gl.GL_QUADS);
					Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 0);
					Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 0);
					Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 0);
					Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 0);
					Gl.glEnd();
				}
				Gl.glRotatef(spin, 0, 0, 1);
				Gl.glColor4ub(stars[loop].r, stars[loop].g, stars[loop].b, 255);
				Gl.glBegin(Gl.GL_QUADS);
				Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 0);
				Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 0);
				Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 0);
				Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 0);
				Gl.glEnd();
				spin += 0.01f;
				stars[loop].angle += ((float) loop / num);
				stars[loop].dist -= 0.01f;
				if(stars[loop].dist < 0) 
				{
					stars[loop].dist += 5;
					stars[loop].r = (byte) (rand.Next() % 256);
					stars[loop].g = (byte) (rand.Next() % 256);
					stars[loop].b = (byte) (rand.Next() % 256);
				}
			}

			Video.GLSwapBuffers();
		}
		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.T: 
					if (!tp)
					{
						tp = true;
						twinkle = !twinkle;
					}
					else
					{ 
						tp = false;
					}
					break;	
				case Key.PageUp:
					zoom -= 0.2f;
					break;
				case Key.PageDown:
					zoom += 0.2f;
					break;
				case Key.UpArrow: 
					tilt -= 0.01f;
					break;
				case Key.DownArrow:
					tilt += 0.01f;
					break;
			}
		}
		#region bool override LoadGLTextures()
		/// <summary>
		///     Load bitmaps and convert to textures.
		/// </summary>
		/// <returns>
		///     <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		protected override void LoadGLTextures() 
		{                                           
			if (File.Exists(this.DataDirectory + this.TextureName))
			{																	
				this.FilePath = "";															
			}                                              
			// Status Indicator
			Bitmap[] textureImage = new Bitmap[1];                              
			// Create Storage Space For The Texture

			textureImage[0] = new Bitmap(this.FilePath + this.DataDirectory + this.TextureName);
			// Load The Bitmap
			// Check For Errors, If Bitmap's Not Found, Quit
			if(textureImage[0] != null) 
			{
				Gl.glGenTextures(1, out this.Texture[0]);                                   

				textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     
				// Flip The Bitmap Along The Y-Axis
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
				
				if(textureImage[0] != null) 
				{                                   
					// If Texture Exists
					textureImage[0].UnlockBits(bitmapData);                     
					// Unlock The Pixel Data From Memory
					textureImage[0].Dispose();                                  
					// Dispose The Bitmap
				}
			}
		}
		#endregion bool LoadGLTextures()
	}
}