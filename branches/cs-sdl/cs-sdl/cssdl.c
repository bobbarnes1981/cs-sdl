// dmutils created on 14/07/2001 at 00:56
//gcc -shared -o dmutils.dll dmutils.c -lSDL
#ifdef WIN32
	#define DLLOBJ  __declspec(dllexport)
#else
	#define DLLOBJ
#endif

#include <stdlib.h>
#include <stdio.h>
#include <errno.h>
#include <string.h>
#include <signal.h>
#include "SDL.h"

// Pointer
DLLOBJ void DMmemcpy(void * dst, void * src, unsigned size)
{
	memcpy(dst, src, (size_t) size);
}
DLLOBJ void DMmemset(void * dst, char c, unsigned len)
{
	memset(dst, c, (size_t) len);
}

// Library
#ifdef WIN32
#include <windows.h>
#endif
DLLOBJ int DMCheckLibrary(char * s) {
	void * lib;
#ifdef WIN32
	lib = LoadLibrary(s);
	if(lib)
		FreeLibrary(lib);
#else // UNIX ? // is it ok ? i haven't test it so far...
	lib = dlopen(s, 0 /* arg seems not matter */);
	if(lib)
		dlclose(lib);
#endif
	return lib == NULL ? 0 : -1;
}

// Signal
// hack due to calling convention...
typedef void (__stdcall * SGNCALLBACK) (int);
static SGNCALLBACK SIGNALS_TABLE[] = 
{
	NULL, NULL, NULL, NULL, NULL, 
	NULL, NULL, NULL, NULL, NULL, 
	NULL, NULL, NULL, NULL, NULL, 
	NULL, NULL, NULL, NULL, NULL, 
	NULL, NULL, NULL, NULL, NULL, 
	NULL, NULL, NULL, NULL, NULL
};
static int FCN_SIZE = sizeof(SIGNALS_TABLE) / sizeof(SGNCALLBACK);
static void MySignal(int sgn)
{
	if(sgn >= FCN_SIZE)
		return;
	if(SIGNALS_TABLE[sgn])
		SIGNALS_TABLE[sgn] (sgn);
}
DLLOBJ void DMsignal(int sig, void (__stdcall * fct)(int))
{
	if(sig >= FCN_SIZE)
		return;
	
	switch((int) fct) {
		case SIG_DFL:
		case SIG_IGN:
			SIGNALS_TABLE[sig] = NULL;
			signal(sig, (void (*)(int)) fct);
			break;
		default:
			SIGNALS_TABLE[sig] = fct;
			signal(sig, &MySignal);
	}
}
DLLOBJ void DMexit(int err)
{
	exit(err);
}
DLLOBJ void DMatexit(void (* fct)())
{
	atexit(fct);
}

// general
#define N 256
static char globerr[N] = "";
static void setError(char * s)
{
	int i;
	if(!s)
		s = "unknown error";

	globerr[N-1] = 0;
	for(i=0; i<N-1; i++) {
		globerr[i] = s[i];
		if(!s[i])
			break;
	}
}

/** any cleaning function */
DLLOBJ void DMExitHandler()
{
	SDL_Quit();
}
/** call at init to be sure that cleaning function are called 
 * at exit time.
 *
 * @return 0 on success
 */
DLLOBJ int DMInstallExitHandler()
{
	int ret = atexit(DMExitHandler);
    if(ret) {
    	setError(strerror(errno));
    	return ret;
    }
	return 0;
}
/** get last error of this module */
DLLOBJ char * DMInstallHandlerError()
{
	return globerr;
}

DLLOBJ SDL_Surface * DMLoadBmp(char * filename) {
	return SDL_LoadBMP(filename);
}

/** get sys endianity */
DLLOBJ int DMEndianity() { return SDL_BYTEORDER; }

/* SDL_VideoDriverName */
static char vid_driver_name[256];
DLLOBJ void DMVideoDriver(char ** s) {
	*s = SDL_VideoDriverName(vid_driver_name, 256);
}


// ---------------------------------------
// SDL_RWops convenience functions

static int (_stdcall * cs_seek) (struct SDL_RWops *context, int offset, int whence) = NULL;
static int (_stdcall * cs_read) (struct SDL_RWops *context, int size, void * ptr) = NULL;
static int (_stdcall * cs_write)(struct SDL_RWops *context, int size, const void *ptr) = NULL;
static int (_stdcall * cs_close)(struct SDL_RWops *context) = NULL;
DLLOBJ void DMInitRWops(
	int (_stdcall * fseek) (struct SDL_RWops *context, int offset, int whence), 
	int (_stdcall * fread) (struct SDL_RWops *context, int size, void * ptr), 
	int (_stdcall * fwrite)(struct SDL_RWops *context, int size, const void *ptr), 
	int (_stdcall * fclose)(struct SDL_RWops *context))
{
	cs_seek  = fseek;
	cs_read  = fread;
	cs_write = fwrite;
	cs_close = fclose;
}

// hack because of calling convention (& provide byte[] instead of byte *)...
static int c_seek(struct SDL_RWops *context, int offset, int whence) {
	return cs_seek(context, offset, whence); 
}
static int c_read(struct SDL_RWops *context, void * ptr, int size, int maxnum) {
	int ret = cs_read(context, size * maxnum, ptr);
	return ret > 0 ? ret / size : ret;
}
static int c_write(struct SDL_RWops *context, const void *ptr, int size, int maxnum) {
	int ret = cs_write(context, size * maxnum, ptr);
	return ret > 0 ? ret / size : maxnum;
}
static int c_close(struct SDL_RWops *context) {
	return cs_close(context);
}
DLLOBJ SDL_RWops * DMRWopsFromStream()
{
	struct SDL_RWops * io = SDL_AllocRW();
	if(!io)
		return NULL;
	io->seek  = &c_seek;
	io->read  = &c_read;
	io->write = &c_write;
	io->close = &c_close;
	return io;
}
DLLOBJ void DMRWopsFree(SDL_RWops * io)
{
	if(!io)
		return;
	SDL_FreeRW(io);
}

// -------------------------------------
// SDL_mixer convenience function
#if 0
static void (_stdcall * cs_mix_func) (void *udata, Uint8 *stream, int len) = NULL;
static void (_stdcall * cs_postmix_func) (void *udata, Uint8 *stream, int len), void *arg) = NULL;

static void c_mix_func(void *udata, Uint8 *stream, int len) {
	if(cs_mix_func)
		cs_mix_func(udata, stream, len);
}
static void c_postmix_func(void *udata, Uint8 *stream, int len), void *arg) {
	if(cs_postmix_func)
		cs_postmix_func(udata, stream, len);
}

DLLOBJ void DM_Mix_SetPostMix(PostMix mix_func, void * arg) {
	cs_postmix_func = mix_func;
	if(mix_func)
		Mix_SetPostMix(c_postmix_func, arg);
	else
		Mix_SetPostMix(NULL, NULL);
}
DLLOBJ void DM_Mix_HookMusic(HookMusic mix_func, void * arg) {
	cs_mix_func = mix_func;
	if(mix_func)
		Mix_HookMix(c_mix_func, arg);
	else
		Mix_HookMix(NULL, NULL);
}
#endif
