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

namespace MezzanineLib.ClientServer
{	
	/// <summary>
	/// 
	/// </summary>
	public enum ServerType
	{ 
		ST_EMPTY, 
		ST_LOCAL, 
		ST_TCPIP 
	};

	public sealed class Server
	{
		public static int maxclients = 8;
		public static bool notgotitems = true;        // true when map has changed and waiting for clients to send item
		public static int mode = 0;

		public static int interm = 0;
		public static int minremain = 0;
		public static int mapend = 0;
		public static bool mapreload = false;
		public static bool isdedicated;
		public static int bsend = 0;
		public static int brec = 0;
		public static int laststatus = 0;
		public static int lastsec = 0;
		public const int MAXOBUF = 100000;
		public static int nonlocalclients = 0;
		public static int lastconnect = 0;
	}
}