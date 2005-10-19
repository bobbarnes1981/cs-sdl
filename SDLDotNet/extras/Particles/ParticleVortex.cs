using System;
using System.Drawing;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleVortex.
	/// </summary>
	public class ParticleVortex : IParticleManipulator
	{
		public ParticleVortex()
		{
		}
		public ParticleVortex(float x, float y)
		{
			m_x = x;
			m_y = y;
		}
		public ParticleVortex(float x, float y, float strength)
		{
			m_x = x;
			m_y = y;
			m_Strength = strength;
		}
		public ParticleVortex(float strength)
		{
			m_Strength = strength;
		}

		private float m_x = 0;
		private float m_y = 0;
		public float Y
		{
			get
			{
				return m_y;
			}
			set
			{
				m_y = value;
			}
		}
		public float X
		{
			get
			{
				return m_x;
			}
			set
			{
				m_x = value;
			}
		}
		private float m_Strength = 0.2f;
		public float Strength
		{
			get
			{
				return m_Strength;
			}
			set
			{
				m_Strength = value;
			}
		}
		#region IParticleManipulator Members

		public void Manipulate(ParticleCollection particles)
		{
			foreach(Particle p in particles)
			{
				Vector v = new Vector(p.X,p.Y,m_x,m_y);
				v.Length = m_Strength;///v.Length;
				p.Velocity += v;
			}
		}

		#endregion
	}
}
