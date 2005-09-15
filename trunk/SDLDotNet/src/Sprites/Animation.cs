using System;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	public class Animation : SurfaceCollection
	{
		public Animation()
		{
		}

		public Animation(SurfaceCollection surfaces) : this()
		{
			this.Add(surfaces);
		}

		public Animation(Surface surface) : this()
		{
			this.Add(surface);
		}


		private double m_Delay = 800;
		public double Delay
		{
			get
			{
				return m_Delay;
			}
			set
			{
				m_Delay = value;
			}
		}


		private bool m_Loop = true;
		public bool Loop
		{
			get{ return m_Loop; }
			set{ m_Loop = value; }
		}
	}
}
