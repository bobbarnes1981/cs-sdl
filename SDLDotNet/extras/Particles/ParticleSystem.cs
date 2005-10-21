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
using SdlDotNet.Particles.Particle;
using SdlDotNet.Particles.Manipulators;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A collection of particles manipulated by a number of common manipulators.
	/// </summary>
	public class ParticleSystem : ParticleCollection
	{
		private ParticleManipulatorCollection m_Manipulators;

		/// <summary>
		/// Gets and sets the collection of manipulators to manipulate the particles in the system.
		/// </summary>
		public ParticleManipulatorCollection Manipulators
		{
			get
			{
				return m_Manipulators;
			}
			set
			{
				m_Manipulators = value;
			}
		}

		/// <summary>
		/// Creates an empty particle system with no manipulators.
		/// </summary>
		public ParticleSystem()
		{
			m_Manipulators = new ParticleManipulatorCollection();
		}

		/// <summary>
		/// Creates a particle system with a collection of particles already in it.
		/// </summary>
		/// <param name="particles">The particles to use with this system.</param>
		public ParticleSystem(ParticleCollection particles)
		{
			m_Manipulators = new ParticleManipulatorCollection();
			this.Add(particles);
		}

		/// <summary>
		/// Copy Constructor.
		/// </summary>
		/// <param name="system">The particle system to copy.</param>
		public ParticleSystem(ParticleSystem system)
		{
			m_Manipulators = new ParticleManipulatorCollection(system.Manipulators);
			this.Add(system);
		}

		/// <summary>
		/// Creates a particle system with an already created manipulators and particles.
		/// </summary>
		/// <param name="manipulators">The manipulators to associate with this particle system.</param>
		/// <param name="particles">The particles to add to this particle system.</param>
		public ParticleSystem(ParticleManipulatorCollection manipulators, ParticleCollection particles)
		{
			m_Manipulators = manipulators;
			this.Add(particles);
		}

		/// <summary>
		/// Creates an empty particle system with the desired paritcle manipulators.
		/// </summary>
		/// <param name="manipulators">The manipulators to use with the contained particles.</param>
		public ParticleSystem(ParticleManipulatorCollection manipulators)
		{
			m_Manipulators = manipulators;
		}

		/// <summary>
		/// Creates an empty particle system with one particle manipulator.
		/// </summary>
		/// <param name="manipulator">The manipulator to use with this particle system.</param>
		public ParticleSystem(IParticleManipulator manipulator)
		{
			m_Manipulators = new ParticleManipulatorCollection();
			m_Manipulators.Add(manipulator);
		}

		/// <summary>
		/// Updates all particles within this system using the given gravity.
		/// </summary>
		public override bool Update()
		{
			m_Manipulators.Manipulate(this);
			return base.Update();
		}
	}
}
