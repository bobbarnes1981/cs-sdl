/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

namespace MfGames.Sdl.Gui
{
	public class Padding
	{
		private int [] padding = new int [] { 0, 0, 0, 0 };

		public Padding()
		{
		}

		public Padding(int each)
		{
			padding = new int [] { each, each, each, each };
		}

		public Padding(int sides, int height)
		{
			padding = new int [] { sides, height, sides, height };
		}

		public Padding(int left, int top, int right, int bottom)
		{
			padding = new int [] { left, top, right, bottom };
		}

		public int Horizontal
		{
			get { return Left + Right; }
		}

		public int Vertical
		{
			get { return Top + Bottom; }
		}

		public int Left
		{
			get { return padding[0]; }
			set { padding[0] = value; }
		}

		public int Right
		{
			get { return padding[2]; }
			set { padding[2] = value; }
		}

		public int Top
		{
			get { return padding[1]; }
			set { padding[1] = value; }
		}

		public int Bottom
		{
			get { return padding[3]; }
			set { padding[3] = value; }
		}
	}
}
