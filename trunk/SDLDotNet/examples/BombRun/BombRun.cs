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

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class BombRun
	{
		//int lastupdate = 0;	
		Surface screen;
		static float bombSpeed = 1;
		Surface background;
		Surface alternateBackground;
		Surface temporary;
		Player player;
		SpriteCollection bombs = new SpriteCollection();
		SpriteCollection players = new SpriteCollection();
		ArrayList bullets = new ArrayList();
		ArrayList mustdispose = new ArrayList(); // see below

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			screen = Video.SetVideoModeWindow(640, 480, true);
			Surface tempSurface = new Surface("../../Data/Background1.png");
			background = tempSurface.Convert();
			Surface tempSurface2 = new Surface("../../Data/Background2.png");
			alternateBackground = tempSurface.Convert();
			//alternateBackground.SetColorKey(Color.Magenta, true);
			//alternateBackground.TransparentColor = Color.Magenta;
			//alternateBackground.Transparent = true;

			temporary = screen.CreateCompatibleSurface(32, 32, true);
			temporary.TransparentColor = Color.Magenta;
			//FromArgb(0, 255, 0, 255);
			temporary.Transparent = true;

			player = new Player(new Point(screen.Width / 2 - 16,
				screen.Height - 32));
			players.Add(player);
			players.EnableTickEvent();
			players.EnableKeyboardEvent();

			for(int i = 0; i < 25; i++)
			{
				bombs.Add(new Bomb());
			}
			bombs.EnableTickEvent();

			Video.Mouse.ShowCursor = false;
			Video.WindowCaption =
				"SdlDotNet - Bomb Run";
			Events.KeyboardDown +=
				new KeyboardEventHandler(Keyboard);
			player.WeaponFired += new FireEventHandler(PlayerWeaponFired);

			Events.Tick += new TickEventHandler(this.OnTick);
			Events.Run();
		}

		[STAThread]
		static void Main()
		{
			BombRun bombRun = new BombRun();
			bombRun.Run();
		}

		private void PlayerWeaponFired(object sender, FireEventArgs e)
		{
			// create a new bullet
			Bullet bullet = new Bullet(e.Location, 0, -300);
			bullet.DisposeRequest += 
				new DisposeRequestEventHandler(BulletDisposeRequest);
			bullets.Add(bullet);
		}

		private void BulletDisposeRequest(object sender, EventArgs e)
		{
			mustdispose.Add(sender); // see Game.Run, the large comment
		}

		private void Keyboard(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape || e.Key == Key.Q)
			{
				Events.QuitApp();
			}
		}

		private void OnTick(object sender, TickEventArgs args)
		{
			screen.Blit(background, new Rectangle(new Point(0, 0),
				background.Size));
			//alternateBackground.Blit(players);
			screen.Blit(alternateBackground);
			//screen.Blit(temporary);
			screen.Blit(players);
			screen.Blit(bombs);

			//			foreach(Bullet o in bullets)
			//				screen.Blit(o.Surface, new
			//					Rectangle(o.Position,
			//					o.Surface.Size));
			//
			//			screen.Fill(new Rectangle(3, 3, (int)bombSpeed, 2), Color.White);

			//			// if lastupdate is 0 and the part below is done, one would get quite
			//			// a funny result. it is set later in this method
			//			if(lastupdate != 0)
			//			{
			//				float seconds = (float)(Timer.TicksElapsed - lastupdate) / 1000;
			//
			//				bombSpeed += seconds * 3;
			//
			//				// things can't be deleted from a collection in a foreach loop when
			//				// the foreach-ed collection and the target collection are the same.
			//				// that is why i put the must-be-deleted objects in a serperate
			//				// collection
			//				foreach(object o in mustdispose)
			//				{
			//					bullets.Remove(o);
			//				}
			//
			//				// which ofcourse must be emptied
			//				mustdispose = new ArrayList();
			//			}
			//
			//			lastupdate = Timer.TicksElapsed;
			screen.Flip();

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
