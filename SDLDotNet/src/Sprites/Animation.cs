using System;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	public class Animation : SurfaceCollection
	{
		/// <summary>
		/// 
		/// </summary>
		public Animation()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surfaces"></param>
		public Animation(SurfaceCollection surfaces) : this()
		{
			this.Add(surfaces);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public Animation(Surface surface) : this()
		{
			this.Add(surface);
		}

		private double m_Delay = 800;
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		public bool Loop
		{
			get
			{ 
				return m_Loop;
			}
			set
			{ 
				m_Loop = value; 
			}
		}
	}
}
