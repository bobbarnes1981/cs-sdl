// clientextras.cpp: stuff that didn't fit in client.cpp or clientgame.cpp :)

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::ClientServer;


// render players & monsters
// very messy ad-hoc handling of animation frames, should be made more configurable

//              D    D    D    D'   D    D    D    D'   A   A'  P   P'  I   I' R,  R'  E    L    J   J'
int frame[] = { 178, 184, 190, 137, 183, 189, 197, 164, 46, 51, 54, 32, 0,  0, 40, 1,  162, 162, 67, 168 };
int range[] = { 6,   6,   8,   28,  1,   1,   1,   1,   8,  19, 4,  18, 40, 1, 6,  15, 1,   1,   1,  1   };

void renderclient(dynent *d, bool team, char *mdlname, bool hellpig, float scale)
{
    int n = 3;
    float speed = 100.0f;
    float mz = d->o.z-d->eyeheight+1.55f*scale;
    int basetime = -((int)d&0xFFF);
    if(d->state==CSStatus::CS_DEAD)
    {
        int r;
        if(hellpig) { n = 2; r = range[3]; } else { n = (int)d%3; r = range[n]; };
        basetime = d->lastaction;
        int t = GameInit::LastMillis-d->lastaction;
        if(t<0 || t>20000) return;
        if(t>(r-1)*100) { n += 4; if(t>(r+10)*100) { t -= (r+10)*100; mz -= t*t/10000000000.0f*t; }; };
        if(mz<-1000) return;
        //mdl = (((int)d>>6)&1)+1;
        //mz = d->o.z-d->eyeheight+0.2f;
        //scale = 1.2f;
    }
    else if(d->state==CSStatus::CS_EDITING)                   { n = 16; }
    else if(d->state==CSStatus::CS_LAGGED)                    { n = 17; }
    else if(d->monsterstate==MonsterStates::M_ATTACKING)           { n = 8;  }
    else if(d->monsterstate==MonsterStates::M_PAIN)                { n = 10; } 
    else if((!d->move && !d->strafe) || !d->moving) { n = 12; } 
    else if(!d->onfloor && d->timeinair>100)        { n = 18; }
    else                                            { n = 14; speed = 1200/d->maxspeed*scale; if(hellpig) speed = 300/d->maxspeed;  }; 
    if(hellpig) { n++; scale *= 32; mz -= 1.9f; };
	rendermodel(mdlname, frame[n], range[n], 0, 1.5f, d->o.x, mz, d->o.y, d->yaw+90, d->pitch/2, team, scale, speed, 0, basetime);
};

void renderclients()
{
    dynent *d;
    loopv(players) if((d = players[i]) && (!GameInit::DemoPlayback || i!=ClientGame::DemoClientNum)) renderclient(d, isteam(player1->team, d->team), "monster/ogro", false, 1.0f);
};

// creation of scoreboard pseudo-menu

void showscores(bool on)
{
    ClientExtras::scoreson = on;
    menuset(((int)on)-1);
};

struct sline { string s; };
vector<sline> scorelines;

void renderscore(dynent *d)
{
    sprintf_sd(lag)("%d", d->plag);
    sprintf_sd(name) ("(%s)", d->name); 
    sprintf_s(scorelines.add().s)("%d\t%s\t%d\t%s\t%s", d->frags, d->state==CSStatus::CS_LAGGED ? "LAG" : lag, d->ping, d->team, d->state==CSStatus::CS_DEAD ? name : d->name);
    menumanual(0, scorelines.length()-1, scorelines.last().s); 
};

char *teamname[ClientExtras::maxteams];
int teamscore[ClientExtras::maxteams];
string teamscores;

void addteamscore(dynent *d)
{
    if(!d) return;
    loopi(ClientExtras::teamsused) if(strcmp(teamname[i], d->team)==0) { teamscore[i] += d->frags; return; };
    if(ClientExtras::teamsused==ClientExtras::maxteams) return;
    teamname[ClientExtras::teamsused] = d->team;
    teamscore[ClientExtras::teamsused++] = d->frags;
};

void renderscores()
{
    if(!ClientExtras::scoreson) return;
    scorelines.setsize(0);
    if(!GameInit::DemoPlayback) renderscore(player1);
    loopv(players) if(players[i]) renderscore(players[i]);
    sortmenu(0, scorelines.length());
    if(m_teammode)
    {
        ClientExtras::teamsused = 0;
        loopv(players) addteamscore(players[i]);
        if(!GameInit::DemoPlayback) addteamscore(player1);
        teamscores[0] = 0;
        loopj(ClientExtras::teamsused)
        {
            sprintf_sd(sc)("[ %s: %d ]", teamname[j], teamscore[j]);
            strcat_s(teamscores, sc);
        };
        menumanual(0, scorelines.length(), "");
        menumanual(0, scorelines.length()+1, teamscores);
    };
};

// sendmap/getmap commands, should be replaced by more intuitive map downloading

void sendmap(char *mapname)
{
    if(*mapname) save_world(mapname);
    changemap(mapname);
    mapname = getclientmap();
    int mapsize;
    uchar *mapdata = readmap(mapname, &mapsize); 
    if(!mapdata) return;
    ENetPacket *packet = enet_packet_create(NULL, GameInit::MAXTRANS + mapsize, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, NetworkMessages::SV_SENDMAP);
    sendstring(mapname, p);
    putint(p, mapsize);
    if(65535 - (p - start) < mapsize)
    {
        conoutf("map %s is too large to send", mapname);
        free(mapdata);
        enet_packet_destroy(packet);
        return;
    };
    memcpy(p, mapdata, mapsize);
    p += mapsize;
    free(mapdata); 
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    sendpackettoserv(packet);
    conoutf("sending map %s to server...", mapname);
    sprintf_sd(msg)("[map %s uploaded to server, \"getmap\" to receive it]", mapname);
    toserver(msg);
}

void getmap()
{
    ENetPacket *packet = enet_packet_create(NULL, GameInit::MAXTRANS, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, NetworkMessages::SV_RECVMAP);
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    sendpackettoserv(packet);
    conoutf("requesting map from server...");
}

COMMAND(sendmap, Support::FunctionSignatures::ARG_1STR);
COMMAND(getmap, Support::FunctionSignatures::ARG_NONE);

