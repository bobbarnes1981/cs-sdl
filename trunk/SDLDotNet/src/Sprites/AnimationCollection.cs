using System;
using System.Collections;

namespace SdlDotNet.Sprites
{
	/// <summary>
	/// Summary description for AnimationCollection.
	/// </summary>
	public class AnimationCollection : DictionaryBase
	{
		public AnimationCollection() : base()
		{
		}

		public AnimationCollection(string key, Animation anim)
		{
			this.Add(key, anim);
		}


		/// <summary>
		/// Creates a new AnimationCollection with the contents of an existing AnimationCollection.
		/// </summary>
		/// <param name="musicCollection">The existing music collection to add.</param>
		public AnimationCollection(AnimationCollection animCollection)
		{
			this.Add(animCollection);
		}

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

		public int Add(string key, Animation anim) 
		{
			Dictionary.Add(key, anim);
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
		/// Removes an element from the collection.
		/// </summary>
		/// <param name="key">The element's key to remove.</param>
		public void Remove(string key)
		{
			Dictionary.Remove(key);
		}
	}
}
