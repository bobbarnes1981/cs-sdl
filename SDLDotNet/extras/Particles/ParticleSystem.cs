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

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A collection of particles manipulated by a number of common manipulators.
	/// </summary>
	public class ParticleSystem : ParticleCollection
	{

		internal static Random random = new Random();

		internal static float Range(float min, float max)
		{
			return min + (float)random.NextDouble() * (max - min);
		}

		private ParticleManipulatorCollection m_Manipulators;

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

		public ParticleSystem()
		{
			m_Manipulators = new ParticleManipulatorCollection();
		}

		public ParticleSystem(ParticleManipulatorCollection manipulators)
		{
			m_Manipulators = manipulators;
		}

		public ParticleSystem(IParticleManipulator manipulator)
		{
			m_Manipulators.Add(manipulator);
		}

		public void Add(int minParticles, int maxParticles, System.Type particleType)
		{

		}
		



		/// <summary>
		/// Updates all particles within this system using the given gravity.
		/// </summary>
		public override void Update()
		{
			m_Manipulators.Manipulate(this);
			base.Update();
		}
	}
}
