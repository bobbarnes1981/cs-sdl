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
	public class Bomb
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
			Game.Debug("Constructing Bomb");

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
				Reset();

			if(Game.BombSpeed > maxspeed)
			{
				Game.BombSpeed = maxspeed / 2;
				maxspeed = maxspeed * 2;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static Surface Image{ get{ return _Image; }}

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
	public class Player
	{
		Surface _Image;
		PointF _Location;

		// these things are better kept as constants
		const Key LEFT = Key.LeftArrow;
		const Key RIGHT = Key.RightArrow;
		const Key JUMP = Key.UpArrow;

		// weither the respective keys are pressed
		bool left = false;
		bool right = false;
		bool jump = false;

		int jumpstart;
		bool falling = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Location"></param>
		public Player(Point Location)
		{
			Game.Debug("Constructing Player");

			_Location = Location;
			jumpstart = Location.Y;

			Surface tempSurface = new Surface("../../Data/Head.bmp");
			_Image = tempSurface.Convert();
			_Image.SetColorKey(Color.White, true);

			Events.KeyboardDown += new KeyboardEventHandler(Keyboard);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			// how far the player should move this frame, calculated on basis of the
			// elapsed number of seconds :)
			float change = seconds * 250;
			float jumpspeed = seconds * Game.BombSpeed * 2;

			if(jump || falling)
				change = change / 2;

			if(left) _Location.X -= change;
			if(right) _Location.X += change;

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

			if(_Location.X < 0) _Location.X = 0;

			if(_Location.X + _Image.Size.Width > Game.Screen.Width)
				_Location.X = Game.Screen.Width - _Image.Width;
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
				case LEFT: left = e.Down; break;
				case RIGHT: right = e.Down; break;

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
			}
		}

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
		static float _BombSpeed = 50;
		Surface _Background;
		Surface _AlternateBackground;
		Surface _Temporary;
		Player player;
		Bomb[] bombs;
		bool quit = false;

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
				bombs[i] = new Bomb();

			Video.Mouse.ShowCursor(false);
			Video.WindowCaption =
				"Bomb Run, (c) 2003 CL Game Studios (Sijmen Mulder)";
			Events.KeyboardDown +=
				new KeyboardEventHandler(Keyboard);
			Events.Quit += new QuitEventHandler(Quit);

			Game.Debug("Starting game loop");

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

				Screen.Fill(new Rectangle(3, 3, (int)_BombSpeed, 2), Color.White);

				// if lastupdate is 0 and the part below is done, one would get quite
				// a funny result. it is set later in this method
				if(lastupdate != 0)
				{
					float seconds = (float)(Timer.Ticks - lastupdate) / 1000;

					player.Update(seconds);

					_BombSpeed += seconds * 3;

					for(int i = 0; i < bombs.Length; i++)
						bombs[i].Update(seconds);
				}

				lastupdate = Timer.Ticks;
				_Screen.Flip();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Keyboard(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape || e.Key == Key.Q)
				quit = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Quit(object sender, QuitEventArgs e)
		{
			Game.Debug("Quit was requested");
			quit = true;
		}

		// used by the collision detection of some ingame objects
		/// <summary>
		/// 
		/// </summary>
		public static Surface Screen{ get{ return _Screen; }}

		/// <summary>
		/// 
		/// </summary>
		public static float BombSpeed
		{
			get{ return _BombSpeed; }
			set{ _BombSpeed = value; }
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class BombRun
	{
		/// <summary>
		/// 
		/// </summary>
		public static void Main()
		{
			Console.WriteLine("Bomb Run, (C) 2003 Sijmen Mulder");

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
//				try { SDL.Instance.Dispose(); } 
//				catch {} // = bugfix

				Console.WriteLine("Bye");
			}
		}
	}
}
