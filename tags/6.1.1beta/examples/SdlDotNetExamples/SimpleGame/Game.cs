#region LICENSE
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
#endregion LICENSE

using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

using SdlDotNet;
using SdlDotNet.Audio;
using SdlDotNet.Graphics;

namespace SdlDotNetExamples.SimpleGame
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Preparing,
        /// <summary>
        /// 
        /// </summary>
        Started,
        /// <summary>
        /// 
        /// </summary>
        Running,
        /// <summary>
        /// 
        /// </summary>
        Paused,
        /// <summary>
        /// 
        /// </summary>
        Stopped
    }

    /// <summary>
    /// Summary description for Game.
    /// </summary>
    public class Game
    {
        GameStatus gameStatus;
        EventManager eventManager;
        ArrayList players;
        Map map;
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventManager"></param>
        public Game(EventManager eventManager)
        {
            this.gameStatus = GameStatus.Preparing;
            this.eventManager = eventManager;
            this.players = new ArrayList();
            this.players.Add(new Player(eventManager));
            this.map = new Map(eventManager);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            GameView gameView = new GameView(eventManager);
            gameView.CreateView();
            map.Build();

            if (File.Exists(Path.Combine(dataDirectory, "fard-two.ogg")))
            {
                filePath = "";
            }

            Music music = new Music(Path.Combine(filePath, Path.Combine(dataDirectory, "fard-two.ogg")));

            try
            {
                music.Play(-1);
            }
            catch (DivideByZeroException)
            {
                // Linux audio problem
            }
           
            this.gameStatus = GameStatus.Started;
            eventManager.Publish(new GameStatusEventArgs(this, GameStatus.Started));
        }

        /// <summary>
        /// 
        /// </summary>
        public GameStatus GameStatus
        {
            get
            {
                return this.gameStatus;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Map Map
        {
            get
            {
                return this.map;
            }
        }
    }
}
