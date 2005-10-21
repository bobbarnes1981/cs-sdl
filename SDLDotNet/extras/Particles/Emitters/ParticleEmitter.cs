/*
 * $RCSfile$
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Drawing;

using SdlDotNet.Particles;
using SdlDotNet.Particles.Particle;

namespace SdlDotNet.Particles.Emitters
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ParticleEmitter : BaseParticle
	{
		private ParticleCollection m_Target = null;

		/// <summary>
		/// The random number generator associated with the particle emitters.
		/// </summary>
		protected static Random Random = new Random();
		/// <summary>
		/// A helper method to get a random float between the given range.
		/// </summary>
		/// <param name="min">The lower bound.</param>
		/// <param name="max">The upper bound.</param>
		/// <returns>A float between the two numbers.</returns>
		protected static float GetRange(float min, float max)
		{
			return min + (float)Random.NextDouble() * (max - min);
		}


		private float m_Width = 1f;
		private float m_Height = 1f;
		/// <summary>
		/// Gets and sets the height of the particle emitter.
		/// </summary>
		public float Height
		{
			get
			{
				
				return m_Height;
			}
			set
			{
				m_Height = value;
			}
		}
		/// <summary>
		/// Gets and sets the width of the particle emitter.
		/// </summary>
		public float Width
		{
			get
			{
				return m_Width;
			}
			set
			{
				m_Width = value;
			}
		}

		private int m_LifeFullMin = -1;
		private int m_LifeFullMax = -1;
		/// <summary>
		/// Gets and sets the minimum life a particle can have.
		/// </summary>
		public int LifeFullMin
		{
			get
			{
				return m_LifeFullMin;
			}
			set
			{
				m_LifeFullMin = value;
			}
		}
		/// <summary>
		/// Gets and sets the maximum life a particle can have.
		/// </summary>
		public int LifeFullMax
		{
			get
			{
				return m_LifeFullMax;
			}
			set
			{
				m_LifeFullMax = value;
			}
		}

		private int m_LifeMin = -1;
		private int m_LifeMax = -1;
		/// <summary>
		/// Gets and sets the minimum life a particle can have.
		/// </summary>
		public int LifeMin
		{
			get
			{
				return m_LifeMin;
			}
			set
			{
				m_LifeMin = value;
			}
		}
		/// <summary>
		/// Gets and sets the maximum life a particle can have.
		/// </summary>
		public int LifeMax
		{
			get
			{
				return m_LifeMax;
			}
			set
			{
				m_LifeMax = value;
			}
		}

		private float m_DirectionMin = 0f;
		private float m_DirectionMax = (float)(2 * Math.PI);

		/// <summary>
		/// Gets and sets the minimum direction range of particles.
		/// </summary>
		public float DirectionMin
		{
			get
			{
				return m_DirectionMin;
			}
			set
			{
				m_DirectionMin = value;
			}
		}
		/// <summary>
		/// Gets and sets the maximum direction range of particles.
		/// </summary>
		public float DirectionMax
		{
			get
			{
				return m_DirectionMax;
			}
			set
			{
				m_DirectionMax = value;
			}
		}

		private float m_SpeedMin = 0.5f;
		private float m_SpeedMax = 5f;

		/// <summary>
		/// Gets and sets the minimum speed a particle can have.
		/// </summary>
		public float SpeedMin
		{
			get
			{
				return m_SpeedMin;
			}
			set
			{
				m_SpeedMin = value;
			}
		}
		/// <summary>
		/// Gets and sets the maximum speed a particle can have.
		/// </summary>
		public float SpeedMax
		{
			get
			{
				return m_SpeedMax;
			}
			set
			{
				m_SpeedMax = value;
			}
		}


		private float m_FrequencyCounter = 0f;
		private float m_Frequency = 1000f;
		/// <summary>
		/// Gets and sets the frequency of particle emission.  Measured in particle per 1000 updates.
		/// </summary>
		public float Frequency
		{
			get
			{
				return m_Frequency;
			}
			set
			{
				m_Frequency = value;
			}
		}
		

		/// <summary>
		/// Gets and sets the particle collection where this emitter is to send its particles.
		/// </summary>
		public ParticleCollection Target
		{
			get
			{
				return m_Target;
			}
			set
			{
				m_Target = value;
			}
		}

		/// <summary>
		/// Creates a new particle emitter.
		/// </summary>
		public ParticleEmitter()
		{
			this.Static = true;
		}

		/// <summary>
		/// Updates the particle emitter.
		/// </summary>
		/// <returns>True if it's still alive, false if not.</returns>
		public override bool Update()
		{
			m_FrequencyCounter += m_Frequency / 1000f;
			while(m_FrequencyCounter >= 1){
				m_Target.Add(CreateParticle());
				m_FrequencyCounter -= 1f;
			}
			return base.Update();
		}

		/// <summary>
		/// A protected method that will change the attributes of the passed in particle to fit the emitter's description.
		/// </summary>
		/// <param name="p">The particle to change.</param>
		/// <returns>The particle with the changed properties.</returns>
		/// <remarks>Use this method when overriding the CreateParticle method.</remarks>
		protected BaseParticle CreateParticle(BaseParticle p)
		{
			p.Life = Random.Next(m_LifeMin, m_LifeMax);
			p.LifeFull = Random.Next(m_LifeFullMin, m_LifeFullMax);
			p.Velocity = new Vector(GetRange(m_DirectionMin, m_DirectionMax));
			p.Velocity.Length = GetRange(m_SpeedMin, m_SpeedMax);
			p.X = GetRange(this.X, this.X + m_Width);
			p.Y = GetRange(this.Y, this.Y + m_Height);
			return p;
		}

		/// <summary>
		/// Abstract method that will create a particle based on the emitter's attributes.
		/// </summary>
		/// <returns></returns>
		protected abstract BaseParticle CreateParticle();

	}
}
