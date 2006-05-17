// world.cpp: core map management stuff

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::World;

extern char *entnames[];                // lookup from map entities above to strings

sqr *world = NULL;

header hdr;
header * gethdr(void)
{
	header * p = &hdr;
	return p;
}

void settag(int tag, int type)          // set all cubes with "tag" to space, if tag is 0 then reset ALL tagged cubes according to type
{
    int maxx = 0, maxy = 0, minx = MezzanineLib::GameInit::SSize, miny = MezzanineLib::GameInit::SSize;
    loop(x, MezzanineLib::GameInit::SSize) loop(y, MezzanineLib::GameInit::SSize)
    {
        sqr *s = S(x, y);
        if(s->tag)
        {
            if(tag)
            {
                if(tag==s->tag) s->type = MezzanineLib::BlockTypes::SPACE;
                else continue;
            }
            else
            {
                s->type = type ? MezzanineLib::BlockTypes::SOLID : MezzanineLib::BlockTypes::SPACE;
            };
            if(x>maxx) maxx = x;
            if(y>maxy) maxy = y;
            if(x<minx) minx = x;
            if(y<miny) miny = y;
        };
    };
    block b = { minx, miny, maxx-minx+1, maxy-miny+1 };
    if(maxx) remip(b);      // remip minimal area of changed geometry
};

void resettagareas() { settag(0, 0); };                                                         // reset for editing or map saving
void settagareas() { settag(0, 1); loopv(ents) if(ents[i].type==MezzanineLib::StaticEntity::CARROT) setspawn(i, true); };   // set for playing

void trigger(int tag, int type, bool savegame)
{
    if(!tag) return;
    settag(tag, type);
    if(!savegame && type!=3) playsound(MezzanineLib::Sounds::S_RUMBLE);
    sprintf_sd(aliasname)("level_trigger_%d", tag);
    if(identexists(aliasname)) execute(aliasname);
    if(type==2) endsp(false);
};

COMMAND(trigger, MezzanineLib::Support::FunctionSignatures::ARG_2INT);

// main geometric mipmapping routine, recursively rebuild mipmaps within block b.
// tries to produce cube out of 4 lower level mips as well as possible,
// sets defer to 0 if mipped cube is a perfect mip, i.e. can be rendered at this
// mip level indistinguishable from its constituent cubes (saves considerable
// rendering time if this is possible).

