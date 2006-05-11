#region License
/*
 * Copyright (C) 2001-2005 Wouter van Oortmerssen.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software
 * in a product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 * 
 * additional clause specific to Cube:
 * 
 * 4. Source versions may not be "relicensed" under a different license
 * without my explicitly written permission.
 *
 */

/* 
 * All code Copyright (C) 2006 David Y. Hudson
 */
#endregion License

using System;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using SdlDotNet;
using System.Runtime.InteropServices;

namespace MezzanineLib.World
{
	/// <summary>
	/// Summary description for WorldOcull.
	/// </summary>
	public sealed class WorldOcull
	{
		static WorldOcull()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public const int NumRays = 512;
//		/// <summary>
//		/// 
//		/// </summary>
//		public static int NumRays
//		{
//			get
//			{
//				return numRays;
//			}
//		}

		static bool ocull = true;
		/// <summary>
		/// 
		/// </summary>
		public static bool Ocull
		{
			get
			{
				return ocull;
			}
			set
			{
				ocull = value;
			}
		}

		static float oDist = 256;
		/// <summary>
		/// 
		/// </summary>
		public static float ODist
		{
			get
			{
				return oDist;
			}
			set
			{
				oDist = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ToggleOcull() 
		{ 
			ocull = !ocull; 
		}
	}
}
