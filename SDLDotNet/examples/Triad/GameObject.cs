

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
	/// <summary>
	/// 
	/// </summary>
	public abstract class  GameObject
	{
		/// <summary>
		/// 
		/// </summary>
		public GameObject()
		{

		}

		private GameObject _Parent;
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
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
	
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
		public int  X2
		{
			get
			{
				return _X + _Width;
			}
		}
	
		private int  _Y2=0;
		/// <summary>
		/// 
		/// </summary>
		public int  Y2
		{
			get
			{
				return _Y + _Height;
			}
		}

		
		/// <summary>
		/// 
		/// </summary>
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
	
		
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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
		/// <summary>
		/// 
		/// </summary>
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


		/// <summary>
		/// 
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
		public Point ScreenLocation
		{
			get
			{
				return new Point(this.ScreenX,ScreenY);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point BottomRightCorner
		{
			get
			{
				return new Point(_X2,_Y2);
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public abstract void Update();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		protected abstract void DrawGameObject(Surface surface);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="surface"></param>
		public void Draw(Surface surface)
		{
			if(surface == null)
			{
				throw new GameException("Input surface is NullReferenceException");
			}

			DrawGameObject(surface);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool Contains(int x, int y)
		{
			bool inSideX = (ScreenX <= x)&&(x<=ScreenX2);
			bool inSideY = (ScreenY <= y)&&(y<=ScreenY2);
			return inSideX&&inSideY;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool Contains(Point p)
		{
			return Contains(p.X,p.Y);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Hits(GameObject obj)
		{		
			if(obj == null)
				return false;

			return Contains(obj.Location) || Contains(obj.BottomRightCorner) ;
		}

	
	}
}
