// main.cpp: initialisation & main loop

#include "cube.h"
#using <mscorlib.dll>
#using <TessLib.dll>

void quit()                     // normal exit
{
	TessLib::GameInit::Quit();
};

void *alloc(int s)              // for some big chunks... most other allocs use the memory pool
{
    void *b = calloc(1,s);
	if(!b) TessLib::GameInit::Fatal("out of memory!");
    return b;
};

void screenshot()
{
	TessLib::GameInit::Screenshot();
};

COMMAND(screenshot, ARG_NONE);
COMMAND(quit, ARG_NONE);

dynent * getplayer1(void)
{
	return player1;
}

VARF(gamespeed, 10, 100, 1000, if(multiplayer()) gamespeed = 100);