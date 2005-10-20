using System;
using System.Drawing;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleEmitter.
	/// </summary>
	public delegate Particle AddParticleEvent(ParticleEmitter sender, EventArgs e);
	public class ParticleEmitter : Particle
	{
		private ParticleCollection m_Children = new ParticleCollection();
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
		public ParticleEmitter()
		{
		}
		public event AddParticleEvent AddParticle;


		private bool m_Static = true;
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

		public override void Render(Surface destination)
		{
			m_Children.Render(destination);
		}

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
