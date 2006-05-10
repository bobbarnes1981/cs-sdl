// editing.cpp: most map editing commands go here, entity editing commands are in world.cpp

#include "cube.h"
#using <mscorlib.dll>
#using <MezzanineLib.dll> 

// the current selection, used by almost all editing commands
// invariant: all code assumes that these are kept inside MINBORD distance of the edge of the map

block sel =
{
    variable("selx",  0, 0, 4096, &sel.x,  NULL, false),
    variable("sely",  0, 0, 4096, &sel.y,  NULL, false),
    variable("selxs", 0, 0, 4096, &sel.xs, NULL, false),
    variable("selys", 0, 0, 4096, &sel.ys, NULL, false),
};

int selh = 0;
bool selset = false;

#define loopselxy(b) { makeundo(); loop(x,sel.xs) loop(y,sel.ys) { sqr *s = S(sel.x+x, sel.y+y); b; }; remip(sel); }

int cx, cy, ch;

int curedittex[] = { -1, -1, -1 };

bool dragging = false;
int lastx, lasty, lasth;

int lasttype = 0, lasttex = 0;
sqr rtex;

VAR(editing,0,0,1);

void toggleedit()
{
    if(player1->state==MezzanineLib::CSStatus::CS_DEAD) return;                 // do not allow dead players to edit to avoid state confusion
    if(!MezzanineLib::GameInit::EditMode && !allowedittoggle()) return;         // not in most multiplayer modes
    MezzanineLib::GameInit::EditMode = !MezzanineLib::GameInit::EditMode;
	if(!MezzanineLib::GameInit::EditMode)
    {
        settagareas();                                  // reset triggers to allow quick playtesting
        entinmap(player1);                              // find spawn closest to current floating pos
    }
    else
    {
        resettagareas();                                // clear trigger areas to allow them to be edited
        player1->health = 100;
        if(m_classicsp) monsterclear();                 // all monsters back at their spawns for editing
        projreset();
    };
	SdlDotNet::Keyboard::KeyRepeat = MezzanineLib::GameInit::EditMode;
    selset = false;
    editing = MezzanineLib::GameInit::EditMode;
};

COMMANDN(edittoggle, toggleedit, MezzanineLib::Support::FunctionSignatures::ARG_NONE);

void correctsel()                                       // ensures above invariant
{
    selset = !OUTBORD(sel.x, sel.y);
    int bsize = MezzanineLib::GameInit::SSize-MezzanineLib::GameInit::MinBord;
    if(sel.xs+sel.x>bsize) sel.xs = bsize-sel.x;
    if(sel.ys+sel.y>bsize) sel.ys = bsize-sel.y;
    if(sel.xs<=0 || sel.ys<=0) selset = false;
};

bool noteditmode()
{
    correctsel();
    if(!MezzanineLib::GameInit::EditMode) conoutf("this function is only allowed in edit mode");
    return !MezzanineLib::GameInit::EditMode;
};

bool noselection()
{
    if(!selset) conoutf("no selection");
    return !selset;
};

#define EDITSEL   if(noteditmode() || noselection()) return;
#define EDITSELMP if(noteditmode() || noselection() || multiplayer()) return;
#define EDITMP    if(noteditmode() || multiplayer()) return;

void selectpos(int x, int y, int xs, int ys)
{
    block s = { x, y, xs, ys };
    sel = s;
    selh = 0;
    correctsel();
};

void makesel()
{
    block s = { min(lastx,cx), min(lasty,cy), abs(lastx-cx)+1, abs(lasty-cy)+1 };
    sel = s;
    selh = max(lasth,ch);
    correctsel();
    if(selset) rtex = *S(sel.x, sel.y);
};

VAR(flrceil,0,0,2);

float sheight(sqr *s, sqr *t, float z)                  // finds out z height when cursor points at wall
{
    return !flrceil //z-s->floor<s->ceil-z
        ? (s->type==MezzanineLib::BlockTypes::FHF ? s->floor-t->vdelta/4.0f : (float)s->floor)
        : (s->type==MezzanineLib::BlockTypes::CHF ? s->ceil+t->vdelta/4.0f : (float)s->ceil);
};

