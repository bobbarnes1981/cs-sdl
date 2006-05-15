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
//	[StructLayout(LayoutKind.Sequential, Pack=4)]
//	public struct vertex 
//	{ 
//		float u, v, x, y, z; 
//		char r, g, b, a; 
//	}
	public sealed class RenderCubes
	{
		public static int curvert;
		public static int curmaxverts = 10000;
		public static int nquads;
		public const float TEXTURESCALE = 32.0f;
		public static bool floorstrip = false;
		public static bool deltastrip = false;
		public static int oh;
		public static int oy;
		public static int ox;
		public static int ogltex;                         // the o* vars are used by the stripification
		public static int ol3r;
		public static int ol3g;
		public static int ol3b;
		public static int ol4r;
		public static int ol4g;
		public static int ol4b;      
		public static int firstindex;
		public static bool showm = false;
		public static int wx1;
		public static int wy1;
		public static int wx2;
		public static int wy2;

		public static float dx(float x) 
		{ 
			return x+(float)Math.Sin(x*2+GameInit.LastMillis/1000.0f)*0.04f; 
		}
		public static float dy(float x) 
		{ 
			return x+(float)Math.Sin(x*2+GameInit.LastMillis/900.0f+Math.PI/5)*0.05f; 
		}

		public static void AddWaterQuad(int x, int y, int size)       // update bounding rect that contains water
		{
			int x2 = x+size;
			int y2 = y+size;
			if(RenderCubes.wx1<0)
			{
				RenderCubes.wx1 = x;
				RenderCubes.wy1 = y;
				RenderCubes.wx2 = x2;
				RenderCubes.wy2 = y2;
			}
			else
			{
				if(x<RenderCubes.wx1) RenderCubes.wx1 = x;
				if(y<RenderCubes.wy1) RenderCubes.wy1 = y;
				if(x2>RenderCubes.wx2) RenderCubes.wx2 = x2;
				if(y2>RenderCubes.wy2) RenderCubes.wy2 = y2;
			};
		}

		/// <summary>
		/// 
		/// </summary>
		public static void showmip() 
		{ 
			showm = !showm; 
		}
		/// <summary>
		/// TODO
		/// </summary>
		public static void ResetCubes()
		{
			//if(!verts) reallocv();
			RenderCubes.floorstrip = false;
			RenderCubes.deltastrip = false;
			RenderCubes.wx1 = -1;
			RenderCubes.nquads = 0;
			//sbright.r = sbright.g = sbright.b = 255;
			//sdark.r = sdark.g = sdark.b = 0;
		}

	}
}
