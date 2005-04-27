// Copyright 2005 David Hudson (jendave@yahoo.com)
// This file is part of SimpleGame.
//
// SimpleGame is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// SimpleGame is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SimpleGame; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using SdlDotNet;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// 
	/// </summary>
	public enum Direction
	{
		/// <summary>
		/// 
		/// </summary>
		Up,
		/// <summary>
		/// 
		/// </summary>
		Down,
		/// <summary>
		/// 
		/// </summary>
		Left,
		/// <summary>
		/// 
		/// </summary>
		Right
	}

	/// <summary>
	/// Derived class
	/// </summary>
	public class InputController
	{
		bool quitFlag = false;

		EventManager eventManager;
		/// <summary>
		/// constructor
		/// </summary>
		public InputController(EventManager eventManager)
		{
			this.eventManager = eventManager;
			this.eventManager.OnQuitEvent += new QuitEventHandler(Subscribe);
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eventManager"></param>
		/// <param name="e"></param>
		public void Subscribe(object eventManager, QuitEventArgs e)
		{
			LogFile.WriteLine("InputController received a Quit event");
			quitFlag = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void KeyboardDown(object sender, KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape || e.Key == Key.Q)
			{
				eventManager.Publish(new QuitEventArgs());
			}
			else if (e.Key == Key.UpArrow)
			{
				eventManager.Publish(new EntityMoveRequestEventArgs(Direction.Up));
			}
			else if (e.Key == Key.DownArrow)
			{
				eventManager.Publish(new EntityMoveRequestEventArgs(Direction.Down));
			}
			else if (e.Key == Key.LeftArrow)
			{
				eventManager.Publish(new EntityMoveRequestEventArgs(Direction.Left));
			}
			else if (e.Key == Key.RightArrow)
			{
				eventManager.Publish(new EntityMoveRequestEventArgs(Direction.Right));
			}
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Quit(object sender, QuitEventArgs e) 
		{
			eventManager.Publish(new QuitEventArgs());
		}
		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			try 
			{
				while (!quitFlag) 
				{
					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						eventManager.Publish(new TickEventArgs());
					} 
					catch (SurfaceLostException) 
					{
					}
				}
			} 
			catch 
			{
				throw; // for this example we'll just throw it to the debugger
			}
		}
	}
}
