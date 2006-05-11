// server.cpp: little more than enhanced multicaster
// runs dedicated or as client coroutine

#include "cube.h" 
#using <mscorlib.dll>
#using <MezzanineLib.dll>

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
	case MezzanineLib::ClientServer::ServerType::ST_TCPIP:
        {
            enet_peer_send(clients[n].peer, 0, packet);
            MezzanineLib::ClientServer::Server::bsend += packet->dataLength;
            break;
        };

        case MezzanineLib::ClientServer::ServerType::ST_LOCAL:
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
    putint(p, MezzanineLib::NetworkMessages::SV_SERVMSG);
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
	clients[n].type = MezzanineLib::ClientServer::ServerType::ST_EMPTY;
    send2(true, -1, MezzanineLib::NetworkMessages::SV_CDIS, n);
};

void resetitems() { sents.setsize(0); MezzanineLib::ClientServer::Server::notgotitems = true; };

void pickup(uint i, int sec, int sender)         // server side item pickup, acknowledge first client that gets it
{
    if(i>=(uint)sents.length()) return;
    if(sents[i].spawned)
    {
        sents[i].spawned = false;
        sents[i].spawnsecs = sec;
        send2(true, sender, MezzanineLib::NetworkMessages::SV_ITEMACC, i);
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
    loopv(clients) if(clients[i].type!=MezzanineLib::ClientServer::ServerType::ST_EMPTY)
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
    char text[MezzanineLib::GameInit::MAXTRANS];
    int cn = -1, type;

    while(p<end) switch(type = getint(p))
    {
        case MezzanineLib::NetworkMessages::SV_TEXT:
            sgetstr();
            break;

        case MezzanineLib::NetworkMessages::SV_INITC2S:
            sgetstr();
            strcpy_s(clients[cn].name, text);
            sgetstr();
            getint(p);
            break;

        case MezzanineLib::NetworkMessages::SV_MAPCHANGE:
        {
            sgetstr();
            int reqmode = getint(p);
            if(reqmode<0) reqmode = 0;
            if(smapname[0] && !MezzanineLib::ClientServer::Server::mapreload && !vote(text, reqmode, sender)) return;
            MezzanineLib::ClientServer::Server::mapreload = false;
            MezzanineLib::ClientServer::Server::mode = reqmode;
            MezzanineLib::ClientServer::Server::minremain = MezzanineLib::ClientServer::Server::mode&1 ? 15 : 10;
            MezzanineLib::ClientServer::Server::mapend = MezzanineLib::ClientServer::Server::lastsec+MezzanineLib::ClientServer::Server::minremain*60;
            MezzanineLib::ClientServer::Server::interm = 0;
            strcpy_s(smapname, text);
            resetitems();
            sender = -1;
            break;
        };
        
        case MezzanineLib::NetworkMessages::SV_ITEMLIST:
        {
            int n;
            while((n = getint(p))!=-1) if(MezzanineLib::ClientServer::Server::notgotitems)
            {
                server_entity se = { false, 0 };
                while(sents.length()<=n) sents.add(se);
                sents[n].spawned = true;
            };
            MezzanineLib::ClientServer::Server::notgotitems = false;
            break;
        };

        case MezzanineLib::NetworkMessages::SV_ITEMPICKUP:
        {
            int n = getint(p);
            pickup(n, getint(p), sender);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_PING:
            send2(false, cn, MezzanineLib::NetworkMessages::SV_PONG, getint(p));
            break;

        case MezzanineLib::NetworkMessages::SV_POS:
        {
            cn = getint(p);
            if(cn<0 || cn>=clients.length() || clients[cn].type==MezzanineLib::ClientServer::ServerType::ST_EMPTY)
            {
                disconnect_client(sender, "client num");
                return;
            };
            int size = msgsizelookup(type);
            assert(size!=-1);
            loopi(size-2) getint(p);
            break;
        };

        case MezzanineLib::NetworkMessages::SV_SENDMAP:
        {
            sgetstr();
            int mapsize = getint(p);
            sendmaps(sender, text, mapsize, p);
            return;
        }

        case MezzanineLib::NetworkMessages::SV_RECVMAP:
			send(sender, recvmap(sender));
            return;
            
        case MezzanineLib::NetworkMessages::SV_EXT:   // allows for new features that require no server updates 
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
    ENetPacket * packet = enet_packet_create (NULL, MezzanineLib::GameInit::MAXTRANS, ENET_PACKET_FLAG_RELIABLE);
    uchar *start = packet->data;
    uchar *p = start+2;
    putint(p, MezzanineLib::NetworkMessages::SV_INITS2C);
    putint(p, n);
    putint(p, MezzanineLib::GameInit::PROTOCOL_VERSION);
    putint(p, smapname[0]);
    sendstring(serverpassword, p);
    putint(p, clients.length()>MezzanineLib::ClientServer::Server::maxclients);
    if(smapname[0])
    {
        putint(p, MezzanineLib::NetworkMessages::SV_MAPCHANGE);
        sendstring(smapname, p);
        putint(p, MezzanineLib::ClientServer::Server::mode);
        putint(p, MezzanineLib::NetworkMessages::SV_ITEMLIST);
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
    loopv(clients) if(clients[i].type==MezzanineLib::ClientServer::ServerType::ST_EMPTY) return clients[i];
    return clients.add();
};

void checkintermission()
{
    if(!MezzanineLib::ClientServer::Server::minremain)
    {
        MezzanineLib::ClientServer::Server::interm = MezzanineLib::ClientServer::Server::lastsec+10;
        MezzanineLib::ClientServer::Server::mapend = MezzanineLib::ClientServer::Server::lastsec+1000;
    };
    send2(true, -1, MezzanineLib::NetworkMessages::SV_TIMEUP, MezzanineLib::ClientServer::Server::minremain--);
};

void startintermission() { MezzanineLib::ClientServer::Server::minremain = 0; checkintermission(); };

void resetserverifempty()
{
    loopv(clients) if(clients[i].type!=MezzanineLib::ClientServer::ServerType::ST_EMPTY) return;
    clients.setsize(0);
    smapname[0] = 0;
    resetvotes();
    resetitems();
    MezzanineLib::ClientServer::Server::mode = 0;
    MezzanineLib::ClientServer::Server::mapreload = false;
    MezzanineLib::ClientServer::Server::minremain = 10;
    MezzanineLib::ClientServer::Server::mapend = MezzanineLib::ClientServer::Server::lastsec+MezzanineLib::ClientServer::Server::minremain*60;
    MezzanineLib::ClientServer::Server::interm = 0;
};

void serverslice(int seconds, unsigned int timeout)   // main server update, called from cube main loop in sp, or dedicated server loop
{
    loopv(sents)        // spawn entities when timer reached
    {
        if(sents[i].spawnsecs && (sents[i].spawnsecs -= seconds-MezzanineLib::ClientServer::Server::lastsec)<=0)
        {
            sents[i].spawnsecs = 0;
            sents[i].spawned = true;
            send2(true, -1, MezzanineLib::NetworkMessages::SV_ITEMSPAWN, i);
        };
    };
    
    MezzanineLib::ClientServer::Server::lastsec = seconds;
    
    if((MezzanineLib::ClientServer::Server::mode>1 || (MezzanineLib::ClientServer::Server::mode==0 && MezzanineLib::ClientServer::Server::nonlocalclients)) && seconds>MezzanineLib::ClientServer::Server::mapend-MezzanineLib::ClientServer::Server::minremain*60) checkintermission();
    if(MezzanineLib::ClientServer::Server::interm && seconds>MezzanineLib::ClientServer::Server::interm)
    {
        MezzanineLib::ClientServer::Server::interm = 0;
        loopv(clients) if(clients[i].type!=MezzanineLib::ClientServer::ServerType::ST_EMPTY)
        {
            send2(true, i, MezzanineLib::NetworkMessages::SV_MAPRELOAD, 0);    // ask a client to trigger map reload
            MezzanineLib::ClientServer::Server::mapreload = true;
            break;
        };
    };

    resetserverifempty();
    
    if(!MezzanineLib::ClientServer::Server::isdedicated) return;     // below is network only

	int numplayers = 0;
	loopv(clients) if(clients[i].type!=MezzanineLib::ClientServer::ServerType::ST_EMPTY) ++numplayers;
	serverms(MezzanineLib::ClientServer::Server::mode, numplayers, MezzanineLib::ClientServer::Server::minremain, smapname, seconds, clients.length()>=MezzanineLib::ClientServer::Server::maxclients);

    if(seconds-MezzanineLib::ClientServer::Server::laststatus>60)   // display bandwidth stats, useful for server ops
    {
        MezzanineLib::ClientServer::Server::nonlocalclients = 0;
        loopv(clients) if(clients[i].type==MezzanineLib::ClientServer::ServerType::ST_TCPIP) MezzanineLib::ClientServer::Server::nonlocalclients++;
        MezzanineLib::ClientServer::Server::laststatus = seconds;     
        if(MezzanineLib::ClientServer::Server::nonlocalclients || MezzanineLib::ClientServer::Server::bsend || MezzanineLib::ClientServer::Server::brec) printf("status: %d remote clients, %.1f send, %.1f rec (K/sec)\n", MezzanineLib::ClientServer::Server::nonlocalclients, MezzanineLib::ClientServer::Server::bsend/60.0f/1024, MezzanineLib::ClientServer::Server::brec/60.0f/1024);
        MezzanineLib::ClientServer::Server::bsend = MezzanineLib::ClientServer::Server::brec = 0;
    };

    ENetEvent event;
    if(enet_host_service(serverhost, &event, timeout) > 0)
    {
        switch(event.type)
        {
            case ENET_EVENT_TYPE_CONNECT:
            {
                client &c = addclient();
                c.type = MezzanineLib::ClientServer::ServerType::ST_TCPIP;
                c.peer = event.peer;
                c.peer->data = (void *)(&c-&clients[0]);
                char hn[1024];
                strcpy_s(c.hostname, (enet_address_get_host(&c.peer->address, hn, sizeof(hn))==0) ? hn : "localhost");
                printf("client connected (%s)\n", c.hostname);
                send_welcome(MezzanineLib::ClientServer::Server::lastconnect = &c-&clients[0]);
                break;
            }
            case ENET_EVENT_TYPE_RECEIVE:
                MezzanineLib::ClientServer::Server::brec += event.packet->dataLength;
                process(event.packet, (int)event.peer->data); 
                if(event.packet->referenceCount==0) enet_packet_destroy(event.packet);
                break;

            case ENET_EVENT_TYPE_DISCONNECT: 
                if((int)event.peer->data<0) break;
                printf("disconnected client (%s)\n", clients[(int)event.peer->data].hostname);
                clients[(int)event.peer->data].type = MezzanineLib::ClientServer::ServerType::ST_EMPTY;
                send2(true, -1, MezzanineLib::NetworkMessages::SV_CDIS, (int)event.peer->data);
                event.peer->data = (void *)-1;
                break;
        };
        
        if(numplayers>MezzanineLib::ClientServer::Server::maxclients)   
        {
            disconnect_client(MezzanineLib::ClientServer::Server::lastconnect, "maxclients reached");
        };
    };
};

void cleanupserver()
{
    if(serverhost) enet_host_destroy(serverhost);
};

void localdisconnect()
{
    loopv(clients) if(clients[i].type==MezzanineLib::ClientServer::ServerType::ST_LOCAL) clients[i].type = MezzanineLib::ClientServer::ServerType::ST_EMPTY;
};

void localconnect()
{
    client &c = addclient();
    c.type = MezzanineLib::ClientServer::ServerType::ST_LOCAL;
    strcpy_s(c.hostname, "local");
    send_welcome(&c-&clients[0]); 
};

void initserver(bool dedicated, int uprate, char *sdesc, char *ip, char *master, char *passwd, int maxcl)
{
    serverpassword = passwd;
    MezzanineLib::ClientServer::Server::maxclients = maxcl;
	servermsinit(master ? master : "wouter.fov120.com/cube/masterserver/", sdesc, dedicated);
    
    if(MezzanineLib::ClientServer::Server::isdedicated = dedicated)
    {
        ENetAddress address = { ENET_HOST_ANY, MezzanineLib::GameInit::CUBE_SERVER_PORT };
        if(*ip && enet_address_set_host(&address, ip)<0) printf("WARNING: server ip not resolved");
        serverhost = enet_host_create(&address, MezzanineLib::GameInit::MAXCLIENTS, 0, uprate);
		if(!serverhost) MezzanineLib::GameInit::Fatal("could not create server host\n");
        loopi(MezzanineLib::GameInit::MAXCLIENTS) serverhost->peers[i].data = (void *)-1;
    };

    resetserverifempty();

    if(MezzanineLib::ClientServer::Server::isdedicated)       // do not return, this becomes main loop
    {
        SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);
        printf("dedicated server started, waiting for clients...\nCtrl-C to exit\n\n");
        atexit(cleanupserver);
        atexit(enet_deinitialize);
        for(;;) serverslice(/*enet_time_get_sec()*/time(NULL), 5);
    };
};
