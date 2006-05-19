// clientgame.cpp: core game related stuff

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::ClientServer;


int nextmode = 0;         // nextmode becomes gamemode after next map load
VAR(gamemode, 1, 0, 0);

VARF(gamespeed, 10, 100, 1000, if(multiplayer()) gamespeed = 100);

void mode(int n) { addmsg(1, 2, NetworkMessages::SV_GAMEMODE, nextmode = n); };
COMMAND(mode, Support::FunctionSignatures::ARG_1INT);

dynent *player1 = newdynent();          // our client
dvector players;   // other clients

dynent * getplayer1(void)
{
	return player1;
}

VARP(sensitivity, 0, 10, 10000);
VARP(sensitivityscale, 1, 1, 10000);
VARP(invmouse, 0, 0, 1);

//GameInit::CurrentTime = 10;
string clientmap;

char *getclientmap() { return clientmap; };

void resetmovement(dynent *d)
{
    d->k_left = false;
    d->k_right = false;
    d->k_up = false;
    d->k_down = false;  
    d->jumpnext = false;
    d->strafe = 0;
    d->move = 0;
};

void spawnstate(dynent *d)              // reset player state not persistent across spawns
{
    resetmovement(d);
    d->vel.x = d->vel.y = d->vel.z = 0; 
    d->onfloor = false;
    d->timeinair = 0;
    d->health = 100;
    d->armour = 50;
    d->armourtype = ArmorTypes::A_BLUE;
    d->quadmillis = 0;
    d->lastattackgun = d->gunselect = Gun::GUN_SG;
    d->gunwait = 0;
	d->attacking = false;
    d->lastaction = 0;
    loopi(Gun::NUMGUNS) d->ammo[i] = 0;
    d->ammo[Gun::GUN_FIST] = 1;
    if(m_noitems)
    {
        d->gunselect = Gun::GUN_RIFLE;
        d->armour = 0;
        if(m_noitemsrail)
        {
            d->health = 1;
            d->ammo[Gun::GUN_RIFLE] = 100;
        }
        else
        {
            if(gamemode==12) { d->gunselect = Gun::GUN_FIST; return; };  // eihrul's secret "instafist" mode
            d->health = 256;
            if(m_tarena)
            {
                int gun1 = rnd(4)+1;
                baseammo(d->gunselect = gun1);
                for(;;)
                {
                    int gun2 = rnd(4)+1;
                    if(gun1!=gun2) { baseammo(gun2); break; };
                };
            }
            else if(m_arena)    // insta arena
            {
                d->ammo[Gun::GUN_RIFLE] = 100;
            }
            else // efficiency
            {
                loopi(4) baseammo(i+1);
                d->gunselect = Gun::GUN_CG;
            };
            d->ammo[Gun::GUN_CG] /= 2;
        };
    }
    else
    {
        d->ammo[Gun::GUN_SG] = 5;
    };
};
    
dynent *newdynent()                 // create a new blank player or monster
{
    dynent *d = (dynent *)gp()->alloc(sizeof(dynent));
    d->o.x = 0;
    d->o.y = 0;
    d->o.z = 0;
    d->yaw = 270;
    d->pitch = 0;
    d->roll = 0;
    d->maxspeed = 22;
    d->outsidemap = false;
    d->inwater = false;
    d->radius = 1.1f;
    d->eyeheight = 3.2f;
    d->aboveeye = 0.7f;
    d->frags = 0;
    d->plag = 0;
    d->ping = 0;
    d->lastupdate = GameInit::LastMillis;
    d->enemy = NULL;
    d->monsterstate = 0;
    d->name[0] = d->team[0] = 0;
    d->blocked = false;
    d->lifesequence = 0;
    d->state = CSStatus::CS_ALIVE;
    spawnstate(d);
    return d;
};

void respawnself()
{
	spawnplayer(player1);
	showscores(false);
};

