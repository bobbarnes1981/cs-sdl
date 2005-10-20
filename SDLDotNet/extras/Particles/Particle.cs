/*
 * $RCSfile$
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
using SdlDotNet.Sprites;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// An abstract class describing a base particle.
	/// </summary>
	/// <remarks>
	/// Some implementations of the particle class 
	/// include ParticlePixel and ParticleSprite.</remarks>
	public abstract class Particle
	{
//		public static Particle Randomize(Particle p, Rectangle bounds, float minSpeed, float maxSpeed, int minLife, int maxLife, float minDir, float maxDir)
//		{
//			p.X = ParticleSystem.Range(bounds.Left,bounds.Right);
//			p.Y = ParticlesSystem.Range(bounds.Top,bounds.Bottom);
//			p.Life = ParticleSystem.random.Next(minLife,maxLife);
//			p.LifeFull = p.Life;
//			p.Velocity.Length = ParticleSystem.Range(minSpeed, maxSpeed);
//			p.Velocity.Direction = ParticleSystem.Range(minDir, maxDir);
//			return p;
//		}
//		public static Particle Randomize(Particle p, Rectangle bounds, float minSpeed, float maxSpeed, int minLife, int maxLife)
//		{
//			return Randomize(p, bounds, minSpeed, maxSpeed, minLife, maxLife, 0, ParticleSystem.random.NextDouble() * Math.PI * 2);
//		}

		private int m_Life = -1;
		/// <summary>
		/// The current life of the particle. -1 means infinate life.
		/// </summary>
		/// <remarks>
		/// This is decreased when the Update method is called.
		/// </remarks>
		public int Life
		{
			get
			{
				return m_Life;
			}
			set
			{
				m_Life = value;
			}
		}

		private int m_LifeFull = 100;
		/// <summary>
		/// Gets and sets the value representing the full life of the particle.
		/// </summary>
		/// <remarks>
		/// This is usually used when distinguishing when the 
		/// particle should start dying out with alpha transparency.
		/// </remarks>
		public int LifeFull
		{
			get
			{
				return m_LifeFull;
			}
			set
			{
				m_LifeFull = value;
			}
		}

		private float m_X = 0;
		/// <summary>
		/// The X coordinate of the particle.
		/// </summary>
		public float X
		{
			get
			{
				return m_X;
			}
			set
			{
				m_X = value;
			}
		}
		private float m_Y = 0;
		/// <summary>
		/// The Y coordinate of the particle.
		/// </summary>
		public float Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				m_Y = value;
			}
		}

		private Vector m_Velocity = new Vector(0,0);
		/// <summary>
		/// The speed and direction the particle is going.
		/// </summary>
		public Vector Velocity
		{
			get
			{
				return m_Velocity;
			}
			set
			{
				m_Velocity = value;
			}
		}
		
		/// <summary>
		/// Draws the particle onto the destination.
		/// </summary>
		/// <param name="destination">
		/// The destination surface of the particle.
		/// </param>
		public abstract void Render(Surface destination);

		/// <summary>
		/// Updates the location and life of the particle.
		/// </summary>
		/// <returns>
		/// True if the particle is still alive, 
		/// false if the particle is to be destroyed.
		/// </returns>
		public virtual bool Update()
		{
			if(m_Life == 0)
			{
				return false;
			}
			m_X += m_Velocity.X;
			m_Y += m_Velocity.Y;
			if(m_Life != -1) // -1 is alife forever.
			{
				if(--m_Life == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
