/*
 * SdlTextBox by Jon B. Stefansson
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
	/// SdlTextBox by Jón Brynjar Stefánsson
	/// </summary>
	public class SdlTextBox
	{
		private int x; //X position
		private int y; //Y position
		private int length; //TextLength (bugged.. unfinshed really)
		//private bool mouseOver; //Is the mouse over the textbox
		private bool isEnabled; //Is the textbox active (has is been clicked)

		//ASCII code for the text
		private int code; 
		private int lastCode;

		//The text in the textbox
		private string boxText; 

		SdlDotNet.Font boxFont; //the font to use

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public int X { get { return x; } }
		/// <summary>
		/// 
		/// </summary>
		public int Y { get { return y; } }
		/// <summary>
		/// 
		/// </summary>
		public int Length{ get { return length; } }
		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get { return boxText; }
			set { boxText = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Enabled { get { return isEnabled; } }
		#endregion

		/// <summary>
		/// Constructor, Initialize variables
		/// </summary>
		/// <param name="posX">X position</param>
		/// <param name="posY">Y position</param>
		/// <param name="textLength">Textlength (in pixels :P)</param>
		public SdlTextBox(int posX, int posY, int textLength)
		{

			x = posX;
			y = posY;
			length = textLength;
			boxText = "";
			boxFont = new SdlDotNet.Font("../../Vera.ttf", 12); //font
			
			//Events
			Events.MouseButtonDown += new MouseButtonEventHandler(Events_MouseButtonDown);
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
		}

		/// <summary>
		/// If the mouse is over the textbox then true
		/// </summary>
		/// <param name="posX">mouse x pos</param>
		/// <param name="posY">mouse y pos</param>
		/// <returns></returns>
		public bool IsOver(int posX, int posY)
		{
			if(posX > x && posX < x+length)
			{
				if(posY > y && posY < y+20)
				{
					//mouseOver = true;
					return true;
				}
			}
			
			//mouseOver = false;
			return false;
		}

		/// <summary>
		/// If the textbox has been clicked then true
		/// </summary>
		/// <param name="posX">mouse x pos</param>
		/// <param name="posY">mouse y pos</param>
		/// <returns></returns>
		public bool IsHit(int posX, int posY)
		{
			if(posX > x && posY < x+length)
			{
				if(posX > y && posY < y+20)
				{
					isEnabled = true;
					return true;
				}
			}

			isEnabled = false;
			return false;
		}

		/// <summary>
		/// Draws the textbox
		/// </summary>
		/// <param name="surf"></param>
		public void Draw(Surface surf)
		{
			surf.Fill(new Rectangle(x, y, length, 20), Color.GhostWhite);
			if(boxText.Length > 0)
			{
				//boxFont.Render(boxText, Color.Black, surf, x, y);
				Surface fontSurface = boxFont.Render(boxText, Color.Black);
				surf.Blit(
					fontSurface,
					new System.Drawing.Point(x, y));
			}
		}

		/// <summary>
		/// Run when the mousebutton is clicked
		/// </summary>
		private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(this.IsOver(e.X, e.Y)) //If the mouse is over the textbox
			{
				this.IsHit(e.X, e.Y); //Enable the textbox
			}
			else
			{
				isEnabled = false;
			}
		}

		/// <summary>
		/// Run when the keyboard is down
		/// </summary>
		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			code = Convert.ToInt32(e.Key); //Get the ASCII code of the key that was pressed

			if(isEnabled && boxText.Length < length/7) //if the textbox is enabled and the text is not too long
			{
				if(code == 8) //BACKSPACE
				{
					if(boxText.Length > 0)
						boxText = boxText.Remove(boxText.Length-1, 1);
				}
				else if((code > 32 && code < 123))
				{
					if(lastCode == 303 || lastCode == 304) //SHIFT
						code -= 32; //Capitalize the letter

					boxText += Convert.ToString((char)code); //Add char to text
				}
				else if(code == 32) //SPACE
				{
					boxText += " ";
				}
				if(code >= 256 && code <= 265) //numbers
				{
					int temp = code - 256;

					boxText += temp.ToString();
				}

			}
			lastCode = code;
		}
	}
}