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
	public class Main
	{
		
		static IntPtr player1Ptr;
		static DynamicEntity player1;

		public Main()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static void Log(string input)
		{
			Console.WriteLine("init: {0}", input);
		}

		public static void Fatal(string s, string o)
		{
			Console.WriteLine(s + o + Sdl.SDL_GetError());
			Bindings.cleanup(s + o + Sdl.SDL_GetError());
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
	}
}
