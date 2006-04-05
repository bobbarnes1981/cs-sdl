using System;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using System.Runtime.InteropServices;

namespace Tess
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
}
