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
    /// Object with gravity
    /// </summary>
    public class ObjectGravity : Object3d
    {
        /* The object_gravity class defines objects which can be pulled by gravity.

           If the object is touching something gravity will be turned off and will not be applied.
           If the object is not touching something gravity will be turned on and it will descend
           until it touches something. 

           gravity: if gravity is turned on: boolean
           coltime: the last collision time: float
           touching: if the actor is touching something: boolean
           touched_faces: list of faces being touched: list of face integers (0-5)
           touched_objects: list of objects being touched: list of Object3d class or subclass
        */

        private bool gravity;

        public bool Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }
        // Flag to show if this object is touching on its bottom z face
        private bool touching;

        public bool Touching
        {
            get { return touching; }
            set { touching = value; }
        }
        // List for the other objects being touched and by what face
        private ArrayList touchedObjects = new ArrayList();

        public ArrayList TouchedObjects
        {
            get { return touchedObjects; }
        }
        private ArrayList touchedFaces = new ArrayList();

        public ArrayList TouchedFaces
        {
            get { return touchedFaces; }
        }
        // Time of the collision? Doesnt seem to be used
        private int collisionTime;

        public int CollisionTime
        {
            get { return collisionTime; }
            set { collisionTime = value; }
        }

        //public object_gravity(int[] pos,int[] size,int objtype,bool fixedob=false):
        public ObjectGravity(int[] pos, int[] size, int objectType, bool fixedObject)
            :
            base(pos, size, objectType, fixedObject)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Tick()
        {
            /* Redfined tick event handler with gravity and touching.*/
            // Velocity movement: Standard object movement
            //System.Console.WriteLine("object_gravity:tick entry");
            base.Tick();
            // Gravity control: Turn on gravity if nothing is touching us below.
            if (touching == false)
            {
                gravity = true;
            }
            if (gravity == true)
            {
                //System.Console.WriteLine("Gravity on!");
                GetVelocity()[2] = GetVelocity()[2] - 1;
            }
            // Clear the touching information for the next tick.
            touching = false;
            touchedObjects.Clear();
            touchedFaces.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other_obj"></param>
        /// <param name="impact_face"></param>
        public override void EventCollision(Object3d otherObject, int impactFace)
        {
            /* Redefined collision event handler for gravity and touching.*/
            // When colliding with something on the z axis while gravity is on
            //System.Console.WriteLine("Object_gravity: event collision entry");
            if (impactFace == 5 && gravity == true)
            {
                GetVelocity()[2] = 0;
                gravity = false;
            }
            //***WARNING TAKEN OUT UNTIL DEBUG WE NEED A GLOBAL TIME VAR!!!
            //coltime=gametime.get_time();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impact"></param>
        /// <param name="other_obj"></param>
        /// <param name="impact_face"></param>
        public override void EventTouch(bool impact, Object3d otherObject, int impactFace)
        {
            /* Redefined touch event handler for gravity and touching.*/
            // Add the impact information to the touching lists of objects and faces
            if (impact == true)
            {
                touchedObjects.Add(otherObject);
                touchedFaces.Add(impactFace);
            }
            if (impact == true && impactFace == 5)
            {
                //this.vel[2]=0
                touching = true;
                //this.gravity=false
            }
            //this gets called because the two objects are not touching not because
            //this object is not touching any other 
            else
            {
                //this.gravity=true
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            /* Sets the objects velocity in all directions to zero. */
            if (gravity == false)
            {
                GetVelocity()[0] = 0;
                GetVelocity()[1] = 0;
                GetVelocity()[2] = 0;
            }
        }
    }
}
