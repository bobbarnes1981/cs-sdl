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
using System.Globalization;

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

	/// <summary>
	/// 
	/// </summary>
	public class TextItem
	{
		Rectangle _Position;
		Surface _Image;

		const float inspeed = 510;
		const float outspeed = 64;

		float time;
		float starttime;
		float endtime;

		TextFadeState state = TextFadeState.BeforeFadeIn;
		float alpha;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <param name="y"></param>
		public TextItem(int number, int y)
		{
			if(_Image == null)
			{
				_Image = 
					Graphics.LoadText(string.Format(CultureInfo.CurrentCulture,"../../Data/Text{0}.bmp", number));
			}

			_Position = new Rectangle(25, y, _Image.Width, _Image.Height);

			starttime = number * 2;
			endtime = starttime + 4.5f;

			_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seconds"></param>
		public void Update(float seconds)
		{
			time += seconds;

			switch(state)
			{
				case TextFadeState.BeforeFadeIn:
					if(time >= starttime)
					{
						state = TextFadeState.FadeIn;
					}
					break;

				case TextFadeState.FadeIn:
					if (seconds <= (float.MaxValue / inspeed) - alpha)
					{
						alpha += seconds * inspeed;
					}
					else
					{
						alpha = float.MaxValue;
					}

					if(alpha >= 255)
					{
						alpha = 255;
						state = TextFadeState.BeforeFadeOut;
					}

					_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, (byte)alpha);
					break;

				case TextFadeState.BeforeFadeOut:
					if(time >= endtime)
					{
						state = TextFadeState.FadeOut;
					}
					break;

				case TextFadeState.FadeOut:
					alpha -= seconds * outspeed;

					if(alpha <= 0)
					{
						alpha = 0;
						state = TextFadeState.Finished;
					}

					_Image.SetAlpha(Alphas.SourceAlphaBlending | Alphas.RleEncoded, (byte)alpha);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Rectangle Position
		{ 
			get
			{ 
				return _Position; 
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Surface Image
		{ 
			get
			{ 
				return _Image; 
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class Texts
	{
		TextItem[] texts = new TextItem[6];

		/// <summary>
		/// 
		/// </summary>
		public Texts()
		{
			texts[0] = new TextItem(0, 25);

			for(int i = 1; i < texts.Length; i++)
			{
				texts[i] = new TextItem(i, texts[i-1].Position.Bottom + 10);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Length
		{
			get
			{
				return texts.Length;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public TextItem this[int index]
		{
			get
			{
				return texts[index];
			}
		}
	}
}
