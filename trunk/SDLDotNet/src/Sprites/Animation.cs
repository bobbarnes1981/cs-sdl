/*
 * $RCSfile: Animation.cs,v $
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
	/// Animation.
	/// </summary>
	public class Animation : CollectionBase, ICollection
	{

		#region Constructors
		/// <summary>
		/// Creates a new empty Animation.
		/// </summary>
		public Animation()
		{
			m_Frames = new SurfaceCollection();
		}

		/// <summary>
		/// Creates a new Animation with a SurfaceCollection representing the animation.
		/// </summary>
		/// <param name="frames">The collection of surfaces in the animation.</param>
		public Animation(SurfaceCollection frames)
		{
			m_Frames = frames;
		}

		/// <summary>
		/// Creates an Animation with one surface to start off the animation.
		/// </summary>
		/// <param name="firstFrame">The surface representing the animation.
		/// </param>
		public Animation(Surface firstFrame)
		{
			m_Frames = new SurfaceCollection(firstFrame);
		}

		/// <summary>
		/// Creates a new Animation with a SurfaceCollection representing the animation.
		/// </summary>
		/// <param name="frames">The collection of surfaces in the animation.
		/// </param>
		/// <param name="delay">The amount of delay to be had between each frame.
		/// </param>
		/// <param name="loop">Whether or not the animation is 
		/// to loop when reached the end. Defaults to true.
		/// </param>
		public Animation(SurfaceCollection frames, double delay, bool loop)
		{
			m_Frames = frames;
			Delay = delay;
			Loop = loop;
		}

		/// <summary>
		/// Creates a new Animation with a SurfaceCollection representing the animation.
		/// </summary>
		/// <param name="frames">The collection of 
		/// surfaces in the animation.</param>
		/// <param name="delay">The amount of delay to be 
		/// had between each frame. Defaults to 30.</param>
		public Animation(SurfaceCollection frames, double delay)
		{
			m_Frames = frames;
			Delay = delay;
		}
		#endregion Constructors

		#region Properties
		private double m_Delay = 30;
		/// <summary>
		/// Gets and sets the amount of time delay that 
		/// should be had before moving onto the next frame.
		/// </summary>
		public double Delay
		{
			get
			{
				return m_Delay;
			}
			set
			{
				m_Delay = value;
			}
		}

		/// <summary>
		/// Gets and sets how long the animation should take to finish.
		/// </summary>
		public double AnimationTime
		{
			get
			{
				return m_Delay * this.Count;
			}
			set 
			{
				m_Delay = this.Count / value;
			}
		}

		private SurfaceCollection m_Frames;
		/// <summary>
		/// Gets the SurfaceCollection used to create the frames of the animation.
		/// </summary>
		public SurfaceCollection Frames
		{
			get
			{
				return m_Frames;
			}
		}

		/// <summary>
		/// Adds surface to group
		/// </summary>
		/// <param name="surface">Surface to add</param>
		public virtual int Add(Surface surface)
		{
			return (List.Add(surface));
		}

		/// <summary>
		/// Removes surface from group
		/// </summary>
		/// <param name="surface">Surface to remove</param>
		public virtual void Remove(Surface surface)
		{
			List.Remove(surface);
		}

		/// <summary>
		/// Insert a Surface into the collection
		/// </summary>
		/// <param name="index">Index at which to insert the surface</param>
		/// <param name="surface">Surface to insert</param>
		public virtual void Insert(int index, Surface surface)
		{
			List.Insert(index, surface);
		} 

		/// <summary>
		/// Gets the index of the given surface in the collection.
		/// </summary>
		/// <param name="surface">The surface to search for.</param>
		/// <returns>The index of the given surface.</returns>
		public virtual int IndexOf(Surface surface)
		{
			return List.IndexOf(surface);
		} 

		/// <summary>
		/// Checks if surface is in the container
		/// </summary>
		/// <param name="surface">Surface to query for</param>
		/// <returns>True is the surface is in the container.</returns>
		public bool Contains(Surface surface)
		{
			return (List.Contains(surface));
		}

		private bool m_Loop = true;
		/// <summary>
		/// Gets and sets whether or not the animation should loop.
		/// </summary>
		public bool Loop
		{
			get
			{ 
				return m_Loop;
			}
			set
			{ 
				m_Loop = value; 
			}
		}

		private int m_FrameIncrement = 1;
		/// <summary>
		/// Gets and sets the number of frames to go forward 
		/// when moving onto the next frame.
		/// </summary>
		/// <remarks>Making this one would result in the 
		/// animation going forwards one frame. 
		/// -1 would mean that the animation would go backwards. 
		/// Cannot be 0.</remarks>
		public int FrameIncrement
		{
			get
			{
				return m_FrameIncrement;
			}
			set
			{
				if (value == 0)
					m_FrameIncrement = 1;
				else
					m_FrameIncrement = value;
			}
		}

		/// <summary>
		/// Gets and sets whether the animation goes forwards or backwards.
		/// </summary>
		public bool AnimateForward
		{
			get
			{
				return m_FrameIncrement >= 0;
			}
			set
			{
				if (value)
				{
					// Make positive
					if (m_FrameIncrement < 0)
					{
						m_FrameIncrement *= -1;
					}
				}
				else
				{
					// Make negative
					if (m_FrameIncrement > 0)
					{
						m_FrameIncrement *= -1;
					}
				}

			}
		}

		/// <summary>
		/// Indexer of the frames.
		/// </summary>
		public Surface this[int index]
		{
			get
			{
				return m_Frames[index];
			}
			set
			{
				m_Frames[index] = value;
			}
		}

		#endregion Properties

		#region ICollection Members

		/// <summary>
		/// Gets whether the collection is synchronized.
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

//		/// <summary>
//		/// Gets the number of frames in the animation.
//		/// </summary>
//		public override int Count
//		{
//			get
//			{
//				return m_Frames.Count;
//			}
//		}

		/// <summary>
		/// Copies the frames of the animation to the given array.
		/// </summary>
		/// <param name="array">The array to copy to.</param>
		/// <param name="index">The start index.</param>
		public void CopyTo(Array array, int index)
		{
			m_Frames.CopyTo(array, index);
		}

		/// <summary>
		/// Gets an object representing the syncroot of the frames.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Provide the explicit interface member for ICollection.
		/// </summary>
		/// <param name="array">Array to copy collection to</param>
		/// <param name="index">Index at which to insert the collection items</param>
		void ICollection.CopyTo(Array array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// Provide the explicit interface member for ICollection.
		/// </summary>
		/// <param name="array">Array to copy collection to</param>
		/// <param name="index">Index at which to insert the collection items</param>
		public virtual void CopyTo(Surface[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		#endregion

//		#region IEnumerable Members
//
//		/// <summary>
//		/// Returns an enumerator that can iterate through the frames collection base.
//		/// </summary>
//		/// <returns></returns>
//		public override System.Collections.IEnumerator GetEnumerator()
//		{
//			return m_Frames.GetEnumerator();
//		}
//
//		#endregion
	}
}
