// client processing of the incoming network stream

#include "cube.h"
#using <mscorlib.dll>
#using <MezzanineLib.dll>

extern string toservermap;
extern string clientpassword;

void neterr(char *s)
{
    conoutf("illegal network message (%s)", s);
    disconnect();
};

void changemapserv(char *name, int mode)        // forced map change from the server
{
    gamemode = mode;
    load_world(name);
};

void changemap(char *name)                      // request map change, server may ignore
{
    strcpy_s(toservermap, name);
};

// update the position of other clients in the game in our world
// don't care if he's in the scenery or other players,
// just don't overlap with our client

void updatepos(dynent *d)
{
    const float r = player1->radius+d->radius;
    const float dx = player1->o.x-d->o.x;
    const float dy = player1->o.y-d->o.y;
    const float dz = player1->o.z-d->o.z;
    const float rz = player1->aboveeye+d->eyeheight;
    const float fx = (float)fabs(dx), fy = (float)fabs(dy), fz = (float)fabs(dz);
    if(fx<r && fy<r && fz<rz && d->state!=MezzanineLib::CSStatus::CS_DEAD)
    {
        if(fx<fy) d->o.y += dy<0 ? r-fy : -(r-fy);  // push aside
        else      d->o.x += dx<0 ? r-fx : -(r-fx);
    };
    int lagtime = MezzanineLib::GameInit::LastMillis-d->lastupdate;
    if(lagtime)
    {
        d->plag = (d->plag*5+lagtime)/6;
        d->lastupdate = MezzanineLib::GameInit::LastMillis;
    };
};

