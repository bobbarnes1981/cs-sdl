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
	public class Triad : GameObject,System.IDisposable
	{
		public Triad(BlockGrid blockGridObject)
		{
			this.topBlock = new Block();
			this.middleBlock = new Block();
			this.bottomBlock = new Block();
			this.blockGrid = blockGridObject;

			int startColumn = blockGrid.GridSize.Width/2;
			this.X = startColumn*Block.BlockWidth;
			this.Y = 0;

		}

		void placeBlocks()
		{
			this.topBlock.X = this.X;
			this.topBlock.Y = this.Y;
			this.middleBlock.X = this.X;
			this.middleBlock.Y = this.Y+Block.BlockWidth;
			this.bottomBlock.X	 = this.X;
			this.bottomBlock.Y = this.Y+(Block.BlockWidth*2);

		}
		
		Block topBlock;
		Block middleBlock;
		Block bottomBlock;
		BlockGrid blockGrid;
		public Block TopBlock
		{
			get
			{
              return topBlock;   
			}
		}

		public Block MiddleBlock
		{
			get
			{
				return middleBlock;   
			}
		}

		public Block BottomBlock
		{
			get
			{
				return bottomBlock;   
			}
		}

		protected override void DrawGameObject(Surface surface)
		{
			this.topBlock.Parent = this.Parent;
			this.middleBlock.Parent = this.Parent;
			this.bottomBlock.Parent = this.Parent;

			topBlock.Draw(surface);
			middleBlock.Draw(surface);
			bottomBlock.Draw(surface);
		}


		static int halfOfBlock = 0;
		bool canMoveLeftRightBy(int deltaX)
		{
			 if(halfOfBlock==0)
				 halfOfBlock = (Block.BlockWidth/2);

			//Calc three points to represent the position of the tree blocks of the Triad...
			Point newPoint  = new Point(this.ScreenLocation.X + deltaX + halfOfBlock,this.ScreenLocation.Y+ halfOfBlock);
			Point newPoint2  = new Point(this.ScreenLocation.X + deltaX + halfOfBlock,this.ScreenLocation.Y+Block.BlockWidth+ halfOfBlock);
			Point newPoint3  = new Point(this.ScreenLocation.X + deltaX + halfOfBlock,this.ScreenLocation.Y+Block.BlockWidth+Block.BlockWidth+ halfOfBlock);
			

			bool isInsideBlockGrid = blockGrid.Contains(newPoint);

			bool isInsideBlockGridChildren = false;
			foreach(GameObject obj in blockGrid.GameObjectList)
			{
				if(obj.Contains(newPoint) || obj.Contains(newPoint2) ||obj.Contains(newPoint3) )
				{
					isInsideBlockGridChildren = true;
					break;
                }
				
                
			}

			return isInsideBlockGrid && !isInsideBlockGridChildren;

																 


		}
		

		public void MoveLeft()
		{
			int deltaX = -Block.BlockWidth;
			if(canMoveLeftRightBy(deltaX))
				this.X -= Block.BlockWidth;

		}

		public void MoveRight()
		{
			int deltaX = Block.BlockWidth;
			if(canMoveLeftRightBy(deltaX))
				this.X += Block.BlockWidth;

		}

		public bool CanMoveDown()
		{
			return canMoveDown(Block.BlockWidth);
		}


		bool canMoveDown(int deltaY)
		{
			if(halfOfBlock==0)
				halfOfBlock = (Block.BlockWidth/2);

			Point newPoint  = new Point(this.ScreenLocation.X  + halfOfBlock,this.ScreenLocation.Y+Block.BlockWidth+Block.BlockWidth+ halfOfBlock + deltaY);
			

			bool isInsideBlockGrid = blockGrid.Contains(newPoint);

			bool isInsideBlockGridChildren = false;
			foreach(GameObject obj in blockGrid.GameObjectList)
			{
				if(obj.Contains(newPoint))
				{
					isInsideBlockGridChildren = true;
					break;
				}
				
                
			}

			return isInsideBlockGrid && !isInsideBlockGridChildren;
		}

		public void MoveDown()
		{
			if(canMoveDown(Block.BlockWidth))
				this.Y += Block.BlockWidth;
		}

		public void Permute()
		{
            Block tempBlock = this.bottomBlock;
			this.bottomBlock = this.middleBlock;
			this.middleBlock = this.topBlock;
			this.topBlock = tempBlock;
			
		}

		public override void Update()
		{
			placeBlocks();
		}

		public void Dispose()
		{	
			this.blockGrid = null;
			this.bottomBlock = null;
			this.middleBlock = null;
			this.Parent = null;
			this.topBlock = null;
			

		}
	}
}
