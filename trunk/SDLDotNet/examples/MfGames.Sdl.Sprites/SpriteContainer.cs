/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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

using MfGames.Utility;
using SdlDotNet;
using System;
using System.Collections;
using System.Drawing;

namespace MfGames.Sdl.Sprites
{
  /// <summary>
  /// The SpriteContainer is a special case of sprite. It is used to
  /// group other sprites into an easily managed whole. The sprite
  /// manager has no size.
  /// </summary>
  public class SpriteContainer : Sprite
  {
    public SpriteContainer()
    {
      IsTickable = true;
    }

    #region Display
    public override void Render(RenderArgs args)
    {
      // Check for our own X and Y
      RenderArgs args1 = args.Clone();
      args1.TranslateX += Coords.X;
      args1.TranslateY += Coords.Y;

      // Check for a size
      if (size != null)
      {
	// Set the size
	args1.Size = size;
	Rectangle2 clip = new Rectangle2(args1.TranslateX,
					 args1.TranslateY,
					 size.Width,
					 size.Height);

	// Debugging
	if (IsTraced)
	{
//	  MfGames.Sdl.Gui.GuiManager.DrawRect(args.Surface, clip,
//					      System.Drawing
//					      .Color.BlanchedAlmond);
	}

	// Set the clipping rectangle
	args1.SetClipping(clip);
      }

      // Check for viewport
      if (viewport != null)
      {
	viewport.AdjustViewport(ref args1);
      }

      // Go through the sprites. We make a copy to make sure nothing
      // else changes the list while we are doing so.
      ArrayList list = new ArrayList(sprites);
      list.Sort();

      foreach (Sprite s in list)
	s.Render(args1);

      // Clear the clipping
      args1.ClearClipping();
    }
    #endregion

    #region Sprites
    private ArrayList sprites = new ArrayList();

    public void Add(Sprite sprite)
    {
      if (!sprites.Contains(sprite))
	sprites.Add(sprite);
    }

    public void Remove(Sprite sprite)
    {
      sprites.Remove(sprite);
    }
    #endregion

    #region Geometry
    private Dimension2 size = null;

	  public override Dimension2 Size
	  {
		  get
		  {
			  if (size == null)
				  return new Dimension2();
			  else
				  return size;
		  }
		  set
		  {
			  size = value;
		  }
	  }

    public override bool IntersectsWith(Vector2 point)
    {
      if (size == null)
      {
	// Containers have no bounds
	return true;
      }
      else
      {
	// We actually have a size
	return base.IntersectsWith(point);
      }
    }

    public override bool IntersectsWith(Rectangle2 rect)
    {
      if (size == null)
      {
	// Containers have no bounds
	return true;
      }
      else
      {
	// We actually have a size
	return base.IntersectsWith(rect);
      }
    }
    #endregion

    #region Events
    private Sprite eventLock = null;

    public override bool IsKeyboardSensitive { get { return true; } }

    public override bool IsMouseSensitive { get { return true; } }

    public Sprite EventLock
    {
      get { return eventLock; }
      set { eventLock = value; }
    }

    /// <summary>
    /// This causes the sprite manager to add itself to the SDL
    /// events. This enables it to handle the bulk of the processing
    /// for mouse and button events.
    /// </summary>
    public void ListenToSdlEvents()
    {
      Events.MouseButtonUp +=
	new MouseButtonEventHandler(OnSdlMouseButtonUp);
      Events.MouseButtonDown +=
	new MouseButtonEventHandler(OnSdlMouseButtonDown);
      Events.MouseMotion +=
	new MouseMotionEventHandler(OnSdlMouseMotion);
    }

    /// <summary>
    /// Processes the keyboard. If this function returns true, then
    /// the system no longer processes the keyboard event. If it is
    /// not true, then the next sprite that is keyboard sensitive is
    /// processed.
    /// </summary>
    public override bool OnKeyboard(object sender, KeyboardEventArgs e)
    {
      return false;
    }

