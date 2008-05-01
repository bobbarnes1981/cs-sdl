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
using System.IO;
using System.Threading;

using SdlDotNet.Core;
using SdlDotNet.Input;

namespace SdlDotNetExamples.LargeDemos
{
    public static class SharpNesMain
    {
        //private static Thread gameThread;
        private static bool gameIsRunning;
        private static NesEngine myEngine;
        //private static ThreadStart myThreadCreator;
        static string fileDirectory = "Data";

        public static string FileDirectory
        {
            get { return SharpNesMain.fileDirectory; }
            set { SharpNesMain.fileDirectory = value; }
        }
        static string filePath = Path.Combine("..", "..");

        public static string FilePath
        {
            get { return SharpNesMain.filePath; }
            set { SharpNesMain.filePath = value; }
        }
        static string fileName = "SolarWars2001.nes";
        static bool fullScreen;

        public static bool FullScreen
        {
            get { return SharpNesMain.fullScreen; }
            set { SharpNesMain.fullScreen = value; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Run()
        {
            //foreach (string arg in args)
            //{
            //    if (!String.IsNullOrEmpty(arg) && arg.Trim().Length != 0)
            //    {
            //        if (arg.Trim() == "-f")
            //        {
            //            fullScreen = true;
            //            Mouse.ShowCursor = false;
            //        }
            //        else
            //        {
            //            fileName = arg.Trim();
            //        }
            //    }
            //}
            Run(fileName);
        }

        public static void Run(string defaultRom)
        {
            if (String.IsNullOrEmpty(defaultRom))
            {
                throw new ArgumentNullException("defaultRom");
            }
            try
            {
                if (File.Exists(defaultRom))
                {
                    filePath = "";
                    fileDirectory = "";
                }
                else if (File.Exists(Path.Combine(fileDirectory, defaultRom)))
                {
                    filePath = "";
                }
                //our game-specific bool
                //FIXME: move this to a more sane place when I figure out where to put it
                gameIsRunning = false;
                myEngine = new NesEngine();
                //If they gave us a ROM to run on the commandline, go ahead and start it up 
                if (!String.IsNullOrEmpty(defaultRom))
                {
                    try
                    {
                        RunCart(Path.Combine(Path.Combine(filePath, fileDirectory), defaultRom));
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.StackTrace);
                        Console.WriteLine("defaultRom: " + defaultRom);
                        Console.WriteLine("fileDirectory: " + fileDirectory);
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("defaultRom: " + defaultRom);
                Console.WriteLine("fileDirectory: " + fileDirectory);
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
        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "SharpNes: NES emulator";
            }
        }
    }
}
