// rendergl.cpp: core opengl rendering stuff

#include "cube.h"
#using <mscorlib.dll>

using namespace MezzanineLib;
using namespace MezzanineLib::Render;

void purgetextures();

GLUquadricObj *qsphere = NULL;

// management of texture slots
// each texture slot can have multople texture frames, of which currently only the first is used
// additional frames can be used for various shaders

int texx[RenderGl::MAXTEX];                           // ( loaded texture ) -> ( name, size )
int texy[RenderGl::MAXTEX];                           
string texname[RenderGl::MAXTEX];

int mapping[256][RenderGl::MAXFRAMES];                // ( cube texture, frame ) -> ( opengl id, name )
string mapname[256][RenderGl::MAXFRAMES];

void purgetextures()
{
    loopi(256) loop(j,RenderGl::MAXFRAMES) mapping[i][j] = 0;
};

void texturereset() 
{
	Render::RenderGl::TextureReset(); 
};

void texture(char *aframe, char *name)
{
	int num = Render::RenderGl::CurrentTextureNumber++, frame = atoi(aframe);
    if(num<0 || num>=256 || frame<0 || frame>=RenderGl::MAXFRAMES) return;
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

    if(tid>=RenderGl::FIRSTTEX)
    {
        xs = texx[tid-RenderGl::FIRSTTEX];
        ys = texy[tid-RenderGl::FIRSTTEX];
        return tid;
    };

    xs = ys = 16;
    if(!tid) return 1;                  // crosshair :)

    loopi(Render::RenderGl::CurrentTextureNumber)       // lazily happens once per "texture" command, basically
    {
        if(strcmp(mapname[tex][frame], texname[i])==0)
        {
            mapping[tex][frame] = tid = i+RenderGl::FIRSTTEX;
            xs = texx[i];
            ys = texy[i];
            return tid;
        };
    };

    if(Render::RenderGl::CurrentTextureNumber==RenderGl::MAXTEX) GameInit::Fatal("loaded too many textures");

    int tnum = Render::RenderGl::CurrentTextureNumber+RenderGl::FIRSTTEX;
    strcpy_s(texname[Render::RenderGl::CurrentTextureNumber], mapname[tex][frame]);

	sprintf_sd(name)("packages%c%s", GameInit::PATHDIV, texname[Render::RenderGl::CurrentTextureNumber]);

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
        return mapping[tex][frame] = RenderGl::FIRSTTEX;  // temp fix
    };
};

struct strip { int tex, start, num; };
vector<strip> strips;

void renderstripssky()
{
    glBindTexture(GL_TEXTURE_2D, RenderGl::skyoglid);
    loopv(strips) if(strips[i].tex==RenderGl::skyoglid) glDrawArrays(GL_TRIANGLE_STRIP, strips[i].start, strips[i].num);
};
void setstrips()
{
	strips.setsize(0);
};

void renderstrips()
{
    int lasttex = -1;
    loopv(strips) if(strips[i].tex!=RenderGl::skyoglid)
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
   // float f = gamma/100.0f;
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

//VARP(fov, 10, 105, 120);
//VAR(fog, 64, 180, 1024);
//VAR(fogcolour, 0, 0x8099B3, 0xFFFFFF);

VARP(hudgun,0,1,1);