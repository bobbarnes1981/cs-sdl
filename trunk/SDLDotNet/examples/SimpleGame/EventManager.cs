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
using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for EventManager.
	/// </summary>
	public class EventManager
	{
		/// <summary>
		/// 
		/// </summary>
		public EventManager()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate void MapBuiltEventHandler(object sender, MapBuiltEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event MapBuiltEventHandler OnMapBuiltEvent;

		/// <summary>
		/// 
		/// </summary>
		public delegate void EntityMoveEventHandler(object sender, EntityMoveEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event EntityMoveEventHandler OnEntityMoveEvent;

		/// <summary>
		/// 
		/// </summary>
		public delegate void EntityMoveRequestEventHandler(object sender, EntityMoveRequestEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event EntityMoveRequestEventHandler OnEntityMoveRequestEvent;

		/// <summary>
		/// 
		/// </summary>
		public delegate void EntityPlaceEventHandler(object sender, EntityPlaceEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event EntityPlaceEventHandler OnEntityPlaceEvent;

		/// <summary>
		/// 
		/// </summary>
		public delegate void GameStatusEventHandler(object sender, GameStatusEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event GameStatusEventHandler OnGameStatusEvent;

		/// <summary>
		/// 
		/// </summary>
		public delegate void TickEventHandler(object sender, TickEventArgs e);
		/// <summary>
		/// 
		/// </summary>
		public event TickEventHandler OnTickEvent;

		/// <summary>
		/// 
		/// </summary>
		public event QuitEventHandler OnQuitEvent;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public void Publish(Object obj)
		{
			if (obj is GameStatusEventArgs)
			{
				if (OnGameStatusEvent != null) 
				{
					LogFile.WriteLine("EventManager has received GameStatus event");
					OnGameStatusEvent(this, (GameStatusEventArgs) obj);
				}
			}
			else if (obj is SdlDotNet.QuitEventArgs)
			{
				if (OnQuitEvent != null) 
				{
					LogFile.WriteLine("EventManager has received Quit event");
					OnQuitEvent(this, (SdlDotNet.QuitEventArgs) obj);
				}
			}
			else if (obj is EntityMoveRequestEventArgs)
			{
				if (OnEntityMoveRequestEvent != null) 
				{
					LogFile.WriteLine("EventManager has received EntityMoveRequest event");
					OnEntityMoveRequestEvent(this, (EntityMoveRequestEventArgs)obj);
				}
			}
			else if (obj is MapBuiltEventArgs)
			{
				if (OnMapBuiltEvent != null) 
				{
					LogFile.WriteLine("EventManager has received a MapBuilt event");
					OnMapBuiltEvent(this, (MapBuiltEventArgs) obj);
				}
			}
			else if (obj is TickEventArgs)
			{
				if (OnTickEvent != null) 
				{
					//LogFile.Instance.WriteLine("EventManager has received a Tick event");
					OnTickEvent(this, (TickEventArgs) obj);
				}
			}		
			else if (obj is EntityMoveEventArgs)
			{
				if (OnEntityMoveEvent != null) 
				{
					LogFile.WriteLine("EventManager has received an EntityMove event");
					OnEntityMoveEvent(this, (EntityMoveEventArgs) obj);
				}
			}
			else if (obj is EntityPlaceEventArgs)
			{	
				if (OnEntityPlaceEvent != null) 
				{
					LogFile.WriteLine("EventManager has received an EntityPlace event");
					OnEntityPlaceEvent(this, (EntityPlaceEventArgs) obj);
				}
			}
		}

	}
}


