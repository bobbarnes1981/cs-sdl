#region LICENSE
/* 
 * (c) 2005 Simon Gillespie
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
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.Core;
using SdlDotNetExamples.LargeDemos;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// // Isotope: Isometric Object Oriented Pygame Engine
    // Isometric game engine module for pygame
    // Author: Simon Gillespie
    // License: GPL Copyright Oct 2005
    /*
    *                       
           I S O T O P E <>
                 <> <>
            <> <>
       
           The Isometric Engine for Python

     Isotope is an isometric game engine based on Pygame, and written in Python. It provides
     the framework for constructing an isometric graphics game with actors who can pick up objects,
     jump onto platforms. Automated actors who can interact with the player or their environment.

     Features:   
        <> Actors: used for player and monster game  Capable of facing, gravity,
             collision response, jumping, automation and inventory.
        <> Multiframe animation and images
        <> Simple physics simulation of velocity, collision and touch detection.
        <> All objects can be customised and extended using Python.
        <> Free commented open source code.

     Author: Simon Gillespie
     License: GPL Oct 2005

     ISOTOPE Modules:

        <> Control interfaces: Interfaces to control the Isotope system

        isotope:          A complete game engine with information panel, keyboard control and examine object surface mode.
        isotope_elements: A lower level interface to isotope which allows direct control of the isometric view and
                          the object simulator.

        <> Atomic classes: Class definitions for defining objects and how they appear in an isometric view

        actors:           Definitions of high level objects which can face, jump and carry 
        special_objects:  Complex Object definitions.
        objects:          Object definitions for basic objects and objects that can be carried or affected by gravity.
        scene:            Definitions to support scenes, groups of objects and scenetypes.
        skins:            Translators from object information into sprite images.

        <> Function Libraries: Low level routines to support the Isotope system

        isometric:        Function Library to draw isometric views.
        physics:          Function Library for 3d simulation of object physics.
        sprites:          Function Library to draw the sprites on the surface.
        vector:           Function Library for 3d vector mathematics.
    */
    /// </summary>
    public class Engine : IDisposable
    {
        /* Isometric game engine class
            Provides the basic elements required for an Isometric game:
              <> Isometric view: Draws the objects in an isometric projection using skin information
              <> Scene simulator: Simulates all the actions of the objects in the game
              <> Player keyboard events
              <> Information panel
              <> Examine surface

           player: The lead actor being used for the player: lead_actor class
           skin_group: The group of skins to be used in the engines isometric view: skin class
           surface: The area of the surface to draw into from the pygame window: surface class
           keys: the keyboard control set for the player to control the lead actor: key class
           titlefile: The filename of the titlebar image to be used for the information display: string
           font: The font object to be used for the information display text: font class

           title_sprite: The titlebar image: sprite class
           simulator: The isotope element used by the engine to simulate the players scene: simulator class
        */

        private int timeLimit;

        public int TimeLimit
        {
            get { return timeLimit; }
            set { timeLimit = value; }
        }
        private Keys keys;

        public Keys Keys
        {
            get { return keys; }
            set { keys = value; }
        }
        //private Simulator simulator;

        //public Simulator Simulator
        //{
        //    get { return simulator; }
        //    set { simulator = value; }
        //}
        private LeadActor player;

        public LeadActor Player
        {
            get { return player; }
            set { player = value; }
        }
        private Sprite titleSprite;

        public Sprite TitleSprite
        {
            get { return titleSprite; }
            set { titleSprite = value; }
        }
        private View display;

        public View Display
        {
            get { return display; }
            set { display = value; }
        }
        private Skin[] skinGroup;

        private Surface surface;

        public Surface Surface
        {
            get { return surface; }
            set { surface = value; }
        }
        private SdlDotNet.Graphics.Font font;

        public SdlDotNet.Graphics.Font Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="skin_group"></param>
        /// <param name="surface"></param>
        /// <param name="keys"></param>
        /// <param name="titlefile"></param>
        public Engine(LeadActor player, Skin[] skinGroup, Surface surface, Keys keys, string titleFile)
        {
            /* Initialise the Isotope Engine */
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            //Game Control
            //the lower limit of msec per frame
            this.timeLimit = 100;
            //define the keys using default values, users can redefine the keys by changing the key codes
            this.keys = keys;

            //Physical simulation elements
            //this.simulator = new Simulator();
            //Pick the players actor object and remember it
            this.player = player;

            //Graphical display elements
            //load the titlebar graphic as a sprite for drawing later. Users can reload their own image.
            this.titleSprite = new Sprite();
            this.titleSprite.Surface = new Surface(titleFile);

            int[] offset ={ 200, 170 };
            this.display = new View(surface, this.player.Scene, skinGroup, offset);
            //Isometric display elements
            this.skinGroup = skinGroup;
            //remember the surface
            this.surface = surface;

            //Load the default font: Do we need some tester here to ensure we find a font?
            //if (font==None)
            this.font = new SdlDotNet.Graphics.Font(Path.Combine(IsotopeMain.FilePath, "FreeSans.ttf"), 10);
            Events.Quit += new EventHandler<QuitEventArgs>(Events_Quit);
        }

        void Events_Quit(object sender, QuitEventArgs e)
        {
            quit = 1;
        }

        int quit;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Start()
        {
            /* Starts the Isotope Engine

                quit: returns 1 for window close or ctrl-c, and 2 for game quit : integer 
            */

            //draw_info_panel(this.surface,this.player,this.skin_group);
            surface.Update();

            // Main game loop controlled with quit

            int start_time, end_time, frame_time;

            while (quit == 0)
            {
                // Record the start time of the loop for the frame time control
                start_time = Timer.TicksElapsed;

                // Check the players control events
                //Console.WriteLine("PlayerControl");
                quit = this.PlayerControl(this.player);
                // Note: It is very usingant that objects modify their positions or the object lists in their
                // tick routines. Modifying these values in event receiver routines will mean that often a necessary collision
                // detection has not occurred
                // Update the movement of the objects in the players scene
                Simulator.Update(this.player.Scene);

                // Update the isometric display
                if (this.player.NewScene == this.player.Scene)
                {
                    this.display.DisplayUpdate(this.player.Scene, this.skinGroup);
                }
                else
                {
                    this.display.RedrawDisplay(this.player.NewScene, this.skinGroup);
                }
                // Update the information panel
                this.DrawInfoPanel(this.surface, this.player, this.skinGroup);
                // Time limiting each frame and updating the game time clock
                end_time = Timer.TicksElapsed;
                frame_time = end_time - start_time;
                //Console.WriteLine(frame_time);
                if (frame_time < this.timeLimit)
                {
                    Timer.DelayTicks(this.timeLimit - frame_time);
                }
                //gametime.update_time();
                //Console.WriteLine("End of Loop");

            }
            return (quit);
        }
        //end of game_loop function


        //public int PlayerControl(ArrayList objectGroup, Skin[] skinGroup, Surface surface, LeadActor player)
        public int PlayerControl(LeadActor player)
        {
            /* Checks for key presses and quit events from the player

                objectGroup: The group of objects in the players scene: list of Object3d class or subclass
                player: The lead actor being used for the player: lead_actor class
                skin_group: The group of skins to be used in the engines isometric view: skin class
                surface: The area of the surface to draw into from the pygame window: surface class

                kquit: returns 1 if quit event occurs: integer */
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            //Check movement keys based on direct access to the keyboard state
            //keys=pygame.key.get_pressed();
            //Checks for the direction keys: up down left right
            int[] W ={ -2, 0, 0 };
            int[] E ={ 2, 0, 0 };
            int[] N ={ 0, 2, 0 };
            int[] S ={ 0, -2, 0 };
            Events.Poll();
            if (Keyboard.IsKeyPressed(this.keys.Up) == true || Keyboard.IsKeyPressed(this.keys.Down) == true ||
                Keyboard.IsKeyPressed(this.keys.Left) == true || Keyboard.IsKeyPressed(keys.Right) == true)
            {
                if (Keyboard.IsKeyPressed(this.keys.Up) == true)
                {
                    player.Move(W);
                }
                if (Keyboard.IsKeyPressed(this.keys.Down) == true)
                {
                    player.Move(E);
                }
                if (Keyboard.IsKeyPressed(this.keys.Left) == true)
                {
                    player.Move(N);
                }
                if (Keyboard.IsKeyPressed(this.keys.Right) == true)
                {
                    player.Move(S);
                }
            }
            //if no direction key is pressed then stop the player
            else
            {
                player.Stop();
            }
            //Check for the Jump key
            if (Keyboard.IsKeyPressed(this.keys.Jump) == true)
            {
                player.Jump();
            }
            if (Keyboard.IsKeyPressed(this.keys.Pickup) == true)
            {
                player.EventPickup();
            }
            if (Keyboard.IsKeyPressed(this.keys.Drop) == true)
            {
                player.EventDrop();
            }
            if (Keyboard.IsKeyPressed(this.keys.UsingKey) == true)
            {
                player.EventUsingObject();
            }
            int kquit = 0;
            if (Keyboard.IsKeyPressed(Key.Q) == true)
            {
                kquit = 1;
            }
            if (Keyboard.IsKeyPressed(Key.Escape) == true)
            {
                kquit = 1;
            }
            return kquit;
        }

        public void DrawInfoPanel(Surface surface, LeadActor player, Skin[] skinGroup)
        {
            /* Draws the information panel on the surface.

                surface: The area of the surface to draw into from the pygame window: surface class
                player: The lead actor being used for the player: lead_actor class
                skin_group: The group of skins to be used in the engines isometric view: skin class
            */
            if (surface == null)
            {
                throw new ArgumentNullException("surface");
            }
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }
            //draw titlebar
            Rectangle rect = surface.Blit(this.titleSprite.Surface, this.titleSprite.Rectangle);
            int[] draw_order;
            //draw inventory
            Object3d[] inventory_array = new Object3d[player.Inventory.Count];
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                inventory_array[i] = (Object3d)player.Inventory[i];
            }
            if (player.Inventory.Count > 0)
            {
                Sprite[] sprite_group = Sprites.UpdateImages(skinGroup, inventory_array);
                int p = 155;
                draw_order = new int[player.Inventory.Count];
                int q = 0;
                for (int i = player.UsingObject; i < player.Inventory.Count; i++)
                {
                    draw_order[q] = i;
                    q++;
                }
                for (int i = 0; i < player.UsingObject; i++)
                {
                    draw_order[q] = i;
                    q++;
                }

                foreach (int i in draw_order)
                {
                    sprite_group[i].X = p;
                    sprite_group[i].Y = 38 - sprite_group[i].Height;
                    surface.Blit(sprite_group[i].Surface, sprite_group[i].Rectangle);
                    Surface text = this.font.Render(skinGroup[inventory_array[i].ObjectType].Name, Color.FromArgb(255, 255, 255));
                    Point textpos = new Point(0, 0);
                    textpos.X = p - skinGroup[inventory_array[i].ObjectType].Name.Length * 3 + sprite_group[i].Width / 2;
                    textpos.Y = 35;
                    surface.Blit(text, textpos);
                    p = p + sprite_group[i].Width + 20;
                }
            }
            //Update the display with the panel changes
            surface.Update(rect);
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.titleSprite != null)
                    {
                        this.titleSprite.Dispose();
                        this.titleSprite = null;
                    }
                    if (this.font != null)
                    {
                        this.font.Dispose();
                        this.font = null;
                    }
                }
                this.disposed = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        ~Engine()
        {
            Dispose(false);
        }

        #endregion
    }
}
