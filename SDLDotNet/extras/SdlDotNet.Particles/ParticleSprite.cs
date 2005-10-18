/*
 * $RCSfile: Surface.cs,v $
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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

using System;

using SdlDotNet.Sprites;


namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle represented by a Sprite.
	/// </summary>
	public class ParticleSprite : Particle
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		public ParticleSprite(Sprite sprite)
		{
			m_Sprite = sprite;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="velocity"></param>
		public ParticleSprite(Sprite sprite, Vector velocity)
		{
			this.Velocity = velocity;
			m_Sprite = sprite;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ParticleSprite(Sprite sprite, float x, float y)
		{
			this.X = x;
			this.Y = y;
			m_Sprite = sprite;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		public ParticleSprite(Sprite sprite, float x, float y, Vector velocity)
		{
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			m_Sprite = sprite;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="velocity"></param>
		/// <param name="life"></param>
		public ParticleSprite(Sprite sprite, Vector velocity, int life)
		{
			this.Velocity = velocity;
			m_Sprite = sprite;
			this.Life = life;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="life"></param>
		public ParticleSprite(Sprite sprite, int life)
		{
			m_Sprite = sprite;
			this.Life = life;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		/// <param name="life"></param>
		public ParticleSprite(Sprite sprite, float x, float y, Vector velocity, int life)
		{
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			m_Sprite = sprite;
			this.Life = life;
		}

		private Sprite m_Sprite;
		/// <summary>
		/// The sprite representing the particle on the destination surface.
		/// </summary>
		public Sprite Sprite
		{
			get
			{
				return m_Sprite;
			}
			set
			{
				m_Sprite = value;
			}
		}
		/// <summary>
		/// Renders the sprite on the destination surface.
		/// </summary>
		/// <param name="destination">The surface to render the sprite.</param>
		public override void Render(Surface destination)
		{
			m_Sprite.X = (int)this.X;
			m_Sprite.Y = (int)this.Y;
			m_Sprite.Render(destination);
		}
	}
}
