// loading and saving of savegames & demos, dumps the spawn state of all mapents, the full state of all dynents (monsters + player)

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::Game;

gzFile f = NULL;
dvector playerhistory;

void startdemo();

void gzput(int i) { gzputc(f, i); };
void gzputi(int i) { gzwrite(f, &i, sizeof(int)); };
void gzputv(vec &v) { gzwrite(f, &v, sizeof(vec)); };

void gzcheck(int a, int b) { if(a!=b) GameInit::Fatal("savegame file corrupt (short)"); };
int gzget() { char c = gzgetc(f); return c; };
int gzgeti() { int i; gzcheck(gzread(f, &i, sizeof(int)), sizeof(int)); return i; };
void gzgetv(vec &v) { gzcheck(gzread(f, &v, sizeof(vec)), sizeof(vec)); };

void stop()
{
    if(f)
    {
        if(SaveGameDemo::demorecording) gzputi(-1);
        gzclose(f);
    };
    f = NULL;
    SaveGameDemo::demorecording = false;
    GameInit::DemoPlayback = false;
    SaveGameDemo::demoloading = false;
    loopv(playerhistory) zapdynent(playerhistory[i]);
    playerhistory.setsize(0);
};

void stopifrecording() { if(SaveGameDemo::demorecording) stop(); };

void savestate(char *fn)
{
    stop();
    f = gzopen(fn, "wb9");
    if(!f) { conoutf("could not write %s", fn); return; };
    gzwrite(f, (void *)"CUBESAVE", 8);
    gzputc(f, SaveGameDemo::IsLittleEndian);  
	gzputi(GameInit::SAVEGAMEVERSION);
    gzputi(sizeof(dynent));
    gzwrite(f, getclientmap(), _MAXDEFSTR);
    gzputi(gamemode);
    gzputi(ents.length());
    loopv(ents) gzputc(f, ents[i].spawned);
    gzwrite(f, player1, sizeof(dynent));
    dvector &monsters = getmonsters();
    gzputi(monsters.length());
    loopv(monsters) gzwrite(f, monsters[i], sizeof(dynent));
    gzputi(players.length());
    loopv(players)
    {
        gzput(players[i]==NULL);
        gzwrite(f, players[i], sizeof(dynent));
    };
};

void savegame(char *name)
{
    if(!m_classicsp) { conoutf("can only save classic sp games"); return; };
    sprintf_sd(fn)("savegames/%s.csgz", name);
    savestate(fn);
    stop();
    conoutf("wrote %s", fn);
};

void loadstate(char *fn)
{
    stop();
    if(multiplayer()) return;
    f = gzopen(fn, "rb9");
    if(!f) { conoutf("could not open %s", fn); return; };
    
    string buf;
    gzread(f, buf, 8);
    if(strncmp(buf, "CUBESAVE", 8)) goto out;
    if(gzgetc(f)!=SaveGameDemo::IsLittleEndian) goto out;     // not supporting save->load accross incompatible architectures simpifies things a LOT
    if(gzgeti()!=GameInit::SAVEGAMEVERSION || gzgeti()!=sizeof(dynent)) goto out;
    string mapname;
    gzread(f, mapname, _MAXDEFSTR);
    nextmode = gzgeti();
    changemap(mapname); // continue below once map has been loaded and client & server have updated 
    return;
    out:    
    conoutf("aborting: savegame/demo from a different version of cube or cpu architecture");
    stop();
};

void loadgame(char *name)
{
    sprintf_sd(fn)("savegames/%s.csgz", name);
    loadstate(fn);
};

void loadgameout()
{
    stop();
    conoutf("loadgame incomplete: savegame from a different version of this map");
};