void localservertoclient(uchar *buf, int len)   // processes any updates from the server
{
    if(ENET_NET_TO_HOST_16(*(ushort *)buf)!=len) neterr("packet length");
    incomingdemodata(buf, len);
    
    uchar *end = buf+len;
    uchar *p = buf+2;
    char text[MezzanineLib::GameInit::MAXTRANS];
    int cn = -1, type;
    dynent *d = NULL;
    bool mapchanged = false;

    while(p<end) switch(type = getint(p))
    {
        case MezzanineLib::NetworkMessages::SV_INITS2C:                    // welcome messsage from the server
        {
            cn = getint(p);
            int prot = getint(p);
            if(prot!=MezzanineLib::GameInit::PROTOCOL_VERSION)
            {
                conoutf("you are using a different game protocol (you: %d, server: %d)", MezzanineLib::GameInit::PROTOCOL_VERSION, prot);
                disconnect();
                return;
            };
            toservermap[0] = 0;
            MezzanineLib::ClientServer::Client::ClientNum = cn;                 // we are now fully connected
            if(!getint(p)) strcpy_s(toservermap, getclientmap());   // we are the first client on this server, set map
            sgetstr();
           /* if(text[0] && strcmp(text, clientpassword))
            {
                conoutf("you need to set the correct password to join this server!");
                disconnect();
                return;
            };*/
            if(getint(p)==1)
            {
                conoutf("server is FULL, disconnecting..");
            };
            break;
        };

        case MezzanineLib::NetworkMessages::SV_POS:                        // position of another client
        {
            cn = getint(p);
            d = getclient(cn);
            if(!d) return;
            d->o.x   = getint(p)/MezzanineLib::GameInit::DMF;
            d->o.y   = getint(p)/MezzanineLib::GameInit::DMF;
            d->o.z   = getint(p)/MezzanineLib::GameInit::DMF;
            d->yaw   = getint(p)/MezzanineLib::GameInit::DAF;
            d->pitch = getint(p)/MezzanineLib::GameInit::DAF;
            d->roll  = getint(p)/MezzanineLib::GameInit::DAF;
            d->vel.x = getint(p)/MezzanineLib::GameInit::DVF;
            d->vel.y = getint(p)/MezzanineLib::GameInit::DVF;
            d->vel.z = getint(p)/MezzanineLib::GameInit::DVF;
            int f = getint(p);
            d->strafe = (f&3)==3 ? -1 : f&3;
            f >>= 2; 
            d->move = (f&3)==3 ? -1 : f&3;
            d->onfloor = (f>>2)&1;
            int state = f>>3;
            if(state==MezzanineLib::CSStatus::CS_DEAD && d->state!=MezzanineLib::CSStatus::CS_DEAD) d->lastaction = MezzanineLib::GameInit::LastMillis;
            d->state = state;
            if(!MezzanineLib::GameInit::DemoPlayback) updatepos(d);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_SOUND:
            playsound(getint(p), &d->o);
            break;

        case MezzanineLib::NetworkMessages::SV_TEXT:
            sgetstr();
            conoutf("%s:\f %s", d->name, text); 
            break;

        case MezzanineLib::NetworkMessages::SV_MAPCHANGE:     
            sgetstr();
            changemapserv(text, getint(p));
            mapchanged = true;
            break;
        
        case MezzanineLib::NetworkMessages::SV_ITEMLIST:
        {
            int n;
            if(mapchanged) { MezzanineLib::ClientServer::Client::SendItemsToServer = false; resetspawns(); };
            while((n = getint(p))!=-1) if(mapchanged) setspawn(n, true);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_MAPRELOAD:          // server requests next map
        {
            getint(p);
            sprintf_sd(nextmapalias)("nextmap_%s", getclientmap());
            char *map = getalias(nextmapalias);     // look up map in the cycle
            changemap(map ? map : getclientmap());
            break;
        };

        case MezzanineLib::NetworkMessages::SV_INITC2S:            // another client either connected or changed name/team
        {
            sgetstr();
            if(d->name[0])          // already connected
            {
                if(strcmp(d->name, text))
                    conoutf("%s is now known as %s", d->name, text);
            }
            else                    // new client
            {
                MezzanineLib::ClientServer::Client::c2sinit = false;    // send new players my info again 
                conoutf("connected: %s", text);
            }; 
            strcpy_s(d->name, text);
            sgetstr();
            strcpy_s(d->team, text);
            d->lifesequence = getint(p);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_CDIS:
            cn = getint(p);
            if(!(d = getclient(cn))) break;
			conoutf("player %s disconnected", d->name[0] ? d->name : "[incompatible client]"); 
            zapdynent(players[cn]);
            break;

        case MezzanineLib::NetworkMessages::SV_SHOT:
        {
            int gun = getint(p);
            vec s, e;
            s.x = getint(p)/MezzanineLib::GameInit::DMF;
            s.y = getint(p)/MezzanineLib::GameInit::DMF;
            s.z = getint(p)/MezzanineLib::GameInit::DMF;
            e.x = getint(p)/MezzanineLib::GameInit::DMF;
            e.y = getint(p)/MezzanineLib::GameInit::DMF;
            e.z = getint(p)/MezzanineLib::GameInit::DMF;
            if(gun==MezzanineLib::Gun::GUN_SG) createrays(s, e);
            shootv(gun, s, e, d);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_DAMAGE:             
        {
            int target = getint(p);
            int damage = getint(p);
            int ls = getint(p);
            if(target==MezzanineLib::ClientServer::Client::ClientNum) { if(ls==player1->lifesequence) selfdamage(damage, cn, d); }
            else playsound(MezzanineLib::Sounds::S_PAIN1+rnd(5), &getclient(target)->o);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_DIED:
        {
            int actor = getint(p);
            if(actor==cn)
            {
                conoutf("%s suicided", d->name);
            }
            else if(actor==MezzanineLib::ClientServer::Client::ClientNum)
            {
                int frags;
                if(isteam(player1->team, d->team))
                {
                    frags = -1;
                    conoutf("you fragged a teammate (%s)", d->name);
                }
                else
                {
                    frags = 1;
                    conoutf("you fragged %s", d->name);
                };
                addmsg(1, 2, MezzanineLib::NetworkMessages::SV_FRAGS, player1->frags += frags);
            }
            else
            {
                dynent *a = getclient(actor);
                if(a)
                {
                    if(isteam(a->team, d->name))
                    {
                        conoutf("%s fragged his teammate (%s)", a->name, d->name);
                    }
                    else
                    {
                        conoutf("%s fragged %s", a->name, d->name);
                    };
                };
            };
            playsound(MezzanineLib::Sounds::S_DIE1+rnd(2), &d->o);
            d->lifesequence++;
            break;
        };

        case MezzanineLib::NetworkMessages::SV_FRAGS:
            players[cn]->frags = getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_ITEMPICKUP:
            setspawn(getint(p), false);
            getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_ITEMSPAWN:
        {
            uint i = getint(p);
            setspawn(i, true);
            if(i>=(uint)ents.length()) break;
            vec v = { ents[i].x, ents[i].y, ents[i].z };
            playsound(MezzanineLib::Sounds::S_ITEMSPAWN, &v); 
            break;
        };

        case MezzanineLib::NetworkMessages::SV_ITEMACC:            // server acknowledges that I picked up this item
            realpickup(getint(p), player1);
            break;

        case MezzanineLib::NetworkMessages::SV_EDITH:              // coop editing messages, should be extended to include all possible editing ops
        case MezzanineLib::NetworkMessages::SV_EDITT:
        case MezzanineLib::NetworkMessages::SV_EDITS:
        case MezzanineLib::NetworkMessages::SV_EDITD:
        case MezzanineLib::NetworkMessages::SV_EDITE:
        {
            int x  = getint(p);
            int y  = getint(p);
            int xs = getint(p);
            int ys = getint(p);
            int v  = getint(p);
            block b = { x, y, xs, ys };
            switch(type)
            {
                case MezzanineLib::NetworkMessages::SV_EDITH: editheightxy(v!=0, getint(p), b); break;
                case MezzanineLib::NetworkMessages::SV_EDITT: edittexxy(v, getint(p), b); break;
                case MezzanineLib::NetworkMessages::SV_EDITS: edittypexy(v, b); break;
                case MezzanineLib::NetworkMessages::SV_EDITD: setvdeltaxy(v, b); break;
                case MezzanineLib::NetworkMessages::SV_EDITE: editequalisexy(v!=0, b); break;
            };
            break;
        };

        case MezzanineLib::NetworkMessages::SV_EDITENT:            // coop edit of ent
        {
            uint i = getint(p);
            while((uint)ents.length()<=i) ents.add().type = MezzanineLib::StaticEntity::NOTUSED;
            int to = ents[i].type;
            ents[i].type = getint(p);
            ents[i].x = getint(p);
            ents[i].y = getint(p);
            ents[i].z = getint(p);
            ents[i].attr1 = getint(p);
            ents[i].attr2 = getint(p);
            ents[i].attr3 = getint(p);
            ents[i].attr4 = getint(p);
            ents[i].spawned = false;
            if(ents[i].type==MezzanineLib::StaticEntity::LIGHT || to==MezzanineLib::StaticEntity::LIGHT) calclight();
            break;
        };

        case MezzanineLib::NetworkMessages::SV_PING:
            getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_PONG: 
            addmsg(0, 2, MezzanineLib::NetworkMessages::SV_CLIENTPING, player1->ping = (player1->ping*5+MezzanineLib::GameInit::LastMillis-getint(p))/6);
            break;

        case MezzanineLib::NetworkMessages::SV_CLIENTPING:
            players[cn]->ping = getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_GAMEMODE:
            nextmode = getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_TIMEUP:
            timeupdate(getint(p));
            break;

        case MezzanineLib::NetworkMessages::SV_RECVMAP:
        {
            sgetstr();
            conoutf("received map \"%s\" from server, reloading..", text);
            int mapsize = getint(p);
            writemap(text, mapsize, p);
            p += mapsize;
            changemapserv(text, gamemode);
            break;
        };
        
        case MezzanineLib::NetworkMessages::SV_SERVMSG:
            sgetstr();
            conoutf("%s", text);
            break;

        case MezzanineLib::NetworkMessages::SV_EXT:        // so we can messages without breaking previous clients/servers, if necessary
        {
            for(int n = getint(p); n; n--) getint(p);
            break;
        };

        default:
            neterr("type");
            return;
    };
};
