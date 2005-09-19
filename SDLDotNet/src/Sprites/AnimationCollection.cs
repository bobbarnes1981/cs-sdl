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
using System.Collections;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Summary description for AnimationCollection.
	/// </summary>
	public class AnimationCollection : DictionaryBase
    {
        #region Constructors
        /// <summary>
		/// Creates an empty AnimationCollection.
		/// </summary>
		public AnimationCollection() : base()
		{
		}

        /// <summary>
        /// Creates an AnimationCollection with one animation with the key "Default".
        /// </summary>
        /// <param name="anim"></param>
        public AnimationCollection(Animation anim)
        {
            this.Add("Default", anim);
        }

		/// <summary>
		/// Creates an AnimationCollection with one element within it.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="anim"></param>
		public AnimationCollection(string key, Animation anim)
		{
			this.Add(key, anim);
		}

        /// <summary>
        /// Creates an AnimationCollection with a "Default" animation of surfaces.
        /// </summary>
        /// <param name="surfaces"></param>
        public AnimationCollection(SurfaceCollection surfaces)
        {
            this.Add("Default", surfaces);
        }


		/// <summary>
		/// Creates a new AnimationCollection with the contents of an existing AnimationCollection.
		/// </summary>
		/// <param name="animCollection">The existing music collection to add.</param>
		public AnimationCollection(AnimationCollection animCollection)
		{
			this.Add(animCollection);
        }
        #endregion Constructors

        #region Properties
        /// <summary>
		/// Gets and sets an animation object within the collection using the animation's key.
		/// </summary>
		public Animation this[string key]
		{
			get 
			{
				return((Animation)Dictionary[key]);
			}
			set
			{
				Dictionary[key] = value;
			}
		}
		
		/// <summary>
		/// Gets all the Keys in the Collection.
		/// </summary>
		public ICollection Keys  
		{
			get  
			{
				return Dictionary.Keys;
			}
		}
        
		/// <summary>
		/// Gets all the Values in the Collection.
		/// </summary>
		public ICollection Values  
		{
			get  
			{
				return Dictionary.Values;
			}
        }

        /// <summary>
        /// Gets the average delay of all animations in the collection, sets the delay of every animation in the collection.
        /// </summary>
        public double Delay
        {
            get
            {
                double average = 0;
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    average += ((Animation)dict.Value).Delay;
                return average / this.Count;
            }
            set
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    ((Animation)dict.Value).Delay = value;
            }
        }

        /// <summary>
        /// Gets the average FrameIncrement for each Animation.  Sets the FrameIncrement of each Animation in the collection.
        /// </summary>
        public int FrameIncrement
        {
            get
            {
                int average = 0;
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    average += ((Animation)dict.Value).FrameIncrement;
                return average / this.Count;
            }
            set
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    ((Animation)dict.Value).FrameIncrement = value;
            }
        }


        /// <summary>
        /// Gets whether all animations animate forward and sets whether all animations in the collection are to animate forward.
        /// </summary>
        public bool AnimateForward
        {
            get
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                {
                    if (!((Animation)dict.Value).AnimateForward)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    ((Animation)dict.Value).AnimateForward = value;
            }
        }

        /// <summary>
        /// Gets whether the first animation is looping, sets whether every animation in the collection is to be looped.
        /// </summary>
        public bool Loop
        {
            get
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    return ((Animation)dict.Value).Loop;
                return true;
            }
            set
            {
                IDictionaryEnumerator dict = Dictionary.GetEnumerator();
                while (dict.MoveNext())
                    ((Animation)dict.Value).Loop = value;
            }
        }
        #endregion Properties
        
        #region Functions
        /// <summary>
		/// Adds an animation to the collection.
		/// </summary>
		/// <param name="key">The name of the animation.</param>
		/// <param name="anim">The animation object.</param>
		/// <returns>The final number of elements within the collection.</returns>
		public int Add(string key, Animation anim) 
		{
			Dictionary.Add(key, anim);
			return Dictionary.Count;
		}

        /// <summary>
        /// Adds a surface collection to the collection as an animation.
        /// </summary>
        /// <param name="key">The name of the animation.</param>
        /// <param name="surfaces">The SurfaceCollection that represents the animation.</param>
        /// <returns>The final number of elements within the collection.</returns>
        public int Add(string key, SurfaceCollection surfaces)
        {
            Dictionary.Add(key, new Animation(surfaces));
            return Dictionary.Count;
        }
		
		/// <summary>
		/// Adds a collection of music to the current music collection.
		/// </summary>
		/// <param name="animCollection">The collection of music samples to add.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(AnimationCollection animCollection)
		{
			IDictionaryEnumerator dict = animCollection.GetEnumerator();
			while(dict.MoveNext())
				this.Add((string)dict.Key, (Animation)dict.Value);
			return Dictionary.Count;
        }

        /// <summary>
        /// Returns true if the collection contains the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Dictionary.Contains(key);
        }
        
		/// <summary>
		/// Removes an element from the collection.
		/// </summary>
		/// <param name="key">The element's key to remove.</param>
		public void Remove(string key)
		{
			Dictionary.Remove(key);
		}
        #endregion Functions

	}
}
