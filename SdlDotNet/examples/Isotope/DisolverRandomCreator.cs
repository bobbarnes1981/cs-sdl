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

namespace SdlDotNet.Examples.Isotope
{
	/// <summary>
	/// 
	/// </summary>
	public class DisolverRandomCreator : Object3d
	{
		int new_object_time=0;
		Scene scene;
		System.Random randint = new System.Random();
		// Creator class which makes disolver objects
		public DisolverRandomCreator(int[] pos,int[] size,int objtype, Scene scene,bool fixedob):
			base(pos,size,objtype,fixedob)
		{
			new_object_time=0;
			this.scene=scene;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void Tick()
		{
			base.Tick();
			if (new_object_time == 1)
			{
				position=new int[]{randint.Next(10,170),randint.Next(10,170),100};
				this.scene.ObjectGroup.Add(new Disolver(position,new int[]{16,10,18},2,scene));
				new_object_time=0;
			}
			new_object_time=new_object_time+1;
		}
	}
}