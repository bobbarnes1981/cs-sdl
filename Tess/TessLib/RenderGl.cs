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
using System.Runtime.InteropServices;

namespace TessLib
{
	public class RenderGl
	{
		static Glu.GLUquadric qsphere;
		static int glmaxtexsize = 256;

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
        
			Bindings.purgetextures();

			qsphere = Glu.gluNewQuadric();
			//if(!(qsphere = Glu.gluNewQuadric())) 
			//{
				//Tess.Fatal("glu sphere");
			//}
			Glu.gluQuadricDrawStyle(qsphere, Glu.GLU_FILL);
			Glu.gluQuadricOrientation(qsphere, Glu.GLU_INSIDE);
			Glu.gluQuadricTexture(qsphere, Gl.GL_TRUE);
			Gl.glNewList(1, Gl.GL_COMPILE);
			Glu.gluSphere(qsphere, 1, 12, 6);
			Gl.glEndList();
		}

		static void CleanGl()
		{
			//if(qsphere) 
			//{
				Glu.gluDeleteQuadric(qsphere);
			//}
		}

//		static void GlDrawFrame(int w, int h, float currentFps)
//		{
//			float hf = hdr.waterlevel-0.3f;
//			float fovy = (float)fov*h/w;
//			float aspect = w/(float)h;
//			bool underwater = player1->o.z<hf;
//    
//			Gl.glFogi(Gl.GL_FOG_START, (fog+64)/8);
//			Gl.glFogi(Gl.GL_FOG_END, fog);
//			float fogc[4] = { (fogcolour>>16)/256.0f, ((fogcolour>>8)&255)/256.0f, (fogcolour&255)/256.0f, 1.0f };
//			Gl.glFogfv(GL_FOG_COLOR, fogc);
//			Gl.glClearColor(fogc[0], fogc[1], fogc[2], 1.0f);
//
//			if(underwater)
//			{
//				fovy += (float)sin(lastmillis/1000.0)*2.0f;
//				aspect += (float)sin(lastmillis/1000.0+PI)*0.1f;
//				Gl.glFogi(Gl.GL_FOG_START, 0);
//				Gl.glFogi(Gl.GL_FOG_END, (fog+96)/8);
//			};
//    
//			Gl.glClear((player1->outsidemap ? Gl.GL_COLOR_BUFFER_BIT : 0) | GL_DEPTH_BUFFER_BIT);
//
//			Gl.glMatrixMode(Gl.GL_PROJECTION);
//			Gl.glLoadIdentity();
//			int farplane = fog*5/2;
//			Gl.gluPerspective(fovy, aspect, 0.15f, farplane);
//			Gl.glMatrixMode(Gl.GL_MODELVIEW);
//
//			transplayer();
//
//			Gl.glEnable(Gl.GL_TEXTURE_2D);
//    
//			int xs, ys;
//			skyoglid = lookuptexture(TextureNumbers.DEFAULT_SKY, xs, ys);
//   
//			resetcubes();
//            
//			curvert = 0;
//			strips.setsize(0);
//  
//			render_world(player1->o.x, player1->o.y, player1->o.z, 
//				(int)player1->yaw, (int)player1->pitch, (float)fov, w, h);
//			finishstrips();
//
//			setupworld();
//
//			renderstripssky();
//
//			Gl.glLoadIdentity();
//			Gl.glRotated(player1->pitch, -1.0, 0.0, 0.0);
//			Gl.glRotated(player1->yaw,   0.0, 1.0, 0.0);
//			Gl.glRotated(90.0, 1.0, 0.0, 0.0);
//			Gl.glColor3f(1.0f, 1.0f, 1.0f);
//			Gl.glDisable(Gl.GL_FOG);
//			Gl.glDepthFunc(Gl.GL_GREATER);
//			draw_envbox(14, fog*4/3);
//			Gl.glDepthFunc(Gl.GL_LESS);
//			Gl.glEnable(Gl.GL_FOG);
//
//			transplayer();
//        
//			overbright(2);
//    
//			renderstrips();
//
//			xtraverts = 0;
//
//			renderclients();
//			monsterrender();
//
//			renderentities();
//
//			renderspheres(curtime);
//			renderents();
//
//			Gl.glDisable(Gl.GL_CULL_FACE);
//
//			drawhudgun(fovy, aspect, farplane);
//
//			overbright(1);
//			int nquads = renderwater(hf);
//    
//			overbright(2);
//			render_particles(curtime);
//			overbright(1);
//
//			Gl.glDisable(Gl.GL_FOG);
//
//			Gl.glDisable(Gl.GL_TEXTURE_2D);
//
//			gl_drawhud(w, h, (int)currentFps, nquads, curvert, underwater);
//
//			Gl.glEnable(Gl.GL_CULL_FACE);
//			Gl.glEnable(Gl.GL_FOG);
//		}

		static void TransPlayer()
		{
			Gl.glLoadIdentity();
    
			Gl.glRotated(TessLib.Main.Player1.roll,0.0,0.0,1.0);
			Gl.glRotated(TessLib.Main.Player1.pitch,-1.0,0.0,0.0);
			Gl.glRotated(TessLib.Main.Player1.yaw,0.0,1.0,0.0);

			Gl.glTranslated(-TessLib.Main.Player1.o.x, (TessLib.Main.Player1.state==(int)CSStatus.CS_DEAD ? TessLib.Main.Player1.eyeheight-0.2f : 0)-TessLib.Main.Player1.o.z, -TessLib.Main.Player1.o.y);   
		}
	}
}