void cursorupdate()                                     // called every frame from hud
{
    flrceil = ((int)(player1->pitch>=0))*2;

    volatile float x = worldpos.x;                      // volatile needed to prevent msvc7 optimizer bug?
    volatile float y = worldpos.y;
    volatile float z = worldpos.z;
    
    cx = (int)x;
    cy = (int)y;

    if(OUTBORD(cx, cy)) return;
    sqr *s = S(cx,cy);
    
    if(fabs(sheight(s,s,z)-z)>1)                        // selected wall
    {
        x += x>player1->o.x ? 0.5f : -0.5f;             // find right wall cube
        y += y>player1->o.y ? 0.5f : -0.5f;

        cx = (int)x;
        cy = (int)y;

        if(OUTBORD(cx, cy)) return;
    };
        
    if(dragging) makesel();

    const int GRIDSIZE = 5;
    const float GRIDW = 0.5f;
    const float GRID8 = 2.0f;
    const float GRIDS = 2.0f;
    const int GRIDM = 0x7;
    
    // render editing grid

    for(int ix = cx-GRIDSIZE; ix<=cx+GRIDSIZE; ix++) for(int iy = cy-GRIDSIZE; iy<=cy+GRIDSIZE; iy++)
    {
        if(OUTBORD(ix, iy)) continue;
        sqr *s = S(ix,iy);
        if(SOLID(s)) continue;
        float h1 = sheight(s, s, z);
        float h2 = sheight(s, SWS(s,1,0,MezzanineLib::GameInit::SSize), z);
        float h3 = sheight(s, SWS(s,1,1,MezzanineLib::GameInit::SSize), z);
        float h4 = sheight(s, SWS(s,0,1,MezzanineLib::GameInit::SSize), z);
		if(s->tag) MezzanineLib::Render::RenderExtras::LineStyle(GRIDW, 0xFF, 0x40, 0x40);
		else if(s->type==MezzanineLib::BlockTypes::FHF || s->type==MezzanineLib::BlockTypes::CHF) MezzanineLib::Render::RenderExtras::LineStyle(GRIDW, 0x80, 0xFF, 0x80);
        else MezzanineLib::Render::RenderExtras::LineStyle(GRIDW, 0x80, 0x80, 0x80);
        block b = { ix, iy, 1, 1 };
        box(b, h1, h2, h3, h4);
		MezzanineLib::Render::RenderExtras::LineStyle(GRID8, 0x40, 0x40, 0xFF);
        if(!(ix&GRIDM))   MezzanineLib::Render::RenderExtras::Line(ix,   iy,   h1, ix,   iy+1, h4);
        if(!(ix+1&GRIDM)) MezzanineLib::Render::RenderExtras::Line(ix+1, iy,   h2, ix+1, iy+1, h3);
        if(!(iy&GRIDM))   MezzanineLib::Render::RenderExtras::Line(ix,   iy,   h1, ix+1, iy,   h2);
        if(!(iy+1&GRIDM)) MezzanineLib::Render::RenderExtras::Line(ix,   iy+1, h4, ix+1, iy+1, h3);
    };

    if(!SOLID(s))
    {
        float ih = sheight(s, s, z);
		MezzanineLib::Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0xFF, 0xFF);
        block b = { cx, cy, 1, 1 };
        box(b, ih, sheight(s, SWS(s,1,0,MezzanineLib::GameInit::SSize), z), sheight(s, SWS(s,1,1,MezzanineLib::GameInit::SSize), z), sheight(s, SWS(s,0,1,MezzanineLib::GameInit::SSize), z));
		MezzanineLib::Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0x00, 0x00);
        MezzanineLib::Render::RenderExtras::Dot(cx, cy, ih);
        ch = (int)ih;
    };

    if(selset)
    {
		MezzanineLib::Render::RenderExtras::LineStyle(GRIDS, 0xFF, 0x40, 0x40);
        box(sel, (float)selh, (float)selh, (float)selh, (float)selh);
    };
};

vector<block *> undos;                                  // unlimited undo
VARP(undomegs, 0, 1, 10);                                // bounded by n megs

void pruneundos(int maxremain)                          // bound memory
{
    int t = 0;
    loopvrev(undos)
    {
        t += undos[i]->xs*undos[i]->ys*sizeof(sqr);
        if(t>maxremain) free(undos.remove(i));
    };
};

void makeundo()
{
    undos.add(blockcopy(sel));
    pruneundos(undomegs<<20);
};

void editundo()
{
    EDITMP;
    if(undos.empty()) { conoutf("nothing more to undo"); return; };
    block *p = undos.pop();
    blockpaste(*p);
    free(p);
};

block *copybuf = NULL;

void copy()
{
    EDITSELMP;
    if(copybuf) free(copybuf);
    copybuf = blockcopy(sel);
};

