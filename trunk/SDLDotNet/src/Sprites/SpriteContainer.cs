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
using System;
using System.Collections;
using System.Drawing;
using System.Globalization;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// The SpriteContainer is a special case of sprite. It is used to
	/// group other sprites into an easily managed whole. The sprite
	/// manager has no size.
	/// </summary>
	public class SpriteContainer : Sprite
	{
		/// <summary>
		/// 
		/// </summary>
		public SpriteContainer()
		{
			IsTickable = true;
		}

		#region Display
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public override void Render(RenderArgs args)
		{
			// Check for our own X and Y
			RenderArgs args1 = args.Clone();
			args1.TranslateX += Coordinates.X;
			args1.TranslateY += Coordinates.Y;

			// Check for a size
			if (!size.IsEmpty)
			{
				// Set the size
				args1.Size = size;
				Rectangle clip = new Rectangle(args1.TranslateX,
					args1.TranslateY,
					size.Width,
					size.Height);

				// Set the clipping rectangle
				args1.Clipping = clip;
			}

			// Check for viewport
			if (viewport != null)
			{
				viewport.AdjustViewport(args1);
			}

			// Go through the sprites. We make a copy to make sure nothing
			// else changes the list while we are doing so.
			ArrayList list = new ArrayList(sprites);
			list.Sort();

			foreach (Sprite s in list)
			{
				s.Render(args1);
			}

			// Clear the clipping
			args1.ClearClipping();
		}
		#endregion

		#region Sprites
		private ArrayList sprites = new ArrayList();

		/// <summary>
		/// Adds sprite to container
		/// </summary>
		/// <param name="sprite">Sprite to add</param>
		public void Add(Sprite sprite)
		{
			if (!sprites.Contains(sprite))
			{
				sprites.Add(sprite);
			}
		}

		/// <summary>
		/// Removes sprite from container
		/// </summary>
		/// <param name="sprite">Sprite to remove</param>
		public void Remove(Sprite sprite)
		{
			sprites.Remove(sprite);
		}

		/// <summary>
		/// Clears all sprites out of the container
		/// </summary>
		public void Clear()
		{
			sprites.Clear();
		}

		/// <summary>
		/// Checks if sprite is in the container
		/// </summary>
		/// <param name="sprite">Sprite to query for</param>
		/// <returns>True is the sprite is in the container.</returns>
		public bool Contains(Sprite sprite)
		{
			return sprites.Contains(sprite);
		}

		/// <summary>
		/// Returns a copy of the arraylist
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return sprites.Clone();
		}

		/// <summary>
		/// Returns the number of sprites in the container.
		/// </summary>
		public int Count
		{
			get
			{
				return sprites.Count;
			}
		}

		/// <summary>
		/// Get and sets number of sprites that the container can contain.
		/// </summary>
		public int Capacity
		{
			get
			{
				return sprites.Capacity;
			}
			set
			{
				sprites.Capacity = value;
			}
		}



		#endregion

		#region Geometry
		private Size size;

		/// <summary>
		/// 
		/// </summary>
		public override Size Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Point point)
		{
			
			if (size.IsEmpty)
			{
				return true;
			}
			else
			{
				// We actually have a size
				return base.IntersectsWith(point);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public override bool IntersectsWith(Rectangle rect)
		{
			// We actually have a size
			return base.IntersectsWith(rect);
		}
		#endregion

		#region Events
		private Sprite eventLock = null;

		/// <summary>
		/// 
		/// </summary>
		public override bool IsKeyboardSensitive { get { return true; } }

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMouseSensitive 
		{ 
			get 
			{ 
				return true; 
			} 
		}

		/// <summary>
		/// 
		/// </summary>
		public Sprite EventLock
		{
			get 
			{ 
				return eventLock; 
			}
			set 
			{ 
				eventLock = value; 
			}
		}

		/// <summary>
		/// This causes the sprite manager to add itself to the SDL
		/// events. This enables it to handle the bulk of the processing
		/// for mouse and button events.
		/// </summary>
		public void EnableEvents()
		{
			Events.MouseButtonUp +=
				new MouseButtonEventHandler(OnMouseButtonUp);
			Events.MouseButtonDown +=
				new MouseButtonEventHandler(OnMouseButtonDown);
			Events.MouseMotion +=
				new MouseMotionEventHandler(OnMouseMotion);
			Events.KeyboardUp += new KeyboardEventHandler(OnKeyboard);
			Events.KeyboardDown += new KeyboardEventHandler(OnKeyboard);
			Events.TickEvent += new TickEventHandler(OnTick);
		}

		/// <summary>
		/// Processes the keyboard. If this function returns true, then
		/// the system no longer processes the keyboard event. If it is
		/// not true, then the next sprite that is keyboard sensitive is
		/// processed.
		/// </summary>
		public override void OnKeyboard(object sender, KeyboardEventArgs args)
		{
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. If this function returns true, then processing is
		/// stopped. If it returns false, then the next sprite is
		/// processed.
		/// </summary>
		public override void OnMouseButtonDown(object sender, MouseButtonEventArgs args)
		{
			// Check for an event lock
			if (EventLock != null)
			{
				EventLock.OnMouseButtonDown(this, args);
				if (EventLock.IsMouseButtonLocked == true)
				{
					return;
				}
			}

			// Go through the sprites. We make a copy to make sure nothing
			// else changes the list while we are doing so.
			ArrayList list = new ArrayList(sprites);
			Point point = new Point(args.X - Coordinates.X, args.Y - Coordinates.Y);
			list.Sort();

			foreach (Sprite s in list)
			{
				// Don't bother if not mouse sensitive
				if (!s.IsMouseSensitive)
				{
					continue;
				}

				// Check for bounds
				if (s.IntersectsWith(point))
				{
					s.OnMouseButtonDown(this, new MouseButtonEventArgs(args.Button, args.ButtonPressed, (short)(args.X - Coordinates.X), (short)(args.Y - Coordinates.Y)));
				}
			}
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. If this function returns true, then processing is
		/// stopped. If it returns false, then the next sprite is
		/// processed.
		/// </summary>
		public override void OnMouseButtonUp(object sender, MouseButtonEventArgs args)
		{
			// Check for an event lock
			if (EventLock != null)
			{
				EventLock.OnMouseButtonUp(this, args);
				return;
			}
			// Go through the sprites. We make a copy to make sure nothing
			// else changes the list while we are doing so.
			ArrayList list = new ArrayList(sprites);
			Point point = new Point(args.X - Coordinates.X, args.Y - Coordinates.Y);
			list.Sort();

			foreach (Sprite s in list)
			{
				// Don't bother if not mouse sensitive
				if (!s.IsMouseSensitive)
				{
					continue;
				}

				// Check for bounds
				if (s.IntersectsWith(point))
				{
					s.OnMouseButtonUp(this, new MouseButtonEventArgs(args.Button, args.ButtonPressed, (short)(args.X - Coordinates.X), (short)(args.Y - Coordinates.Y)));
				}
			}
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. If this returns true, then processing of the motion event
		/// is stopped, otherwise the next sprite is processed. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		public override void OnMouseMotion(object sender, MouseMotionEventArgs args)
		{	 
			// Check for an event lock
			if (EventLock != null)
			{
				EventLock.OnMouseMotion(this, args);
				if (EventLock.IsMouseMotionLocked == true)
				{
					return;
				}
			}

			// Go through the sprites. We make a copy to make sure nothing
			// else changes the list while we are doing so.
			ArrayList list = new ArrayList(sprites);
			Point point = new Point(args.X, args.Y);
			list.Sort();

			foreach (Sprite s in list)
			{
				// Don't bother if not mouse sensitive
				if (!s.IsMouseSensitive)
				{
					continue;
				}

				// Check for bounds
				if (s.IntersectsWith(point))
				{
					s.OnMouseMotion(this, new MouseMotionEventArgs(args.ButtonPressed,(short)(args.X - Coordinates.X), (short)(args.Y - Coordinates.Y), args.RelativeX, args.RelativeY));
				}
			}
		}

		/// <summary>
		/// Calls the OnTick for all sprites inside the container.
		/// </summary>
		public override void OnTick(object sender, TickEventArgs args)
		{
			// Go through the sprites
			foreach (Sprite s in new ArrayList(sprites))
			{
				if (s.IsTickable)
				{
					s.OnTick(this, args);
				}
			}
		}
		#endregion

		#region Operators
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format(CultureInfo.CurrentCulture, "(container {0}", sprites.Count);
		}
		#endregion

		#region Properties
		private IViewport viewport = null;

		/// <summary>
		/// 
		/// </summary>
		public IViewport Viewport
		{
			get { return viewport; }
			set { viewport = value; }
		}
		#endregion
	}
}
