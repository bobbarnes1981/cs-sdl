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
		ParticleSystemCollection particles = new ParticleSystemCollection();
		ParticleEmitter emit = new ParticleEmitter();

		/// <summary>
		/// Constructor
		/// </summary>
		public ParticlesExample()
		{

			particles.Add(emit);

			emit.AddParticle += new AddParticleEventHandler(Events_AddParticle);
			
			// Make the first particle (a pixel)
			Particle first = new ParticlePixel(Color.White, 100,200,new Vector(0,0),-1);
			particles.Add(first); // Add it to the system

			// Make the second particle (an animated sprite)
			Animation anim = new Animation(new SurfaceCollection("../../Data/marble1.png", new Size(50,50)),1);
			AnimatedSprite marble = new AnimatedSprite(anim);
			marble.Animate = true;
			Particle second = new ParticleSprite(marble, 200, 200, new Vector(-7,-9), 500);
			second.Life = -1;
			particles.Add(second); // Add it to the system

			ParticleGravity grav = new ParticleGravity(0.5f);
			particles.Manipulators.Add(grav);

			ParticleFriction frict = new ParticleFriction(0.1f);
			particles.Manipulators.Add(frict);

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

		private void Events_AddParticle(object sender, AddParticleEventArgs e)
		{
			//return new ParticlePixel(Color.Red, sender.X,sender.Y,new Vector(1,1),30);
		}

		private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
		{
			emit.X = e.X;
			emit.Y = e.Y;
		}
	}
}
