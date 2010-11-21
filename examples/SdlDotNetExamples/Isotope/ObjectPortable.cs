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

/* Object engine classes for Isotope
    Note: It is very important that objects modify their positions or the object lists in their
    tick routines. Modifying these values in event receiver routines will mean that often a necessary collision
    detection has not occurred and that the drawn display will have errors */

using System.Collections;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectPortable : ObjectGravity
    {
        /* object_portable is a class for objects that can be picked up.

           pickedup: A flag indicating if the object is being carried: boolean
           carrier: The object that is carrying this object: Object3d or subclass:
           Note changed to type int for C# conversion
        */
        private bool pickedUp;

        public bool PickedUp
        {
            get { return pickedUp; }
            set { pickedUp = value; }
        }
        private int carrier;

        public int Carrier
        {
            get { return carrier; }
            set { carrier = value; }
        }

        //public object_portable(pos,size,objtype,fixedob=false):base(pos,size,objtype,fixedob){	      
        //}
        public ObjectPortable(int[] pos, int[] size, int objectType, bool fixedObject)
            : base(pos, size, objectType, fixedObject)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RequestPickup()
        {
            /* A handler to manage a pickup event

               lead_actor: The object that wants to pick up this object (unused)
            */
            //carrier=lead_actor;
            pickedUp = true;
            return (true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool RequestDrop()
        {
            /* A handler to manage a drop event

               scene: The scene to put this object into: scene class (unused)
            */
            pickedUp = false;
            carrier = 0;
            return (true);
        }
    }
    // End of object with gravity
}
