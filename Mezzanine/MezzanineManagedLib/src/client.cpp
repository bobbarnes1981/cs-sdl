// client.cpp, mostly network related client game code

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::ClientServer;

ENetHost *clienthost = NULL;

bool multiplayer()
{
    // check not correct on listen server?
    if(clienthost) conoutf("operation not available in multiplayer");
    return clienthost!=NULL;
};

bool allowedittoggle()
{
    bool allow = !clienthost || gamemode==1;
    if(!allow) conoutf("editing in multiplayer requires coopedit mode (1)");
    return allow; 
};

VARF(rate, 0, 0, 25000, if(clienthost && (!rate || rate>1000)) enet_host_bandwidth_limit (clienthost, rate, rate));

void throttle();

VARF(throttle_interval, 0, 5, 30, throttle());
VARF(throttle_accel,    0, 2, 32, throttle());
VARF(throttle_decel,    0, 2, 32, throttle());

void throttle()
{
    if(!clienthost || Client::connecting) return;
    assert(ENET_PEER_PACKET_THROTTLE_SCALE==32);
    enet_peer_throttle_configure(clienthost->peers, throttle_interval*1000, throttle_accel, throttle_decel);
};

void newname(char *name) { Client::c2sinit = false; strn0cpy(player1->name, name, 16); };
void newteam(char *name) { Client::c2sinit = false; strn0cpy(player1->team, name, 5); };

COMMANDN(team, newteam, MezzanineLib::Support::FunctionSignatures::ARG_1STR);
COMMANDN(name, newname, MezzanineLib::Support::FunctionSignatures::ARG_1STR);

void writeclientinfo(FILE *f)
{
    fprintf(f, "name \"%s\"\nteam \"%s\"\n", player1->name, player1->team);
};

void connects(char *servername)
{   
    disconnect(1);  // reset state
    addserver(servername);

    conoutf("attempting to connect to %s", servername);
    ENetAddress address = { ENET_HOST_ANY, MezzanineLib::GameInit::CUBE_SERVER_PORT };
    if(enet_address_set_host(&address, servername) < 0)
    {
        conoutf("could not resolve server %s", servername);
        return;
    };

    clienthost = enet_host_create(NULL, 1, rate, rate);

    if(clienthost)
    {
        enet_host_connect(clienthost, &address, 1); 
        enet_host_flush(clienthost);
        Client::connecting = MezzanineLib::GameInit::LastMillis;
        Client::connattempts = 0;
    }
    else
    {
        conoutf("could not connect to server");
        disconnect();
    };
};

void disconnect(int onlyclean, int async)
{
    if(clienthost) 
    {
        if(!Client::connecting && !Client::disconnecting) 
        {
            enet_peer_disconnect(clienthost->peers);
            enet_host_flush(clienthost);
            Client::disconnecting = MezzanineLib::GameInit::LastMillis;
        };
        if(clienthost->peers->state != ENET_PEER_STATE_DISCONNECTED)
        {
            if(async) return;
            enet_peer_reset(clienthost->peers);
        };
        enet_host_destroy(clienthost);
    };

    if(clienthost && !Client::connecting) conoutf("disconnected");
    clienthost = NULL;
    Client::connecting = 0;
    Client::connattempts = 0;
    Client::disconnecting = 0;
    Client::ClientNum = -1;
    Client::c2sinit = false;
    player1->lifesequence = 0;
    loopv(players) zapdynent(players[i]);
    
    localdisconnect();

    if(!onlyclean) { stop(); localconnect(); };
};

void trydisconnect()
{
    if(!clienthost)
    {
        conoutf("not connected");
        return;
    };
    if(Client::connecting) 
    {
        conoutf("aborting connection attempt");
        disconnect();
        return;
    };
    conoutf("attempting to disconnect...");
    disconnect(0, !Client::disconnecting);
};

string ctext;
void toserver(char *text) { conoutf("%s:\f %s", player1->name, text); strn0cpy(ctext, text, 80); };
void echo(char *text) { conoutf("%s", text); };

COMMAND(echo, MezzanineLib::Support::FunctionSignatures::ARG_VARI);
COMMANDN(say, toserver, MezzanineLib::Support::FunctionSignatures::ARG_VARI);
COMMANDN(connect, connects, MezzanineLib::Support::FunctionSignatures::ARG_1STR);
COMMANDN(disconnect, trydisconnect, MezzanineLib::Support::FunctionSignatures::ARG_NONE);

// collect c2s messages conveniently

vector<ivector> messages;

void addmsg(int rel, int num, int type, ...)
{
    //if(MezzanineLib::GameInit::DemoPlayback) return; TODO
	//if(num!=msgsizelookup(type)) { sprintf_sd(s)("inconsistant msg size for %d (%d != %d)", type, num, msgsizelookup(type)); MezzanineLib::GameInit::Fatal(s); };
    if(messages.length()==100) { conoutf("command flood protection (type %d)", type); return; };
    ivector &msg = messages.add();
    msg.add(num);
    msg.add(rel);
    msg.add(type);
    va_list marker;
    va_start(marker, type); 
    loopi(num-1) msg.add(va_arg(marker, int));
    va_end(marker);  
};

void server_err()
{
    conoutf("server network error, disconnecting...");
    disconnect();
};

string toservermap;
string clientpassword;
void password(char *p) { strcpy_s(clientpassword, p); };
COMMAND(password, MezzanineLib::Support::FunctionSignatures::ARG_1STR);

bool netmapstart() { Client::SendItemsToServer = true; return clienthost!=NULL; };

void initclientnet()
{
    ctext[0] = 0;
    toservermap[0] = 0;
    clientpassword[0] = 0;
    newname("unnamed");
    newteam("red");
};

