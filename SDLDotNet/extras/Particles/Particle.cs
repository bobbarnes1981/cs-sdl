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

using SdlDotNet;
using SdlDotNet.Sprites;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public abstract class Particle
	{
		private int m_Life = -1;
		/// <summary>
		/// The current life of the particle.
		/// </summary>
		/// <remarks>This is decreased when the Update method is called.</remarks>
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
		/// <param name="destination">The destination surface of the particle.</param>
		public abstract void Render(Surface destination);

		/// <summary>
		/// Updates the location and life of the particle.
		/// </summary>
		/// <returns>True if the particle is still alive, false if the particle is to be destroyed.</returns>
		public virtual bool Update()
		{
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
