/* This file is part of SnowDemo
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

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Snowflake
	{
		Rectangle _Position;
		Surface _Image;
		static Random random = new Random();

		float x;
		float y;
		float speed;
		float wind;

		void reset()
		{
			wind = random.Next(3) / 10.0f;

			x = random.Next(-1 * (int)(wind * 640), 640 - _Image.Width);
			y = 0 - _Image.Width;

			speed = random.Next(50, 150);

			_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded,
				(byte)((150 - 50) / (speed - 50) * -255));
		}

		void updaterectangle()
		{
			_Position.X = (int)x;
			_Position.Y = (int)y;
		}

		/// <summary>
		/// 
		/// </summary>
		public Snowflake()
		{
			_Image = Graphics.LoadImage("../../Data/snowflake.bmp",
				Color.FromArgb(255, 0, 255));

			_Position = new Rectangle(_Image.Width, _Image.Height, 0, 0);

			reset();
			y = -1 * random.Next(5000 - _Image.Height);

			updaterectangle();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			float change = seconds * speed;

			y += change;
			x += change * wind;

			if(y > 480)
			{
				reset();
			}

			updaterectangle();
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Position
		{
			get
			{
				return _Position;
			}
		}
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
	}
}