void arenacount(dynent *d, int &alive, int &dead, char *&lastteam, bool &oneteam)
{
    if(d->state!=CSStatus::CS_DEAD)
    {
        alive++;
        if(lastteam && strcmp(lastteam, d->team)) oneteam = false;
        lastteam = d->team;
    }
    else
    {
        dead++;
    };
};

void arenarespawn()
{
    if(ClientGame::arenarespawnwait)
    {
        if(ClientGame::arenarespawnwait<GameInit::LastMillis)
        {
            ClientGame::arenarespawnwait = 0;
            conoutf("new round starting... fight!");
            respawnself();
        };
    }
    else if(ClientGame::arenadetectwait==0 || ClientGame::arenadetectwait<GameInit::LastMillis)
    {
        ClientGame::arenadetectwait = 0;
        int alive = 0, dead = 0;
        char *lastteam = NULL;
        bool oneteam = true;
        loopv(players) if(players[i]) arenacount(players[i], alive, dead, lastteam, oneteam);
        arenacount(player1, alive, dead, lastteam, oneteam);
        if(dead>0 && (alive<=1 || (m_teammode && oneteam)))
        {
            conoutf("arena round is over! next round in 5 seconds...");
            if(alive) conoutf("team %s is last man standing", lastteam);
            else conoutf("everyone died!");
            ClientGame::arenarespawnwait = GameInit::LastMillis+5000;
            ClientGame::arenadetectwait  = GameInit::LastMillis+10000;
            player1->roll = 0;
        }; 
    };
};

void zapdynent(dynent *&d)
{
    if(d) gp()->dealloc(d, sizeof(dynent));
    d = NULL;
};

void otherplayers()
{
    loopv(players) if(players[i])
    {
        const int lagtime = GameInit::LastMillis-players[i]->lastupdate;
        if(lagtime>1000 && players[i]->state==CSStatus::CS_ALIVE)
        {
            players[i]->state = CSStatus::CS_LAGGED;
            continue;
        };
        if(lagtime && players[i]->state != CSStatus::CS_DEAD && (!GameInit::DemoPlayback || i!=ClientGame::DemoClientNum)) moveplayer(players[i], 2, false);   // use physics to extrapolate player position
    };
};

void respawn()
{
    if(player1->state==CSStatus::CS_DEAD)
    { 
        player1->attacking = false;
        if(m_arena) { conoutf("waiting for new round to start..."); return; };
        if(m_sp) { nextmode = gamemode; changemap(getclientmap()); return; };    // if we die in SP we try the same map again
		respawnself();
	};
};

string sleepcmd;
void sleepf(char *msec, char *cmd) { ClientGame::sleepwait = atoi(msec)+GameInit::LastMillis; strcpy_s(sleepcmd, cmd); };
COMMANDN(sleep, sleepf, Support::FunctionSignatures::ARG_2STR);

void updateworld(int millis)        // main game update loop
{
    if(GameInit::LastMillis)
    {     
        GameInit::CurrentTime = millis - GameInit::LastMillis;
        if(ClientGame::sleepwait && GameInit::LastMillis>ClientGame::sleepwait) { ClientGame::sleepwait = 0; execute(sleepcmd); };
		Game::Physics::PhysicsFrame();
        checkquad(GameInit::CurrentTime);
		if(m_arena) arenarespawn();
        moveprojectiles((float)GameInit::CurrentTime);
        demoplaybackstep();
        if(!GameInit::DemoPlayback)
        {
            if(Client::ClientNum>=0) shoot(player1, worldpos);     // only shoot when connected to server
            gets2c();           // do this first, so we have most accurate information when our player moves
        };
        otherplayers();
        if(!GameInit::DemoPlayback)
        {
            monsterthink();
            if(player1->state==CSStatus::CS_DEAD)
            {
				if(GameInit::LastMillis-player1->lastaction<2000)
				{
					player1->move = player1->strafe = 0;
					moveplayer(player1, 10, false);
				}
                else if(!m_arena && !m_sp && GameInit::LastMillis-player1->lastaction>10000) respawn();
            }
            else if(!ClientGame::intermission)
            {
                moveplayer(player1, 20, true);
                checkitems();
            };
            c2sinfo(player1);   // do this last, to reduce the effective frame lag
        };
    };
    GameInit::LastMillis = millis;
};