void sendpackettoserv(void *packet)
{
    if(clienthost) { enet_host_broadcast(clienthost, 0, (ENetPacket *)packet); enet_host_flush(clienthost); }
    else localclienttoserver((ENetPacket *)packet);
}

void c2sinfo(dynent *d)                     // send update to the server
{
    if(Client::ClientNum<0) return;                 // we haven't had a welcome message from the server yet
    if(MezzanineLib::GameInit::LastMillis-Client::lastupdate<40) return;    // don't update faster than 25fps
    ENetPacket *packet = enet_packet_create (NULL, MezzanineLib::GameInit::MAXTRANS, 0);
    uchar *start = packet->data;
    uchar *p = start+2;
    bool serveriteminitdone = false;
    if(toservermap[0])                      // suggest server to change map
    {                                       // do this exclusively as map change may invalidate rest of update
        packet->flags = ENET_PACKET_FLAG_RELIABLE;
        putint(p, MezzanineLib::NetworkMessages::SV_MAPCHANGE);
        sendstring(toservermap, p);
        toservermap[0] = 0;
        putint(p, nextmode);
    }
    else
    {
        putint(p, MezzanineLib::NetworkMessages::SV_POS);
        putint(p, Client::ClientNum);
        putint(p, (int)(d->o.x*MezzanineLib::GameInit::DMF));       // quantize coordinates to 1/16th of a cube, between 1 and 3 bytes
        putint(p, (int)(d->o.y*MezzanineLib::GameInit::DMF));
        putint(p, (int)(d->o.z*MezzanineLib::GameInit::DMF));
        putint(p, (int)(d->yaw*MezzanineLib::GameInit::DAF));
        putint(p, (int)(d->pitch*MezzanineLib::GameInit::DAF));
        putint(p, (int)(d->roll*MezzanineLib::GameInit::DAF));
        putint(p, (int)(d->vel.x*MezzanineLib::GameInit::DVF));     // quantize to 1/100, almost always 1 byte
        putint(p, (int)(d->vel.y*MezzanineLib::GameInit::DVF));
        putint(p, (int)(d->vel.z*MezzanineLib::GameInit::DVF));
        // pack rest in 1 byte: strafe:2, move:2, onfloor:1, state:3
        putint(p, (d->strafe&3) | ((d->move&3)<<2) | (((int)d->onfloor)<<4) | ((MezzanineLib::GameInit::EditMode ? MezzanineLib::CSStatus::CS_EDITING : d->state)<<5) );
 
        if(Client::SendItemsToServer)
        {
            packet->flags = ENET_PACKET_FLAG_RELIABLE;
            putint(p, MezzanineLib::NetworkMessages::SV_ITEMLIST);
            if(!m_noitems) putitems(p);
            putint(p, -1);
            Client::SendItemsToServer = false;
            serveriteminitdone = true;
        };
        if(ctext[0])    // player chat, not flood protected for now
        {
            packet->flags = ENET_PACKET_FLAG_RELIABLE;
            putint(p, MezzanineLib::NetworkMessages::SV_TEXT);
            sendstring(ctext, p);
            ctext[0] = 0;
        };
        if(!Client::c2sinit)    // tell other clients who I am
        {
            packet->flags = ENET_PACKET_FLAG_RELIABLE;
            Client::c2sinit = true;
            putint(p, MezzanineLib::NetworkMessages::SV_INITC2S);
            sendstring(player1->name, p);
            sendstring(player1->team, p);
            putint(p, player1->lifesequence);
        };
        loopv(messages)     // send messages collected during the previous frames
        {
            ivector &msg = messages[i];
            if(msg[1]) packet->flags = ENET_PACKET_FLAG_RELIABLE;
            loopi(msg[0]) putint(p, msg[i+2]);
        };
        messages.setsize(0);
        if(MezzanineLib::GameInit::LastMillis-Client::lastping>250)
        {
            putint(p, MezzanineLib::NetworkMessages::SV_PING);
            putint(p, MezzanineLib::GameInit::LastMillis);
            Client::lastping = MezzanineLib::GameInit::LastMillis;
        };
    };
    *(ushort *)start = ENET_HOST_TO_NET_16(p-start);
    enet_packet_resize(packet, p-start);
    incomingdemodata(start, p-start, true);
    if(clienthost) { enet_host_broadcast(clienthost, 0, packet); enet_host_flush(clienthost); }
    else localclienttoserver(packet);
    Client::lastupdate = MezzanineLib::GameInit::LastMillis;
    if(serveriteminitdone) loadgamerest();  // hack
};

void gets2c()           // get updates from the server
{
    ENetEvent event;
    if(!clienthost) return;
    if(Client::connecting && MezzanineLib::GameInit::LastMillis/3000 > Client::connecting/3000)
    {
        conoutf("attempting to connect...");
        Client::connecting = MezzanineLib::GameInit::LastMillis;
        ++Client::connattempts; 
        if(Client::connattempts > 3)
        {
            conoutf("could not connect to server");
            disconnect();
            return;
        };
    };
    while(clienthost!=NULL && enet_host_service(clienthost, &event, 0)>0)
    switch(event.type)
    {
        case ENET_EVENT_TYPE_CONNECT:
            conoutf("connected to server");
            Client::connecting = 0;
            throttle();
            break;
         
        case ENET_EVENT_TYPE_RECEIVE:
            if(Client::disconnecting) conoutf("attempting to disconnect...");
            else localservertoclient(event.packet->data, event.packet->dataLength);
            enet_packet_destroy(event.packet);
            break;

        case ENET_EVENT_TYPE_DISCONNECT:
            if(Client::disconnecting) disconnect();
            else server_err();
            return;
    }
};