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
	//Define the gametime global variable
	public class ObjectTime 
	{
		public static int time;
		/// <summary>
		/// 
		/// </summary>
		public ObjectTime()
		{
			time=0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static int GetTime()
		{
			return(time);
		}
		/// <summary>
		/// 
		/// </summary>
		public static void UpdateTime()
		{
			time = time + 1;
		}
	}
}
