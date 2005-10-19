using System;
using System.Collections;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Describes a collection of particle manipulators.
	/// </summary>
	public class ParticleManipulatorCollection : CollectionBase, IParticleManipulator
	{
		/// <summary>
		/// Create an empty Particle Manipulator collection.
		/// </summary>
		public ParticleManipulatorCollection()
		{
		}
		/// <summary>
		/// Create a particle manipulator collection with one element in it.
		/// </summary>
		/// <param name="manipulator">The first manipulator in the collection.</param>
		public ParticleManipulatorCollection(IParticleManipulator manipulator)
		{
			Add(manipulator);
		}
		/// <summary>
		/// Create a particle manipulator based off an already existing collection.
		/// </summary>
		/// <param name="manipulators">The manipulators to add to the collection.</param>
		public ParticleManipulatorCollection(ParticleManipulatorCollection manipulators)
		{
			Add(manipulators);
		}

		/// <summary>
		/// Add a particle manipulator to the collection.
		/// </summary>
		/// <param name="manipulator">The manipulator to add to the collection.</param>
		public void Add(IParticleManipulator manipulator)
		{
			List.Add(manipulator);
		}

		/// <summary>
		/// Add a collection of particle manipulators to the collection.
		/// </summary>
		/// <param name="manipulators">The manipulators to add to the collection.</param>
		public void Add(ParticleManipulatorCollection manipulators)
		{
			List.Add(manipulators);
		}
		/// <summary>
		/// Indexer
		/// </summary>
		public IParticleManipulator this[int index]
		{
			get
			{
				return (IParticleManipulator)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		#region IParticleManipulator Members

		/// <summary>
		/// Manipulate a collection of particles with the manipulators contained in the collection.
		/// </summary>
		/// <param name="particles">The particles to manipulate.</param>
		public void Manipulate(ParticleCollection particles)
		{
			foreach(IParticleManipulator manipulator in List)
			{
				manipulator.Manipulate(particles);
			}
		}

		#endregion
	}
}
