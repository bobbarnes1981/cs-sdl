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
	/// <summary>
	/// 
	/// </summary>
	public class MainObject
	{
		private bool _quitflag;

		/// <summary>
		/// 
		/// </summary>
		public MainObject() 
		{
			_quitflag = false;
		}
		
		BlockGrid grid = null;
		ScoreBoard board = null;
		
		Sound levelUpSound = null;

		/// <summary>
		/// 
		/// </summary>
		public void Go() 
		{
			DateTime startTime = DateTime.Now;

			int width = 800;
			int height = 600;
			
			Video.WindowCaption = "Triad";
			Events.KeyboardDown += new KeyboardEventHandler(this.SDL_KeyboardDown); 
			Events.KeyboardUp += new KeyboardEventHandler(this.SDL_KeyboardUp); 
			
			
			Events.MouseButtonDown += new MouseButtonEventHandler(Events_MouseButton);
			Events.Quit += new QuitEventHandler(this.SDL_Quit);

			
			board = new ScoreBoard();
			board.X = 600;
			board.Y = 0;
			board.Size = new Size(200,400);

			try 
			{
				Surface screen = Video.SetVideoModeWindow(width, height, true);
				Surface surf = screen.CreateCompatibleSurface(width, height, true);
				surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
				Rectangle screenRec = new Rectangle(0,0,screen.Width,screen.Height);

				grid = new BlockGrid(new Point(20,20),new Size(11,13));
				grid.BlocksDestroyed += new BlocksDestroyedEventHandler(grid_BlocksDestroyed);


				levelUpSound = Mixer.Sound("../../Data/levelup.wav");
				
				while (!_quitflag) 
				{
					while (Events.Poll()) {} 

					//Clear and draw the space for the grid...
					surf.Fill(grid.Rectangle, Color.Black); 
					grid.Update();
					grid.Draw(surf);
				
					//Clear and draw the space for the score board...
					surf.Fill(board.Rectangle, Color.Black); 
					board.Update();
					board.Draw(surf);

					//Blit the grid and the board to the screen surface...
					screen.Blit(surf, board.Rectangle);
					screen.Blit(surf, grid.Rectangle);						
			
					screen.Flip();

				}
			} 
			catch 
			{
				throw; 
			}
		}

		private void SDL_KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape || e.Key == Key.Q)
			{
				_quitflag = true;
			}
	
			grid.HandleSDLKeyDownEvent(e);
		}

		private void SDL_KeyboardUp(object sender, KeyboardEventArgs e) 
		{
			grid.HandleSDLKeyUpEvent(e);
		}

		private void SDL_Quit(object sender, QuitEventArgs e) 
		{
			_quitflag = true;
		}

		[STAThread]
		static void Main() 
		{
			MainObject s = new MainObject();
			s.Go();
		}

		private void Events_MouseButton(object sender, MouseButtonEventArgs e)
		{
		}

		int blockCount = 0;
		private void grid_BlocksDestroyed(object sender, BlockDestroyedEventArgs args)
		{
			this.blockCount += args.BlocksCount;
			if(blockCount > 30)
			{
				this.blockCount = 0;
				this.grid.SpeedFactor = grid.SpeedFactor * 1.025f;
				this.board.Level += 1;
				//GC.Collect();
				this.levelUpSound.Play();
				
			}
			
			this.board.BlocksDestroyed += args.BlocksCount;
			this.board.Score += args.BlocksCount * 100 * args.ReductionCount;
		}
	}
}