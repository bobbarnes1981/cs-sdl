

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
using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Examples
{
	public abstract class  GameObject
	{
		public GameObject()
		{

		}

		private GameObject _Parent;
		public GameObject Parent
		{
			get
			{
				return _Parent;
			}
			set
			{
				_Parent = value;				
			}
		}
	
		private int  _X;
		public int  X
		{
			get
			{
				return _X;
			}
			set
			{
				_X = value;				
			}
		}

		public int ScreenX
		{
			get
			{
                int x = this._X;
				if(this._Parent != null)
					x += this._Parent.ScreenX;
				return x;
			}
		}
	
		public int ScreenY
		{
			get
			{
				int y = this._Y;
				if(this._Parent != null)
					y += this._Parent.ScreenY;
				return y;
			}
		}
	
		private int  _Y;
		public int  Y
		{
			get
			{
				return _Y;
			}
			set
			{
				_Y = value;				
			}
		}

		private int  _Width;
		public int  Width
		{
			get
			{
				return _Width;
			}
			set
			{
				if(_Width <= 0)
					throw new GameException("Width is set to zero or negative value.");

				_Width = value;				
			}
		}
	

		private int  _Height;
		public int  Height
		{
			get
			{
				return _Height;
			}
			set
			{
				if(value <= 0)
					throw new GameException("Height is set to zero or negative value.");

				_Height = value;				
			}
		}
	
			
				
		private int  _X2=0;
		public int  X2
		{
			get
			{
				return _X + _Width;
			}
		}
	
		private int  _Y2=0;
		public int  Y2
		{
			get
			{
				return _Y + _Height;
			}
		}

		
		public int  ScreenX2
		{
			get
			{
				int offSetX = 0;
				if(this._Parent != null)
					offSetX = this._Parent.ScreenX;


				return this.X2 + offSetX;
			}
		}
	
		
		public int  ScreenY2
		{
			get
			{
				int offSetY = 0;
				if(this._Parent != null)
					offSetY = this._Parent.ScreenY;


				return _Y + _Height + offSetY;
			}
		}
		
		int _previousWidth = 0;
		int _previousHeight = 0;
		Size currentSize;
		public Size Size
		{
			get
			{
				if((_previousWidth != _Width )||(_previousHeight != _Height))
				{
					currentSize = new Size(_Width,_Height);					
				}

				_previousWidth = _Width;
				_previousHeight = _Height;

				return currentSize;
			}
			set
			{
				this._Width = value.Width;
				this._Height = value.Height;
			}
		}

		System.Drawing.Rectangle currentRectangle;
		Point previousLocation;
		public Rectangle Rectangle
		{
			get
			{


				if(previousLocation != Location)
				{
					currentRectangle = new System.Drawing.Rectangle(Location,this.Size);					
				}

				previousLocation = Location;
				return currentRectangle;
				
			}
		}	


		Point previousScreenLocation;
		System.Drawing.Rectangle currentScreenRectangle;
		public Rectangle ScreenRectangle
		{
			get
			{

				if(previousScreenLocation != ScreenLocation)
				{
					currentScreenRectangle = new System.Drawing.Rectangle(ScreenLocation,this.Size);					
				}


				previousScreenLocation = ScreenLocation;
				return currentScreenRectangle;
			}

		}	


		public Point Location
		{
			get
			{
				return new Point(_X,_Y);
			}
			set
			{	
				_X = value.X;
				_Y = value.Y;
			}
		}

		public Point ScreenLocation
		{
			get
			{
				return new Point(this.ScreenX,ScreenY);
			}
		}

		public Point BottomRightCorner
		{
			get
			{
				return new Point(_X2,_Y2);
			}
		}
		
		public abstract void Update();
		protected abstract void DrawGameObject(Surface surface);
		public void Draw(Surface surface)
		{
			if(surface == null)
			{
				throw new GameException("Input surface is NullReferenceException");
			}

			DrawGameObject(surface);
		}
		
		
		public bool Contains(int x, int y)
		{
			bool inSideX = (ScreenX <= x)&&(x<=ScreenX2);
			bool inSideY = (ScreenY <= y)&&(y<=ScreenY2);
			return inSideX&&inSideY;
		}
		public bool Contains(Point p)
		{
			return Contains(p.X,p.Y);
		}

		public bool Hits(GameObject obj)
		{		
			if(obj == null)
				return false;

			return Contains(obj.Location) || Contains(obj.BottomRightCorner) ;
		}

	
	}
}
