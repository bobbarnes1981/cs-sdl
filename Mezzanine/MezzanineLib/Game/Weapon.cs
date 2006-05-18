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
using MezzanineLib.ClientServer;

namespace MezzanineLib.Game
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct GunInfo 
	{ 
		public short sound;
		public short attackdelay;
		public short damage;
		public short projspeed;
		public short part;
		public short kickamount; 
		public string name; 

		public GunInfo(short sound, short attackdelay, short damage, short projspeed, short part, short kickamount, string name)
		{
			this.sound = sound;
			this.attackdelay = attackdelay;
			this.damage = damage;
			this.projspeed = projspeed;
			this.part = part;
			this.kickamount = kickamount;
			this.name = name;
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class Weapon
	{
		public const int MONSTERDAMAGEFACTOR = 4;
		public const int SGRAYS = 20;
		public const float SGSPREAD = 2;
		public const int MAXPROJ = 100;
		public const float RL_RADIUS = 5;
		public const float RL_DAMRAD = 7;   // hack

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="c"></param>
		public static void SelectGun(int a, int b, int c)
		{
			if(a<-1 || b<-1 || c<-1 || a>=(int)Gun.NUMGUNS || b>=(int)Gun.NUMGUNS || c>=(int)Gun.NUMGUNS)
			{
				return;
			}
			int s = GameInit.Player1.gunselect;
			if(a>=0 && s!=a && GameInit.Player1.ammo[a]!=0) 
			{
				s = a;
			}
			else if(b>=0 && s!=b && GameInit.Player1.ammo[b]!=0) 
			{
				s = b;
			}
			else if(c>=0 && s!=c && GameInit.Player1.ammo[c]!=0) 
			{
				s = c;
			}
			else if(s!=(int)Gun.GUN_RL && GameInit.Player1.ammo[(int)Gun.GUN_RL]!=0) 
			{
				s = (int)Gun.GUN_RL;
			}
			else if(s!=(int)Gun.GUN_CG && GameInit.Player1.ammo[(int)Gun.GUN_CG]!=0) 
			{
				s = (int)Gun.GUN_CG;
			}
			else if(s!=(int)Gun.GUN_SG && GameInit.Player1.ammo[(int)Gun.GUN_SG]!=0) 
			{
				s = (int)Gun.GUN_SG;
			}
			else if(s!=(int)Gun.GUN_RIFLE && GameInit.Player1.ammo[(int)Gun.GUN_RIFLE]!=0) 
			{
				s = (int)Gun.GUN_RIFLE;
			}
			else 
			{
				s = (int)Gun.GUN_FIST;
			}
			if(s!=GameInit.Player1.gunselect) 
			{
				Bindings.playsoundc((int)Sounds.S_WEAPLOAD);
			}
			DynamicEntity tempEntity = GameInit.Player1;
			tempEntity.gunselect = s;
			GameInit.Player1 = tempEntity;
			//conoutf("%s selected", (int)guns[s].name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="a1"></param>
		/// <param name="a2"></param>
		/// <param name="a3"></param>
		public static void WeaponSelect(string a1, string a2, string a3)
		{
			SelectGun(a1 != null ? Convert.ToInt32(a1) : -1, 
				a2 != null ? Convert.ToInt32(a2) : -1,
				a3 != null ? Convert.ToInt32(a3) : -1
				);
		}
	}
}