void entinmap(dynent *d)    // brute force but effective way to find a free spawn spot in the map
{
    loopi(100)              // try max 100 times
    {
        float dx = (rnd(21)-10)/10.0f*i;  // increasing distance
        float dy = (rnd(21)-10)/10.0f*i;
        d->o.x += dx;
        d->o.y += dy;
        if(collide(d, true, 0, 0)) return;
        d->o.x -= dx;
        d->o.y -= dy;
    };
    conoutf("can't find entity spawn spot! (%d, %d)", (int)d->o.x, (int)d->o.y);
    // leave ent at original pos, possibly stuck
};

void spawnplayer(dynent *d)   // place at random spawn. also used by monsters!
{
    int r = ClientGame::fixspawn-->0 ? 4 : rnd(10)+1;
    loopi(r) ClientGame::spawncycle = findentity(StaticEntity::PLAYERSTART, ClientGame::spawncycle+1);
    if(ClientGame::spawncycle!=-1)
    {
        d->o.x = ents[ClientGame::spawncycle].x;
        d->o.y = ents[ClientGame::spawncycle].y;
        d->o.z = ents[ClientGame::spawncycle].z;
        d->yaw = ents[ClientGame::spawncycle].attr1;
        d->pitch = 0;
        d->roll = 0;
    }
    else
    {
        d->o.x = d->o.y = (float)GameInit::SSize/2;
        d->o.z = 4;
    };
    entinmap(d);
    spawnstate(d);
    d->state = CSStatus::CS_ALIVE;
};

// movement input code

#define dir(name,v,d,s,os) void name(bool isdown) { player1->s = isdown; player1->v = isdown ? d : (player1->os ? -(d) : 0); player1->lastmove = GameInit::LastMillis; };

dir(backward, move,   -1, k_down,  k_up);
dir(forward,  move,    1, k_up,    k_down);
dir(left,     strafe,  1, k_left,  k_right); 
dir(right,    strafe, -1, k_right, k_left); 

void attack(bool on)
{
    if(ClientGame::intermission) return;
    if(GameInit::EditMode) editdrag(on);
    else if(player1->attacking = on) respawn();
};

void jumpn(bool on) { if(!ClientGame::intermission && (player1->jumpnext = on)) respawn(); };

COMMAND(backward, Support::FunctionSignatures::ARG_DOWN);
COMMAND(forward, Support::FunctionSignatures::ARG_DOWN);
COMMAND(left, Support::FunctionSignatures::ARG_DOWN);
COMMAND(right, Support::FunctionSignatures::ARG_DOWN);
COMMANDN(jump, jumpn, Support::FunctionSignatures::ARG_DOWN);
COMMAND(attack, Support::FunctionSignatures::ARG_DOWN);
COMMAND(showscores, Support::FunctionSignatures::ARG_DOWN);

void fixplayer1range()
{
    const float MAXPITCH = 90.0f;
    if(player1->pitch>MAXPITCH) player1->pitch = MAXPITCH;
    if(player1->pitch<-MAXPITCH) player1->pitch = -MAXPITCH;
    while(player1->yaw<0.0f) player1->yaw += 360.0f;
    while(player1->yaw>=360.0f) player1->yaw -= 360.0f;
};

void mousemove(int dx, int dy)
{
    if(player1->state==CSStatus::CS_DEAD || ClientGame::intermission) return;
    const float SENSF = 33.0f;     // try match quake sens
    player1->yaw += (dx/SENSF)*(sensitivity/(float)sensitivityscale);
    player1->pitch -= (dy/SENSF)*(sensitivity/(float)sensitivityscale)*(invmouse ? -1 : 1);
	fixplayer1range();
};

