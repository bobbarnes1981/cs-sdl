#region LICENSE
/* 
 * (c) 200 Simon Gillespie
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

namespace SdlDotNet.Examples.Isotope
{
	//public static obj_time gametime=obj_time();

	//The Base Object class

	public class Object3d
	{
		/*Base 3d object used for physical interactions

		   pos: position vector in space: list of 3 integers [x,y,z]
		   size: size, dimension vector relative to the position: list of 3 integers [width-x,width-y,height-z]
		   objtype: the object type or id : integer
		   fixedob: A flag indicating if an object can is fixedob in position : boolean

		   vel: velocity vector : list of 3 integers [vx,vy,vz]
		   old_pos: The previous position vector of the object (unused ??): list of 3 integers [x,y,z]
		*/
		public int[] position={0,0,0};
		public int[] old_pos={0,0,0};
		public int[] vel={0,0,0};
		public int[] size={0,0,0};
		public bool fixedob;
		public int objtype;
	      
		//public Object3d(int[] pos,int[] size,int objtype=0,bool fixedob=true){
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		/// <param name="objtype"></param>
		/// <param name="fixedob"></param>
		public Object3d(int[] pos,int[] size,int objtype,bool fixedob)
		{
			pos.CopyTo(this.position,0);
			pos.CopyTo(old_pos,0);
			size.CopyTo(this.size,0);
			this.fixedob=fixedob;
			this.objtype=objtype;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual void Tick()
		{
			/*Tick event handler. A general function which allows the object to perform actions */
			//movement, basic velocity driver
			position.CopyTo(old_pos,0);
			position=Vector.AddVector(position,vel);
			position.CopyTo(this.position,0);
		}
	   
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other_obj"></param>
		/// <param name="impact_face"></param>
		public virtual void EventCollision(Object3d other_obj,int impact_face)
		{
			/*Collision event handler. A function to record a collision with other objects

			   other_obj: The other object that collided with this object: Object3d or subclass
			   impact_face: The face hit by the other object: face integer (0-5)
			*/
		}
	   
		/// <summary>
		/// 
		/// </summary>
		/// <param name="impact"></param>
		/// <param name="other_obj"></param>
		/// <param name="impact_face"></param>
		public virtual void EventTouch(bool impact,Object3d other_obj,int impact_face)
		{
			/*Touch event handler. A function to record a collision with other objects
	 
			   impact: Indicates if the other object has touched this object: boolean
			   other_obj: The other object that collided with this object: Object3d or subclass
			   impact_face: The face hit by the other object: face integer (0-5)
			*/
		}

	}
}
