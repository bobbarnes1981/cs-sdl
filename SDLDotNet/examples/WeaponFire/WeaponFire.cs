/*
* WeaponFire build 20032022
* Copyright (c) 2003 CL Game Studios (Sijmen Mulder)
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

using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using SdlDotNet;

namespace SdlDotNet.Examples
{

	public delegate void FireEventHandler(Point Location);
	public delegate void DisposeRequestEventHandler(object Asker);

	// used for the bullets
	public struct Speed
	{
		public Speed(int x, int y)
		{
			X = x;
			Y = y;
		}

		public int X;
		public int Y;
	}

	// item fired by a weapon
	public class WeaponParticle
	{
		Surface _Image;
		PointF _Location;
		Speed _Speed;

		public WeaponParticle(Point Location, Speed Speed)
		{
			Game.Debug("Constructing WeaponParticle");

			_Location = Location;
			_Speed = Speed;

			// a white box for now
			_Image = Game.Screen.CreateCompatibleSurface(16, 32, true);
			_Image.Fill(new Rectangle(new Point(0,0), _Image.Size), Color.White);
		}

		public void Update(float Seconds)
		{
			_Location.X += Seconds * Speed.X;
			_Location.Y += Seconds * Speed.Y;

			// check if the particle is outside the visible area of the game, it
			// should be disposed. this request is handled by the Game class
			if(DisposeRequest != null && (_Location.X + _Image.Size.Width < 0 ||
				_Location.X > Game.Screen.Width || _Location.Y + _Image.Size.Height <
				0 || _Location.Y > Game.Screen.Height))
			{
				Game.Debug("Requesting disposal of WeaponParticle");
				DisposeRequest(this);
			}
		}

		public event DisposeRequestEventHandler DisposeRequest;

		public Surface Image{ get{ return _Image; }}
		public Speed Speed{ get{ return _Speed; }}

		public Point Location
		{
			get{ return new Point((int)_Location.X, (int)_Location.Y); }
		}
	}

	public class Ship
	{
		Surface _Image;
		PointF _Location;

		// these things are better kept as constants
		const Key UP = Key.UpArrow;
		const Key DOWN = Key.DownArrow;
		const Key LEFT = Key.LeftArrow;
		const Key RIGHT = Key.RightArrow;
		const Key FIRE = Key.Space;

		// weither the respective keys are pressed
		bool up = false;
		bool down = false;
		bool left = false;
		bool right = false;
		bool fire = false;

		// the tick of the last fire action
		int lastfire = 0;

		public Ship(Point Location)
		{
			Game.Debug("Constructing Ship");

			_Location = Location;

			// just a white box for now
			_Image = Game.Screen.CreateCompatibleSurface(32, 32, true);
			_Image.Fill(new Rectangle(new Point(0,0), _Image.Size), Color.White);

			Events.Keyboard += new KeyboardEventHandler(this.SDL_Keyboard);
		}

		public void Update(float Seconds)
		{
			// how far the ship should move this frame, calculated on basis of the
			// elapsed number of seconds :)
			float change = Seconds * 250;

			if(up) _Location.Y -= change;
			if(down) _Location.Y += change;
			if(left) _Location.X -= change;
			if(right) _Location.X += change;

			// collision detection

			if(_Location.X < 0) _Location.X = 0;
			if(_Location.Y < 0) _Location.Y = 0;

			if(_Location.X + _Image.Size.Width > Game.Screen.Width)
				_Location.X = Game.Screen.Width - _Image.Width;

			if(_Location.Y + _Image.Size.Height > Game.Screen.Height)
				_Location.Y = Game.Screen.Height - _Image.Height;

			// fire if needed. the 250 stands for the delay between two shots
			if(fire && lastfire + 250 < Timer.Ticks)
			{
				if(WeaponFired != null)
					WeaponFired(Location);

				// dont forget this
				lastfire = Timer.Ticks;
			}
		}

		public void SDL_Keyboard(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
					// the =Down trick works quite well
				case Key.UpArrow: up = e.Down; break;
				case Key.DownArrow: down = e.Down; break;
				case Key.LeftArrow: left = e.Down; break;
				case Key.RightArrow: right = e.Down; break;
				case Key.Space: fire = e.Down; break;
			}
		}

		public event FireEventHandler WeaponFired;

		public Surface Image{ get{ return _Image; }}

		public Point Location
		{
			get{ return new Point((int)_Location.X, (int)_Location.Y); }
		}
	}

	public class Game
	{
		static Surface _Screen;
		Ship ship;
		ArrayList bullets = new ArrayList();
		ArrayList mustdispose = new ArrayList(); // see below
		bool quit = false;

		// messages for debugging purposes are sent to this method, therefore it
		// also is static
		[Conditional("DEBUG")]
		public static void Debug(string Message)
		{
			Console.WriteLine("> {0}", Message);
		}

		public void Run()
		{
			Game.Debug("Running");

#if DEBUG
			_Screen = Video.SetVideoModeWindow(640, 480, true);
#else
_Screen = Video.SetVideoMode(640, 480, 16);
#endif

			ship = new Ship(new Point(50, 235));
			ship.WeaponFired += new FireEventHandler(Ship_WeaponFired);

			Video.Mouse.ShowCursor(false);
			Video.WindowCaption =
				"WeaponFire, (c) 2003 CL Game Studios";
			Events.Keyboard +=
				new KeyboardEventHandler(SDL_Keyboard);
			Events.Quit += new QuitEventHandler(SDL_Quit);

			Game.Debug("Starting game loop");

			int lastupdate = 0;
			while(!quit)
			{
				Events.Poll();
				_Screen.Fill(new Rectangle(new Point(0, 0), _Screen.Size),
					Color.Black);

				_Screen.Blit(ship.Image,
					new Rectangle(ship.Location, ship.Image.Size));

				foreach(object o in bullets)
					_Screen.Blit(((WeaponParticle)o).Image, new
						Rectangle(((WeaponParticle)o).Location,
						((WeaponParticle)o).Image.Size));

				// if lastupdate is 0 and the part below is done, one would get quite
				// a funny result. it is set later in this method
				if(lastupdate != 0)
				{
					float seconds = (float)(Timer.Ticks - lastupdate) / 1000;
					ship.Update(seconds);

					foreach(object o in bullets)
						((WeaponParticle)o).Update(seconds);

					// things can't be deleted from a collection in a foreach loop when
					// the foreach-ed collection and the target collection are the same.
					// that is why i put the must-be-deleted objects in a serperate
					// collection
					foreach(object o in mustdispose)
						bullets.Remove(o);

					// which ofcourse must be emptied
					mustdispose = new ArrayList();
				}

				lastupdate = Timer.Ticks;
				_Screen.Flip();
			}
		}

		public void Ship_WeaponFired(Point Location)
		{
			Game.Debug("Fire in the hole!");

			// create a new bullet
			WeaponParticle bullet = new WeaponParticle(Location, new Speed(300,0));
			bullet.DisposeRequest += new DisposeRequestEventHandler(
				Bullet_DisposeRequest);

			bullets.Add(bullet);
		}

		public void Bullet_DisposeRequest(Object Asker)
		{
			Game.Debug("Disposing a bullet");
			mustdispose.Add(Asker); // see Game.Run, the large comment
		}

		public void SDL_Keyboard(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape || e.Key == Key.Q)
				quit = true;
		}

		public void SDL_Quit(object sender, QuitEventArgs e)
		{
			Game.Debug("Quit was requested");
			quit = true;
		}

		// used by the collision detection of some ingame objects
		public static Surface Screen{ get{ return _Screen; }}
	}

	public class WeaponFire
	{
		public static void Main()
		{
			Console.WriteLine("Bamboembam, (C) 2003 Sijmen Mulder");

			try
			{
				(new Game()).Run();
			}
#if DEBUG // whe dont want this to happen when there is no console
			catch(Exception e)
			{
				Console.WriteLine(e);
				Console.ReadLine();
			}
#endif
			finally
			{
				//SDL.Instance.Dispose();
				Console.WriteLine("Bye");
			}
		}
	}
}
