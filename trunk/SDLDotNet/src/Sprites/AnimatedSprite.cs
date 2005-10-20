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
		/// Constructor
		/// </summary>
		public AnimatedSprite() : base()
		{
			m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(m_Timer_Elapsed);
			m_Timer.Interval = 20;
		}

		/// <summary>
		/// Create AnimatedSprite from Animation
		/// </summary>
		/// <param name="name">Name of animation</param>
		/// <param name="anim">animation</param>
		public AnimatedSprite(string name, AnimationCollection anim) : this()
		{
			m_Animations.Add(name, anim);
		}

		/// <summary>
		/// Create AnimatedSprite from Animation
		/// </summary>
		/// <param name="anim">animation</param>
		public AnimatedSprite(AnimationCollection anim) : this()
		{
			m_Animations.Add("Default", anim);
		}

		/// <summary>
		/// Create Animated Sprite from SurfaceCollection
		/// </summary>
		/// <param name="d">SurfaceCollection</param>
		/// <param name="coordinates">Initial Coordinates</param>
		public AnimatedSprite(SurfaceCollection d, Point coordinates) :this()	{
			m_Animations.Add("Default", new AnimationCollection(d));
			this.Surface = d[0];
			this.Rectangle = d[0].Rectangle;
			this.Position = coordinates;
		}

		/// <summary>
		/// Creates AnumatedSprite from Surface Collection
		/// </summary>
		/// <param name="d">SurfaceCollection</param>
		/// <param name="coordinates">Starting coordinates</param>
		/// <param name="z">initial Z position</param>
		public AnimatedSprite(SurfaceCollection d, Point coordinates, int z) : this()
		{
			m_Animations.Add("Default", new AnimationCollection(d));
			this.Surface = d[0];
			this.Rectangle = d[0].Rectangle;
			this.Position = coordinates;
			this.Z = z;
		}

        /// <summary>
        /// Creates AnimatedSprite from SurfaceCollection
        /// </summary>
        /// <param name="d">SurfaceCollection</param>
        public AnimatedSprite(SurfaceCollection d) : this()
        {
            m_Animations.Add("Default", new AnimationCollection(d));
            this.Surface = d[0];
            this.Rectangle = d[0].Rectangle;
            this.Position = new Point(0, 0);
            this.Z = 0;
        }
		#endregion Constructors

		#region Properties
 
		
		private AnimationDictionary m_Animations = new AnimationDictionary();

		/// <summary>
		/// The collection of animations for the animated sprite
		/// </summary>
		public AnimationDictionary Animations
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
                // Check to see if it exists.
				if (!m_Animations.Contains(value))
				{
					throw new SdlException("The given animation (" + value + ") does not exist in this AnimatedSprite AnimationCollection.");
				}

                // Set the animation settings.
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
		/// <returns>
		/// A surface representing the rendered animated sprite.
		/// </returns>
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

		/// <summary>
		/// Gets and sets the AnimateForward flag for all animations in this sprite's Animations.
		/// </summary>
		public bool AnimateForward
		{
			get
			{
				return m_Animations.AnimateForward;
			}
			set
			{
                m_Animations.AnimateForward = value;
			}
		}


		#endregion

		#region Private Methods
		private System.Timers.Timer m_Timer = new System.Timers.Timer(500);
		private void m_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			AnimationCollection current = m_Animations[m_CurrentAnimation];

            if (m_Frame >= current.Count && current.AnimateForward) // Going forwards and past last frame
			{
				if(current.Loop)
				{
					m_Frame = 0;
				}
			}
            else if (m_Frame <= 0 && !current.AnimateForward) // Going backwards and past first frame
			{
				if(current.Loop)
				{
					m_Frame = current.Count - 1;
				}
			}
            else // Still going
			{
                m_Frame = m_Frame + current.FrameIncrement;
			}
		}
		#endregion

        #region IDisposable Members
        // TODO: AnimatedSprite Disposable Members
        #endregion IDisposable Members

    }
}
