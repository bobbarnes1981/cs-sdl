// rendergl.cpp: core opengl rendering stuff

#include "cube.h"
#using <mscorlib.dll>

using namespace MezzanineLib;
//#using <Tao.OpenGl.dll>
//using namespace Tao.OpenGl;

extern int curvert;

void purgetextures();

GLUquadricObj *qsphere = NULL;

// management of texture slots
// each texture slot can have multople texture frames, of which currently only the first is used
// additional frames can be used for various shaders

const int MAXTEX = 1000;
int texx[MAXTEX];                           // ( loaded texture ) -> ( name, size )
int texy[MAXTEX];                           
string texname[MAXTEX];
const int FIRSTTEX = 1000;                  // opengl id = loaded id + FIRSTTEX
// std 1+, sky 14+, mdls 20+

const int MAXFRAMES = 2;                    // increase to allow more complex shader defs
int mapping[256][MAXFRAMES];                // ( cube texture, frame ) -> ( opengl id, name )
string mapname[256][MAXFRAMES];

void purgetextures()
{
    loopi(256) loop(j,MAXFRAMES) mapping[i][j] = 0;
};

void texturereset() 
{
	Render::RenderGl::TextureReset(); 
};

void texture(char *aframe, char *name)
{
	int num = Render::RenderGl::CurrentTextureNumber++, frame = atoi(aframe);
    if(num<0 || num>=256 || frame<0 || frame>=MAXFRAMES) return;
    mapping[num][frame] = 1;
    char *n = mapname[num][frame];
    strcpy_s(n, name);
    path(n);
};

COMMAND(texturereset, Support::FunctionSignatures::ARG_NONE);
COMMAND(texture, Support::FunctionSignatures::ARG_2STR);

int lookuptexture(int tex, int &xs, int &ys)
{
    int frame = 0;                      // other frames?
    int tid = mapping[tex][frame];

    if(tid>=FIRSTTEX)
    {
        xs = texx[tid-FIRSTTEX];
        ys = texy[tid-FIRSTTEX];
        return tid;
    };

    xs = ys = 16;
    if(!tid) return 1;                  // crosshair :)

    loopi(Render::RenderGl::CurrentTextureNumber)       // lazily happens once per "texture" command, basically
    {
        if(strcmp(mapname[tex][frame], texname[i])==0)
        {
            mapping[tex][frame] = tid = i+FIRSTTEX;
            xs = texx[i];
            ys = texy[i];
            return tid;
        };
    };

    if(Render::RenderGl::CurrentTextureNumber==MAXTEX) GameInit::Fatal("loaded too many textures");

    int tnum = Render::RenderGl::CurrentTextureNumber+FIRSTTEX;
    strcpy_s(texname[Render::RenderGl::CurrentTextureNumber], mapname[tex][frame]);

    sprintf_sd(name)("packages%c%s", PATHDIV, texname[Render::RenderGl::CurrentTextureNumber]);

	if(Render::RenderGl::InstallTexture(tnum, name, &xs, &ys))
    {
        mapping[tex][frame] = tnum;
        texx[Render::RenderGl::CurrentTextureNumber] = xs;
        texy[Render::RenderGl::CurrentTextureNumber] = ys;
        Render::RenderGl::CurrentTextureNumber++;
        return tnum;
    }
    else
    {
        return mapping[tex][frame] = FIRSTTEX;  // temp fix
    };
};

int skyoglid;

struct strip { int tex, start, num; };
vector<strip> strips;

void renderstripssky()
{
    glBindTexture(GL_TEXTURE_2D, skyoglid);
    loopv(strips) if(strips[i].tex==skyoglid) glDrawArrays(GL_TRIANGLE_STRIP, strips[i].start, strips[i].num);
};

void renderstrips()
{
    int lasttex = -1;
    loopv(strips) if(strips[i].tex!=skyoglid)
    {
        if(strips[i].tex!=lasttex)
        {
            glBindTexture(GL_TEXTURE_2D, strips[i].tex); 
            lasttex = strips[i].tex;
        };
        glDrawArrays(GL_TRIANGLE_STRIP, strips[i].start, strips[i].num);  
    };   
};

void addstrip(int tex, int start, int n)
{
    strip &s = strips.add();
    s.tex = tex;
    s.start = start;
    s.num = n;
};

