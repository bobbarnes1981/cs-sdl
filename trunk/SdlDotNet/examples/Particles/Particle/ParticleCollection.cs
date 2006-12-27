/*
 * $RCSfile$
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
using System.Collections.ObjectModel;
using System.Collections.Generic;

using SdlDotNet.Graphics;

using SdlDotNet.Particles.Emitters;

namespace SdlDotNet.Particles.Particle
{
    /// <summary>
    /// A collection of particles.
    /// </summary>
    public class ParticleCollection : Collection<BaseParticle>
    {
        /// <summary>
        /// Creates a new ParticleCollection.
        /// </summary>
        public ParticleCollection()
        {
        }

        /// <summary>
        /// Creates a new ParticleCollection with one particle element.
        /// </summary>
        /// <param name="particle">The particle to start off the collection.</param>
        public ParticleCollection(BaseParticle particle)
        {
            Add(particle);
        }

        /// <summary>
        /// Creates a new ParticleCollection from an already existing collection of particles.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        public ParticleCollection(ParticleCollection collection)
        {
            foreach (BaseParticle b in collection)
            {
                Add(b);
            }
        }

        /// <summary>
        /// Creates a particle collection from the particles in the provided system.
        /// </summary>
        /// <param name="system">The system containing all the particles to add.</param>
        public ParticleCollection(ParticleSystem system)
        {
            Add(system);
        }

        /// <summary>
        /// Adds a particle emitter to the collection.
        /// </summary>
        /// <param name="emitter">The emitter to add to the collection.</param>
        public void Add(ParticleEmitter emitter)
        {
            Add(emitter, true);
        }

        /// <summary>
        /// Adds a particle emitter to the collection.
        /// </summary>
        /// <param name="emitter">The emitter to add to the collection.</param>
        /// <param name="changeEmitterTarget">Flag to change the emitter's target particle collection.  Defaults to true.</param>
        public void Add(ParticleEmitter emitter, bool changeEmitterTarget)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException("emitter");
            }
            this.Add(emitter);
            if (changeEmitterTarget)
            {
                emitter.Target.Clear();
                foreach (BaseParticle b in this)
                {
                    emitter.Target.Add(b);
                }
                //emitter.Target = this;
            }
        }

        /// <summary>
        /// Adds a collection of particles from a particle system.
        /// </summary>
        /// <param name="system">The system containing all the particles.</param>
        public void Add(ParticleSystem system)
        {
            if (system == null)
            {
                throw new ArgumentNullException("system");
            }
            foreach (BaseParticle b in system.Particles)
            {
                Add(b);
            }
        }

        /// <summary>
        /// Updates all particles in the collection.
        /// </summary>
        /// <returns>True if a particle is still alive.</returns>
        public virtual bool Update()
        {
            BaseParticle particle;
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                particle = (BaseParticle)this[i];
                if (!particle.Update())
                {
                    this.RemoveAt(i--);
                    count--;
                }
            }
            return count > 0;
        }

        /// <summary>
        /// Renders all particles on a destination surface.
        /// </summary>
        /// <param name="destination">The surface to render the particles onto.</param>
        public void Render(Surface destination)
        {
            foreach (BaseParticle particle in this)
            {
                particle.Render(destination);
            }
        }
    }
}
