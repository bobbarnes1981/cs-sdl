#region LICENSE
/* 
 * (c) 2005 Simon Gillespie
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE


//skins module: for defining complex images to draw for an object type and object state

/* Defines the animated images to draw for an object type and object state. */

using System;
using System.Collections;
using System.Drawing;

using SdlDotNet;
using SdlDotNet.Graphics;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public static class SkinsLib
    {
        // utility routines

        public static ArrayList LoadImages(string[] imageNames)
        {
            if (imageNames == null)
            {
                throw new ArgumentNullException("imageNames");
            }
            /*Loads a list of images.

               image_names: The filenames of the images to load: list of string
            */
            ArrayList images = new ArrayList();
            //Load images using a colorkey transparency of whitest white
            //int[] colorkey={255,255,255};
            foreach (string file_name in imageNames)
            {
                Surface image = new Surface(file_name);
                image = image.Convert();
                image.TransparentColor = Color.FromArgb(255, 255, 255);
                image.Transparent = true;
                images.Add(image);
            }
            return (images);
        }
    }
}