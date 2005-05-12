/*
 * $RCSfile$
 * Copyright (C) 2005 David Hudson (jendave@yahoo.com)
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
	/// The SpriteCollection is a special case of sprite. It is used to
	/// group other sprites into an easily managed whole. The sprite
	/// manager has no size.
	/// </summary>
	public class SpriteCollection : CollectionBase, ICollection
	{
		/// <summary>
		/// 
		/// </summary>
		public SpriteCollection() : base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public Sprite this[int index]
		{
			get
			{
				return ((Sprite)List[index]);
			}
			set
			{
				List[index] = value;
			}
		}

		#region Display
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public virtual void Draw(Surface surface)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Visible)
				{
					surface.Blit(this[i].Render(), this[i].Rectangle);
				}
			}
		}

//		/// <summary>
//		/// 
//		/// </summary>
//		/// <param name="surface"></param>
//		/// <param name="background"></param>
//		public virtual void ClearRects(Surface surface, Surface background)
//		{
//		}
		#endregion

		#region Sprites
		/// <summary>
		/// Adds sprite to group
		/// </summary>
		/// <param name="sprite">Sprite to add</param>
		public virtual int Add(Sprite sprite)
		{
			return (List.Add(sprite));
		}

		/// <summary>
		/// Adds sprites from another group to this group
		/// </summary>
		/// <param name="spriteCollection">SpriteCollection to add Sprites from</param>
		public virtual int Add(SpriteCollection spriteCollection)
		{
			for (int i = 0; i < spriteCollection.Count; i++)
			{
				this.List.Add(spriteCollection[i]);
			}
			return this.Count;
		}

		/// <summary>
		/// Removes sprite from group
		/// </summary>
		/// <param name="sprite">Sprite to remove</param>
		public virtual void Remove(Sprite sprite)
		{
			List.Remove(sprite);
		}

		/// <summary>
		/// Removes sprite from this group if they are contained in the given group
		/// </summary>
		/// <param name="spriteCollection"></param>
		public virtual void Remove(SpriteCollection spriteCollection)
		{
			for (int i = 0; i < spriteCollection.Count; i++)
			{
				if (this.Contains(spriteCollection[i]))
				{
					this.Remove(spriteCollection[i]);
				}
			}
		}

		/// <summary>
		/// Checks if sprite is in the container
		/// </summary>
		/// <param name="sprite">Sprite to query for</param>
		/// <returns>True is the sprite is in the container.</returns>
		public bool Contains(Sprite sprite)
		{
			return (List.Contains(sprite));
		}

		#endregion

		#region Geometry

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Size Size
		{
			get
			{
				if (this.Count > 0)
				{
					return this[0].Size;
				}
				else
				{
					return new Size(0, 0);
				}
			}
		}
		#endregion

 		#region Events
		/// <summary>
		/// 
		/// </summary>
		public void EnableActiveEvent()
		{
			Events.AppActive += new ActiveEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickAxisEvent()
		{
			Events.JoystickAxisMotion += new JoystickAxisEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickBallEvent()
		{
			Events.JoystickBallMotion += new JoystickBallEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickButtonEvent()
		{
			Events.JoystickButtonDown += new JoystickButtonEventHandler(Update);
			Events.JoystickButtonUp += new JoystickButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickButtonDownEvent()
		{
			Events.JoystickButtonDown += new JoystickButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickButtonUpEvent()
		{
			Events.JoystickButtonUp += new JoystickButtonEventHandler(Update);
		}		

		/// <summary>
		/// 
		/// </summary>
		public void EnableJoystickHatEvent()
		{
			Events.JoystickHatMotion += new JoystickHatEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableKeyboardEvent()
		{
			Events.KeyboardUp += new KeyboardEventHandler(Update);
			Events.KeyboardDown += new KeyboardEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableKeyboardDownEvent()
		{
			Events.KeyboardDown += new KeyboardEventHandler(Update);
		}
		/// <summary>
		/// 
		/// </summary>
		public void EnableKeyboardUpEvent()
		{
			Events.KeyboardUp += new KeyboardEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableMouseButtonEvent()
		{
			Events.MouseButtonDown += new MouseButtonEventHandler(Update);
			Events.MouseButtonUp += new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableMouseButtonDownEvent()
		{
			Events.MouseButtonDown += new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableMouseButtonUpEvent()
		{
			Events.MouseButtonUp += new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableMouseMotionEvent()
		{
			Events.MouseMotion += new MouseMotionEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableUserEvent()
		{
			Events.UserEvent += new UserEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableQuitEvent()
		{
			Events.Quit += new QuitEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableVideoExposeEvent()
		{
			Events.VideoExpose += new VideoExposeEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableVideoResizeEvent()
		{
			Events.VideoResize += new VideoResizeEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableChannelFinishedEvent()
		{
			Events.ChannelFinished += new ChannelFinishedEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableMusicFinishedEvent()
		{
			Events.MusicFinished += new MusicFinishedEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void EnableTickEvent()
		{
			Events.TickEvent += new TickEventHandler(Update);
		}


		/// <summary>
		/// 
		/// </summary>
		public void DisableActiveEvent()
		{
			Events.AppActive -= new ActiveEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickAxisEvent()
		{
			Events.JoystickAxisMotion -= new JoystickAxisEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickBallEvent()
		{
			Events.JoystickBallMotion -= new JoystickBallEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickButtonEvent()
		{
			Events.JoystickButtonDown -= new JoystickButtonEventHandler(Update);
			Events.JoystickButtonUp -= new JoystickButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickButtonDownEvent()
		{
			Events.JoystickButtonDown -= new JoystickButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickButtonUpEvent()
		{
			Events.JoystickButtonUp -= new JoystickButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableJoystickHatEvent()
		{
			Events.JoystickHatMotion -= new JoystickHatEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableKeyboardEvent()
		{
			Events.KeyboardUp -= new KeyboardEventHandler(Update);
			Events.KeyboardDown -= new KeyboardEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableKeyboardDownEvent()
		{
			Events.KeyboardDown -= new KeyboardEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableKeyboardUpEvent()
		{
			Events.KeyboardUp -= new KeyboardEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableMouseButtonEvent()
		{
			Events.MouseButtonDown -= new MouseButtonEventHandler(Update);
			Events.MouseButtonUp -= new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableMouseButtonDownEvent()
		{
			Events.MouseButtonDown -= new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableMouseButtonUpEvent()
		{
			Events.MouseButtonUp -= new MouseButtonEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableMouseMotionEvent()
		{
			Events.MouseMotion -= new MouseMotionEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableUserEvent()
		{
			Events.UserEvent -= new UserEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableQuitEvent()
		{
			Events.Quit -= new QuitEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableVideoExposeEvent()
		{
			Events.VideoExpose -= new VideoExposeEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableVideoResizeEvent()
		{
			Events.VideoResize -= new VideoResizeEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableChannelFinishedEvent()
		{
			Events.ChannelFinished -= new ChannelFinishedEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableMusicFinishedEvent()
		{
			Events.MusicFinished -= new MusicFinishedEventHandler(Update);
		}

		/// <summary>
		/// 
		/// </summary>
		public void DisableTickEvent()
		{
			Events.TickEvent -= new TickEventHandler(Update);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, ActiveEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// Processes a joystick motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		private void Update(object sender, JoystickAxisEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}
		
		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		private void Update(object sender, JoystickBallEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}
		
		/// <summary>
		/// Processes a joystick button event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		private void Update(object sender, JoystickButtonEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}
		
		/// <summary>
		/// Processes a joystick hat motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are JoystickSensitive are processed.
		/// </summary>
		private void Update(object sender, JoystickHatEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}
		
		/// <summary>
		/// Processes the keyboard.
		/// </summary>
		private void Update(object sender, KeyboardEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// Processes a mouse button. This event is trigger by the SDL
		/// system. 
		/// </summary>
		private void Update(object sender, MouseButtonEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// Processes a mouse motion event. This event is triggered by
		/// SDL. Only
		/// sprites that are MouseSensitive are processed.
		/// </summary>
		private void Update(object sender, MouseMotionEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, QuitEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, UserEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, VideoExposeEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, VideoResizeEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, ChannelFinishedEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Update(object sender, MusicFinishedEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		/// <summary>
		/// All sprites are tickable, regardless if they actual do
		/// anything. This ensures that the functionality is there, to be
		/// overridden as needed.
		/// </summary>
		private void Update(object sender, TickEventArgs e)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Update(e);
			}
		}

		#endregion

		#region Properties
//		private SpriteCollection lostSprites;
//
//		protected SpriteCollection LostSprites
//		{
//			get
//			{
//				return lostSprites;
//			}
//		}

		#endregion

		// Provide the explicit interface member for ICollection.
		void ICollection.CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public virtual void CopyTo(Sprite[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="sprite"></param>
		public virtual void Insert(int index, Sprite sprite)
		{
			List.Insert(index, sprite);
		} 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <returns></returns>
		public virtual int IndexOf(Sprite sprite)
		{
			return List.IndexOf(sprite);
		} 
	}
}
