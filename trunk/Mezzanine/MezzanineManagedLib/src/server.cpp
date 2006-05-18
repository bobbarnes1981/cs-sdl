// server.cpp: little more than enhanced multicaster
// runs dedicated or as client coroutine

#include "cube.h" 
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::ClientServer;


struct client                   // server side version of "dynent" type
{
    int type;
    ENetPeer *peer;
    string hostname;
    string mapvote;
    string name;
    int modevote;
};

vector<client> clients;

string smapname;

struct server_entity            // server side version of "entity" type
{
    bool spawned;
    int spawnsecs;
};

vector<server_entity> sents;

void restoreserverstate(vector<entity> &ents)   // hack: called from savegame code, only works in SP
{
    loopv(sents)
    {
        sents[i].spawned = ents[i].spawned;
        sents[i].spawnsecs = 0;
    }; 
};

char *serverpassword = "";
ENetHost * serverhost = NULL;

void process(ENetPacket *packet, int sender);
void multicast(ENetPacket *packet, int sender);
void disconnect_client(int n, char *reason);

void send(int n, ENetPacket *packet)
{
	if(!packet) return;
    switch(clients[n].type)
    {
	case ServerType::ST_TCPIP:
        {
            enet_peer_send(clients[n].peer, 0, packet);
            Server::bsend += packet->dataLength;
            break;
        };

        case ServerType::ST_LOCAL:
            localservertoclient(packet->data, packet->dataLength);
            break;

    };
};

void send2(bool rel, int cn, int a, int b)
{
    ENetPacket *packet = enet_packet_create(NULL, 32, rel ? ENET_PACKET_FLAG_RELIABLE : 0);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, a);
    putint(p, b);
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    if(cn<0) process(packet, -1);
    else send(cn, packet);
    if(packet->referenceCount==0) enet_packet_destroy(packet);
};

void sendservmsg(char *msg)
{
    ENetPacket *packet = enet_packet_create(NULL, _MAXDEFSTR+10, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, NetworkMessages::SV_SERVMSG);
    sendstring(msg, p);
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    multicast(packet, -1);
    if(packet->referenceCount==0) enet_packet_destroy(packet);
};

void disconnect_client(int n, char *reason)
{
    printf("disconnecting client (%s) [%s]\n", clients[n].hostname, reason);
    enet_peer_disconnect(clients[n].peer);
	clients[n].type = ServerType::ST_EMPTY;
    send2(true, -1, NetworkMessages::SV_CDIS, n);
};

void resetitems() { sents.setsize(0); Server::notgotitems = true; };

void pickup(uint i, int sec, int sender)         // server side item pickup, acknowledge first client that gets it
{
    if(i>=(uint)sents.length()) return;
    if(sents[i].spawned)
    {
        sents[i].spawned = false;
        sents[i].spawnsecs = sec;
        send2(true, sender, NetworkMessages::SV_ITEMACC, i);
    };
};

void resetvotes()
{
    loopv(clients) clients[i].mapvote[0] = 0;
};

bool vote(char *map, int reqmode, int sender)
{
    strcpy_s(clients[sender].mapvote, map);
    clients[sender].modevote = reqmode;
    int yes = 0, no = 0; 
    loopv(clients) if(clients[i].type!=ServerType::ST_EMPTY)
    {
        if(clients[i].mapvote[0]) { if(strcmp(clients[i].mapvote, map)==0 && clients[i].modevote==reqmode) yes++; else no++; }
        else no++;
    };
    if(yes==1 && no==0) return true;  // single player
    sprintf_sd(msg)("%s suggests %s on map %s (set map to vote)", clients[sender].name, modestr(reqmode), map);
    sendservmsg(msg);
    if(yes/(float)(yes+no) <= 0.5f) return false;
    sendservmsg("vote passed");
    resetvotes();
    return true;    
};

// server side processing of updates: does very little and most state is tracked client only
// could be extended to move more gameplay to server (at expense of lag)

