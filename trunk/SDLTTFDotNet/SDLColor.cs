using System.Runtime.InteropServices;

namespace SDLDotNet
{
	/* This struct really should be part of the base SDLDotNet lib
		or possibly replaced with System.Drawing.Color */

	[StructLayout(LayoutKind.Sequential)]
	public struct SDLColor {
		public byte	r;
		public byte	g;
		public byte	b;
		public byte	unused;

		public SDLColor(byte R, byte G, byte B)
		{
			r = R;
			g = G;
			b = B;
			unused = 0;
		}

		static public SDLColor Red
		{
			get { return new SDLColor(255,0,0); }
		}

		static public SDLColor Green
		{
			get { return new SDLColor(0,255,0); }
		}

		static public SDLColor Blue
		{
			get { return new SDLColor(0,0,255); }
		}

		static public SDLColor MediumPurple
		{	
			get { return new SDLColor(147, 112, 219); }
		}

		static public SDLColor Orange
		{	
			get { return new SDLColor(255, 165, 0); }
		}

		/* TODO: A few more colours */
	}
}
