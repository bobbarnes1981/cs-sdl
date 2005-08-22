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
	/// Represents a collection of music samples held together by a dictionary key-value base.
	/// </summary>
	/// <example>
	/// <code>
	/// MusicCollection tunes = new MusicCollection();
	/// 
	/// tunes.Add("techno", "techno.mid");
	/// tunes.Add("jazz.mid");
	/// 
	/// tunes["techo"].Play();
	/// tunes["jazz.mid"].Play();
	/// </code>
	/// </example>
	public class MusicCollection : DictionaryBase
	{
		/// <summary>
		/// Creates a new empty MusicCollection.
		/// </summary>
		public MusicCollection() : base()
		{
		}

		/// <summary>
		/// Creates a new MusicCollection with one element in it.
		/// </summary>
		/// <param name="key">The key you would like to refer to the music sample as.</param>
		/// <param name="music">The sample object itself.</param>
		public MusicCollection(string key, Music music)
		{
			this.Add(key, music);
		}

		/// <summary>
		/// Creates a new MusicCollection with one element in it.
		/// </summary>
		/// <param name="filename">The filename and key of the single Music object to load.</param>
		public MusicCollection(string filename)
		{    
			this.Add(filename);
		}

		/// <summary>
		/// Creates a new MusicCollection with one element in it.
		/// </summary>
		/// <param name="music">The single music sample to start off the collection.</param>
		public MusicCollection(Music music)
		{
			this.Add(music);
		}

		/// <summary>
		/// Loads multiple files from a directory into the collection.
		/// </summary>
		/// <param name="baseName">The name held before the file index.</param>
		/// <param name="extension">The extension of the files (.mp3)</param>
		public MusicCollection(string baseName, string extension)
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
				this.Dictionary.Add(fn, new Music(fn));
				i++;
			}
		}

		/// <summary>
		/// Creates a new MusicCollection with the contents of an existing MusicCollection.
		/// </summary>
		/// <param name="musicCollection">The existing music collection to add.</param>
		public MusicCollection(MusicCollection musicCollection)
		{
			IDictionaryEnumerator enumer = musicCollection.GetEnumerator();
			while(enumer.MoveNext())
			{
				this.Add((string)enumer.Key, (Music)enumer.Value);
			}
		}

		/// <summary>
		/// Gets and sets a music object within the collection.
		/// </summary>
		public Music this[string key]
		{
			get 
			{
				return((Music)Dictionary[key]);
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
		/// Adds a music sample to the collection.
		/// </summary>
		/// <param name="key">The key to use as reference to the music object.</param>
		/// <param name="music">The sample to add.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(string key, Music music) 
		{
			Dictionary.Add(key, music);
			return Dictionary.Count;
		}

		/// <summary>
		/// Adds a music sample to the collection using the filename as the key.
		/// </summary>
		/// <param name="filename">The music filename to load as well as the key to use as the reference.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(string filename)
		{
			Dictionary.Add(filename, new Music(filename));
			return Dictionary.Count;
		}

		/// <summary>
		/// Adds a music sample to the collection using the Music's ToString method as the key.
		/// </summary>
		/// <param name="music">The music object to add.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(Music music)
		{
			Dictionary.Add(music.ToString(), music);
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
		/// Adds a collection of music to the current music collection.
		/// </summary>
		/// <param name="musicCollection">The collection of music samples to add.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(MusicCollection musicCollection)
		{
			IDictionaryEnumerator dict = musicCollection.GetEnumerator();
			while(dict.MoveNext())
				this.Add((string)dict.Key, (Music)dict.Value);
			return Dictionary.Count;
		}

		/// <summary>
		/// Adds a music sample to the collection.
		/// </summary>
		/// <param name="key">The reference value for the music sample.</param>
		/// <param name="filename">The filename of the music sample to load.</param>
		/// <returns>The total number of elements within the collection after adding the sample.</returns>
		public int Add(string key, string filename)
		{
			Dictionary.Add(key, new Music(filename));
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
	}
}
