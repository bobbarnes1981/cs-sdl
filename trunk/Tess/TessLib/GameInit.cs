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


namespace TessLib
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
		char[] head;               // "CUBE"
		int version;                // any >8bit quantity is a little indian
		int headersize;             // sizeof(header)
		int sfactor;                // in bits
		int numents;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=128 )]
		char[] maptitle;
		//uchar texlists[3][256];
		IntPtr texlists;
		int waterlevel;
		[MarshalAs( UnmanagedType.ByValArray, SizeConst=15 )]
		int[] reserved;
	};

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct block 
	{ 
		int x, y, xs, ys; 
	};

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct mapmodelinfo 
	{ 
		int rad, h, zoff, snap; 
		string name; 
	};


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
	public class GameInit
	{
		
		static IntPtr player1Ptr;
		static DynamicEntity player1;
		static int screenWidth = 640;
		static int screenHeight = 480;
		

		public static int FontH
		{
			get
			{
				return FONTH;
			}
		}
		static int FONTH = 64;

		public GameInit()
		{
			//
			// TODO: Add constructor logic here
			//
		}

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
		public static void Log(string input)
		{
			Console.WriteLine("init: {0}", input);
		}

		public static void Fatal(string s, string o)
		{
			Console.WriteLine(s + o + Sdl.SDL_GetError());
			Cleanup(s + o + Sdl.SDL_GetError());
		}
		public static void Fatal(string s)
		{
			Fatal(s, "");
		}
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
		public static void Cleanup(string msg)
		{	
			Bindings.stop();
			Bindings.disconnect(true, false);
			Bindings.writecfg();
			RenderGl.CleanGl();
			Sound.CleanSound();
			Bindings.cleanupserver();
			Mouse.ShowCursor = true;
			Video.GrabInput = false;
			if(msg != null)
			{				
				Log(msg);	
			}
			Events.QuitApplication();
		}
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
