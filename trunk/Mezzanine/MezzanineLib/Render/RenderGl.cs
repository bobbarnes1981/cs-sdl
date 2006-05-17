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
 * Mezzanine is a .NET port of Cube (version released on 2005-Aug-29).
 * Cube was written by Wouter van Oortmerssen (http://cubeengine.com)
 */
#endregion License

using System;
using System.Collections;
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using SdlDotNet;
using System.Runtime.InteropServices;

namespace MezzanineLib.Render
{
	public sealed class RenderGl
	{
		static Glu.GLUquadric qsphere;
		static int glmaxtexsize = 256;
		static bool hasOverBright = false;
		static string[] hudGunNames = 
{ 
	"hudguns/fist",
	"hudguns/shotg",
	"hudguns/chaing", 
	"hudguns/rocket", 
	"hudguns/rifle" 
};
		public const int MAXTEX = 1000;
		public const int FIRSTTEX = 1000;                  // opengl id = loaded id + FIRSTTEX
		// std 1+, sky 14+, mdls 20+
		public const int MAXFRAMES = 2;                    // increase to allow more complex shader defs

		public static int[] texx= new int[MAXTEX];                           // ( loaded texture ) -> ( name, size )
		public static int[] texy = new int[MAXTEX];                           
		public static string[] texname = new string[MAXTEX];

		public struct strip 
		{ 
			public int tex;
			public int start;
			public int num; 
		}

		//VARP(fov, 10, 105, 120);
		//VAR(fog, 64, 180, 1024);
		//VAR(fogcolour, 0, 0x8099B3, 0xFFFFFF);

		//VARP(hudgun,0,1,1);
		static int fov = 105;
		public static int Fov
		{
			get
			{
				return fov;
			}
			set
			{
				if (value > 120)
				{
					fov = 120;
				}
				else if (value < 10)
				{
					fov = 10;
				}
				else
				{
					fov = value;
				}
			}
		}

		public static int Fog
		{
			get
			{
				return fog;
			}
			set
			{
				if (value > 1024)
				{
					fog = 1024;
				}
				else if (value < 64)
				{
					fog = 64;
				}
				else
				{
					fog = value;
				}
			}
		}
		public static int FogColour
		{
			get
			{
				return fogcolour;
			}
			set
			{
				if (value > 0xFFFFFF)
				{
					fogcolour = 0xFFFFFF;
				}
				else if (value < 0)
				{
					fogcolour = 0;
				}
				else
				{
					fogcolour = value;
				}
			}
		}
		public static int HudGun
		{
			get
			{
				return hudgun;
			}
			set
			{
				if (value > 1)
				{
					hudgun = 1;
				}
				else if (value < 0)
				{
					hudgun = 0;
				}
				else
				{
					hudgun = value;
				}
			}
		}
		static int fog = 180;
		static int fogcolour = 0x8099B3;
		static int hudgun = 1;

		public static ArrayList strips = new ArrayList();

		public static void AddStrip(int tex, int start, int n)
		{
			strip s = new strip();
			s.tex = tex;
			s.start = start;
			s.num = n;
			strips.Add(s);
		}

		public static int skyoglid;

		/// <summary>
		/// 
		/// </summary>
		public static string[] HudGunNames
		{
			get
			{
				return hudGunNames;
			}
		}

		public static void DrawHudGun(float fovy, float aspect, int farplane)
		{
			//if(!hudgun /*|| !player1->gunselect*/) return;
    
			Gl.glEnable(Gl.GL_CULL_FACE);
    
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Glu.gluPerspective(fovy, aspect, 0.3f, farplane);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
    
			int rtime = Bindings.reloadtime(GameInit.Player1.gunselect);
			if(GameInit.Player1.lastaction!=0 && GameInit.Player1.lastattackgun==GameInit.Player1.gunselect && (GameInit.LastMillis-GameInit.Player1.lastaction)<rtime)
			{
				DrawHudModel(7, 18, rtime/18.0f, GameInit.Player1.lastaction);
			}
			else
			{
				DrawHudModel(6, 1, 100, 0);
			};

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Glu.gluPerspective(fovy, aspect, 0.15f, farplane);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);

			Gl.glDisable(Gl.GL_CULL_FACE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="speed"></param>
		/// <param name="baseItem"></param>
		public static void DrawHudModel(int start, int end, float speed, int baseItem)
		{
			Bindings.rendermodel(HudGunNames[GameInit.Player1.gunselect], start, end, 0, 1.0f, GameInit.Player1.o.x, GameInit.Player1.o.z, GameInit.Player1.o.y, GameInit.Player1.yaw+90, GameInit.Player1.pitch, false, 1.0f, speed, 0, baseItem);
		}

		/// <summary>
		/// 
		/// </summary>
		public static bool HasOverBright
		{
			get
			{
				return hasOverBright;
			}
			set
			{
				hasOverBright = value;
			}
		}

		static int xtraverts;
		/// <summary>
		/// 
		/// </summary>
		public static int XtraVerts
		{
			get
			{
				return xtraverts;
			}
			set
			{
				xtraverts = value;
			}
		}

		static int currentTextureNumber = 0;
		/// <summary>
		/// 
		/// </summary>
		public static int CurrentTextureNumber
		{
			get
			{
				return currentTextureNumber;
			}
			set
			{
				currentTextureNumber = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void TextureReset() 
		{ 
			currentTextureNumber = 0; 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="amount"></param>
		public static void OverBright(float amount) 
		{ 
			if(hasOverBright) 
			{
				Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_RGB_SCALE_EXT, amount ); 
			}
		}

		public static void GlInit(int w, int h)
		{
			//#define fogvalues 0.5f, 0.6f, 0.7f, 1.0f

			Gl.glViewport(0, 0, w, h);
			Gl.glClearDepth(1.0);
			Gl.glDepthFunc(Gl.GL_LESS);
			Gl.glEnable(Gl.GL_DEPTH_TEST);
			Gl.glShadeModel(Gl.GL_SMOOTH);
    
    
			Gl.glEnable(Gl.GL_FOG);
			Gl.glFogi(Gl.GL_FOG_MODE, Gl.GL_LINEAR);
			Gl.glFogf(Gl.GL_FOG_DENSITY, 0.25F);
			Gl.glHint(Gl.GL_FOG_HINT, Gl.GL_NICEST);
    

			Gl.glEnable(Gl.GL_LINE_SMOOTH);
			Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
			Gl.glEnable(Gl.GL_POLYGON_OFFSET_LINE);
			Gl.glPolygonOffset(-3.0F, -3.0F);

			Gl.glCullFace(Gl.GL_FRONT);
			Gl.glEnable(Gl.GL_CULL_FACE);

			IntPtr exts = Gl.glGetString(Gl.GL_EXTENSIONS);
    
			//if(strstr(exts, "GL_EXT_texture_env_combine")) hasoverbright = true;
			//else conoutf("WARNING: cannot use overbright lighting, using old lighting model!");
        
			Gl.glGetIntegerv(Gl.GL_MAX_TEXTURE_SIZE, out glmaxtexsize);
        
			PurgeTextures();

			qsphere = Glu.gluNewQuadric();
			//if(!(qsphere = Glu.gluNewQuadric())) 
			//{
			//Mezzanine.Fatal("glu sphere");
			//}
			Glu.gluQuadricDrawStyle(qsphere, Glu.GLU_FILL);
			Glu.gluQuadricOrientation(qsphere, Glu.GLU_INSIDE);
			Glu.gluQuadricTexture(qsphere, Gl.GL_TRUE);
			Gl.glNewList(1, Gl.GL_COMPILE);
			Glu.gluSphere(qsphere, 1, 12, 6);
			Gl.glEndList();
		}

		public static void CleanGl()
		{
			//if(qsphere) 
			//{
			Glu.gluDeleteQuadric(qsphere);
			//}
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="curfps"></param>
		public static void GlDrawFrame(int w, int h, float curfps)
		{
			float hf = 0;
				//GameInit.MapHeader.waterlevel-0.3f;
			float fovy = (float)fov*h/w;
			float aspect = w/(float)h;
			bool underwater = false;
				//GameInit.Player1.o.z<hf;
    
			Gl.glFogi(Gl.GL_FOG_START, (fog+64)/8);
			Gl.glFogi(Gl.GL_FOG_END, fog);
			float[] fogc = { (fogcolour>>16)/256.0f, ((fogcolour>>8)&255)/256.0f, (fogcolour&255)/256.0f, 1.0f };
			Gl.glFogfv(Gl.GL_FOG_COLOR, fogc);
			Gl.glClearColor(fogc[0], fogc[1], fogc[2], 1.0f);

			if(underwater)
			{
				fovy += (float)Math.Sin(GameInit.LastMillis/1000.0)*2.0f;
				aspect += (float)Math.Sin(GameInit.LastMillis/1000.0+System.Math.PI)*0.1f;
				Gl.glFogi(Gl.GL_FOG_START, 0);
				Gl.glFogi(Gl.GL_FOG_END, (fog+96)/8);
			}
    
			Gl.glClear((GameInit.Player1.outsidemap ? Gl.GL_COLOR_BUFFER_BIT : 0) | Gl.GL_DEPTH_BUFFER_BIT);

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			int farplane = fog*5/2;
			Glu.gluPerspective(fovy, aspect, 0.15f, farplane);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);

			TransPlayer();

			Gl.glEnable(Gl.GL_TEXTURE_2D);
    
			int xs;
			int ys;
//			skyoglid = Bindings.lookuptexture((int)TextureNumbers.DEFAULT_SKY, out xs, out ys);
			skyoglid = LookupTexture((int)TextureNumbers.DEFAULT_SKY, out xs, out ys);
   
			Bindings.resetcubes();
            
			RenderCubes.curvert = 0;
			Bindings.setstrips();
			//strips.setsize(0);
  
			Bindings.render_world(GameInit.Player1.o.x, GameInit.Player1.o.y, GameInit.Player1.o.z, 
				(int)GameInit.Player1.yaw, (int)GameInit.Player1.pitch, (float)fov, w, h);
			Bindings.finishstrips();

			SetupWorld();

			//RenderStripsSky();
			Bindings.renderstripssky();

			Gl.glLoadIdentity();
			Gl.glRotated(GameInit.Player1.pitch, -1.0, 0.0, 0.0);
			Gl.glRotated(GameInit.Player1.yaw,   0.0, 1.0, 0.0);
			Gl.glRotated(90.0, 1.0, 0.0, 0.0);
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
			Gl.glDisable(Gl.GL_FOG);
			Gl.glDepthFunc(Gl.GL_GREATER);
			RenderText.DrawEnvBox(14, fog*4/3);
			Gl.glDepthFunc(Gl.GL_LESS);
			Gl.glEnable(Gl.GL_FOG);

			TransPlayer();
        
			OverBright(2);
    
			Bindings.renderstrips();

			XtraVerts = 0;

			Bindings.renderclients();
			Bindings.monsterrender();

			Bindings.renderentities();

			Bindings.renderspheres(GameInit.CurrentTime);
			Bindings.renderents();

			Gl.glDisable(Gl.GL_CULL_FACE);

			DrawHudGun(fovy, aspect, farplane);

			OverBright(1);
			int nquads = Bindings.renderwater(hf);
    
			OverBright(2);
			Bindings.render_particles(GameInit.CurrentTime);
			OverBright(1);

			Gl.glDisable(Gl.GL_FOG);

			Gl.glDisable(Gl.GL_TEXTURE_2D);

			Bindings.gl_drawhud(w, h, (int)curfps, nquads, RenderCubes.curvert, underwater);

			Gl.glEnable(Gl.GL_CULL_FACE);
			Gl.glEnable(Gl.GL_FOG);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void TransPlayer()
		{
			Gl.glLoadIdentity();
    
			Gl.glRotated(MezzanineLib.GameInit.Player1.roll,0.0,0.0,1.0);
			Gl.glRotated(MezzanineLib.GameInit.Player1.pitch,-1.0,0.0,0.0);
			Gl.glRotated(MezzanineLib.GameInit.Player1.yaw,0.0,1.0,0.0);

			Gl.glTranslated(-MezzanineLib.GameInit.Player1.o.x, (MezzanineLib.GameInit.Player1.state==(int)CSStatus.CS_DEAD ? MezzanineLib.GameInit.Player1.eyeheight-0.2f : 0)-MezzanineLib.GameInit.Player1.o.z, -MezzanineLib.GameInit.Player1.o.y);   
		}

		/// <summary>
		/// 
		/// </summary>
		public static void SetupWorld()
		{
			Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
			Gl.glEnableClientState(Gl.GL_COLOR_ARRAY);
			Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY); 
			Bindings.setarraypointers();

			if(HasOverBright)
			{
				Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_COMBINE_EXT); 
				Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_COMBINE_RGB_EXT, Gl.GL_MODULATE);
				Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SOURCE0_RGB_EXT, Gl.GL_TEXTURE);
				Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_SOURCE1_RGB_EXT, Gl.GL_PRIMARY_COLOR_EXT);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tnum"></param>
		/// <param name="texname"></param>
		/// <param name="xs"></param>
		/// <param name="ys"></param>
		/// <returns></returns>
		public static bool InstallTexture(int tnum, string texname, out int xs, out int ys)
		{
			return InstallTexture(tnum, texname, out xs, out ys, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tnum"></param>
		/// <param name="texname"></param>
		/// <param name="xs"></param>
		/// <param name="ys"></param>
		/// <param name="clamp"></param>
		/// <returns></returns>
		public static bool InstallTexture(int tnum, string texname, out int xs, out int ys, bool clamp)
		{
			xs = 0;
			ys = 0;
			Surface s = new Surface(texname);
			if(s == null) 
			{ 
				Console.WriteLine("couldn't load texture %s", texname);
				return false; 
			}
	
			if(s.BitsPerPixel!=24) 
			{ 
				Console.WriteLine("texture must be 24bpp: %s", texname);
				return false; 
			}
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, tnum);
			Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, clamp ? Gl.GL_CLAMP_TO_EDGE : Gl.GL_REPEAT);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, clamp ? Gl.GL_CLAMP_TO_EDGE : Gl.GL_REPEAT);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR); //NEAREST);
			Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE); 
			xs = s.Width;
			ys = s.Height;
			while(xs>glmaxtexsize || ys>glmaxtexsize) 
			{ 
				xs /= 2; 
				ys /= 2; 
			};
			IntPtr scaledimg = s.Pixels;
			if(xs!=s.Width)
			{
				Console.WriteLine("warning: quality loss: scaling %s", texname);     // for voodoo cards under linux
				Glu.gluScaleImage(Gl.GL_RGB, s.Width, s.Height, Gl.GL_UNSIGNED_BYTE, s.Pixels, xs, ys, Gl.GL_UNSIGNED_BYTE, scaledimg);
			};
			if(Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, Gl.GL_RGB, xs, ys, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, scaledimg) != 0) 
			{
				GameInit.Fatal("could not build mipmaps");
			}
			if(xs!=s.Width) 
			{
				scaledimg = IntPtr.Zero;
			}
			s.Dispose();
			return true;
		}

		static int[,] mapping= new int[256,MAXFRAMES];                // ( cube texture, frame ) -> ( opengl id, name )
		static string[,] mapname= new string[256,MAXFRAMES];

		public static void Texture(string aframe, string name)
		{
			int num = CurrentTextureNumber++;
			int frame = Convert.ToInt32(aframe);
			if(num<0 || num>=256 || frame<0 || frame>=MAXFRAMES) 
			{
				return;
			}
			mapping[num, frame] = 1;
			mapname[num, frame] = GameInit.NormalizePath(name);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void RenderStripsSky()
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, skyoglid);
			if(false) 
			{
			} 
			else 
			{
				for(int i = 0; i<RenderGl.strips.Count; i++) 
				{
					if(((strip)strips[i]).tex==skyoglid) Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, ((strip)strips[i]).start, ((strip)strips[i]).num);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void PurgeTextures()
		{
			for(int i = 0; i<(256); i++)
			{
				for(int j = 0; j<(RenderGl.MAXFRAMES); j++)
				{
					mapping[i,j] = 0;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tex"></param>
		/// <param name="xs"></param>
		/// <param name="ys"></param>
		/// <returns></returns>
		public static int LookupTexture(int tex, out int xs, out int ys)
		{
			int frame = 0;                      // other frames?
			int tid = mapping[tex,frame];

			if(tid>=FIRSTTEX)
			{
				xs = texx[tid-FIRSTTEX];
				ys = texy[tid-FIRSTTEX];
				return tid;
			}

			xs = ys = 16;
			if(tid == 0) return 1;                  // crosshair :)

			for(int i = 0; i<(CurrentTextureNumber); i++)       // lazily happens once per "texture" command, basically
			{
				if(mapname[tex,frame] == texname[i])
				{
					mapping[tex,frame] = tid = i+FIRSTTEX;
					xs = texx[i];
					ys = texy[i];
					return tid;
				}
			}

			if(CurrentTextureNumber==MAXTEX) GameInit.Fatal("loaded too many textures");

			int tnum = CurrentTextureNumber+FIRSTTEX;
			texname[CurrentTextureNumber] =  mapname[tex, frame];

			string name = "packages" + GameInit.PATHDIV + texname[CurrentTextureNumber];

			if(InstallTexture(tnum, name, out xs, out ys))
			{
				mapping[tex, frame] = tnum;
				texx[CurrentTextureNumber] = xs;
				texy[CurrentTextureNumber] = ys;
				CurrentTextureNumber++;
				return tnum;
			}
			else
			{
				return mapping[tex, frame] = FIRSTTEX;  // temp fix
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static void RenderStrips()
		{
			int lasttex = -1;
			if(false) 
			{
			} 
			else 
			{
				for(int i = 0; i<RenderGl.strips.Count; i++) 
				{
					if(((strip)RenderGl.strips[i]).tex!=RenderGl.skyoglid)
					{
						if(((strip)RenderGl.strips[i]).tex!=lasttex)
						{
							Gl.glBindTexture(Gl.GL_TEXTURE_2D, ((strip)RenderGl.strips[i]).tex); 
							lasttex = ((strip)RenderGl.strips[i]).tex;
						}
						Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, ((strip)RenderGl.strips[i]).start, ((strip)RenderGl.strips[i]).num);  
					}
				}
			}
		}
	}
}
