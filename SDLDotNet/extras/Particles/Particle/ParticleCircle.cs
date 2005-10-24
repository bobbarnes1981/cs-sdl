using System;

namespace SdlDotNet.Particles.Particle
{
	/// <summary>
	/// A particle represented by a circle.
	/// </summary>
	public class ParticleCircle : ParticlePixel
	{
		/// <summary>
		/// Creates a particle represented by a circle with the default values.
		/// </summary>
		public ParticleCircle()
		{
		}
		/// <summary>
		/// Creates a particle represented by a circle with a set radius.
		/// </summary>
		/// <param name="radius"></param>
		public ParticleCircle(short radius)
		{
			m_Radius = radius;
		}
		private short m_Radius = 1;
		/// <summary>
		/// Gets and sets the radius of the particles.
		/// </summary>
		public short Radius
		{
			get
			{
				return m_Radius;
			}
			set
			{
				m_Radius = value;
			}
		}
		

		/// <summary>
		/// Draws the particle on the destination surface represented by a circle.
		/// </summary>
		/// <param name="destination">The destination surface where to draw the particle.</param>
		public override void Render(Surface destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if(this.LifeFull != -1)
			{
				float alpha;
				if(this.Life >= this.LifeFull)
					alpha = 255;
				else if (this.Life <= 0)
					alpha = 0;
				else
					alpha = ((float)this.Life / this.LifeFull) * 255F;

				destination.DrawFilledCircle(
					new Circle((short)this.X, (short)this.Y, m_Radius), 
					System.Drawing.Color.FromArgb((int)alpha, this.Color));
			}
			else
			{
				destination.DrawFilledCircle(
					new Circle((short)this.X, (short)this.Y, m_Radius), 
					this.Color);
			}
		}
	}
}
