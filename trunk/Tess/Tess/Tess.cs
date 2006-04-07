using System;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using SdlDotNet;
using System.Runtime.InteropServices;

namespace Tess
{
	[StructLayout(LayoutKind.Sequential, Pack=4)]
	public struct Vector
	{ 
		public float x;
		public float y;
		public float z; 
	}

	/// <summary>
	/// Summary description for Tess.
	/// </summary>
	class Tess
	{
		DynamicEntity player1;
		int NUMGUNS = 9;
		int ignore = 5;
		IntPtr player1Ptr;
		float fps = 30.0f;
		int screenWidth = 640;
		int screenHeight = 480;
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
			Tess tess = new Tess();
			tess.Run(args);
		}
		void Log(string input)
		{
			Console.WriteLine("init: {0}", input);
		}

		void Fatal(string s, string o)
		{
			Console.WriteLine(s + o + Sdl.SDL_GetError());
			Cube.cleanup(s + o + Sdl.SDL_GetError());
		}
		void Fatal(string s)
		{
			Fatal(s, "");
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

			Log("sdl");
    
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
							screenWidth  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						case "h": 
							screenHeight  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
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
							Log("unknown commandline option");
							break;
					}
				}
				else
				{ 
					Log("unknown commandline argument");
				}
			};
			
			Log("net");
			if(Cube.enet_initialize()<0)
			{
				Fatal("Unable to initialise network module");
			}
			Cube.initclient();
			Cube.initserver(dedicated, uprate, sdesc, ip, out master, passwd, maxcl);  // never returns if dedicated    
			Log("world");
			Cube.empty_world(7, true);
			Log("video: sdl");
			Video.GLDoubleBufferEnabled = false;
			Log("video: mode");
			Video.WindowIcon();
			Video.WindowCaption = "Tess Engine";
			Video.GrabInput();
			Video.SetVideoModeWindowOpenGL(screenWidth, screenHeight);
			
			Log("video: misc");
			Keyboard.KeyRepeat = false;
			Mouse.ShowCursor = false;
			
			Log("gl");
						
			Cube.gl_init(screenWidth, screenHeight);

			string dataDirectory = @"game/";
			string filepath = @"../../";

			if (File.Exists(dataDirectory + "newchars.png"))
			{
				filepath = "";
			}
			
			Log("basetex");
			int xs = 0;
			int ys = 0;
			if(!Cube.installtex(2, filepath + dataDirectory + "data/newchars.png", out xs, out ys, false) ||
				!Cube.installtex(3, filepath + dataDirectory + "data/martin/base.png", out xs, out ys, false) ||
				!Cube.installtex(6, filepath + dataDirectory + "data/martin/ball1.png", out xs, out ys, false) ||
				!Cube.installtex(7, filepath + dataDirectory + "data/martin/smoke.png", out xs, out ys, false) ||
				!Cube.installtex(8, filepath + dataDirectory + "data/martin/ball2.png", out xs, out ys, false) ||
				!Cube.installtex(9, filepath + dataDirectory + "data/martin/ball3.png", out xs, out ys, false) ||
				!Cube.installtex(4, filepath + dataDirectory + "data/explosion.jpg", out xs, out ys, false) ||
				!Cube.installtex(5, filepath + dataDirectory + "data/items.png", out xs, out ys, false) ||
				!Cube.installtex(1, filepath + dataDirectory + "data/crosshair.png", out xs, out ys, false)) 
			{
				Fatal("could not find core textures (hint: run cube from the parent of the bin directory)");
			}
			
			Log("sound");
			Cube.initsound();
			
			Log("cfg");
			Cube.newmenu("frags\tpj\tping\tteam\tname");
			Cube.newmenu("ping\tplr\tserver");
			Cube.exec( filepath + dataDirectory + "data/keymap.cfg");
			Cube.exec( filepath + dataDirectory + "data/menus.cfg");
			Cube.exec( filepath + dataDirectory + "data/prefabs.cfg");
			Cube.exec( filepath + dataDirectory + "data/sounds.cfg");
			Cube.exec( filepath + dataDirectory + "servers.cfg");
			if(!Cube.execfile( filepath + dataDirectory + "config.cfg")) 
			{
				Cube.execfile( filepath + dataDirectory + "data/defaults.cfg");
			}
			Cube.exec( filepath + dataDirectory + "autoexec.cfg");
			
			Log("localconnect");
			Cube.localconnect();
			// if this map is changed, also change depthcorrect()   
			Cube.changemap("metl3");		
			Log("mainloop");
			
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
			Cube.cleardlights();
			Cube.updateworld(millis);
			if(!demoplayback)
			{
				Cube.serverslice(DateTime.Now.Second, 0);
			}

			fps = 30.0f;
			//fps = (1000.0f/curtime+fps*50)/51;
			player1Ptr = Cube.getplayer1();
			player1 = (DynamicEntity)Marshal.PtrToStructure(player1Ptr, typeof(DynamicEntity));
			Cube.computeraytable(player1.o.x, player1.o.y);
			Cube.readdepth(screenWidth, screenHeight);
					
			Video.GLSwapBuffers();
			Cube.updatevol();
				
			if(framesinmap++<5)	// cheap hack to get rid of initial sparklies, even when triple buffering etc.
			{
				player1.yaw += 5;
				Marshal.StructureToPtr(player1, player1Ptr, false);
				Cube.gl_drawframe(screenWidth, screenHeight, fps);
				player1.yaw -= 5;
				Marshal.StructureToPtr(player1, player1Ptr, false);
			};
			Cube.gl_drawframe(screenWidth, screenHeight, fps);
		}

		private void Quit(object sender, QuitEventArgs e)
		{
			Cube.writeservercfg();
			Cube.cleanup(null);
		}

		private void KeyDown(object sender, KeyboardEventArgs e)
		{
			Cube.keypress((int)e.Key, e.Down == true, e.Unicode);	
		}

		private void KeyUp(object sender, KeyboardEventArgs e)
		{
			Cube.keypress((int)e.Key, e.Down == true, e.Unicode);
		}

		private void MouseMotion(object sender, MouseMotionEventArgs e)
		{
			if(ignore > 0)
			{ 
				ignore--; 
			}
			else
			{
				Cube.mousemove(e.RelativeX, e.RelativeY);
			}
		}

		private void MouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if(lasttype==(byte)e.Type && lastbut == (byte)e.Button)
			{
			}
			else
			{
				Cube.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
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
				Cube.keypress(-1*(byte)e.Button, e.ButtonPressed != false, 0);
				lasttype = (byte)e.Type;
				lastbut = (byte)e.Button;
			}
		}
	}
}
