// misc useful functions used by the server

#include "cube.h"
#using <mscorlib.dll>
#using <TessLib.dll>

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
    TessLib::NetworkMessages::SV_INITS2C, 4, TessLib::NetworkMessages::SV_INITC2S, 0, TessLib::NetworkMessages::SV_POS, 12, TessLib::NetworkMessages::SV_TEXT, 0, TessLib::NetworkMessages::SV_SOUND, 2, TessLib::NetworkMessages::SV_CDIS, 2,
    TessLib::NetworkMessages::SV_EDITH, 7, TessLib::NetworkMessages::SV_EDITT, 7, TessLib::NetworkMessages::SV_EDITS, 6, TessLib::NetworkMessages::SV_EDITD, 6, TessLib::NetworkMessages::SV_EDITE, 6,
    TessLib::NetworkMessages::SV_DIED, 2, TessLib::NetworkMessages::SV_DAMAGE, 4, TessLib::NetworkMessages::SV_SHOT, 8, TessLib::NetworkMessages::SV_FRAGS, 2,
    TessLib::NetworkMessages::SV_MAPCHANGE, 0, TessLib::NetworkMessages::SV_ITEMSPAWN, 2, TessLib::NetworkMessages::SV_ITEMPICKUP, 3, TessLib::NetworkMessages::SV_DENIED, 2,
    TessLib::NetworkMessages::SV_PING, 2, TessLib::NetworkMessages::SV_PONG, 2, TessLib::NetworkMessages::SV_CLIENTPING, 2, TessLib::NetworkMessages::SV_GAMEMODE, 2,
    TessLib::NetworkMessages::SV_TIMEUP, 2, TessLib::NetworkMessages::SV_EDITENT, 10, TessLib::NetworkMessages::SV_MAPRELOAD, 2, TessLib::NetworkMessages::SV_ITEMACC, 2,
    TessLib::NetworkMessages::SV_SENDMAP, 0, TessLib::NetworkMessages::SV_RECVMAP, 1, TessLib::NetworkMessages::SV_SERVMSG, 0, TessLib::NetworkMessages::SV_ITEMLIST, 0,
    TessLib::NetworkMessages::SV_EXT, 0,
    -1
};

char msgsizelookup(int msg)
{
    for(char *p = msgsizesl; *p>=0; p += 2) if(*p==msg) return p[1];
    return -1;
};

// sending of maps between clients

string copyname;
int copysize;
uchar *copydata = NULL;

void sendmaps(int n, string mapname, int mapsize, uchar *mapdata)
{
    if(mapsize <= 0 || mapsize > 256*256) return;
    strcpy_s(copyname, mapname);
    copysize = mapsize;
    if(copydata) free(copydata);
    copydata = (uchar *)alloc(mapsize);
    memcpy(copydata, mapdata, mapsize);
}

ENetPacket *recvmap(int n)
{
    if(!copydata) return NULL;
    ENetPacket *packet = enet_packet_create(NULL, MAXTRANS + copysize, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, TessLib::NetworkMessages::SV_RECVMAP);
    sendstring(copyname, p);
    putint(p, copysize);
    memcpy(p, copydata, copysize);
    p += copysize;
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
	return packet;
}


#ifdef STANDALONE

void localservertoclient(uchar *buf, int len) {};
//void fatal(char *s, char *o) { cleanupserver(); printf("servererror: %s\n", s); exit(1); };
void *alloc(int s) { void *b = calloc(1,s); if(!b) TessLib::GameInit::Fatal("no memory!"); return b; };

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
    
    if(enet_initialize()<0) TessLib::GameInit::Fatal("Unable to initialise network module");
    initserver(true, uprate, sdesc, ip, master, passwd, maxcl);
    return 0;
};
#endif



