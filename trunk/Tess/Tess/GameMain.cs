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
using TessLib;

namespace Tess
{
	/// <summary>
	/// Summary description for Tess.
	/// </summary>
	class GameMain
	{
		float DMF = 16.0f ;
		float DAF = 1.0f ;
		float DVF = 100.0f;

		int VIRTW = 2400;                      // virtual screen size for text & HUD
		int VIRTH = 1800;
		
		//#define SWS(w,x,y,s) (&(w)[(y)*(s)+(x)])
		//#define SW(w,x,y) SWS(w,x,y,ssize)
		//#define S(x,y) SW(world,x,y)            // convenient lookup of a lowest mip cube
		int SMALLEST_FACTOR = 6;             // determines number of mips there can be
		int DEFAULT_FACTOR = 8;
		int LARGEST_FACTOR = 11;               // 10 is already insane
		//#define SOLID(x) ((x)->type==SOLID)
		int MINBORD = 2;                       // 2 cubes from the edge of the world are always solid
		//#define OUTBORD(x,y) ((x)<MINBORD || (y)<MINBORD || (x)>=ssize-MINBORD || (y)>=ssize-MINBORD)

		int mapVersion = 5; // bump if map format changes, see worldio.cpp
		int SAVEGAMEVERSION = 4;
		int MAXCLIENTS = 256;               // in a multiplayer game, can be arbitrarily changed
		int MAXTRANS = 5000;                // max amount of data to swallow in 1 go
		int CUBE_SERVER_PORT = 28765;
		int CUBE_SERVINFO_PORT = 28766;
		int PROTOCOL_VERSION = 122;            // bump when protocol changes
		
		int NUMGUNS = 9;
		int ignore = 5;
		
		float fps = 30.0f;
		//int screenWidth = 640;
		//int screenHeight = 480;
		int gamespeed = 100;
		int lastmillis = 0;
		bool demoplayback;
		int curtime;
		int framesinmap = 0;
		byte lasttype = 0;
		byte lastbut = 0;
		int minmillis = 5;
		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			GameMain tess = new GameMain();
			tess.Run(args);
		}

