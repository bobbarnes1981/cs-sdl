/* This file is part of SnowDemo
 * Graphics.cs, (c) 2003 Sijmen Mulder
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 
 ****************************************************************************/

using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Examples
{	
	/// <summary>
	/// 
	/// </summary>
	public sealed class Graphics
	{
		static Surface screen;
		static Surface background;
		
		private Graphics() 
		{
		}
		
		/// <summary>
		/// 
		/// </summary>
		public static void Init()
		{
			screen = Video.SetVideoModeWindow(640, 480, 16, true);
			background = Graphics.LoadImage("../../Data/background.png", Color.FromArgb(255, 0, 255));
			Video.WindowCaption = "SdlDotNet - Snow Demo";
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="colorKey"></param>
		/// <returns></returns>
		public static Surface LoadImage(string filename, Color colorKey)
		{
			Surface temp1 = new Surface(filename);
			Surface temp2 = temp1.Convert();
			
			temp1.Dispose();
			temp2.SetColorKey(colorKey, true);
			
			return temp2;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static Surface LoadText(string filename)
		{
			Surface temp1 = LoadImage(filename, Color.White);
			Surface temp2 = temp1.CreateCompatibleSurface(temp1.Width + 3,
				temp1.Height + 3, true);
			
			temp2.Fill(new Rectangle(new Point(0, 0), temp2.Size),
				Color.FromArgb(255, 0, 255));
			
			temp2.Fill(new Rectangle(new Point(3, 3), temp1.Size), 
				Color.FromArgb(48, 130, 179));
			temp2.Blit(temp1, new Rectangle(new Point(3, 3), temp1.Size));
			
			temp1.SetColorKey(Color.FromArgb(255, 0, 255), true);
			temp2.Blit(temp1, new Rectangle(new Point(0, 0), temp1.Size));
			
			temp2.SetColorKey(Color.FromArgb(255, 0, 255), true);
			temp1.Dispose();
			
			return temp2;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public static void DrawFrame()
		{
			screen.Fill(new Rectangle(new Point(0, 0), screen.Size),
				Color.FromArgb(64, 175, 239));
			
			for(int i = 0; i < SnowDemo.NumberOfSnowflakes; i++)
			{
				screen.Blit(SnowDemo.GetSnowflakes()[i].Image, SnowDemo.GetSnowflakes()[i].Position);
			}
			
			screen.Blit(background, new Rectangle(new Point(0, 280), background.Size));
			
			for(int i = 0; i < SnowDemo.Texts.Length; i++)
			{
				screen.Blit(SnowDemo.Texts[i].Image, SnowDemo.Texts[i].Position);
			}
			screen.Flip();
		}
	}
}
