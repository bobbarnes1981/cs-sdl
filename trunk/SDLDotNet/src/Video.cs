/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
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
using System.Runtime.InteropServices;
using Tao.Sdl;

namespace SdlDotNet 
{
	/// <summary>
	/// Provides methods to set the video mode, create video surfaces, 
	/// hide and show the mouse cursor,
	/// and interact with OpenGL
	/// </summary>
	public sealed class Video
	{
		static private bool disposed = false;
		static readonly Video instance = new Video();
		static Mouse mouse = Mouse.Instance;
		static VideoInfo videoInfo = VideoInfo.Instance;

//		static Video()
//		{
//		}

		Video()
		{
			Initialize();
		}

		/// <summary>
		/// 
		/// </summary>
		~Video() 
		{
			Dispose(false);
		}
		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Dispose() 
		{
			Dispose(true);
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		/// <param name="disposing"></param>
		public static void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Sdl.SDL_QuitSubSystem(Sdl.SDL_INIT_VIDEO);
				}
				disposed = true;
			}
		}

		/// <summary>
		/// Closes and destroys this object
		/// </summary>
		public static void Close() 
		{
			Dispose();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Initialize()
		{
			if ((Sdl.SDL_WasInit(Sdl.SDL_INIT_VIDEO) & Sdl.SDL_INIT_VIDEO) 
				== (int) SdlFlag.FalseValue)
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO)!= (int) SdlFlag.Success)
				{
					throw SdlException.Generate();
				}
			}
		}

		/// <summary>
		/// Queries if the Video subsystem has been intialized.
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <returns>True if Video subsystem has been initialized, false if it has not.</returns>
		public static bool IsInitialized
		{
			get
			{
				if ((Sdl.SDL_WasInit(Sdl.SDL_INIT_VIDEO) & Sdl.SDL_INIT_VIDEO) 
					== (int) SdlFlag.TrueValue)
				{
					return true;
				}
				else 
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Checks if the requested video mode is supported
		/// </summary>
		/// <param name="width">Width of mode</param>
		/// <param name="height">Height of mode</param>
		/// <param name="fullscreen">Fullscreen or not</param>
		/// <param name="bitsPerPixel">Bits per pixel. Typically 8, 16, 24 or 32</param>
		/// <remarks></remarks>
		/// <returns>True is mode is supported, false if it is not.</returns>
		public static bool IsVideoModeOK(int width, int height, bool fullscreen, int bitsPerPixel)
		{
			int flags = (int)(Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF|Sdl.SDL_FULLSCREEN);
			if (fullscreen)
			{
				flags |= Sdl.SDL_FULLSCREEN;
			}
			int result = Sdl.SDL_VideoModeOK(
				width, 
				height, 
				bitsPerPixel, 
				flags);
			if (result == bitsPerPixel)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Checks if the application is active
		/// </summary>
		/// <returns>True is applications is active</returns>
		public static bool IsActive
		{
			get
			{
				return (Sdl.SDL_GetAppState() & Sdl.SDL_APPACTIVE) !=0;
			}
		}
		/// <summary>
		/// Returns the highest bitsperpixel supported for the given width and height
		/// </summary>
		/// <param name="width">Width of mode</param>
		/// <param name="height">Height of mode</param>
		/// <param name="fullscreen">Fullscreen mode</param>
		public static int BestBitsPerPixel(int width, int height, bool fullscreen)
		{
			int flags = (int)(Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF);
			if (fullscreen)
			{
				flags |= Sdl.SDL_FULLSCREEN;
			}
			return Sdl.SDL_VideoModeOK(
				width, 
				height, 
				VideoInfo.BitsPerPixel, 
				flags);
		}


		/// <summary>
		/// Returns array of modes supported
		/// </summary>
		/// <param name="fullscreen">Fullscreen mode</param>
		/// <returns>Array of Size structs</returns>
		public static Size[] ListModes(bool fullscreen)
		{
			int flags = (int)(Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF);
			if (fullscreen)
			{
				flags |= Sdl.SDL_FULLSCREEN;
			}
			IntPtr format = IntPtr.Zero;
			Sdl.SDL_Rect[] rects = Sdl.SDL_ListModes(format, flags);
			Size[] size = new Size[rects.Length];
			for (int i=0; i<rects.Length; i++)
			{
				size[ i ].Width = rects[ i ].w;
				size[ i ].Height = rects[ i ].h; 
			}
			return size;
		}
		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">width</param>
		/// <param name="height">height</param>
		/// <returns>a surface to draw to</returns>
		public static Surface SetVideoMode(int width, int height) 
		{
			return SetVideoMode(width, height, 0,
				(int)(Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF|Sdl.SDL_FULLSCREEN|
				Sdl.SDL_ANYFORMAT));
		}
		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="bitsPerPixel">bits per pixel</param>
		/// <returns>a surface to draw to</returns>
		public static Surface SetVideoMode(int width, int height, int bitsPerPixel) 
		{
			return SetVideoMode(width, height, bitsPerPixel,
				(int)(Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF|Sdl.SDL_FULLSCREEN|Sdl.SDL_ANYFORMAT));
		}

		/// <summary>
		/// Sets the windowed video mode using current screen bpp
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">
		/// if true, the window will have a frame around it
		/// </param>
		/// <returns>a surface to draw to</returns>
		public static Surface SetVideoModeWindow(int width, int height, bool frame) 
		{
			int flags = Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF|Sdl.SDL_ANYFORMAT;
			if (!frame)
			{
				flags |= Sdl.SDL_NOFRAME;
			}
			return SetVideoMode(width, height, 0, (int)flags);
		}
		/// <summary>
		/// Sets the windowed video mode
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">
		/// if true, the window will have a frame around it
		/// </param>
		/// <param name="bitsPerPixel">bits per pixel</param>
		/// <returns>a surface to draw to</returns>
		public static Surface SetVideoModeWindow(
			int width, 
			int height, 
			int bitsPerPixel, 
			bool frame) 
		{
			int flags = Sdl.SDL_HWSURFACE|Sdl.SDL_DOUBLEBUF|Sdl.SDL_ANYFORMAT;
			if (!frame)
			{
				flags |= Sdl.SDL_NOFRAME;
			}
			return SetVideoMode(width, height, bitsPerPixel, (int)flags);
		}
		
		/// <summary>
		/// Sets a full-screen video mode suitable for drawing with OpenGL
		/// </summary>
		/// <param name="width">the horizontal resolution</param>
		/// <param name="height">the vertical resolution</param>
		/// <param name="bitsPerPixel">bits per pixel</param>
		/// <returns>A Surface representing the screen</returns>
		public static Surface SetVideoModeOpenGL(int width, int height, int bitsPerPixel) 
		{
			return SetVideoMode(width, height, bitsPerPixel,
				(int)(Sdl.SDL_HWSURFACE|Sdl.SDL_OPENGL|Sdl.SDL_FULLSCREEN));
		}
		/// <summary>
		/// Sets a windowed video mode suitable for drawing with OpenGL
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">
		/// If true, the window will have a frame around it
		/// </param>
		/// <returns>A Surface representing the window</returns>
		public static Surface SetVideoModeWindowOpenGL(
			int width, 
			int height, 
			bool frame) 
		{
			int flags = Sdl.SDL_HWSURFACE|Sdl.SDL_OPENGL|Sdl.SDL_ANYFORMAT;
			if (!frame)
			{
				flags |= Sdl.SDL_NOFRAME;
			}
			return SetVideoMode(width, height, 0, (int)flags);
		}
		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="bitsPerPixel">bits per pixel</param>
		/// <param name="flags">
		/// specific flags, see SDL documentation for details
		/// </param>
		/// <returns>A Surface representing the screen</returns>
		public static Surface SetVideoMode(int width, int height, int bitsPerPixel, int flags) 
		{
			IntPtr s = Sdl.SDL_SetVideoMode(width, height, bitsPerPixel, flags);
			if (s == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(s);
		}


		/// <summary>
		/// Gets the surface for the window or screen, 
		/// must be preceded by a call to SetVideoMode
		/// </summary>
		/// <returns>The main screen surface</returns>
		public static Surface Screen
		{
			get
			{
				IntPtr s = Sdl.SDL_GetVideoSurface();
				if (s == IntPtr.Zero)
				{
					throw SdlException.Generate();
				}
				return Surface.FromScreenPtr(s);
			}
		}

		/// <summary>
		/// Creates a new empty surface
		/// </summary>
		/// <param name="width">The width of the surface</param>
		/// <param name="height">The height of the surface</param>
		/// <param name="depth">The bits per pixel of the surface</param>
		/// <param name="redMask">
		/// A bitmask giving the range of red color values in the surface 
		/// pixel format
		/// </param>
		/// <param name="greenMask">
		/// A bitmask giving the range of green color values in the surface 
		/// pixel format
		/// </param>
		/// <param name="blueMask">
		/// A bitmask giving the range of blue color values in the surface 
		/// pixel format
		/// </param>
		/// <param name="alphaMask">
		/// A bitmask giving the range of alpha color values in the surface 
		/// pixel format
		/// </param>
		/// <param name="hardware">
		/// A flag indicating whether or not to attempt to place this surface
		///  into video memory</param>
		/// <returns>A new surface</returns>
		public static Surface CreateRgbSurface(
			int width, 
			int height, 
			int depth, 
			int redMask, 
			int greenMask, 
			int blueMask, 
			int alphaMask, 
			bool hardware) 
		{
			IntPtr ret = Sdl.SDL_CreateRGBSurface(
				hardware?Sdl.SDL_HWSURFACE:Sdl.SDL_SWSURFACE,
				width, height, depth,
				redMask, greenMask, blueMask, alphaMask);
			if (ret == IntPtr.Zero)
			{
				throw SdlException.Generate();
			}
			return new Surface(ret);
		}

		/// <summary>
		/// Mouse object
		/// </summary>
		public static Mouse Mouse
		{
			get
			{
				return Video.mouse;
			}
		}

		/// <summary>
		/// Returns VideoInfo object
		/// </summary>
		public static VideoInfo VideoInfo
		{
			get
			{
				return Video.videoInfo;
			}
		}

		/// <summary>
		/// Swaps the OpenGL screen, only if the double-buffered 
		/// attribute was set.
		/// Call this instead of Surface.Flip() for OpenGL windows.
		/// </summary>
		public static void GLSwapBuffers() 
		{
			Sdl.SDL_GL_SwapBuffers();
		}
		/// <summary>
		/// Sets an OpenGL attribute
		/// </summary>
		/// <param name="attribute">The attribute to set</param>
		/// <param name="attributeValue">The new attribute value</param>
		public static void GLSetAttribute(OpenGLAttr attribute, int attributeValue) 
		{
			if (Sdl.SDL_GL_SetAttribute((int)attribute, attributeValue) != 0)
			{
				throw SdlException.Generate();
			}
		}
		/// <summary>
		/// Gets the value of an OpenGL attribute
		/// </summary>
		/// <param name="attribute">The attribute to get</param>
		/// <returns>The current attribute value</returns>
		public static int GLGetAttribute(OpenGLAttr attribute) 
		{
			int ret;
			if (Sdl.SDL_GL_GetAttribute((int)attribute, out ret) != 0)
			{
				throw SdlException.Generate();
			}
			return ret;
		}

		/// <summary>
		/// gets or sets the text for the current window
		/// </summary>
		public static string WindowCaption 
		{
			get
			{
				string ret;
				string dummy;

				Sdl.SDL_WM_GetCaption(out ret, out dummy);
				return ret;
			}
			set
			{
				Sdl.SDL_WM_SetCaption(value, "");
			}
		}

		/// <summary>
		/// sets the icon for the current window
		/// </summary>
		/// <param name="icon">the surface containing the image</param>
		public static void WindowIcon(Surface icon) 
		{
			Sdl.SDL_WM_SetIcon(icon.Handle, null);
		}

		/// <summary>
		/// Iconifies (minimizes) the current window
		/// </summary>
		/// <returns>True if the action succeeded, otherwise False</returns>
		public static bool IconifyWindow() 
		{
			return (Sdl.SDL_WM_IconifyWindow() != (int) SdlFlag.Success);
		}
		/// <summary>
		/// Forces keyboard focus and prevents the mouse from leaving the window
		/// </summary>
		public static void GrabInput() 
		{
			Sdl.SDL_WM_GrabInput(Sdl.SDL_GRAB_ON);
		}
		/// <summary>
		/// Queries if input has been grabbed.
		/// </summary>
		public static bool IsInputGrabbed
		{
			get
			{
				return (Sdl.SDL_WM_GrabInput(Sdl.SDL_GRAB_QUERY) == Sdl.SDL_GRAB_ON);
			}
		}
		/// <summary>
		/// Releases keyboard and mouse focus from a previous call to GrabInput()
		/// </summary>
		public static void ReleaseInput() 
		{
			Sdl.SDL_WM_GrabInput(Sdl.SDL_GRAB_OFF);
		}

		/// <summary>
		/// Returns video driver name
		/// </summary>
		public string VideoDriver
		{
			get
			{
				string buffer="";
				return Sdl.SDL_VideoDriverName(buffer, 100);
			}
		}

		/// <summary>
		/// Sets gamma
		/// </summary>
		/// <param name="red">Red</param>
		/// <param name="green">Green</param>
		/// <param name="blue">Blue</param>
		public void Gamma(float red, float green, float blue)
		{
			int result = Sdl.SDL_SetGamma(red, green, blue);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// Sets gamma for all colors
		/// </summary>
		/// <param name="gammaValue"></param>
		public void Gamma(float gammaValue)
		{
			int result = Sdl.SDL_SetGamma(gammaValue, gammaValue, gammaValue);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] GetGammaRampRed()
		{
			short[] red = new short[256];
			int result = Sdl.SDL_GetGammaRamp(red, null, null);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
			return red;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gammaArray"></param>
		public void SetGammaRampRed(short[] gammaArray)
		{
			int result = Sdl.SDL_SetGammaRamp(gammaArray, null, null);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] GetGammaRampBlue()
		{
			short[] blue = new short[256];
			int result = Sdl.SDL_GetGammaRamp(null, null, blue);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
			return blue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gammaArray"></param>
		public void SetGammaRampBlue(short[] gammaArray)
		{
			int result = Sdl.SDL_SetGammaRamp(null, null, gammaArray);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public short[] GetGammaRampGreen()
		{
			short[] green = new short[256];
			int result = Sdl.SDL_GetGammaRamp(null, green, null);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
			return green;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gammaArray"></param>
		public void SetGammaRampGreen(short[] gammaArray)
		{
			int result = Sdl.SDL_SetGammaRamp(null, gammaArray, null);
			if (result != 0)
			{
				throw SdlException.Generate();
			}
		}
	}
}
