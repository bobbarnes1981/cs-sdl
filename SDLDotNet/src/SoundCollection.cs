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
using System.IO;

namespace SdlDotNet
{
    /// <summary>
    /// Encapulates a collection of Sound objects in a Sound Dictionary.
    /// </summary>
    /// <remarks>Every sound object within the collection is indexed by a string key.</remarks>
    /// <example>
    /// <code>
    /// SoundCollection sounds = new SoundCollection();
    /// sounds.Add("boom", Mixer.Sound("explosion.wav"));
    /// sounds.Add("boing.wav");
    /// sounds.Add("baseName", ".ogg");
    /// 
    /// sounds["boing.wav"].Play();
    /// sounds["boom"].Play();
    /// sounds["baseName-01.ogg"].Play(); 
    /// </code>
    /// </example>
    /// <seealso cref="Sound"/>
    public class SoundCollection : DictionaryBase
    {
        /// <summary>
        /// Creates a new SoundCollection object.
        /// </summary>
        public SoundCollection() : base()
        {
        }
        
        /// <summary>
        /// Creates a SoundCollection with one loaded item.
        /// </summary>
        /// <param name="key">The key of the sound item.</param>
        /// <param name="sound">The sound object.</param>
        public SoundCollection(string key, Sound sound)
        {
            this.Add(key, sound);
        }
        
        /// <summary>
        /// Creates a SoundCollection with one loaded item.
        /// </summary>
        /// <param name="filename">The sound item's filename to load and set as the key.</param>
        public SoundCollection(string filename)
        {    
            this.Add(filename);
        }
        
        /// <summary>
        /// Loads a number of files.
        /// </summary>
        /// <param name="baseName">The string contained before the file index number.</param>
        /// <param name="extension">The extension of each file.</param>
        public SoundCollection(string baseName, string extension)
        {
            // Save the fields
            //this.filename = baseName + "-*" + extension;
            int i = 0;
            while (true)
            {
                string fn = null;
                if (i < 10)
                    fn = baseName + "-0" + i + extension;
                else
                    fn = baseName + "-" + i + extension;
                
                if (!File.Exists(fn))
                    break;
                
                // Load it
                this.Dictionary.Add(fn, Mixer.Sound(fn));
                i++;
            }
        }
        
        /// <summary>
        /// Adds the contents of an existing SoundCollection to a new one.
        /// </summary>
        /// <param name="soundCollection">The SoundCollection to copy.</param>
        public SoundCollection(SoundCollection soundCollection)
        {
			IDictionaryEnumerator enumer = soundCollection.GetEnumerator();
			while(enumer.MoveNext())
			{
				this.Add((string)enumer.Key, (Sound)enumer.Value);
			}
        }
        
        /// <summary>
        /// Gets and sets a Sound value based off of its key.
        /// </summary>
        public Sound this[string key]
        {
            get 
            {
                return((Sound)Dictionary[key]);
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
        /// Adds a Sound object to the Collection.
        /// </summary>
        /// <param name="key">The key to make reference to the object.</param>
        /// <param name="sound">The sound object to add.</param>
        /// <returns>The final number of elements within the collection.</returns>
        public int Add(string key, Sound sound) 
        {
            Dictionary.Add(key, sound);
            return Dictionary.Count;
        }
        
        /// <summary>
        /// Adds a newly loaded file to the collection.
        /// </summary>
        /// <param name="filename">The filename to load.</param>
        /// <returns>The final number of elements within the collection.</returns>
        public int Add(string filename)
        {
            Dictionary.Add(filename, Mixer.Sound(filename));
            return Dictionary.Count;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Dictionary.Contains(key);
        }
        
        /// <summary>
        /// Adds an existing SoundCollection to the collection.
        /// </summary>
        /// <param name="soundCollection">The SoundCollection to add.</param>
        /// <returns>The final number of objects within the collection.</returns>
        public int Add(SoundCollection soundCollection)
        {
            IDictionaryEnumerator dict = soundCollection.GetEnumerator();
            while(dict.MoveNext())
                this.Add((string)dict.Key, (Sound)dict.Value);
            return Dictionary.Count;
        }
        
        /// <summary>
        /// Loads and adds a new sound object to the collection.
        /// </summary>
        /// <param name="key">The key to give the sound object.</param>
        /// <param name="filename">The sound file to load.</param>
        /// <returns>The final number of elements within the collection.</returns>
        public int Add(string key, string filename)
        {
        	Dictionary.Add(key, Mixer.Sound(filename));
        	return Dictionary.Count;
		}
        
		/// <summary>
		/// Loads and adds a new sound object to the collection.
		/// </summary>
		/// <param name="sound">The sound sample to add. Uses ToString() as the key.</param>
		/// <returns>The final number of elements within the collection.</returns>
		public int Add(Sound sound)
		{
			Dictionary.Add(sound.ToString(), sound);
			return Dictionary.Count;
		}		
        
        /// <summary>
        /// Removes an element from the collection.
        /// </summary>
        /// <param name="key">The element's key to remove.</param>
        public void Remove(string key)
        {
            Dictionary.Remove(key);
        }

		/// <summary>
		/// Stops every sound within the collection.
		/// </summary>
		public void Stop()
		{
			foreach(Sound sound in this.Dictionary.Values)
				sound.Stop();
		}
        
        /// <summary>
        /// Plays every sound within the collection.
        /// </summary>
        public void Play()
        {
        	foreach(Sound sound in this.Dictionary.Values)
        		sound.Play();
        }
        
        /// <summary>
        /// Sets the volume of every sound object within the collection. Gets the average volume of all sound objects within the collection.
        /// </summary>
        public int Volume
        {
        	get
        	{
        		if(Dictionary.Count > 0){
	        		int total = 0;
	        		foreach(Sound sound in this.Dictionary.Values)
	        			total += sound.Volume;
	        		return total / Dictionary.Count;
        		}
        		else
        			return 0;
        	}
        	set
        	{
        		foreach(Sound sound in this.Dictionary.Values)
        			sound.Volume = value;
        	}
        }
    }
}