void loadgamerest()
{
    if(GameInit::DemoPlayback || !f) return;
        
    if(gzgeti()!=ents.length()) return loadgameout();
    loopv(ents)
    {
        ents[i].spawned = gzgetc(f)!=0;   
        if(ents[i].type==StaticEntity::CARROT && !ents[i].spawned) trigger(ents[i].attr1, ents[i].attr2, true);
    };
    restoreserverstate(ents);
    
    gzread(f, player1, sizeof(dynent));
    player1->lastaction = GameInit::LastMillis;
    
    int nmonsters = gzgeti();
    dvector &monsters = getmonsters();
    if(nmonsters!=monsters.length()) return loadgameout();
    loopv(monsters)
    {
        gzread(f, monsters[i], sizeof(dynent));
        monsters[i]->enemy = player1;                                       // lazy, could save id of enemy instead
        monsters[i]->lastaction = monsters[i]->trigger = GameInit::LastMillis+500;    // also lazy, but no real noticable effect on game
        if(monsters[i]->state==CSStatus::CS_DEAD) monsters[i]->lastaction = 0;
    };
    restoremonsterstate();
    
    int nplayers = gzgeti();
    loopi(nplayers) if(!gzget())
    {
        dynent *d = getclient(i);
        assert(d);
        gzread(f, d, sizeof(dynent));        
    };
    
    conoutf("savegame restored");
    if(SaveGameDemo::demoloading) startdemo(); else stop();
};

// demo functions

vec dorig;

void record(char *name)
{
    if(m_sp) { conoutf("cannot record singleplayer games"); return; };
    int cn = ClientServer::Client::ClientNum;
    if(cn<0) return;
    sprintf_sd(fn)("demos/%s.cdgz", name);
    savestate(fn);
    gzputi(cn);
    conoutf("started recording demo to %s", fn);
    SaveGameDemo::demorecording = true;
    SaveGameDemo::starttime = GameInit::LastMillis;
	SaveGameDemo::ddamage = SaveGameDemo::bdamage = 0;
};

void demodamage(int damage, vec &o) { SaveGameDemo::ddamage = damage; dorig = o; };
void demoblend(int damage) { SaveGameDemo::bdamage = damage; };

void incomingdemodata(uchar *buf, int len, bool extras)
{
    if(!SaveGameDemo::demorecording) return;
    gzputi(GameInit::LastMillis-SaveGameDemo::starttime);
    gzputi(len);
    gzwrite(f, buf, len);
    gzput(extras);
    if(extras)
    {
        gzput(player1->gunselect);
        gzput(player1->lastattackgun);
        gzputi(player1->lastaction-SaveGameDemo::starttime);
        gzputi(player1->gunwait);
        gzputi(player1->health);
        gzputi(player1->armour);
        gzput(player1->armourtype);
        loopi(Gun::NUMGUNS) gzput(player1->ammo[i]);
        gzput(player1->state);
		gzputi(SaveGameDemo::bdamage);
		SaveGameDemo::bdamage = 0;
		gzputi(SaveGameDemo::ddamage);
		if(SaveGameDemo::ddamage)	{ gzputv(dorig); SaveGameDemo::ddamage = 0; };
        // FIXME: add all other client state which is not send through the network
    };
};

void demo(char *name)
{
    sprintf_sd(fn)("demos/%s.cdgz", name);
    loadstate(fn);
    SaveGameDemo::demoloading = true;
};

void stopreset()
{
    conoutf("demo stopped (%d msec elapsed)", GameInit::LastMillis-SaveGameDemo::starttime);
    stop();
    loopv(players) zapdynent(players[i]);
    disconnect(0, 0);
};

VAR(demoplaybackspeed, 10, 100, 1000);
int scaletime(int t) { return (int)(t*(100.0f/demoplaybackspeed))+SaveGameDemo::starttime; };

void readdemotime()
{   
    if(gzeof(f) || (SaveGameDemo::playbacktime = gzgeti())==-1)
    {
        stopreset();
        return;
    };
    SaveGameDemo::playbacktime = scaletime(SaveGameDemo::playbacktime);
};

void startdemo()
{
    ClientServer::ClientGame::DemoClientNum = gzgeti();
    GameInit::DemoPlayback = true;
    SaveGameDemo::starttime = GameInit::LastMillis;
    conoutf("now playing demo");
    dynent *d = getclient(ClientServer::ClientGame::DemoClientNum);
    assert(d);
    *d = *player1;
    readdemotime();
};

VAR(demodelaymsec, 0, 120, 500);

void catmulrom(vec &z, vec &a, vec &b, vec &c, float s, vec &dest)		// spline interpolation
{
	vec t1 = b, t2 = c;

	vsub(t1, z); vmul(t1, 0.5f)
	vsub(t2, a); vmul(t2, 0.5f);

	float s2 = s*s;
	float s3 = s*s2;

	dest = a;
	vec t = b;

	vmul(dest, 2*s3 - 3*s2 + 1);
	vmul(t,   -2*s3 + 3*s2);     vadd(dest, t);
    vmul(t1,     s3 - 2*s2 + s); vadd(dest, t1);
	vmul(t2,     s3 -   s2);     vadd(dest, t2);
};

