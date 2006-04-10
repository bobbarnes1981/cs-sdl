using System;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using SdlDotNet;
using System.Runtime.InteropServices;


namespace TessLib
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct vertex 
	{ 
		float u, v, x, y, z; 
		char r, g, b, a; 
	}
}
