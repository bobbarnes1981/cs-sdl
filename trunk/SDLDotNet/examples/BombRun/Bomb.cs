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

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public class Bomb : Sprite
	{
		int speed;
		static float maxspeed = 250;
		static Random random = new Random();

		/// <summary>
		/// 
		/// </summary>
		public Bomb()
		{
			this.Surface = new Surface("../../Data/Bomb.bmp");
			this.Surface.TransparentColor = Color.White;
			this.Size = this.Surface.Size;
			Reset();
		}

		private void Reset()
		{
			this.Position = 
				new Point(random.Next(Video.Screen.Width - this.Surface.Width),
				0 - this.Surface.Height - random.Next(Video.Screen.Height));
			this.speed = 
				random.Next((int)BombRun.BombSpeed,
				(int)BombRun.BombSpeed * 2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
			this.Y += (int)(args.SecondsElapsed * speed);
			//Console.WriteLine(args.SecondsElapsed);

			if(this.Y > Video.Screen.Height)
			{
				Reset();
			}

			if(BombRun.BombSpeed > maxspeed)
			{
				BombRun.BombSpeed = maxspeed / 2;
				maxspeed = maxspeed * 2;
			}
		}
	}
}