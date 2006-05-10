// one big bad include file for the whole engine... nasty!

#include "tools.h"	
#using <mscorlib.dll>
#using <MezzanineLib.dll>
 
struct sqr
{
    uchar type;                 // one of the above
    char floor, ceil;           // height, in cubes
    uchar wtex, ftex, ctex;     // wall/floor/ceil texture ids
    uchar r, g, b;              // light value at upper left vertex
    uchar vdelta;               // vertex delta, used for heightfield cubes
    char defer;                 // used in mipmapping, when true this cube is not a perfect mip
    char occluded;              // true when occluded
    uchar utex;                 // upper wall tex id
    uchar tag;                  // used by triggers
};

struct persistent_entity        // map entity
{
    short x, y, z;              // cube aligned position
    short attr1;
    uchar type;                 // type is one of the above
    uchar attr2, attr3, attr4;        
};

struct entity : public persistent_entity    
{
    bool spawned;               // the only dynamic state of a map entity
};
    
struct header                   // map file format header
{
    char head[4];               // "CUBE"
    int version;                // any >8bit quantity is a little indian
    int headersize;             // sizeof(header)
    int sfactor;                // in bits
    int numents;
    char maptitle[128];
    uchar texlists[3][256];
    int waterlevel;
    int reserved[15];
};

#define SWS(w,x,y,s) (&(w)[(y)*(s)+(x)])
#define SW(w,x,y) SWS(w,x,y,ssize)
#define S(x,y) SW(world,x,y)            // convenient lookup of a lowest mip cube
#define SOLID(x) ((x)->type==MezzanineLib::BlockTypes::SOLID)
#define OUTBORD(x,y) ((x)<MezzanineLib::GameInit::MinBord || (y)<MezzanineLib::GameInit::MinBord || (x)>=ssize-MezzanineLib::GameInit::MinBord || (y)>=ssize-MezzanineLib::GameInit::MinBord)

struct vec { float x, y, z; };
struct block { int x, y, xs, ys; };
struct mapmodelinfo { int rad, h, zoff, snap; char *name; };

struct dynent                           // players & monsters
{
    //vec o, vel;                         // origin, velocity
	vec o;
	vec vel;
    float yaw, pitch, roll;             // used as vec in one place
    float maxspeed;                     // cubes per second, 24 for player
    bool outsidemap;                    // from his eyes
    bool inwater;
    bool onfloor, jumpnext;
    int move, strafe;
    bool k_left, k_right, k_up, k_down; // see input code  
    int timeinair;                      // used for fake gravity
    float radius, eyeheight, aboveeye;  // bounding box size
    int lastupdate, plag, ping;
    int lifesequence;                   // sequence id for each respawn, used in damage test
    int state;                          // one of CS_* below
    int frags;
    int health, armour, armourtype, quadmillis;
    int gunselect, gunwait;
    int lastaction, lastattackgun, lastmove;
    bool attacking;
    int ammo[9];
    int monsterstate;                   // one of M_* below, M_NONE means human
    int mtype;                          // see monster.cpp
    dynent *enemy;                      // monster wants to kill this entity
    float targetyaw;                    // monster wants to look in this direction
    bool blocked, moving;               // used by physics to signal ai
    int trigger;                        // millis at which transition to another monsterstate takes place
    vec attacktarget;                   // delayed attacks
    int anger;                          // how many times already hit by fellow monster
    string name, team;
};

// vertex array format

struct vertex { float u, v, x, y, z; uchar r, g, b, a; }; 

typedef vector<dynent *> dvector;
typedef vector<char *> cvector;
typedef vector<int> ivector;

// globals ooh naughty

extern sqr *world, *wmip[];             // map data, the mips are sequential 2D arrays in memory
extern header hdr;                      // current map header
extern int sfactor, ssize;              // ssize = 2^sfactor
extern int cubicsize, mipsize;          // cubicsize = ssize^2
extern dynent *player1;                 // special client ent that receives input and acts as camera
extern dvector players;                 // all the other clients (in multiplayer)
extern bool editmode;
extern vector<entity> ents;             // map entities
extern vec worldpos;                    // current target of the crosshair in the world
extern int gamemode, nextmode;
extern bool demoplayback;

// simplistic vector ops
#define dotprod(u,v) ((u).x * (v).x + (u).y * (v).y + (u).z * (v).z)
#define vmul(u,f)    { (u).x *= (f); (u).y *= (f); (u).z *= (f); }
#define vdiv(u,f)    { (u).x /= (f); (u).y /= (f); (u).z /= (f); }
#define vadd(u,v)    { (u).x += (v).x; (u).y += (v).y; (u).z += (v).z; };
#define vsub(u,v)    { (u).x -= (v).x; (u).y -= (v).y; (u).z -= (v).z; };
#define vdist(d,v,e,s) vec v = s; vsub(v,e); float d = (float)sqrt(dotprod(v,v));
#define vreject(v,u,max) ((v).x>(u).x+(max) || (v).x<(u).x-(max) || (v).y>(u).y+(max) || (v).y<(u).y-(max))
#define vlinterp(v,f,u,g) { (v).x = (v).x*f+(u).x*g; (v).y = (v).y*f+(u).y*g; (v).z = (v).z*f+(u).z*g; }

#define sgetstr() { char *t = text; do { *t = getint(p); } while(*t++); }   // used by networking

#define m_noitems     (gamemode>=4)
#define m_noitemsrail (gamemode<=5)
#define m_arena       (gamemode>=8)
#define m_tarena      (gamemode>=10)
#define m_teammode    (gamemode&1 && gamemode>2)
#define m_sp          (gamemode<0)
#define m_dmsp        (gamemode==-1)
#define m_classicsp   (gamemode==-2)
#define isteam(a,b)   (m_teammode && strcmp(a, b)==0) 

// nasty macros for registering script functions, abuses globals to avoid excessive infrastructure
#define COMMANDN(name, fun, nargs) static bool __dummy_##fun = addcommand(#name, (void (*)())fun, nargs)
#define COMMAND(name, nargs) COMMANDN(name, name, nargs)
#define VARP(name, min, cur, max) int name = variable(#name, min, cur, max, &name, NULL, true)
#define VAR(name, min, cur, max)  int name = variable(#name, min, cur, max, &name, NULL, false)
#define VARF(name, min, cur, max, body)  void var_##name(); static int name = variable(#name, min, cur, max, &name, var_##name, false); void var_##name() { body; }
#define VARFP(name, min, cur, max, body) void var_##name(); static int name = variable(#name, min, cur, max, &name, var_##name, true); void var_##name() { body; }

#define ATOI(s) strtol(s, NULL, 0)		// supports hexadecimal numbers

#define WIN32_LEAN_AND_MEAN
#include "windows.h"
#define _WINDOWS
#define ZLIB_DLL

#include <time.h>

#include <GL/gl.h>
#include <GL/glu.h>
#include <GL/glext.h>

#include <SDL.h>

#include <enet/enet.h>

#include <zlib.h>

#include "protos.h"				// external function decls