    /// <summary>
    /// Processes a mouse button. This event is trigger by the SDL
    /// system. If this function returns true, then processing is
    /// stopped. If it returns false, then the next sprite is
    /// processed.
    /// </summary>
    public override bool OnMouseButton(object sender, MouseArgs args)
    {
      // Build up the new point
      int x = args.X;
      int y = args.Y;
      int relx = args.RelativeX;
      int rely = args.RelativeY;

      MouseArgs args1 = args.Clone();
      args1.TranslateX -= Coords.X;
      args1.TranslateY -= Coords.Y;
	 
      // Check for an event lock
      if (EventLock != null)
      {
	  if (EventLock.OnMouseButton(this, args1))
	    return true;
      }

      // Go through the sprites. We make a copy to make sure nothing
      // else changes the list while we are doing so.
      ArrayList list = new ArrayList(sprites);
      Vector2 point = args1.Coords;
      list.Sort();

      foreach (Sprite s in list)
      {
	// Don't bother if not mouse sensitive
	if (!s.IsMouseSensitive)
	  continue;

	// Check for bounds
	if (s.IntersectsWith(point))
	{
	  // Execute the button
	  if (s.OnMouseButton(this, args1))
	    return true;
	}
      }

      // We didn't process it
      return false;
    }

    /// <summary>
    /// Processes a mouse motion event. This event is triggered by
    /// SDL. If this returns true, then processing of the motion event
    /// is stopped, otherwise the next sprite is processed. Only
    /// sprites that are MouseSensitive are processed.
    /// </summary>
    public bool OnMouseMotion(MouseArgs args)
    {
      // Build up the new point
      int x = args.X;
      int y = args.Y;
      int relx = args.RelativeX;
      int rely = args.RelativeY;
      
      MouseArgs args1 = args.Clone();
      args1.TranslateX -= Coords.X;
      args1.TranslateY -= Coords.Y;
	 
      // Check for an event lock
      if (EventLock != null)
      {
	if (EventLock.OnMouseMotion(this, args1))
	  return true;
      }

      // Go through the sprites. We make a copy to make sure nothing
      // else changes the list while we are doing so.
      ArrayList list = new ArrayList(sprites);
      Vector2 point = new Vector2(x, y);
      list.Sort();

      foreach (Sprite s in list)
      {
	// Don't bother if not mouse sensitive
	if (!s.IsMouseSensitive)
	  continue;

	// Check for bounds
	if (s.IntersectsWith(point))
	{
	  // Execute the button
	  if (s.OnMouseMotion(this, args1))
	    return true;
	}
      }

      // We didn't process it
      return false;
    }

    /// <summary>
    /// Processes a mouse button event by passing the events to the
    /// appropriate sprites.
    /// </summary>
    public void OnSdlMouseButtonDown(object sender, MouseButtonEventArgs e)
    {
      // Create the mouse args
      MouseArgs args = new MouseArgs();
      args.BaseX = e.X;
      args.BaseY = e.Y;
      args.IsButton1 = true;

      // Call it
      OnMouseButton(this, args);
    }

    /// <summary>
    /// Processes a mouse button event by passing the events to the
    /// appropriate sprites.
    /// </summary>
    public void OnSdlMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
      // Create the mouse args
      MouseArgs args = new MouseArgs();
      args.BaseX = e.X;
      args.BaseY = e.Y;
      args.IsButton1 = false;

      // Call it
      OnMouseButton(this, args);
    }

    /// <summary>
    /// Processes a mouse motion event by passing the events to the
    /// appropriate sprites.
    /// </summary>
    public void OnSdlMouseMotion(object sender, MouseMotionEventArgs e)
//		MouseButtonState state,
//			      int x, int y,
//			      int relx, int rely)
    {
      // Create the mouse args
      MouseArgs args = new MouseArgs();
      args.BaseX = e.X;
      args.BaseY = e.Y;
      args.RelativeX = e.RelativeX;
      args.RelativeY = e.RelativeY;

      // Call ourselves
      OnMouseMotion(args);
    }

    /// <summary>
    /// Calls the OnTick for all sprites inside the container.
    /// </summary>
    public override void OnTick(TickArgs args)
    {
      // Go through the sprites
      foreach (Sprite s in new ArrayList(sprites))
	if (s.IsTickable)
	  s.OnTick(args);
    }
    #endregion

    #region Operators
    public override string ToString()
    {
      return System.String.Format("(container {0})", sprites.Count);
    }
    #endregion

    #region Properties
    private IViewport viewport = null;

    public IViewport Viewport
    {
      get { return viewport; }
      set { viewport = value; }
    }
    #endregion
  }
}
