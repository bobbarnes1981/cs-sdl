/*
 * $RCSfile$
 * Copyright (C) 2003 Lucas Maloney
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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

/*
 * $Revision$
 * $Date$
 *
 *	REVISION HISTORY
 *
 *	Mon 31 Mar 2003 23:28:02 EST LM
 *	Changed namespace from SdlTtfDotNet
 *	Now using singleton architecture
 *
 *	Tue 25 Mar 2003 17:50:09 EST LM
 *	Added error check to Ttf_Init call.  It will return -1 if there was a problem.
 *
 *	Mon 24 Mar 2003 20:45:40 EST LM
 *	There is currently a bug in mono which meant this class did not need an instance of Sdl.
 *	I have fixed this so it does not depend on that bug.
 */

using System;
using System.Runtime.InteropServices;
using SdlDotNet;
using Tao.Sdl;

namespace SdlDotNet
{
	/// <summary>
	/// Text Style
	/// </summary>
	[FlagsAttribute]
	public enum Styles
	{
		/// <summary>
		/// Normal
		/// </summary>
		Normal = SdlTtf.TTF_STYLE_NORMAL,
		/// <summary>
		/// Bold
		/// </summary>
		Bold = SdlTtf.TTF_STYLE_BOLD,
		/// <summary>
		/// Italic
		/// </summary>
		Italic = SdlTtf.TTF_STYLE_ITALIC,
		/// <summary>
		/// Underline
		/// </summary>
		Underline = SdlTtf.TTF_STYLE_UNDERLINE
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	public sealed class Ttf
	{
		static readonly Ttf instance = new Ttf();

		Ttf()
		{
		}

		/// <summary>
		/// Singleton get instance method
		/// </summary>
		public static Ttf Instance 
		{
			get 
			{
				if (SdlTtf.TTF_Init() != (int) SdlFlag.Success)
				{
					TtfException.Generate();
				}
				return instance;
			}
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~Ttf() 
		{
			SdlTtf.TTF_Quit();
		}
	}
}
