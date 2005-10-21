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
//		/// <summary>
//		/// Provide the explicit interface member for ICollection.
//		/// </summary>
//		/// <param name="array">Array to copy collection to</param>
//		/// <param name="index">Index at which to insert the collection items</param>
//		void ICollection.CopyTo(Array array, int index)
//		{
//			this.List.CopyTo(array, index);
//		}

		/// <summary>
		/// Provide the explicit interface member for ICollection.
		/// </summary>
		/// <param name="array">Array to copy collection to</param>
		/// <param name="index">Index at which to insert the collection items</param>
		public virtual void CopyTo(ParticleSystem[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Insert a ParticleSystem into the collection
		/// </summary>
		/// <param name="index">Index at which to insert the sprite</param>
		/// <param name="particleSystem">ParticleSystem to insert</param>
		public virtual void Insert(int index, ParticleSystem particleSystem)
		{
			List.Insert(index, particleSystem);
		} 

		/// <summary>
		/// Gets the index of the given particleSystem in the collection.
		/// </summary>
		/// <param name="particleSystem">The particleSystem to search for.</param>
		/// <returns>The index of the given particleSystem.</returns>
		public virtual int IndexOf(ParticleSystem particleSystem)
		{
			return List.IndexOf(particleSystem);
		}

		/// <summary>
		/// Removes particleSystem from group
		/// </summary>
		/// <param name="particleSystem">particleSystem to remove</param>
		public virtual void Remove(ParticleSystem particleSystem)
		{
			List.Remove(particleSystem);
		}

		/// <summary>
		/// Checks if particleSystem is in the container
		/// </summary>
		/// <param name="particleSystem">particleSystem to query for</param>
		/// <returns>True is the particleSystem is in the container.</returns>
		public bool Contains(ParticleSystem particleSystem)
		{
			return (List.Contains(particleSystem));
		}
	}
}
