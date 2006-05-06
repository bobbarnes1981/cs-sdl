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

namespace TessLib.Support
{
	/// <summary>
	/// 
	/// </summary>
	public enum IDCommands
	{ 
		ID_VAR, 
		ID_COMMAND, 
		ID_ALIAS 
	}

	/// <summary>
	/// function signatures for script functions, see command.cpp
	/// </summary>
	public enum FunctionSignatures 
	{
		ARG_1INT, ARG_2INT, ARG_3INT, ARG_4INT,
		ARG_NONE,
		ARG_1STR, ARG_2STR, ARG_3STR, ARG_5STR,
		ARG_DOWN, ARG_DWN1,
		ARG_1EXP, ARG_2EXP,
		ARG_1EST, ARG_2EST,
		ARG_VARI
	}
}