void remip(block &b, int level)
{
    if(level>=MezzanineLib::GameInit::SmallestFactor) return;
    int lighterr = getvar("lighterror")*3;
    sqr *w = wmip[level];
    sqr *v = wmip[level+1];
    int ws = MezzanineLib::GameInit::SSize>>level;
    int vs = MezzanineLib::GameInit::SSize>>(level+1);
    block s = b;
    if(s.x&1) { s.x--; s.xs++; };
    if(s.y&1) { s.y--; s.ys++; };
    s.xs = (s.xs+1)&~1;
    s.ys = (s.ys+1)&~1;
    for(int x = s.x; x<s.x+s.xs; x+=2) for(int y = s.y; y<s.y+s.ys; y+=2)
    {
        sqr *o[4];
        o[0] = SWS(w,x,y,ws);                               // the 4 constituent cubes
        o[1] = SWS(w,x+1,y,ws);
        o[2] = SWS(w,x+1,y+1,ws);
        o[3] = SWS(w,x,y+1,ws);
        sqr *r = SWS(v,x/2,y/2,vs);                         // the target cube in the higher mip level
        *r = *o[0];
        uchar nums[MezzanineLib::BlockTypes::MAXTYPE];
        loopi(MezzanineLib::BlockTypes::MAXTYPE) nums[i] = 0;
        loopj(4) nums[o[j]->type]++;
        r->type = MezzanineLib::BlockTypes::SEMISOLID;                                // cube contains both solid and space, treated specially in the renderer
        loopk(MezzanineLib::BlockTypes::MAXTYPE) if(nums[k]==4) r->type = k;
        if(!SOLID(r))
        {
            int floor = 127, ceil = -128, num = 0;
            loopi(4) if(!SOLID(o[i]))
            {
                num++;
                int fh = o[i]->floor;
                int ch = o[i]->ceil;
                if(r->type==MezzanineLib::BlockTypes::SEMISOLID)
                {
                    if(o[i]->type==MezzanineLib::BlockTypes::FHF) fh -= o[i]->vdelta/4+2;     // crap hack, needed for rendering large mips next to hfs
                    if(o[i]->type==MezzanineLib::BlockTypes::CHF) ch += o[i]->vdelta/4+2;     // FIXME: needs to somehow take into account middle vertices on higher mips
                };
                if(fh<floor) floor = fh;  // take lowest floor and highest ceil, so we never have to see missing lower/upper from the side
                if(ch>ceil) ceil = ch;
            };
            r->floor = floor;
            r->ceil = ceil;
        };       
        if(r->type==MezzanineLib::BlockTypes::CORNER) goto mip;                       // special case: don't ever split even if textures etc are different
        r->defer = 1;
        if(SOLID(r))
        {
            loopi(3)
            {
                if(o[i]->wtex!=o[3]->wtex) goto c;          // on an all solid cube, only thing that needs to be equal for a perfect mip is the wall texture
            };
        }
        else
        {
            loopi(3)
            {
                if(o[i]->type!=o[3]->type
                || o[i]->floor!=o[3]->floor
                || o[i]->ceil!=o[3]->ceil
                || o[i]->ftex!=o[3]->ftex
                || o[i]->ctex!=o[3]->ctex
                || abs(o[i+1]->r-o[0]->r)>lighterr          // perfect mip even if light is not exactly equal
                || abs(o[i+1]->g-o[0]->g)>lighterr
                || abs(o[i+1]->b-o[0]->b)>lighterr
                || o[i]->utex!=o[3]->utex
                || o[i]->wtex!=o[3]->wtex) goto c;
            };
            if(r->type==MezzanineLib::BlockTypes::CHF || r->type==MezzanineLib::BlockTypes::FHF)                // can make a perfect mip out of a hf if slopes lie on one line
            {
                if(o[0]->vdelta-o[1]->vdelta != o[1]->vdelta-SWS(w,x+2,y,ws)->vdelta
                || o[0]->vdelta-o[2]->vdelta != o[2]->vdelta-SWS(w,x+2,y+2,ws)->vdelta
                || o[0]->vdelta-o[3]->vdelta != o[3]->vdelta-SWS(w,x,y+2,ws)->vdelta
                || o[3]->vdelta-o[2]->vdelta != o[2]->vdelta-SWS(w,x+2,y+1,ws)->vdelta
                || o[1]->vdelta-o[2]->vdelta != o[2]->vdelta-SWS(w,x+1,y+2,ws)->vdelta) goto c;
            };
        };
        { loopi(4) if(o[i]->defer) goto c; };               // if any of the constituents is not perfect, then this one isn't either
        mip:
        r->defer = 0;
        c:;
    };
    s.x  /= 2;
    s.y  /= 2;
    s.xs /= 2;
    s.ys /= 2;
    remip(s, level+1);
};

void remipmore(block &b, int level)
{
    block bb = b;
    if(bb.x>1) bb.x--;
    if(bb.y>1) bb.y--;
    if(bb.xs<MezzanineLib::GameInit::SSize-3) bb.xs++;
    if(bb.ys<MezzanineLib::GameInit::SSize-3) bb.ys++;
    remip(bb, level);
};

int closestent()        // used for delent and edit mode ent display
{
    if(noteditmode()) return -1;
    int best;
    float bdist = 99999;
    loopv(ents)
    {
        entity &e = ents[i];
        if(e.type==MezzanineLib::StaticEntity::NOTUSED) continue;
        vec v = { e.x, e.y, e.z };
        vdist(dist, t, player1->o, v);
        if(dist<bdist)
        {
            best = i;
            bdist = dist;
        };
    };
    return bdist==99999 ? -1 : best; 
};

