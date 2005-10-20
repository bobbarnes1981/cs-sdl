/*
 * $RCSfile: Particle.cs,v $
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

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleEmitter.
	/// </summary>
	public delegate Particle AddParticleEvent(ParticleEmitter sender, EventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public class ParticleEmitter : Particle
	{
		private ParticleCollection m_Children = new ParticleCollection();
		/// <summary>
		/// 
		/// </summary>
		public ParticleCollection Children
		{
			get
			{
				return m_Children;
			}
			set
			{
				m_Children = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ParticleEmitter()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public event AddParticleEvent AddParticle;


		private bool m_Static = true;

		/// <summary>
		/// 
		/// </summary>
		public bool Static
		{
			get
			{
				return m_Static;
			}
			set
			{
				m_Static = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		public override void Render(Surface destination)
		{
			m_Children.Render(destination);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool Update()
		{
			bool stillAlive = false;
			PointF pos = new PointF(this.X,this.Y);
			if(stillAlive = base.Update())
			{
				// Add particles?
				if(AddParticle != null)
				{
					Particle p = AddParticle(this,null);
					m_Children.Add(p);
				}
			}
			if(m_Static)
			{
				this.X = pos.X;
				this.Y = pos.Y;
			}
			return stillAlive || m_Children.Update();
		}
	}
}
