// Copyright 2005 David Hudson (jendave@yahoo.com)
// This file is part of Iron Crown.
//
// Iron Crown is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// Iron Crown is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Iron Crown; if not, write to the Free Software
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
		public EventManager()
		{
		}

		public delegate void MapBuiltEventHandler(object sender, MapBuiltEventArgs e);
		public event MapBuiltEventHandler OnMapBuiltEvent;

		public delegate void EntityMoveEventHandler(object sender, EntityMoveEventArgs e);
		public event EntityMoveEventHandler OnEntityMoveEvent;

		public delegate void EntityMoveRequestEventHandler(object sender, EntityMoveRequestEventArgs e);
		public event EntityMoveRequestEventHandler OnEntityMoveRequestEvent;

		public delegate void EntityPlaceEventHandler(object sender, EntityPlaceEventArgs e);
		public event EntityPlaceEventHandler OnEntityPlaceEvent;

		public delegate void GameStatusEventHandler(object sender, GameStatusEventArgs e);
		public event GameStatusEventHandler OnGameStatusEvent;

		public delegate void TickEventHandler(object sender, TickEventArgs e);
		public event TickEventHandler OnTickEvent;

		public event QuitEventHandler OnQuitEvent;

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