void entproperty(int prop, int amount)
{
    int e = closestent();
    if(e<0) return;
    switch(prop)
    {
        case 0: ents[e].attr1 += amount; break;
        case 1: ents[e].attr2 += amount; break;
        case 2: ents[e].attr3 += amount; break;
        case 3: ents[e].attr4 += amount; break;
    };
};

void delent()
{
    int e = closestent();
    if(e<0) { conoutf("no more entities"); return; };
    int t = ents[e].type;
	conoutf("%s entity deleted", entnames[t]);
    ents[e].type = MezzanineLib::StaticEntity::NOTUSED;
    addmsg(1, 10, MezzanineLib::NetworkMessages::SV_EDITENT, e, MezzanineLib::StaticEntity::NOTUSED, 0, 0, 0, 0, 0, 0, 0);
    if(t==MezzanineLib::StaticEntity::LIGHT) calclight();
};

int findtype(char *what)
{
	loopi(MezzanineLib::StaticEntity::MAXENTTYPES) if(strcmp(what, entnames[i])==0) return i;
    conoutf("unknown entity type \"%s\"", what);
    return MezzanineLib::StaticEntity::NOTUSED;
}

entity *newentity(int x, int y, int z, char *what, int v1, int v2, int v3, int v4)
{
    int type = findtype(what);
    persistent_entity e = { x, y, z, v1, type, v2, v3, v4 };
    switch(type)
    {
        case MezzanineLib::StaticEntity::LIGHT:
            if(v1>32) v1 = 32;
            if(!v1) e.attr1 = 16;
            if(!v2 && !v3 && !v4) e.attr2 = 255;          
            break;
            
        case MezzanineLib::StaticEntity::MAPMODEL:
            e.attr4 = e.attr3;
            e.attr3 = e.attr2;
        case MezzanineLib::StaticEntity::MONSTER:
        case MezzanineLib::StaticEntity::TELEDEST:
            e.attr2 = (uchar)e.attr1;
        case MezzanineLib::StaticEntity::PLAYERSTART:
            e.attr1 = (int)player1->yaw;
            break;
    };           
    addmsg(1, 10, MezzanineLib::NetworkMessages::SV_EDITENT, ents.length(), type, e.x, e.y, e.z, e.attr1, e.attr2, e.attr3, e.attr4);
    ents.add(*((entity *)&e)); // unsafe!
    if(type==MezzanineLib::StaticEntity::LIGHT) calclight();
    return &ents.last();
};

void clearents(char *name)
{  
    int type = findtype(name);
    if(noteditmode() || multiplayer()) return;
    loopv(ents)
    {
        entity &e = ents[i];
        if(e.type==type) e.type = MezzanineLib::StaticEntity::NOTUSED;
    };
    if(type==MezzanineLib::StaticEntity::LIGHT) calclight();
};

COMMAND(clearents, MezzanineLib::Support::FunctionSignatures::ARG_1STR);

void scalecomp(uchar &c, int intens)
{
    int n = c*intens/100;
    if(n>255) n = 255;
    c = n;
};

void scalelights(int f, int intens)
{
    loopv(ents)
    {
        entity &e = ents[i];
        if(e.type!=MezzanineLib::StaticEntity::LIGHT) continue;
        e.attr1 = e.attr1*f/100;
        if(e.attr1<2) e.attr1 = 2;
        if(e.attr1>32) e.attr1 = 32;
        if(intens)
        {
            scalecomp(e.attr2, intens);
            scalecomp(e.attr3, intens);
            scalecomp(e.attr4, intens);
        };
    };
    calclight();
};

COMMAND(scalelights, MezzanineLib::Support::FunctionSignatures::ARG_2INT);

int findentity(int type, int index)
{
    for(int i = index; i<ents.length(); i++) if(ents[i].type==type) return i;
    loopj(index) if(ents[j].type==type) return j;
    return -1;
};
sqr *wmip[11*2];
//sqr *wmip[MezzanineLib::GameInit::LargestFactor*2];

