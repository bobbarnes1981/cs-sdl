/*
 * SdlButton by Jon B. Stefansson
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Drawing;

using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Event handler delegate fyrir SdlButton
	/// </summary>
	public delegate void SdlButtonEventHandler(object source, SdlButtonEventArgs e);

	/// <summary>
	/// Event args fyrir SdlButton
	/// </summary>
	public class SdlButtonEventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public int Xpos;
		/// <summary>
		/// 
		/// </summary>
		public int Ypos;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public SdlButtonEventArgs(int x, int y)
		{
			Xpos = x;
			Ypos = y;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class SdlButton
	{
		/// <summary>
		/// Event onClick
		/// </summary>
		public event SdlButtonEventHandler Click;

		private int x; //X position
		private int y; //Y position
		private int textX; //X position of text
		private int textY; //Y position of text
		private int width;
		private int height;
		private string buttonText; 

		private Font buttonFont; //font

		private Color buttonColor; //Color of the button

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int Height
		{
			get { return height; }
			set { height = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get { return x; }
			set { x = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get { return y; }
			set { y = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get { return buttonText; }
			set { buttonText = value; }
		}
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public SdlButton(int xPos, int yPos, int Width, int Height, Color color, string text)
		{
			x = xPos;
			y = yPos;
			width = Width;
			height = Height;

			buttonColor = color;
			buttonText = text;

			buttonFont = new SdlDotNet.Font("../../Vera.ttf", 12);
			//Events
			Events.MouseButtonDown += new MouseButtonEventHandler(Events_MouseButtonDown);
		}

		/// <summary>
		/// Draw the button
		/// </summary>
		/// <param name="surf"></param>
		public void Draw(Surface surf)
		{
			textX = x + ((width/2) - (buttonText.Length*3));
			textY = y + ((height/2) - 10);
			surf.Fill(new Rectangle(new Point(x,y), new Size(width, height)), buttonColor);

			Surface fontSurface = buttonFont.Render(buttonText, Color.Black);
			surf.Blit(fontSurface, new System.Drawing.Point(textX, textY));
		}

		/// <summary>
		/// On Mouse Down
		/// </summary>
		private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			//If the mouse is over the button
			if(e.X > x && e.X < x+width)
			{
				if(e.Y > y && e.Y < y+height)
				{
					//FIRE the event
					SdlButtonEventArgs args = new SdlButtonEventArgs(e.X, e.Y);
					if(this.Click != null)
						Click(this, args); 
				}
			}
		}
	}
}