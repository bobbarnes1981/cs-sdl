/*
 * $RCSfile: ParticleSystem.cs,v $
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
using System.Drawing;

using SdlDotNet;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle represented by a surface.
	/// </summary>
	public class ParticleSurface : Particle
	{
		private Surface m_Surface;
		/// <summary>
		/// Gets and sets the surface used to represent the particle.
		/// </summary>
		public Surface Surface
		{
			get
			{
				return m_Surface;
			}
			set
			{
				m_Surface = value;
			}
		}

		private Rectangle m_ClipRectangle;
		/// <summary>
		/// Gets and sets the clipping rectangle of the surface.
		/// </summary>
		public Rectangle ClipRectangle
		{
			get
			{
				return m_ClipRectangle;
			}
			set
			{
				m_ClipRectangle = value;
			}
		}

		/// <summary>
		/// Creates a particle surface.
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		/// <param name="life"></param>
		public ParticleSurface(Surface surface, float x, float y, Vector velocity, int life)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			this.Life = life;
			this.LifeFull = life;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		public ParticleSurface(Surface surface, float x, float y, Vector velocity)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ParticleSurface(Surface surface, float x, float y)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="life"></param>
		public ParticleSurface(Surface surface, float x, float y, int life)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Life = life;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public ParticleSurface(Surface surface)
		{
			m_Surface = surface;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="clip"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		/// <param name="life"></param>
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, Vector velocity, int life)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			this.Life = life;
			this.LifeFull = life;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="clip"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, Vector velocity)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="clip"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="clip"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="life"></param>
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, int life)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Life = life;
			this.LifeFull = life;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		/// <param name="clip"></param>
		public ParticleSurface(Surface surface, Rectangle clip)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
		}

		/// <summary>
		/// Renders the surface as the particle.
		/// </summary>
		/// <param name="destination">The surface to blit the particle.</param>
		public override void Render(Surface destination)
		{
			destination.Blit(m_Surface, new Point((int)this.X, (int)this.Y), m_ClipRectangle);
		}

	}
}
