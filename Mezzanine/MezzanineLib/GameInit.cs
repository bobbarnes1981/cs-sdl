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
using MezzanineLib.ClientServer;
using MezzanineLib.Render;
using MezzanineLib.Support;

namespace MezzanineLib
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct Vector
	{ 
		public float x;
		public float y;
		public float z; 
	}

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct Square
	{
		char type;                 // one of the BlockTypes
		char floor, ceil;           // height, in cubes
		char wtex, ftex, ctex;     // wall/floor/ceil texture ids
		char r, g, b;              // light value at upper left vertex
		char vdelta;               // vertex delta, used for heightfield cubes
		char defer;                 // used in mipmapping, when true this cube is not a perfect mip
		char occluded;              // true when occluded
		char utex;                 // upper wall tex id
		char tag;                  // used by triggers
	};

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct MapHeader                   // map file format header
	{
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=4 )]
		public char[] head;               // "CUBE"
		public int version;                // any >8bit quantity is a little indian
		public int headersize;             // sizeof(header)
		public int sfactor;                // in bits
		public int numents;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=128 )]
		public char[] maptitle;
		//uchar texlists[3][256];
		public IntPtr texlists;
		public int waterlevel;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=15 )]
		public int[] reserved;
	};

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct Block 
	{ 
		public int x, y, xs, ys; 
	};

