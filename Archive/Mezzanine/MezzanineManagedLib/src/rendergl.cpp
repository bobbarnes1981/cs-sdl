// rendergl.cpp: core opengl rendering stuff

#include "cube.h"
#using <mscorlib.dll>

using namespace MezzanineLib;
using namespace MezzanineLib::Render;

void texturereset() 
{
	RenderGl::TextureReset(); 
};

void texture(char *aframe, char *name)
{
	RenderGl::Texture(aframe, name);
};

COMMAND(texturereset, Support::FunctionSignatures::ARG_NONE);
COMMAND(texture, Support::FunctionSignatures::ARG_2STR);

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