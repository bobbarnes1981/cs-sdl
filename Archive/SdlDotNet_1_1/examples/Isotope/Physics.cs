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
	/// 
	/// </summary>
	public class Physics
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <returns></returns>
		public static Collider CollisionDetect(Object3d object1,Object3d object2)
		{
			/* Discovers if there is a collision between two objects

				object1: The first object to be detected: class or subclass of Object3d
				object2: The second object to be detected: class or subclass of Object3d
				Returns imp: a collider with the impact time and faces hit on both objects: class collider
		 
				Notes: Simple collision detection, does not handle if objects pass completely
				through each other if they have high velocity and the objects are small
			*/
			Collider imp= new Collider();
			int impact_time_face1,impact_time_face2;
			int impact_face_object1=0,impact_face_object2=0;
			int impact_time_coord=0;
		    
			for(int i=0;i<=2;i++)
			{
				//check if intersecting
                if (object1.position[i] + object1.size[i] <= object2.position[i])
                {
                    return (imp);
                }
                if (object1.position[i] >= object2.position[i] + object2.size[i])
                {
                    return (imp);
                }
			}
			//if intersecting calculate the first face impacted
			imp.impact=true;
			//***OSCALL
			imp.impact_time=1600000;
		    
			for(int i=0;i<=2;i++)
			{
				impact_time_face1=object1.position[i]+object1.size[i]-object2.position[i];
				impact_time_face2=object2.position[i]+object2.size[i]-object1.position[i];
				if (impact_time_face1 < impact_time_face2) 
				{
					impact_face_object1=i<<1;
					impact_face_object2=(i<<1)+1;
					impact_time_coord=impact_time_face1;
				}
				else if (impact_time_face1 >= impact_time_face2) 
				{
					impact_face_object1=(i<<1)+1;
					impact_face_object2=i<<1;
					impact_time_coord=impact_time_face2;
				}
				if (impact_time_coord < imp.impact_time)
				{
					imp.impact_time=impact_time_coord;
					imp.impact_face_object1=impact_face_object1;
					imp.impact_face_object2=impact_face_object2;
				}
			}
			return(imp);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj_group"></param>
		public static void CollisionProcessor(Object3d[] obj_group)
		{
			/* collision_processor: Detects collisions amongst all object pairs and moves them back
				until just before the collision and runs the object collision responses

				obj_group: A list of objects within the scene: list of class or subclass of Object3d
			*/
			Collider imp=new Collider();
			//runs the collision routines until no impacts occur
			while (true)
			{
				bool noimpact=true;
				for (int object1=0;object1<obj_group.Length;object1++)
				{
					for (int object2=object1+1;object2<obj_group.Length;object2++)
					{
						//If both objects are not fixedob call the collision detector to get the first object collided with
						//and time of collision, faces collided with
						if (obj_group[object1].fixedob== false || obj_group[object2].fixedob== false)
						{ 
							imp=CollisionDetect(obj_group[object1],obj_group[object2]);
							if (imp.impact == true)
							{
								//Collision response, currently just moving the two objects apart to just touching
								CollisionResponse(obj_group[object1],obj_group[object2],imp);
								obj_group[object1].EventCollision(obj_group[object2],imp.impact_face_object1);
								obj_group[object2].EventCollision(obj_group[object1],imp.impact_face_object2);
								noimpact=false;
								//Console.WriteLine("End collider loop");
							}
						}
					}
				}
                if (noimpact == true)
                {
                    break;
                }
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <param name="imp"></param>
		public static void CollisionResponse(Object3d object1,Object3d object2, Collider imp)
		{
			/* Moves back the objects until they are adjacent on their colliding sides

				object1: The first object to be moved: class or subclass of Object3d
				object2: The second object to be moved: class or subclass of Object3d
				imp: a collider with the impact time and faces hit on both objects: class collider */
			// calculate the collision coordinate from the face
			int coord=imp.impact_face_object1>>1;
			int delta=0;
			//four cases based on if the object has its fixedob flag to stop pushing.
			if (object1.fixedob == true && object2.fixedob == true)
				// Do nothing as both objects are fixedob
				return;
			if (object1.fixedob == false && object2.fixedob == true)
			{
                if (imp.impact_face_object1 % 2 == 0)
                {
                    object1.position[coord] = object2.position[coord] - object1.size[coord] - 1;
                }
                else
                {
                    object1.position[coord] = object2.position[coord] + object2.size[coord];
                }
			}
			if (object1.fixedob == true && object2.fixedob == false)
			{
                if (imp.impact_face_object2 % 2 == 0)
                {
                    object2.position[coord] = object1.position[coord] - object2.size[coord] - 1;
                }
                else
                {
                    object2.position[coord] = object1.position[coord] + object1.size[coord];
                }
			}
			if (object1.fixedob == false && object2.fixedob == false)
			{
				if (imp.impact_face_object1%2 == 0)
				{
					delta=(object1.position[coord]+object1.size[coord]-object2.position[coord]);
					object1.position[coord]=(int)object1.position[coord]-(int)((float)delta/2.0);
					object2.position[coord]=(int)object2.position[coord]+(int)((float)delta/2.0)+1;
				}
				else
				{
					delta=(object2.position[coord]+object2.size[coord]-object1.position[coord]);
					object2.position[coord]=(int)object2.position[coord]-(int)((float)delta/2.0);
					object1.position[coord]=(int)object1.position[coord]+(int)((float)delta/2.0)+1;
				}
			}
		}

		//NOTE: we are using an impact structure for touch which may not be appropriate

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj_group"></param>
		public static void TouchProcessor(Object3d[] obj_group)
		{
			/*Discover if any of the objects in the group are touching and call their touch response routines

			   obj_group: A list of objects within the scene: list of class or subclass of Object3d
			*/
			Collider imp = new Collider();
			for (int object1=0;object1<obj_group.Length;object1++)
			{
				for (int object2=object1+1;object2<obj_group.Length;object2++)
				{
					//Detect a touch between object 1 and object 2
					imp=Touch(obj_group[object1],obj_group[object2]);
					if (imp.impact == true)
					{
						//Touch response, call the objects touch event handler
						obj_group[object1].EventTouch(imp.impact,obj_group[object2],imp.impact_face_object1);
						obj_group[object2].EventTouch(imp.impact,obj_group[object1],imp.impact_face_object2);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <returns></returns>
		public static Collider Touch(Object3d object1,Object3d object2)
		{
			/* Detect if two objects are touching
				object1: The first object to be detected: class or subclass of Object3d
				object2: The second object to be detected: class or subclass of Object3d

				imp: a collider with the impact time and faces touched on both objects: class collide
			*/    
			//produce an imaginary collision object based on projecting object 1 in the direction of object 2
			Collider imp = new Collider();
			int[] zero={0,0,0};
			Isotope.Object3d sense_object1=new Isotope.Object3d(zero,zero,0,true);
			//Take the 2 centres of the objects
			int[] twos={2,2,2};
			int[] centre_object1=Vector.AddVector(object1.position,Vector.DivideVector(object1.size,twos));
			int[] centre_object2=Vector.AddVector(object2.position,Vector.DivideVector(object2.size,twos));
			//Find the projected vector between them
			int[] project_vector=Vector.MultiplyVector(Vector.Direction(centre_object1,centre_object2),twos);
			//Add the projected vector to the first objects position
			sense_object1.position=Vector.AddVector(object1.position,project_vector);
			Vector.CopyVector(object1.size,sense_object1.size);
			//collision detect the sense object with the object 2
			imp=CollisionDetect(sense_object1,object2);
			return(imp);
		}
      
		/// <summary>
		/// 
		/// </summary>
		/// <param name="test_object"></param>
		/// <param name="object_group"></param>
		/// <returns></returns>
		public static bool TestCollisionGroup(Object3d test_object,ArrayList object_group)
		{
			/* Checks if the test object collides with any object in the object group

				test_object: The object to be tested for a collision: class or subclass of Object3d
				object_group: A list of objects : list of class or subclass of Object3d
				Return true/false: true for a collision : boolean
			*/
			Collider imp = new Collider();
			for (int obj=0;obj<object_group.Count;obj++)
			{
				//call the collision detector to get the first object collided with
				imp=CollisionDetect(test_object,(Object3d)object_group[obj]);
				if(imp.impact == true)
				{
					return(true);
				}
			}
			return(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source_object"></param>
		/// <param name="drop_object"></param>
		/// <param name="facing"></param>
		/// <param name="separation"></param>
		/// <returns></returns>
		public static int[] DropPosition(Object3d source_object,Object3d drop_object,int[] facing,int separation)
		{
			/* Calculates a position to drop and object in front of the facing object

				source_object: the facing object that is dropping the drop_object: class or subclass of Object3d
				drop_object: the object being dropped: class or subclass of Object3d
				facing: 3d facing vector : list of integers: [x,y,z]

				Notes:
				The drop object is in the direction of the facing vector and projected from the centre of
				the source object to its edge and then an additional amount to the centre of the drop object.
				after that it is offset by half the size of the drop objects size vector.
			*/
			int[] twos={2,2,2};
			int[] drop_half_size=Vector.DivideVector(drop_object.size,twos);
			//The centre of the source object
			int[] source_half_size=Vector.DivideVector(source_object.size,twos);
			int[] source_centre=Vector.AddVector(source_object.position,source_half_size);
			//offset from the centre of the source object to the centre of the drop object   
			int[] displ_vect=Vector.AddVector(Vector.MultiplyVector(facing,drop_half_size),Vector.MultiplyVector(facing,source_half_size));
			//add a small offset to distance it from the source object
			int[] sep={separation,separation,separation};
			int[] offset_vect=Vector.AddVector(displ_vect,Vector.MultiplyVector(facing,sep));
			//produce the offset position of the corner of the drop object)
			int[] corner_vect=Vector.SubtractVector(offset_vect,drop_half_size);
			int[] test_pos=Vector.AddVector(corner_vect,source_centre);
			return(test_pos);
		}

	}
}