// monster.cpp: implements AI for single player monsters, currently client only

#include "cube.h"
#using <mscorlib.dll>
using namespace MezzanineLib;
using namespace MezzanineLib::Game;

dvector monsters;

VARF(skill, 1, 3, 10, conoutf("skill is now %d", skill));

dvector &getmonsters() { return monsters; };
void restoremonsterstate() { loopv(monsters) if(monsters[i]->state==CSStatus::CS_DEAD) Monster::numkilled++; };        // for savegames

struct monstertype      // see docs for how these values modify behaviour
{
    short gun, speed, health, freq, lag, rate, pain, loyalty, mscale, bscale;
    short painsound, diesound;
    char *name, *mdlname;
}

monstertypes[Monster::NUMMONSTERTYPES] =
{
    { Gun::GUN_FIREBALL,  15, 100, 3, 0,   100, 800, 1, 10, 10, Sounds::S_PAINO, Sounds::S_DIE1,   "an ogre",     "monster/ogro"    },
    { Gun::GUN_CG,        18,  70, 2, 70,   10, 400, 2,  8,  9, Sounds::S_PAINR, Sounds::S_DEATHR, "a rhino",     "monster/rhino"   },
    { Gun::GUN_SG,        14, 120, 1, 100, 300, 400, 4, 14, 14, Sounds::S_PAINE, Sounds::S_DEATHE, "ratamahatta", "monster/rat"     },
    { Gun::GUN_RIFLE,     15, 200, 1, 80,  300, 300, 4, 18, 18, Sounds::S_PAINS, Sounds::S_DEATHS, "a slith",     "monster/slith"   },
    { Gun::GUN_RL,        13, 500, 1, 0,   100, 200, 6, 24, 24, Sounds::S_PAINB, Sounds::S_DEATHB, "bauul",       "monster/bauul"   },
    { Gun::GUN_BITE,      22,  50, 3, 0,   100, 400, 1, 12, 15, Sounds::S_PAINP, Sounds::S_PIGGR2, "a hellpig",   "monster/hellpig" },
    { Gun::GUN_ICEBALL,   12, 250, 1, 0,    10, 400, 6, 18, 18, Sounds::S_PAINH, Sounds::S_DEATHH, "a knight",    "monster/knight"  },
    { Gun::GUN_SLIMEBALL, 15, 100, 1, 0,   200, 400, 2, 13, 10, Sounds::S_PAIND, Sounds::S_DEATHD, "a goblin",    "monster/goblin"  },
};

dynent *basicmonster(int type, int yaw, int state, int trigger, int move)
{
    if(type>=Monster::NUMMONSTERTYPES)
    {
        conoutf("warning: unknown monster in spawn: %d", type);
        type = 0;
    };
    dynent *m = newdynent();
    monstertype *t = &monstertypes[m->mtype = type];
    m->eyeheight = 2.0f;
    m->aboveeye = 1.9f;
    m->radius *= t->bscale/10.0f;
    m->eyeheight *= t->bscale/10.0f;
    m->aboveeye *= t->bscale/10.0f;
    m->monsterstate = state;
    if(state!=MonsterStates::M_SLEEP) spawnplayer(m);
    m->trigger = GameInit::LastMillis+trigger;
    m->targetyaw = m->yaw = (float)yaw;
    m->move = move;
    m->enemy = player1;
    m->gunselect = t->gun;
    m->maxspeed = (float)t->speed;
    m->health = t->health;
    m->armour = 0;
    loopi(Gun::NUMGUNS) m->ammo[i] = 10000;
    m->pitch = 0;
    m->roll = 0;
    m->state = CSStatus::CS_ALIVE;
    m->anger = 0;
    strcpy_s(m->name, t->name);
    monsters.add(m);
    return m;
};

void spawnmonster()     // spawn a random monster according to freq distribution in DMSP
{
    int n = rnd(Monster::TOTMFREQ), type;
    for(int i = 0; ; i++) if((n -= monstertypes[i].freq)<0) { type = i; break; };
	basicmonster(type, rnd(360), MonsterStates::M_SEARCH, 1000, 1);
};

void monsterclear()     // called after map start of when toggling edit mode to reset/spawn all monsters to initial state
{
    loopv(monsters) gp()->dealloc(monsters[i], sizeof(dynent)); 
    monsters.setsize(0);
    Monster::numkilled = 0;
    Monster::monstertotal = 0;
    Monster::spawnremain = 0;
    if(m_dmsp)
    {
        Monster::nextmonster = Monster::mtimestart = GameInit::LastMillis+10000;
        Monster::monstertotal = Monster::spawnremain = gamemode<0 ? skill*10 : 0;
    }
    else if(m_classicsp)
    {
        Monster::mtimestart = GameInit::LastMillis;
        loopv(ents) if(ents[i].type==StaticEntity::MONSTER)
        {
            dynent *m = basicmonster(ents[i].attr2, ents[i].attr1, MonsterStates::M_SLEEP, 100, 0);  
            m->o.x = ents[i].x;
            m->o.y = ents[i].y;
            m->o.z = ents[i].z;
            entinmap(m);
            Monster::monstertotal++;
        };
    };
};

