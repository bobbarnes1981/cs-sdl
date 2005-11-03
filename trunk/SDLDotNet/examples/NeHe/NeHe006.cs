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
	/// <summary>
	/// 
	/// </summary>
	public class NeHe006 : NeHe001
	{
		/// <summary>
		/// 
		/// </summary>
		public new static string Title
		{
			get
			{
				return "Lesson 06: Texture Mapping";
			}
		}

		private float xrot;                                              
		// X Rotation ( NEW )
		private float yrot;                                              
		// Y Rotation ( NEW )
		private float zrot;                                              
		// Z Rotation ( NEW )
		private int[] texture;                              
		// Storage For One Texture ( NEW )
		string dataDirectory = @"Data/";
		string filePath = @"../../";
		string textureName;
		
		/// <summary>
		/// 
		/// </summary>
		public NeHe006()
		{
			textureName = "NeHe006.bmp";
			texture = new int[1];
		}

		/// <summary>
		/// 
		/// </summary>
		protected string TextureName
		{
			get
			{
				return textureName;
			}
			set
			{
				textureName = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected int [] Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected string DataDirectory
		{
			get
			{
				return dataDirectory;
			}
			set
			{
				dataDirectory = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected string FilePath
		{
			get
			{
				return filePath;
			}
			set
			{
				filePath = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void InitGL()
		{
			this.LoadGLTextures();
			// Enable Texture Mapping ( NEW )
			Gl.glEnable(Gl.GL_TEXTURE_2D);                                      
			base.InitGL ();
		}

		/// <summary>
		/// 
		/// </summary>
		public override void DrawGLScene()
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        
			// Clear The Screen And The Depth Buffer
			Gl.glLoadIdentity();                                                
			// Reset The View
			Gl.glTranslatef(0, 0, -5);

			Gl.glRotatef(xrot, 1, 0, 0);
			Gl.glRotatef(yrot, 0, 1, 0);
			Gl.glRotatef(zrot, 0, 0, 1);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);

			Gl.glBegin(Gl.GL_QUADS);
			// Front Face
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, 1);
			// Back Face
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, -1);
			// Top Face
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			// Bottom Face
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			// Right face
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(1, -1, -1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(1, 1, -1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(1, 1, 1);
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(1, -1, 1);
			// Left Face
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(-1, -1, -1);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(-1, -1, 1);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(-1, 1, 1);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(-1, 1, -1);
			Gl.glEnd();

			xrot += 0.3f;
			yrot += 0.2f;
			zrot += 0.4f;
		}

		#region void LoadGLTextures()
		/// <summary>
		///     Load bitmaps and convert to textures.
		/// </summary>
		protected virtual void LoadGLTextures() 
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
				// Create The Texture

				textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     
				// Flip The Bitmap Along The Y-Axis
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = 
					new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = 
					textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Typical Texture Generation Using Data From The Bitmap
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, this.Texture[0]);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

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
		#endregion void LoadGLTextures()

		/// <summary>
		/// 
		/// </summary>
		protected float XRot
		{
			get
			{
				return xrot;
			}
			set
			{
				xrot = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected float YRot
		{
			get
			{
				return yrot;
			}
			set
			{
				yrot = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected float ZRot
		{
			get
			{
				return zrot;
			}
			set
			{
				zrot = value;
			}
		}
	}
}