void setupworld(int factor)
{
	MezzanineLib::GameInit::SFactor = factor;
    MezzanineLib::GameInit::SSize = 1<<(MezzanineLib::GameInit::SFactor);
    MezzanineLib::GameInit::CubicSize = MezzanineLib::GameInit::SSize*MezzanineLib::GameInit::SSize;
    MezzanineLib::GameInit::MipSize = MezzanineLib::GameInit::CubicSize*134/100;
    sqr *w = world = (sqr *)alloc(MezzanineLib::GameInit::MipSize*sizeof(sqr));
    loopi(MezzanineLib::GameInit::LargestFactor*2) { wmip[i] = w; w += MezzanineLib::GameInit::CubicSize>>(i*2); };
};

void empty_world(int factor, bool force)    // main empty world creation routine, if passed factor -1 will enlarge old world by 1
{
    if(!force && noteditmode()) return; 
    cleardlights();
    pruneundos();
    sqr *oldworld = world;
    bool copy = false;
    if(oldworld && factor<0) { factor = MezzanineLib::GameInit::SFactor+1; copy = true; };
    if(factor<MezzanineLib::GameInit::SmallestFactor) factor = MezzanineLib::GameInit::SmallestFactor;
    if(factor>MezzanineLib::GameInit::LargestFactor) factor = MezzanineLib::GameInit::LargestFactor;
    setupworld(factor);
    
    loop(x,MezzanineLib::GameInit::SSize) loop(y,MezzanineLib::GameInit::SSize)
    {
        sqr *s = S(x,y);
        s->r = s->g = s->b = 150;
        s->ftex = MezzanineLib::TextureNumbers::DEFAULT_FLOOR;
        s->ctex = MezzanineLib::TextureNumbers::DEFAULT_CEIL;
        s->wtex = s->utex = MezzanineLib::TextureNumbers::DEFAULT_WALL;
        s->type = MezzanineLib::BlockTypes::SOLID;
        s->floor = 0;
        s->ceil = 16;
        s->vdelta = 0;
        s->defer = 0;
    };
    
    strncpy(hdr.head, "CUBE", 4);
	hdr.version = MezzanineLib::GameInit::MapVersion;
    hdr.headersize = sizeof(header);
    hdr.sfactor = MezzanineLib::GameInit::SFactor;

    if(copy)
    {
        loop(x,MezzanineLib::GameInit::SSize/2) loop(y,MezzanineLib::GameInit::SSize/2)
        {
            *S(x+MezzanineLib::GameInit::SSize/4, y+MezzanineLib::GameInit::SSize/4) = *SWS(oldworld, x, y, MezzanineLib::GameInit::SSize/2);
        };
        loopv(ents)
        {
            ents[i].x += MezzanineLib::GameInit::SSize/4;
            ents[i].y += MezzanineLib::GameInit::SSize/4;
        };
    }
    else
    {
        strn0cpy(hdr.maptitle, "Untitled Map by Unknown", 128);
        hdr.waterlevel = -100000;
        loopi(15) hdr.reserved[i] = 0;
        loopk(3) loopi(256) hdr.texlists[k][i] = i;
        ents.setsize(0);
        block b = { 8, 8, MezzanineLib::GameInit::SSize-16, MezzanineLib::GameInit::SSize-16 }; 
        edittypexy(MezzanineLib::BlockTypes::SPACE, b);
    };
    
    calclight();
    startmap("base/unnamed");
    if(oldworld)
    {
        free(oldworld);
        toggleedit();
        execute("fullbright 1");
    };
};

void mapenlarge()  { empty_world(-1, false); };
void newmap(int i) { empty_world(i, false); };

COMMAND(mapenlarge, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(newmap, MezzanineLib::Support::FunctionSignatures::ARG_1INT);
COMMANDN(recalc, calclight, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(delent, MezzanineLib::Support::FunctionSignatures::ARG_NONE);
COMMAND(entproperty, MezzanineLib::Support::FunctionSignatures::ARG_2INT);