bool los(float lx, float ly, float lz, float bx, float by, float bz, vec &v) // height-correct line of sight for monster shooting/seeing
{
    if(OUTBORD((int)lx, (int)ly) || OUTBORD((int)bx, (int)by)) return false;
    float dx = bx-lx;
    float dy = by-ly; 
    int steps = (int)(sqrt(dx*dx+dy*dy)/0.9);
    if(!steps) return false;
    float x = lx;
    float y = ly;
    int i = 0;
    for(;;)
    {
        sqr *s = S(fast_f2nat(x), fast_f2nat(y));
        if(SOLID(s)) break;
        float floor = s->floor;
        if(s->type==BlockTypes::FHF) floor -= s->vdelta/4.0f;
        float ceil = s->ceil;
        if(s->type==BlockTypes::CHF) ceil += s->vdelta/4.0f;
        float rz = lz-((lz-bz)*(i/(float)steps));
        if(rz<floor || rz>ceil) break;
        v.x = x;
        v.y = y;
        v.z = rz;
        x += dx/(float)steps;
        y += dy/(float)steps;
        i++;
    };
    return i>=steps;
};

bool enemylos(dynent *m, vec &v)
{
    v = m->o;
    return los(m->o.x, m->o.y, m->o.z, m->enemy->o.x, m->enemy->o.y, m->enemy->o.z, v);
};

// monster AI is sequenced using transitions: they are in a particular state where
// they execute a particular behaviour until the trigger time is hit, and then they
// reevaluate their situation based on the current state, the environment etc., and
// transition to the next state. Transition timeframes are parametrized by difficulty
// level (skill), faster transitions means quicker decision making means tougher AI.

void transition(dynent *m, int state, int moving, int n, int r) // n = at skill 0, n/2 = at skill 10, r = added random factor
{
    m->monsterstate = state;
    m->move = moving;
    n = n*130/100;
    m->trigger = GameInit::LastMillis+n-skill*(n/16)+rnd(r+1);
};

void normalise(dynent *m, float angle)
{
    while(m->yaw<angle-180.0f) m->yaw += 360.0f;
    while(m->yaw>angle+180.0f) m->yaw -= 360.0f;
};

void monsteraction(dynent *m)           // main AI thinking routine, called every frame for every monster
{
    if(m->enemy->state==CSStatus::CS_DEAD) { m->enemy = player1; m->anger = 0; };
    normalise(m, m->targetyaw);
    if(m->targetyaw>m->yaw)             // slowly turn monster towards his target
    {
        m->yaw += GameInit::CurrentTime*0.5f;
        if(m->targetyaw<m->yaw) m->yaw = m->targetyaw;
    }
    else
    {
        m->yaw -= GameInit::CurrentTime*0.5f;
        if(m->targetyaw>m->yaw) m->yaw = m->targetyaw;
    };

    vdist(disttoenemy, vectoenemy, m->o, m->enemy->o);                         
    m->pitch = atan2(m->enemy->o.z-m->o.z, disttoenemy)*180/((float)System::Math::PI);         

    if(m->blocked)                                                              // special case: if we run into scenery
    {
        m->blocked = false;
        if(!rnd(20000/monstertypes[m->mtype].speed))                            // try to jump over obstackle (rare)
        {
            m->jumpnext = true;
        }
        else if(m->trigger<GameInit::LastMillis && (m->monsterstate!=MonsterStates::M_HOME || !rnd(5)))  // search for a way around (common)
        {
            m->targetyaw += 180+rnd(180);                                       // patented "random walk" AI pathfinding (tm) ;)
            transition(m, MonsterStates::M_SEARCH, 1, 400, 1000);
        };
    };
    
    float enemyyaw = -(float)atan2(m->enemy->o.x - m->o.x, m->enemy->o.y - m->o.y)/((float)System::Math::PI)*180+180;
    
    switch(m->monsterstate)
    {
        case MonsterStates::M_PAIN:
        case MonsterStates::M_ATTACKING:
        case MonsterStates::M_SEARCH:
            if(m->trigger<GameInit::LastMillis) transition(m, MonsterStates::M_HOME, 1, 100, 200);
            break;
            
        case MonsterStates::M_SLEEP:                       // state classic sp monster start in, wait for visual contact
        {
            vec target;
            if(GameInit::EditMode || !enemylos(m, target)) return;   // skip running physics
            normalise(m, enemyyaw);
            float angle = (float)fabs(enemyyaw-m->yaw);
            if(disttoenemy<8                   // the better the angle to the player, the further the monster can see/hear
            ||(disttoenemy<16 && angle<135)
            ||(disttoenemy<32 && angle<90)
            ||(disttoenemy<64 && angle<45)
            || angle<10)
            {
                transition(m, MonsterStates::M_HOME, 1, 500, 200);
                playsound(Sounds::S_GRUNT1+rnd(2), &m->o);
            };
            break;
        };
        
        case MonsterStates::M_AIMING:                      // this state is the delay between wanting to shoot and actually firing
            if(m->trigger<GameInit::LastMillis)
            {
                m->lastaction = 0;
                m->attacking = true;
                shoot(m, m->attacktarget);
                transition(m, MonsterStates::M_ATTACKING, 0, 600, 0);
            };
            break;

        case MonsterStates::M_HOME:                        // monster has visual contact, heads straight for player and may want to shoot at any time
            m->targetyaw = enemyyaw;
            if(m->trigger<GameInit::LastMillis)
            {
                vec target;
                if(!enemylos(m, target))    // no visual contact anymore, let monster get as close as possible then search for player
                {
                    transition(m, MonsterStates::M_HOME, 1, 800, 500);
                }
                else  // the closer the monster is the more likely he wants to shoot
                {
                    if(!rnd((int)disttoenemy/3+1) && m->enemy->state==CSStatus::CS_ALIVE)         // get ready to fire
                    { 
                        m->attacktarget = target;
                        transition(m, MonsterStates::M_AIMING, 0, monstertypes[m->mtype].lag, 10);
                    }
                    else                                                                // track player some more
                    {
                        transition(m, MonsterStates::M_HOME, 1, monstertypes[m->mtype].rate, 0);
                    };
                };
            };
            break;
    };

    moveplayer(m, 1, false);        // use physics to move monster
};

