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
    /// <summary>
    /// 
    /// </summary>
    public class DissolverRandomCreator : Object3d
    {
        int newObjectTime;
        Scene scene;
        System.Random randint = new System.Random();
        // Creator class which makes disolver objects
        public DissolverRandomCreator(int[] position, int[] size, int objectType, Scene scene, bool fixedObject)
            :
            base(position, size, objectType, fixedObject)
        {
            this.scene = scene;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Tick()
        {
            base.Tick();
            if (newObjectTime == 1)
            {
                SetPosition(new int[] { randint.Next(10, 170), randint.Next(10, 170), 100 });
                this.scene.ObjectGroup.Add(new Dissolver(GetPosition(), new int[] { 16, 10, 18 }, 2, scene));
                newObjectTime = 0;
            }
            newObjectTime = newObjectTime + 1;
        }
    }
}