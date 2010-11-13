// sound.cpp: uses sdl_mixer
#include "cube.h"
#include "SDL_mixer.h"
#using <mscorlib.dll>

using namespace MezzanineLib;
using namespace MezzanineLib::Support;

VARP(soundvol, 0, 255, 255);
VARP(musicvol, 0, 128, 255);

struct soundloc { vec loc; bool inuse; } soundlocs[Sound::MAXCHAN];

VAR(soundbufferlen, 128, 1024, 4096);

void music(char *name)
{
	Sound::Music(name);
};

COMMAND(music, FunctionSignatures::ARG_1STR);

int registersound(char *name)
{
	return Sound::RegisterSound(name);
};

COMMAND(registersound, FunctionSignatures::ARG_1EST);

VAR(stereo, 0, 1, 1);

void updatechanvol(int chan, vec *loc)
{
	int vol = soundvol, pan = 255/2;
	if(loc)
	{
		vdist(dist, v, *loc, player1->o);
		vol -= (int)(dist*3*soundvol/255); // simple mono distance attenuation
		if(stereo && (v.x != 0 || v.y != 0))
		{
			float yaw = -atan2(v.x, v.y) - player1->yaw*((float)System::Math::PI) / 180.0f; // relative angle of sound along X-Y axis
			pan = int(255.9f*(0.5*sin(yaw)+0.5f)); // range is from 0 (left) to 255 (right)
		};
	};
	vol = (vol*MIX_MAX_VOLUME)/255;
	Mix_Volume(chan, vol);
	Mix_SetPanning(chan, 255-pan, pan);
};  

void newsoundloc(int chan, vec *loc)
{
	assert(chan>=0 && chan<Sound::MAXCHAN);
	soundlocs[chan].loc = *loc;
	soundlocs[chan].inuse = true;
};

void updatevol()
{
	if(Sound::noSound) return;
	loopi(Sound::MAXCHAN) if(soundlocs[i].inuse)
	{
		if(Mix_Playing(i))
			updatechanvol(i, &soundlocs[i].loc);
		else soundlocs[i].inuse = false;
	};
};

void playsoundc(int n) { addmsg(0, 2, NetworkMessages::SV_SOUND, n); playsound(n); };

void playsound(int n, vec *loc)
{
	Support::Sound::PlaySound(n);
	//if(loc) newsoundloc(chan, loc);
	//updatechanvol(chan, loc);
};

void sound(int n) 
{ 
	Support::Sound::PlaySound(n); 
};
COMMAND(sound, FunctionSignatures::ARG_1INT);