void process(ENetPacket * packet, int sender)   // sender may be -1
{
    if(ENET_NET_TO_HOST_16(*(ushort *)packet->data)!=packet->dataLength)
    {
        disconnect_client(sender, "packet length");
        return;
    };
        
    uchar *end = packet->data+packet->dataLength;
    uchar *p = packet->data+2;
    char text[GameInit::MAXTRANS];
    int cn = -1, type;

    while(p<end) switch(type = getint(p))
    {
        case NetworkMessages::SV_TEXT:
            sgetstr();
            break;

        case NetworkMessages::SV_INITC2S:
            sgetstr();
            strcpy_s(clients[cn].name, text);
            sgetstr();
            getint(p);
            break;

        case NetworkMessages::SV_MAPCHANGE:
        {
            sgetstr();
            int reqmode = getint(p);
            if(reqmode<0) reqmode = 0;
            if(smapname[0] && !Server::mapreload && !vote(text, reqmode, sender)) return;
            Server::mapreload = false;
            Server::mode = reqmode;
            Server::minremain = Server::mode&1 ? 15 : 10;
            Server::mapend = Server::lastsec+Server::minremain*60;
            Server::interm = 0;
            strcpy_s(smapname, text);
            resetitems();
            sender = -1;
            break;
        };
        
        case NetworkMessages::SV_ITEMLIST:
        {
            int n;
            while((n = getint(p))!=-1) if(Server::notgotitems)
            {
                server_entity se = { false, 0 };
                while(sents.length()<=n) sents.add(se);
                sents[n].spawned = true;
            };
            Server::notgotitems = false;
            break;
        };

        case NetworkMessages::SV_ITEMPICKUP:
        {
            int n = getint(p);
            pickup(n, getint(p), sender);
            break;
        };

        case NetworkMessages::SV_PING:
            send2(false, cn, NetworkMessages::SV_PONG, getint(p));
            break;

        case NetworkMessages::SV_POS:
        {
            cn = getint(p);
            if(cn<0 || cn>=clients.length() || clients[cn].type==ServerType::ST_EMPTY)
            {
                disconnect_client(sender, "client num");
                return;
            };
            int size = msgsizelookup(type);
            assert(size!=-1);
            loopi(size-2) getint(p);
            break;
        };

        case NetworkMessages::SV_SENDMAP:
        {
            sgetstr();
            int mapsize = getint(p);
            sendmaps(sender, text, mapsize, p);
            return;
        }

        case NetworkMessages::SV_RECVMAP:
			send(sender, recvmap(sender));
            return;
            
        case NetworkMessages::SV_EXT:   // allows for new features that require no server updates 
        {
            for(int n = getint(p); n; n--) getint(p);
            break;
        };

        default:
        {
            int size = msgsizelookup(type);
            if(size==-1) { disconnect_client(sender, "tag type"); return; };
            loopi(size-1) getint(p);
        };
    };

    if(p>end) { disconnect_client(sender, "end of packet"); return; };
    multicast(packet, sender);
};

void send_welcome(int n)
{
    ENetPacket * packet = enet_packet_create (NULL, GameInit::MAXTRANS, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, NetworkMessages::SV_INITS2C);
    putint(p, n);
    putint(p, GameInit::PROTOCOL_VERSION);
    putint(p, smapname[0]);
    sendstring(serverpassword, p);
    putint(p, clients.length()>Server::maxclients);
    if(smapname[0])
    {
        putint(p, NetworkMessages::SV_MAPCHANGE);
        sendstring(smapname, p);
        putint(p, Server::mode);
        putint(p, NetworkMessages::SV_ITEMLIST);
        loopv(sents) if(sents[i].spawned) putint(p, i);
        putint(p, -1);
    };
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    send(n, packet);
};

void multicast(ENetPacket *packet, int sender)
{
    loopv(clients)
    {
        if(i==sender) continue;
        send(i, packet);
    };
};

void localclienttoserver(ENetPacket *packet)
{
    process(packet, 0);
    if(!packet->referenceCount) enet_packet_destroy (packet);
};

client &addclient()
{
    loopv(clients) if(clients[i].type==ServerType::ST_EMPTY) return clients[i];
    return clients.add();
};

void checkintermission()
{
    if(!Server::minremain)
    {
        Server::interm = Server::lastsec+10;
        Server::mapend = Server::lastsec+1000;
    };
    send2(true, -1, NetworkMessages::SV_TIMEUP, Server::minremain--);
};

void startintermission() { Server::minremain = 0; checkintermission(); };

void resetserverifempty()
{
    loopv(clients) if(clients[i].type!=ServerType::ST_EMPTY) return;
    clients.setsize(0);
    smapname[0] = 0;
    resetvotes();
    resetitems();
    Server::mode = 0;
    Server::mapreload = false;
    Server::minremain = 10;
    Server::mapend = Server::lastsec+Server::minremain*60;
    Server::interm = 0;
};

