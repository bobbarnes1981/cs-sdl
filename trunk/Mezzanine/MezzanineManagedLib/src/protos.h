// protos for ALL external functions in cube... 
#ifndef DECLSPEC 
define DECLSPEC	__declspec(dllexport)
#endif
#ifndef CDECL
#define CDECL __cdecl
#endif

#ifdef __cplusplus
extern "C" {
#endif

// command
extern DECLSPEC int CDECL variable(char *name, int min, int cur, int max, int *storage, void (*fun)(), bool persist);
extern DECLSPEC void CDECL setvar(char *name, int i);
extern DECLSPEC int CDECL getvar(char *name);
extern DECLSPEC bool CDECL identexists(char *name);
extern DECLSPEC bool CDECL addcommand(char *name, void (*fun)(), int narg);
extern DECLSPEC int CDECL execute(char *p, bool down = true);
extern DECLSPEC void CDECL exec(char *cfgfile);
extern DECLSPEC bool CDECL execfile(char *cfgfile);
extern DECLSPEC void CDECL resetcomplete();
extern DECLSPEC void CDECL complete(char *s);
extern DECLSPEC void CDECL alias(char *name, char *action);
extern DECLSPEC char CDECL *getalias(char *name);
extern DECLSPEC void CDECL writecfg();

// console
extern DECLSPEC void CDECL keypress(int code, bool isdown, int cooked);
extern DECLSPEC void CDECL renderconsole();
extern DECLSPEC void CDECL conoutf(const char *s, ...);
extern DECLSPEC char CDECL *getcurcommand();
extern DECLSPEC void CDECL writebinds(FILE *f);

// menus
extern DECLSPEC bool CDECL rendermenu();
extern DECLSPEC void CDECL menuset(int menu);
extern DECLSPEC void CDECL menumanual(int m, int n, char *text);
extern DECLSPEC void CDECL sortmenu(int start, int num);
extern DECLSPEC bool CDECL menukey(int code, bool isdown);
extern DECLSPEC void CDECL newmenu(char *name);

// serverbrowser
extern DECLSPEC void CDECL addserver(char *servername);
extern DECLSPEC char CDECL *getservername(int n);
extern DECLSPEC void CDECL writeservercfg();

// rendergl
extern DECLSPEC void CDECL purgetextures();
extern DECLSPEC void CDECL gl_drawframe(int w, int h, float curfps);
extern DECLSPEC void CDECL mipstats(int a, int b, int c);
extern DECLSPEC void CDECL vertf(float v1, float v2, float v3, sqr *ls, float t1, float t2);
extern DECLSPEC void CDECL addstrip(int tex, int start, int n);
extern DECLSPEC int CDECL lookuptexture(int tex, int &xs, int &ys);

// rendercubes
extern DECLSPEC void CDECL resetcubes();
extern DECLSPEC void CDECL render_flat(int tex, int x, int y, int size, int h, sqr *l1, sqr *l2, sqr *l3, sqr *l4, bool isceil);
extern DECLSPEC void CDECL render_flatdelta(int wtex, int x, int y, int size, float h1, float h2, float h3, float h4, sqr *l1, sqr *l2, sqr *l3, sqr *l4, bool isceil);
extern DECLSPEC void CDECL render_square(int wtex, float floor1, float floor2, float ceil1, float ceil2, int x1, int y1, int x2, int y2, int size, sqr *l1, sqr *l2, bool topleft);
extern DECLSPEC void CDECL render_tris(int x, int y, int size, bool topleft, sqr *h1, sqr *h2, sqr *s, sqr *t, sqr *u, sqr *v);
extern DECLSPEC void CDECL addwaterquad(int x, int y, int size);
extern DECLSPEC int CDECL renderwater(float hf);
extern DECLSPEC void CDECL finishstrips();
extern DECLSPEC void CDECL setarraypointers();

// client
extern DECLSPEC void CDECL localservertoclient(uchar *buf, int len);
extern DECLSPEC void CDECL connects(char *servername);
extern DECLSPEC void CDECL disconnect(int onlyclean = 0, int async = 0);
extern DECLSPEC void CDECL toserver(char *text);
extern DECLSPEC void CDECL addmsg(int rel, int num, int type, ...);
extern DECLSPEC bool CDECL multiplayer();
extern DECLSPEC bool CDECL allowedittoggle();
extern DECLSPEC void CDECL sendpackettoserv(void *packet);
extern DECLSPEC void CDECL gets2c();
extern DECLSPEC void CDECL c2sinfo(dynent *d);
extern DECLSPEC void CDECL neterr(char *s);
extern DECLSPEC void CDECL initclientnet();
extern DECLSPEC bool CDECL netmapstart();
extern DECLSPEC int CDECL getclientnum();
extern DECLSPEC void CDECL changemapserv(char *name, int mode);
extern DECLSPEC void CDECL writeclientinfo(FILE *f);

// clientgame
extern DECLSPEC void CDECL mousemove(int dx, int dy); 
extern DECLSPEC void CDECL updateworld(int millis);
extern DECLSPEC void CDECL startmap(char *name);
extern DECLSPEC void CDECL changemap(char *name);
//extern DECLSPEC void CDECL initclient();
extern DECLSPEC void CDECL spawnplayer(dynent *d);
extern DECLSPEC void CDECL selfdamage(int damage, int actor, dynent *act);
extern DECLSPEC dynent CDECL *newdynent();
extern DECLSPEC char CDECL *getclientmap();
extern DECLSPEC const char CDECL *modestr(int n);
extern DECLSPEC void CDECL zapdynent(dynent *&d);
extern DECLSPEC dynent CDECL *getclient(int cn);
extern DECLSPEC void CDECL timeupdate(int timeremain);
extern DECLSPEC void CDECL resetmovement(dynent *d);
extern DECLSPEC void CDECL fixplayer1range();

// clientextras
extern DECLSPEC void CDECL renderclients();
extern DECLSPEC void CDECL renderclient(dynent *d, bool team, char *mdlname, bool hellpig, float scale);
extern DECLSPEC void CDECL showscores(bool on);
extern DECLSPEC void CDECL renderscores();

// world
extern DECLSPEC void CDECL setupworld(int factor);
extern DECLSPEC void CDECL empty_world(int factor, bool force);
extern DECLSPEC void CDECL remip(block &b, int level = 0);
extern DECLSPEC void CDECL remipmore(block &b, int level = 0);
extern DECLSPEC int CDECL closestent();
extern DECLSPEC int CDECL findentity(int type, int index = 0);
extern DECLSPEC void CDECL trigger(int tag, int type, bool savegame);
extern DECLSPEC void CDECL resettagareas();
extern DECLSPEC void CDECL settagareas();
extern DECLSPEC entity CDECL *newentity(int x, int y, int z, char *what, int v1, int v2, int v3, int v4);

// worldlight
extern DECLSPEC void CDECL calclight();
extern DECLSPEC void CDECL dodynlight(vec &vold, vec &v, int reach, int strength, dynent *owner);
extern DECLSPEC void CDECL cleardlights();
extern DECLSPEC block CDECL *blockcopy(block &b);
extern DECLSPEC void CDECL blockpaste(block &b);

// worldrender
extern DECLSPEC void CDECL render_world(float vx, float vy, float vh, int yaw, int pitch, float widef, int w, int h);

// worldocull
extern DECLSPEC void CDECL computeraytable(float vx, float vy);
extern DECLSPEC int CDECL isoccluded(float vx, float vy, float cx, float cy, float csize);

// main

extern DECLSPEC void CDECL *alloc(int s);
extern DECLSPEC void CDECL quit();
extern DECLSPEC dynent * CDECL getplayer1();

// rendertext
extern DECLSPEC void CDECL draw_text(char *str, int left, int top, int gl_num);
extern DECLSPEC void CDECL draw_textf(char *fstr, int left, int top, int gl_num, ...);
extern DECLSPEC int CDECL text_width(char *str);

// editing
extern DECLSPEC void CDECL cursorupdate();
extern DECLSPEC void CDECL toggleedit();
extern DECLSPEC void CDECL editdrag(bool isdown);
extern DECLSPEC void CDECL setvdeltaxy(int delta, block &sel);
extern DECLSPEC void CDECL editequalisexy(bool isfloor, block &sel);
extern DECLSPEC void CDECL edittypexy(int type, block &sel);
extern DECLSPEC void CDECL edittexxy(int type, int t, block &sel);
extern DECLSPEC void CDECL editheightxy(bool isfloor, int amount, block &sel);
extern DECLSPEC bool CDECL noteditmode();
extern DECLSPEC void CDECL pruneundos(int maxremain = 0);

// renderextras
extern DECLSPEC void CDECL box(block &b, float z1, float z2, float z3, float z4);
extern DECLSPEC void CDECL newsphere(vec &o, float max, int type);
extern DECLSPEC void CDECL renderspheres(int time);
extern DECLSPEC void CDECL gl_drawhud(int w, int h, int curfps, int nquads, int curvert, bool underwater);
extern DECLSPEC void CDECL readdepth(int w, int h);
extern DECLSPEC void CDECL damageblend(int n);

// renderparticles
extern DECLSPEC void CDECL setorient(vec &r, vec &u);
extern DECLSPEC void CDECL particle_splash(int type, int num, int fade, vec &p);
extern DECLSPEC void CDECL particle_trail(int type, int fade, vec &from, vec &to);
extern DECLSPEC void CDECL render_particles(int time);

// worldio
extern DECLSPEC void CDECL save_world(char *fname);
extern DECLSPEC void CDECL load_world(char *mname);
extern DECLSPEC void CDECL writemap(char *mname, int msize, uchar *mdata);
extern DECLSPEC uchar CDECL *readmap(char *mname, int *msize);
extern DECLSPEC void CDECL loadgamerest();
extern DECLSPEC void CDECL incomingdemodata(uchar *buf, int len, bool extras = false);
extern DECLSPEC void CDECL demoplaybackstep();
extern DECLSPEC void CDECL stop();
extern DECLSPEC void CDECL stopifrecording();
extern DECLSPEC void CDECL demodamage(int damage, vec &o);
extern DECLSPEC void CDECL demoblend(int damage);

// physics
extern DECLSPEC void CDECL moveplayer(dynent *pl, int moveres, bool local);
extern DECLSPEC bool CDECL collide(dynent *d, bool spawn, float drop, float rise);
extern DECLSPEC void CDECL entinmap(dynent *d);
extern DECLSPEC void CDECL setentphysics(int mml, int mmr);
extern DECLSPEC void CDECL physicsframe();

// sound
extern DECLSPEC void CDECL playsound(int n, vec *loc = 0);
extern DECLSPEC void CDECL playsoundc(int n);
extern DECLSPEC void CDECL updatevol();

// rendermd2
extern DECLSPEC void CDECL rendermodel(char *mdl, int frame, int range, int tex, float rad, float x, float y, float z, float yaw, float pitch, bool teammate, float scale, float speed, int snap = 0, int basetime = 0);
extern DECLSPEC mapmodelinfo CDECL &getmminfo(int i);

// server
extern DECLSPEC void CDECL initserver(bool dedicated, int uprate, char *sdesc, char *ip, char *master, char *passwd, int maxcl);
extern DECLSPEC void CDECL cleanupserver();
extern DECLSPEC void CDECL localconnect();
extern DECLSPEC void CDECL localdisconnect();
extern DECLSPEC void CDECL localclienttoserver(struct _ENetPacket *);
extern DECLSPEC void CDECL serverslice(int seconds, unsigned int timeout);
extern DECLSPEC void CDECL putint(uchar *&p, int n);
extern DECLSPEC int CDECL getint(uchar *&p);
extern DECLSPEC void CDECL sendstring(char *t, uchar *&p);
extern DECLSPEC void CDECL startintermission();
extern DECLSPEC void CDECL restoreserverstate(vector<entity> &ents);
extern DECLSPEC uchar CDECL *retrieveservers(uchar *buf, int buflen);
extern DECLSPEC char msgsizelookup(int msg);
extern DECLSPEC void CDECL serverms(int mode, int numplayers, int minremain, char *smapname, int seconds, bool isfull);
extern DECLSPEC void CDECL servermsinit(const char *master, char *sdesc, bool listen);
extern DECLSPEC void CDECL sendmaps(int n, string mapname, int mapsize, uchar *mapdata);
extern DECLSPEC ENetPacket *recvmap(int n);

// weapon
extern DECLSPEC void CDECL selectgun(int a = -1, int b = -1, int c =-1);
extern DECLSPEC void CDECL shoot(dynent *d, vec &to);
extern DECLSPEC void CDECL shootv(int gun, vec &from, vec &to, dynent *d = 0, bool local = false);
extern DECLSPEC void CDECL createrays(vec &from, vec &to);
extern DECLSPEC void CDECL moveprojectiles(float time);
extern DECLSPEC void CDECL projreset();
extern DECLSPEC char CDECL *playerincrosshair();
extern DECLSPEC int CDECL reloadtime(int gun);

// monster
extern DECLSPEC void CDECL monsterclear();
extern DECLSPEC void CDECL restoremonsterstate();
extern DECLSPEC void CDECL monsterthink();
extern DECLSPEC void CDECL monsterrender();
extern DECLSPEC dvector CDECL &getmonsters();
extern DECLSPEC void CDECL monsterpain(dynent *m, int damage, dynent *d);
extern DECLSPEC void CDECL endsp(bool allkilled);

// entities
extern DECLSPEC void CDECL renderents();
extern DECLSPEC void CDECL putitems(uchar *&p);
extern DECLSPEC void CDECL checkquad(int time);
extern DECLSPEC void CDECL checkitems();
extern DECLSPEC void CDECL realpickup(int n, dynent *d);
extern DECLSPEC void CDECL renderentities();
extern DECLSPEC void CDECL resetspawns();
extern DECLSPEC void CDECL setspawn(uint i, bool on);
extern DECLSPEC void CDECL teleport(int n, dynent *d);
extern DECLSPEC void CDECL baseammo(int gun);

#ifdef __cplusplus
}
#endif