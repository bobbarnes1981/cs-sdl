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

using System;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// Monster: template for a automated actor which turns around from any collision
    /// </summary>
    public class Monster : Actor
    {
        /* An automated actor which turns around from any collision */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="objtype"></param>
        /// <param name="fixedob"></param>
        public Monster(int[] position, int[] size, int objectType, bool fixedObject)
            : base(position, size, objectType, fixedObject)
        {
            int[] twos ={ 2, 0, 0 };
            GetVelocity().CopyTo(twos, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other_obj"></param>
        /// <param name="impact_face"></param>
        public new void EventCollision(Object3d otherObject, int impactFace)
        {
            /*/ Turn around 180 degrees and continue walking /*/
            //call actors standard collision code
            base.EventCollision(otherObject, impactFace);
            int[] ones ={ 1, 0, 0 };
            int[] negones ={ -1, 0, 0 };
            // simple toggle movement
            if (CollisionTime == ObjectTime.Time)
            {
                if (GetFacing() == ones && impactFace == 0)
                {
                    GetVelocity()[0] = -2;
                    SetFacing(negones);
                }
                else if (GetFacing() == negones && impactFace == 1)
                {
                    GetVelocity()[0] = 2;
                    SetFacing(ones);
                }
                //WARNING MISSING TIME FUNCTION MUST BE FIXED
                CollisionTime = ObjectTime.Time;
            }
        }
    }
}