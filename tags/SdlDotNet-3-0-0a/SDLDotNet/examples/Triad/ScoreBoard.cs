//********************************************************************************		
//	This program is free software; you can redistribute it and/or
//	modify it under the terms of the GNU General Public License
//	as published by the Free Software Foundation; either version 2
//	of the License, or (at your option) any later version.
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//	You should have received a copy of the GNU General Public License
//	along with this program; if not, write to the Free Software
//	Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//	
//	Created by Michael Rosario
//	July 29th,2003
//	Contact me at mrosario@scrypt.net	
//********************************************************************************



using System;
using System.Drawing;
using SdlDotNet;

namespace SdlDotNet.Examples
{
	public class ScoreBoard : GameArea
	{
		SdlDotNet.Font font = null;
		public ScoreBoard()
		{
			font = new SdlDotNet.Font(@"C:\WINNT\Fonts\arial.ttf",18);
		}

		protected override void DrawGameObject(Surface surface)
		{
			int currentY = 0;

			Surface fontSurface = font.Render("Score: " + this._Score, Color.FromArgb(255,255,255));
			surface.Blit(fontSurface, new System.Drawing.Point(this.ScreenX, this.ScreenY + currentY));

			currentY+=20;
			fontSurface = font.Render("Blocks Destroyed: " + this._BlocksDestroyed, Color.FromArgb(255,255,255));
			surface.Blit(fontSurface, new System.Drawing.Point(this.ScreenX, this.ScreenY + currentY));

			currentY+=20;

			fontSurface = font.Render("Level: " + this._Level, Color.FromArgb(255,255,255));
			surface.Blit(fontSurface, new System.Drawing.Point(this.ScreenX, this.ScreenY + currentY));
		}


		public override void HandleSDLKeyDownEvent(KeyboardEventArgs args)
		{

		}

		public override void HandleSDLKeyUpEvent(KeyboardEventArgs args)
		{

		}

		private int _Score;
		public int Score
		{
			get
			{
				return _Score;
			}
			set
			{
				_Score = value;				
			}
		}
	
		private int _Level;
		public int Level
		{
			get
			{
				return _Level;
			}
			set
			{
				_Level = value;				
			}
		}

		private int _BlocksDestroyed;
		public int BlocksDestroyed
		{
			get
			{
				return _BlocksDestroyed;
			}
			set
			{
				_BlocksDestroyed = value;				
			}
		}
	}
}
