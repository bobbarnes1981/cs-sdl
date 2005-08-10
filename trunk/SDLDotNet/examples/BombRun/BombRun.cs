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
using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics; // Conditional attribute
using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	class Bomb
	{
		static Surface _Image;
		PointF _Location;
		int speed;
		static float maxspeed = 250;
		static Random random = new Random();

		/// <summary>
		/// 
		/// </summary>
		public Bomb()
		{
			if(_Image == null)
			{
				Surface tempSurface = new Surface("../../Data/Bomb.bmp");
				_Image = tempSurface.Convert();
				_Image.SetColorKey(Color.White, true);
			}

			Reset();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Reset()
		{
			_Location = new PointF(random.Next(Game.Screen.Width - _Image.Width), 0 -
				_Image.Height - random.Next(Game.Screen.Height));

			speed = random.Next((int)Game.BombSpeed / 2, (int)Game.BombSpeed * 2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			_Location.Y += seconds * speed;

			if(Location.Y > Game.Screen.Height)
			{
				Reset();
			}

			if(Game.BombSpeed > maxspeed)
			{
				Game.BombSpeed = maxspeed / 2;
				maxspeed = maxspeed * 2;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static Surface Image
		{ 
			get
			{ 
				return _Image; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get
			{ 
				return new Point((int)_Location.X, (int)_Location.Y); 
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class Player
	{
		Surface _Image;
		PointF _Location;

		// these things are better kept as constants
		const Key LEFT = Key.LeftArrow;
		const Key RIGHT = Key.RightArrow;
		const Key JUMP = Key.UpArrow;
		//const Key FIRE = Key.Space;

		// weither the respective keys are pressed
		bool left;
		bool right;
		bool jump;
		bool fire;

		// the tick of the last fire action
		int lastfire;

		int jumpstart;
		bool falling;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public Player(Point location)
		{
			_Location = location;
			jumpstart = location.Y;

			Surface tempSurface = new Surface("../../Data/Head.bmp");
			_Image = tempSurface.Convert();
			_Image.SetColorKey(Color.White, true);

			Events.KeyboardDown += new KeyboardEventHandler(Keyboard);
			Events.KeyboardUp += new KeyboardEventHandler(Keyboard);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			// how far the player should move this frame, calculated on basis of the
			float change;
			// elapsed number of seconds :)
			if (seconds <= (float.MaxValue /250))
			{
				change = seconds * 250;
			}
			else
			{
				change = float.MaxValue;
			}
			//float change = seconds * 250;
			float jumpspeed = seconds * Game.BombSpeed * 2;

			if(jump || falling)
			{
				change = change / 2;
			}

			if(left)
			{
				_Location.X -= change;
			}
			if(right)
			{
				_Location.X += change;
			}

			if(jump)
			{
				if(_Location.Y < jumpstart - Game.Screen.Height / 3)
				{
					jump = false;
					falling = true;
				}
				else
				{
					_Location.Y -= jumpspeed * 1.5f;
				}
			}
			else if(falling)
			{
				_Location.Y += jumpspeed;

				if(_Location.Y > jumpstart)
				{
					_Location.Y = jumpstart;
					falling = false;
				}
			}

			// collision detection
			if(_Location.X < 0)
			{
				_Location.X = 0;
			}

			if(_Location.X + _Image.Size.Width > Game.Screen.Width)
			{
				_Location.X = Game.Screen.Width - _Image.Width;
			}

			// fire if needed. the 250 stands for the delay between two shots
			if(fire && lastfire + 250 < Timer.Ticks)
			{
				if(WeaponFired != null)
				{
					WeaponFired(this, new FireEventArgs(Location));
				}

				// dont forget this
				lastfire = Timer.Ticks;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Keyboard(object sender, KeyboardEventArgs e)
		{
			switch(e.Key)
			{
					// the =Down trick works quite well
				case LEFT: 
					left = e.Down; 
					break;

				case RIGHT: 
					right = e.Down; 
					break;

				case JUMP:
					if(e.Down && !falling)
					{
						jump = true;
					}
					else if(!e.Down)
					{
						jump = false;
						falling = true;
					}
					break;
				case Key.Space: 
					fire = e.Down; 
					break;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public event FireEventHandler WeaponFired;

		/// <summary>
		/// 
		/// </summary>
		public Surface Image
		{ 
			get
			{ 
				return _Image; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get
			{ 
				return new Point((int)_Location.X, (int)_Location.Y); 
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class Game
	{
		static Surface _Screen;
		static float _BombSpeed = 50;
		Surface _Background;
		Surface _AlternateBackground;
		Surface _Temporary;
		Player player;
		Bomb[] bombs;
		bool quit;
		ArrayList bullets = new ArrayList();
		ArrayList mustdispose = new ArrayList(); // see below

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			_Screen = Video.SetVideoModeWindow(640, 480, true);
			Surface tempSurface = new Surface("../../Data/Background1.png");
			_Background = tempSurface.Convert();
			tempSurface = new Surface("../../Data/Background2.png");
			_AlternateBackground = tempSurface.Convert();

			_Temporary = _Screen.CreateCompatibleSurface(32, 32, true);
			_Temporary.SetColorKey(Color.FromArgb(0, 255, 0, 255), true);

			player = new Player(new Point(_Screen.Width / 2 - 16,
				_Screen.Height - 32));
			bombs = new Bomb[25];

			for(int i = 0; i < bombs.Length; i++)
			{
				bombs[i] = new Bomb();
			}

			Video.Mouse.ShowCursor = false;
			Video.WindowCaption =
				"SdlDotNet - Bomb Run";
			Events.KeyboardDown +=
				new KeyboardEventHandler(Keyboard);
			Events.Quit += new QuitEventHandler(Quit);
			player.WeaponFired += new FireEventHandler(PlayerWeaponFired);

			int lastupdate = 0;
			while(!quit)
			{
				Events.Poll();

				_Screen.Blit(_Background, new Rectangle(new Point(0, 0),
					_Background.Size));

				Rectangle src = new Rectangle(new Point(0, 0), player.Image.Size);
				Rectangle dest = new Rectangle(player.Location, player.Image.Size);

				_Temporary.Blit(_AlternateBackground, dest, src);
				_Temporary.Blit(player.Image, src);
				_Screen.Blit(_Temporary, dest);

				// note that Bomb.Image is static
				for(int i = 0; i < bombs.Length; i++)
				{
					src = new Rectangle(new Point(0, 0), Bomb.Image.Size);
					dest = new Rectangle(bombs[i].Location, Bomb.Image.Size);

					_Temporary.Blit(_AlternateBackground, src, dest);
					_Temporary.Blit(Bomb.Image, src);
					_Screen.Blit(_Temporary, dest);
				}
				foreach(WeaponParticle o in bullets)
					_Screen.Blit(o.Image, new
						Rectangle(o.Location,
						o.Image.Size));

				Screen.Fill(new Rectangle(3, 3, (int)_BombSpeed, 2), Color.White);

				// if lastupdate is 0 and the part below is done, one would get quite
				// a funny result. it is set later in this method
				if(lastupdate != 0)
				{
					float seconds = (float)(Timer.Ticks - lastupdate) / 1000;

					player.Update(seconds);

					_BombSpeed += seconds * 3;

					for(int i = 0; i < bombs.Length; i++)
					{
						bombs[i].Update(seconds);
					}
					foreach(object o in bullets)
					{
						((WeaponParticle)o).Update(seconds);
					}

					// things can't be deleted from a collection in a foreach loop when
					// the foreach-ed collection and the target collection are the same.
					// that is why i put the must-be-deleted objects in a serperate
					// collection
					foreach(object o in mustdispose)
					{
						bullets.Remove(o);
					}

					// which ofcourse must be emptied
					mustdispose = new ArrayList();
				}

				lastupdate = Timer.Ticks;
				_Screen.Flip();
			}
		}

		private void PlayerWeaponFired(object sender, FireEventArgs e)
		{
			// create a new bullet
			WeaponParticle bullet = new WeaponParticle(e.Location, new Speed(0,-300));
			bullet.DisposeRequest += 
				new DisposeRequestEventHandler(BulletDisposeRequest);
			bullets.Add(bullet);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BulletDisposeRequest(object sender, EventArgs e)
		{
			mustdispose.Add(sender); // see Game.Run, the large comment
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Keyboard(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape || e.Key == Key.Q)
			{
				quit = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Quit(object sender, QuitEventArgs e)
		{
			quit = true;
		}

		// used by the collision detection of some ingame objects
		/// <summary>
		/// 
		/// </summary>
		public static Surface Screen
		{
			get
			{ 
				return _Screen; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static float BombSpeed
		{
			get
			{ 
				return _BombSpeed; 
			}
			set
			{
				_BombSpeed = value; 
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class BombRun
	{
		BombRun()
		{}

		/// <summary>
		/// 
		/// </summary>
		public static void Main()
		{
			(new Game()).Run();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public delegate void FireEventHandler(object sender, FireEventArgs e);
	/// <summary>
	/// 
	/// </summary>
	public delegate void DisposeRequestEventHandler(object sender, EventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public class FireEventArgs : EventArgs
	{
		Point location;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="location"></param>
		public FireEventArgs(Point location)
		{
			this.location = location;
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get
			{
				return location;
			}
		}
	}
	/// <summary>
	/// item fired by a weapon
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
			_Location = location;
			_Speed = speed;

			// a white box for now
			_Image = Game.Screen.CreateCompatibleSurface(8, 16, true);
			_Image.Fill(new Rectangle(new Point(0,0), _Image.Size), Color.DarkBlue);
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
		public Surface Image
		{ 
			get
			{ 
				return _Image; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Speed Speed
		{ 
			get
			{ 
				return _Speed; 
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point Location
		{
			get
			{ 
				return new Point((int)_Location.X, (int)_Location.Y); 
			}
		}
	}
	// used for the bullets
	/// <summary>
	/// 
	/// </summary>
	public class Speed
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
}