// damage arriving from the network, monsters, yourself, all ends up here.

void selfdamage(int damage, int actor, dynent *act)
{
    if(player1->state!=CSStatus::CS_ALIVE || GameInit::EditMode || ClientGame::intermission) return;
    Render::RenderExtras::DamageBlend(damage);
	demoblend(damage);
    int ad = damage*(player1->armourtype+1)*20/100;     // let armour absorb when possible
    if(ad>player1->armour) ad = player1->armour;
    player1->armour -= ad;
    damage -= ad;
    float droll = damage/0.5f;
    player1->roll += player1->roll>0 ? droll : (player1->roll<0 ? -droll : (rnd(2) ? droll : -droll));  // give player a kick depending on amount of damage
    if((player1->health -= damage)<=0)
    {
        if(actor==-2)
        {
            conoutf("you got killed by %s!", act->name);
        }
        else if(actor==-1)
        {
            actor = Client::ClientNum;
            conoutf("you suicided!");
            addmsg(1, 2, NetworkMessages::SV_FRAGS, --player1->frags);
        }
        else
        {
            dynent *a = getclient(actor);
            if(a)
            {
                if(isteam(a->team, player1->team))
                {
                    conoutf("you got fragged by a teammate (%s)", a->name);
                }
                else
                {
                    conoutf("you got fragged by %s", a->name);
                };
            };
        };
        showscores(true);
        addmsg(1, 2, NetworkMessages::SV_DIED, actor);
        player1->lifesequence++;
        player1->attacking = false;
        player1->state = CSStatus::CS_DEAD;
        player1->pitch = 0;
        player1->roll = 60;
        playsound(Sounds::S_DIE1+rnd(2));
        spawnstate(player1);
        player1->lastaction = GameInit::LastMillis;
    }
    else
    {
        playsound(Sounds::S_PAIN6);
    };
};

void timeupdate(int timeremain)
{
    if(!timeremain)
    {
        ClientGame::intermission = true;
        player1->attacking = false;
        conoutf("intermission:");
        conoutf("game has ended!");
        showscores(true);
    }
    else
    {
        conoutf("time remaining: %d minutes", timeremain);
    };
};

dynent *getclient(int cn)   // ensure valid entity
{
	if(cn<0 || cn>=GameInit::MAXCLIENTS)
    {
        neterr("clientnum");
        return NULL;
    };
    while(cn>=players.length()) players.add(NULL);
    return players[cn] ? players[cn] : (players[cn] = newdynent());
};

void startmap(char *name)   // called just after a map load
{
    if(netmapstart() && m_sp) { gamemode = 0; conoutf("coop sp not supported yet"); };
    ClientGame::sleepwait = 0;
    monsterclear();
    projreset(); 
    ClientGame::spawncycle = -1;
    spawnplayer(player1);
    player1->frags = 0;
    loopv(players) if(players[i]) players[i]->frags = 0;
    resetspawns();
    strcpy_s(getclientmap(), name);
    if(GameInit::EditMode) toggleedit();
    setvar("gamespeed", 100);
	//setvar("fog", 180);
	//setvar("fogcolour", 0x8099B3);
	Render::RenderGl::Fog = 180;
	Render::RenderGl::FogColour = 0x8099B3;
    showscores(false);
    ClientGame::intermission = false;
    ClientGame::FramesInMap = 0;
    conoutf("game mode is %s", modestr(gamemode));
}; 

COMMANDN(map, changemap, Support::FunctionSignatures::ARG_1STR);

void quit()                     // normal exit
{
	GameInit::Quit();
};

void screenshot()
{
	GameInit::Screenshot();
};

COMMAND(screenshot, Support::FunctionSignatures::ARG_NONE);
COMMAND(quit, Support::FunctionSignatures::ARG_NONE);
