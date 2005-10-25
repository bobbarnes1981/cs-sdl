/*
 * $RCSfile: ParticlesExample.cs,v $
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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
using SdlDotNet;
using SdlDotNet.Particles;
using SdlDotNet.Particles.Emitters;
using SdlDotNet.Particles.Manipulators;
using SdlDotNet.Particles.Particle;
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
		ParticleSystem particles = new ParticleSystem();

		// Make a new emitter and a particle vortex for manipulating the particles.
		ParticlePixelEmitter emit;
		ParticleVortex vort = new ParticleVortex(1f, 200f);

		/// <summary>
		/// Constructor
		/// </summary>
		public ParticlesExample()
		{
			// Setup SDL.NET!
			Video.SetVideoModeWindow(400,300);
			Video.WindowCaption = "SDL.NET - ParticlesExample";
			Events.KeyboardDown += new KeyboardEventHandler(Events_KeyboardDown);
			Events.MouseButtonDown += new MouseButtonEventHandler(Events_MouseButtonDown);
			Events.MouseMotion += new MouseMotionEventHandler(Events_MouseMotion);
			Events.Fps = 30;
			Events.Tick+=new TickEventHandler(Events_Tick);
		}

		/// <summary>
		/// Run the application
		/// </summary>
		public void Run()
		{

			// Make the particle emitter.
			emit = new ParticlePixelEmitter(particles);
			emit.Frequency = 100000; // 100000 every 1000 updates.
			emit.LifeFullMin = 20;
			emit.LifeFullMax = 50;
			emit.LifeMin = 10;
			emit.LifeMax = 30;
			emit.DirectionMin = -2; // shoot up in radians.
			emit.DirectionMax = -1;
			emit.ColorMin = Color.DarkBlue;
			emit.ColorMax = Color.LightBlue;
			emit.SpeedMin = 5;
			emit.SpeedMax = 20;
			
			// Make the first particle (a pixel)
			ParticlePixel first = new ParticlePixel(Color.White, 100,200,new Vector(0,0),-1);
			particles.Add(first); // Add it to the system

			// Make the second particle (an animated sprite)
			AnimationCollection anim = new AnimationCollection(new SurfaceCollection("../../Data/marble1.png", new Size(50,50)),1);
			AnimatedSprite marble = new AnimatedSprite(anim);
			marble.Animate = true;
			ParticleSprite second = new ParticleSprite(marble, 200, 200, new Vector(-7,-9), 500);
			second.Life = -1;
			particles.Add(second); // Add it to the system

			// Add some manipulators to the particle system.
			ParticleGravity grav = new ParticleGravity(0.5f);
			particles.Manipulators.Add(grav); // Gravity of 0.5f
			particles.Manipulators.Add(new ParticleFriction(0.1f)); // Slow down particles
			particles.Manipulators.Add(vort); // A particle vortex fixed on the mouse
			particles.Manipulators.Add(new ParticleBoundary(SdlDotNet.Video.Screen.Size)); // fix particles on screen.


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
			//emit.Target.Update();

			// Draw scene
			Video.Screen.Fill(Color.Black);
			particles.Render(Video.Screen);
			//emit.Target.Render(Video.Screen);

			Video.Screen.Update();
			Video.WindowCaption = "SDL.NET - ParticlesExample - Particles: " + particles.Particles.Count;
		}

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape)
			{
				Events.QuitApplication();
			}
		}
		private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
		{
			// Fix the emitter and the vortex manipulator to the mouse.
			emit.X = e.X;
			emit.Y = e.Y;
			vort.X = e.X;
			vort.Y = e.Y;
		}

		private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			// Toogle the emitter off and on.
			emit.Emitting = !emit.Emitting;

			// Make an explosion of pixels on the particle system..
			ParticleCircleEmitter explosion = new ParticleCircleEmitter(particles, Color.Red, Color.Orange, 1, 2);
			explosion.X = emit.X; // location
			explosion.Y = emit.Y;
			explosion.Life = 3; // life of the explosion
			explosion.Frequency = 150000;
			explosion.LifeMin = 5;
			explosion.LifeMax = 15;
			explosion.LifeFullMin = 5;
			explosion.LifeFullMax = 5;
			explosion.SpeedMin = 8;
			explosion.SpeedMax = 20;
		}
	}
}
