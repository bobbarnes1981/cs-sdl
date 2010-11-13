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
using Tao.Sdl;
using Tao.OpenGl;
using System.Runtime.InteropServices;

namespace MezzanineLib.Game
{
	/// <summary>
	/// 
	/// </summary>
	public struct ItemStat 
	{ 
		/// <summary>
		/// 
		/// </summary>
		public int add;
		/// <summary>
		/// 
		/// </summary>
		public int max;
		/// <summary>
		/// 
		/// </summary>
		public int sound; 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="add"></param>
		/// <param name="max"></param>
		/// <param name="sound"></param>
		public ItemStat(int add, int max, int sound)
		{
			this.add = add;
			this.max = max;
			this.sound = sound;
		}
	} 
	/// <summary>
	/// 
	/// </summary>
	public sealed class Entities
	{
		public static int triggertime = 0;

		public static string[] EntityModelNames = new string[10];

		public static ItemStat[] itemstats = new ItemStat[9];

		
//		itemstats[] =
//	{
//		10,    50, Sounds::S_ITEMAMMO,
//		20,   100, Sounds::S_ITEMAMMO,
//		5,    25, Sounds::S_ITEMAMMO,
//		5,    25, Sounds::S_ITEMAMMO,
//		25,   100, Sounds::S_ITEMHEALTH,
//		50,   200, Sounds::S_ITEMHEALTH,
//		100,   100, Sounds::S_ITEMARMOUR,
//		150,   150, Sounds::S_ITEMARMOUR,
//		20000, 30000, Sounds::S_ITEMPUP,
//	};
		 
		static Entities()
		{
			EntityModelNames[0] = "shells";
			EntityModelNames[1] = "bullets";
			EntityModelNames[2] = "rockets";
			EntityModelNames[3] = "rrounds";
			EntityModelNames[4] = "health";
			EntityModelNames[5] = "boost";
			EntityModelNames[6] = "g_armour";
			EntityModelNames[7] = "y_armour";
			EntityModelNames[8] = "quad";
			EntityModelNames[9] = "teleporter";
			itemstats[0] = new ItemStat(10,50, (int)Sounds.S_ITEMAMMO);
			itemstats[1] = new ItemStat(20,100, (int)Sounds.S_ITEMAMMO);
			itemstats[2] = new ItemStat(5,25, (int)Sounds.S_ITEMAMMO);
			itemstats[3] = new ItemStat(5,25, (int)Sounds.S_ITEMAMMO);
			itemstats[4] = new ItemStat(25,100, (int)Sounds.S_ITEMHEALTH);
			itemstats[5] = new ItemStat(50,200, (int)Sounds.S_ITEMHEALTH);
			itemstats[6] = new ItemStat(100,100, (int)Sounds.S_ITEMARMOUR);
			itemstats[7] = new ItemStat(150,150, (int)Sounds.S_ITEMARMOUR);
			itemstats[8] = new ItemStat(20000,30000, (int)Sounds.S_ITEMPUP);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gun"></param>
		public static void BaseAmmo(int gun) 
		{ 
			GameInit.Player1.ammo[gun] = itemstats[gun-1].add*2; 
		}
		}
}
