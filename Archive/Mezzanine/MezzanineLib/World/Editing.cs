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
using System.IO;
using Tao.Sdl;
using Tao.OpenGl;
using System.Runtime.InteropServices;

namespace MezzanineLib.World
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Editing
	{
		public static int selh = 0;
		public static bool selset = false;
		public static int cx;
		public static int cy;
		public static int ch;
		public static bool dragging = false;
		public static int lastx;
		public static int lasty;
		public static int lasth;
		public static int lasttype = 0;
		public static int lasttex = 0;
		public const int MAXARCHVERT = 50;
		public static bool archvinit = false;

        //// editing.cpp: most map editing commands go here, entity editing commands are in world.cpp
        //
        //#include "cube.h"
        //#using <mscorlib.dll>
        //using namespace MezzanineLib;
        //using namespace MezzanineLib::World;
        //
        //// the current selection, used by almost all editing commands
        //// invariant: all code assumes that these are kept inside MINBORD distance of the edge of the map
        //
        //block sel =
        //{
        //    variable("selx",  0, 0, 4096, &sel.x,  NULL, false),
        //    variable("sely",  0, 0, 4096, &sel.y,  NULL, false),
        //    variable("selxs", 0, 0, 4096, &sel.xs, NULL, false),
        //    variable("selys", 0, 0, 4096, &sel.ys, NULL, false),
        //};
        //
        //#define loopselxy(b) { makeundo(); loop(x,sel.xs) loop(y,sel.ys) { sqr *s = S(sel.x+x, sel.y+y); b; }; remip(sel); }
        //int curedittex[] = { -1, -1, -1 };
        //sqr rtex;
        //
        //VAR(editing,0,0,1);
        //
        //void toggleedit()
        //{
        //    if(player1->state==CSStatus::CS_DEAD) return;                 // do not allow dead players to edit to avoid state confusion
        //    if(!GameInit::EditMode && !allowedittoggle()) return;         // not in most multiplayer modes
        //    GameInit::EditMode = !GameInit::EditMode;
        //	if(!GameInit::EditMode)
        //    {
        //        settagareas();                                  // reset triggers to allow quick playtesting
        //        entinmap(player1);                              // find spawn closest to current floating pos
        //    }
        //    else
        //    {
        //        resettagareas();                                // clear trigger areas to allow them to be edited
        //        player1->health = 100;
        //        if(m_classicsp) monsterclear();                 // all monsters back at their spawns for editing
        //        projreset();
        //    };
        //	SdlDotNet::Input::Keyboard::KeyRepeat = GameInit::EditMode;
        //    Editing::selset = false;
        //    editing = GameInit::EditMode;
        //};
        //
        //COMMANDN(edittoggle, toggleedit, Support::FunctionSignatures::ARG_NONE);
        //
        //void correctsel()                                       // ensures above invariant
        //{
        //    Editing::selset = !OUTBORD(sel.x, sel.y);
        //    int bsize = GameInit::SSize-GameInit::MinBord;
        //    if(sel.xs+sel.x>bsize) sel.xs = bsize-sel.x;
        //    if(sel.ys+sel.y>bsize) sel.ys = bsize-sel.y;
        //    if(sel.xs<=0 || sel.ys<=0) Editing::selset = false;
        //};
        //
        //bool noteditmode()
        //{
        //    correctsel();
        //    if(!GameInit::EditMode) conoutf("this function is only allowed in edit mode");
        //    return !GameInit::EditMode;
        //};
        //
        //bool noselection()
        //{
        //    if(!Editing::selset) conoutf("no selection");
        //    return !Editing::selset;
        //};
        //
        //#define EDITSEL   if(noteditmode() || noselection()) return;
        //#define EDITSELMP if(noteditmode() || noselection() || multiplayer()) return;
        //#define EDITMP    if(noteditmode() || multiplayer()) return;
        //
        //void selectpos(int x, int y, int xs, int ys)
        //{
        //    block s = { x, y, xs, ys };
        //    sel = s;
        //    Editing::selh = 0;
        //    correctsel();
        //};
        //
        //void makesel()
        //{
        //    block s = { min(Editing::lastx,Editing::cx), min(Editing::lasty,Editing::cy), abs(Editing::lastx-Editing::cx)+1, abs(Editing::lasty-Editing::cy)+1 };
        //    sel = s;
        //    Editing::selh = max(Editing::lasth,Editing::ch);
        //    correctsel();
        //    if(Editing::selset) rtex = *S(sel.x, sel.y);
        //};
        //
        //VAR(flrceil,0,0,2);
        //
        //float sheight(sqr *s, sqr *t, float z)                  // finds out z height when cursor points at wall
        //{
        //    return !flrceil //z-s->floor<s->ceil-z
        //        ? (s->type==BlockTypes::FHF ? s->floor-t->vdelta/4.0f : (float)s->floor)
        //        : (s->type==BlockTypes::CHF ? s->ceil+t->vdelta/4.0f : (float)s->ceil);
        //};
        //
        //void cursorupdate()                                     // called every frame from hud
        //{
        //    flrceil = ((int)(player1->pitch>=0))*2;
        //
        //    volatile float x = worldpos.x;                      // volatile needed to prevent msvc7 optimizer bug?
        //    volatile float y = worldpos.y;
        //    volatile float z = worldpos.z;
        //    
        //    Editing::cx = (int)x;
        //    Editing::cy = (int)y;
        //
        //    if(OUTBORD(Editing::cx, Editing::cy)) return;
        //    sqr *s = S(Editing::cx,Editing::cy);
        //    
        //    if(fabs(sheight(s,s,z)-z)>1)                        // selected wall
        //    {
        //        x += x>player1->o.x ? 0.5f : -0.5f;             // find right wall cube
        //        y += y>player1->o.y ? 0.5f : -0.5f;
        //
        //        Editing::cx = (int)x;
        //        Editing::cy = (int)y;
        //
        //        if(OUTBORD(Editing::cx, Editing::cy)) return;
        //    };
        //        
        //    if(Editing::dragging) makesel();
        //
        //    const int GRIDSIZE = 5;
        //    const float GRIDW = 0.5f;
        //    const float GRID8 = 2.0f;
        //    const float GRIDS = 2.0f;
        //    const int GRIDM = 0x7;
        //    
        //    // render editing grid
        //
        //    for(int ix = Editing::cx-GRIDSIZE; ix<=Editing::cx+GRIDSIZE; ix++) for(int iy = Editing::cy-GRIDSIZE; iy<=Editing::cy+GRIDSIZE; iy++)
        //    {
        //        if(OUTBORD(ix, iy)) continue;
        //        sqr *s = S(ix,iy);
        //        if(SOLID(s)) continue;
        //        float h1 = sheight(s, s, z);
        //        float h2 = sheight(s, SWS(s,1,0,GameInit::SSize), z);
        //        float h3 = sheight(s, SWS(s,1,1,GameInit::SSize), z);
        //        float h4 = sheight(s, SWS(s,0,1,GameInit::SSize), z);
        //		if(s->tag) Render::RenderExtras::LineStyle(GRIDW, 0xFF, 0x40, 0x40);
        //		else if(s->type==BlockTypes::FHF || s->type==BlockTypes::CHF) Render::RenderExtras::LineStyle(GRIDW, 0x80, 0xFF, 0x80);
        //        else Render::RenderExtras::LineStyle(GRIDW, 0x80, 0x80, 0x80);
        //        block b = { ix, iy, 1, 1 };
        //        box(b, h1, h2, h3, h4);
        //		Render::RenderExtras::LineStyle(GRID8, 0x40, 0x40, 0xFF);
        //        if(!(ix&GRIDM))   Render::RenderExtras::Line(ix,   iy,   h1, ix,   iy+1, h4);
        //        if(!(ix+1&GRIDM)) Render::RenderExtras::Line(ix+1, iy,   h2, ix+1, iy+1, h3);
        //        if(!(iy&GRIDM))   Render::RenderExtras::Line(ix,   iy,   h1, ix+1, iy,   h2);
        //        if(!(iy+1&GRIDM)) Render::RenderExtras::Line(ix,   iy+1, h4, ix+1, iy+1, h3);
        //    };
        //
        //    if(!SOLID(s))
        //    {
        //        float ih = sheight(s, s, z);
        //		Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0xFF, 0xFF);
        //        block b = { Editing::cx, Editing::cy, 1, 1 };
        //        box(b, ih, sheight(s, SWS(s,1,0,GameInit::SSize), z), sheight(s, SWS(s,1,1,GameInit::SSize), z), sheight(s, SWS(s,0,1,GameInit::SSize), z));
        //		Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0x00, 0x00);
        //        Render::RenderExtras::Dot(Editing::cx, Editing::cy, ih);
        //        Editing::ch = (int)ih;
        //    };
        //
        //    if(Editing::selset)
        //    {
        //		Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0x40, 0x40);
        //        box(sel, (float)Editing::selh, (float)Editing::selh, (float)Editing::selh, (float)Editing::selh);
        //    };
        //};
        //
        //vector<block *> undos;                                  // unlimited undo
        //VARP(undomegs, 0, 1, 10);                                // bounded by n megs
        //
        //void pruneundos(int maxremain)                          // bound memory
        //{
        //    int t = 0;
        //    loopvrev(undos)
        //    {
        //        t += undos[i]->xs*undos[i]->ys*sizeof(sqr);
        //        if(t>maxremain) free(undos.remove(i));
        //    };
        //};
        //
        //void makeundo()
        //{
        //    undos.add(blockcopy(sel));
        //    pruneundos(undomegs<<20);
        //};
        //
        //void editundo()
        //{
        //    EDITMP;
        //    if(undos.empty()) { conoutf("nothing more to undo"); return; };
        //    block *p = undos.pop();
        //    blockpaste(*p);
        //    free(p);
        //};
        //
        //block *copybuf = NULL;
        //
        //void copy()
        //{
        //    EDITSELMP;
        //    if(copybuf) free(copybuf);
        //    copybuf = blockcopy(sel);
        //};
        //
        //void paste()
        //{
        //    EDITMP;
        //    if(!copybuf) { conoutf("nothing to paste"); return; };
        //    sel.xs = copybuf->xs;
        //    sel.ys = copybuf->ys;
        //    correctsel();
        //    if(!Editing::selset || sel.xs!=copybuf->xs || sel.ys!=copybuf->ys) { conoutf("incorrect selection"); return; };
        //    makeundo();
        //    copybuf->x = sel.x;
        //    copybuf->y = sel.y;
        //    blockpaste(*copybuf);
        //};
        //
        //void tofronttex()                                       // maintain most recently used of the texture lists when applying texture
        //{
        //    loopi(3)
        //    {
        //        int c = curedittex[i];
        //        if(c>=0)
        //        {
        //            uchar *p = hdr.texlists[i];
        //            int t = p[c];
        //            for(int a = c-1; a>=0; a--) p[a+1] = p[a];
        //            p[0] = t;
        //            curedittex[i] = -1;
        //        };
        //    };
        //};
        //
        //void editdrag(bool isdown)
        //{
        //    if(Editing::dragging = isdown)
        //    {
        //        Editing::lastx = Editing::cx;
        //        Editing::lasty = Editing::cy;
        //        Editing::lasth = Editing::ch;
        //        Editing::selset = false;
        //        tofronttex();
        //    };
        //    makesel();
        //};
        //
        //// the core editing function. all the *xy functions perform the core operations
        //// and are also called directly from the network, the function below it is strictly
        //// triggered locally. They all have very similar structure.
        //
        //void editheightxy(bool isfloor, int amount, block &sel)
        //{
        //    loopselxy(if(isfloor)
        //    {
        //        s->floor += amount;
        //        if(s->floor>=s->ceil) s->floor = s->ceil-1;
        //    }
        //    else
        //    {
        //        s->ceil += amount;
        //        if(s->ceil<=s->floor) s->ceil = s->floor+1;
        //    });
        //};
        //
        //void editheight(int flr, int amount)
        //{
        //    EDITSEL;
        //    bool isfloor = flr==0;
        //    editheightxy(isfloor, amount, sel);
        //    addmsg(1, 7, NetworkMessages::SV_EDITH, sel.x, sel.y, sel.xs, sel.ys, isfloor, amount);
        //};
        //
        //COMMAND(editheight, Support::FunctionSignatures::ARG_2INT);
        //
        //void edittexxy(int type, int t, block &sel)            
        //{
        //    loopselxy(switch(type)
        //    {
        //        case 0: s->ftex = t; break;
        //        case 1: s->wtex = t; break;
        //        case 2: s->ctex = t; break;
        //        case 3: s->utex = t; break;
        //    });
        //};
        //
        //void edittex(int type, int dir)
        //{
        //    EDITSEL;
        //    if(type<0 || type>3) return;
        //    if(type!=Editing::lasttype) { tofronttex(); Editing::lasttype = type; };
        //    int atype = type==3 ? 1 : type;
        //    int i = curedittex[atype];
        //    i = i<0 ? 0 : i+dir;
        //    curedittex[atype] = i = min(max(i, 0), 255);
        //    int t = Editing::lasttex = hdr.texlists[atype][i];
        //    edittexxy(type, t, sel);
        //    addmsg(1, 7, NetworkMessages::SV_EDITT, sel.x, sel.y, sel.xs, sel.ys, type, t);
        //};
        //
        //void replace()
        //{
        //    EDITSELMP;
        //    loop(x,GameInit::SSize) loop(y,GameInit::SSize)
        //    {
        //        sqr *s = S(x, y);
        //        switch(Editing::lasttype)
        //        {
        //            case 0: if(s->ftex == rtex.ftex) s->ftex = Editing::lasttex; break;
        //            case 1: if(s->wtex == rtex.wtex) s->wtex = Editing::lasttex; break;
        //            case 2: if(s->ctex == rtex.ctex) s->ctex = Editing::lasttex; break;
        //            case 3: if(s->utex == rtex.utex) s->utex = Editing::lasttex; break;
        //        };
        //    };
        //    block b = { 0, 0, GameInit::SSize, GameInit::SSize }; 
        //    remip(b);
        //};
        //
        //void edittypexy(int type, block &sel)
        //{
        //    loopselxy(s->type = type);
        //};
        //
        //void edittype(int type)
        //{
        //    EDITSEL;
        //    if(type==BlockTypes::CORNER && (sel.xs!=sel.ys || sel.xs==3 || sel.xs>4 && sel.xs!=8
        //                   || sel.x&~-sel.xs || sel.y&~-sel.ys))
        //                   { conoutf("corner selection must be power of 2 aligned"); return; };
        //    edittypexy(type, sel);
        //    addmsg(1, 6, NetworkMessages::SV_EDITS, sel.x, sel.y, sel.xs, sel.ys, type);
        //};
        //
        //void heightfield(int t) { edittype(t==0 ? BlockTypes::FHF : BlockTypes::CHF); };
        //void solid(int t)       { edittype(t==0 ? BlockTypes::SPACE : BlockTypes::SOLID); };
        //void corner()           { edittype(BlockTypes::CORNER); };
        //
        //COMMAND(heightfield, Support::FunctionSignatures::ARG_1INT);
        //COMMAND(solid, Support::FunctionSignatures::ARG_1INT);
        //COMMAND(corner, Support::FunctionSignatures::ARG_NONE);
        //
        //void editequalisexy(bool isfloor, block &sel)
        //{
        //    int low = 127, hi = -128;
        //    loopselxy(
        //    {
        //        if(s->floor<low) low = s->floor;
        //        if(s->ceil>hi) hi = s->ceil;
        //    });
        //    loopselxy(
        //    {
        //        if(isfloor) s->floor = low; else s->ceil = hi;
        //        if(s->floor>=s->ceil) s->floor = s->ceil-1;
        //    });
        //};
        //
        //void equalize(int flr)
        //{
        //    bool isfloor = flr==0;
        //    EDITSEL;
        //    editequalisexy(isfloor, sel);
        //    addmsg(1, 6, NetworkMessages::SV_EDITE, sel.x, sel.y, sel.xs, sel.ys, isfloor);
        //};
        //
        //COMMAND(equalize, Support::FunctionSignatures::ARG_1INT);
        //
        //void setvdeltaxy(int delta, block &sel)
        //{
        //    loopselxy(s->vdelta = max(s->vdelta+delta, 0));
        //    remipmore(sel);    
        //};
        //
        //void setvdelta(int delta)
        //{
        //    EDITSEL;
        //    setvdeltaxy(delta, sel);
        //    addmsg(1, 6, NetworkMessages::SV_EDITD, sel.x, sel.y, sel.xs, sel.ys, delta);
        //};
        //
        //int archverts[Editing::MAXARCHVERT][Editing::MAXARCHVERT];
        //
        //void archvertex(int span, int vert, int delta)
        //{
        //    if(!Editing::archvinit)
        //    {
        //        Editing::archvinit = true;
        //        loop(s,Editing::MAXARCHVERT) loop(v,Editing::MAXARCHVERT) archverts[s][v] = 0;
        //    };
        //    if(span>=Editing::MAXARCHVERT || vert>=Editing::MAXARCHVERT || span<0 || vert<0) return;
        //    archverts[span][vert] = delta;
        //};
        //
        //void arch(int sidedelta, int _a)
        //{
        //    EDITSELMP;
        //    sel.xs++;
        //    sel.ys++;
        //    if(sel.xs>Editing::MAXARCHVERT) sel.xs = Editing::MAXARCHVERT;
        //    if(sel.ys>Editing::MAXARCHVERT) sel.ys = Editing::MAXARCHVERT;
        //    loopselxy(s->vdelta =
        //        sel.xs>sel.ys
        //            ? (archverts[sel.xs-1][x] + (y==0 || y==sel.ys-1 ? sidedelta : 0))
        //            : (archverts[sel.ys-1][y] + (x==0 || x==sel.xs-1 ? sidedelta : 0)));
        //    remipmore(sel);
        //};
        //
        //void slope(int xd, int yd)
        //{
        //    EDITSELMP;
        //    int off = 0;
        //    if(xd<0) off -= xd*sel.xs;
        //    if(yd<0) off -= yd*sel.ys;
        //    sel.xs++;
        //    sel.ys++;
        //    loopselxy(s->vdelta = xd*x+yd*y+off);
        //    remipmore(sel);
        //};
        //
        //
        //VARF(fullbright, 0, 0, 1,
        //    if(fullbright)
        //    {
        //        if(noteditmode()) return;
        //        loopi(GameInit::MipSize) world[i].r = world[i].g = world[i].b = 176;
        //    };
        //);
        //
        //void edittag(int tag)
        //{
        //    EDITSELMP;
        //    loopselxy(s->tag = tag);
        //};
        //
        //void newent(char *what, char *a1, char *a2, char *a3, char *a4)
        //{
        //    EDITSEL;
        //    newentity(sel.x, sel.y, (int)player1->o.z, what, ATOI(a1), ATOI(a2), ATOI(a3), ATOI(a4));
        //};
        //
        //COMMANDN(select, selectpos, Support::FunctionSignatures::ARG_4INT);
        //COMMAND(edittag, Support::FunctionSignatures::ARG_1INT);
        //COMMAND(replace, Support::FunctionSignatures::ARG_NONE);
        //COMMAND(archvertex, Support::FunctionSignatures::ARG_3INT);
        //COMMAND(arch, Support::FunctionSignatures::ARG_2INT);
        //COMMAND(slope, Support::FunctionSignatures::ARG_2INT);
        //COMMANDN(vdelta, setvdelta, Support::FunctionSignatures::ARG_1INT);
        //COMMANDN(undo, editundo, Support::FunctionSignatures::ARG_NONE);
        //COMMAND(copy, Support::FunctionSignatures::ARG_NONE);
        //COMMAND(paste, Support::FunctionSignatures::ARG_NONE);
        //COMMAND(edittex, Support::FunctionSignatures::ARG_2INT);
        //COMMAND(newent, Support::FunctionSignatures::ARG_5STR);
        //
        //

	}
}
