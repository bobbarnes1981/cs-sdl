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

using SdlDotNet;

using SdlDotNet.Sprites;
using System.Collections;
using System.Drawing;
using System;

namespace MfGames.Sdl.Demos
{
	/// <summary>
	/// An abstract page to encapsulates the common functionality of all
	/// demo pages.
	/// </summary>
	public abstract class DemoMode : ITickable
	{
		//private bool running = true;
		private static Hashtable marbles = new Hashtable();

		protected SpriteContainer sm = new SpriteContainer();
		Random rand = new Random();

		public DemoMode()
		{
		}

		#region Drawables
		/// <summary>
		/// Loads a floor title into memory.
		/// </summary>
		protected IDrawable LoadFloor(int number)
		{
			ImageDrawable id = new ImageDrawable("../../Data/floor" + number + ".png");
			return id;
		}

		/// <summary>
		/// Loads the marble series into memory and returns the
		/// collection.
		/// </summary>
		protected IDrawable LoadMarble(string name)
		{
			// We cache it to speed things up
			ImageCollectionDrawable icd =
				(ImageCollectionDrawable) marbles["icd:" + name];

			if (icd != null)
				return icd;

			// Load the marble and cache it before returning
			icd = new ImageCollectionDrawable("../../Data/" + name, ".png");
			marbles["icd:" + name] = icd;
			return icd;
		}

		protected IDrawable LoadRandomMarble()
		{
			return LoadMarble("marble" + (rand.Next() % 6 + 1));
		}

		/// <summary>
		/// Loads a marble from a single image and tiles it.
		/// </summary>
		protected IDrawable LoadTiledMarble(string name)
		{
			// Load the marble
			ImageDrawable id = new ImageDrawable("../../Data/" + name + ".png");
			TiledDrawable td = new TiledDrawable(id, new Size(64, 64), 6, 6);

			return td;
		}
		#endregion

		#region Mode Switching
		/// <summary>
		/// Indicates to the demo page that it should start displaying its
		/// data in the given sprite manager.
		/// </summary>
		public virtual void Start(SpriteContainer manager)
		{
			manager.Add(sm);
		}

		/// <summary>
		/// Indicates to the demo page that it should stop displaying its
		/// data in the given sprite manager.
		/// </summary>
		public virtual void Stop(SpriteContainer manager)
		{
			manager.Remove(sm);
		}
		#endregion

		#region Events
		public virtual bool IsTickable 
		{ 
			get 
			{ 
				return true; 
			} 
		}

		public virtual void OnTick(object sender, TickEventArgs args)
		{
		}

		public void EnableEvents()
		{
			Events.TickEvent += new TickEventHandler(OnTick);
		}
		#endregion
	}
}
