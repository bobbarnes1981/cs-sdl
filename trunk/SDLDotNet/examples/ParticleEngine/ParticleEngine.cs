/*
 * $RCSfile$
 * Copyright (C) 2005 Miguel De Sousa (DeSousa_87@hotmail.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System; 
using System.Drawing; 
using System.Collections; 

using SdlDotNet;

namespace SdlDotNet.Examples
{ 
	/// <summary> 
	/// Summary description for ParticleEngine.  
	/// </summary> 
	class ParticleEngine
	{ 
		/// <summary>
		/// 
		/// </summary>
		public ParticleEngine() 
		{ 
			this.InitializeStars();
		} 
		ArrayList _particles = new ArrayList(); 
		//we will use an array list because it's more flexible 

		private bool quitFlag;
		int width = 800;
		int height = 600;

		private Random _rnd = new Random(System.DateTime.Now.Millisecond); 
		//then we declare the _rnd with a seed on millisecond so we can randomise things; 
		
		Point _POINT; 
		Color _COLOR; 
		//this is for accessing information out the class about the location and the color of the particle; 
 
		const int NUM =50; 
		//this is the limiter of the particles you can change this for more or less particles...; 
  
		//now we are going to create the struct of our particle; 
		struct Particle 
		{ 
			public Point P; //location 
			public int Velocity; //velocity 
			public Point Direction; //direction 
			public Color COLOR; 
			public Particle(Point p1, int velocity, Point direction, Color color) 
			{ 
				COLOR=color; 
				Direction=direction; 
				Velocity=velocity; 
				P=p1; 
			} 
		} 
 
		/// <summary>
		/// now we do the initial of the stars so it can all be created... 
		/// </summary>
		private void InitializeStars() 
		{ 
			Particle particle; //here we call the struct 
			int Velo; // we declare an int that we will put velocity so later on we can do some fancy colors ;) 
 
			for ( int i= 1; i<NUM;i++) 
			{ 
				Velo=_rnd.Next(1,5); 
				particle= new Particle( new Point(_rnd.Next(1,width),_rnd.Next(1,height)),Velo, new Point(-1,0),Color.FromArgb(255,51*Velo,51*Velo)); 
				//hehehe int the last line we have give the data to the particle we have randomized the location 
				//that initial begins and we have generated the color of the star by its velocity cause being near 
				//means that it travels faster and it shines more, cool hu? :) 
				_particles.Add(particle); //and here we add the particle to the array 
			} 
		} 
  
		/// <summary>
		/// the stars_recycler is for the particles that have reach 
		/// point 0 of X in this example and we do 
		/// as was going to initialize but don't use the _particles.
		/// Add(particle); because it's already created 
		/// </summary>
		/// <param name="number"></param>
		public void RecycleStars( int number) 
		{ 
			Particle particle; 
			int Velo =_rnd.Next(1,5); 
			particle = new Particle(new Point(width,_rnd.Next(1,height)), Velo, new Point(-1,0),Color.FromArgb(255,51*Velo,51*Velo)); 
			_particles[number]= particle; 
		} 

		/// <summary>
		/// next we are going to make the stars move (yea yea here we are the all powerful :D ) 
		/// </summary>
		/// <param name="i"></param>
		public void MoveStars( int i) 
		{ 
			Particle particle; // call the struct 
			particle = (Particle)_particles[i]; // put the data from particle i 
			particle.P.X=particle.P.X+(particle.Direction.X*particle.Velocity); //calculate the x 
			particle.P.Y=particle.P.Y+(particle.Direction.Y*particle.Velocity); //calculate the y 
			//next we are going to check if the particle reaches the limits if not you pass it else recycle 
			//I have put it checking in case of you want it moving in the y to :P 
			if ((particle.P.X > 0)&&(particle.P.Y > 0)&&(particle.P.Y < height)) 
			{ 
				_COLOR = particle.COLOR; //set the _COLOR 
				_POINT = particle.P; //set the _POINT 
				_particles[i]= particle; //store the data 
			} 
			else 
			{ 
				RecycleStars(i); // recycle 
			} 
		} 

		private Box box = new Box(0, 0, 2, 2);
		/// <summary>
		/// here we return the location 
		/// </summary>
		public Point Point 
		{ 
			get 
			{ 
				return _POINT;
			} 
		} 
		/// <summary>
		/// here we return the Color 
		/// </summary>
		public Color Color 
		{ 
			get 
			{ 
				return _COLOR;
			} 
		} 
		//
		/// <summary>
		/// here we return the number of the particle I use it like that 
		/// and not the const cause sometimes you may have cases that 
		/// it's not full and/or you don't have to have the limit 
		/// </summary>
		public int NumberOfParticles 
		{ 
			get 
			{ 
				return this ._particles.Count;
			} 
		} 

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			Surface screen = Video.SetVideoModeWindow(width, height, true); 

			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);

			while (!quitFlag) 
			{
				while (Events.Poll()) 
				{
					// handle events till the queue is empty
				} 
					
				try 
				{
					screen.Fill(Color.FromArgb(0,0,0)); 
					//the above line paint the screen in black no need to put it if your going to put some kind 
					//of UFO, star ship or songoku in it 
					//then we do a cycle that goes updating and drawing all our particles 
					for ( int i =0; i< this.NumberOfParticles;i++) 
					{ 
						this.MoveStars(i); //update our particles... doing all the back work 
						box.Location = new Point(this.Point.X, this.Point.Y);
						screen.DrawFilledBox(box, this.Color);
						//screen.DrawPixel(this.Return_Point.X,this.Return_Point.Y,this.Return_Color); 
						//then draw it calling the variables that we have declared on the very beginning of the class remember??? 
					} 
					screen.Flip(); //well ... don't know really what it does but its necessary for the scene be draw 
				} 
				catch (SurfaceLostException) 
				{
				}
			}
		}
		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
		}

		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

		[STAThread]
		static void Main() 
		{
			ParticleEngine particleEngine = new ParticleEngine();
			particleEngine.Run();
		}
	} 
} 
