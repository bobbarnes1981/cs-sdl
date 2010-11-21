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

/*Function Library for 3d simulation of object physics.
   Collision detection, response and touch detection
*/

using System;
using System.Collections;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// A collider to store the impact, often refered to as imp in the code so perhaps
    /// impact would be a better word. Also used for touch routines as well.
    /// </summary>
    public class CollisionImpact
    {
        /* stores the impact of a collision

            impact: indicates an impact: Boolean
            impact_face_object1: hit face number of the first object: integer 0-5
            impact_face_object2: hit face number of the second object: integer 0-5
            impact_time: the integer time of the collision (now really a distance) see collision detect routine: integer
        */
        private bool impact;

        public bool Impact
        {
            get { return impact; }
            set { impact = value; }
        }
        private int impactFaceObject1;

        public int ImpactFaceObject1
        {
            get { return impactFaceObject1; }
            set { impactFaceObject1 = value; }
        }
        private int impactFaceObject2;

        public int ImpactFaceObject2
        {
            get { return impactFaceObject2; }
            set { impactFaceObject2 = value; }
        }
        private int impactTime;

        public int ImpactTime
        {
            get { return impactTime; }
            set { impactTime = value; }
        }
    }
}