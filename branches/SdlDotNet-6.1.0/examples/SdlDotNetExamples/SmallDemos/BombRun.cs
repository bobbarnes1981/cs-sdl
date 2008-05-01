#region LICENSE
/* This file is part of BombRun
 * (c) 2003 Sijmen Mulder
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

using System;
using System.IO;
using System.Drawing;
using System.Collections;

using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// 
    /// </summary>
    public static class BombRun
    {
        static Surface screen;
        static float bombSpeed = 100;
        static Surface background;
        static Surface alternateBackground;
        static Surface temporary;
        static Player player;
        static Surface tempSurface;
        static SpriteCollection bombs = new SpriteCollection();
        static SpriteCollection players = new SpriteCollection();
        static SpriteCollection bullets = new SpriteCollection();
        static SpriteCollection master = new SpriteCollection();
        static string dataDirectory = "Data";
        static string filePath = Path.Combine("..", "..");

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "BombRun: Sprite animation";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        public static void Run()
        {
            if (File.Exists(Path.Combine(dataDirectory, "Background1.png")))
            {
                filePath = "";
            }
            Video.WindowIcon();
            Video.WindowCaption =
                "SDL.NET - Bomb Run";
            screen = Video.SetVideoMode(640, 480);
            tempSurface = new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "Background1.png")));
            background = tempSurface.Convert();
            tempSurface = new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "Background2.png")));
            alternateBackground = tempSurface.Convert();

            temporary = screen.CreateCompatibleSurface(32, 32);
            temporary.TransparentColor = Color.FromArgb(0, 255, 0, 255);
            temporary.Transparent = true;

            player = new Player(new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "Head.bmp"))), new Point(screen.Width / 2 - 16,
                screen.Height - 32));
            players.Add(player);
            players.EnableKeyboardEvent();
            bullets.EnableTickEvent();
            master.EnableTickEvent();

            for (int i = 0; i < 25; i++)
            {
                bombs.Add(new Bomb(new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "Bomb.bmp")))));
            }
            foreach (Sprite bomb in bombs)
            {
                master.Add(bomb);
            }
            foreach (Sprite playerSprite in players)
            {
                master.Add(playerSprite);
            }

            Mouse.ShowCursor = false;
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(Keyboard);
            Events.Quit += new EventHandler<QuitEventArgs>(Quit);
            player.WeaponFired += new EventHandler<FireEventArgs>(PlayerWeaponFired);

            Events.Tick += new EventHandler<TickEventArgs>(OnTick);
            Events.Run();
        }

        private static void PlayerWeaponFired(object sender, FireEventArgs e)
        {
            Bullet bullet = new Bullet(e.Location, 0, 250);
            bullets.Add(bullet);

        }

        private static void Keyboard(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private static void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        static Rectangle src;
        static Rectangle dest;

        private static void OnTick(object sender, TickEventArgs args)
        {
            //Console.WriteLine(args.SecondsElapsed);
            screen.Blit(background);

            foreach (Sprite s in master)
            {
                src = new Rectangle(new Point(0, 0), s.Size);
                dest = new Rectangle(s.Position, s.Size);

                temporary.Blit(alternateBackground, src, dest);
                temporary.Blit(s.Surface, src);
                screen.Blit(temporary, dest);
            }

            screen.Blit(bullets);
            screen.Update();
            //screen.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        public static float BombSpeed
        {
            get
            {
                return bombSpeed;
            }
            set
            {
                bombSpeed = value;
            }
        }
    }
}
