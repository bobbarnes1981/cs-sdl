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

	/// <summary>
	/// 
	/// </summary>
	public delegate void FireEventHandler(object sender, Point location);
	/// <summary>
	/// 
	/// </summary>
	public delegate void DisposeRequestEventHandler(object sender, EventArgs e);

	// used for the bullets
	/// <summary>
	/// 
	/// </summary>
	public struct Speed
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Speed(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		int x;
		int y;

		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}
	}

	// item fired by a weapon
	/// <summary>
	/// 
	/// </summary>
	public class WeaponParticle
	{
		Surface _Image;
		PointF _Location;
		Speed _Speed;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <param name="speed"></param>
		public WeaponParticle(Point location, Speed speed)
		{
			Game.Debug("Constructing WeaponParticle");

			_Location = location;
			_Speed = speed;

			// a white box for now
			_Image = Game.Screen.CreateCompatibleSurface(16, 32, true);
			_Image.Fill(new Rectangle(new Point(0,0), _Image.Size), Color.White);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			_Location.X += seconds * Speed.X;
			_Location.Y += seconds * Speed.Y;

			// check if the particle is outside the visible area of the game, it
			// should be disposed. this request is handled by the Game class
			if(DisposeRequest != null && (_Location.X + _Image.Size.Width < 0 ||
				_Location.X > Game.Screen.Width || _Location.Y + _Image.Size.Height <
				0 || _Location.Y > Game.Screen.Height))
			{
				Game.Debug("Requesting disposal of WeaponParticle");
				DisposeRequest(this, new EventArgs());
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public event DisposeRequestEventHandler DisposeRequest;

		/// <summary>
		/// 
		/// </summary>
		public Surface Image{ get{ return _Image; }}
		/// <summary>
		/// 
		/// </summary>
		public Speed Speed{ get{ return _Speed; }}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get{ return new Point((int)_Location.X, (int)_Location.Y); }
		}
	}

	/// <summary>
	/// 
	/// </summary>
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
		bool up;
		bool down;
		bool left;
		bool right;
		bool fire;

		// the tick of the last fire action
		int lastfire;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public Ship(Point location)
		{
			Game.Debug("Constructing Ship");

			_Location = location;

			// just a white box for now
			_Image = Game.Screen.CreateCompatibleSurface(32, 32, true);
			_Image.Fill(new Rectangle(new Point(0,0), _Image.Size), Color.White);

			Events.KeyboardDown += new KeyboardEventHandler(this.SdlKeyboard);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			// how far the ship should move this frame, calculated on basis of the
			// elapsed number of seconds :)
			float change;
			if (seconds <= (float.MaxValue /250))
			{
				change = seconds * 250;
			}
			else
			{
				change = float.MaxValue;
			}


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
					WeaponFired(this, Location);

				// dont forget this
				lastfire = Timer.Ticks;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SdlKeyboard(object sender, KeyboardEventArgs e)
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

		/// <summary>
		/// 
		/// </summary>
		public event FireEventHandler WeaponFired;

		/// <summary>
		/// 
		/// </summary>
		public Surface Image{ get{ return _Image; }}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get{ return new Point((int)_Location.X, (int)_Location.Y); }
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class Game
	{
		static Surface _Screen;
		Ship ship;
		ArrayList bullets = new ArrayList();
		ArrayList mustdispose = new ArrayList(); // see below
		bool quit;

		// messages for debugging purposes are sent to this method, therefore it
		// also is static
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		[Conditional("DEBUG")]
		public static void Debug(string message)
		{
			Console.WriteLine("> {0}", message);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			Game.Debug("Running");

#if DEBUG
			_Screen = Video.SetVideoModeWindow(640, 480, true);
#else
_Screen = Video.SetVideoMode(640, 480, 16);
#endif

			ship = new Ship(new Point(50, 235));
			ship.WeaponFired += new FireEventHandler(ShipWeaponFired);

			Video.Mouse.ShowCursor(false);
			Video.WindowCaption =
				"WeaponFire, (c) 2003 CL Game Studios";
			Events.KeyboardDown +=
				new KeyboardEventHandler(SdlKeyboard);
			Events.Quit += new QuitEventHandler(SdlQuit);

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		/// <param name="sender"></param>
		public void ShipWeaponFired(object sender, Point location)
		{
			Game.Debug("Fire in the hole!");

			// create a new bullet
			WeaponParticle bullet = new WeaponParticle(location, new Speed(300,0));
			bullet.DisposeRequest += new DisposeRequestEventHandler(
				BulletDisposeRequest);

			bullets.Add(bullet);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BulletDisposeRequest(object sender, EventArgs e)
		{
			Game.Debug("Disposing a bullet");
			mustdispose.Add(sender); // see Game.Run, the large comment
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SdlKeyboard(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape || e.Key == Key.Q)
				quit = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SdlQuit(object sender, QuitEventArgs e)
		{
			Game.Debug("Quit was requested");
			quit = true;
		}

		// used by the collision detection of some ingame objects
		/// <summary>
		/// 
		/// </summary>
		public static Surface Screen{ get{ return _Screen; }}
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class WeaponFire
	{
		WeaponFire()
		{
		}
		/// <summary>
		/// 
		/// </summary>
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
