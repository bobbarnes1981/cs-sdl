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

using SdlDotNet;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A collection of particles manipulated by a common gravity.
	/// </summary>
	public class ParticleSystem : ParticleCollection
	{
		private Vector m_Gravity;

		/// <summary>
		/// Creates a new ParticleSystem with a common gravity.
		/// </summary>
		/// <param name="gravity">The gravity (horizontal and vertical) of the particle system.</param>
		public ParticleSystem(Vector gravity)
		{
			m_Gravity = gravity;
		}

		/// <summary>
		/// Creates a new ParticleSystem using gravity and wind.
		/// </summary>
		/// <param name="gravity">The vertical gravity of the system.</param>
		/// <param name="wind">The horizontal gravity of the system.  This is commonly refered to as wind.</param>
		public ParticleSystem(float gravity, float wind) : this(new Vector(wind, gravity))
		{
		}

		/// <summary>
		/// Creates a new ParticleSystem with a vertical gravity.
		/// </summary>
		/// <param name="gravity">The vertical gravity of the system.</param>
		public ParticleSystem(float gravity) : this(new Vector(0,gravity))
		{
		}

		/// <summary>
		/// Creates a new ParticleSystem.
		/// </summary>
		/// <remarks>The gravity defaults as "new Vector()".</remarks>
		public ParticleSystem() : this(new Vector())
		{
		}

		/// <summary>
		/// Updates all particles within this system using the given gravity.
		/// </summary>
		public override void Update()
		{
			foreach(Particle particle in this.List)
				particle.Velocity += m_Gravity;
			base.Update ();
		}
	}
}
