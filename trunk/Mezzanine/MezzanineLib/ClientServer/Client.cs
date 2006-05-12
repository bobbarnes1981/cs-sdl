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
 * Mezzanine is a .NET port of Cube (http://cubeengine.com).
 * Cube is written by Wouter van Oortmerssen
 */
#endregion License

using System;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using System.Runtime.InteropServices;

namespace MezzanineLib.ClientServer
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Client
	{
		public static int connecting = 0;
		public static int connattempts = 0;
		public static int disconnecting = 0;
		static int clientnum = -1;         // our client id in the game
		public static bool c2sinit = false;       // whether we need to tell the other clients our stats

		public static int ClientNum 
		{ 
			get
			{
				return clientnum; 
			}
			set
			{
				clientnum = value;
			}
		}

		public static int lastupdate = 0;
		public static int lastping = 0;
		static bool senditemstoserver = false;     // after a map change, since server doesn't have map data

		public static bool SendItemsToServer 
		{ 
			get
			{
				return senditemstoserver; 
			}
			set
			{
				senditemstoserver = value;
			}
		}
	}
}
