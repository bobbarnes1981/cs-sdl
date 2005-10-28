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
	class NeHe007 : NeHe006
	{    
		private float xrot;                                              
		// X Rotation ( NEW )
		private float yrot;                                              
		// Y Rotation ( NEW )
		
		private float xspeed;                                            
		// X Rotation Speed
		private float yspeed;                                            
		// Y Rotation Speed
		private float z = -5;                                            
		// Depth Into The Screen
		private int filter;    
		// Lighting ON/OFF ( NEW )
        private bool light;   
		private bool lp;                                                
		// L Pressed? ( NEW )
		private bool fp;                                                
		// F Pressed? ( NEW )
		// Storage For One Texture ( NEW )
		private float[] lightAmbient = {0.5f, 0.5f, 0.5f, 1};
		private float[] lightDiffuse = {1, 1, 1, 1};
		private float[] lightPosition = {0, 0, 2, 1};

		public NeHe007()
		{
			this.TextureName = "NeHe007.bmp";
			this.Texture = new int[3];
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
		}

		public override void InitGL()
		{
			base.InitGL ();
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, lightAmbient);            
			// Setup The Ambient Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, lightDiffuse);            
			// Setup The Diffuse Light
			Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lightPosition);          
			// Position The Light
			Gl.glEnable(Gl.GL_LIGHT1);                                          
			// Enable Light One
		}


		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();                                                
			// Reset The View
			Gl.glTranslatef(0, 0, z);

			Gl.glRotatef(xrot, 1, 0, 0);
			Gl.glRotatef(yrot, 0, 1, 0);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[filter]);

			Gl.glBegin(Gl.GL_QUADS);
			// Front Face
			Gl.glNormal3f(0, 0, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 1);
			// Back Face
			Gl.glNormal3f(0, 0, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, -1);
			// Top Face
			Gl.glNormal3f(0, 1, 0);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			// Bottom Face
			Gl.glNormal3f(0, -1, 0);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			// Right face
			Gl.glNormal3f(1, 0, 0);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			// Left Face
			Gl.glNormal3f(-1, 0, 0);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glEnd();

			xrot += xspeed;
			yrot += yspeed;

			Video.GLSwapBuffers();
		}
		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			switch (e.Key) 
			{
				case Key.Escape:
					this.QuitFlag = true;
					break;
				case Key.F1:
					if ((this.Screen.FullScreen)) 
					{
						this.Screen = Video.SetVideoModeWindowOpenGL(this.Width, this.Height, true);
						this.WindowAttributes();
					}
					else 
					{
						this.Screen = Video.SetVideoModeOpenGL(this.Width, this.Height, this.Bpp);
					}
					Reshape();
					break;
				case Key.L: 
					if (!lp)
					{
						lp = true;
						light = !light;
						if(!light) 
						{
							Gl.glDisable(Gl.GL_LIGHTING);
						}
						else 
						{
							Gl.glEnable(Gl.GL_LIGHTING);
						}
					}
					else
					{ 
						lp = false;
					}
					break;	
				case Key.F:
					if (!fp)
					{
						fp = true;
						filter += 1;
						if(filter > 2) 
						{
							filter = 0;
						}
					}
					else
					{
						fp = false;
					}
					break;
				case Key.PageUp:
					z -= 0.02f;
					break;
				case Key.PageDown:
					z += 0.02f;
					break;
				case Key.UpArrow: 
					xspeed -= 0.01f;
					break;
				case Key.DownArrow:
					xspeed += 0.01f;
					break;
				case Key.RightArrow:
					yspeed += 0.01f;
					break;
				case Key.LeftArrow:
					yspeed -= 0.01f;
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
				Gl.glGenTextures(3, this.Texture);                                   
				// Create Three Textures

				textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     
				// Flip The Bitmap Along The Y-Axis
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Create Nearest Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[1]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[2]);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
				Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

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

		protected bool Lp
		{
			get
			{
				return lp;
			}
			set
			{
				lp = value;
			}
		}

		protected bool Fp
		{
			get
			{
				return fp;
			}
			set
			{
				fp = value;
			}
		}

		protected bool Light
		{
			get
			{
				return light;
			}
			set
			{
				light = value;
			}
		}
	}
}