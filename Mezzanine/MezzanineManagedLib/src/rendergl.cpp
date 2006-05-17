// rendergl.cpp: core opengl rendering stuff

#include "cube.h"
#using <mscorlib.dll>

using namespace MezzanineLib;
using namespace MezzanineLib::Render;

void texturereset() 
{
	Render::RenderGl::TextureReset(); 
};

void texture(char *aframe, char *name)
{
	RenderGl::Texture(aframe, name);
};

COMMAND(texturereset, Support::FunctionSignatures::ARG_NONE);
COMMAND(texture, Support::FunctionSignatures::ARG_2STR);

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