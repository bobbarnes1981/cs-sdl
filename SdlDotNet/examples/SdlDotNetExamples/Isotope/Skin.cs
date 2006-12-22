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
    // skin: translates an objects state into an image

    public class Skin
    {
        /*The base skin class used to represent an object type on the screen.

           images: The images used to display an object type: list of image
           name: name of the object type: string
           get_image(obj) : returns the image to display based on the state of the object */
        private ArrayList images;

        public ArrayList Images
        {
            get { return images; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Skin(ArrayList images, string name)
        {
            this.images = images;
            this.name = name;
        }

        //default behaviour returns image 0 from the images
        public virtual Surface GetImage(Object3d obj)
        {
            /*Returns the image to display based on the state of an object 

               obj: The individual object: Object3d class or subclass
            */
            return ((Surface)images[0]);
        }
    }
}