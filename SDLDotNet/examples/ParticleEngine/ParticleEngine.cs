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
	/// ParticleEngine is a small program that creates 
	/// a moving starfield on the screen using SDL.NET.
	/// </summary> 
	class ParticleEngine
	{ 
		/// <summary>
		/// Constructor for application. It simply initializes the stars.
		/// </summary>
		public ParticleEngine() 
		{ 
			this.InitializeStars();
		}
		// we will use an array list because it's more flexible 
		private ArrayList particles = new ArrayList(); 
		
		// quitflag will tell the app to quit the main polling loop and exit.
		private bool quitFlag;

		// width of display screen
		private int width = 800;

		//height of display screen
		private int height = 600;

		// then we declare the rand with a seed on millisecond 
		// so we can randomize things; 
		private Random rand = new Random(System.DateTime.Now.Millisecond); 
 
		// This is the number of particles created. 
		// You can change this for more or fewer particles. 
		private int num = 50; 
  
		//now we are going to create the struct of our particle; 
		class Particle 
		{ 
			private Point position; //location 
			private int velocity; //velocity 
			private Point direction; //direction 
			private Color color; // color of particle

			public Particle(
				Point position, int velocity, 
				Point direction, Color color) 
			{ 
				this.color = color; 
				this.direction = direction; 
				this.velocity = velocity; 
				this.position = position; 
			} 

			// Property getters/setters
			public Point Position
			{
				get
				{
					return position;
				}
				set
				{
					position = value;
				}
			}

			public Point Direction
			{
				get
				{
					return direction;
				}
				set
				{
					direction = value;
				}
			}


			public int Velocity
			{
				get
				{
					return velocity;
				}
				set
				{
					velocity = value;
				}
			}

			public Color Color
			{
				get
				{
					return color;
				}
				set
				{
					color = value;
				}
			}

			// particle will appear as a 2x2 pixel box
			public Box Star
			{
				get
				{
					return new Box(
						(short)this.position.X, 
						(short)this.position.Y, 
						(short)(this.position.X + 2), 
						(short)(this.position.Y + 2));
				}

			} 
		}
 
		/// <summary>
		/// Now we do the initialization of the stars so it can all be created.
		/// </summary>
		private void InitializeStars() 
		{ 
			Particle particle; //here we call the particle struct 

			// we declare an int that we will put velocity 
			int velocity; 
 
			// for loop that will set up all of the particles.
			for ( int i= 1; i < num;i++) 
			{ 
				// set a random velocity
				velocity = rand.Next(1,5);

				// creates new particle
				particle = 
					new Particle(
					// at a random point on the screen,
					new Point(rand.Next(1,width), rand.Next(1,height)),
					// with a random velocity
					velocity, 
					// moving from right to left
					new Point(-1, 0),
					// with a color based on its velocity. 
					// If it travels faster, it shines more.
					Color.FromArgb(255, 51 * velocity, 51 * velocity)
					); 
				//and here we add the particle to the arraylist
				particles.Add(particle); 
			} 
		} 

		/// <summary>
		/// next we are going to make the stars move 
		/// (yea yea here we are the all powerful :D ) 
		/// </summary>
		/// <param name="particle"></param>
		private void MoveStars(Particle particle) 
		{ 
			//calculate the x 
			//calculate the y 
			particle.Position = 
				new Point(
				particle.Position.X + 
				(particle.Direction.X * particle.Velocity), 
				particle.Position.Y + 
				(particle.Direction.Y * particle.Velocity)
				); 
			
			// Next we are going to check if the particle 
			// reaches the limits of the screen. 
			// If so, we reset its position to the starting point.
			// I have put it checking in case of you want it 
			// moving in the y to :P
			if ((particle.Position.X <= 0) ||
				(particle.Position.X >= this.width) ||
				(particle.Position.Y <= 0)||
				(particle.Position.Y >= this.height)
				) 
			{ 
				particle.Position = 
					new Point(this.width, rand.Next(1, this.height));
				particle.Velocity = rand.Next(1,5);
				particle.Direction = new Point(-1, 0);
				particle.Color = 
					Color.FromArgb(255, 51 * particle.Velocity, 51 * particle.Velocity); 
			}
		} 

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			Particle particle;
			//Set up screen to display app.
			Surface screen = Video.SetVideoModeWindow(width, height); 

			// Allow for app to respond to keybaord presses.
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			// Allow app to quit by clicking on the 'X' on the window frame.
			Events.Quit += new QuitEventHandler(this.Quit);

			// Will loop until quitflag is true, then app will quit.
			while (!quitFlag) 
			{
				while (Events.Poll()) 
				{
					// handle input events till the queue is empty
				} 
					
				try 
				{
					// Paints the screen in black and erases all items
					screen.Fill(Color.FromArgb(0,0,0)); 
					
					// Then we do a cycle that goes 
					// updating and drawing all our particles 
					for (int i = 0; i < this.particles.Count;i++) 
					{ 
						particle = (Particle)particles[i];
						//update our particles... doing all the back work 
						this.MoveStars(particle); 
						// draw particles to the screen back screen buffer
						screen.DrawFilledBox(particle.Star, particle.Color);
						
					} 
					// updates screen by flipping results 
					// to front screen buffer.
					screen.Flip(); 
				} 
				catch (SurfaceLostException e) 
				{
					Console.WriteLine(e);
				}
			}
		}

		// Event handler for keyboard events.
		// This accepts the Escape key and 'Q' key and will exit the app.
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

		// Event handler for when the 'X' on the window frame is clicked
		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

		// Main() of program.
		[STAThread]
		static void Main() 
		{
			ParticleEngine particleEngine = new ParticleEngine();
			particleEngine.Run();
		}
	} 
}
