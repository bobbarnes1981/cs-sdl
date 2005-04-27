// Copyright 2005 David Hudson (jendave@yahoo.com)
// This file is part of SimpleGame.
//
// SimpleGame is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// SimpleGame is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SimpleGame; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections;
using System.Drawing;
using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for Sector.
	/// </summary>
	public class SectorSprite
	{
		Sector sector;
		Rectangle rect;
		//ArrayList arrayList;
		Surface sprite;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="sector"></param>
		public SectorSprite(Surface screen, Sector sector)
		{
			this.sector = sector;
			//this.arrayList = arrayList;
			this.sprite = screen.CreateCompatibleSurface(128, 128, true);
			this.sprite.Fill(Color.FromArgb(0, 255, 128));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="screen"></param>
		/// <param name="sector"></param>
		/// <param name="rect"></param>
		public SectorSprite(Surface screen, Sector sector, Rectangle rect)
		{
			this.sector = sector;
			//this.arrayList = arrayList;
			this.sprite = screen.CreateCompatibleSurface(128, 128, true);
			this.sprite.Fill(Color.FromArgb(0, 255, 128));
			this.rect = rect;
		}

		/// <summary>
		/// 
		/// </summary>
		public Sector Sector
		{
			get
			{
				return this.sector;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Surface Surface
		{
			get
			{
				return this.sprite;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Rect
		{
			get
			{ 
				return this.rect;
			}
			set
			{
				this.rect = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int CenterX
		{
			get
			{
				return ((this.rect.X) + (this.rect.Width)/2);
			}
			set
			{
				this.rect.X = (value - this.rect.Width/2);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int CenterY
		{
			get
			{
				return ((this.rect.Y) + (this.rect.Height)/2);
			}
			set
			{
				this.rect.Y = (value - this.rect.Height/2);
			}
		}
	}
}
