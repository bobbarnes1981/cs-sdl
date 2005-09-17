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

using SdlDotNet.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// 
	/// </summary>
	public class AnimatedSprite : Sprite
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public AnimatedSprite() : base()
		{
			m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(m_Timer_Elapsed);
			m_Timer.Interval = 20;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="anim"></param>
		public AnimatedSprite(string name, Animation anim) : this()
		{
			m_Animations.Add(name, anim);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="anim"></param>
		public AnimatedSprite(Animation anim) : this()
		{
			m_Animations.Add("Default", anim);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="coordinates"></param>
		public AnimatedSprite(SurfaceCollection d, Point coordinates) :this()	{
			m_Animations.Add("Default", new Animation(d));
			this.Surface = d[0];
			this.Rectangle = d[0].Rectangle;
			this.Position = coordinates;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="coordinates"></param>
		/// <param name="z"></param>
		public AnimatedSprite(SurfaceCollection d, Point coordinates, int z) : this()
		{
			m_Animations.Add("Default", new Animation(d));
			this.Surface = d[0];
			this.Rectangle = d[0].Rectangle;
			this.Position = coordinates;
			this.Z = z;
		}
		#endregion Constructors


		#region Properties
 
		
		private AnimationCollection m_Animations = new AnimationCollection();

		/// <summary>
		/// The collection of animations for the animated sprite
		/// </summary>
		public AnimationCollection Animations
		{
			get
			{
				return m_Animations;
			}
			set
			{
				m_Animations = value;
			}
		}

		

		/// <summary>
		/// Gets and sets whether the animation is going.
		/// </summary>
		public bool Animate
		{
			get
			{
				return m_Timer.Enabled;
			}
			set
			{
				m_Timer.Enabled = value;
			}
		}

		private string m_CurrentAnimation = "Default";
		/// <summary>
		/// Gets and sets the current animation.
		/// </summary>
		public string CurrentAnimation
		{
			get
			{
				return m_CurrentAnimation; 
			}
			set
			{ 
				m_CurrentAnimation = value;
				m_Timer.Interval = m_Animations[m_CurrentAnimation].Delay;
			}
		}

		/// <summary>
		/// Gets the current animations surface
		/// </summary>
		public override Surface Surface
		{
			get
			{
				return this.m_Animations[m_CurrentAnimation][m_Frame];
			}
			set
			{
				base.Surface = value;
			}
		}
		
		/// <summary>
		/// Renders the surface
		/// </summary>
		/// <returns>A surface representing the rendered animated sprite.</returns>
		public override Surface Render()
		{
			return this.m_Animations[m_CurrentAnimation][m_Frame];
		}



		private int m_Frame = 0;
		/// <summary>
		/// Gets and sets the current frame in the animation.
		/// </summary>
		public int Frame
		{
			get{ return m_Frame; }
			set{ m_Frame = value; }
		}


		#endregion

		#region Private Methods
		private System.Timers.Timer m_Timer = new System.Timers.Timer(500);
		private void m_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Animation current = m_Animations[m_CurrentAnimation];
			if(m_Frame < current.Count)
			{
				m_Frame++;
			}
			else
			{
				if(current.Loop)
					m_Frame = 0;
			}
		}
		#endregion

		private bool disposed;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					if (disposing)
					{
						// Perform disposing
					}
				}
				finally
				{
					base.Dispose(disposing);
					this.disposed = true;
				}
			}
		}
	}
}
