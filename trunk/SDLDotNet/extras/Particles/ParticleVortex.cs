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

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle manipulator that pulls particles towards a point.
	/// </summary>
	/// <remarks>If the radius is set to -1, the pull (strength) on all particles is constant.</remarks>
	public class ParticleVortex : IParticleManipulator
	{
		/// <summary>
		/// Creates a particle vortex manipulator with the default values.
		/// </summary>
		public ParticleVortex()
		{
		}
		/// <summary>
		/// Creates a particle vortex manipulator from just the location.
		/// </summary>
		/// <param name="x">The X coordinate of the vortex.</param>
		/// <param name="y">The Y coordinate of the vortex.</param>
		public ParticleVortex(float x, float y)
		{
			m_x = x;
			m_y = y;
		}
		/// <summary>
		/// Creates a particle vortex manipulator.
		/// </summary>
		/// <param name="x">The X coordinate of the vortex.</param>
		/// <param name="y">The Y coordinate of the vortex.</param>
		/// <param name="strength">The amount of pull applied to the particles.</param>
		/// <param name="radius">The size of the vortex. -1 is infinate size.</param>
		public ParticleVortex(float x, float y, float strength, float radius)
		{
			m_x = x;
			m_y = y;
			m_Strength = strength;
			m_Radius = radius;
		}
		/// <summary>
		/// Creates a particle vortex manipulator with an infinate size.
		/// </summary>
		/// <param name="x">The X coordinate of the vortex.</param>
		/// <param name="y">The Y coordinate of the vortex.</param>
		/// <param name="strength">The amount of pull applied to the particles.</param>
		public ParticleVortex(float x, float y, float strength)
		{
			m_x = x;
			m_y = y;
			m_Strength = strength;
		}
		/// <summary>
		/// Creates a particle vortex manipulator from just the strength.
		/// </summary>
		/// <param name="strength">The amount of pull applied to the particles.</param>
		public ParticleVortex(float strength)
		{
			m_Strength = strength;
		}

		private float m_x = 0;
		private float m_y = 0;
		/// <summary>
		/// Gets and sets the Y coordinate of the vortex.
		/// </summary>
		public float Y
		{
			get
			{
				return m_y;
			}
			set
			{
				m_y = value;
			}
		}
		/// <summary>
		/// Gets and sets the X coordinate of the vortex.
		/// </summary>
		public float X
		{
			get
			{
				return m_x;
			}
			set
			{
				m_x = value;
			}
		}
		private float m_Radius = -1f;
		/// <summary>
		/// Gets and sets the size, the radius, of the vortex.  If set to -1 the pull will be constant on each particle.
		/// </summary>
		public float Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}
		private float m_Strength = 0.2f;
		/// <summary>
		/// Gets and sets the amount of pull put onto each particle in the vortex's radius.
		/// </summary>
		public float Strength
		{
			get
			{
				return m_Strength;
			}
			set
			{
				m_Strength = value;
			}
		}
		#region IParticleManipulator Members

		/// <summary>
		/// Applies the vortex force on each particle in the vortex's radius.
		/// </summary>
		/// <param name="particles">The collection of particles to manipulate.</param>
		public void Manipulate(ParticleCollection particles)
		{
			foreach(Particle p in particles)
			{
				Vector v = new Vector(p.X,p.Y,m_x,m_y);
				if(m_Radius == -1f)
				{
					v.Length = m_Strength;
				}
				else
				{
					if(v.Length > m_Radius)
						continue;
					v.Length = (1f - v.Length / m_Radius) * m_Strength;
				}
				p.Velocity += v;
			}
		}

		#endregion
	}
}
