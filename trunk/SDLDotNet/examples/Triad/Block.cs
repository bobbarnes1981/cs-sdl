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
	public class Block : GameObject, System.IDisposable
	{
		/// <summary>
		/// 
		/// </summary>
		public const int BlockWidth = 36;
		/// <summary>
		/// 
		/// </summary>
		public readonly static Size BlockSize = new System.Drawing.Size(BlockWidth,BlockWidth);
		BlockType blockType;
		/// <summary>
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
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

		private bool destroy;

		/// <summary>
		/// 
		/// </summary>
		public bool Destroy
		{
			get
			{
				return destroy;
			}
			set
			{
				destroy = value;
			}
		}
	
		static Surface redBlock;
		Surface getRedBlock()
		{
			if(redBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/redBlock.bmp");
				redBlock = new Surface(bmp);
			}

			return redBlock;
		}

		static Surface whiteBlock;
		Surface getWhiteBlock()
		{
			if(whiteBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/whiteBlock.bmp");
				whiteBlock = new Surface(bmp);
			}

			return whiteBlock;
		}
		

		static Surface yellowBlock;
		Surface getYellowBlock()
		{
			if(yellowBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/yellowBlock.bmp");
				yellowBlock = new Surface(bmp);
			}

			return yellowBlock;
		}


		static Surface purpleBlock;
		Surface getPurpleBlock()
		{
			if(purpleBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/purpleBlock.bmp");
				purpleBlock = new Surface(bmp);
			}

			return purpleBlock;
		}


		static Surface blueBlock;
		Surface getBlueBlock()
		{
			if(blueBlock==null)
			{
				Bitmap bmp = new System.Drawing.Bitmap("../../Data/blueBlock.bmp");
				blueBlock = new Surface(bmp);
			}

			return blueBlock;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		protected override void DrawGameObject(Surface surface)
		{

			Surface image;
			
			switch(blockType)
			{
				case BlockType.Purple:	image = this.getPurpleBlock();	break;
				case BlockType.Red:		image = this.getRedBlock();		break;
				case BlockType.White:	image = this.getWhiteBlock();	break;
				case BlockType.Yellow:	image = this.getYellowBlock();	break;
				case BlockType.Blue:	image = this.getBlueBlock();	break;
				default: image = this.getBlueBlock(); break;
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
				{
					surface.Blit(image,this.ScreenRectangle);	
				}
				//surface.FillRect(this.ScreenRectangle, currentColor);
			}
			else
			{
				surface.Fill(this.ScreenRectangle,Color.SlateGray);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Update()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			this.Parent = null;			
		}


	}
}