void paste()
{
    EDITMP;
    if(!copybuf) { conoutf("nothing to paste"); return; };
    sel.xs = copybuf->xs;
    sel.ys = copybuf->ys;
    correctsel();
    if(!selset || sel.xs!=copybuf->xs || sel.ys!=copybuf->ys) { conoutf("incorrect selection"); return; };
    makeundo();
    copybuf->x = sel.x;
    copybuf->y = sel.y;
    blockpaste(*copybuf);
};

void tofronttex()                                       // maintain most recently used of the texture lists when applying texture
{
    loopi(3)
    {
        int c = curedittex[i];
        if(c>=0)
        {
            uchar *p = hdr.texlists[i];
            int t = p[c];
            for(int a = c-1; a>=0; a--) p[a+1] = p[a];
            p[0] = t;
            curedittex[i] = -1;
        };
    };
};

void editdrag(bool isdown)
{
    if(dragging = isdown)
    {
        lastx = cx;
        lasty = cy;
        lasth = ch;
        selset = false;
        tofronttex();
    };
    makesel();
};

// the core editing function. all the *xy functions perform the core operations
// and are also called directly from the network, the function below it is strictly
// triggered locally. They all have very similar structure.

void editheightxy(bool isfloor, int amount, block &sel)
{
    loopselxy(if(isfloor)
    {
        s->floor += amount;
        if(s->floor>=s->ceil) s->floor = s->ceil-1;
    }
    else
    {
        s->ceil += amount;
        if(s->ceil<=s->floor) s->ceil = s->floor+1;
    });
};

void editheight(int flr, int amount)
{
    EDITSEL;
    bool isfloor = flr==0;
    editheightxy(isfloor, amount, sel);
    addmsg(1, 7, MezzanineLib::NetworkMessages::SV_EDITH, sel.x, sel.y, sel.xs, sel.ys, isfloor, amount);
};

COMMAND(editheight, MezzanineLib::Support::FunctionSignatures::ARG_2INT);

void edittexxy(int type, int t, block &sel)            
{
    loopselxy(switch(type)
    {
        case 0: s->ftex = t; break;
        case 1: s->wtex = t; break;
        case 2: s->ctex = t; break;
        case 3: s->utex = t; break;
    });
};

void edittex(int type, int dir)
{
    EDITSEL;
    if(type<0 || type>3) return;
    if(type!=lasttype) { tofronttex(); lasttype = type; };
    int atype = type==3 ? 1 : type;
    int i = curedittex[atype];
    i = i<0 ? 0 : i+dir;
    curedittex[atype] = i = min(max(i, 0), 255);
    int t = lasttex = hdr.texlists[atype][i];
    edittexxy(type, t, sel);
    addmsg(1, 7, MezzanineLib::NetworkMessages::SV_EDITT, sel.x, sel.y, sel.xs, sel.ys, type, t);
};

void replace()
{
    EDITSELMP;
    loop(x,MezzanineLib::GameInit::SSize) loop(y,MezzanineLib::GameInit::SSize)
    {
        sqr *s = S(x, y);
        switch(lasttype)
        {
            case 0: if(s->ftex == rtex.ftex) s->ftex = lasttex; break;
            case 1: if(s->wtex == rtex.wtex) s->wtex = lasttex; break;
            case 2: if(s->ctex == rtex.ctex) s->ctex = lasttex; break;
            case 3: if(s->utex == rtex.utex) s->utex = lasttex; break;
        };
    };
    block b = { 0, 0, MezzanineLib::GameInit::SSize, MezzanineLib::GameInit::SSize }; 
    remip(b);
};

void edittypexy(int type, block &sel)
{
    loopselxy(s->type = type);
};

void edittype(int type)
{
    EDITSEL;
    if(type==MezzanineLib::BlockTypes::CORNER && (sel.xs!=sel.ys || sel.xs==3 || sel.xs>4 && sel.xs!=8
                   || sel.x&~-sel.xs || sel.y&~-sel.ys))
                   { conoutf("corner selection must be power of 2 aligned"); return; };
    edittypexy(type, sel);
    addmsg(1, 6, MezzanineLib::NetworkMessages::SV_EDITS, sel.x, sel.y, sel.xs, sel.ys, type);
};

void heightfield(int t) { edittype(t==0 ? MezzanineLib::BlockTypes::FHF : MezzanineLib::BlockTypes::CHF); };
void solid(int t)       { edittype(t==0 ? MezzanineLib::BlockTypes::SPACE : MezzanineLib::BlockTypes::SOLID); };
void corner()           { edittype(MezzanineLib::BlockTypes::CORNER); };

