/*
 * $RCSfile$
 * Copyright (C) 2003 Lucas Maloney
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

using System.Runtime.InteropServices;

namespace SDLDotNet
{
	/* This struct really should be part of the base SDLDotNet lib
		or possibly replaced with System.Drawing.Color */

	/// <summary>
	/// SDL Color Class
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLColor {
		/// <summary>
		/// Red Channel
		/// </summary>
		private byte redChannel;
		/// <summary>
		/// Green Channel
		/// </summary>
		private byte greenChannel;
		/// <summary>
		/// Blue Channel
		/// </summary>
		private byte blueChannel;
		/// <summary>
		/// Alpha Channel
		/// Currently unused
		/// </summary>
		private byte alphaChannel;

		/// <summary>
		/// Constructor for SDL Color
		/// </summary>
		/// <param name="R"></param>
		/// <param name="G"></param>
		/// <param name="B"></param>
		public SDLColor(byte R, byte G, byte B)
		{
			redChannel = R;
			greenChannel = G;
			blueChannel = B;
			alphaChannel = 0;
		}

		/// <summary>
		/// Property for red channel
		/// </summary>
		public byte R
		{
			get { return redChannel; }
			set { redChannel = value; }
		}

		/// <summary>
		/// Property for green channel
		/// </summary>
		public byte G
		{
			get { return greenChannel; }
			set { greenChannel = value; }
		}

		/// <summary>
		/// Property for blue channel
		/// </summary>
		public byte B
		{
			get { return blueChannel; }
			set { blueChannel = value; }
		}

		/// <summary>
		/// Get Red Instance
		/// </summary>
		static public SDLColor Red
		{
			get { return new SDLColor(255,0,0); }
		}

		/// <summary>
		/// Get Green Instance
		/// </summary>
		static public SDLColor Green
		{
			get { return new SDLColor(0,255,0); }
		}

		/// <summary>
		/// Get Blue Instance
		/// </summary>
		static public SDLColor Blue
		{
			get { return new SDLColor(0,0,255); }
		}

		/// <summary>
		/// Get MediumPurple instance
		/// </summary>
		static public SDLColor MediumPurple
		{	
			get { return new SDLColor(147, 112, 219); }
		}

		/// <summary>
		/// Get orange instance
		/// </summary>
		static public SDLColor Orange
		{	
			get { return new SDLColor(255, 165, 0); }
		}

		/* TODO: A few more colours. xfree colors and/or .NET colors */

	}
}