void fixwrap(dynent *a, dynent *b)
{
	while(b->yaw-a->yaw>180)  a->yaw += 360;  
	while(b->yaw-a->yaw<-180) a->yaw -= 360;
};

void demoplaybackstep()
{
    while(GameInit::DemoPlayback && GameInit::LastMillis>=SaveGameDemo::playbacktime)
    {
        int len = gzgeti();
        if(len<1 || len>GameInit::MAXTRANS)
        {
            conoutf("error: huge packet during demo play (%d)", len);
            stopreset();
            return;
        };
        uchar buf[GameInit::MAXTRANS];
        gzread(f, buf, len);
        localservertoclient(buf, len);  // update game state
        
        dynent *target = players[ClientServer::ClientGame::DemoClientNum];
        assert(target); 
        
		int extras;
        if(extras = gzget())     // read additional client side state not present in normal network stream
        {
            target->gunselect = gzget();
            target->lastattackgun = gzget();
            target->lastaction = scaletime(gzgeti());
            target->gunwait = gzgeti();
            target->health = gzgeti();
            target->armour = gzgeti();
            target->armourtype = gzget();
            loopi(Gun::NUMGUNS) target->ammo[i] = gzget();
            target->state = gzget();
            target->lastmove = SaveGameDemo::playbacktime;
			if(SaveGameDemo::bdamage = gzgeti()) Render::RenderExtras::DamageBlend(SaveGameDemo::bdamage);
			if(SaveGameDemo::ddamage = gzgeti()) { gzgetv(dorig); particle_splash(3, SaveGameDemo::ddamage, 1000, dorig); };
            // FIXME: set more client state here
        };
        
        // insert latest copy of player into history
        if(extras && (playerhistory.empty() || playerhistory.last()->lastupdate!=SaveGameDemo::playbacktime))
        {
            dynent *d = newdynent();
            *d = *target;
            d->lastupdate = SaveGameDemo::playbacktime;
            playerhistory.add(d);
            if(playerhistory.length()>20)
            {
                zapdynent(playerhistory[0]);
                playerhistory.remove(0);
            };
        };
        
        readdemotime();
    };
    
    if(GameInit::DemoPlayback)
    {
        int itime = GameInit::LastMillis-demodelaymsec;
        loopvrev(playerhistory) if(playerhistory[i]->lastupdate<itime)      // find 2 positions in history that surround interpolation time point
        {
            dynent *a = playerhistory[i];
            dynent *b = a;
            if(i+1<playerhistory.length()) b = playerhistory[i+1];
            *player1 = *b;
            if(a!=b)                                // interpolate pos & angles
            {
				dynent *c = b;
				if(i+2<playerhistory.length()) c = playerhistory[i+2];
				dynent *z = a;
				if(i-1>=0) z = playerhistory[i-1];
				//if(a==z || b==c) printf("* %d\n", GameInit::LastMillis);
				float bf = (itime-a->lastupdate)/(float)(b->lastupdate-a->lastupdate);
				fixwrap(a, player1);
				fixwrap(c, player1);
				fixwrap(z, player1);
				vdist(dist, v, z->o, c->o);
				if(dist<16)		// if teleport or spawn, dont't interpolate
				{
					catmulrom(z->o, a->o, b->o, c->o, bf, player1->o);
					catmulrom(*(vec *)&z->yaw, *(vec *)&a->yaw, *(vec *)&b->yaw, *(vec *)&c->yaw, bf, *(vec *)&player1->yaw);
				};
				fixplayer1range();
			};
            break;
        };
        //if(player1->state!=CSStatus::CS_DEAD) showscores(false);
    };
};

void stopn() { if(GameInit::DemoPlayback) stopreset(); else stop(); conoutf("demo stopped"); };

COMMAND(record, Support::FunctionSignatures::ARG_1STR);
COMMAND(demo, Support::FunctionSignatures::ARG_1STR);
COMMANDN(stop, stopn, Support::FunctionSignatures::ARG_NONE);

COMMAND(savegame, Support::FunctionSignatures::ARG_1STR);
COMMAND(loadgame, Support::FunctionSignatures::ARG_1STR);
