/*
 * $RCSfile$
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

namespace SDLDotNet {
	/// <summary>
	/// OpenGL Attributes
	/// </summary>
	public enum GLAttribute {
		/// <summary></summary>
		RedSize,
		/// <summary></summary>
		GreenSize,
		/// <summary></summary>
		BlueSize,
		/// <summary></summary>
		AlphaSize,
		/// <summary></summary>
		BufferSize,
		/// <summary></summary>
		DoubleBuffer,
		/// <summary></summary>
		DepthSize,
		/// <summary></summary>
		StencilSize,
		/// <summary></summary>
		AccumRedSize,
		/// <summary></summary>
		AccumGreenSize,
		/// <summary></summary>
		AccumBlueSize,
		/// <summary></summary>
		AccumAlphaSize,
		/// <summary></summary>
		Stereo
	}

	/// <summary>
	/// Provides methods to set the video mode, create video surfaces, hide and show the mouse cursor,
	/// and interact with OpenGL
	/// </summary>
	unsafe public class Video {
		internal Video() {
			if (Natives.SDL_InitSubSystem((int)Natives.Init.Video) != 0)
				throw SDLException.Generate();
		}

		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">width</param>
		/// <param name="height">height</param>
		/// <returns>a surface to draw to</returns>
		public Surface SetVideoMode(int width, int height) {
			return SetVideoMode(width, height, 0,
				(int)(Natives.Video.HWSurface|Natives.Video.DoubleBuf|Natives.Video.FullScreen|Natives.Video.AnyFormat));
		}
		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="bpp">bits per pixel</param>
		/// <returns>a surface to draw to</returns>
		public Surface SetVideoMode(int width, int height, int bpp) {
			return SetVideoMode(width, height, bpp,
				(int)(Natives.Video.HWSurface|Natives.Video.DoubleBuf|Natives.Video.FullScreen|Natives.Video.AnyFormat));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="flags">sets mode parameters</param>
		/// <returns></returns>
		public Surface SetVideoMode(int width, int height, uint flags) 
		{
			return SetVideoMode(width, height, 0,
				(int)(flags));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="bpp">bits per pixel</param>
		/// <param name="flags">sets mode parameters</param>
		/// <returns></returns>
		public Surface SetVideoMode(int width, int height, int bpp, uint flags) 
		{
			return SetVideoMode(width, height, bpp,
				(int)(flags));
		}
		/// <summary>
		/// Sets the windowed video mode using current screen bpp
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">if true, the window will have a frame around it</param>
		/// <returns>a surface to draw to</returns>
		public Surface SetVideoModeWindow(int width, int height, bool frame) {
			Natives.Video flags = Natives.Video.HWSurface|Natives.Video.DoubleBuf|Natives.Video.AnyFormat;
			if (!frame)
				flags |= Natives.Video.NoFrame;
			return SetVideoMode(width, height, 0, (int)flags);
		}
		/// <summary>
		/// Sets the windowed video mode
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">if true, the window will have a frame around it</param>
		/// <param name="bpp">bits per pixel</param>
		/// <returns>a surface to draw to</returns>
		public Surface SetVideoModeWindow(int width, int height, int bpp, bool frame) {
			Natives.Video flags = Natives.Video.HWSurface|Natives.Video.DoubleBuf|Natives.Video.AnyFormat;
			if (!frame)
				flags |= Natives.Video.NoFrame;
			return SetVideoMode(width, height, bpp, (int)flags);
		}
		
		/// <summary>
		/// Sets a full-screen video mode suitable for drawing with OpenGL
		/// </summary>
		/// <param name="width">the horizontal resolution</param>
		/// <param name="height">the vertical resolution</param>
		/// <param name="bpp">bits per pixel</param>
		/// <returns>A Surface representing the screen</returns>
		public Surface SetVideoModeOpenGL(int width, int height, int bpp) {
			return SetVideoMode(width, height, bpp,
				(int)(Natives.Video.HWSurface|Natives.Video.OpenGL|Natives.Video.FullScreen));
		}
		/// <summary>
		/// Sets a windowed video mode suitable for drawing with OpenGL
		/// </summary>
		/// <param name="width">The width of the window</param>
		/// <param name="height">The height of the window</param>
		/// <param name="frame">If true, the window will have a frame around it</param>
		/// <returns>A Surface representing the window</returns>
		public Surface SetVideoModeWindowOpenGL(int width, int height, bool frame) {
			Natives.Video flags = Natives.Video.HWSurface|Natives.Video.OpenGL|Natives.Video.AnyFormat;
			if (!frame)
				flags |= Natives.Video.NoFrame;
			return SetVideoMode(width, height, 0, (int)flags);
		}
		/// <summary>
		/// Sets the video mode of a fullscreen application
		/// </summary>
		/// <param name="width">screen width</param>
		/// <param name="height">screen height</param>
		/// <param name="bpp">bits per pixel</param>
		/// <param name="flags">specific flags, see SDL documentation for details</param>
		/// <returns>A Surface representing the screen</returns>
		public Surface SetVideoMode(int width, int height, int bpp, int flags) {
			Natives.SDL_Surface *s = Natives.SDL_SetVideoMode(width, height, bpp, flags);
			if (s == null)
				throw SDLException.Generate();
			return Surface.FromScreenPtr(s);
		}


		/// <summary>
		/// Gets the surface for the window or screen, must be preceded by a call to SetVideoMode*
		/// </summary>
		/// <returns>The main screen surface</returns>
		public Surface GetVideoSurface() {
			Natives.SDL_Surface *s = Natives.SDL_GetVideoSurface();
			if (s == null)
				throw SDLException.Generate();
			return Surface.FromScreenPtr(s);
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
			//Natives.SDL_Surface *s = (Natives.SDL_Surface*)pointer.ToPointer();
			Surface s = Surface.FromPtr((Natives.SDL_Surface*)pointer.ToPointer()); 
			if (s == null)
				throw SDLException.Generate();
			//return Surface.FromScreenPtr(s); 
			return s; 
		}

		/// <summary>
		/// Loads a .bmp file from disk
		/// </summary>
		/// <param name="file">The filename of the bitmap to load</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadBMP(string file) {
			return Surface.FromBMPFile(file);
		}

		/// <summary>
		/// Loads a bitmap from a System.Drawing.Bitmap object, usually obtained from a resource
		/// </summary>
		/// <param name="bitmap">The bitmap object to load</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadBMP(System.Drawing.Bitmap bitmap) {
			if (Environment.Version.ToString() == "0.0")
				throw new NotSupportedException("Method not supported on Mono");

			return Surface.FromBitmap(bitmap);
		}

		/// <summary>
		/// Loads a bitmap from an array of bytes in memory.
		/// </summary>
		/// <param name="bitmap">The bitmap data</param>
		/// <returns>A Surface representing the bitmap</returns>
		public Surface LoadBMP(byte[] bitmap) {
			return Surface.FromBitmap(bitmap);
		}

		/// <summary>
		/// Creates a new empty surface
		/// </summary>
		/// <param name="width">The width of the surface</param>
		/// <param name="height">The height of the surface</param>
		/// <param name="depth">The bits per pixel of the surface</param>
		/// <param name="Rmask">A bitmask giving the range of red color values in the surface pixel format</param>
		/// <param name="Gmask">A bitmask giving the range of green color values in the surface pixel format</param>
		/// <param name="Bmask">A bitmask giving the range of blue color values in the surface pixel format</param>
		/// <param name="Amask">A bitmask giving the range of alpha color values in the surface pixel format</param>
		/// <param name="hardware">A flag indicating whether or not to attempt to place this surface into video memory</param>
		/// <returns>A new surface</returns>
		public Surface CreateRGBSurface(int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask, bool hardware) {
			Natives.SDL_Surface *ret = Natives.SDL_CreateRGBSurface(hardware?(int)Natives.Video.HWSurface:(int)Natives.Video.SWSurface,
				width, height, depth,
				Rmask, Gmask, Bmask, Amask);
			if (ret == null)
				throw SDLException.Generate();
			return Surface.FromPtr(ret);
		}

		/// <summary>
		/// Shows the mouse cursor
		/// </summary>
		public void ShowMouseCursor() {
			Natives.SDL_ShowCursor((int)Natives.Enable.Enable);
		}
		/// <summary>
		/// Hides the mouse cursor
		/// </summary>
		public void HideMouseCursor() {
			Natives.SDL_ShowCursor((int)Natives.Enable.Disable);
		}
		/// <summary>
		/// Queries the current cursor state
		/// </summary>
		/// <returns>True if the cursor is visible, otherwise False</returns>
		public bool IsCursorVisible() {
			return (Natives.SDL_ShowCursor((int)Natives.Enable.Query) == (int)Natives.Enable.Enable);
		}
		/// <summary>
		/// Move the mouse cursor to a specific location
		/// </summary>
		/// <param name="x">The X coordinite</param>
		/// <param name="y">The Y coordinite</param>
		public void WarpCursor(int x, int y) {
			Natives.SDL_WarpMouse((ushort)x, (ushort)y);
		}

		/// <summary>
		/// Swaps the OpenGL screen, only if the double-buffered attribute was set.
		/// Call this instead of Surface.Flip() for OpenGL windows.
		/// </summary>
		public void GL_SwapBuffers() {
			Natives.SDL_GL_SwapBuffers();
		}
		/// <summary>
		/// Sets an OpenGL attribute
		/// </summary>
		/// <param name="attrib">The attribute to set</param>
		/// <param name="val">The new attribute value</param>
		public void GL_SetAttribute(GLAttribute attrib, int val) {
			if (Natives.SDL_GL_SetAttribute((int)attrib, val) != 0)
				throw SDLException.Generate();
		}
		/// <summary>
		/// Gets the value of an OpenGL attribute
		/// </summary>
		/// <param name="attrib">The attribute to get</param>
		/// <returns>The current attribute value</returns>
		public int GL_GetAttribute(GLAttribute attrib) {
			int ret;
			if (Natives.SDL_GL_GetAttribute((int)attrib, out ret) != 0)
				throw SDLException.Generate();
			return ret;
		}
	}
}
