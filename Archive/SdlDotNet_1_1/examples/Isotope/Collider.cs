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

namespace SdlDotNet.Examples.Isotope 
{
	/// <summary>
	/// A collider to store the impact, often refered to as imp in the code so perhaps
	/// impact would be a better word. Also used for touch routines as well.
	/// </summary>
	public class Collider
	{
		/* stores the impact of a collision

			impact: indicates an impact: Boolean
			impact_face_object1: hit face number of the first object: integer 0-5
			impact_face_object2: hit face number of the second object: integer 0-5
			impact_time: the integer time of the collision (now really a distance) see collision detect routine: integer
		*/
		public bool impact=false;
		public int impact_face_object1=0;
		public int impact_face_object2=0;
		public int impact_time=0;
	}
}