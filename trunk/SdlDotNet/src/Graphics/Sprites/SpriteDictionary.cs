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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Globalization;
using System.Security.Permissions;

using SdlDotNet;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Audio;

namespace SdlDotNet.Graphics.Sprites
{
    /// <summary>
    /// The SpriteDictionary is used to group sprites into an easily managed whole. 
    /// </summary>
    /// <remarks>The sprite manager has no size.</remarks>
    [Serializable]
    public class SpriteDictionary : Dictionary<Sprite, Rectangle>, ISerializable
    {
        #region Constructors
        /// <summary>
        /// Creates a new SpriteDictionary without any elements in it.
        /// </summary>
        public SpriteDictionary()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SpriteDictionary(
           SerializationInfo info, 
           StreamingContext context)
        {
        }

        /// <summary>
        /// Creates a new SpriteDictionary with one sprite element in it.
        /// </summary>
        /// <param name="sprite">Sprite to add to collection</param>
        public SpriteDictionary(Sprite sprite)
            : base()
        {
            this.Add(sprite);
        }

        /// <summary>
        /// Creates a new SpriteDictionary based off a different sprite collection.
        /// </summary>
        /// <param name="spriteDictionary">Add SpriteDictionary to this SpriteDictionary</param>
        public SpriteDictionary(SpriteDictionary spriteDictionary)
            : base()
        {
            foreach (Sprite s in spriteDictionary.Keys)
            {
                this.Add(s, spriteDictionary[s]);
            }
        }


        #endregion

