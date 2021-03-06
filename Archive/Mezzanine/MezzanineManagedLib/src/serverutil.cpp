// misc useful functions used by the server

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::ClientServer;


// all network traffic is in 32bit ints, which are then compressed using the following simple scheme (assumes that most values are small).

void putint(uchar *&p, int n)
{
    if(n<128 && n>-127) { *p++ = n; }
    else if(n<0x8000 && n>=-0x8000) { *p++ = 0x80; *p++ = n; *p++ = n>>8;  }
    else { *p++ = 0x81; *p++ = n; *p++ = n>>8; *p++ = n>>16; *p++ = n>>24; };
};

int getint(uchar *&p)
{
    int c = *((char *)p);
    p++;
    if(c==-128) { int n = *p++; n |= *((char *)p)<<8; p++; return n;}
    else if(c==-127) { int n = *p++; n |= *p++<<8; n |= *p++<<16; return n|(*p++<<24); } 
    else return c;
};

void sendstring(char *t, uchar *&p)
{
    while(*t) putint(p, *t++);
    putint(p, 0);
};

const char *modenames[] =
{
    "SP", "DMSP", "ffa/default", "coopedit", "ffa/duel", "teamplay",
    "instagib", "instagib team", "efficiency", "efficiency team",
    "insta arena", "insta clan arena", "tactics arena", "tactics clan arena",
};
      
const char *modestr(int n) { return (n>=-2 && n<12) ? modenames[n+2] : "unknown"; };

char msgsizesl[] =               // size inclusive message token, 0 for variable or not-checked sizes
{ 
    NetworkMessages::SV_INITS2C, 4, NetworkMessages::SV_INITC2S, 0, NetworkMessages::SV_POS, 12, NetworkMessages::SV_TEXT, 0, NetworkMessages::SV_SOUND, 2, NetworkMessages::SV_CDIS, 2,
    NetworkMessages::SV_EDITH, 7, NetworkMessages::SV_EDITT, 7, NetworkMessages::SV_EDITS, 6, NetworkMessages::SV_EDITD, 6, NetworkMessages::SV_EDITE, 6,
    NetworkMessages::SV_DIED, 2, NetworkMessages::SV_DAMAGE, 4, NetworkMessages::SV_SHOT, 8, NetworkMessages::SV_FRAGS, 2,
    NetworkMessages::SV_MAPCHANGE, 0, NetworkMessages::SV_ITEMSPAWN, 2, NetworkMessages::SV_ITEMPICKUP, 3, NetworkMessages::SV_DENIED, 2,
    NetworkMessages::SV_PING, 2, NetworkMessages::SV_PONG, 2, NetworkMessages::SV_CLIENTPING, 2, NetworkMessages::SV_GAMEMODE, 2,
    NetworkMessages::SV_TIMEUP, 2, NetworkMessages::SV_EDITENT, 10, NetworkMessages::SV_MAPRELOAD, 2, NetworkMessages::SV_ITEMACC, 2,
    NetworkMessages::SV_SENDMAP, 0, NetworkMessages::SV_RECVMAP, 1, NetworkMessages::SV_SERVMSG, 0, NetworkMessages::SV_ITEMLIST, 0,
    NetworkMessages::SV_EXT, 0,
    -1
};

char msgsizelookup(int msg)
{
    for(char *p = msgsizesl; *p>=0; p += 2) if(*p==msg) return p[1];
    return -1;
};

// sending of maps between clients

string copyname;
uchar *copydata = NULL;

void sendmaps(int n, string mapname, int mapsize, uchar *mapdata)
{
    if(mapsize <= 0 || mapsize > 256*256) return;
    strcpy_s(copyname, mapname);
    ServerUtil::copysize = mapsize;
    if(copydata) free(copydata);
    copydata = (uchar *)alloc(mapsize);
    memcpy(copydata, mapdata, mapsize);
}

ENetPacket *recvmap(int n)
{
    if(!copydata) return NULL;
    ENetPacket *packet = enet_packet_create(NULL, GameInit::MAXTRANS + ServerUtil::copysize, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, NetworkMessages::SV_RECVMAP);
    sendstring(copyname, p);
    putint(p, ServerUtil::copysize);
    memcpy(p, copydata, ServerUtil::copysize);
    p += ServerUtil::copysize;
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
	return packet;
}


#ifdef STANDALONE

void localservertoclient(uchar *buf, int len) {};
//void fatal(char *s, char *o) { cleanupserver(); printf("servererror: %s\n", s); exit(1); };
void *alloc(int s) { void *b = calloc(1,s); if(!b) GameInit::Fatal("no memory!"); return b; };

int main(int argc, char* argv[])
{
    int uprate = 0, maxcl = 4;
    char *sdesc = "", *ip = "", *master = NULL, *passwd = "";
    
    for(int i = 1; i<argc; i++)
    {
        char *a = &argv[i][2];
        if(argv[i][0]=='-') switch(argv[i][1])
        {
            case 'u': uprate = atoi(a); break;
            case 'n': sdesc  = a; break;
            case 'i': ip     = a; break;
            case 'm': master = a; break;
            case 'p': passwd = a; break;
            case 'c': maxcl  = atoi(a); break;
            default: printf("WARNING: unknown commandline option\n");
        };
    };
    
    if(enet_initialize()<0) GameInit::Fatal("Unable to initialise network module");
    initserver(true, uprate, sdesc, ip, master, passwd, maxcl);
    return 0;
};
#endif



