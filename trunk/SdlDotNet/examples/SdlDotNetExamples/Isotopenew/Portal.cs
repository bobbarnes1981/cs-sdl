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
	/// 
	/// </summary>
	public class Portal : Object3d
	{
		Scene dest_scene;
		int[] dest_pos={0,0,0};
		/* An object which can signal to lead actors for a scene change if they touch the portal */
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="size"></param>
		/// <param name="objtype"></param>
		/// <param name="dest_scene"></param>
		/// <param name="dest_pos"></param>
		public Portal(int[] pos,int[] size,int objtype, Scene dest_scene,int[] dest_pos)
			: base(pos,size,objtype,true)
		{
			this.dest_scene=dest_scene;
			Vector.CopyVector(dest_pos,this.dest_pos);
		}
	   
		/// <summary>
		/// 
		/// </summary>
		/// <param name="impact"></param>
		/// <param name="other_obj"></param>
		/// <param name="impact_face"></param>
		public override void EventTouch(bool impact,Object3d other_obj,int impact_face)
		{
			if (other_obj is LeadActor)
			{
				((LeadActor)other_obj).EventChangeScene(dest_scene,dest_pos);
			}
		}
	}
}