//	[StructLayout(LayoutKind.Sequential, Pack=4)]
//	public struct mapmodelinfo 
//	{ 
//		int rad, h, zoff, snap; 
//		string name; 
//	};


	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct PersistentEntity        // map entity
	{
		short x, y, z;              // cube aligned position
		short attr1;
		char type;                 // type is one of the above
		char attr2, attr3, attr4;        
	};

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct Entity        // map entity
	{
		short x, y, z;              // cube aligned position
		short attr1;
		char type;                 // type is one of the above
		char attr2, attr3, attr4;
		bool spawned;               // the only dynamic state of a map entity
	};

	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public sealed class GameInit
	{
		
		static IntPtr player1Ptr;
		static IntPtr hdrPtr;
		static MapHeader hdr;
		static DynamicEntity player1;
		static int screenWidth = 640;
		static int screenHeight = 480;
		static int lastmillis = 0;	
		static int mapVersion = 5;
		//static string path;
		public static readonly char PATHDIV = '\\';
		/// <summary>
		/// 
		/// </summary>
		public static string NormalizePath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			return path.Replace('/', PATHDIV);
		}

		/// <summary>
		/// bump if map format changes, see worldio.cpp
		/// </summary>
		public static int MapVersion
		{
			get
			{
				return mapVersion;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public static int LastMillis
		{
			get
			{
				return lastmillis;
			}
			set
			{
				lastmillis = value;
			}
		}

		static bool editmode;

		/// <summary>
		/// 
		/// </summary>
		public static bool EditMode
		{
			get
			{
				return editmode;
			}
			set
			{
				editmode = value;
			}
		}

		static int cubicsize;
		/// <summary>
		/// 
		/// </summary>
		public static int CubicSize
		{
			get
			{
				return cubicsize;
			}
			set
			{
				cubicsize = value;
			}
		}
		static int mipsize;          // cubicsize = ssize^2
		/// <summary>
		/// 
		/// </summary>
		public static int MipSize
		{
			get
			{
				return mipsize;
			}
			set
			{
				mipsize = value;
			}
		}

		static bool demoplayback;
		/// <summary>
		/// 
		/// </summary>
		public static bool DemoPlayback
		{
			get
			{
				return demoplayback;
			}
			set
			{
				demoplayback = value;
			}
		}



		public const int MinBord = 2; // 2 cubes from the edge of the world are always solid
		public const int SAVEGAMEVERSION = 4;              // bump if dynent/netprotocol changes or any other savegame/demo data
		public const int MAXCLIENTS = 256;           // in a multiplayer game, can be arbitrarily changed
		public const int MAXTRANS = 5000;          // max amount of data to swallow in 1 go
		public const int CUBE_SERVER_PORT = 28765;
		public const int CUBE_SERVINFO_PORT = 28766;
		public const int PROTOCOL_VERSION = 122;       // bump when protocol changes
		public const float DMF = 16.0f ;
		public const float DAF = 1.0f ;
		public const float DVF = 100.0f;

		public const int VIRTW = 2400;                      // virtual screen size for text & HUD
		public const int VIRTH = 1800;
		public const int PIXELTAB = (VIRTW/12);
		static int sfactor;
		static int ssize;              // ssize = 2^sfactor

		/// <summary>
		/// 
		/// </summary>
		public static int SFactor
		{
			get
			{
				return sfactor;
			}
			set
			{
				sfactor = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static int SSize
		{
			get
			{
				return ssize;
			}
			set
			{
				ssize = value;
			}
		}

		static int gamemode;
		/// <summary>
		/// 
		/// </summary>
		public static int GameMode
		{
			get
			{
				return gamemode;
			}
			set
			{
				gamemode = value;
			}
		}
		static int nextmode;
		/// <summary>
		/// 
		/// </summary>
		public static int NextMode
		{
			get
			{
				return nextmode;
			}
			set
			{
				nextmode = value;
			}
		}



		static int curtime;

		/// <summary>
		/// 
		/// </summary>
		public static int CurrentTime
		{
			get
			{
				return curtime;
			}
			set
			{
				curtime = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static int FontH
		{
			get
			{
				return FONTH;
			}
		}
		static int FONTH = 64;

		/// <summary>
		/// determines number of mips there can be
		/// </summary>
		public static int SmallestFactor
		{
			get
			{
				return SMALLEST_FACTOR;
			}
		}
		static int SMALLEST_FACTOR = 6;

		/// <summary>
		/// 10 is already insane
		/// </summary>
		public static int LargestFactor
		{
			get
			{
				return LARGEST_FACTOR;
			}
		}
		static int LARGEST_FACTOR = 11;

		/// <summary>
		/// 
		/// </summary>
		public static int DefaultFactor
		{
			get
			{
				return DEFAULT_FACTOR;
			}
		}
		static int DEFAULT_FACTOR = 8;

		/// <summary>
		/// 
		/// </summary>
		public GameInit()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// 
		/// </summary>
		public static int ScreenWidth
		{
			get
			{
				return screenWidth;
			}
			set
			{
				screenWidth = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static int ScreenHeight
		{
			get
			{
				return screenHeight;
			}
			set
			{
				screenHeight = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		public static void Log(string input)
		{
			System.Console.WriteLine("init: {0}", input);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="o"></param>
		public static void Fatal(string s, string o)
		{
			System.Console.WriteLine(s + o + Sdl.SDL_GetError());
			Cleanup(s + o + Sdl.SDL_GetError());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public static void Fatal(string s)
		{
			Fatal(s, "");
		}

		/// <summary>
		/// 
		/// </summary>
		public static DynamicEntity Player1
		{
			get
			{
				return player1 = (DynamicEntity)Marshal.PtrToStructure(player1Ptr, typeof(DynamicEntity));
			}
			set
			{
				Marshal.StructureToPtr(value, player1Ptr, false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static MapHeader MapHeader
		{
			get
			{
				return hdr = (MapHeader)Marshal.PtrToStructure(hdrPtr, typeof(MapHeader));
			}
			set
			{
				Marshal.StructureToPtr(value, hdrPtr, false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static IntPtr Player1Ptr
		{
			get
			{
				return player1Ptr;
			}
			set
			{
				player1Ptr = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="msg"></param>
		public static void Cleanup(string msg)
		{	
			Bindings.stop();
			Bindings.disconnect(true, false);
			Bindings.writecfg();
			RenderGl.CleanGl();
			MezzanineLib.Support.Sound.CleanSound();
			Bindings.cleanupserver();
			Mouse.ShowCursor = true;
			Video.GrabInput = false;
			if(msg != null)
			{				
				Log(msg);	
			}
			Events.QuitApplication();
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Quit()
		{
			Bindings.writeservercfg();
			GameInit.Cleanup(null);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Screenshot()
		{
			//Note that the openGL surface is flip upside down and the bits are reversed. These lines correct that.
			Surface temp = new Surface(ScreenWidth, ScreenHeight, 24, 0x0000FF, 0x00FF00, 0xFF0000, 0);
			Gl.glReadPixels(0, 0, ScreenWidth, ScreenHeight, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, temp.Pixels);
			temp.FlipVertical();
			temp.SaveBmp("screenshots/screenshot_"+Timer.TicksElapsed.ToString()+".bmp");					
			temp.Dispose();
		}
	}
}
