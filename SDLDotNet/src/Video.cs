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
using Tao.Sdl;

namespace SdlDotNet 
{
	#region OpenGLAttribute
	/// <summary>
	/// Public enumeration for setting the OpenGL window Attributes
	/// </summary>
	/// <remarks>
	/// While you can set most OpenGL attributes normally, 
	/// the attributes list above must be known before SDL 
	/// sets the video mode.
	/// </remarks>
	public enum OpenGLAttr
	{
		/// <summary>
		/// Size of the framebuffer red component, in bits
		/// </summary>
		RedSize = Sdl.SDL_GL_RED_SIZE,
		/// <summary>
		/// Size of the framebuffer green component, in bits
		/// </summary>
		GreenSize = Sdl.SDL_GL_GREEN_SIZE,
		/// <summary>
		/// Size of the framebuffer blue component, in bits
		/// </summary>
		BlueSize = Sdl.SDL_GL_BLUE_SIZE,
		/// <summary>
		/// Size of the framebuffer alpha component, in bits
		/// </summary>
		AlphaSize = Sdl.SDL_GL_ALPHA_SIZE,
		/// <summary>
		/// Size of the framebuffer, in bits
		/// </summary>
		BufferSize = Sdl.SDL_GL_BUFFER_SIZE,
		/// <summary>
		/// 0 or 1, enable or disable double buffering
		/// </summary>
		DoubleBuffer = Sdl.SDL_GL_DOUBLEBUFFER,
		/// <summary>
		/// Size of the depth buffer, in bits
		/// </summary>
		DepthSize = Sdl.SDL_GL_DEPTH_SIZE,
		/// <summary>
		/// Size of the stencil buffer, in bits.
		/// </summary>
		StencilSize = Sdl.SDL_GL_STENCIL_SIZE,
		/// <summary>
		/// Size of the accumulation buffer red component, in bits.
		/// </summary>
		AccumulationRedSize = Sdl.SDL_GL_ACCUM_RED_SIZE,
		/// <summary>
		/// Size of the accumulation buffer green component, in bits.
		/// </summary>
		AccumulationGreenSize = Sdl.SDL_GL_ACCUM_GREEN_SIZE,
		/// <summary>
		/// Size of the accumulation buffer blue component, in bits.
		/// </summary>
		AccumulationBlueSize = Sdl.SDL_GL_ACCUM_BLUE_SIZE,
		/// <summary>
		/// Size of the accumulation buffer alpha component, in bits.
		/// </summary>
		AccumulationAlphaSize = Sdl.SDL_GL_ACCUM_ALPHA_SIZE,
		/// <summary>
		/// 
		/// </summary>
		Stereo = Sdl.SDL_GL_STEREO,
		/// <summary>
		/// 
		/// </summary>
		MultiSampleBuffers = Sdl.SDL_GL_MULTISAMPLEBUFFERS,
		/// <summary>
		/// 
		/// </summary>
		MultiSampleSamples = Sdl.SDL_GL_MULTISAMPLESAMPLES
	}
	#endregion OpenGLAttribute

	/// <summary>
	/// Provides methods to set the video mode, create video surfaces, 
	/// hide and show the mouse cursor,
	/// and interact with OpenGL
	/// </summary>
	public sealed class Video
	{
		static readonly Video instance = new Video();

		Video()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public static Video Instance
		{
			get 
			{
				if (Sdl.SDL_Init(Sdl.SDL_INIT_VIDEO)!= 0)
				{
					throw SdlException.Generate();
				}
				return instance;
			}
		}

		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">width</param>
		/// <param name="height">height</param>
		/// <returns>a surface to draw to</returns>
		public Surface SetVideoMode(int width, int height) 
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
		public Surface SetVideoMode(int width, int height, int bitsPerPixel) 
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
		public Surface SetVideoModeWindow(int width, int height, bool frame) 
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
		public Surface SetVideoModeWindow(
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
		public Surface SetVideoModeOpenGL(int width, int height, int bitsPerPixel) 
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
		public Surface SetVideoModeWindowOpenGL(
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
		public Surface SetVideoMode(int width, int height, int bitsPerPixel, int flags) 
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
		public Surface GetVideoSurface
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
		/// This method can be used to generate a Surface from 
		/// a pointer generated in an external library. It is 
		/// intended for use by people building wrappers of 
		/// external SDL libraries such as SDL_ttf.
		/// </summary>
		/// <returns>An Surface representing the real
		/// SDL_Surface underlying the IntPtr.</returns>
		public Surface GenerateSurfaceFromPointer( IntPtr pointer )
		{
			Surface s = new Surface(pointer); 
			if (s == null)
			{
				throw SdlException.Generate();
			}
			//return Surface.FromScreenPtr(s); 
			return s; 
		}

		/// <summary>
		/// Loads an image file from disk
		/// </summary>
		/// <param name="file">The filename of the bitmap to load</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadImage(string file) 
		{
			return Surface.FromImageFile(file);
		}

		/// <summary>
		/// Loads a bitmap from a System.Drawing.Bitmap object, 
		/// usually obtained from a resource
		/// </summary>
		/// <param name="bitmap">The bitmap object to load</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadImage(System.Drawing.Bitmap bitmap) 
		{
			return Surface.FromBitmap(bitmap);
		}

		/// <summary>
		/// Loads a bitmap from an array of bytes in memory.
		/// </summary>
		/// <param name="bitmap">The bitmap data</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadBmp(byte[] bitmap) 
		{
			return Surface.FromBitmap(bitmap);
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
		public Surface CreateRgbSurface(
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
		/// Shows the mouse cursor
		/// </summary>
		public void ShowMouseCursor() 
		{
			Sdl.SDL_ShowCursor(Sdl.SDL_ENABLE);
		}

		/// <summary>
		/// Hides the mouse cursor
		/// </summary>
		public void HideMouseCursor() 
		{
			Sdl.SDL_ShowCursor(Sdl.SDL_DISABLE);
		}

		/// <summary>
		/// Queries the current cursor state
		/// </summary>
		/// <returns>True if the cursor is visible, otherwise False</returns>
		public bool IsCursorVisible() 
		{
			return (Sdl.SDL_ShowCursor(Sdl.SDL_QUERY) == Sdl.SDL_ENABLE);
		}

		/// <summary>
		/// Move the mouse cursor to a specific location
		/// </summary>
		/// <param name="x">The X coordinite</param>
		/// <param name="y">The Y coordinite</param>
		public void WarpCursor(int x, int y) 
		{
			Sdl.SDL_WarpMouse((short)x, (short)y);
		}

		/// <summary>
		/// Swaps the OpenGL screen, only if the double-buffered 
		/// attribute was set.
		/// Call this instead of Surface.Flip() for OpenGL windows.
		/// </summary>
		public void GLSwapBuffers() 
		{
			Sdl.SDL_GL_SwapBuffers();
		}
		/// <summary>
		/// Sets an OpenGL attribute
		/// </summary>
		/// <param name="attribute">The attribute to set</param>
		/// <param name="attributeValue">The new attribute value</param>
		public void GLSetAttribute(OpenGLAttr attribute, int attributeValue) 
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
		public int GLGetAttribute(OpenGLAttr attribute) 
		{
			int ret;
			if (Sdl.SDL_GL_GetAttribute((int)attribute, out ret) != 0)
			{
				throw SdlException.Generate();
			}
			return ret;
		}
	}
}
