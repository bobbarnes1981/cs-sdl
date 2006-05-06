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
 */

/* 
 * All C# code Copyright (C) 2006 David Y. Hudson
 * Mezzanine is a .NET port of Sauerbraten (http://sauerbraten.org).
 * Sauerbraten is written by Wouter van Oortmerssen
 */
#endregion License

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace MezzanineLib
{
	#region Class Documentation
	#endregion Class Documentation
	/// <summary>
	/// 
	/// </summary>
	[SuppressUnmanagedCodeSecurityAttribute()]
	public sealed class Bindings
	{
		#region Private Constants
		#region string MEZZANINE_NATIVE_LIBRARY
		/// <summary>
		///     Specifies Tess's native library archive.
		/// </summary>
		/// <remarks>
		///     Specifies TessLib.dll everywhere; will be mapped via .config for mono.
		/// </remarks>
		private const string MEZZANINE_NATIVE_LIBRARY = "MezzanineManagedLib.dll";
		#endregion string MEZZANINE_NATIVE_LIBRARY

		#region CallingConvention CALLING_CONVENTION
		/// <summary>
		///     Specifies the calling convention.
		/// </summary>
		/// <remarks>
		///     Specifies <see cref="CallingConvention.Cdecl" /> 
		///     for Windows and Linux.
		/// </remarks>
		private const CallingConvention CALLING_CONVENTION = 
			CallingConvention.Cdecl;
		#endregion CallingConvention CALLING_CONVENTION

		#endregion Private Constants

		#region Private Methods
		#endregion Private Methods

		#region Public Constants
		#endregion Public Constants

		#region Public Enums

		#endregion Public Enums

		#region Public Structs

		#endregion Public Structs

		#region Private Static Fields

		#endregion Private Static Fields

		#region Constructors & Destructors
		#region Bindings()
		/// <summary>
		///     Prevents instantiation.
		/// </summary>
		private Bindings() 
		{
		}
		#endregion Bindings()
		#endregion Constructors & Destructors

		#region Public Delegates
		#endregion Public Delegates

		#region Bindings Methods

//		#region bool installtex(int tnum, string texname, int xs, int ys, bool clamp)
//		/// <summary>
//		///     install textures
//		/// </summary>
//		/// <remarks>
//		/// <p>Binds to C-function call in rendergl.cpp:
//		///     <code>extern DECLSPEC bool CDECL installtex(int tnum, char *texname, int &xs, int &ys, bool clamp = false);</code>
//		///     </p>
//		/// </remarks>
//		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
//		public static extern bool installtex(int tnum, string texname, out int xs, out int ys, bool clamp);
//		#endregion bool installtex(int tnum, string texname, int xs, int ys, bool clamp)

		#region int enet_initialize()
		/// <summary>
		///     initialize enet network
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in enet.h:
		///     <code>ENET_API DECLSPEC int CDECL enet_initialize (void);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern int enet_initialize();
		#endregion int enet_initialize()

		#region void stop()
		/// <summary>
		///     stop
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in main.cpp:
		///     <code>extern DECLSPEC void CDECL stop();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void stop();
		#endregion void stop()

		#region int native_main(int argc, string[] argv)
		/// <summary>
		///     main
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in main.cpp:
		///     <code>extern DECLSPEC void CDECL stop();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern int native_main(int argc, string[] argv);
		#endregion int native_main(int argc, string[] argv)

		#region void computeraytable(float vx, float vy)
		/// <summary>
		///     computerraytable
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in worldocull.cpp:
		///     <code>extern DECLSPEC void CDECL computeraytable(float vx, float vy);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void computeraytable(float vx, float vy);
		#endregion void computeraytable(float vx, float vy)

		#region void readdepth(int w, int h)
		/// <summary>
		///     readdepth
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in renderextras.cpp:
		///     <code>extern DECLSPEC void CDECL readdepth(int w, int h);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void readdepth(int w, int h);
		#endregion void readdepth(int w, int h)

		#region void purgetextures()
		/// <summary>
		///     gl_init
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in rendergl.cpp:
		///     <code>extern DECLSPEC void CDECL purgetextures();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void purgetextures();
		#endregion void purgetextures()

		#region void writecfg()
		/// <summary>
		///     writecfg
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in command.cpp:
		///     <code>extern DECLSPEC void CDECL writecfg()</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void writecfg();
		#endregion void writecfg()

		#region void cleanupserver()
		/// <summary>
		///     cleanupserver
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in command.cpp:
		///     <code>extern DECLSPEC void CDECL cleanupserver()</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void cleanupserver();
		#endregion void cleanupserver()

		#region void gl_drawframe(int w, int h, float curfps)
		/// <summary>
		///     gl_drawframe
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in rendergl.cpp:
		///     <code>extern DECLSPEC void CDECL gl_drawframe(int w, int h, float curfps);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void gl_drawframe(int w, int h, float curfps);
		#endregion void gl_drawframe(int w, int h, float curfps)

		#region void empty_world(int factor, bool force)
		/// <summary>
		///     Empty world
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in world.cpp:
		///     <code>extern DECLSPEC void CDECL empty_world(int factor, bool force);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void empty_world(int factor, bool force);
		#endregion void empty_world(int factor, bool force)

		#region void disconnect(bool onlyclean, bool async)
		/// <summary>
		///     Empty world
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in client.cpp:
		///     <code>extern DECLSPEC void CDECL disconnect(int onlyclean = 0, int async = 0)</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void disconnect(bool onlyclean, bool async);
		#endregion void disconnect(bool onlyclean, bool async)

		#region void serverslice(int seconds, int timeout)
		/// <summary>
		///     server slice
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in server.cpp:
		///     <code>extern DECLSPEC void CDECL serverslice(int seconds, unsigned int timeout);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void serverslice(int seconds, int timeout);
		#endregion void serverslice(int seconds, int timeout)

		#region void calclight()
		/// <summary>
		///     calclight
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in main.cpp:
		///     <code>extern DECLSPEC void CDECL calclight();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void calclight();
		#endregion void calclight()

		#region void writeservercfg()
		/// <summary>
		///     writeservercfg
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in serverbrowser.cpp:
		///     <code>extern DECLSPEC void CDECL writeservercfg();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void writeservercfg();
		#endregion void writeservercfg()

		#region void updatevol()
		/// <summary>
		///     updatevol
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in sound.cpp:
		///     <code>extern DECLSPEC void CDECL updatevol();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void updatevol();
		#endregion void updatevol()

		#region void initclientnet()
		/// <summary>
		///     initclientmap
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in clientgame.cpp:
		///     <code>extern DECLSPEC void CDECL initclientnet();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void initclientnet();
		#endregion void initclientnet()

		#region string getclientmap()
		/// <summary>
		///     getclientmap
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in clientgame.cpp:
		///     <code>extern DECLSPEC char CDECL *getclientmap();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern string getclientmap();
		#endregion string getclientmap()

		#region void initserver()
		/// <summary>
		///     initserver
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in server.cpp:
		///     <code>extern DECLSPEC void CDECL initserver(bool dedicated, int uprate, char *sdesc, char *ip, char *master, char *passwd, int maxcl);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void initserver(bool dedicated, int uprate, string sdesc, string ip, out string master, string passwd, int maxcl);
		#endregion void initserver()

		#region void localconnect()
		/// <summary>
		///     localconnect
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in server.cpp:
		///     <code>extern DECLSPEC void CDECL localconnect();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void localconnect();
		#endregion void localconnect()

		#region void cleardlights()
		/// <summary>
		///     cleardlights
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in worldlight.cpp:
		///     <code>extern DECLSPEC void CDECL cleardlights();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void cleardlights();
		#endregion void cleardlights()

		#region void setarraypointers()
		/// <summary>
		///     setarraypointers
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in rendercubes.cpp:
		///     <code>extern DECLSPEC void CDECL setarraypointers();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void setarraypointers();
		#endregion void setarraypointers()

		#region IntPtr getplayer1()
		/// <summary>
		///     cleardlights
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in worldlight.cpp:
		///     <code>extern DECLSPEC dynent * CDECL getplayer1();</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern IntPtr getplayer1();
		#endregion IntPtr getplayer1()

		#region void updateworld(int millis)
		/// <summary>
		///     updateworld
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in clientgame.cpp:
		///     <code>extern DECLSPEC void CDECL updateworld(int millis);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void updateworld(int millis);
		#endregion void updateworld(int millis)

		#region void mousemove(int dx, int dy)
		/// <summary>
		///     mousemove
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in clientgame.cpp:
		///     <code>extern DECLSPEC void CDECL mousemove(int dx, int dy)</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void mousemove(int dx, int dy);
		#endregion void mousemove(int dx, int dy)

		#region void newmenu(string name)
		/// <summary>
		///     new menu
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in menus.cpp:
		///     <code>extern DECLSPEC void CDECL newmenu(char *name);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void newmenu(string name);
		#endregion void newmenu(string name)

		#region void changemap(string name)
		/// <summary>
		///     changemap
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in clientgame.cpp:
		///     <code>extern DECLSPEC void CDECL changemap(char *name);</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void changemap(string name);
		#endregion void changemap(string name)

		#region void keypress(int code, bool isdown, int cooked)
		/// <summary>
		///     keypress
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in console.cpp:
		///     <code>extern DECLSPEC void CDECL keypress(int code, bool isdown, int cooked)</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void keypress(int code, bool isdown, int cooked);
		#endregion void keypress(int code, bool isdown, int cooked)

		#region void exec(string cfgfile)
		/// <summary>
		///     exec cfg file
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in command.cpp:
		///     <code>extern DECLSPEC void CDECL exec(char *cfgfile);;</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void exec(string cfgfile);
		#endregion void exec(string cfgfile)

		#region bool execfile(string cfgfile)
		/// <summary>
		///     exec cfg file
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in command.cpp:
		///     <code>extern DECLSPEC bool CDECL execfile(char *cfgfile);;</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern bool execfile(string cfgfile);
		#endregion bool execfile(string cfgfile)

		#region void rendermodel(string mdl, int frame, int range, int tex, float rad, float x, float y, float z, float yaw, float pitch, bool teammate, float scale, float speed, int snap, int basetime))
		/// <summary>
		///     keypress
		/// </summary>
		/// <remarks>
		/// <p>Binds to C-function call in console.cpp:
		///     <code>extern DECLSPEC void CDECL rendermodel(char *mdl, int frame, int range, int tex, float rad, float x, float y, float z, float yaw, float pitch, bool teammate, float scale, float speed, int snap = 0, int basetime = 0)</code>
		///     </p>
		/// </remarks>
		[DllImport(MEZZANINE_NATIVE_LIBRARY, CallingConvention=CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
		public static extern void rendermodel(string mdl, int frame, int range, int tex, float rad, float x, float y, float z, float yaw, float pitch, bool teammate, float scale, float speed, int snap, int basetime);
		#endregion void rendermodel(string mdl, int frame, int range, int tex, float rad, float x, float y, float z, float yaw, float pitch, bool teammate, float scale, float speed, int snap, int basetime))

		#endregion Tess Methods
	}
}
