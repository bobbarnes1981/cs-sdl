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
 * All C# code Copyright (C) 2006 David Y. Hudson
 * Mezzanine is a .NET port of Cube (version released on 2005-Aug-29).
 * Cube was written by Wouter van Oortmerssen (http://cubeengine.com)
 */
#endregion License

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MezzanineLib.Render
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class RenderMD2
	{
		public const int FIRSTMDL = 20;
		public static int modelnum = 0;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		public static float Snap(int sn, float f) 
		{ 
			return (sn!=0 ? (float)(((int)(f+sn*0.5f))&(~(sn-1))) : f); 
		}
	}
}
