using System;
using System.IO;
using System.Drawing;
using SDLDotNet;

namespace SDLDotNet.Images
{

	/// <summary>
	/// <para>Represents a Image that can be drawn on a SDLDotNet.Surface.</para>
	/// <para>The image can be loaded from from a file, a System.Drawing.Bitmap, or a byte array.</para>
	/// <para>Supported image formats follows the development cycle of SDL_Image. Currently, supported and planned supported image formats are:</para>
	/// 
	/// <para>
	/// <list type="bullet">
	///		<item><term>.BMP</term><description>Windows Bitmap</description></item>
	///		<item><term>.PNM</term><description>Portable Anymap File Format</description></item>
	///		<item><term>.XPM</term><description>X PixMap</description></item>
	///		<item><term>.LBM</term><description>Tagged Image File Format</description></item>
	///		<item><term>.PCX</term><description>Z-Soft’s PC Paintbrush file format</description></item>
	///		<item><term>.GIF</term><description>Graphics Interchange Format</description></item>
	///		<item><term>.JPG</term><description>Joint Photographic Experts Group (JPEG)</description></item>
	///		<item><term>.TIF</term><description>Tagged Image File Format</description></item>
	///		<item><term>.PNG</term><description>Portable Network Graphics</description></item>
	///		<item><term>.TGA</term><description>Truevision (Targa) File Format</description></item>
	///	</list>
	///	</para>
	/// 
	/// <para>Usage:</para>
	/// <code>
	/// SDLImage image = SDLImage("mybitmap.jpg")
	/// image.Draw(screen, new Rectangle(new Point(0,0),image.Size))
	/// </code>
	/// </summary> 
	public unsafe class SDLImage
	{

		/// <summary>
		/// Private field that holds an instance of SDLDotNet.SDL
		/// </summary>
		private SDL sdl = SDL.Instance;

		/// <summary>
		/// Private field that holds the surface of the image.
		/// </summary>
		private Surface surface;

		/// <summary>
		/// Private field. Used by the Transparent property 
		/// </summary>
		private bool transparent;

		/// <summary>
		/// Private field. Used by the TransparentColor property 
		/// </summary>
		private Color transparentcolor;

		/// <summary>
		/// Private field. Used by the AlphaFlags property 
		/// </summary>
		private Alpha alphaflags;

		/// <summary>
		/// Private field. Used by the AlphaValue property 
		/// </summary>
		private byte alphavalue;


		/// <summary>
		/// Create a SDLImage instance from a diskfile
		/// </summary>
		/// <param name="filename">The filename of the image to load</param>
		public SDLImage(string filename)
		{
			IntPtr pSurface = Natives.IMG_Load(filename);
			if (pSurface == IntPtr.Zero) throw SDLImageException.Generate();
			surface = sdl.Video.GenerateSurfaceFromPointer(pSurface);
		}

	
		/// <summary>
		/// Create a SDLImage instance from a byte array in memory.
		/// </summary>
		/// <param name="arr">A array of byte that shold the image data</param>
		public SDLImage(byte[] arr)
		{
			IntPtr pSurface = Natives.IMG_Load_RW(Natives.SDL_RWFromMem(arr, arr.Length), 1);
			if (pSurface == IntPtr.Zero) throw SDLImageException.Generate();
			surface = sdl.Video.GenerateSurfaceFromPointer(pSurface);
		}

		
		#if !__MONO__
		/// <summary>
		/// Create a SDLImage instance from a System.Drawing.Bitmap object. Loads a bitmap from a System.Drawing.Bitmap object, usually obtained from a resource.
		/// </summary>
		/// <param name="bitmap">A System.Drawing.Bitmap object</param>
		public SDLImage(System.Drawing.Bitmap bitmap)
		{
			MemoryStream stream = new MemoryStream();
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			byte[] arr = stream.ToArray();
			IntPtr pSurface = Natives.IMG_Load_RW(Natives.SDL_RWFromMem(arr, arr.Length), 1);
			if (pSurface == IntPtr.Zero) throw SDLImageException.Generate();
			surface = sdl.Video.GenerateSurfaceFromPointer(pSurface);
		}
		#endif
		

		~SDLImage() {
			surface.Dispose();
		}


		/// <summary>
		/// The SDLDotNet.Surface that represents the Surface of the SDLImage. 
		/// </summary>
		public Surface Surface 
		{ 
			get { return surface; } 
		}


		/// <summary>
		/// The width of the image
		/// </summary>
		public int Width 
		{ 
			get { return surface.Width; } 
		}

		/// <summary>
		/// The height of the image
		/// </summary>
		public int Height
		{ 
			get { return surface.Height;} 		
		}

		/// <summary>
		/// The size of the image
		/// </summary>
		public Size Size
		{ 
			get { return surface.Size;} 		
		}


		/// <summary>
		/// Get/set the transparency of the image.  
		/// </summary>
		public bool Transparent
		{
			get {return transparent;}
			set	
			{
				transparent = value;
				if (value)
				{
					surface.SetColorKey(transparentcolor,true);
				}
				else 
				{
					surface.ClearColorKey();
				}
			}
		}

		/// <summary>
		/// Get/set the transparent color of the image.
		/// </summary>
		public Color TransparentColor
		{
			get {return transparentcolor;}
			set	
			{
				transparentcolor = value;
				if (Transparent) surface.SetColorKey(transparentcolor,true);
			}
		}

		/// <summary>
		/// Get/set the Alpha flags of the image.
		/// </summary>
		public Alpha AlphaFlags
		{
			get {return alphaflags;}
			set	
			{
				alphaflags = value;
				surface.SetAlpha(alphaflags,alphavalue);
			}
		}

		/// <summary>
		/// Get/set the Alpha value of the image. 0 indicates that the image fully transparent. 255 indicates that the image is not tranparent.
		/// </summary>
		public byte AlphaValue
		{
			get {return alphavalue;}
			set	
			{
				alphavalue = value;
				surface.SetAlpha(alphaflags,alphavalue);
			}
		}


		/// <summary>
		/// Draws the image on a SDLDotNet.Surface.
		/// </summary>
		/// <param name="dest">The SDLDotNet.Surface to draw the image upon</param>
		/// <param name="destrect">The position of the image on the destination surface</param>
		public void Draw(Surface dest, Rectangle destrect) 
		{
			surface.Blit(dest,destrect);
		}

		/// <summary>
		/// Draws the image on a SDLDotNet.Surface.
		/// </summary>
		/// <param name="srcrect">The area of the image that is to be drawn on the destination surface</param>
		/// <param name="dest">The SDLDotNet.Surface to draw the image upon</param>
		/// <param name="destrect">The position of the image on the destination surface</param>
		public void Draw(Rectangle srcrect, Surface dest, Rectangle destrect) 
		{
			surface.Blit(srcrect,dest,destrect);
		}


	}
}
