using System;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle manipulator that pulls all particles by a common gravity.
	/// </summary>
	public class ParticleGravity : IParticleManipulator
	{
		private Vector m_Velocity = new Vector(0f,0.2f);
		/// <summary>
		/// Creates a new ParticleSystem with a common gravity.
		/// </summary>
		/// <param name="velocity">The velocity (horizontal and vertical) of the particle system.</param>
		public ParticleGravity(Vector velocity)
		{
			m_Velocity = velocity;
		}

		/// <summary>
		/// Creates a new ParticleSystem using gravity and wind.
		/// </summary>
		/// <param name="gravity">The vertical gravity of the system.</param>
		/// <param name="wind">The horizontal gravity of the system.  This is commonly refered to as wind.</param>
		public ParticleGravity(float gravity, float wind) : this(new Vector(wind, gravity))
		{
		}

		/// <summary>
		/// Creates a new ParticleSystem with a vertical gravity.
		/// </summary>
		/// <param name="gravity">The vertical gravity of the system.</param>
		public ParticleGravity(float gravity) : this(new Vector(0,gravity))
		{
		}

		/// <summary>
		/// Creates a new ParticleSystem.
		/// </summary>
		/// <remarks>The gravity defaults as "new Vector()".</remarks>
		public ParticleGravity() : this(new Vector())
		{
		}

		/// <summary>
		/// Gets and sets the velocity (direction and speed) of the gravity as a vector.
		/// </summary>
		public Vector Velocity
		{
			get
			{
				return m_Velocity;
			}
			set
			{
				m_Velocity = value;
			}
		}

		/// <summary>
		/// Gets and sets the vertical pull of the gravity.
		/// </summary>
		public float Gravity
		{
			get
			{
				return m_Velocity.Y;
			}
			set
			{
				m_Velocity.Y = value;
			}
		}

		/// <summary>
		/// Gets and sets the horizontal push of the gravity.
		/// </summary>
		public float Wind
		{
			get
			{
				return m_Velocity.X;
			}
			set
			{
				m_Velocity.X = value;
			}
		}


		#region IParticleManipulator Members

		/// <summary>
		/// Manipulate particles by the gravity.
		/// </summary>
		/// <param name="particles">The particles to pull by the gravity.</param>
		public void Manipulate(ParticleCollection particles)
		{
			if (particles == null)
			{
				throw new ArgumentNullException("particles");
			}
			foreach(Particle p in particles)
			{
				if(p is ParticleEmitter)
				{
					Manipulate(((ParticleEmitter)p).Children);
				}
				else
				{
					p.Velocity += m_Velocity;
				}
			}
		}

		#endregion
	}
}
