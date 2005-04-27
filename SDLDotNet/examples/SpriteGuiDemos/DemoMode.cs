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

namespace SdlDotNet.Examples
{
	/// <summary>
	/// An abstract page to encapsulates the common functionality of all
	/// demo pages.
	/// </summary>
	public abstract class DemoMode : IDisposable
	{
		private static Hashtable marbles = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		private Surface surf = new Surface();

		/// <summary>
		/// 
		/// </summary>
		private SpriteCollection sprites = new SpriteCollection();
		Random rand = new Random();

		#region Drawables
		/// <summary>
		/// Loads a floor title into memory.
		/// </summary>
		protected SurfaceCollection LoadFloor()
		{
			SurfaceCollection id = new SurfaceCollection("../../Data/floor", ".png");
			return id;
		}

		/// <summary>
		/// Loads the marble series into memory and returns the
		/// collection.
		/// </summary>
		protected SurfaceCollection LoadMarble(string name)
		{
			// We cache it to speed things up
			SurfaceCollection icd =
				(SurfaceCollection) marbles["icd:" + name];

			if (icd != null)
			{
				return icd;
			}

			// Load the marble and cache it before returning
			icd = new SurfaceCollection("../../Data/" + name, ".png");
			marbles["icd:" + name] = icd;
			return icd;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected SurfaceCollection LoadRandomMarble()
		{
			return LoadMarble("marble" + (rand.Next() % 6 + 1));
		}

		/// <summary>
		/// Loads a marble from a single image and tiles it.
		/// </summary>
		protected SurfaceCollection LoadTiledMarble(string name)
		{
			// Load the marble
			Sprite id = 
				new Sprite(new Surface("../../Data/" + name + ".png"));
			SurfaceCollection surfCollection = new SurfaceCollection();
			surfCollection.Add(id.Surface);
			TiledSurfaceCollection td = 
				new TiledSurfaceCollection(new Surface("../../Data/" + name + ".png"), new Size(64, 64));

			return td;
		}
		#endregion

//
//		/// <summary>
//		/// 
//		/// </summary>
//		public void EnableTickEvent()
//		{
//			Events.TickEvent += new TickEventHandler(OnTick);
//		}


		#region Mode Switching
		/// <summary>
		/// Indicates to the demo page that it should start displaying its
		/// data in the given sprite manager.
		/// </summary>
		public virtual void Start(SpriteCollection manager)
		{
			manager.Add(Sprites);
		}

		/// <summary>
		/// Indicates to the demo page that it should stop displaying its
		/// data in the given sprite manager.
		/// </summary>
		public virtual void Stop(SpriteCollection manager)
		{
			manager.Remove(Sprites);
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Surface Surface
		{
			get
			{
				return surf;
			}
			set
			{
				surf = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual Surface RenderSurface()
		{	
			surf.Fill(Color.Black);
			foreach (Sprite s in Sprites)
			{
				surf.Blit(s.Render(), s.Rectangle);
			}
			return surf;
		}

		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection Sprites
		{
			get
			{
				return sprites;
			}
		}
		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			// TODO:  Add DemoMode.Dispose implementation
		}

		#endregion
	}
}
