#region LICENSE
/*
 * Copyright (C) 2004 - 2006 David Hudson (jendave@yahoo.com)
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Audio;

namespace SdlDotNet.Graphics.Sprites
{
    /// <summary>
    /// The SpriteCollection is used to group sprites into an easily managed whole. 
    /// </summary>
    /// <remarks>The sprite manager has no size.</remarks>
    public class SpriteCollection : Dictionary<Sprite, Rectangle>
    {
        #region Constructors
        /// <summary>
        /// Creates a new SpriteCollection without any elements in it.
        /// </summary>
        public SpriteCollection()
            : base()
        {
        }

        /// <summary>
        /// Creates a new SpriteCollection with one sprite element in it.
        /// </summary>
        /// <param name="sprite">Sprite to add to collection</param>
        public SpriteCollection(Sprite sprite)
            : base()
        {
            //this.AddInternal(sprite);
        }

        /// <summary>
        /// Creates a new SpriteCollection based off a different sprite collection.
        /// </summary>
        /// <param name="spriteCollection">Add Spritecollection to this SpriteCollection</param>
        public SpriteCollection(SpriteCollection spriteCollection)
            : base()
        {
            //this.AddInternal(spriteCollection);
        }


        #endregion

        #region Display
        private List<Rectangle> lostRects = new List<Rectangle>();
        List<Rectangle> rects = new List<Rectangle>();
        Dictionary<Sprite, Rectangle> tempDict = new Dictionary<Sprite, Rectangle>();
        /// <summary>
        /// Draws all surfaces within the collection on the given destination.
        /// </summary>
        /// <param name="destination">The destination surface.</param>
        public virtual List<Rectangle> Draw(Surface destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            rects.Clear();
            tempDict.Clear();

            foreach (Sprite s in this.Keys)
            {
                if (s.Visible)
                {
                    rects.Add(this[s]);
                    tempDict.Add(s, destination.Blit(s.Render(), s.Rectangle));
                    //rects.Add(newRect);
                }
            }
            foreach (Sprite s in tempDict.Keys)
            {
                if (s.Visible)
                {
                    this[s] = tempDict[s];
                    rects.Add(tempDict[s]);
                }
            }

            return rects;
        }

        /// <summary>
        /// Erases SpriteCollection from surface
        /// </summary>
        /// <param name="surface">
        /// Surface to remove the SpriteCollection from</param>
        /// <param name="background">B
        /// ackground to use to paint over Sprites in SpriteCollection
        /// </param>
        public void Erase(Surface surface, Surface background)
        {
            if (surface == null)
            {
                throw new ArgumentNullException("surface");
            }
            for (int i = 0; i < this.lostRects.Count; i++)
            {
                surface.Blit(background, this.lostRects[i], this.lostRects[i]);
            }
            surface.Update(this.lostRects);

            foreach (Sprite s in this.Keys)
            {
                surface.Blit(background, s.Rectangle, s.Rectangle);
            }
            this.lostRects.Clear();
        }

        #endregion

        #region Sprites
        /// <summary>
        /// Adds sprite to group
        /// </summary>
        /// <param name="sprite">Sprite to add</param>
        public virtual void Add(Sprite sprite)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }
            //sprite.AddInternal(this);
            Add(sprite, sprite.Rectangle);
        }

        /// <summary>
        /// Adds sprite to group
        /// </summary>
        /// <param name="sprite">Sprite to add</param>
        public void AddInternal(Sprite sprite)
        {
            Add(sprite, sprite.Rectangle);
        }

        /// <summary>
        /// Adds sprites from another group to this group
        /// </summary>
        /// <param name="spriteCollection">SpriteCollection to add Sprites from</param>
        public virtual int Add(SpriteCollection spriteCollection)
        {
            if (spriteCollection == null)
            {
                throw new ArgumentNullException("spriteCollection");
            }
            foreach (Sprite s in spriteCollection.Keys)
            {
                //s.AddInternal(this);
                Add(s);
            }
            return this.Count;
        }

        private int AddInternal(SpriteCollection spriteCollection)
        {
            if (spriteCollection == null)
            {
                throw new ArgumentNullException("spriteCollection");
            }
            foreach (Sprite s in spriteCollection.Keys)
            {
                //spriteCollection[i].AddInternal(this);
                Add(s);
            }
            return this.Count;
        }

        /// <summary>
        /// Rectangles of Sprites that have been removed
        /// </summary>
        /// <remarks>
        /// These Rectangles are kept temporarily until their 
        /// positions can be properly erased.
        /// </remarks>
        protected List<Rectangle> LostRects
        {
            get
            {
                return this.lostRects;
            }
        }

        ///// <summary>
        ///// Removes sprite from group
        ///// </summary>
        ///// <param name="sprite">Sprite to remove</param>
        //public override void Remove(Sprite sprite)
        //{
        //    if (sprite == null)
        //    {
        //        throw new ArgumentNullException("sprite");
        //    }
        //    this.lostRects.Add(sprite.RectangleDirty);
        //    sprite.RemoveInternal(this);
        //    List.Remove(sprite);
        //}

        ///// <summary>
        ///// Removes sprite from group
        ///// </summary>
        ///// <param name="sprite">Sprite to remove</param>
        //public void RemoveInternal(Sprite sprite)
        //{
        //    if (sprite == null)
        //    {
        //        throw new ArgumentNullException("sprite");
        //    }
        //    this.lostRects.Add(sprite.RectangleDirty);
        //    List.Remove(sprite);
        //}

        /// <summary>
        /// Removes sprite from this group if they are contained in the given group
        /// </summary>
        /// <param name="spriteCollection">
        /// Remove SpriteCollection from this SpriteCollection.
        /// </param>
        public virtual void Remove(SpriteCollection spriteCollection)
        {
            if (spriteCollection == null)
            {
                throw new ArgumentNullException("spriteCollection");
            }
            foreach (Sprite s in spriteCollection.Keys)
            {
                if (this.ContainsKey(s))
                {
                    this.Remove(s);
                }
            }
        }

        ///// <summary>
        ///// Checks if sprite is in the container
        ///// </summary>
        ///// <param name="sprite">Sprite to query for</param>
        ///// <returns>True is the sprite is in the container.</returns>
        //public bool Contains(Sprite sprite)
        //{
        //    return (List.Contains(sprite));
        //}

        #endregion

        #region Geometry

        /// <summary>
        /// Gets the size of the first sprite in the collection, otherwise a size of 0,0.
        /// </summary>
        /// <returns>The size of the first sprite in the collection.</returns>
        public virtual Size Size
        {
            get
            {
                //this.Keys.GetEnumerator().;
                return new Size(0, 0);
                //}
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableActiveEvent()
        {
            Events.AppActive += new ActiveEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickAxisEvent()
        {
            Events.JoystickAxisMotion += new JoystickAxisEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickBallEvent()
        {
            Events.JoystickBallMotion += new JoystickBallEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickButtonEvent()
        {
            Events.JoystickButtonDown += new JoystickButtonEventHandler(Update);
            Events.JoystickButtonUp += new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickButtonDownEvent()
        {
            Events.JoystickButtonDown += new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickButtonUpEvent()
        {
            Events.JoystickButtonUp += new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableJoystickHatEvent()
        {
            Events.JoystickHatMotion += new JoystickHatEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableKeyboardEvent()
        {
            Events.KeyboardUp += new KeyboardEventHandler(Update);
            Events.KeyboardDown += new KeyboardEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableKeyboardDownEvent()
        {
            Events.KeyboardDown += new KeyboardEventHandler(Update);
        }
        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableKeyboardUpEvent()
        {
            Events.KeyboardUp += new KeyboardEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableMouseButtonEvent()
        {
            Events.MouseButtonDown += new MouseButtonEventHandler(Update);
            Events.MouseButtonUp += new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableMouseButtonDownEvent()
        {
            Events.MouseButtonDown += new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableMouseButtonUpEvent()
        {
            Events.MouseButtonUp += new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableMouseMotionEvent()
        {
            Events.MouseMotion += new MouseMotionEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableUserEvent()
        {
            Events.UserEvent += new UserEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableQuitEvent()
        {
            Events.Quit += new QuitEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableVideoExposeEvent()
        {
            Events.VideoExpose += new VideoExposeEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableVideoResizeEvent()
        {
            Events.VideoResize += new VideoResizeEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableChannelFinishedEvent()
        {
            Events.ChannelFinished += new ChannelFinishedEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableMusicFinishedEvent()
        {
            Events.MusicFinished += new MusicFinishedEventHandler(Update);
        }

        /// <summary>
        /// Enables Event for SpriteCollection
        /// </summary>
        public void EnableTickEvent()
        {
            Events.Tick += new TickEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableActiveEvent()
        {
            Events.AppActive -= new ActiveEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickAxisEvent()
        {
            Events.JoystickAxisMotion -= new JoystickAxisEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickBallEvent()
        {
            Events.JoystickBallMotion -= new JoystickBallEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickButtonEvent()
        {
            Events.JoystickButtonDown -= new JoystickButtonEventHandler(Update);
            Events.JoystickButtonUp -= new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickButtonDownEvent()
        {
            Events.JoystickButtonDown -= new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickButtonUpEvent()
        {
            Events.JoystickButtonUp -= new JoystickButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableJoystickHatEvent()
        {
            Events.JoystickHatMotion -= new JoystickHatEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableKeyboardEvent()
        {
            Events.KeyboardUp -= new KeyboardEventHandler(Update);
            Events.KeyboardDown -= new KeyboardEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableKeyboardDownEvent()
        {
            Events.KeyboardDown -= new KeyboardEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableKeyboardUpEvent()
        {
            Events.KeyboardUp -= new KeyboardEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableMouseButtonEvent()
        {
            Events.MouseButtonDown -= new MouseButtonEventHandler(Update);
            Events.MouseButtonUp -= new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableMouseButtonDownEvent()
        {
            Events.MouseButtonDown -= new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableMouseButtonUpEvent()
        {
            Events.MouseButtonUp -= new MouseButtonEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableMouseMotionEvent()
        {
            Events.MouseMotion -= new MouseMotionEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableUserEvent()
        {
            Events.UserEvent -= new UserEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableQuitEvent()
        {
            Events.Quit -= new QuitEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableVideoExposeEvent()
        {
            Events.VideoExpose -= new VideoExposeEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableVideoResizeEvent()
        {
            Events.VideoResize -= new VideoResizeEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableChannelFinishedEvent()
        {
            Events.ChannelFinished -= new ChannelFinishedEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableMusicFinishedEvent()
        {
            Events.MusicFinished -= new MusicFinishedEventHandler(Update);
        }

        /// <summary>
        /// Disables Event for SpriteCollection
        /// </summary>
        public void DisableTickEvent()
        {
            Events.Tick -= new TickEventHandler(Update);
        }


        /// <summary>
        /// Processes an active event.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, ActiveEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a joystick motion event. This event is triggered by
        /// SDL. Only
        /// sprites that are JoystickSensitive are processed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, JoystickAxisEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a joystick hat motion event. This event is triggered by
        /// SDL. Only
        /// sprites that are JoystickSensitive are processed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, JoystickBallEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a joystick button event. This event is triggered by
        /// SDL. Only
        /// sprites that are JoystickSensitive are processed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, JoystickButtonEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a joystick hat motion event. This event is triggered by
        /// SDL. Only
        /// sprites that are JoystickSensitive are processed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, JoystickHatEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes the keyboard.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, KeyboardEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a mouse button. This event is trigger by the SDL
        /// system. 
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, MouseButtonEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a mouse motion event. This event is triggered by
        /// SDL. Only
        /// sprites that are MouseSensitive are processed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, MouseMotionEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a Quit event
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, QuitEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a user event
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, UserEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a VideoExposeEvent
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, VideoExposeEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a VideoResizeEvent
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, VideoResizeEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a ChannelFinishedEvent
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, ChannelFinishedEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// Processes a MusicfinishedEvent
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, MusicFinishedEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        /// <summary>
        /// All sprites are tickable, regardless if they actual do
        /// anything. This ensures that the functionality is there, to be
        /// overridden as needed.
        /// </summary>
        /// <param name="sender">Object that sent event</param>
        /// <param name="e">Event arguments</param>
        private void Update(object sender, TickEventArgs e)
        {
            foreach (Sprite s in this.Keys)
            {
                s.Update(e);
            }
        }

        #endregion

        //#region Properties
        ///// <summary>
        ///// Gets and sets a sprite in the collection based on the index.
        ///// </summary>
        //public Sprite this[int index]
        //{
        //    get
        //    {
        //        return ((Sprite)List[index]);
        //    }
        //    set
        //    {
        //        List[index] = value;
        //    }
        //}
        //#endregion

        #region Public methods

        /// <summary>
        /// Removes sprites from all SpriteCollections
        /// </summary>
        public virtual void Kill()
        {
            foreach (Sprite s in this.Keys)
            {
                s.Kill();
            }
        }

        /// <summary>
        /// Detects if a given sprite intersects with any sprites in this sprite collection.
        /// </summary>
        /// <param name="sprite">Sprite to intersect with</param>
        /// <returns>
        /// SpriteCollection of sprite in this SpriteCollection that 
        /// intersect with the given Sprite
        /// </returns>
        public virtual SpriteCollection IntersectsWith(Sprite sprite)
        {
            SpriteCollection intersection = new SpriteCollection();
            foreach (Sprite s in this.Keys)
            {
                if (s.IntersectsWith(sprite))
                {
                    intersection.Add(s, s.Rectangle);
                }
            }
            return intersection;
        }

        /// <summary>
        /// Detects if any sprites in a given SpriteCollection 
        /// intersect with any sprites in this SpriteCollection.
        /// </summary>
        /// <param name="spriteCollection">
        /// SpriteCollection to check intersections
        /// </param>
        /// <returns>
        /// Hashtable with sprites in this SpriteCollection as 
        /// keys and SpriteCollections containing sprites they 
        /// intersect with from the given SpriteCollection
        /// </returns>
        public virtual Hashtable IntersectsWith(SpriteCollection spriteCollection)
        {
            if (spriteCollection == null)
            {
                throw new ArgumentNullException("spriteCollection");
            }
            Hashtable intersection = new Hashtable();
            foreach (Sprite s in this.Keys)
            {
                foreach (Sprite t in spriteCollection.Keys)
                {
                    if (s.IntersectsWith(t))
                    {
                        if (intersection.Contains(s))
                        {
                            //((SpriteCollection)intersection[s]).Add(t);
                        }
                        else
                        {
                            intersection.Add(s, t);
                        }
                    }
                }
            }
            return intersection;
        }

        ///// <summary>
        ///// Provide the explicit interface member for ICollection.
        ///// </summary>
        ///// <param name="array">Array to copy collection to</param>
        ///// <param name="index">Index at which to insert the collection items</param>
        //void ICollection.CopyTo(Array array, int index)
        //{
        //    this.List.CopyTo(array, index);
        //}

        /// <summary>
        /// Provide the explicit interface member for ICollection.
        /// </summary>
        /// <param name="array">Array to copy collection to</param>
        /// <param name="index">Index at which to insert the collection items</param>
        public virtual void CopyTo(Sprite[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        #endregion

        ///// <summary>
        ///// Insert a Sprite into the collection
        ///// </summary>
        ///// <param name="index">Index at which to insert the sprite</param>
        ///// <param name="sprite">Sprite to insert</param>
        //public virtual void Insert(int index, Sprite sprite)
        //{
        //    List.Insert(index, sprite);
        //} 

        ///// <summary>
        ///// Gets the index of the given sprite in the collection.
        ///// </summary>
        ///// <param name="sprite">The sprite to search for.</param>
        ///// <returns>The index of the given sprite.</returns>
        //public virtual int IndexOf(Sprite sprite)
        //{
        //    return List.IndexOf(sprite);
        //} 

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Comparer"></param>
        //public void Sort(IComparer Comparer) 
        //{
        //    base.InnerList.Sort(Comparer);
        //}
        #region IComparer Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            return ((Sprite)x).Z.CompareTo(((Sprite)y).Z);

        }

        #endregion
    }
}
