using System;
using System.Drawing;

namespace SdlDotNet.Particles
{
	/// <summary>
	/// Summary description for ParticleBoundry.
	/// </summary>
	public class ParticleBoundry : IParticleManipulator
	{
		public ParticleBoundry()
		{
			m_Boundry = new RectangleF(0,0,0,0);
		}
		public ParticleBoundry(SizeF size)
		{
			m_Boundry = new RectangleF(0,0,size.Width,size.Height);
		}
		public ParticleBoundry(Size size)
		{
			m_Boundry = new RectangleF(0,0,size.Width,size.Height);
		}
		public ParticleBoundry(Rectangle rect)
		{
			m_Boundry = rect;
		}
		public ParticleBoundry(RectangleF rect)
		{
			m_Boundry = rect;
		}
		public ParticleBoundry(float x, float y, float width, float height)
		{
			m_Boundry = new RectangleF(x,y,width,height);
		}

		private RectangleF m_Boundry;

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

		public float Left
		{
			get
			{
				return m_Boundry.Left;
			}
		}

		public float Right
		{
			get
			{
				return m_Boundry.Right;
			}
		}

		public float Top
		{
			get
			{
				return m_Boundry.Top;
			}
		}

		public float Bottom
		{
			get
			{
				return m_Boundry.Bottom;
			}
		}

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

		public void Manipulate(ParticleCollection particles)
		{
			foreach(Particle p in particles)
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

		#endregion


	}
}
