//*****************************************************************************
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
//*****************************************************************************

using System;

namespace SdlDotNetExamples.Triad
{
    /// <summary>
    /// 
    /// </summary>
    public enum BlockType
    {
        /// <summary>
        /// 
        /// </summary>
        Red,
        /// <summary>
        /// 
        /// </summary>
        Purple,
        /// <summary>
        /// 
        /// </summary>
        White,
        /// <summary>
        /// 
        /// </summary>
        Yellow,
        /// <summary>
        /// 
        /// </summary>
        Blue
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BlockGridState
    {
        /// <summary>
        /// 
        /// </summary>
        MoveTriad,
        /// <summary>
        /// 
        /// </summary>
        MarkBlocksToDestroy,
        /// <summary>
        /// 
        /// </summary>
        ShowBlocksDestroyed,
        /// <summary>
        /// 
        /// </summary>
        ReduceGrid,
        /// <summary>
        /// 
        /// </summary>
        CreateTriad,
        /// <summary>
        /// 
        /// </summary>
        PauseGame,
        /// <summary>
        /// 
        /// </summary>
        GameOver
    }
}