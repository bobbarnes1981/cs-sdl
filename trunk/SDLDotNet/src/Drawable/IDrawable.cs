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

using SdlDotNet.Utility;
using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Drawable
{
  /// <summary>
  /// Defines the interface to the generic drawable system. These
  /// drawables have multiple uses, ranging from representing an image
  /// in memory or some data value, to more specific operations, such
  /// as adding a red filter to the image.
  /// </summary>
  public interface IDrawable
  {
    #region Frames
    /// <summary>
    /// This gives an accessor to the surfaces of this
    /// drawable. This should throw an exception of there is no
    /// surface, but the system assumes that if there is at least one
    /// valid surface, then a drawable will always return the
    /// surface. This allows the system to request image #12 from a
    /// drawable with only one frame and get that same frame.
    /// </summary>
    Surface this[int frame] { get; }

    /// <summary>
    /// Property that contains the number of frames (surfaces) in the
    /// drawable.
    /// </summary>
    int FrameCount { get; }

    /// <summary>
    /// Returns a cache key which represents a unique identifier of
    /// the drawable. This is used by CachedDrawable to keep the CPU
    /// generation low.
    /// </summary>
    string GetCacheKey(int frame);
    #endregion

    #region Geometry
    /// <summary>
    /// Returns the size of the drawable, in pixels.
    /// </summary>
    Size Size { get; }
    #endregion
  }
}
