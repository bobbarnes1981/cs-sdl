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
            this.Add(soundCollection);
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
        public void Add(string key, Sound sound) 
        {
            Dictionary.Add(key, sound);
        }
        
        /// <summary>
        /// Adds a newly loaded file to the collection.
        /// </summary>
        /// <param name="filename">The filename to load.</param>
        public void Add(string filename)
        {
            Dictionary.Add(filename, Mixer.Sound(filename));
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
            return this.Count;
        }
        
        /// <summary>
        /// Removes an element from the collection.
        /// </summary>
        /// <param name="key">The element's key to remove.</param>
        public void Remove(string key)
        {
            Dictionary.Remove(key);
        }
    }
}
