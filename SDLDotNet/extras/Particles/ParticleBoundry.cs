using System;
using System.Drawing;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// A particle manipulator the keeps particles within a boundry.
	/// </summary>
	public class ParticleBoundry : IParticleManipulator
	{
		/// <summary>
		/// Create a ParticleBoundry with an empty boundry.
		/// </summary>
		public ParticleBoundry()
		{
			m_Boundry = new RectangleF(0,0,0,0);
		}
		/// <summary>
		/// Create a ParticleBoundry from a given size.
		/// </summary>
		/// <param name="size"></param>
		public ParticleBoundry(SizeF size)
		{
			m_Boundry = new RectangleF(0,0,size.Width,size.Height);
		}
		/// <summary>
		/// Create a ParticleBoundry from a given size.
		/// </summary>
		/// <param name="size">The width and height of the boundry.</param>
		public ParticleBoundry(Size size)
		{
			m_Boundry = new RectangleF(0,0,size.Width,size.Height);
		}
		/// <summary>
		/// Create a ParticleBoundry in the given rectangle boundry.
		/// </summary>
		/// <param name="rect">The rectangle representing the boundry.</param>
		public ParticleBoundry(Rectangle rect)
		{
			m_Boundry = rect;
		}
		/// <summary>
		/// Create a ParticleBoundry from a given rectangle boundry.
		/// </summary>
		/// <param name="rect">The rectangle representing the boundry.</param>
		public ParticleBoundry(RectangleF rect)
		{
			m_Boundry = rect;
		}
		/// <summary>
		/// Create a ParticleBoundry from a given bounds.
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">The width of the boundry.</param>
		/// <param name="height">The height of the boundry.</param>
		public ParticleBoundry(float x, float y, float width, float height)
		{
			m_Boundry = new RectangleF(x,y,width,height);
		}
		/// <summary>
		/// Create a ParticleBoundry from a given size.
		/// </summary>
		/// <param name="width">The width of the boundry.</param>
		/// <param name="height">The height of the boundry.</param>
		public ParticleBoundry(float width, float height) : this(0,0,width,height)
		{
		}

		private RectangleF m_Boundry;
		/// <summary>
		/// Gets and sets the boundry rectangle.
		/// </summary>
		public RectangleF Boundry
		{
			get
			{
				return m_Boundry;
			}
			set
			{
				m_Boundry = value;
			}
		}

		/// <summary>
		/// Gets and set the x-coordinate of the upper-left corner of the rectangle.</param>
		/// </summary>
		public float X
		{
			get
			{
				return m_Boundry.X;
			}
			set
			{
				m_Boundry.X = value;
			}
		}

		/// <summary>
		/// Gets and sets the y-coordinate of the upper-left corner of the rectangle.</param>
		/// </summary>
		public float Y
		{
			get
			{
				return m_Boundry.Y;
			}
			set
			{
				m_Boundry.Y = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Left
		{
			get
			{
				return m_Boundry.Left;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Right
		{
			get
			{
				return m_Boundry.Right;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Top
		{
			get
			{
				return m_Boundry.Top;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Bottom
		{
			get
			{
				return m_Boundry.Bottom;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Width
		{
			get
			{
				return m_Boundry.Width;
			}
			set
			{
				m_Boundry.Width = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public float Height
		{
			get
			{
				return m_Boundry.Height;
			}
			set
			{
				m_Boundry.Height = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SizeF Size
		{
			get
			{
				return m_Boundry.Size;
			}
			set
			{
				m_Boundry.Size = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public PointF Location
		{
			get
			{
				return m_Boundry.Location;
			}
			set
			{
				m_Boundry.Location = value;
			}
		}


		#region IParticleManipulator Members

		/// <summary>
		/// Makes sure that every particle is within the given boundry.
		/// </summary>
		/// <param name="particles">The particle collection to set inside the bounds.</param>
		/// <remarks>Particles that reach the outside the rectangle are bounced back into bounds.</remarks>
		public void Manipulate(ParticleCollection particles)
		{
			foreach(Particle p in particles)
			{
				if(p is ParticleEmitter)
				{
					Manipulate(((ParticleEmitter)p).Children);
				}
				else
				{
					if(p.X < this.X)
					{
						p.X = this.X;
						p.Velocity.X*=-1;
					}
					else if(p.X > this.Right)
					{
						p.X = this.Right;
						p.Velocity.X*=-1;
					}
					else if(p.Y < this.Y)
					{
						p.Y = this.Y;
						p.Velocity.Y*=-1;
					}
					else if(p.Y > this.Bottom)
					{
						p.Y = this.Bottom;
						p.Velocity.Y*=-1;
					}
				}
			}
		}

		#endregion


	}
}
