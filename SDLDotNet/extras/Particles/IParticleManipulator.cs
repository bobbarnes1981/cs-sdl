/*
 * $RCSfile: Particle.cs,v $
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

namespace SdlDotNet.Particles
{
	/// <summary>
	/// An interface describing a force that manipulates a group of particles.
	/// </summary>
	public interface IParticleManipulator
	{
		/// <summary>
		/// Manipulates the given group of particles by the manipulators force.
		/// </summary>
		/// <param name="particles">The collection of particles to manipulate.</param>
		/// <remarks>This should only affect the particles' velocity.</remarks>
		void Manipulate(ParticleCollection particles);
	}
}
