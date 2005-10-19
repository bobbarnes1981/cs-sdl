using System;
using System.Drawing;

using SdlDotNet;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle represented by a surface.
	/// </summary>
	public class ParticleSurface : Particle
	{
		private Surface m_Surface;
		public Surface Surface
		{
			get
			{
				return m_Surface;
			}
			set
			{
				m_Surface = value;
			}
		}

		private Rectangle m_ClipRectangle;
		public Rectangle ClipRectangle
		{
			get
			{
				return m_ClipRectangle;
			}
			set
			{
				m_ClipRectangle = value;
			}
		}

		public ParticleSurface(Surface surface, float x, float y, Vector velocity, int life)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			this.Life = life;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		public ParticleSurface(Surface surface, float x, float y, Vector velocity)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		public ParticleSurface(Surface surface, float x, float y)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		public ParticleSurface(Surface surface, float x, float y, int life)
		{
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Life = life;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}
		public ParticleSurface(Surface surface)
		{
			m_Surface = surface;
			m_ClipRectangle = new Rectangle(0, 0, m_Surface.Width, m_Surface.Height);
		}

		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, Vector velocity, int life)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
			this.Life = life;
		}
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, Vector velocity)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Velocity = velocity;
		}
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
		}
		public ParticleSurface(Surface surface, Rectangle clip, float x, float y, int life)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
			this.X = x;
			this.Y = y;
			this.Life = life;
		}
		public ParticleSurface(Surface surface, Rectangle clip)
		{
			m_ClipRectangle = clip;
			m_Surface = surface;
		}

		public override void Render(Surface destination)
		{
			destination.Blit(m_Surface, new Point((int)this.X, (int)this.Y), m_ClipRectangle);
		}

	}
}
