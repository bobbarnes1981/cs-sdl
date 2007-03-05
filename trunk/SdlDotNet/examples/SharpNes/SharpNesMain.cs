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

// created on 2/4/2005 at 8:26 PM
using System;
using System.Threading;
using SdlDotNet.Core;

namespace SdlDotNetExamples.SharpNes
{
    public static class SharpNesMain
    {
        //private static Thread gameThread;
        private static bool gameIsRunning;
        private static NesEngine myEngine;
        //private static ThreadStart myThreadCreator;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //If they gave us a rom name on the commandline, run it.
            if (args.GetLength(0) == 0)
            {
                Run("MarioBro.nes");
            }
            else
            {
                Run(args[0]);
            }
        }

        public static void Run(string defaultRom)
        {
            //our game-specific bool
            //FIXME: move this to a more sane place when I figure out where to put it
            gameIsRunning = false;
            myEngine = new NesEngine();
            //If they gave us a ROM to run on the commandline, go ahead and start it up 
            if (!String.IsNullOrEmpty(defaultRom))
            {
                RunCart(defaultRom);
            }
        }

        static void RunCart(string filename)
        {
            if (gameIsRunning)
            {
                //myEngine.QuitEngine(this, new QuitEventArgs());
                //while (!myEngine.hasQuit);

                //gameThread.Join();
                gameIsRunning = false;
                myEngine.RestartEngine();
            }

            if (myEngine.LoadCart(filename))
            {
                //myThreadCreator = new ThreadStart(myEngine.RunCart);
                //gameThread = new Thread(myThreadCreator);
                //gameThread.Start();
                myEngine.RunCart();
                gameIsRunning = true;
            }
            //Events.Run();
        }

        //void PlayPauseActivated(object o, EventArgs e)
        //{
        //    myEngine.TogglePause();
        //}

        //void FullScreenActivated(object o, EventArgs e)
        //{
        //    myEngine.MyPpu.MyVideo.ToggleFullscreen();
        //}

        //void QuitActivated(object o, EventArgs e)
        //{
        //    if (!myEngine.IsQuitting)
        //    {
        //        myEngine.QuitEngine();
        //        //gameThread.Join();
        //    }
        //}
    }
}