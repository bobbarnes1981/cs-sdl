using System;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleFriction.
	/// </summary>
	public class ParticleFriction : IParticleManipulator
	{
		public ParticleFriction()
		{
		}
		public ParticleFriction(float friction)
		{
			m_Friction = friction;
		}

		private float m_Friction = 0.1f;
		public float Friction
		{
			get
			{
				return m_Friction;
			}
			set
			{
				m_Friction = value;
			}
		}

		#region IParticleManipulator Members

		public void Manipulate(ParticleCollection particles)
		{
			foreach(Particle p in particles)
			{
				p.Velocity.Length -= m_Friction;
			}
		}

		#endregion
	}
}
