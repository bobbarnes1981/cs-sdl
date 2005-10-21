using System;
using System.Collections;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A collection of independant particle systems.
	/// </summary>
	public class ParticleSystemCollection : CollectionBase
	{
		/// <summary>
		/// Creates an empty collection of particle systems.
		/// </summary>
		public ParticleSystemCollection()
		{
		}

		/// <summary>
		/// Creates a collection of particle systems.
		/// </summary>
		/// <param name="system">The system to start off the collection.</param>
		public ParticleSystemCollection(ParticleSystem system)
		{
			Add(system);
		}

		/// <summary>
		/// Adds a system to the collection.
		/// </summary>
		/// <param name="system">The system to add.</param>
		public void Add(ParticleSystem system)
		{
			List.Add(system);
		}

		/// <summary>
		/// Updates all particle systems within the collection.
		/// </summary>
		public void Update()
		{
			foreach(ParticleSystem system in List)
			{
				system.Update();
			}
		}
		/// <summary>
		/// Renders all particle systems within the collection on the destination surface.
		/// </summary>
		/// <param name="destination">The surface to render the particle systems on.</param>
		public void Render(Surface destination)
		{
			foreach(ParticleSystem system in List)
			{
				system.Render(destination);
			}
		}

		/// <summary>
		/// Indexer
		/// </summary>
		public ParticleSystem this[int index]
		{
			get
			{
				return (ParticleSystem)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

	}
}