        #region Display
        private Collection<Rectangle> lostRects = new Collection<Rectangle>();
        Collection<Rectangle> rects = new Collection<Rectangle>();
        Dictionary<Sprite, Rectangle> tempDict = new Dictionary<Sprite, Rectangle>();
        /// <summary>
        /// Draws all surfaces within the collection on the given destination.
        /// </summary>
        /// <param name="destination">The destination surface.</param>
        public virtual Collection<Rectangle> Draw(Surface destination)
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
        /// Erases SpriteDictionary from surface
        /// </summary>
        /// <param name="surface">
        /// Surface to remove the SpriteDictionary from</param>
        /// <param name="background">B
        /// ackground to use to paint over Sprites in SpriteDictionary
        /// </param>
        public void Erase(Surface surface, BaseSdlResource background)
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
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }
            Add(sprite, sprite.Rectangle);
        }

        /// <summary>
        /// Adds sprites from another group to this group
        /// </summary>
        /// <param name="spriteDictionary">SpriteDictionary to add Sprites from</param>
        public virtual int Add(SpriteDictionary spriteDictionary)
        {
            if (spriteDictionary == null)
            {
                throw new ArgumentNullException("SpriteDictionary");
            }
            foreach (Sprite s in spriteDictionary.Keys)
            {
                //s.AddInternal(this);
                Add(s);
            }
            return this.Count;
        }

        //private int AddInternal(SpriteDictionary SpriteDictionary)
        //{
        //    if (SpriteDictionary == null)
        //    {
        //        throw new ArgumentNullException("SpriteDictionary");
        //    }
        //    foreach (Sprite s in SpriteDictionary.Keys)
        //    {
        //        //SpriteDictionary[i].AddInternal(this);
        //        Add(s);
        //    }
        //    return this.Count;
        //}

        /// <summary>
        /// Rectangles of Sprites that have been removed
        /// </summary>
        /// <remarks>
        /// These Rectangles are kept temporarily until their 
        /// positions can be properly erased.
        /// </remarks>
        protected Collection<Rectangle> LostRects
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
        /// <param name="spriteDictionary">
        /// Remove SpriteDictionary from this SpriteDictionary.
        /// </param>
        public virtual void Remove(SpriteDictionary spriteDictionary)
        {
            if (spriteDictionary == null)
            {
                throw new ArgumentNullException("SpriteDictionary");
            }
            foreach (Sprite s in spriteDictionary.Keys)
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
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableActiveEvent()
        {
            Events.AppActive += new EventHandler<ActiveEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickAxisEvent()
        {
            Events.JoystickAxisMotion += new EventHandler<JoystickAxisEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickBallEvent()
        {
            Events.JoystickBallMotion += new EventHandler<JoystickBallEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickButtonEvent()
        {
            Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(Update);
            Events.JoystickButtonUp += new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickButtonDownEvent()
        {
            Events.JoystickButtonDown += new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickButtonUpEvent()
        {
            Events.JoystickButtonUp += new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableJoystickHatEvent()
        {
            Events.JoystickHatMotion += new EventHandler<JoystickHatEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableKeyboardEvent()
        {
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(Update);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableKeyboardDownEvent()
        {
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(Update);
        }
        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableKeyboardUpEvent()
        {
            Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableMouseButtonEvent()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(Update);
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableMouseButtonDownEvent()
        {
            Events.MouseButtonDown += new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableMouseButtonUpEvent()
        {
            Events.MouseButtonUp += new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableMouseMotionEvent()
        {
            Events.MouseMotion += new EventHandler<MouseMotionEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableUserEvent()
        {
            Events.UserEvent += new EventHandler<UserEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableQuitEvent()
        {
            Events.Quit += new EventHandler<QuitEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableVideoExposeEvent()
        {
            Events.VideoExpose += new EventHandler<VideoExposeEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableVideoResizeEvent()
        {
            Events.VideoResize += new EventHandler<VideoResizeEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableChannelFinishedEvent()
        {
            Events.ChannelFinished += new EventHandler<ChannelFinishedEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableMusicFinishedEvent()
        {
            Events.MusicFinished += new EventHandler<MusicFinishedEventArgs>(Update);
        }

        /// <summary>
        /// Enables Event for SpriteDictionary
        /// </summary>
        public void EnableTickEvent()
        {
            Events.Tick += new EventHandler<TickEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableActiveEvent()
        {
            Events.AppActive -= new EventHandler<ActiveEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickAxisEvent()
        {
            Events.JoystickAxisMotion -= new EventHandler<JoystickAxisEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickBallEvent()
        {
            Events.JoystickBallMotion -= new EventHandler<JoystickBallEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickButtonEvent()
        {
            Events.JoystickButtonDown -= new EventHandler<JoystickButtonEventArgs>(Update);
            Events.JoystickButtonUp -= new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickButtonDownEvent()
        {
            Events.JoystickButtonDown -= new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickButtonUpEvent()
        {
            Events.JoystickButtonUp -= new EventHandler<JoystickButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableJoystickHatEvent()
        {
            Events.JoystickHatMotion -= new EventHandler<JoystickHatEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableKeyboardEvent()
        {
            Events.KeyboardUp -= new EventHandler<KeyboardEventArgs>(Update);
            Events.KeyboardDown -= new EventHandler<KeyboardEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableKeyboardDownEvent()
        {
            Events.KeyboardDown -= new EventHandler<KeyboardEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableKeyboardUpEvent()
        {
            Events.KeyboardUp -= new EventHandler<KeyboardEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableMouseButtonEvent()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(Update);
            Events.MouseButtonUp -= new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableMouseButtonDownEvent()
        {
            Events.MouseButtonDown -= new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableMouseButtonUpEvent()
        {
            Events.MouseButtonUp -= new EventHandler<MouseButtonEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableMouseMotionEvent()
        {
            Events.MouseMotion -= new EventHandler<MouseMotionEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableUserEvent()
        {
            Events.UserEvent -= new EventHandler<UserEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableQuitEvent()
        {
            Events.Quit -= new EventHandler<QuitEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableVideoExposeEvent()
        {
            Events.VideoExpose -= new EventHandler<VideoExposeEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableVideoResizeEvent()
        {
            Events.VideoResize -= new EventHandler<VideoResizeEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableChannelFinishedEvent()
        {
            Events.ChannelFinished -= new EventHandler<ChannelFinishedEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableMusicFinishedEvent()
        {
            Events.MusicFinished -= new EventHandler<MusicFinishedEventArgs>(Update);
        }

        /// <summary>
        /// Disables Event for SpriteDictionary
        /// </summary>
        public void DisableTickEvent()
        {
            Events.Tick -= new EventHandler<TickEventArgs>(Update);
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
        /// Removes sprites from all SpriteDictionarys
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
        /// SpriteDictionary of sprite in this SpriteDictionary that 
        /// intersect with the given Sprite
        /// </returns>
        public virtual SpriteDictionary IntersectsWith(Sprite sprite)
        {
            SpriteDictionary intersection = new SpriteDictionary();
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
        /// Detects if any sprites in a given SpriteDictionary 
        /// intersect with any sprites in this SpriteDictionary.
        /// </summary>
        /// <param name="spriteDictionary">
        /// SpriteDictionary to check intersections
        /// </param>
        /// <returns>
        /// Hashtable with sprites in this SpriteDictionary as 
        /// keys and SpriteDictionarys containing sprites they 
        /// intersect with from the given SpriteDictionary
        /// </returns>
        public virtual Dictionary<Sprite, Sprite> IntersectsWith(SpriteDictionary spriteDictionary)
        {
            if (spriteDictionary == null)
            {
                throw new ArgumentNullException("SpriteDictionary");
            }
            Dictionary<Sprite, Sprite> intersection = new Dictionary<Sprite, Sprite>();
            foreach (Sprite s in this.Keys)
            {
                foreach (Sprite t in spriteDictionary.Keys)
                {
                    if (s.IntersectsWith(t))
                    {
                        if (intersection.ContainsKey(s))
                        {
                            //((SpriteDictionary)intersection[s]).Add(t);
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

        #endregion

        #region ISerializable Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand,
          SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException(Events.StringManager.GetString(
                        "NotImplemented", CultureInfo.CurrentUICulture));
        }

        #endregion
    }
}
