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
    //Animated skin who has a different image for each direction faced and part of their movement cycle
    internal class Pointing : Skin
    {
        /* pointing: Animated skin who has a different image for each direction they face
            and for each frame of animation

            This skin requires a list of 12 images which are divided into 4 directions of facing
            on the x,y plane and 3 cycles in each direction. Each image direction group comprises
            of 3 images which are animated in the sequence [0,1,0,2], to complete a full movement.

            For facing [1,0,0]: images [0,1,2]
                facing [-1,0,0]: images [3,4,5]
                facing [0,1,0]: images [6,7,8]
                facing [0,-1,0]: images [9,10,11]
        */
        public Pointing(ArrayList images, string name)
            : base(images, name)
        {
        }

        public override Surface GetImage(Object3d obj)
        {
            /*Redefined get_image to allow multidirectional animation */
            int[] sequence ={ 0, 1, 0, 2 };
            //int[] E ={ 1, 0, 0 };
            //int[] W ={ -1, 0, 0 };
            //int[] N ={ 0, 1, 0 };
            //int[] S ={ 0, -1, 0 };

            // The 'as' statement performs a duplicate cast operation.
            Actor aObj = obj as Actor;
            if (aObj != null)
            {
                int actorfacing = Vector.VectorToFace(aObj.GetFacing());
                int actorcycle = aObj.Cycle;
                if (actorfacing == 0)
                {
                    return ((Surface)Images[0 + sequence[actorcycle]]);
                }
                if (actorfacing == 1)
                {
                    return ((Surface)Images[3 + sequence[actorcycle]]);
                }
                if (actorfacing == 2)
                {
                    return ((Surface)Images[6 + sequence[actorcycle]]);
                }
                if (actorfacing == 3)
                {
                    return ((Surface)Images[9 + sequence[actorcycle]]);
                }
                //default, this should never happen but just in case
            }
            return ((Surface)Images[0]);
        }
    }

    //Possibly unused?: Fix Later if we need to
    //extends the skin so that the object can be examined and display a picture
    //class examinable:skin){
    /*A skin subclass to allow a more detailed picture of an object when its being examined

          examine_image: detailed image of the object: image class
       */
    //   def __init__(self,images,name,examine_image){
    //      skin.__init__(self,images,name);
    //      self.examine_image=examine_image;
    //   }
    //}
}