VARFP(gamma, 30, 100, 300,
{
    float f = gamma/100.0f;
    //if(SDL_SetGamma(f,f,f)==-1)
	//if(SdlDotNet::Video::Gamma(f,f,f)==-1)
	//SdlDotNet::Video::Gamma(f,f,f); //will throw exception if it cannot set it.
	//	conoutf("Could not set gamma (card/driver doesn't support it?)");
	//	conoutf("sdl: %s", SdlDotNet::SdlException::GetError());
    //{
       // conoutf("Could not set gamma (card/driver doesn't support it?)");
		//conoutf("sdl: %s", SdlDotNet::SdlException::GetError()
        //conoutf("sdl: %s", SDL_GetError());
   // };
});

VARP(fov, 10, 105, 120);
VAR(fog, 64, 180, 1024);
VAR(fogcolour, 0, 0x8099B3, 0xFFFFFF);

VARP(hudgun,0,1,1);

void gl_drawframe(int w, int h, float curfps)
{
    float hf = hdr.waterlevel-0.3f;
    float fovy = (float)fov*h/w;
    float aspect = w/(float)h;
    bool underwater = player1->o.z<hf;
    
    glFogi(GL_FOG_START, (fog+64)/8);
    glFogi(GL_FOG_END, fog);
    float fogc[4] = { (fogcolour>>16)/256.0f, ((fogcolour>>8)&255)/256.0f, (fogcolour&255)/256.0f, 1.0f };
    glFogfv(GL_FOG_COLOR, fogc);
    glClearColor(fogc[0], fogc[1], fogc[2], 1.0f);

    if(underwater)
    {
        fovy += (float)sin(MezzanineLib::GameInit::LastMillis/1000.0)*2.0f;
        aspect += (float)sin(MezzanineLib::GameInit::LastMillis/1000.0+System::Math::PI)*0.1f;
        glFogi(GL_FOG_START, 0);
        glFogi(GL_FOG_END, (fog+96)/8);
    };
    
    glClear((player1->outsidemap ? GL_COLOR_BUFFER_BIT : 0) | GL_DEPTH_BUFFER_BIT);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    int farplane = fog*5/2;
    gluPerspective(fovy, aspect, 0.15f, farplane);
    glMatrixMode(GL_MODELVIEW);

	Render::RenderGl::TransPlayer();

    glEnable(GL_TEXTURE_2D);
    
    int xs, ys;
    skyoglid = lookuptexture(TextureNumbers::DEFAULT_SKY, xs, ys);
   
    resetcubes();
            
    curvert = 0;
    strips.setsize(0);
  
    render_world(player1->o.x, player1->o.y, player1->o.z, 
            (int)player1->yaw, (int)player1->pitch, (float)fov, w, h);
    finishstrips();

	Render::RenderGl::SetupWorld();

    renderstripssky();

    glLoadIdentity();
    glRotated(player1->pitch, -1.0, 0.0, 0.0);
    glRotated(player1->yaw,   0.0, 1.0, 0.0);
    glRotated(90.0, 1.0, 0.0, 0.0);
    glColor3f(1.0f, 1.0f, 1.0f);
    glDisable(GL_FOG);
    glDepthFunc(GL_GREATER);
	Render::RenderText::DrawEnvBox(14, fog*4/3);
    glDepthFunc(GL_LESS);
    glEnable(GL_FOG);

	Render::RenderGl::TransPlayer();
        
    Render::RenderGl::OverBright(2);
    
    renderstrips();

    Render::RenderGl::XtraVerts = 0;

    renderclients();
    monsterrender();

    renderentities();

    renderspheres(MezzanineLib::GameInit::CurrentTime);
    renderents();

    glDisable(GL_CULL_FACE);

    Render::RenderGl::DrawHudGun(fovy, aspect, farplane);

    Render::RenderGl::OverBright(1);
    int nquads = renderwater(hf);
    
    Render::RenderGl::OverBright(2);
    render_particles(MezzanineLib::GameInit::CurrentTime);
    Render::RenderGl::OverBright(1);

    glDisable(GL_FOG);

    glDisable(GL_TEXTURE_2D);

    gl_drawhud(w, h, (int)curfps, nquads, curvert, underwater);

    glEnable(GL_CULL_FACE);
    glEnable(GL_FOG);
};

