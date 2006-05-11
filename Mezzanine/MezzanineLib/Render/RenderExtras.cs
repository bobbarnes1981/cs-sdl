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

namespace MezzanineLib.Render
{
	/// <summary>
	/// Summary description for RenderExtras.
	/// </summary>
	public class RenderExtras
	{
		static RenderExtras()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public const int MAXSPHERES = 50;
		public static bool sinit = false;

		static int dBlend = 0;
		/// <summary>
		/// 
		/// </summary>
		public static int DBlend
		{
			get
			{
				return dBlend;
			}
			set
			{
				dBlend = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="n"></param>
		public static void DamageBlend(int n) 
		{ 
			dBlend += n; 
		}

		// stupid function to cater for stupid ATI linux drivers that return incorrect depth values

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <returns></returns>
		public static float DepthCorrect(float d)
		{
			return (d<=1/256.0f) ? d*256 : d;
		}

		/// <summary>
		/// 
		/// </summary>
		public static string[] EntityNames =
		{
			"none?", "light", "playerstart",
			"shells", "bullets", "rockets", "riflerounds",
			"health", "healthboost", "greenarmour", "yellowarmour", "quaddamage", 
			"teleport", "teledest", 
			"mapmodel", "monster", "trigger", "jumppad",
			"?", "?", "?", "?", "?"
		};

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tx"></param>
		/// <param name="ty"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static void DrawIcon(float tx, float ty, int x, int y)
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 5);
			Gl.glBegin(Gl.GL_QUADS);
			tx /= 192;
			ty /= 192;
			float o = 1/3.0f;
			int s = 120;
			Gl.glTexCoord2f(tx,   ty);   
			Gl.glVertex2i(x,   y);
			Gl.glTexCoord2f(tx+o, ty);   
			Gl.glVertex2i(x+s, y);
			Gl.glTexCoord2f(tx+o, ty+o); 
			Gl.glVertex2i(x+s, y+s);
			Gl.glTexCoord2f(tx,   ty+o); 
			Gl.glVertex2i(x,   y+s);
			Gl.glEnd();
			RenderGl.XtraVerts += 4;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="z1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="z2"></param>
		public static void Line(int x1, int y1, float z1, int x2, int y2, float z2)
		{
			Gl.glBegin(Gl.GL_POLYGON);
			Gl.glVertex3f((float)x1, z1, (float)y1);
			Gl.glVertex3f((float)x1, z1, y1+0.01f);
			Gl.glVertex3f((float)x2, z2, y2+0.01f);
			Gl.glVertex3f((float)x2, z2, (float)y2);
			Gl.glEnd();
			RenderGl.XtraVerts += 4;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="border"></param>
		public static void BlendBox(int x1, int y1, int x2, int y2, bool border)
		{
			Gl.glDepthMask(Gl.GL_FALSE);
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glBlendFunc(Gl.GL_ZERO, Gl.GL_ONE_MINUS_SRC_COLOR);
			Gl.glBegin(Gl.GL_QUADS);
			if(border) 
			{
				Gl.glColor3d(0.5, 0.3, 0.4); 
			}
			else 
			{
				Gl.glColor3d(1.0, 1.0, 1.0);
			}
			Gl.glVertex2i(x1, y1);
			Gl.glVertex2i(x2, y1);
			Gl.glVertex2i(x2, y2);
			Gl.glVertex2i(x1, y2);
			Gl.glEnd();
			Gl.glDisable(Gl.GL_BLEND);
			Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
			Gl.glBegin(Gl.GL_POLYGON);
			Gl.glColor3d(0.2, 0.7, 0.4); 
			Gl.glVertex2i(x1, y1);
			Gl.glVertex2i(x2, y1);
			Gl.glVertex2i(x2, y2);
			Gl.glVertex2i(x1, y2);
			Gl.glEnd();
			Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);
			RenderGl.XtraVerts += 8;
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glDepthMask(Gl.GL_TRUE);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public static void Dot(int x, int y, float z)
		{
			const float DOF = 0.1f;
			Gl.glBegin(Gl.GL_POLYGON);
			Gl.glVertex3f(x-DOF, (float)z, y-DOF);
			Gl.glVertex3f(x+DOF, (float)z, y-DOF);
			Gl.glVertex3f(x+DOF, (float)z, y+DOF);
			Gl.glVertex3f(x-DOF, (float)z, y+DOF);
			Gl.glEnd();
			RenderGl.XtraVerts += 4;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="width"></param>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		public static void LineStyle(float width, int r, int g, int b)
		{
			Gl.glLineWidth(width);
			Gl.glColor3ub((byte)r,(byte)g,(byte)b);
		}
	}
}