COMMAND(heightfield, MezzanineLib::Support::FunctionSignatures::ARG_1INT);
COMMAND(solid, MezzanineLib::Support::FunctionSignatures::ARG_1INT);
COMMAND(corner, MezzanineLib::Support::FunctionSignatures::ARG_NONE);

void editequalisexy(bool isfloor, block &sel)
{
    int low = 127, hi = -128;
    loopselxy(
    {
        if(s->floor<low) low = s->floor;
        if(s->ceil>hi) hi = s->ceil;
    });
    loopselxy(
    {
        if(isfloor) s->floor = low; else s->ceil = hi;
        if(s->floor>=s->ceil) s->floor = s->ceil-1;
    });
};

void equalize(int flr)
{
    bool isfloor = flr==0;
    EDITSEL;
    editequalisexy(isfloor, sel);
    addmsg(1, 6, MezzanineLib::NetworkMessages::SV_EDITE, sel.x, sel.y, sel.xs, sel.ys, isfloor);
};

COMMAND(equalize, MezzanineLib::Support::FunctionSignatures::ARG_1INT);

void setvdeltaxy(int delta, block &sel)
{
    loopselxy(s->vdelta = max(s->vdelta+delta, 0));
    remipmore(sel);    
};

void setvdelta(int delta)
{
    EDITSEL;
    setvdeltaxy(delta, sel);
    addmsg(1, 6, MezzanineLib::NetworkMessages::SV_EDITD, sel.x, sel.y, sel.xs, sel.ys, delta);
};

const int MAXARCHVERT = 50;
int archverts[MAXARCHVERT][MAXARCHVERT];
bool archvinit = false;

void archvertex(int span, int vert, int delta)
{
    if(!archvinit)
    {
        archvinit = true;
        loop(s,MAXARCHVERT) loop(v,MAXARCHVERT) archverts[s][v] = 0;
    };
    if(span>=MAXARCHVERT || vert>=MAXARCHVERT || span<0 || vert<0) return;
    archverts[span][vert] = delta;
};

void arch(int sidedelta, int _a)
{
    EDITSELMP;
    sel.xs++;
    sel.ys++;
    if(sel.xs>MAXARCHVERT) sel.xs = MAXARCHVERT;
    if(sel.ys>MAXARCHVERT) sel.ys = MAXARCHVERT;
    loopselxy(s->vdelta =
        sel.xs>sel.ys
            ? (archverts[sel.xs-1][x] + (y==0 || y==sel.ys-1 ? sidedelta : 0))
            : (archverts[sel.ys-1][y] + (x==0 || x==sel.xs-1 ? sidedelta : 0)));
    remipmore(sel);
};

void slope(int xd, int yd)
{
    EDITSELMP;
    int off = 0;
    if(xd<0) off -= xd*sel.xs;
    if(yd<0) off -= yd*sel.ys;
    sel.xs++;
    sel.ys++;
    loopselxy(s->vdelta = xd*x+yd*y+off);
    remipmore(sel);
};


VARF(fullbright, 0, 0, 1,
    if(fullbright)
    {
        if(noteditmode()) return;
        loopi(mipsize) world[i].r = world[i].g = world[i].b = 176;
    };
);

void edittag(int tag)
{
    EDITSELMP;
    loopselxy(s->tag = tag);
};

void newent(char *what, char *a1, char *a2, char *a3, char *a4)
{
    EDITSEL;
    newentity(sel.x, sel.y, (int)player1->o.z, what, ATOI(a1), ATOI(a2), ATOI(a3), ATOI(a4));
};

COMMANDN(select, selectpos, MezzanineLib::Support::FunctionSignatures::ARG_4INT);
COMMAND(edittag, MezzanineLib::Support::FunctionSignatures::ARG_1INT);
COMMAND(replace, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(archvertex, MezzanineLib::Support::FunctionSignatures::ARG_3INT);
COMMAND(arch, MezzanineLib::Support::FunctionSignatures::ARG_2INT);
COMMAND(slope, MezzanineLib::Support::FunctionSignatures::ARG_2INT);
COMMANDN(vdelta, setvdelta, MezzanineLib::Support::FunctionSignatures::ARG_1INT);
COMMANDN(undo, editundo, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(copy, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(paste, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(edittex, MezzanineLib::Support::FunctionSignatures::ARG_2INT);
COMMAND(newent, MezzanineLib::Support::FunctionSignatures::ARG_5STR);


