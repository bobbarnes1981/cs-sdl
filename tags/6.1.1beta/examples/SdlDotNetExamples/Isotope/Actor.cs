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

using System.Collections;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// Defines extended object physical behaviour for actors with example jump, pick_up(get), 
    /// drop, collision based actions
    /// </summary>
    /// <remarks>
    /// Defines a new object subclass which can do human physical movement: facing, jumping,
    /// and a movement cycle
    /// cycle: position of moviment cycle, ie leg lifted, leg down etc: integer
    /// facing* : to show which way the actor is facing: list of 3 facing integers [x,y,z]
    /// * Note: due to facing not being taken into account for collision detection and drawing routines
    /// the actor sprites must be equal dimensions on the base of their bounding box.
    /// </remarks>
    public class Actor : ObjectGravity
    {
        int cycle;

        /// <summary>
        /// 
        /// </summary>
        public int Cycle
        {
            get
            {
                return cycle;
            }
            set
            {
                cycle = value;
            }
        }

        int[] facing = { -1, 0, 0 };
        /// <summary>
        /// 
        /// </summary>
        public int[] GetFacing()
        {
            return facing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facing"></param>
        public void SetFacing(int[] facing)
        {
            this.facing = facing;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="objtype"></param>
        /// <param name="fixedob"></param>
        public Actor(int[] position, int[] size, int objectType, bool fixedObject)
            : base(position, size, objectType, fixedObject)
        {
        }

        /// <summary>
        /// time based action
        /// </summary>
        public override void Tick()
        {
            // Walking animation: Cycle through each of the 4 movement frames
            if (GetPosition()[0] != GetOldPosition()[0] || GetPosition()[1] != GetOldPosition()[1])
            {
                cycle = cycle + 1;
                if (cycle == 4)
                {
                    cycle = 0;
                }
            }
            else
            {
                cycle = 0;
            }
            // Velocity movement: Standard object movement
            base.Tick();
        }

        /// <summary>
        /// action to change velocity based on movement
        /// </summary>
        /// <param name="offset"></param>
        public void Move(int[] offset)
        {
            /*/ offset: the value for the velocity: list of 3 integers [vx,vy,vz] /*/
            if (Gravity == false)
            {
                SetVelocity(offset);
                int[] zero ={ 0, 0, 0 };
                facing = Vector.Direction(zero, offset);
                CollisionTime = ObjectTime.Time;
            }
        }

        /// <summary>
        /// action for the actor to jump
        /// </summary>
        public void Jump()
        {
            if (Gravity == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    GetVelocity()[i] = facing[i] * 2;
                }
                GetVelocity()[2] = GetVelocity()[2] + 8;
                Gravity = true;
            }
        }
    }
}