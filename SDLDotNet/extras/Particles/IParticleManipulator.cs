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