void monsterpain(dynent *m, int damage, dynent *d)
{
    if(d->monsterstate)     // a monster hit us
    {
        if(m!=d)            // guard for RL guys shooting themselves :)
        {
            m->anger++;     // don't attack straight away, first get angry
            int anger = m->mtype==d->mtype ? m->anger/2 : m->anger;
            if(anger>=monstertypes[m->mtype].loyalty) m->enemy = d;     // monster infight if very angry
        };
    }
    else                    // player hit us
    {
        m->anger = 0;
        m->enemy = d;
    };
    transition(m, MonsterStates::M_PAIN, 0, monstertypes[m->mtype].pain,200);      // in this state monster won't attack
    if((m->health -= damage)<=0)
    {
        m->state = CSStatus::CS_DEAD;
        m->lastaction = GameInit::LastMillis;
        Game::Monster::numkilled++;
        player1->frags = Monster::numkilled;
        playsound(monstertypes[m->mtype].diesound, &m->o);
        int remain = Monster::monstertotal-Monster::numkilled;
        if(remain>0 && remain<=5) conoutf("only %d monster(s) remaining", remain);
    }
    else
    {
        playsound(monstertypes[m->mtype].painsound, &m->o);
    };
};

void endsp(bool allkilled)
{
    conoutf(allkilled ? "you have cleared the map!" : "you reached the exit!");
    conoutf("score: %d kills in %d seconds", Monster::numkilled, (GameInit::LastMillis-Monster::mtimestart)/1000);
    Monster::monstertotal = 0;
    startintermission();
};

void monsterthink()
{
    if(m_dmsp && Monster::spawnremain && GameInit::LastMillis>Monster::nextmonster)
    {
        if(Monster::spawnremain--==Monster::monstertotal) conoutf("The invasion has begun!");
        Monster::nextmonster = GameInit::LastMillis+1000;
        spawnmonster();
    };
    
    if(Monster::monstertotal && !Monster::spawnremain && Monster::numkilled==Monster::monstertotal) endsp(true);
    
    loopv(ents)             // equivalent of player entity touch, but only teleports are used
    {
        entity &e = ents[i];
        if(e.type!=StaticEntity::TELEPORT) continue;
        if(OUTBORD(e.x, e.y)) continue;
        vec v = { e.x, e.y, S(e.x, e.y)->floor };
        loopv(monsters) if(monsters[i]->state==CSStatus::CS_DEAD)
        {
			if(GameInit::LastMillis-monsters[i]->lastaction<2000)
			{
				monsters[i]->move = 0;
				moveplayer(monsters[i], 1, false);
			};
        }
        else
        {
            v.z += monsters[i]->eyeheight;
            vdist(dist, t, monsters[i]->o, v);
            v.z -= monsters[i]->eyeheight;
            if(dist<4) teleport((int)(&e-&ents[0]), monsters[i]);
        };
    };
    
    loopv(monsters) if(monsters[i]->state==CSStatus::CS_ALIVE) monsteraction(monsters[i]);
};

void monsterrender()
{
    loopv(monsters) renderclient(monsters[i], false, monstertypes[monsters[i]->mtype].mdlname, monsters[i]->mtype==5, monstertypes[monsters[i]->mtype].mscale/10.0f);
};
