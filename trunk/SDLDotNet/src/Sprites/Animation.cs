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

		private double m_Delay = 30;
		/// <summary>
		/// Gets and sets the amount of time delay that should be had before moving onto the next frame.
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

        /// <summary>
        /// Gets and sets how long the animation should take to finish.
        /// </summary>
        public double AnimationTime
        {
            get
            {
                return m_Delay * this.Count;
            }
            set 
            {
                m_Delay = this.Count / value;
            }
        }


		private bool m_Loop = true;
		/// <summary>
		/// Gets and sets whether or not the animation should loop.
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
