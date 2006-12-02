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
		public Monster(int[] pos,int[] size,int objtype,bool fixedob):base(pos,size,objtype,fixedob)
		{
			int[] twos={2,0,0};
			vel.CopyTo(twos,0);
		}
	   
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other_obj"></param>
		/// <param name="impact_face"></param>
		public new void EventCollision(Object3d other_obj,int impact_face)
		{
			/*/ Turn around 180 degrees and continue walking /*/
			//call actors standard collision code
			base.EventCollision(other_obj,impact_face);
			int[] ones={1,0,0};
			int[] negones={-1,0,0};
			// simple toggle movement
			if (coltime == ObjectTime.GetTime())
			{
				if (Facing==ones && impact_face == 0)
				{
					vel[0]=-2;
					Facing=negones;
				}
				else if (Facing==negones && impact_face == 1)
				{
					vel[0]=2;
					Facing=ones;
				}
				//WARNING MISSING TIME FUNCTION MUST BE FIXED
				coltime=ObjectTime.GetTime();
			}
		}
	}
}