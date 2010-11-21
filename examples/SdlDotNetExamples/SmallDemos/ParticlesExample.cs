#region LICENSE
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
#endregion LICENSE

using System;
using System.IO;
using System.Drawing;

using SdlDotNet.Particles;
using SdlDotNet.Particles.Emitters;
using SdlDotNet.Particles.Manipulators;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// An example program using particles.
    /// </summary>
    public class ParticlesExample : IDisposable
    {
        // Make a new particle system with some gravity
        ParticleSystem particles = new ParticleSystem();

        // Make a new emitter and a particle vortex for manipulating the particles.
        ParticleRectangleEmitter emit;
        ParticleVortex vort = new ParticleVortex(1f, 200f);
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticlesExample()
        {
            // Setup SDL.NET!
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - ParticlesExample";
            Video.SetVideoMode(400, 300);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(this.MouseButtonDown);
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(this.MouseMotion);
            Events.Fps = 30;
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
        }

        /// <summary>
        /// Run the application
        /// </summary>
        public void Go()
        {

            // Make the particle emitter.
            emit = new ParticleRectangleEmitter(particles);
            emit.Frequency = 50000; // 100000 every 1000 updates.
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
            emit.MaxSize = new SizeF(5, 5);
            emit.MinSize = new SizeF(1, 1);

            // Make the first particle (a pixel)
            ParticlePixel first = new ParticlePixel(Color.White, 100, 200, new Vector(0, 0, 0), -1);
            particles.Add(first); // Add it to the system

            if (File.Exists(Path.Combine(dataDirectory, "marble1.png")))
            {
                filePath = "";
            }

            // Make the second particle (an animated sprite)
            AnimationCollection anim = new AnimationCollection();
            SurfaceCollection surfaces = new SurfaceCollection();
            surfaces.Add(Path.Combine(filePath, Path.Combine(dataDirectory, "marble1.png")), new Size(50, 50));
            anim.Add(surfaces, 1);
            AnimatedSprite marble = new AnimatedSprite(anim);
            marble.Animate = true;
            ParticleSprite second = new ParticleSprite(marble, 200, 200, new Vector(-7, -9, 0), 500);
            second.Life = -1;
            particles.Add(second); // Add it to the system

            // Add some manipulators to the particle system.
            ParticleGravity grav = new ParticleGravity(0.5f);
            particles.Manipulators.Add(grav); // Gravity of 0.5f
            particles.Manipulators.Add(new ParticleFriction(0.1f)); // Slow down particles
            particles.Manipulators.Add(vort); // A particle vortex fixed on the mouse
            particles.Manipulators.Add(new ParticleBoundary(SdlDotNet.Graphics.Video.Screen.Size)); // fix particles on screen.


            Events.Run();
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        [STAThread]
        public static void Run()
        {
            ParticlesExample p = new ParticlesExample();
            p.Go();
        }

        /// <summary>
        /// An update tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, TickEventArgs e)
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

        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Events.QuitApplication();
            }
            else if (e.Key == Key.Space)
            {
                CreateExplosion();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
        private void MouseMotion(object sender, MouseMotionEventArgs e)
        {
            // Fix the emitter and the vortex manipulator to the mouse.
            emit.X = e.X;
            emit.Y = e.Y;
            vort.X = e.X;
            vort.Y = e.Y;
        }

        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Toogle the emitter off and on.
            emit.Emitting = !emit.Emitting;

            CreateExplosion();
        }

        private void CreateExplosion()
        {
            // Make an explosion of pixels on the particle system..
            ParticleCircleEmitter explosion = new ParticleCircleEmitter(particles, Color.Red, Color.Orange, 1, 2);
            explosion.X = emit.X; // location
            explosion.Y = emit.Y;
            explosion.Life = 3; // life of the explosion
            explosion.Frequency = 100000;
            explosion.LifeMin = 5;
            explosion.LifeMax = 20;
            explosion.LifeFullMin = 5;
            explosion.LifeFullMax = 5;
            explosion.SpeedMin = 8;
            explosion.SpeedMax = 20;
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "ParticlesExample: Uses the Particle Engine";
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~ParticlesExample()
        {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //					if (this.background != null)
                    //					{
                    //						this.background.Dispose();
                    //						this.background = null;
                    //					}
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}
