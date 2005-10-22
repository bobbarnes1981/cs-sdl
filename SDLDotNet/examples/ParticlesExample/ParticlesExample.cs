/*
 * $RCSfile: AudioExample.cs,v $
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

		// Make a new emitter and add it to the particle system.
		ParticlePixelEmitter emit;
		ParticleVortex vort = new ParticleVortex(1f, 200f);

		/// <summary>
		/// Constructor
		/// </summary>
		public ParticlesExample()
		{
			emit = new ParticlePixelEmitter(particles);
			emit.Frequency = 100000f;
			emit.LifeFullMin = 20;
			emit.LifeFullMax = 50;
			emit.LifeMin = 20;
			emit.LifeMax = 30;
			emit.DirectionMin = -2f;
			emit.DirectionMax = -1f;
			emit.MaxR = 255;
			emit.MinR = 200;
			emit.MaxG = 50;
			emit.MinG = 0;
			emit.MaxB = 50;
			emit.MinB = 0;
			emit.SpeedMin = 3f;
			emit.SpeedMax = 20f;
			
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

			ParticleGravity grav = new ParticleGravity(0.5f);
			particles.Manipulators.Add(grav);

			ParticleFriction frict = new ParticleFriction(0.1f);
			particles.Manipulators.Add(frict);

			particles.Manipulators.Add(vort);
			

			// Setup SDL.NET!
			Video.SetVideoModeWindow(400,300);
			Video.WindowCaption = "SDL.NET - ParticlesExample";
			Events.Tick+=new TickEventHandler(Events_Tick);
			Events.KeyboardDown+=new KeyboardEventHandler(Events_KeyboardDown);

			ParticleBoundary bound = new ParticleBoundary(SdlDotNet.Video.Screen.Size);
			particles.Manipulators.Add(bound);

			SdlDotNet.Events.MouseMotion+=new MouseMotionEventHandler(Events_MouseMotion);
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

		private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
		{
			if(e.Key == Key.Escape)
			{
				Events.QuitApplication();
			}
		}
		private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
		{
			emit.X = e.X;
			emit.Y = e.Y;
			vort.X = e.X;
			vort.Y = e.Y;
		}
	}
}
