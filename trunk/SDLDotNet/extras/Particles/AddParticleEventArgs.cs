/*
 * $RCSfile$
 * Copyright (C) 2004, 2005 David Hudson (jendave@yahoo.com)
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
	/// Summary description for AddParticleEventArgs.
	/// </summary>
	public class AddParticleEventArgs : SdlEventArgs 
	{
		Particle particle;
		/// <summary>
		/// Create Event args
		/// </summary>
		/// <param name="particle">Particle added</param>
		public AddParticleEventArgs(Particle particle)
		{
			this.particle = particle;
		}

		/// <summary>
		/// Particle Added
		/// </summary>
		public Particle Particle
		{
			get
			{
				return this.particle;
			}
		}
	}
}