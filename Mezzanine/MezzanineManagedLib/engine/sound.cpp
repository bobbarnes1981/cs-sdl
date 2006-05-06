// sound.cpp: uses  sdl_mixer

#include "pch.h"
#include "engine.h"

bool nosound = true;

#define MAXCHAN 32
#define SOUNDFREQ 22050

struct soundloc { vec loc; bool inuse; int sound; } soundlocs[MAXCHAN];
struct _Mix_Music {}; //Managed C++ hack

#include "SDL_mixer.h"
#define MAXVOL MIX_MAX_VOLUME
Mix_Music *mod = NULL;
void *stream = NULL;    // TODO

void setmusicvol(int musicvol)
{
	if(nosound) return;
	if(mod) Mix_VolumeMusic((musicvol*MAXVOL)/255);
};

VARP(soundvol, 0, 255, 255);
VARFP(musicvol, 0, 128, 255, setmusicvol(musicvol));

void stopsound()
{
	if(nosound) return;
	if(mod)
	{
		Mix_HaltMusic();
		Mix_FreeMusic(mod);
		mod = NULL;
	};
	if(stream)
	{
		stream = NULL;
	};
};

VAR(soundbufferlen, 128, 1024, 4096);

void initsound()
{
	memset(soundlocs, 0, sizeof(soundloc)*MAXCHAN);
	if(Mix_OpenAudio(SOUNDFREQ, MIX_DEFAULT_FORMAT, 2, soundbufferlen)<0)
	{
		conoutf("sound init failed (SDL_mixer): %s", (size_t)Mix_GetError());
		return;
	};
	Mix_AllocateChannels(MAXCHAN);	
	nosound = false;
};

string musicdonecmd;

void musicdone()
{
	if(mod) Mix_FreeMusic(mod);
	mod = NULL;
	stream = NULL;
	if(musicdonecmd[0]) execute(musicdonecmd);
};

void music(char *name, char *cmd)
{
	if(nosound) return;
	stopsound();
	if(soundvol && musicvol)
	{
		if(cmd[0]) s_strcpy(musicdonecmd, cmd);
		else musicdonecmd[0] = 0;
		string sn;
		s_strcpy(sn, "packages/");
		s_strcat(sn, name);
		if(mod = Mix_LoadMUS(path(sn)))
		{
			Mix_HookMusicFinished(cmd[0] ? musicdone : NULL);
			Mix_PlayMusic(mod, cmd[0] ? 0 : -1);
			Mix_VolumeMusic((musicvol*MAXVOL)/255);
		}
		else
		{
			conoutf("could not play music: %s", sn);
		};
	};
};

COMMAND(music, ARG_2STR);

struct sample
{
	char *name;
	int vol;
	Mix_Chunk *sound;

	~sample() { delete[] name; };
};

vector<sample> samples;

int registersound(char *name, char *vol)
{
	loopv(samples) if(strcmp(samples[i].name, name)==0) return i;
	sample &s = samples.add();
	s.name = newstring(name);
	s.sound = NULL;
	s.vol = atoi(vol);
	if(!s.vol) s.vol = 100;
	return samples.length()-1;
};

COMMAND(registersound, ARG_2STR);

void clear_sound()
{
	if(nosound) return;
	stopsound();
	samples.setsize(0);
	Mix_CloseAudio();
};

VAR(stereo, 0, 1, 1);

void updatechanvol(int chan, vec *loc, int sound)
{
	int vol = soundvol, pan = 255/2;
	if(loc)
	{
		vec v;
		float dist = player->o.dist(*loc, v);
		vol -= (int)(dist*3/4*soundvol/255); // simple mono distance attenuation
		if(vol<0) vol = 0;
		if(stereo && (v.x != 0 || v.y != 0))
		{
			float yaw = -atan2(v.x, v.y) - player->yaw*RAD; // relative angle of sound along X-Y axis
			pan = int(255.9f*(0.5*sin(yaw)+0.5f)); // range is from 0 (left) to 255 (right)
		};
	};
	vol = (vol*MAXVOL*samples[sound].vol)/255/255;
	Mix_Volume(chan, vol);
	Mix_SetPanning(chan, 255-pan, pan);
};  

void newsoundloc(int chan, vec *loc, int sound)
{
	ASSERT(chan>=0 && chan<MAXCHAN);
	soundlocs[chan].loc = *loc;
	soundlocs[chan].inuse = true;
	soundlocs[chan].sound = sound;
};

void updatevol()
{
	if(nosound) return;
	loopi(MAXCHAN) if(soundlocs[i].inuse)
	{
		if(Mix_Playing(i))
			updatechanvol(i, &soundlocs[i].loc, soundlocs[i].sound);
		else soundlocs[i].inuse = false;
	};
};

int soundsatonce = 0, lastsoundmillis = 0;

void playsound(int n, vec *loc)
{
	if(nosound) return;
	if(!soundvol) return;
	if(lastmillis==lastsoundmillis) soundsatonce++; else soundsatonce = 1;
	lastsoundmillis = lastmillis;
	if(soundsatonce>5) return;  // avoid bursts of sounds with heavy packetloss and in sp
	if(n<0 || n>=samples.length()) { conoutf("unregistered sound: %d", n); return; };

	if(!samples[n].sound)
	{
		s_sprintfd(buf)("packages/sounds/%s.wav", samples[n].name);
		samples[n].sound = Mix_LoadWAV(path(buf));
		if(!samples[n].sound) { conoutf("failed to load sample: %s", buf); return; };
	};

	int chan = Mix_PlayChannel(-1, samples[n].sound, 0);
	if(chan<0) return;
	if(loc) newsoundloc(chan, loc, n);
	updatechanvol(chan, loc, n);
};

void sound(int n) { playsound(n, NULL); };
COMMAND(sound, ARG_1INT);


