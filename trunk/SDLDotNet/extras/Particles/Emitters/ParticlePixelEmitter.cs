using System;
using System.Drawing;

using SdlDotNet.Particles.Particle;

namespace SdlDotNet.Particles.Emitters
{
	/// <summary>
	/// A particle emitter that emits pixel particles.
	/// </summary>
	public class ParticlePixelEmitter : ParticleEmitter
	{
		/// <summary>
		/// Creates a new pixel particle emitter.
		/// </summary>
		public ParticlePixelEmitter() : base()
		{
		}

		/// <summary>
		/// Creates a new pixel particle emitter inside a particle system.
		/// </summary>
		/// <param name="system">The system that the particle emitter should be added to.</param>
		public ParticlePixelEmitter(ParticleSystem system) : base(system)
		{
		}

		/// <summary>
		/// Creates a particle emitter that emits particels inside the given system with min and max color values.
		/// </summary>
		/// <param name="system">The system that the particle emitter should be added to.</param>
		/// <param name="minColor">The minimum color values of emitted particles.</param>
		/// <param name="maxColor">The maximum color values of emitted particles.</param>
		public ParticlePixelEmitter(ParticleSystem system, Color minColor, Color maxColor) : base(system)
		{
			ColorMin = minColor;
			ColorMax = maxColor;
		}

		/// <summary>
		/// Creates a particle emitter with min and max color values.
		/// </summary>
		/// <param name="minColor">The minimum color values of emitted particles.</param>
		/// <param name="maxColor">The maximum color values of emitted particles.</param>
		public ParticlePixelEmitter(Color minColor, Color maxColor) : base()
		{
			ColorMin = minColor;
			ColorMax = maxColor;
		}

		/// <summary>
		/// Gets and sets the minimum color values.
		/// </summary>
		public Color ColorMin
		{
			get
			{
				return Color.FromArgb(m_MinR, m_MinG, m_MinB);
			}
			set
			{
				m_MinR = value.R;
				m_MinG = value.G;
				m_MinB = value.B;
			}
		}

		/// <summary>
		/// Gets and sets the maximum color values.
		/// </summary>
		public Color ColorMax
		{
			get
			{
				return Color.FromArgb(m_MaxR, m_MaxG, m_MaxB);
			}
			set
			{
				m_MaxR = value.R;
				m_MaxG = value.G;
				m_MaxB = value.B;
			}
		}

		private byte m_MinR;
		private byte m_MinG;
		private byte m_MinB;
		private byte m_MaxR = 255;
		private byte m_MaxG = 255;
		private byte m_MaxB = 255;

		/// <summary>
		/// Gets and sets the minimum R value for emitted particles.
		/// </summary>
		public byte MinR
		{
			get
			{
				return m_MinR;
			}
			set
			{
				m_MinR = value;
			}
		}

		/// <summary>
		/// Gets and sets the maximum R value for emitted particles.
		/// </summary>
		public byte MaxR
		{
			get
			{
				return m_MaxR;
			}
			set
			{
				m_MaxR = value;
			}
		}

		/// <summary>
		/// Gets and sets the minimum G value for emitted particles.
		/// </summary>
		public byte MinG
		{
			get
			{
				return m_MinG;
			}
			set
			{
				m_MinG = value;
			}
		}

		/// <summary>
		/// Gets and sets the maximum G value for emitted particles.
		/// </summary>
		public byte MaxG
		{
			get
			{
				return m_MaxG;
			}
			set
			{
				m_MaxG = value;
			}
		}

		/// <summary>
		/// Gets and sets the minimum B value for emitted particles.
		/// </summary>
		public byte MinB
		{
			get
			{
				return m_MinB;
			}
			set
			{
				m_MinB = value;
			}
		}

		/// <summary>
		/// Gets and sets the maximum B value for emitted particles.
		/// </summary>
		public byte MaxB
		{
			get
			{
				return m_MaxB;
			}
			set
			{
				m_MaxB = value;
			}
		}

		/// <summary>
		/// A protected method to return a new particle pixel with the randomized color attributes.
		/// </summary>
		/// <returns>A new particle pixel with the new color values.</returns>
		protected override SdlDotNet.Particles.Particle.BaseParticle CreateParticle()
		{
			ParticlePixel p = new ParticlePixel();
			p.Color = Color.FromArgb(
				Random.Next(m_MinR, m_MaxR),
				Random.Next(m_MinG, m_MaxG),
				Random.Next(m_MinB, m_MaxB));
			return p;
		}
	}
}
