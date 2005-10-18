using System;
using SdlDotNet;
using SdlDotNet.Particles;
using SdlDotNet.Sprites;
using System.Drawing;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// An example program using particles.
	/// </summary>
	public class ParticlesExample
	{
		// Make a new particle system with some gravity
		ParticleSystem particles = new ParticleSystem(0.3f);

		/// <summary>
		/// Constructor
		/// </summary>
		public ParticlesExample()
		{
			
			// Make the first particle (a pixel)
			Particle first = new ParticlePixel(Color.White, 100,100,new Vector(-1,-7),70);
			particles.Add(first); // Add it to the system

			// Make the second particle (an animated sprite)
			Animation anim = new Animation(new SurfaceCollection("../../Data/marble1.png", new Size(50,50)),1);
			AnimatedSprite marble = new AnimatedSprite(anim);
			marble.Animate = true;
			Particle second = new ParticleSprite(marble, 200, 200, new Vector(2,-10), 500);
			particles.Add(second); // Add it to the system

			// Setup SDL.NET!
			Video.SetVideoModeWindow(400,300);
			Video.WindowCaption = "SDL.NET - ParticlesExample";
			Events.Tick+=new TickEventHandler(Events_Tick);
		}

		/// <summary>
		/// Run the application
		/// </summary>
		public void Run()
		{
			Events.Run();
		}

		/// <summary>
		/// Main entery point
		/// </summary>
		public static void Main()
		{
			ParticlesExample p = new ParticlesExample();
			p.Run();
		}

		/// <summary>
		/// An update tick
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Events_Tick(object sender, TickEventArgs e)
		{
			// Update all particles
			particles.Update();

			// Draw scene
			Video.Screen.Fill(Color.Black);
			particles.Render(Video.Screen);
			Video.Screen.Update();
		}
	}
}
