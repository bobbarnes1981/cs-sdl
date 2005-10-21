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
		public ParticlePixelEmitter()
		{
		}

		private byte m_MinR = 0;
		private byte m_MinG = 0;
		private byte m_MinB = 0;
		private byte m_MaxR = 255;
		private byte m_MaxG = 255;
		private byte m_MaxB = 255;

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

		protected override SdlDotNet.Particles.Particle.BaseParticle CreateParticle()
		{
			ParticlePixel p = (ParticlePixel)CreateParticle(new ParticlePixel());
			p.Color = Color.FromArgb(
				Random.Next(m_MinR, m_MaxR),
				Random.Next(m_MinG, m_MaxG),
				Random.Next(m_MinB, m_MaxB));
			return p;
		}

		public override void Render(Surface destination)
		{

		}


	}
}
