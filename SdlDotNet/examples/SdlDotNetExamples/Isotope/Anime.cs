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

using System.Collections;
using System.Drawing;

using SdlDotNet.Graphics;

namespace SdlDotNetExamples.Isotope
{
    //Predefined extensions to the base class

    //Simple animated skin that cycles through each images image every tick
    public class Anime : Skin
    {
        /* anime: Simple animated skin that cycles through each images image every tick */
        public Anime(ArrayList images, string name)
            : base(images, name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Surface GetImage(Object3d obj)
        {
            /*Redefined image query to allow cycled animation */
            return ((Surface)Images[ObjectTime.Time % Images.Count]);
        }
    }
}