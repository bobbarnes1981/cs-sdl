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

/*
 * $Revision$
 * $Date$
 *
 *	REVISION HISTORY
 *
 *	Mon 31 Mar 2003 23:28:02 EST LM
 *	Changed namespace from SDLTTFDotNet
 *	Now using singleton architecture
 *
 *	Tue 25 Mar 2003 17:50:09 EST LM
 *	Added error check to TTF_Init call.  It will return -1 if there was a problem.
 *
 *	Mon 24 Mar 2003 20:45:40 EST LM
 *	There is currently a bug in mono which meant this class did not need an instance of SDL.
 *	I have fixed this so it does not depend on that bug.
 */

using System;
using System.Runtime.InteropServices;
using SDLDotNet;
//[assembly: CLSCompliantAttribute(true)]
namespace SDLDotNet.TTF
{
	/// <summary>
	/// Text Style
	/// </summary>
	public enum Style {
		/// <summary>
		/// Normal
		/// </summary>
		Normal = 0x00,
		/// <summary>
		/// Bold
		/// </summary>
		Bold = 0x01,
		/// <summary>
		/// Italic
		/// </summary>
		Italic = 0x02,
		/// <summary>
		/// Underline
		/// </summary>
		Underline = 0x04
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	public class SDLTTF
	{
		const string TTF_DLL = "SDL_ttf";
		static private SDLTTF mInstance;

		[DllImport(TTF_DLL)]
		private static extern int TTF_Init();

		[DllImport(TTF_DLL)]
		private static extern void TTF_Quit();

		/// <summary>
		/// Singleton get instance method
		/// </summary>
		public static SDLTTF Instance {
			get {
				if (mInstance == null) mInstance = new SDLTTF();
				return mInstance;
			}
		}

		private SDLTTF() {
			if (TTF_Init() != 0)
				SDLTTFException.Generate();
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~SDLTTF() {
			TTF_Quit();
		}
	}
}
