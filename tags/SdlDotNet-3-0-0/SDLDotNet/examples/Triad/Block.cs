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
	public class Block : GameObject, System.IDisposable
	{
		public readonly static int BlockWidth = 36;
		public readonly static Size BlockSize = new System.Drawing.Size(BlockWidth,BlockWidth);
		BlockType blockType;
		public BlockType TypeOfBlock
		{
			set
			{
                blockType = value;
			}

			get
			{
                return blockType;
			}

		}
		static Random random;

		public Block()
		{
			this.Size = BlockSize;

			if(random==null)
			{
				random = new Random(DateTime.Now.Millisecond);
			}
			
			blockType = (BlockType)random.Next(5);
		}

		private int _GridX;
		public int GridX
		{
			get
			{
				return _GridX;
			}
			set
			{
				_GridX = value;				
			}
		}
	
		private int _GridY;
		public int GridY
		{
			get
			{
				return _GridY;
			}
			set
			{
				_GridY = value;				
			}
		}

		public Point GridLocation
		{
			get
			{
				return new Point(_GridX,_GridY);
			}
			set
			{
                _GridX = value.X;
				_GridY = value.Y;
			}

		}

		public bool Destroy = false;
	


		static Surface redBlock = null;
		Surface getRedBlock()
		{
			if(redBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/redBlock.bmp");
				redBlock = new Surface(bmp);
			}

			return redBlock;
		}

		static Surface whiteBlock = null;
		Surface getWhiteBlock()
		{
			if(whiteBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/whiteBlock.bmp");
				whiteBlock = new Surface(bmp);
			}

			return whiteBlock;
		}
		

		static Surface yellowBlock = null;
		Surface getYellowBlock()
		{
			if(yellowBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/yellowBlock.bmp");
				yellowBlock = new Surface(bmp);
			}

			return yellowBlock;
		}


		static Surface purpleBlock = null;
		Surface getPurpleBlock()
		{
			if(purpleBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/purpleBlock.bmp");
				purpleBlock = new Surface(bmp);
			}

			return purpleBlock;
		}


		static Surface blueBlock = null;
		Surface getBlueBlock()
		{
			if(blueBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/blueBlock.bmp");
				blueBlock = new Surface(bmp);
			}

			return blueBlock;
		}

		protected override void DrawGameObject(Surface surface)
		{

			Surface image = null;
			
			switch(blockType)
			{
				case BlockType.Purple:	image = this.getPurpleBlock();	break;
				case BlockType.Red:		image = this.getRedBlock();		break;
				case BlockType.White:	image = this.getWhiteBlock();	break;
				case BlockType.Yellow:	image = this.getYellowBlock();	break;
				case BlockType.Blue:	image = this.getBlueBlock();	break;
			}

		
//			Color currentColor = Color.Black;
//			switch(blockType)
//			{
//				case BlockType.Purple:	currentColor = Color.Purple;	break;
//				case BlockType.Red:		currentColor = Color.Red;	break;
//				case BlockType.White:	currentColor = Color.White;	break;
//				case BlockType.Yellow:	currentColor = Color.Yellow;	break;
//				case BlockType.Blue:	currentColor = Color.Blue;	break;
//			}

			if(!this.Destroy)
			{
				if(image != null)
					surface.Blit(image,this.ScreenRectangle);	
				//surface.FillRect(this.ScreenRectangle, currentColor);
			}
			else
			{
                surface.Fill(this.ScreenRectangle,Color.SlateGray);
			}
		}

		public override void Update()
		{

		}

		public void Dispose()
		{
			this.Parent = null;			
		}


	}
}
