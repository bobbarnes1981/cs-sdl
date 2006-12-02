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

namespace SdlDotNetExamples.Isotope
{
	/* class exploder(object_gravity):
	   """ a class for objects which delete themselves when they collide with another object """
	   def __init__(self,pos,size,objtype,scene,fixed=False):
		  object_gravity.__init__(self,pos,size,objtype,fixed)
		  self.die=False
		  self.scene=scene

	   def tick(self):
		  object_gravity.tick(self)
		  if self.die is True:
			 self.scene.object_group.remove(self)

	   def event_collision(self,other_obj,impact_face):
		  self.die=True

	class exploder_random_creator(Object3d):
	   def __init__(self,pos,size,objtype,scene,fixed=True):
		  Object3d.__init__(self,pos,size,objtype,fixed)
		  self.new_object_time=0
		  self.scene=scene

	   def tick(self):
		  Object3d.tick(self)
		  if self.new_object_time == 100:
			 pos=[randint(20,130),randint(20,130),100]
			 self.scene.object_group.append(exploder(pos,[30,30,30],8,self.scene))
			 self.new_object_time=0
		  self.new_object_time=self.new_object_time+1

	*/

	public class Disolver : ObjectGravity
	{
		//bool die;
		int new_object_time=0;
		Scene scene;
		// a class for objects which delete themselves after a set period of ticks
		public Disolver(int[] pos,int[] size,int objtype, Scene scene)
			:base(pos,size,objtype,false)
		{
			//die=false;
			new_object_time=0;
			this.scene=scene;
		}
		/// <summary>
		/// 
		/// </summary>
		public override void Tick()
		{
			base.Tick();
			if (new_object_time == 20)
			{
				//pos=new int[]{System.Random.Next(20,130),System.Random.Next(20,130),100};
				scene.ObjectGroup.Remove(this);
			}
			new_object_time=new_object_time+1;
		}
	}
}