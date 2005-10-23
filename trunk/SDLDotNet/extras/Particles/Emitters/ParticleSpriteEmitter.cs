using System;
using SdlDotNet.Sprites;
using SdlDotNet.Particles.Particle;

namespace SdlDotNet.Particles.Emitters
{
	/// <summary>
	/// A particle emitter that shoots out sprites from a sprite collection.
	/// </summary>
	public class ParticleSpriteEmitter : ParticleEmitter
	{
		private SpriteCollection m_Sprites;
		/// <summary>
		/// Gets and sets the collection of sprites assosiated with this particle emitter.
		/// </summary>
		public SpriteCollection Sprites
		{
			get
			{
				return m_Sprites;
			}
//			set
//			{
//				m_Sprites = value;
//			}
		}
		/// <summary>
		/// Creates a new particle emitter that emits sprite objects.
		/// </summary>
		/// <param name="system">The particle system to add this particle emitter.</param>
		/// <param name="sprite">The sprite to emit.</param>
		public ParticleSpriteEmitter(ParticleSystem system, Sprite sprite) : base(system)
		{
			m_Sprites = new SpriteCollection(sprite);
		}
		/// <summary>
		/// Creates a new particle emitter that emits sprite objects.
		/// </summary>
		/// <param name="system">The particle system to add this particle emitter.</param>
		/// <param name="sprites">The sprite collection to choose sprites from when emitting.</param>
		public ParticleSpriteEmitter(ParticleSystem system, SpriteCollection sprites) : base(system)
		{
			m_Sprites = sprites;
		}
		/// <summary>
		/// Creates a new particle emitter that emits sprite objects.
		/// </summary>
		/// <param name="sprite">The sprite to emit.</param>
		public ParticleSpriteEmitter(Sprite sprite)
		{
			m_Sprites = new SpriteCollection(sprite);
		}
		/// <summary>
		/// Creates a new particle emitter that emits sprite objects.
		/// </summary>
		/// <param name="sprites">The sprite collection to choose sprites from when emitting.</param>
		public ParticleSpriteEmitter(SpriteCollection sprites)
		{
			m_Sprites = sprites;
		}
		/// <summary>
		/// Creates a particle based on the sprite parameters.
		/// </summary>
		/// <returns>The created particle.</returns>
		protected override BaseParticle CreateParticle()
		{
			if(m_Sprites.Count == 0)
				return null;
			return CreateParticle(
				new ParticleSprite(
					m_Sprites[Random.Next(0,m_Sprites.Count-1)]
				));
		}
	}
}
