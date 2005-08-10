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
using System.Drawing;
using System.Collections;
using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Derived class
	/// </summary>
	public class GameView
	{
		Map map;
		int height;
		int width;
		Surface surf;
		//Names names;
		EntitySprite entitySprite;
		Rectangle[] mapRectangles;
		ArrayList backSprites;
		ArrayList frontSprites;
		Sound sound;
		/// <summary>
		/// constructor
		/// </summary>
		public GameView (EventManager eventManager)
		{
			eventManager.OnMapBuiltEvent += new EventManager.MapBuiltEventHandler(Subscribe);
			eventManager.OnEntityPlaceEvent += new EventManager.EntityPlaceEventHandler(Subscribe);
			eventManager.OnEntityMoveEvent += new EventManager.EntityMoveEventHandler(Subscribe);
			eventManager.OnTickEvent += new EventManager.TickEventHandler(Subscribe);
			this.width = 424;
			this.height = 440;
			//this.names = Names.Instance;
			this.backSprites = new ArrayList();
			this.frontSprites = new ArrayList();
			try
			{
				this.sound = Mixer.Sound("../../boing.wav");
			}
			catch (SdlException)
			{
				// Linux audio problem
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void ShowMap(Map map)
		{
			mapRectangles = new Rectangle[9];
			int x = 10;
			int y = 10;
			int width = 128;
			int height = 128;
			
			int i = 0;
			//for (int i=0; i < 9; i++)
			foreach (Sector sec in map.GetSectors())
			{
				if (i < 3)
				{
					mapRectangles[i] = new Rectangle(x, y, width ,height);
					LogFile.WriteLine(mapRectangles[i].ToString());
					x+=138;
				} 
				else if (i >= 3 && i < 6)
				{
					if (i == 3)
					{
						x = 10;
					}
					y = 148;					
				
					mapRectangles[i] = new Rectangle(x, y, width ,height);
					LogFile.WriteLine(mapRectangles[i].ToString());
					x+=138;
				} 
				else if (i >= 6)
				{
					if (i == 6)
					{
						x = 10;
					}
					y = 286;					
			
					mapRectangles[i] = new Rectangle(x, y, width ,height);
					LogFile.WriteLine(mapRectangles[i].ToString());
					x+=138;
				}	
				backSprites.Add(new SectorSprite(Video.Screen, sec, mapRectangles[i]));
				i++;
			}					
		}

		/// <summary>
		/// 
		/// </summary>
		public void CreateView()
		{
			Video.SetVideoModeWindow(this.width, this.height, true);
			this.surf = Video.Screen.CreateCompatibleSurface(width, height, true);
			//fill the surface with black
			this.surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);
			Video.WindowCaption = Names.WindowCaption;
			Video.Mouse.ShowCursor = false;
		}

		/// <summary>
		/// 
		/// </summary>
		public void UpdateView()
		{
			this.surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);
			foreach (SectorSprite i in this.backSprites)
			{
				Video.Screen.Blit(i.Surface, i.Rect);
			}
			foreach (EntitySprite j in this.frontSprites)
			{
				Video.Screen.Blit(j.Surface, j.Rect);
			}
			Video.Screen.Flip();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		public void ShowEntity(Entity entity)
		{
			this.entitySprite = new EntitySprite(surf);
			this.frontSprites.Add(this.entitySprite);
			SectorSprite sectSprite = this.GetSectorSprite(entity.Sector);
			this.entitySprite.CenterX = sectSprite.CenterX;
			this.entitySprite.CenterY = sectSprite.CenterY;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		public void MoveEntity(Entity entity)
		{
			SectorSprite sectSprite = this.GetSectorSprite(entity.Sector);
			this.entitySprite.CenterX = sectSprite.CenterX;
			this.entitySprite.CenterY = sectSprite.CenterY;
			if (this.sound != null)
			{
				this.sound.Play();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventManager"></param>
		/// <param name="e"></param>
		void Subscribe(object eventManager, MapBuiltEventArgs e)
		{
			LogFile.WriteLine("GameView received a MapBuilt event");
			this.map = e.Map;
			ShowMap(this.map);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventManager"></param>
		/// <param name="e"></param>
		void Subscribe(object eventManager, EntityPlaceEventArgs e)
		{
			LogFile.WriteLine("GameView received a EntityPlace event");
			ShowEntity(e.Entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventManager"></param>
		/// <param name="e"></param>
		void Subscribe(object eventManager, EntityMoveEventArgs e)
		{
			LogFile.WriteLine("GameView received a EntityMove event");
			MoveEntity(e.Entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventManager"></param>
		/// <param name="e"></param>
		void Subscribe(object eventManager, TickEventArgs e)
		{
			//LogFile.WriteLine("GameView received a Tick event");
			UpdateView();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sector"></param>
		/// <returns></returns>
		public SectorSprite GetSectorSprite(Sector sector)
		{
			SectorSprite sectSprite = null;
			foreach (SectorSprite s in this.backSprites)
			{
				if (s.Sector == sector)
				{
					sectSprite = s;
				}
			}
			return sectSprite;
		}
	}
}