		void Run(string[] args)
		{
			bool dedicated = false;
			int uprate = 0;
			int maxcl = 4;
			string sdesc = "";
			string ip = "";
			string master = null; 
			string passwd = "";

			int retval = ManagedWrapper.minitialize();

			TessLib.GameInit.Log("sdl");
    
			foreach(string a in args)
			{
				if(a.StartsWith("-"))
				{
					switch(a.Substring(1,1))
					{
						case "d": 
							dedicated = true; 
							break;
						case "t": 
							//fs = 0; 
							break;
						case "w": 
							GameInit.ScreenWidth  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						case "h": 
							GameInit.ScreenHeight  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						case "u": 
							uprate = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						case "n": 
							sdesc = a.Substring(2, a.Length - 2); 
							break;
						case "i": 
							ip     = a.Substring(2, a.Length - 2); 
							break;
						case "m": 
							master = a.Substring(2, a.Length - 2); 
							break;
						case "p": 
							passwd = a.Substring(2, a.Length - 2); 
							break;
						case "c": 
							maxcl  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						default:  
							TessLib.GameInit.Log("unknown commandline option");
							break;
					}
				}
				else
				{ 
					TessLib.GameInit.Log("unknown commandline argument");
				}
			};
			
			TessLib.GameInit.Log("net");
			if(TessLib.Bindings.enet_initialize()<0)
			{
				TessLib.GameInit.Fatal("Unable to initialise network module");
			}
			TessLib.ClientGame.initclient();
			TessLib.Bindings.initserver(dedicated, uprate, sdesc, ip, out master, passwd, maxcl);  // never returns if dedicated    
			TessLib.GameInit.Log("world");
			TessLib.Bindings.empty_world(7, true);
			TessLib.GameInit.Log("video: sdl");
			Video.GLDoubleBufferEnabled = false;
			TessLib.GameInit.Log("video: mode");
			Video.WindowIcon();
			Video.WindowCaption = "Tess Engine";
			//Video.GrabInput = true;
			Video.SetVideoModeWindowOpenGL(GameInit.ScreenWidth, GameInit.ScreenHeight);
			
			TessLib.GameInit.Log("video: misc");
			Keyboard.KeyRepeat = false;
			Mouse.ShowCursor = false;
			
			TessLib.GameInit.Log("gl");
				
			TessLib.RenderGl.GlInit(GameInit.ScreenWidth, GameInit.ScreenHeight);

			string dataDirectory = "";
			string filepath = "";

			if (File.Exists(dataDirectory + "newchars.png"))
			{
				filepath = "";
			}
			
			TessLib.GameInit.Log("basetex");
			int xs = 0;
			int ys = 0;
			if(!TessLib.Bindings.installtex(2, filepath + dataDirectory + "data/newchars.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(3, filepath + dataDirectory + "data/martin/base.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(6, filepath + dataDirectory + "data/martin/ball1.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(7, filepath + dataDirectory + "data/martin/smoke.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(8, filepath + dataDirectory + "data/martin/ball2.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(9, filepath + dataDirectory + "data/martin/ball3.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(4, filepath + dataDirectory + "data/explosion.jpg", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(5, filepath + dataDirectory + "data/items.png", out xs, out ys, false) ||
				!TessLib.Bindings.installtex(1, filepath + dataDirectory + "data/crosshair.png", out xs, out ys, false)) 
			{
				TessLib.GameInit.Fatal("could not find core textures (hint: run cube from the parent of the bin directory)");
			}
			
			TessLib.GameInit.Log("sound");
			Mixer.Initialize();
			
			TessLib.GameInit.Log("cfg");
			TessLib.Bindings.newmenu("frags\tpj\tping\tteam\tname");
			TessLib.Bindings.newmenu("ping\tplr\tserver");
			TessLib.Bindings.exec( filepath + dataDirectory + "data/keymap.cfg");
			TessLib.Bindings.exec( filepath + dataDirectory + "data/menus.cfg");
			TessLib.Bindings.exec( filepath + dataDirectory + "data/prefabs.cfg");
			TessLib.Bindings.exec( filepath + dataDirectory + "data/sounds.cfg");
			TessLib.Bindings.exec( filepath + dataDirectory + "servers.cfg");
			if(!TessLib.Bindings.execfile( filepath + dataDirectory + "config.cfg")) 
			{
				TessLib.Bindings.execfile( filepath + dataDirectory + "data/defaults.cfg");
			}
			TessLib.Bindings.exec( filepath + dataDirectory + "autoexec.cfg");
			
			TessLib.GameInit.Log("localconnect");
			TessLib.Bindings.localconnect();
			// if this map is changed, also change depthcorrect()   
			TessLib.Bindings.changemap("metl3");		
			TessLib.GameInit.Log("mainloop");
			
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown);
			Events.KeyboardUp += new KeyboardEventHandler(this.KeyUp);
			Events.Tick += new TickEventHandler(this.Tick);
			Events.Quit += new QuitEventHandler(this.Quit);
			Events.MouseMotion += new MouseMotionEventHandler(this.MouseMotion);
			Events.MouseButtonUp += new MouseButtonEventHandler(this.MouseButtonUp);
			Events.MouseButtonDown += new MouseButtonEventHandler(this.MouseButtonDown);
			Events.Run();
		}
		private void Tick(object sender, TickEventArgs e)
		{
			int millis = Timer.TicksElapsed*gamespeed/100;
			if(millis-lastmillis>200) 
			{
				lastmillis = millis-200;
			}
			else if(millis-lastmillis<1) 
			{
				lastmillis = millis-1;
			}
			if(millis-lastmillis<minmillis) 
			{
				Timer.DelayTicks(minmillis-(millis-lastmillis));
			}
			TessLib.Bindings.cleardlights();
			TessLib.Bindings.updateworld(millis);
			if(!demoplayback)
			{
				TessLib.Bindings.serverslice(DateTime.Now.Second, 0);
			}

			fps = 30.0f;
			//fps = (1000.0f/curtime+fps*50)/51;
			TessLib.GameInit.Player1Ptr = TessLib.Bindings.getplayer1();
			
			TessLib.Bindings.computeraytable(TessLib.GameInit.Player1.o.x, TessLib.GameInit.Player1.o.y);
			TessLib.Bindings.readdepth(GameInit.ScreenWidth, GameInit.ScreenHeight);
					
			Video.GLSwapBuffers();
			TessLib.Bindings.updatevol();
				
			if(framesinmap++<5)	// cheap hack to get rid of initial sparklies, even when triple buffering etc.
			{
				//player1.yaw += 5;
				//TessLib.Main.Player1 = player1;
				TessLib.Bindings.gl_drawframe(GameInit.ScreenWidth, GameInit.ScreenHeight, fps);
				//player1.yaw -= 5;
				//TessLib.Main.Player1 = player1;
			};
			TessLib.Bindings.gl_drawframe(GameInit.ScreenWidth, GameInit.ScreenHeight, fps);
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			TessLib.GameInit.Quit();
			ManagedWrapper.mterminate();
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			if (e.Key == Key.F12)
			{
				TessLib.GameInit.Screenshot();
			}
			TessLib.Bindings.keypress((int)e.Key, e.Down == true, e.Unicode);	
		}

		private void KeyUp(object sender, KeyboardEventArgs e)
		{
			TessLib.Bindings.keypress((int)e.Key, e.Down == true, e.Unicode);
		}

		private void MouseMotion(object sender, MouseMotionEventArgs e)
		{
			if(ignore > 0)
			{ 
				ignore--; 
			}
			else
			{
				TessLib.Bindings.mousemove(e.RelativeX, e.RelativeY);
			}
		}

		private void MouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if(lasttype==(byte)e.Type && lastbut == (byte)e.Button)
			{
			}
			else
			{
				TessLib.Bindings.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
				lasttype = (byte)e.Type;
				lastbut = (byte)e.Button;
			}
		}

		private void MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(lasttype==(byte)e.Type && lastbut == (byte)e.Button)
			{
			}
			else
			{
				TessLib.Bindings.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
				lasttype = (byte)e.Type;
				lastbut = (byte)e.Button;
			}
		}
	}
}
