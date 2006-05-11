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
using System.Runtime.InteropServices;

namespace MezzanineLib.ClientServer
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct DynamicEntity                           // players & monsters
	{
		public Vector o;
		public Vector vel;                         // origin, velocity
		public float yaw, pitch, roll;             // used as vec in one place
		public float maxspeed;                     // cubes per second, 24 for player
		public bool outsidemap;                    // from his eyes
		public bool inwater;
		public bool onfloor, jumpnext;
		public int move, strafe;
		public bool k_left, k_right, k_up, k_down; // see input code  
		public int timeinair;                      // used for fake gravity
		public float radius, eyeheight, aboveeye;  // bounding box size
		public int lastupdate, plag, ping;
		public int lifesequence;                   // sequence id for each respawn, used in damage test
		public int state;                          // one of CS_* below
		public int frags;
		public int health, armour, armourtype, quadmillis;
		public int gunselect, gunwait;
		public int lastaction, lastattackgun, lastmove;
		public bool attacking;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=9 )]
		public int[] ammo;
		public int monsterstate;                   // one of M_* below, M_NONE means human
		public int mtype;                          // see monster.cpp
		public IntPtr enemy;                      // monster wants to kill this entity
		public float targetyaw;                    // monster wants to look in this direction
		public bool blocked;
		public bool moving;							// used by physics to signal ai
		public int trigger;                        // millis at which transition to another monsterstate takes place
		public Vector attacktarget;                   // delayed attacks
		public int anger;                          // how many times already hit by fellow monster
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=260 )]
		public char[] name;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=260 )]
		public char[] team;
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class ClientGame
	{
		public static bool intermission = false;
		static int framesInMap;
		/// <summary>
		/// 
		/// </summary>
		public static int FramesInMap
		{
			get
			{
				return framesInMap;
			}
			set
			{
				framesInMap = value;
			}
		}

//		static string clientMap;
//
//		/// <summary>
//		/// 
//		/// </summary>
//		public static string ClientMap 
//		{
//			get
//			{
//				return clientMap; 
//			}
//			set
//			{
//				clientMap = value;
//			}
//		}

		
		public static int arenarespawnwait = 0;
		public static int arenadetectwait  = 0;
		public static int spawncycle = -1;
		public static int fixspawn = 2;
		public static int sleepwait = 0;

		
		static int demoClientNum;

		/// <summary>
		/// 
		/// </summary>
		public static int DemoClientNum
		{
			get
			{
				return demoClientNum;
			}
			set
			{
				demoClientNum = value;
			}
		}

		public static void initclient()
		{
			//Bindings.getclientmap()[0] = '0';
			Bindings.initclientnet();
		}
	}
}
