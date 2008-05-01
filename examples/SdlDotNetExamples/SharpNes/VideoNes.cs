#region License
/*
Copyright (c) 2005, Jonathan Turner
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of Sharpnes nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion License

// created on 10/24/2004 at 11:51
using System;
using System.Runtime.InteropServices;
using SdlDotNet.Graphics;
using System.Threading;

namespace SdlDotNetExamples.LargeDemos
{
    public class VideoNes
    {
        IntPtr sdlBuffer;	 //to be displayed buffer
        int bpp = 16;
        int width = 256;
        int height = 224;
        DateTime dtbefore;
        DateTime dtafter;
        int fps;
        int framecount;
        Ppu myPPU;

        bool willSleep;
        int sleepTime;

        public void BlitScreen()
        {
            //int i;
            //int j;
#if FOURSCREEN
		int currentNameTableAddress = myPPU.nameTableAddress;
		//FIXME: This is for debugging
		unsafe {
			short *p = (short *)sdlBuffer;
			int f, g;
			//int maxlength = width*height;
			int maxlength = 256 * 224;
			
			for (f = 0; f < maxlength; f++)
			{
				p[(f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
			}
		}

		myPPU.nameTableAddress = 0x2000;
		myPPU.scrollH = 0;
		myPPU.scrollV = 0;
		for (i = 0; i < 240; i++)
		{
			myPPU.currentScanline = i;
			//Clean up the line from before
			for (j = 0; j < 256; j++)
			{
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + i] = (short)myPPU.Nes_Palette[myPPU.nameTables[0x1f00]];
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = 0;
				myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = (short)myPPU.Nes_Palette[myPPU.nameTables[0x1f00]];
			}
			
			myPPU.RenderBackground();
		}
		myPPU.currentScanline = 240;
		//FIXME: This is for debugging
		unsafe {
			short *p = (short *)sdlBuffer;
			int f, g;
			//int maxlength = width*height;
			int maxlength = 256 * 224;
			
			for (f = 0; f < maxlength; f++)
			{
				p[264 + (f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
			}
		}

		myPPU.nameTableAddress = 0x2400;
		myPPU.scrollH = 0;
		myPPU.scrollV = 0;
		for (i = 0; i < 240; i++)
		{
			myPPU.currentScanline = i;
			//Clean up the line from before
			for (j = 0; j < 256; j++)
			{
				//offscreenBuffer[(currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = 0;
				myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = (short)myPPU.Nes_Palette[myPPU.nameTables[0x1f00]];
			}
			
			myPPU.RenderBackground();
		}
		myPPU.currentScanline = 240;
		//FIXME: This is for debugging
		unsafe {
			short *p = (short *)sdlBuffer;
			int f, g;
			//int maxlength = width*height;
			int maxlength = 256 * 224;
			
			for (f = 0; f < maxlength; f++)
			{
				//p[187980 + 264 + (f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
				p[256 + 264 + (f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
			}
		}
		
		myPPU.nameTableAddress = 0x2800;
		myPPU.scrollH = 0;
		myPPU.scrollV = 0;
		for (i = 0; i < 240; i++)
		{
			myPPU.currentScanline = i;
			//Clean up the line from before
			for (j = 0; j < 256; j++)
			{
				//offscreenBuffer[(currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = 0;
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = (short)myPPU.Nes_Palette[myPPU.nameTables[0x1f00]];
			}
			
			myPPU.RenderBackground();
		}
		myPPU.currentScanline = 240;
		//FIXME: This is for debugging
		unsafe {
			short *p = (short *)sdlBuffer;
			int f, g;
			//int maxlength = width*height;
			int maxlength = 256 * 224;
			
			for (f = 0; f < maxlength; f++)
			{
				p[187980 + 264 + (f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
			}
		}
		myPPU.nameTableAddress = 0x2C00;
		myPPU.scrollH = 0;
		myPPU.scrollV = 0;
		for (i = 0; i < 240; i++)
		{
			myPPU.currentScanline = i;
			//Clean up the line from before
			for (j = 0; j < 256; j++)
			{
				//offscreenBuffer[(currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = 0;
				//myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + i] = (int)Nes_Palette[(int)nameTables[0x1f00]];
				myPPU.offscreenBuffer[(myPPU.currentScanline * 256) + j] = (short)myPPU.Nes_Palette[myPPU.nameTables[0x1f00]];
			}
			
			myPPU.RenderBackground();
		}
		myPPU.currentScanline = 240;
		//FIXME: This is for debugging
		unsafe {
			short *p = (short *)sdlBuffer;
			int f, g;
			//int maxlength = width*height;
			int maxlength = 256 * 224;
			
			for (f = 0; f < maxlength; f++)
			{
				p[187980 + 256 + 264 + (f % 256) + 780 * (f / 256)] = myPPU.offscreenBuffer[f];
			}
		}
		
		myPPU.nameTableAddress = currentNameTableAddress;
#else
            Marshal.Copy(myPPU.OffScreenBuffer, 256 * 8, sdlBuffer, width * height);
#endif
            Video.Update();
            //Sdl.SDL_Flip(surfacePtr);
            //FIXME: Come up with a better way of frame limiting

            //Sdl.SDL_Delay((int)(16.667 - ((dtafter-dtbefore).Ticks / 10000.0)));
            //Console.WriteLine("Will Delay: {0}", 60 * ((dtafter-dtbefore).Ticks / 1000000.0));
            framecount++;
            if ((framecount % 100) == 0)
            {
                //FIXME: This assumes NTSC

                dtafter = DateTime.Now;
                fps = (int)(((dtafter - dtbefore).Ticks) / 100000);
                //Console.WriteLine("Current Speed: {0}", 100.0 / fps);
                if (fps < 100)
                {
                    willSleep = true;
                    sleepTime++;
                    //Console.WriteLine("Will Delay: {0}", (100 - fps) * 1000);
                }
                dtbefore = DateTime.Now;
            }
            if (willSleep)
            {
                Thread.Sleep(sleepTime);
            }
            //Thread.Sleep(10);
        }

        public VideoNes(Ppu thePPU)
        {
            //Initialize video emulation framework
            myPPU = thePPU;
            //END Initialize video emulation framework
        }

        //public void ToggleFullScreen()
        //{
        //    int mouse_state;

        //    Sdl.SDL_WM_ToggleFullScreen(surfacePtr);

        //    mouse_state = Sdl.SDL_ShowCursor(Sdl.SDL_QUERY);
        //    if (mouse_state == Sdl.SDL_ENABLE)
        //        Sdl.SDL_ShowCursor(Sdl.SDL_DISABLE);
        //    else
        //        Sdl.SDL_ShowCursor(Sdl.SDL_ENABLE);

        //    /*
        //    gm = (Sdl.SDL_GrabMode)Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_QUERY);
        //    if (gm == Sdl.SDL_GrabMode.SDL_GRAB_OFF)
        //        Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_ON);
        //    else
        //        Sdl.SDL_WM_GrabInput(Sdl.SDL_GrabMode.SDL_GRAB_OFF);
        //    */
        //}

        public void StartVideo()
        {
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - NES Window";
            Surface surfaceVideo = Video.SetVideoMode(width, height, bpp, false, SharpNesMain.FullScreen);


            //DEBUG ONLY!!
#if FOURSCREEN		
		debugBackgroundPtr = Sdl.SDL_SetVideoMode(
			780, 
			480, 
			bpp, 
			flags);
#endif
            sdlBuffer = surfaceVideo.Pixels;

            //END Initialize the SDL frontend

            dtbefore = DateTime.Now;
            framecount = 0;

            willSleep = false;
            sleepTime = 0;
        }
    }
}
