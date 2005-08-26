/* This file is part of SnowDemo
* (c) 2005 David Hudson
* Based on code by Sijmen Mulder
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

using SdlDotNet.Sprites;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Snowflake Sprite Class
	/// </summary>
	/// <remarks>
	/// A snowflakes is simply a 5x5 pixel white Surface. 
	/// It will start out at a random speed and sideways movement.
	/// </remarks>
	public class Snowflake : Sprite
	{
		static Random random = new Random();

		float speed;
		float wind;
		float delta = 0.05f;
		
		/// <summary>
		/// 
		/// </summary>
		public Snowflake() : base(new Surface(5, 5))
		{
			Initialize();
			Reset();
			this.Y = -1 * random.Next(5000 - this.Surface.Height);
		}

		void Initialize()
		{
			this.Surface.Fill(Color.White);
			this.Surface.SetColorKey(Color.FromArgb(255, 0, 255), true);
			this.Rectangle = new Rectangle(this.Surface.Width, this.Surface.Height, 0, 0);
		}

		void Reset()
		{
			wind = random.Next(3) / 10.0f;

			this.X = (int)random.Next(-1 * (int)(wind * 640), 640 - this.Surface.Width);
			this.Y = 0 - this.Width;

			speed = random.Next(50, 150);

			this.Surface.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded,
				(byte)((150 - 50) / (speed - 50) * -255));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Update(TickEventArgs args)
		{
			float change = delta * speed;

			this.Y += (int)change;
			this.X += (int)Math.Ceiling(change * wind);

			if (this.Y > 480)
			{
				Reset();
			}
		}
	}
}
