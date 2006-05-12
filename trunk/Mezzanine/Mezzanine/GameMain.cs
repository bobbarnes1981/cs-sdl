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
using MezzanineLib;

namespace Mezzanine
{
	/// <summary>
	/// Summary description for Mezzanine.
	/// </summary>
	class GameMain
	{
		int mapVersion = 5; // bump if map format changes, see worldio.cpp
		
		int NUMGUNS = 9;
		int ignore = 5;
		
		float fps = 30.0f;
		int gamespeed = 100;
		bool demoplayback;
		int framesinmap = 0;
		byte lasttype = 0;
		byte lastbut = 0;
		int minmillis = 5;

		bool grabMouse = true;
		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			GameMain mezzanine = new GameMain();
			mezzanine.Run(args);
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

			MezzanineLib.GameInit.Log("sdl");
    
			foreach(string a in args)
			{
				if(a.StartsWith("-"))
				{
					switch(a.Substring(1,1))
					{
						case "g":
							grabMouse = false;
							break;
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
							MezzanineLib.GameInit.Log("unknown commandline option");
							break;
					}
				}
				else
				{ 
					MezzanineLib.GameInit.Log("unknown commandline argument");
				}
			};
			
			MezzanineLib.GameInit.Log("net");
			if(MezzanineLib.Bindings.enet_initialize()<0)
			{
				MezzanineLib.GameInit.Fatal("Unable to initialise network module");
			}
			MezzanineLib.ClientServer.ClientGame.initclient();
			MezzanineLib.Bindings.initserver(dedicated, uprate, sdesc, ip, out master, passwd, maxcl);  // never returns if dedicated    
			MezzanineLib.GameInit.Log("world");
			MezzanineLib.Bindings.empty_world(7, true);
			MezzanineLib.GameInit.Log("video: sdl");
			Video.GLDoubleBufferEnabled = false;
			MezzanineLib.GameInit.Log("video: mode");
			Video.WindowIcon();
			Video.WindowCaption = "Mezzanine Engine";
			if (grabMouse)
			{
				Video.GrabInput = true;
			}
			Video.SetVideoModeWindowOpenGL(GameInit.ScreenWidth, GameInit.ScreenHeight);
			
			MezzanineLib.GameInit.Log("video: misc");
			Keyboard.KeyRepeat = false;
			Mouse.ShowCursor = false;
			
			MezzanineLib.GameInit.Log("gl");
				
			MezzanineLib.Render.RenderGl.GlInit(GameInit.ScreenWidth, GameInit.ScreenHeight);

			string dataDirectory = "";
			string filepath = "";

			if (File.Exists(dataDirectory + "newchars.png"))
			{
				filepath = "";
			}
			
			MezzanineLib.GameInit.Log("basetex");
			int xs = 0;
			int ys = 0;
			if(!MezzanineLib.Render.RenderGl.InstallTexture(2, filepath + dataDirectory + "data/newchars.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(3, filepath + dataDirectory + "data/martin/base.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(6, filepath + dataDirectory + "data/martin/ball1.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(7, filepath + dataDirectory + "data/martin/smoke.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(8, filepath + dataDirectory + "data/martin/ball2.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(9, filepath + dataDirectory + "data/martin/ball3.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(4, filepath + dataDirectory + "data/explosion.jpg", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(5, filepath + dataDirectory + "data/items.png", out xs, out ys, false) ||
				!MezzanineLib.Render.RenderGl.InstallTexture(1, filepath + dataDirectory + "data/crosshair.png", out xs, out ys, false)) 
		{
				MezzanineLib.GameInit.Fatal("could not find core textures (hint: run cube from the parent of the bin directory)");
			}
			
			MezzanineLib.GameInit.Log("sound");
			Mixer.Initialize();
			
			MezzanineLib.GameInit.Log("cfg");
			MezzanineLib.Bindings.newmenu("frags\tpj\tping\tteam\tname");
			MezzanineLib.Bindings.newmenu("ping\tplr\tserver");
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "data/keymap.cfg");
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "data/menus.cfg");
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "data/prefabs.cfg");
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "data/sounds.cfg");
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "servers.cfg");
			if(!MezzanineLib.Bindings.execfile( filepath + dataDirectory + "config.cfg")) 
			{
				MezzanineLib.Bindings.execfile( filepath + dataDirectory + "data/defaults.cfg");
			}
			MezzanineLib.Bindings.exec( filepath + dataDirectory + "autoexec.cfg");
			
			MezzanineLib.GameInit.Log("localconnect");
			MezzanineLib.Bindings.localconnect();
			// if this map is changed, also change depthcorrect()   
			MezzanineLib.Bindings.changemap("metl3");		
			MezzanineLib.GameInit.Log("mainloop");
			
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
			if(millis-GameInit.LastMillis>200) 
			{
				GameInit.LastMillis = millis-200;
			}
			else if(millis-GameInit.LastMillis<1) 
			{
				GameInit.LastMillis = millis-1;
			}
			if(millis-GameInit.LastMillis<minmillis) 
			{
				Timer.DelayTicks(minmillis-(millis-GameInit.LastMillis));
			}
			MezzanineLib.Bindings.cleardlights();
			MezzanineLib.Bindings.updateworld(millis);
			if(!demoplayback)
			{
				MezzanineLib.Bindings.serverslice(DateTime.Now.Second, 0);
			}

			fps = 30.0f;
			//fps = (1000.0f/curtime+fps*50)/51;
			MezzanineLib.GameInit.Player1Ptr = MezzanineLib.Bindings.getplayer1();
			
			MezzanineLib.Bindings.computeraytable(MezzanineLib.GameInit.Player1.o.x, MezzanineLib.GameInit.Player1.o.y);
			MezzanineLib.Bindings.readdepth(GameInit.ScreenWidth, GameInit.ScreenHeight);
					
			Video.GLSwapBuffers();
			MezzanineLib.Bindings.updatevol();
				
			if(framesinmap++<5)	// cheap hack to get rid of initial sparklies, even when triple buffering etc.
			{
				//player1.yaw += 5;
				//MezzanineLib.Main.Player1 = player1;
				MezzanineLib.Bindings.gl_drawframe(GameInit.ScreenWidth, GameInit.ScreenHeight, fps);
				//player1.yaw -= 5;
				//MezzanineLib.Main.Player1 = player1;
			};
			MezzanineLib.Bindings.gl_drawframe(GameInit.ScreenWidth, GameInit.ScreenHeight, fps);
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			MezzanineLib.GameInit.Quit();
			ManagedWrapper.mterminate();
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			if (e.Key == Key.F12)
			{
				MezzanineLib.GameInit.Screenshot();
			}
			MezzanineLib.Bindings.keypress((int)e.Key, e.Down == true, e.Unicode);	
		}

		private void KeyUp(object sender, KeyboardEventArgs e)
		{
			MezzanineLib.Bindings.keypress((int)e.Key, e.Down == true, e.Unicode);
		}

		private void MouseMotion(object sender, MouseMotionEventArgs e)
		{
			if(ignore > 0)
			{ 
				ignore--; 
			}
			else
			{
				MezzanineLib.Bindings.mousemove(e.RelativeX, e.RelativeY);
			}
		}

		private void MouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if(lasttype==(byte)e.Type && lastbut == (byte)e.Button)
			{
			}
			else
			{
				MezzanineLib.Bindings.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
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
				MezzanineLib.Bindings.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
				lasttype = (byte)e.Type;
				lastbut = (byte)e.Button;
			}
		}
	}
}
