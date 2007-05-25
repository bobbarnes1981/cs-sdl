#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.IO;
using System.Threading;
using System.Globalization;

using SdlDotNet.Input;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

using System.Text;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public enum UIScreenType
    {
        /* not including title */
        /// <summary>
        ///
        /// </summary>
        MainMenu,
        /// <summary>
        ///
        /// </summary>
        LogOn,
        /// <summary>
        ///
        /// </summary>
        Connection,
        /// <summary>
        ///
        /// </summary>
        ScreenCount
    }

    /// <summary>
    ///
    /// </summary>
    public class Game
    {
        UIScreen[] screens;

        const int GAME_ANIMATION_TICK = 50; // number of milliseconds between animation updates

        bool isBroodWar;
        bool playingBroodWar;

        Race race;

        Mpq broodatMpq;
        Mpq stardatMpq;
        Mpq bwInstallExe;
        Mpq scInstallExe;

        MpqContainer installedMpq;
        MpqContainer playingMpq;

        Painter painter;

        uint cachedCursorX;
        uint cachedCursorY;

        string rootDir;

        static Game instance;

        /// <summary>
        ///
        /// </summary>
        public static Game Instance
        {
            get { return instance; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scProgramDir"></param>
        /// <param name="scCDDir"></param>
        /// <param name="bwCDDir"></param>
        public Game(string scProgramDir, string scCDDir, string bwCDDir)
        {
            instance = this;

            screens = new UIScreen[(int)UIScreenType.ScreenCount];

            installedMpq = new MpqContainer();
            playingMpq = new MpqContainer();

            if (scProgramDir != null)
            {
                foreach (string path in Directory.GetFileSystemEntries(scProgramDir))
                {
                    if (String.Compare(Path.GetFileName(path), "broodat.mpq", true, CultureInfo.CurrentCulture) == 0)
                    {
                        try
                        {
                            broodatMpq = GetMpq(path);
                            Console.WriteLine("found BrooDat.mpq");
                        }
                        catch (SCException e)
                        {
                            throw new SCException(String.Format(CultureInfo.CurrentCulture, "Could not read mpq archive {0}",
                            path), e);
                        }
                    }
                    else if (String.Compare(Path.GetFileName(path), "stardat.mpq", true, CultureInfo.CurrentCulture) == 0)
                    {
                        try
                        {
                            stardatMpq = GetMpq(path);
                            Console.WriteLine("found StarDat.mpq");
                        }
                        catch (SCException e)
                        {
                            throw new SCException(String.Format(CultureInfo.CurrentCulture, "could not read mpq archive {0}",
                            path), e);
                        }
                    }
                }
            }

            if (stardatMpq == null)
            {
                throw new SCException("unable to locate stardat.mpq, please check your SCDirectory configuration setting");
            }

            if (scCDDir != null)
            {
                foreach (string path in Directory.GetFileSystemEntries(scCDDir))
                {
                    if (String.Compare(Path.GetFileName(path), "install.exe", true, CultureInfo.CurrentCulture) == 0)
                    {
                        try
                        {
                            scInstallExe = GetMpq(path);
                            Console.WriteLine("found SC install.exe");
                        }
                        catch (SCException e)
                        {
                            throw new SCException(String.Format(CultureInfo.CurrentCulture, "could not read mpq archive {0}",
                            path),
                            e);
                        }
                    }
                }
            }

            if (bwCDDir != null)
            {
                foreach (string path in Directory.GetFileSystemEntries(bwCDDir))
                {
                    if (String.Compare(Path.GetFileName(path), "install.exe", true, CultureInfo.CurrentCulture) == 0)
                    {
                        try
                        {
                            bwInstallExe = GetMpq(path);
                            Console.WriteLine("found BW install.exe");
                        }
                        catch (SCException e)
                        {
                            throw new SCException(String.Format(CultureInfo.CurrentCulture, "could not read mpq archive {0}",
                            path),
                            e);
                        }
                    }
                }
            }

            if (bwInstallExe == null)
            {
                throw new SCException("unable to locate broodwar cd's install.exe, please check your BroodwarCDDirectory configuration setting");
            }

            if (broodatMpq != null)
            {
                installedMpq.Add(broodatMpq);
            }
            if (bwInstallExe != null)
            {
                installedMpq.Add(bwInstallExe);
            }
            if (stardatMpq != null)
            {
                installedMpq.Add(stardatMpq);
            }
            if (scInstallExe != null)
            {
                installedMpq.Add(broodatMpq);
            }

            PlayingBroodWar = (broodatMpq != null);
            isBroodWar = (broodatMpq != null);

            this.rootDir = scProgramDir;
        }

        static Mpq GetMpq(string path)
        {
            if (Directory.Exists(path))
            {
                return new MpqDirectory(path);
            }
            else if (File.Exists(path))
            {
                return new MpqArchiveContainer(path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string RootDirectory
        {
            get { return rootDir; }
        }

        /// <summary>
        ///
        /// </summary>
        public bool PlayingBroodWar
        {
            get { return playingBroodWar; }
            set
            {
                playingBroodWar = value;
                playingMpq.Clear();
                if (playingBroodWar)
                {
                    if (bwInstallExe == null)
                    {
                        throw new SCException("you need the Broodwar CD to play Broodwar games. Please check the BroodwarCDDirectory configuration setting.");
                    }
                    playingMpq.Add(bwInstallExe);
                    playingMpq.Add(broodatMpq);
                    playingMpq.Add(stardatMpq);
                }
                else
                {
                    if (scInstallExe == null)
                    {
                        throw new SCException("you need the Starcraft CD to play original games. Please check the StarcraftCDDirectory configuration setting.");
                    }
                    playingMpq.Add(scInstallExe);
                    playingMpq.Add(stardatMpq);
                }
            }

        }

        /// <summary>
        ///
        /// </summary>
        public bool IsBroodWar
        {
            get { return isBroodWar; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fullScreen"></param>
        public void Startup(bool fullScreen)
        {
            /* create our window and hook up to the events we care about */
            CreateWindow(fullScreen);

            Events.UserEvent += UserEvent;
            Events.MouseMotion += PointerMotion;
            Events.MouseButtonDown += MouseButtonDown;
            Events.MouseButtonUp += MouseButtonUp;
            Events.KeyboardUp += KeyboardUp;
            Events.KeyboardDown += KeyboardDown;
            //Events.Quit += Quit;

            DisplayTitle();

            /* start everything up */
            Events.Run();
        }

        /// <summary>
        ///
        /// </summary>
        internal static void Quit(object sender, EventArgs e)
        {
            Events.QuitApplication();
        }

        void DisplayTitle()
        {
            /* create the title screen, and make sure we
            don't start loading anything else until
            it's on the screen */
            UIScreen screen = new TitleScreen(installedMpq);
            screen.FirstPainted += TitleScreenReady;
            SwitchToScreen(screen);
        }

        void CreateWindow(bool fullScreen)
        {
            Video.WindowIcon();
            Video.WindowCaption = "SCSharp";

            painter = new Painter(fullScreen, GAME_ANIMATION_TICK);
        }

        void UserEvent(object sender, UserEventArgs args)
        {
            EventHandler<SCEventArgs> d = (EventHandler<SCEventArgs>)args.UserEvent;
            d(this, new SCEventArgs());
        }

        void PointerMotion(object sender, MouseMotionEventArgs args)
        {
            cachedCursorX = (uint)args.X;
            cachedCursorY = (uint)args.Y;

            if (cursor != null)
            {
                cursor.SetPosition(cachedCursorX, cachedCursorY);
            }

            if (currentScreen != null)
            {
                currentScreen.HandlePointerMotion(args);
            }
        }

        void MouseButtonDown(object sender, MouseButtonEventArgs args)
        {
            cachedCursorX = (uint)args.X;
            cachedCursorY = (uint)args.Y;

            if (cursor != null)
            {
                cursor.SetPosition(cachedCursorX, cachedCursorY);
            }

            if (currentScreen != null)
            {
                currentScreen.HandleMouseButtonDown(args);
            }
        }

        void MouseButtonUp(object sender, MouseButtonEventArgs args)
        {
            cachedCursorX = (uint)args.X;
            cachedCursorY = (uint)args.Y;

            if (cursor != null)
            {
                cursor.SetPosition(cachedCursorX, cachedCursorY);
            }

            if (currentScreen != null)
            {
                currentScreen.HandleMouseButtonUp(args);
            }
        }

        void KeyboardUp(object sender, KeyboardEventArgs args)
        {
            if (currentScreen != null)
            {
                currentScreen.HandleKeyboardUp(args);
            }
        }

        void KeyboardDown(object sender, KeyboardEventArgs args)
        {
#if !RELEASE
            if ((args.Mod & ModifierKeys.LeftControl) != 0)
            {
                if (args.Key == Key.Q)
                {
                    Quit(this, new SCEventArgs());
                }
                else if (args.Key == Key.F)
                {
                    painter.FullScreen = !painter.FullScreen;
                }
            }
#endif
            if (currentScreen != null)
            {
                currentScreen.HandleKeyboardDown(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Painter Painter
        {
            get { return painter; }
        }

        /// <summary>
        ///
        /// </summary>
        public Race Race
        {
            get { return race; }
            set { race = value; }
        }

        CursorAnimator cursor;

        /// <summary>
        ///
        /// </summary>
        public CursorAnimator Cursor
        {
            get { return cursor; }
            set
            {
                if (cursor != null)
                {
                    painter.Remove(Layer.Cursor, cursor.Paint);
                }
                cursor = value;
                if (cursor == null)
                {
                    Mouse.ShowCursor = true;
                }
                else
                {
                    painter.Add(Layer.Cursor, cursor.Paint);
                    cursor.SetPosition(cachedCursorX, cachedCursorY);
                    Mouse.ShowCursor = false;
                }
            }
        }

        UIScreen currentScreen;

        /// <summary>
        ///
        /// </summary>
        /// <param name="screen"></param>
        public void SetGameScreen(UIScreen screen)
        {
            if (currentScreen != null)
            {
                currentScreen.RemoveFromPainter(painter);
            }
            currentScreen = screen;
            if (currentScreen != null)
            {
                currentScreen.AddToPainter(painter);
            }
        }

        UIScreen screenToSwitchTo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="screen"></param>
        public void SwitchToScreen(UIScreen screen)
        {
            if (screen == null)
            {
                throw new ArgumentNullException("screen");
            }
            screen.Ready += SwitchReady;
            screenToSwitchTo = screen;
            screenToSwitchTo.Load();
            return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="screenType"></param>
        public void SwitchToScreen(UIScreenType screenType)
        {
            int index = (int)screenType;
            if (screens[index] == null)
            {
                switch (screenType)
                {
                    case UIScreenType.MainMenu:
                        screens[index] = new MainMenu(installedMpq);
                        break;
                    case UIScreenType.LogOn:
                        screens[index] = new LogOnScreen(playingMpq);
                        break;
                    case UIScreenType.Connection:
                        screens[index] = new ConnectionScreen(playingMpq);
                        break;
                    default:
                        throw new SCException();
                }
            }

            SwitchToScreen(screens[(int)screenType]);
        }

        /// <summary>
        ///
        /// </summary>
        public Mpq PlayingMpq
        {
            get { return playingMpq; }
        }

        /// <summary>
        ///
        /// </summary>
        public Mpq InstalledMpq
        {
            get { return installedMpq; }
        }

        void SwitchReady(object sender, EventArgs e)
        {
            screenToSwitchTo.Ready -= SwitchReady;
            SetGameScreen(screenToSwitchTo);
            screenToSwitchTo = null;
        }

        void GlobalResourcesLoaded(object sender, EventArgs e)
        {
            SwitchToScreen(UIScreenType.MainMenu);
        }

        void TitleScreenReady(object sender, EventArgs e)
        {
            Console.WriteLine("Loading global resources");
            GlobalResources.LoadMpq(stardatMpq, broodatMpq);
            GlobalResources.Ready += GlobalResourcesLoaded;
            GlobalResources.Load();
        }
    }
}
