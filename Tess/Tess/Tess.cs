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
		int scr_w = 640;
		int scr_h = 480;
		int gamespeed = 100;
		int lastmillis = 0;
		bool demoplayback;
		int curtime;
		int framesinmap = 0;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//Cube.native_main(args.Length, args);
			//while (true)
			//{};
			Tess t = new Tess();
			t.Run(args);

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
			int fs = 0; // normally Sdl.SDL_FULLSCREEN;
			//int par = 0;
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
							fs = 0; 
							break;
						case "w": 
							scr_w  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
							break;
						case "h": 
							scr_h  = Convert.ToInt32(a.Substring(2, a.Length - 2)); 
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
    
			if(Sdl.SDL_Init(Sdl.SDL_INIT_TIMER|Sdl.SDL_INIT_VIDEO)<0)
			{
				Fatal("Unable to initialize SDL");
			}
			
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
			if(Sdl.SDL_InitSubSystem(Sdl.SDL_INIT_VIDEO)<0) 
			{
				Fatal("Unable to initialize SDL Video");
			}
//			Video.GLDoubleBufferEnabled = true;
			Log("video: mode");
			Sdl.SDL_GL_SetAttribute(Sdl.SDL_GL_DOUBLEBUFFER, 1);
			//Video.WindowCaption = "Tess Engine";
			Sdl.SDL_WM_SetCaption("cube engine", null);
			//Video.GrabInput();
			Sdl.SDL_WM_GrabInput(Sdl.SDL_GRAB_ON);
			//Video.SetVideoModeWindowOpenGL(scr_w, scr_h);
			if(Sdl.SDL_SetVideoMode(scr_w, scr_h, 0, Sdl.SDL_OPENGL|fs)== IntPtr.Zero)
			{
				Fatal("Unable to create OpenGL screen");
			}
			
			Log("video: misc");
			//Keyboard.KeyRepeat = false;
			Cube.keyrepeat(false);
			//Timer.Initialize();
			//Mouse.ShowCursor = false;
			Sdl.SDL_ShowCursor(0);
			
			Log("gl");
						
			Cube.gl_init(scr_w, scr_h);

			string data_directory = @"game/";
			string filepath = @"../../";

			if (File.Exists(data_directory + "newchars.png"))
			{
				filepath = "";
			}
			
			Log("basetex");
			int xs = 0;
			int ys = 0;
			if(!Cube.installtex(2, filepath + data_directory + "data/newchars.png", out xs, out ys, false) ||
				!Cube.installtex(3, filepath + data_directory + "data/martin/base.png", out xs, out ys, false) ||
				!Cube.installtex(6, filepath + data_directory + "data/martin/ball1.png", out xs, out ys, false) ||
				!Cube.installtex(7, filepath + data_directory + "data/martin/smoke.png", out xs, out ys, false) ||
				!Cube.installtex(8, filepath + data_directory + "data/martin/ball2.png", out xs, out ys, false) ||
				!Cube.installtex(9, filepath + data_directory + "data/martin/ball3.png", out xs, out ys, false) ||
				!Cube.installtex(4, filepath + data_directory + "data/explosion.jpg", out xs, out ys, false) ||
				!Cube.installtex(5, filepath + data_directory + "data/items.png", out xs, out ys, false) ||
				!Cube.installtex(1, filepath + data_directory + "data/crosshair.png", out xs, out ys, false)) 
			{
				Fatal("could not find core textures (hint: run cube from the parent of the bin directory)");
			}
			
			Log("sound");
			Cube.initsound();
			
			Log("cfg");
			Cube.newmenu("frags\tpj\tping\tteam\tname");
			Cube.newmenu("ping\tplr\tserver");
			Cube.exec( filepath + data_directory + "data/keymap.cfg");
			Cube.exec( filepath + data_directory + "data/menus.cfg");
			Cube.exec( filepath + data_directory + "data/prefabs.cfg");
			Cube.exec( filepath + data_directory + "data/sounds.cfg");
			Cube.exec( filepath + data_directory + "servers.cfg");
			if(!Cube.execfile( filepath + data_directory + "config.cfg")) 
			{
				Cube.execfile( filepath + data_directory + "data/defaults.cfg");
			}
			Cube.exec( filepath + data_directory + "autoexec.cfg");
			
			Log("localconnect");
			Cube.localconnect();
			// if this map is changed, also change depthcorrect()   
			Cube.changemap("metl3");		
			Log("mainloop");
			int ignore = 5;
			int minmillis = 5;
			float fps = 30.0f;
			IntPtr player1Ptr;
			DynamicEntity player1;

			while (true)
			{
				int millis = Sdl.SDL_GetTicks()*gamespeed/100;
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
					Sdl.SDL_Delay(minmillis-(millis-lastmillis));
				}
				Cube.cleardlights();
				Cube.updateworld(millis);
				if(!demoplayback)
				{
					Cube.serverslice(DateTime.Now.Second, 0);
				}

				fps = 30.0f;
				//fps = (1000.0f/curtime+fps*50)/51;
				//System.Threading.Thread.Sleep(1000);
				//Cube.setplayer1yaw(5);
				//Console.WriteLine("Yaw: " + Cube.getplayer1yaw());
				player1Ptr = Cube.getplayer1();
				player1 = (DynamicEntity)Marshal.PtrToStructure(player1Ptr, typeof(DynamicEntity));
				//Console.WriteLine("Yaw2: " + player1.yaw);
				Cube.computeraytable(player1.o.x, player1.o.y);
				Cube.readdepth(scr_w, scr_h);
					
				Sdl.SDL_GL_SwapBuffers(); 
				Cube.updatevol();
				
				if(framesinmap++<5)	// cheap hack to get rid of initial sparklies, even when triple buffering etc.
				{
					player1.yaw += 5;
					//Console.WriteLine("Yaw3: " + player1.yaw);
					Marshal.StructureToPtr(player1, player1Ptr, false);
					//Console.WriteLine("Yaw3.5: " + Cube.getplayer1yaw());
					Cube.gl_drawframe(scr_w, scr_h, fps);
					player1.yaw -= 5;
					//Console.WriteLine("Yaw4: " + player1.yaw);
					Marshal.StructureToPtr(player1, player1Ptr, false);
					//Console.WriteLine("Yaw4.5: " + Cube.getplayer1yaw());
				};
				Cube.gl_drawframe(scr_w, scr_h, fps);
				Sdl.SDL_Event sdlevent;
				byte lasttype = 0;
				byte lastbut = 0;
				while(Sdl.SDL_PollEvent(out sdlevent) > 0)
				{
					switch(sdlevent.type)
					{
						case Sdl.SDL_QUIT:
							Cube.quit();
							break;
						case Sdl.SDL_KEYDOWN: 
						case Sdl.SDL_KEYUP: 
							Cube.keypress(sdlevent.key.keysym.sym, sdlevent.key.state==Sdl.SDL_PRESSED, sdlevent.key.keysym.unicode);
							break;
						case Sdl.SDL_MOUSEMOTION:
							if(ignore > 0)
							{ 
								ignore--; 
								break;
							}
							Cube.mousemove(sdlevent.motion.xrel, sdlevent.motion.yrel);
							break;
						case Sdl.SDL_MOUSEBUTTONDOWN:
						case Sdl.SDL_MOUSEBUTTONUP:
							if(lasttype==sdlevent.type && lastbut==sdlevent.button.button) 
							{
								break; // why?? get event twice without it
							}
							Cube.keypress(-sdlevent.button.button, sdlevent.button.state!=0, 0);
							lasttype = sdlevent.type;
							lastbut = sdlevent.button.button;
							break;
					}
				}
			}
		}
	}
}
