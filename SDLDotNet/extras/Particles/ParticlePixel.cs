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

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle represented by a pixel on the destination surface.
	/// </summary>
	public class ParticlePixel : Particle
	{
		/// <summary>
		/// Creates a new ParticlePixel.
		/// </summary>
		public ParticlePixel()
		{
		}
		/// <summary>
		/// Creates a new ParticlePixel.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="color">The color of the pixel on the destination surface.</param>
		public ParticlePixel(Color color, float x, float y)
		{
			this.X = x;
			this.Y = y;
			m_Color = color;
		}
		/// <summary>
		/// Creates a new ParticlePixel.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="velocity">The speed and direction of the particle.</param>
		public ParticlePixel(float x, float y, Vector velocity)
		{
			this.Velocity = velocity;
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// Creates a new ParticlePixel.
		/// </summary>
		/// <param name="color">The color of the pixel on the destination surface.</param>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="life">How long the particle is to stay alive.</param>
		public ParticlePixel(Color color, float x, float y, int life)
		{
			this.X = x;
			this.Y = y;
			m_Color = color;
			Life = life;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		/// <param name="life"></param>
		public ParticlePixel(Color color, float x, float y, Vector velocity, int life)
		{
			this.Velocity = velocity;
			this.X = x;
			this.Y = y;
			m_Color = color;
			Life = life;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="velocity"></param>
		public ParticlePixel(Color color, float x, float y, Vector velocity)
		{
			this.Velocity = velocity;
			this.X = x;
			this.Y = y;
			m_Color = color;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		/// <param name="velocity"></param>
		public ParticlePixel(Color color, Vector velocity)
		{
			this.Velocity = velocity;
			m_Color = color;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ParticlePixel(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="color"></param>
		public ParticlePixel(Color color)
		{
			m_Color = color;
		}
		private Color m_Color = Color.Black;
		/// <summary>
		/// The color of the particle pixel when drawn on the destination surface.
		/// </summary>
		public Color Color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
			}
		}

		#region IParticle Members


		/// <summary>
		/// Draws the particle on the destination surface represented by a pixel.
		/// </summary>
		/// <param name="destination">The destination surface where to draw the particle.</param>
		public override void Render(Surface destination)
		{
			destination.DrawPixel((int)this.X,(int)this.Y, m_Color);
		}


		#endregion
	}
}
