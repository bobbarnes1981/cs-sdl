using System;
using SdlDotNet;
using SdlDotNet.Particles.Particle;

namespace SdlDotNet.Particles.Emitters
{
	/// <summary>
	/// A particle emitter that emits surfaces from a surface collection to represent particles..
	/// </summary>
	public class ParticleSurfaceEmitter : ParticleEmitter
	{
		private SurfaceCollection m_Surfaces;
		/// <summary>
		/// Gets and sets the collection of surfaces assosiated with this particle emitter.
		/// </summary>
		public SurfaceCollection Surfaces
		{
			get
			{
				return m_Surfaces;
			}
//			set
//			{
//				m_Surfaces = value;
//			}
		}
		/// <summary>
		/// Creates a new particle emitter that emits surface objects.
		/// </summary>
		/// <param name="system">The particle system to add this particle emitter.</param>
		/// <param name="surface">The surface to emit.</param>
		public ParticleSurfaceEmitter(ParticleSystem system, Surface surface) : base(system)
		{
			m_Surfaces = new SurfaceCollection(surface);
		}
		/// <summary>
		/// Creates a new particle emitter that emits surface objects.
		/// </summary>
		/// <param name="system">The particle system to add this particle emitter.</param>
		/// <param name="surfaces">The surface collection to choose surfaces from when emitting.</param>
		public ParticleSurfaceEmitter(ParticleSystem system, SurfaceCollection surfaces) : base(system)
		{
			m_Surfaces = surfaces;
		}
		/// <summary>
		/// Creates a new particle emitter that emits surface objects.
		/// </summary>
		/// <param name="surface">The surface to emit.</param>
		public ParticleSurfaceEmitter(Surface surface)
		{
			m_Surfaces = new SurfaceCollection(surface);
		}
		/// <summary>
		/// Creates a new particle emitter that emits surface objects.
		/// </summary>
		/// <param name="surfaces">The surface collection to choose surfaces from when emitting.</param>
		public ParticleSurfaceEmitter(SurfaceCollection surfaces)
		{
			m_Surfaces = surfaces;
		}
		/// <summary>
		/// Creates a particle from a surface in the surface collection.
		/// </summary>
		/// <returns>The created particle.</returns>
		protected override BaseParticle CreateParticle()
		{
			if(m_Surfaces.Count == 0)
			{
				return null;
			}
			return new ParticleSurface(
				m_Surfaces[Random.Next(0,m_Surfaces.Count-1)]
				);
		}
	}
}
