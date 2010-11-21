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

// created on 10/24/2004 at 11:39
using System;

namespace SdlDotNetExamples.LargeDemos
{
    //The base class that defines how an engine works
    public class EngineBase
    {
        /* 
           There is a sizeable handful of tasks the engine will have to accomplish.  In 
           our case it's going to control the memory and cpu, attach itself to a video 
           output, and load the cart into its memory.  Every console will need to be able 
           to do all four of those tasks, so we abstract away what functions, at a minimum, 
           each must provide. 
        */

        // Our CPU functions
        // FIXME: should SetPC and GetPC type functions be here as well?
        public virtual void RunNextInstruction()
        {
            return;
        }

        // Our memory functions.  In the future these could be consolidated, but I've let
        // them separate for the sake of the slight speed gain that grants us
        // FIXME: these are commented out because of a ushort vs. uint issue
        /*
        public virtual byte ReadMemory8(uint address)
        {
            return 0;
        }
	
        public virtual ushort ReadMemory16(uint address)
        {
            return 0;
        }
	
        public virtual byte WriteMemory8(uint address, byte data)
        {
            return 0;
        }
	
        public virtual byte WriteMemory16(uint address, ushort data)
        {
            return 0;
        }
        */
        // Our video functions.  There are two sides to video, the internal renderer
        // and the external display.
        // FIXME: I need a way to think about scanlines in an abstract way
        public virtual void RenderNextScanline()
        {
            return;
        }

        public virtual void DisplayToVideo()
        {
            return;
        }

        //public virtual void InitializeEngine()
        //{
        //    return;
        //}

        // Our cart load function
        // FIXME: look up error handling in C# so failures can be handled correctly

        public virtual byte LoadCart(string fileName, string numInstructions)
        {
            return 0;
        }

        public virtual void RunCart()
        {
            return;
        }
    }
}