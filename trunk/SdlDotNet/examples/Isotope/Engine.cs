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
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples.Isotope
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
	public class Engine
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

		public int time_limit;
		public Keys keys;
		public Simulator simulator;
		public LeadActor player;
		public Sprite title_sprite;
		public View display;
		public Skin[] skin_group;
		public Surface surface;
		public SdlDotNet.Font font;
       
		/// <summary>
		/// 
		/// </summary>
		/// <param name="player"></param>
		/// <param name="skin_group"></param>
		/// <param name="surface"></param>
		/// <param name="keys"></param>
		/// <param name="titlefile"></param>
		public Engine(LeadActor player, Skin[] skin_group,Surface surface,Keys keys,string titlefile/*,SdlDotNet.Font font*/)
		{
			/* Initialise the Isotope Engine */

			//Game Control
			//the lower limit of msec per frame
			this.time_limit=100;
			//define the keys using default values, users can redefine the keys by changing the key codes
			this.keys=keys;
	 
			//Physical simulation elements
			this.simulator=new Simulator();
			//Pick the players actor object and remember it
			this.player=player;
	 
			//Graphical display elements
			//load the titlebar graphic as a sprite for drawing later. Users can reload their own image.
			//if (titlefile==null)
			//   titlefile=os.path.join(os.path.dirname(__file__),"titlebar.png");

			this.title_sprite=new Sprite();
			this.title_sprite.Surface=new Surface(titlefile);
			//this.title_sprite.rect=this.title_sprite.image.get_rect();

			//Rectangle rect=Surface.Rectangle;
			int[] offset={200,170};
			this.display=new View(surface,this.player.scene,skin_group,offset);
			//Isometric display elements
			this.skin_group=skin_group;
			//remember the surface
			this.surface=surface;

			//Load the default font: Do we need some tester here to ensure we find a font?
			//if (font==None)
			this.font = new SdlDotNet.Font(Path.Combine(MainClass.FilePath, "FreeSans.ttf"), 10);
			//this.font = font;
		}
	   
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
			int quit=0;
			int start_time,end_time,frame_time;

			while(quit==0)
			{
				// Record the start time of the loop for the frame time control
				start_time=Timer.TicksElapsed;
	         
				// Check the players control events
				//Console.WriteLine("PlayerControl");
				quit = this.PlayerControl(this.player.scene.ObjectGroup,this.skin_group,this.surface,this.player);
				// Note: It is very usingant that objects modify their positions or the object lists in their
				// tick routines. Modifying these values in event receiver routines will mean that often a necessary collision
				// detection has not occurred
				// Update the movement of the objects in the players scene
				//Console.WriteLine("Simulator update");
				this.simulator.Update(this.player.scene);
				//Console.WriteLine("Display Draw");
				//Console.WriteLine(this.skin_group);
				//Console.WriteLine(this.player.new_scene);
				// Update the isometric display
				if (this.player.new_scene==this.player.scene)
				{
					this.display.DisplayUpdate(this.player.scene,this.skin_group);
				}
				else
				{
					this.display.RedrawDisplay(this.player.new_scene,this.skin_group);
				}
					// Update the information panel
				this.DrawInfoPanel(this.surface,this.player,this.skin_group);
				// Time limiting each frame and updating the game time clock
				end_time=Timer.TicksElapsed;
				frame_time=end_time-start_time;
				//Console.WriteLine(frame_time);
                if (frame_time < this.time_limit)
                {
                    Timer.DelayTicks(this.time_limit - frame_time);
                }
				//gametime.update_time();
				//Console.WriteLine("End of Loop");
	         
			}
			return(quit);
		}
		//end of game_loop function


		public int PlayerControl(ArrayList objectGroup, Skin[] skin_group,Surface surface,LeadActor player)
		{
			/* Checks for key presses and quit events from the player

				objectGroup: The group of objects in the players scene: list of Object3d class or subclass
				player: The lead actor being used for the player: lead_actor class
				skin_group: The group of skins to be used in the engines isometric view: skin class
				surface: The area of the surface to draw into from the pygame window: surface class

				kquit: returns 1 if quit event occurs: integer */

			//Check movement keys based on direct access to the keyboard state
			//keys=pygame.key.get_pressed();
			//Checks for the direction keys: up down left right
			int[] W={-2,0,0};
			int[] E={2,0,0};
			int[] N={0,2,0};
			int[] S={0,-2,0};
			Events.Poll();
            if (Keyboard.IsKeyPressed(this.keys.up) == true || Keyboard.IsKeyPressed(this.keys.down) == true ||
                Keyboard.IsKeyPressed(this.keys.left) == true || Keyboard.IsKeyPressed(keys.right) == true)
            {
                if (Keyboard.IsKeyPressed(this.keys.up) == true)
                {
                    player.Move(W);
                }
                if (Keyboard.IsKeyPressed(this.keys.down) == true)
                {
                    player.Move(E);
                }
                if (Keyboard.IsKeyPressed(this.keys.left) == true)
                {
                    player.Move(N);
                }
                if (Keyboard.IsKeyPressed(this.keys.right) == true)
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
            if (Keyboard.IsKeyPressed(this.keys.jump) == true)
            {
                player.Jump();
            }
            if (Keyboard.IsKeyPressed(this.keys.pick_up) == true)
            {
                player.EventPickUp();
            }
            if (Keyboard.IsKeyPressed(this.keys.drop) == true)
            {
                player.EventDrop();
            }
            if (Keyboard.IsKeyPressed(this.keys.usingk) == true)
            {
                player.EventUsingOb();
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


			//Check other keys and window close/QUIT based on the event queue system
			/*for (event in pygame.event.get()){
			   //Check for a quit program action caused by the window close and Control-C keypress
			   if (event.type is pygame.QUIT)
				   kquit=1;
			   //Check for a quit game action
			   else if (event.type is pygame.KEYDOWN and event.key == pygame.K_ESCAPE)
				   kquit=2;
			   //Check for the examine key
			   else if (event.type is pygame.KEYDOWN and event.key == this.keys.examine) {
				   //If the player is carrying an object then show its examine images
				   if (player.inventory.Length > 0 and isinstance(skin_group[player.inventory[player.usingk].objtype]
					 ,examinable))
					  this.examine(objectGroup,skin_group,player,surface);
			   }
			   //Check for pick up key
			   else if (event.type is pygame.KEYDOWN and event.key == this.keys.pick_up)
				   player.event_pick_up();
			   //Check for the drop key
			   else if (event.type is pygame.KEYDOWN and event.key == this.keys.drop)
				   player.event_drop();
			   //Check for the using key
			   else if (event.type is pygame.KEYDOWN and event.key == this.keys.usingk)
				   player.event_using();
			}*/
			return kquit;
		}

		/*public examine(this,objectGroup,skin_group,player,surface){*/
		/* Displays the examine images for the object that the player is using.

			  objectGroup: The group of objects in the players scene: list of Object3d class or subclass
			  player: The lead actor being used for the player: lead_actor class
			  skin_group: The group of skins to be used in the engines isometric view: skin class
			  surface: The area of the surface to draw into from the pygame window: surface class

			  Note: examine freezes the game and returns control when the player presses
			  the examine key again
		  */
		/* //get the object number that we are using
		  using=player.using;
		  image=0;
		  quit=False;
		  //Display the examine images in sequence every keypress. If the player presses the examine key then exit
		  while (not quit){
			 //Display the image
			 examine_image = skin_group[player.inventory[using].objtype].examine_image[image];
			 examine_sprite=pygame.sprite.Sprite();
			 examine_sprite.image=examine_image;
			 examine_sprite.rect=examine_sprite.image.get_rect();
			 examine_sprite.rect.top=surface.get_rect().height/2-examine_sprite.rect.height/2;
			 examine_sprite.rect.left=surface.get_rect().width/2-examine_sprite.rect.width/2;
			 surface.blit(examine_sprite.image,examine_sprite.rect);
			 pygame.display.flip();
			 //check for a keypress
			 done=False;     
			 while (not done){
				for (event in pygame.event.get()){
				   if (event.type == pygame.KEYDOWN ){
					  if (event.key == this.keys.examine)
						 quit = True;
					  done = True;
			 image=(image+1)%len(skin_group[player.inventory[using].objtype].examine_image);
		  //Clean up the surface and return to the main game
		  //Display the background
		  this.display.redraw_display(player.scene,skin_group);
		  //Display the Information panel
		  this.draw_info_panel(surface,player,skin_group);
		  pygame.display.flip();
	   }
	   */
		public void DrawInfoPanel(Surface surface,LeadActor player, Skin[] skin_group)
		{
			/* Draws the information panel on the surface.

				surface: The area of the surface to draw into from the pygame window: surface class
				player: The lead actor being used for the player: lead_actor class
				skin_group: The group of skins to be used in the engines isometric view: skin class
			*/
			//draw titlebar
			Rectangle rect=surface.Blit(this.title_sprite.Surface,this.title_sprite.Rectangle);
			int[] draw_order;
			//draw inventory
			Object3d[] inventory_array=new Object3d[player.inventory.Count];
            for (int i = 0; i < player.inventory.Count; i++)
            {
                inventory_array[i] = (Object3d)player.inventory[i];
            }
			if (player.inventory.Count>0)
			{
				Sprite[] sprite_group=Sprites.UpdateImages(skin_group,inventory_array);
				int p=155;
				draw_order=new int[player.inventory.Count];
				int q=0;
				for(int i=player.usingob;i<player.inventory.Count;i++)
				{
					draw_order[q]=i;
					q++;
				}
				for(int i=0;i<player.usingob;i++)
				{
					draw_order[q]=i;
					q++;
				}
				//Range(player.usingob,player.inventory.Count)+Range(player.usingob);
				foreach(int i in draw_order)
				{
					sprite_group[i].X=p;
					sprite_group[i].Y=38-sprite_group[i].Height;
					surface.Blit(sprite_group[i].Surface,sprite_group[i].Rectangle);
					Surface text = this.font.Render(skin_group[inventory_array[i].objtype].name, Color.FromArgb(255,255,255));
					Point textpos=new Point(0,0);
					textpos.X=p-skin_group[inventory_array[i].objtype].name.Length*3+sprite_group[i].Width/2;
					textpos.Y=35;
					surface.Blit(text,textpos);
					p=p+sprite_group[i].Width+20;
				}
			}
			//Update the display with the panel changes
			surface.Update(rect);
		}
	}
}
