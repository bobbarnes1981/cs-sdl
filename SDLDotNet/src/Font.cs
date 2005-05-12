/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Font Class
	/// </summary>
	public class Font : BaseSdlResource
	{
		private bool disposed;
	
		/// <summary>
		/// Font Constructor
		/// </summary>
		/// <param name="filename">Font filename</param>
		/// <param name="pointSize">Size of font</param>
		public Font(string filename, int pointSize) 
		{
			if (!FontSystem.IsInitialized)
			{
				FontSystem.Initialize();
			}

			this.Handle = SdlTtf.TTF_OpenFont(filename, pointSize);
			if (this.Handle == IntPtr.Zero) 
			{
				throw FontException.Generate();
			}
		}

		internal Font(IntPtr handle) 
		{
			this.Handle = handle;
		}

		/// <summary>
		/// Destroys the surface object and frees its memory
		/// </summary>
		/// <param name="disposing">If true, it will dispose all obejects</param>
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
					}
					CloseHandle();
					//GC.KeepAlive(this);
					GC.SuppressFinalize(this);
					this.disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		public override bool HasBeenDisposed
//		{
//			get
//			{
//				return this.disposed;
//			}
//		}

		/// <summary>
		/// Closes Surface handle
		/// </summary>
		protected override void CloseHandle() 
		{
			if (this.Handle != IntPtr.Zero)
			{
				SdlTtf.TTF_CloseFont(this.Handle);
				GC.KeepAlive(this);
				this.Handle = IntPtr.Zero;
			}
		}

		// Possibly add Bold/Italic/Underline properties

		/// <summary>
		/// Style Property
		/// </summary>
		public Styles Style 
		{
			set 
			{ 
				SdlTtf.TTF_SetFontStyle(this.Handle, (int) value); 
				GC.KeepAlive(this);
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
				GC.KeepAlive(this);
				return style; 
			}
		}

		/// <summary>
		/// Bold Property
		/// </summary>
		public bool Bold 
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style | (int) Styles.Bold); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style ^ (int) Styles.Bold); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Bold) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Italic Property
		/// </summary>
		public bool Italic
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style | (int) Styles.Italic); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style ^ (int) Styles.Italic); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Italic) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Underline Property
		/// </summary>
		public bool Underline
		{
			set 
			{ 
				if (value == true)
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style | (int) Styles.Underline); 
					GC.KeepAlive(this);
				}
				else
				{
					Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) style ^ (int) Styles.Underline); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
				GC.KeepAlive(this);
				if ((int)(style & Styles.Underline) == (int) SdlFlag.FalseValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Normal Property
		/// </summary>
		public bool Normal
		{
			set 
			{ 
				if (value == true)
				{
					SdlTtf.TTF_SetFontStyle(this.Handle, (int) Styles.None); 
					GC.KeepAlive(this);
				}
			}
			get 
			{ 
				Styles style = (Styles)SdlTtf.TTF_GetFontStyle(this.Handle);
				GC.KeepAlive(this);
				if ((int)(style | Styles.Underline | Styles.Bold | Styles.Italic) == (int) SdlFlag.TrueValue)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Height Property
		/// </summary>
		public int Height 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontHeight(this.Handle);
				GC.KeepAlive(this);
				return result; 
			}
		}

		/// <summary>
		/// Ascent Property
		/// </summary>
		public int Ascent 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontAscent(this.Handle);
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Descent Property
		/// </summary>
		public int Descent
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontDescent(this.Handle);
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Line Size property
		/// </summary>
		public int LineSize 
		{
			get 
			{ 
				int result = SdlTtf.TTF_FontLineSkip(this.Handle); 
				GC.KeepAlive(this);
				return result;
			}
		}

		/// <summary>
		/// Size
		/// </summary>
		/// <param name="textItem">String to display</param>
		/// <returns></returns>
		public Size SizeText(string textItem) 
		{
			int width;
			int height;

			SdlTtf.TTF_SizeUNICODE(this.Handle, textItem, out width, out height);
			GC.KeepAlive(this);
			return new Size(width, height);
		}

		/// <summary>
		/// Render Text to Solid
		/// </summary>
		/// <param name="textItem">String to display</param>
		/// <param name="color">Color of text</param>
		/// <returns>Surface containing the text</returns>
		private Surface RenderTextSolid(string textItem, Color color) 
		{
			Sdl.SDL_Color colorSdl = SdlColor.ConvertColor(color);
			return new Surface(SdlTtf.TTF_RenderUNICODE_Solid(this.Handle, textItem, colorSdl));
		}

		/// <summary>
		/// Shade text
		/// </summary>
		/// <param name="textItem"></param>
		/// <param name="backgroundColor"></param>
		/// <param name="foregroundColor"></param>
		/// <returns></returns>
		private Surface RenderTextShaded(
			string textItem, Color foregroundColor, Color backgroundColor) 
		{
			Sdl.SDL_Color foregroundColorSdl = 
				SdlColor.ConvertColor(foregroundColor);
			Sdl.SDL_Color backgroundColorSdl = 
				SdlColor.ConvertColor(backgroundColor);
			return new Surface(SdlTtf.TTF_RenderUNICODE_Shaded(
				this.Handle, textItem, foregroundColorSdl, backgroundColorSdl));
		}

		/// <summary>
		/// Blended Text
		/// </summary>
		/// <param name="color"></param>
		/// <param name="textItem"></param>
		/// <returns></returns>
		private Surface RenderTextBlended(
			string textItem, Color color) 
		{
			Sdl.SDL_Color colorSdl = SdlColor.ConvertColor(color);
			return new Surface(SdlTtf.TTF_RenderUNICODE_Blended(
				this.Handle, textItem, colorSdl));
		}


		/// <summary>
		/// Render text to a surface.
		/// </summary>
		/// <param name="textItem">String to display</param>
		/// <param name="antiAlias">If true, text will be anti-aliased</param>
		/// <param name="foregroundColor">Color of text</param>
		/// <returns>Surface with text</returns>
		public Surface Render(string textItem, bool antiAlias, Color foregroundColor)
		{
			if (antiAlias)
			{
				return Render(textItem, foregroundColor);
			}
			else
			{
				return RenderTextSolid(textItem, foregroundColor);
			}
		}

		/// <summary>
		/// Render text to a surface with a background color
		/// </summary>
		/// <param name="textItem">String to display</param>
		/// <param name="foregroundColor">Color of text</param>
		/// <param name="backgroundColor">Color of background</param>
		/// <returns>Surface with text</returns>
		public Surface Render(string textItem, Color foregroundColor, Color backgroundColor)
		{
			return RenderTextShaded(textItem, foregroundColor, backgroundColor);
		}

		/// <summary>
		/// Render Text to a surface
		/// </summary>
		/// <param name="textItem">Text string</param>
		/// <param name="foregroundColor">Color of text</param>
		/// <returns>Surface with text</returns>
		public Surface Render(string textItem, Color foregroundColor)
		{
			return RenderTextBlended(textItem, foregroundColor);
		}
	}
}
