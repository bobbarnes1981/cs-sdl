using System.Runtime.InteropServices;

namespace SDLDotNet
{
	/* This struct really should be part of the base SDLDotNet lib
		or possibly replaced with System.Drawing.Color */

	/// <summary>
	/// 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SDLColor {
		/// <summary>
		/// 
		/// </summary>
		public byte	r;
		/// <summary>
		/// 
		/// </summary>
		public byte	g;
		/// <summary>
		/// 
		/// </summary>
		public byte	b;
		/// <summary>
		/// 
		/// </summary>
		public byte	unused;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <param name="G"></param>
		/// <param name="B"></param>
		public SDLColor(byte R, byte G, byte B)
		{
			r = R;
			g = G;
			b = B;
			unused = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		static public SDLColor Red
		{
			get { return new SDLColor(255,0,0); }
		}

		/// <summary>
		/// 
		/// </summary>
		static public SDLColor Green
		{
			get { return new SDLColor(0,255,0); }
		}

		/// <summary>
		/// 
		/// </summary>
		static public SDLColor Blue
		{
			get { return new SDLColor(0,0,255); }
		}

		/// <summary>
		/// 
		/// </summary>
		static public SDLColor MediumPurple
		{	
			get { return new SDLColor(147, 112, 219); }
		}

		/// <summary>
		/// 
		/// </summary>
		static public SDLColor Orange
		{	
			get { return new SDLColor(255, 165, 0); }
		}

		/* TODO: A few more colours */
	}
}