void serverslice(int seconds, unsigned int timeout)   // main server update, called from cube main loop in sp, or dedicated server loop
{
    loopv(sents)        // spawn entities when timer reached
    {
        if(sents[i].spawnsecs && (sents[i].spawnsecs -= seconds-Server::lastsec)<=0)
        {
            sents[i].spawnsecs = 0;
            sents[i].spawned = true;
            send2(true, -1, NetworkMessages::SV_ITEMSPAWN, i);
        };
    };
    
    Server::lastsec = seconds;
    
    if((Server::mode>1 || (Server::mode==0 && Server::nonlocalclients)) && seconds>Server::mapend-Server::minremain*60) checkintermission();
    if(Server::interm && seconds>Server::interm)
    {
        Server::interm = 0;
        loopv(clients) if(clients[i].type!=ServerType::ST_EMPTY)
        {
            send2(true, i, NetworkMessages::SV_MAPRELOAD, 0);    // ask a client to trigger map reload
            Server::mapreload = true;
            break;
        };
    };

    resetserverifempty();
    
    if(!Server::isdedicated) return;     // below is network only

	int numplayers = 0;
	loopv(clients) if(clients[i].type!=ServerType::ST_EMPTY) ++numplayers;
	serverms(Server::mode, numplayers, Server::minremain, smapname, seconds, clients.length()>=Server::maxclients);

    if(seconds-Server::laststatus>60)   // display bandwidth stats, useful for server ops
    {
        Server::nonlocalclients = 0;
        loopv(clients) if(clients[i].type==ServerType::ST_TCPIP) Server::nonlocalclients++;
        Server::laststatus = seconds;     
        if(Server::nonlocalclients || Server::bsend || Server::brec) printf("status: %d remote clients, %.1f send, %.1f rec (K/sec)\n", Server::nonlocalclients, Server::bsend/60.0f/1024, Server::brec/60.0f/1024);
        Server::bsend = Server::brec = 0;
    };

    ENetEvent event;
    if(enet_host_service(serverhost, &event, timeout) > 0)
    {
        switch(event.type)
        {
            case ENET_EVENT_TYPE_CONNECT:
            {
                client &c = addclient();
                c.type = ServerType::ST_TCPIP;
                c.peer = event.peer;
                c.peer->data = (void *)(&c-&clients[0]);
                char hn[1024];
                strcpy_s(c.hostname, (enet_address_get_host(&c.peer->address, hn, sizeof(hn))==0) ? hn : "localhost");
                printf("client connected (%s)\n", c.hostname);
                send_welcome(Server::lastconnect = &c-&clients[0]);
                break;
            }
            case ENET_EVENT_TYPE_RECEIVE:
                Server::brec += event.packet->dataLength;
                process(event.packet, (int)event.peer->data); 
                if(event.packet->referenceCount==0) enet_packet_destroy(event.packet);
                break;

            case ENET_EVENT_TYPE_DISCONNECT: 
                if((int)event.peer->data<0) break;
                printf("disconnected client (%s)\n", clients[(int)event.peer->data].hostname);
                clients[(int)event.peer->data].type = ServerType::ST_EMPTY;
                send2(true, -1, NetworkMessages::SV_CDIS, (int)event.peer->data);
                event.peer->data = (void *)-1;
                break;
        };
        
        if(numplayers>Server::maxclients)   
        {
            disconnect_client(Server::lastconnect, "maxclients reached");
        };
    };
};

void cleanupserver()
{
    if(serverhost) enet_host_destroy(serverhost);
};

void localdisconnect()
{
    loopv(clients) if(clients[i].type==ServerType::ST_LOCAL) clients[i].type = ServerType::ST_EMPTY;
};

void localconnect()
{
    client &c = addclient();
    c.type = ServerType::ST_LOCAL;
    strcpy_s(c.hostname, "local");
    send_welcome(&c-&clients[0]); 
};

void initserver(bool dedicated, int uprate, char *sdesc, char *ip, char *master, char *passwd, int maxcl)
{
    serverpassword = passwd;
    Server::maxclients = maxcl;
	servermsinit(master ? master : "wouter.fov120.com/cube/masterserver/", sdesc, dedicated);
    
    if(Server::isdedicated = dedicated)
    {
        ENetAddress address = { ENET_HOST_ANY, GameInit::CUBE_SERVER_PORT };
        if(*ip && enet_address_set_host(&address, ip)<0) printf("WARNING: server ip not resolved");
        serverhost = enet_host_create(&address, GameInit::MAXCLIENTS, 0, uprate);
		if(!serverhost) GameInit::Fatal("could not create server host\n");
        loopi(GameInit::MAXCLIENTS) serverhost->peers[i].data = (void *)-1;
    };

    resetserverifempty();

    if(Server::isdedicated)       // do not return, this becomes main loop
    {
        SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);
        printf("dedicated server started, waiting for clients...\nCtrl-C to exit\n\n");
        atexit(cleanupserver);
        atexit(enet_deinitialize);
        for(;;) serverslice(/*enet_time_get_sec()*/time(NULL), 5);
    };
};
