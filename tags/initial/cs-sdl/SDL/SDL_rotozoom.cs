/*
 * BSD Licence:
 * Copyright (c) 2001, Lloyd Dupont (lloyd@galador.net)
 * <ORGANIZATION> 
 * All rights reserved.
 * 
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright 
 * notice, this list of conditions and the following disclaimer in the 
 * documentation and/or other materials provided with the distribution.
 * 3. Neither the name of the <ORGANIZATION> nor the names of its contributors
 * may be used to endorse or promote products derived from this software
 * without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 */
/*  

    SDL_rotozoom.c

    Copyright (C) A. Schiffler, July 2001

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

*/
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace CsGL.SDL
{
	[StructLayout(LayoutKind.Sequential)]
	struct tColorRGBA
	{
		public byte r;
		public byte g;
		public byte b;
		public byte a;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct tColorY
	{
		public byte y;
	}

	/// <summary> 
	/// 32bit Zoomer with optional anti-aliasing by bilinear interpolation.
	/// Zoomes 32bit RGBA/ABGR 'src' surface to 'dst' surface.
	/// </summary>
	public unsafe abstract class SDL_rotozoom : SDL_video
	{
		/// <summary> 
		/// 32bit Zoomer with optional anti-aliasing by bilinear interpolation.
		/// Zoomes 32bit RGBA/ABGR 'src' surface to 'dst' surface.
		/// </summary> 
		public static int
		zoomSurfaceRGBA (SDL_Surface * src, SDL_Surface * dst, bool smooth)
		{
		  int x, y, sx, sy, csx, csy, ex, ey, t1, t2, sstep;
		  int* csax, csay;
		  tColorRGBA* c00, c01, c10, c11;
		  tColorRGBA* sp, csp, dp;
		  int sgap, dgap, orderRGBA;
		
		  /* Variable setup */
		  if (smooth)
		    {
		      /* For interpolation: assume source dimension is one pixel */
		      /* smaller to avoid overflow on right and bottom edge.     */
		      sx = (int) (65536.0 * (float) (src->w - 1) / (float) dst->w);
		      sy = (int) (65536.0 * (float) (src->h - 1) / (float) dst->h);
		    }
		  else
		    {
		      sx = (int) (65536.0 * (float) src->w / (float) dst->w);
		      sy = (int) (65536.0 * (float) src->h / (float) dst->h);
		    }
		
		  /* Allocate memory for row increments */
		  int * sax = stackalloc int[(int)(dst->w + 1)];
		  int * say = stackalloc int[(int)(dst->h + 1)];
		
		  /* Precalculate row increments */
		  csx = 0;
		  csax = sax;
		  for (x = 0; x <= dst->w; x++)
		    {
		      *csax = csx;
		      csax++;
		      csx &= 0xffff;
		      csx += sx;
		    }
		  csy = 0;
		  csay = say;
		  for (y = 0; y <= dst->h; y++)
		    {
		      *csay = csy;
		      csay++;
		      csy &= 0xffff;
		      csy += sy;
		    }
		
		  /* Pointer setup */
		  sp = csp = (tColorRGBA *) src->pixels;
		  dp = (tColorRGBA *) dst->pixels;
		  sgap = (int) (src->pitch - src->w * 4);
		  dgap = (int) (dst->pitch - dst->w * 4);
		  orderRGBA = (src->format->Rmask == 0x000000ff) ? 1 : 0;
		
		  /* Switch between interpolating and non-interpolating code */
		  if (smooth)
		    {
		
		      /* Interpolating Zoom */
		
		      /* Scan destination */
		      csay = say;
		      for (y = 0; y < dst->h; y++)
			{
			  /* Setup color source pointers */
			  c00 = csp;
			  c01 = csp;
			  c01++;
			  c10 = (tColorRGBA *) ((byte *) csp + src->pitch);
			  c11 = c10;
			  c11++;
			  csax = sax;
			  for (x = 0; x < dst->w; x++)
			    {
			      /* ABGR ordering */
			      /* Interpolate colors */
			      ex = (*csax & 0xffff);
			      ey = (*csay & 0xffff);
			      t1 = ((((c01->r - c00->r) * ex) >> 16) + c00->r) & 0xff;
			      t2 = ((((c11->r - c10->r) * ex) >> 16) + c10->r) & 0xff;
			      dp->r = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
			      t1 = ((((c01->g - c00->g) * ex) >> 16) + c00->g) & 0xff;
			      t2 = ((((c11->g - c10->g) * ex) >> 16) + c10->g) & 0xff;
			      dp->g = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
			      t1 = ((((c01->b - c00->b) * ex) >> 16) + c00->b) & 0xff;
			      t2 = ((((c11->b - c10->b) * ex) >> 16) + c10->b) & 0xff;
			      dp->b = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
			      t1 = ((((c01->a - c00->a) * ex) >> 16) + c00->a) & 0xff;
			      t2 = ((((c11->a - c10->a) * ex) >> 16) + c10->a) & 0xff;
			      dp->a = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
			      /* Advance source pointers */
			      csax++;
			      sstep = (*csax >> 16);
			      c00 += sstep;
			      c01 += sstep;
			      c10 += sstep;
			      c11 += sstep;
			      /* Advance destination pointer */
			      dp++;
			    }
			  /* Advance source pointer */
			  csay++;
			  csp = (tColorRGBA *) ((byte *) csp + (*csay >> 16) * src->pitch);
			  /* Advance destination pointers */
			  dp = (tColorRGBA *) ((byte *) dp + dgap);
			}
		
		    }
		  else
		    {
		
		      /* Non-Interpolating Zoom */
		
		      csay = say;
		      for (y = 0; y < dst->h; y++)
			{
			  sp = csp;
			  csax = sax;
			  for (x = 0; x < dst->w; x++)
			    {
			      /* Draw */
			      *dp = *sp;
			      /* Advance source pointers */
			      csax++;
			      sp += (*csax >> 16);
			      /* Advance destination pointer */
			      dp++;
			    }
			  /* Advance source pointer */
			  csay++;
			  csp = (tColorRGBA *) ((byte *) csp + (*csay >> 16) * src->pitch);
			  /* Advance destination pointers */
			  dp = (tColorRGBA *) ((byte *) dp + dgap);
			}
		
		    }
		
		return (0);
		}
		
		/// <summary> 
		/// 8bit Zoomer without smoothing.
		/// Zoomes 8bit palette/Y 'src' surface to 'dst' surface.
		/// </summary> 
		public static int
		zoomSurfaceY (SDL_Surface * src, SDL_Surface * dst)
		{
		  uint x, y, sx, sy, csx, csy;
		  uint* csax, csay;
		  byte* sp, dp, csp;
		  int dgap;
		
		  /* Variable setup */
		  sx = (uint) (65536.0 * (float) src->w / (float) dst->w);
		  sy = (uint) (65536.0 * (float) src->h / (float) dst->h);
		
		  /* Allocate memory for row increments */
		  uint * sax = stackalloc uint[(int) dst->w];
		  uint * say = stackalloc uint[(int) dst->h];
		
		  /* Precalculate row increments */
		  csx = 0;
		  csax = sax;
		  for (x = 0; x < dst->w; x++)
		    {
		      csx += sx;
		      *csax = (csx >> 16);
		      csx &= 0xffff;
		      csax++;
		    }
		  csy = 0;
		  csay = say;
		  for (y = 0; y < dst->h; y++)
		    {
		      csy += sy;
		      *csay = (csy >> 16);
		      csy &= 0xffff;
		      csay++;
		    }
		
		  csx = 0;
		  csax = sax;
		  for (x = 0; x < dst->w; x++)
		    {
		      csx += (*csax);
		      csax++;
		    }
		  csy = 0;
		  csay = say;
		  for (y = 0; y < dst->h; y++)
		    {
		      csy += (*csay);
		      csay++;
		    }
		
		  /* Pointer setup */
		  sp = csp = (byte *) src->pixels;
		  dp = (byte *) dst->pixels;
		  dgap = (int)( dst->pitch - dst->w );
		
		  /* Draw */
		  csay = say;
		  for (y = 0; y < dst->h; y++)
		    {
		      csax = sax;
		      sp = csp;
		      for (x = 0; x < dst->w; x++)
			{
			  /* Draw */
			  *dp = *sp;
			  /* Advance source pointers */
			  sp += (*csax);
			  csax++;
			  /* Advance destination pointer */
			  dp++;
			}
		      /* Advance source pointer (for row) */
		      csp += ((*csay) * src->pitch);
		      csay++;
		      /* Advance destination pointers */
		      dp += dgap;
		    }
		
		  return (0);
		}
		
		/// <summary> 
		/// 32bit Rotozoomer with optional anti-aliasing by bilinear interpolation.
		/// Rotates and zoomes 32bit RGBA/ABGR 'src' surface to 'dst' surface.
		/// </summary> 
		public static void
		transformSurfaceRGBA (SDL_Surface * src, SDL_Surface * dst, int cx, int cy,
				      int isin, int icos, bool smooth)
		{
		  int x, y, t1, t2, dx, dy, xd, yd, sdx, sdy, ax, ay, ex, ey, sw, sh;
		  tColorRGBA c00 = new tColorRGBA(), 
		             c01 = new tColorRGBA(),
		             c10 = new tColorRGBA(),
		             c11 = new tColorRGBA();
		  tColorRGBA* pc, sp;
		  int gap, orderRGBA;
		
		  /* Variable setup */
		  xd = (int)( (src->w - dst->w) << 15 );
		  yd = (int)( (src->h - dst->h) << 15 );
		  ax = (cx << 16) - (icos * cx);
		  ay = (cy << 16) - (isin * cx);
		  sw = (int)( src->w - 1 );
		  sh = (int)( src->h - 1 );
		  pc = (tColorRGBA*) dst->pixels;
		  gap = (int)( dst->pitch - dst->w * 4 );
		  orderRGBA = (src->format->Rmask == 0x000000ff) ? 1 : 0;
		
		  /* Switch between interpolating and non-interpolating code */
		  if (smooth)
		    {
		      for (y = 0; y < dst->h; y++)
			{
			  dy = cy - y;
			  sdx = (ax + (isin * dy)) + xd;
			  sdy = (ay - (icos * dy)) + yd;
			  for (x = 0; x < dst->w; x++)
			    {
			      dx = (sdx >> 16);
			      dy = (sdy >> 16);
			      if ((dx >= -1) && (dy >= -1) && (dx < src->w) && (dy < src->h))
				{
				  if ((dx >= 0) && (dy >= 0) && (dx < sw) && (dy < sh))
				    {
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      sp += dx;
				      c00 = *sp;
				      sp += 1;
				      c01 = *sp;
				      sp = (tColorRGBA *) ((byte *) sp + src->pitch);
				      sp -= 1;
				      c10 = *sp;
				      sp += 1;
				      c11 = *sp;
				    }
				  else if ((dx == sw) && (dy == sh))
				    {
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      sp += dx;
				      c00 = *sp;
				      c01 = *pc;
				      c10 = *pc;
				      c11 = *pc;
				    }
				  else if ((dx == -1) && (dy == -1))
				    {
				      sp = (tColorRGBA *) (src->pixels);
				      c00 = *pc;
				      c01 = *pc;
				      c10 = *pc;
				      c11 = *sp;
				    }
				  else if ((dx == -1) && (dy == sh))
				    {
				      sp = (tColorRGBA *) (src->pixels);
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      c00 = *pc;
				      c01 = *sp;
				      c10 = *pc;
				      c11 = *pc;
				    }
				  else if ((dx == sw) && (dy == -1))
				    {
				      sp = (tColorRGBA *) (src->pixels);
				      sp += dx;
				      c00 = *pc;
				      c01 = *pc;
				      c10 = *sp;
				      c11 = *pc;
				    }
				  else if (dx == -1)
				    {
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      c00 = *pc;
				      c01 = *sp;
				      c10 = *pc;
				      sp = (tColorRGBA *) ((byte *) sp + src->pitch);
				      c11 = *sp;
				    }
				  else if (dy == -1)
				    {
				      sp = (tColorRGBA *) (src->pixels);
				      sp += dx;
				      c00 = *pc;
				      c01 = *pc;
				      c10 = *sp;
				      sp += 1;
				      c11 = *sp;
				    }
				  else if (dx == sw)
				    {
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      sp += dx;
				      c00 = *sp;
				      c01 = *pc;
				      sp = (tColorRGBA *) ((byte *) sp + src->pitch);
				      c10 = *sp;
				      c11 = *pc;
				    }
				  else if (dy == sh)
				    {
				      sp =
					(tColorRGBA *) ((byte *) src->pixels +
							src->pitch * dy);
				      sp += dx;
				      c00 = *sp;
				      sp += 1;
				      c01 = *sp;
				      c10 = *pc;
				      c11 = *pc;
				    }
				  /* Interpolate colors */
				  ex = (sdx & 0xffff);
				  ey = (sdy & 0xffff);
				  t1 = ((((c01.r - c00.r) * ex) >> 16) + c00.r) & 0xff;
				  t2 = ((((c11.r - c10.r) * ex) >> 16) + c10.r) & 0xff;
				  pc->r = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
				  t1 = ((((c01.g - c00.g) * ex) >> 16) + c00.g) & 0xff;
				  t2 = ((((c11.g - c10.g) * ex) >> 16) + c10.g) & 0xff;
				  pc->g = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
				  t1 = ((((c01.b - c00.b) * ex) >> 16) + c00.b) & 0xff;
				  t2 = ((((c11.b - c10.b) * ex) >> 16) + c10.b) & 0xff;
				  pc->b = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
				  t1 = ((((c01.a - c00.a) * ex) >> 16) + c00.a) & 0xff;
				  t2 = ((((c11.a - c10.a) * ex) >> 16) + c10.a) & 0xff;
				  pc->a = (byte)( (((t2 - t1) * ey) >> 16) + t1 );
		
				}
			      sdx += icos;
			      sdy += isin;
			      pc++;
			    }
			  pc = (tColorRGBA *) ((byte *) pc + gap);
			}
		    }
		  else
		    {
		      for (y = 0; y < dst->h; y++)
			{
			  dy = cy - y;
			  sdx = (ax + (isin * dy)) + xd;
			  sdy = (ay - (icos * dy)) + yd;
			  for (x = 0; x < dst->w; x++)
			    {
			      dx = (short) (sdx >> 16);
			      dy = (short) (sdy >> 16);
			      if ((dx >= 0) && (dy >= 0) && (dx < src->w) && (dy < src->h))
				{
				  sp =
				    (tColorRGBA *) ((byte *) src->pixels + src->pitch * dy);
				  sp += dx;
				  *pc = *sp;
				}
			      sdx += icos;
			      sdy += isin;
			      pc++;
			    }
			  pc = (tColorRGBA *) ((byte *) pc + gap);
			}
		    }
		}
		
		/// <summary> 
		/// 8bit Rotozoomer without smoothing
		/// Rotates and zoomes 8bit palette/Y 'src' surface to 'dst' surface.
		/// </summary> 
		public static void
		transformSurfaceY (SDL_Surface * src, SDL_Surface * dst, int cx, int cy,
				   int isin, int icos)
		{
		  int x, y, dx, dy, xd, yd, sdx, sdy, ax, ay, sw, sh;
		  tColorY* pc, sp;
		  int gap;
		
		  /* Variable setup */
		  xd = (int) ((src->w - dst->w) << 15);
		  yd = (int) ((src->h - dst->h) << 15);
		  ax = (cx << 16) - (icos * cx);
		  ay = (cy << 16) - (isin * cx);
		  sw = (int)( src->w - 1 );
		  sh = (int)( src->h - 1 );
		  pc = (tColorY*) dst->pixels;
		  gap = (int)( dst->pitch - dst->w );
		  /* Clear surface to colorkey */
		  Pointer.memset (pc, (byte) (src->format->colorkey & 0xff),
			  dst->pitch * dst->h);
		  /* Iterate through destination surface */
		  for (y = 0; y < dst->h; y++)
		    {
		      dy = cy - y;
		      sdx = (ax + (isin * dy)) + xd;
		      sdy = (ay - (icos * dy)) + yd;
		      for (x = 0; x < dst->w; x++)
			{
			  dx = (short) (sdx >> 16);
			  dy = (short) (sdy >> 16);
			  if ((dx >= 0) && (dy >= 0) && (dx < src->w) && (dy < src->h))
			    {
			      sp = (tColorY *) (src->pixels);
			      sp += (src->pitch * dy + dx);
			      *pc = *sp;
			    }
			  sdx += icos;
			  sdy += isin;
			  pc++;
			}
		      pc += gap;
		    }
		}
		
		
		const double VALUE_LIMIT = 0.001;
		
		/// <summary>
		/// rotozoomSurface()
		/// Rotates and zoomes a 32bit or 8bit 'src' surface to newly created 'dst' surface.
		/// 'angle' is the rotation in degrees. 'zoom' a scaling factor. If 'smooth' is 1
		/// then the destination 32bit surface is anti-aliased. If the surface is not 8bit
		/// or 32bit RGBA/ABGR it will be converted into a 32bit RGBA format on the fly.
		/// </summary>
		public static SDL_Surface *
		rotozoomSurface (SDL_Surface * src, double angle, double zoom, bool smooth)
		{
		  SDL_Surface *rz_src;
		  SDL_Surface *rz_dst;
		  double zoominv;
		  double radangle, sanglezoom, canglezoom, sanglezoominv, canglezoominv;
		  int dstwidthhalf, dstwidth, dstheighthalf, dstheight;
		  double x, y, cx, cy, sx, sy;
		  bool is32bit, src_converted;
		  int i;
		
		  /* Sanity check */
		  if (src == null)
		    return (null);
		
		  /* Determine if source surface is 32bit or 8bit */
		  is32bit = (src->format->BitsPerPixel == 32);
		  if ((is32bit) || (src->format->BitsPerPixel == 8))
		    {
		      /* Use source surface 'as is' */
		      rz_src = src;
		      src_converted = false;
		    }
		  else
		    {
		      /* New source surface is 32bit with a defined RGBA ordering */
		      rz_src =
			SDL_CreateRGBSurface (SDL_SWSURFACE, (int)src->w, (int)src->h, 32, 0x000000ff,
					      0x0000ff00, 0x00ff0000, 0xff000000);
		      SDL_BlitSurface (src, null, rz_src, null);
		      src_converted = true;
		      is32bit = true;
		    }
		
		  /* Sanity check zoom factor */
		  if (zoom < VALUE_LIMIT)
		    {
		      zoom = VALUE_LIMIT;
		    }
		  zoominv = 65536.0 / zoom;
		
		  /* Check if we have a rotozoom or just a zoom */
		  if (Math.Abs(angle) > VALUE_LIMIT)
		    {
		
		      /* Angle!=0: full rotozoom */
		      /* ----------------------- */
		
		      /* Calculate target factors from sin/cos and zoom */
		      radangle = angle * (Math.PI / 180.0);
		      sanglezoom = sanglezoominv = Math.Sin (radangle);
		      canglezoom = canglezoominv = Math.Cos (radangle);
		      sanglezoom *= zoom;
		      canglezoom *= zoom;
		      sanglezoominv *= zoominv;
		      canglezoominv *= zoominv;
		
		      /* Determine destination width and height by rotating a centered source box */
		      x = rz_src->w / 2;
		      y = rz_src->h / 2;
		      cx = canglezoom * x;
		      cy = canglezoom * y;
		      sx = sanglezoom * x;
		      sy = sanglezoom * y;
		      dstwidthhalf =
			Math.Max((int)
			     Math.Ceiling(Math.Max
				   (Math.Max
				    (Math.Max(Math.Abs(cx + sy), Math.Abs(cx - sy)), Math.Abs(-cx + sy)),
				    Math.Abs(-cx - sy))), 1);
		      dstheighthalf =
			Math.Max((int)
			     Math.Ceiling(Math.Max
				   (Math.Max
				    (Math.Max(Math.Abs(sx + cy), Math.Abs(sx - cy)), Math.Abs(-sx + cy)),
				    Math.Abs(-sx - cy))), 1);
		      dstwidth = 2 * dstwidthhalf;
		      dstheight = 2 * dstheighthalf;
		
		      /* Alloc space to completely contain the rotated surface */
		      rz_dst = null;
		      if (is32bit)
			{
			  /* Target surface is 32bit with source RGBA/ABGR ordering */
			  rz_dst =
			    SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 32,
						  rz_src->format->Rmask,
						  rz_src->format->Gmask,
						  rz_src->format->Bmask,
						  rz_src->format->Amask);
			}
		      else
			{
			  /* Target surface is 8bit */
			  rz_dst =
			    SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 8, 0, 0,
						  0, 0);
			}
		
		      /* Lock source surface */
		      SDL_LockSurface (rz_src);
		      /* Check which kind of surface we have */
		      if (is32bit)
			{
			  /* Call the 32bit transformation routine to do the rotation (using alpha) */
			  transformSurfaceRGBA (rz_src, rz_dst, dstwidthhalf, dstheighthalf,
						(int) (sanglezoominv),
						(int) (canglezoominv), smooth);
			  /* Turn on source-alpha support */
			  SDL_SetAlpha (rz_dst, SDL_SRCALPHA, 255);
			}
		      else
			{
			  /* Copy palette and colorkey info */
			  for (i = 0; i < rz_src->format->palette->ncolors; i++)
			    {
			      rz_dst->format->palette->colors[i] =
				rz_src->format->palette->colors[i];
			    }
			  rz_dst->format->palette->ncolors = rz_src->format->palette->ncolors;
			  /* Call the 8bit transformation routine to do the rotation */
			  transformSurfaceY (rz_src, rz_dst, dstwidthhalf, dstheighthalf,
					     (int) (sanglezoominv), (int) (canglezoominv));
			  SDL_SetColorKey (rz_dst, SDL_SRCCOLORKEY | SDL_RLEACCEL,
					   rz_src->format->colorkey);
			}
		      /* Unlock source surface */
		      SDL_UnlockSurface (rz_src);
		
		    }
		  else
		    {
		
		      /* Angle=0: Just a zoom */
		      /* -------------------- */
		
		      /* Calculate target size and set rect */
		      dstwidth = (int) ((double) rz_src->w * zoom);
		      dstheight = (int) ((double) rz_src->h * zoom);
		      if (dstwidth < 1)
			{
			  dstwidth = 1;
			}
		      if (dstheight < 1)
			{
			  dstheight = 1;
			}
		
		      /* Alloc space to completely contain the zoomed surface */
		      rz_dst = null;
		      if (is32bit)
			{
			  /* Target surface is 32bit with source RGBA/ABGR ordering */
			  rz_dst =
			    SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 32,
						  rz_src->format->Rmask,
						  rz_src->format->Gmask,
						  rz_src->format->Bmask,
						  rz_src->format->Amask);
			}
		      else
			{
			  /* Target surface is 8bit */
			  rz_dst =
			    SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 8, 0, 0,
						  0, 0);
			}
		
		      /* Lock source surface */
		      SDL_LockSurface (rz_src);
		      /* Check which kind of surface we have */
		      if (is32bit)
			{
			  /* Call the 32bit transformation routine to do the zooming (using alpha) */
			  zoomSurfaceRGBA (rz_src, rz_dst, smooth);
			  /* Turn on source-alpha support */
			  SDL_SetAlpha (rz_dst, SDL_SRCALPHA, 255);
			}
		      else
			{
			  /* Copy palette and colorkey info */
			  for (i = 0; i < rz_src->format->palette->ncolors; i++)
			    {
			      rz_dst->format->palette->colors[i] =
				rz_src->format->palette->colors[i];
			    }
			  rz_dst->format->palette->ncolors = rz_src->format->palette->ncolors;
			  /* Call the 8bit transformation routine to do the zooming */
			  zoomSurfaceY (rz_src, rz_dst);
			  SDL_SetColorKey (rz_dst, SDL_SRCCOLORKEY | SDL_RLEACCEL,
					   rz_src->format->colorkey);
			}
		      /* Unlock source surface */
		      SDL_UnlockSurface (rz_src);
		    }
		
		  /* Cleanup temp surface */
		  if (src_converted)
		    {
		      SDL_FreeSurface (rz_src);
		    }
		
		  /* Return destination surface */
		  return (rz_dst);
		}
		
		/// <summary>
		/// zoomSurface()
		///
		/// Zoomes a 32bit or 8bit 'src' surface to newly created 'dst' surface.
		/// 'zoomx' and 'zoomy' are scaling factors for width and height. If 'smooth' is 1
		/// then the destination 32bit surface is anti-aliased. If the surface is not 8bit
		/// or 32bit RGBA/ABGR it will be converted into a 32bit RGBA format on the fly.
		/// </summary>
		public static SDL_Surface *
		zoomSurface (SDL_Surface * src, double zoomx, double zoomy, bool smooth)
		{
		  SDL_Surface *rz_src;
		  SDL_Surface *rz_dst;
		  int dstwidth, dstheight;
		  bool is32bit, src_converted;
		  int i;
		
		  /* Sanity check */
		  if (src == null)
		    return (null);
		
		  /* Determine if source surface is 32bit or 8bit */
		  is32bit = (src->format->BitsPerPixel == 32);
		  if ((is32bit) || (src->format->BitsPerPixel == 8))
		    {
		      /* Use source surface 'as is' */
		      rz_src = src;
		      src_converted = false;
		    }
		  else
		    {
		      /* New source surface is 32bit with a defined RGBA ordering */
		      rz_src =
			SDL_CreateRGBSurface (SDL_SWSURFACE, (int)src->w, (int)src->h, 32, 0x000000ff,
					      0x0000ff00, 0x00ff0000, 0xff000000);
		      SDL_BlitSurface (src, null, rz_src, null);
		      src_converted = true;
		      is32bit = true;
		    }
		
		  /* Sanity check zoom factors */
		  if (zoomx < VALUE_LIMIT)
		    {
		      zoomx = VALUE_LIMIT;
		    }
		  if (zoomy < VALUE_LIMIT)
		    {
		      zoomy = VALUE_LIMIT;
		    }
		
		  /* Calculate target size and set rect */
		  dstwidth = (int) ((double) rz_src->w * zoomx);
		  dstheight = (int) ((double) rz_src->h * zoomy);
		  if (dstwidth < 1)
		    {
		      dstwidth = 1;
		    }
		  if (dstheight < 1)
		    {
		      dstheight = 1;
		    }
		
		  /* Alloc space to completely contain the zoomed surface */
		  rz_dst = null;
		  if (is32bit)
		    {
		      /* Target surface is 32bit with source RGBA/ABGR ordering */
		      rz_dst =
			SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 32,
					      rz_src->format->Rmask, rz_src->format->Gmask,
					      rz_src->format->Bmask, rz_src->format->Amask);
		    }
		  else
		    {
		      /* Target surface is 8bit */
		      rz_dst =
			SDL_CreateRGBSurface (SDL_SWSURFACE, dstwidth, dstheight, 8, 0, 0, 0,
					      0);
		    }
		
		  /* Lock source surface */
		  SDL_LockSurface (rz_src);
		  /* Check which kind of surface we have */
		  if (is32bit)
		    {
		      /* Call the 32bit transformation routine to do the zooming (using alpha) */
		      zoomSurfaceRGBA (rz_src, rz_dst, smooth);
		      /* Turn on source-alpha support */
		      SDL_SetAlpha (rz_dst, SDL_SRCALPHA, 255);
		    }
		  else
		    {
		      /* Copy palette and colorkey info */
		      for (i = 0; i < rz_src->format->palette->ncolors; i++)
			{
			  rz_dst->format->palette->colors[i] =
			    rz_src->format->palette->colors[i];
			}
		      rz_dst->format->palette->ncolors = rz_src->format->palette->ncolors;
		      /* Call the 8bit transformation routine to do the zooming */
		      zoomSurfaceY (rz_src, rz_dst);
		      SDL_SetColorKey (rz_dst, SDL_SRCCOLORKEY | SDL_RLEACCEL,
				       rz_src->format->colorkey);
		    }
		  /* Unlock source surface */
		  SDL_UnlockSurface (rz_src);
		
		  /* Cleanup temp surface */
		  if (src_converted)
		    {
		      SDL_FreeSurface (rz_src);
		    }
		
		  /* Return destination surface */
		  return (rz_dst);
		}
	}
}
