/* This file is part of SnowDemo
* Text.cs, (c) 2003 Sijmen Mulder
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
****************************************************************************/

using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Examples
{
	enum TextFadeState
	{
		BeforeFadeIn,
		FadeIn,
		BeforeFadeOut,
		FadeOut,
		Finished
	}

	public class Text
	{
		Rectangle _Position;
		Surface _Image = null;

		const float inspeed = 510;
		const float outspeed = 64;

		float time = 0;
		float starttime;
		float endtime;

		TextFadeState state = TextFadeState.BeforeFadeIn;
		float alpha = 0;

		public Text(int Num, int Y)
		{
			if(_Image == null)
			{
				_Image = Graphics.LoadText(string.Format("../../Data/Text{0}.bmp", Num));
			}

			_Position = new Rectangle(25, Y, _Image.Width, _Image.Height);

			starttime = Num * 2;
			endtime = starttime + 4.5f;

			_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, 0);
		}

		public void Update(float Seconds)
		{
			time += Seconds;

			switch(state)
			{
				case TextFadeState.BeforeFadeIn:

					if(time >= starttime)
						state = TextFadeState.FadeIn;
					break;

				case TextFadeState.FadeIn:

					alpha += Seconds * inspeed;

					if(alpha >= 255)
					{
						alpha = 255;
						state = TextFadeState.BeforeFadeOut;
					}

					_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, (byte)alpha);
					break;

				case TextFadeState.BeforeFadeOut:

					if(time >= endtime)
						state = TextFadeState.FadeOut;
					break;

				case TextFadeState.FadeOut:

					alpha -= Seconds * outspeed;

					if(alpha <= 0)
					{
						alpha = 0;
						state = TextFadeState.Finished;
					}

					_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, (byte)alpha);
					break;
			}
		}

		public Rectangle Position{ get{ return _Position; }}
		public Surface Image{ get{ return _Image; }}
	}

	public class Texts
	{
		Text[] texts = new Text[6];

		public Texts()
		{
			texts[0] = new Text(0, 25);

			for(int i = 1; i < texts.Length; i++)
				texts[i] = new Text(i, texts[i-1].Position.Bottom + 10);
		}

		public int Length
		{
			get
			{
				return texts.Length;
			}
		}

		public Text this[int Index]
		{
			get
			{
				return texts[Index];
			}
		}
	}
}
