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

using System.Collections;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public class Scene
    {
        /* A collection of objects and a scenetype to hint at a background image */
        private int sceneType;

        public int SceneType
        {
            get { return sceneType; }
            set { sceneType = value; }
        }
        ArrayList objectGroup;
        /// <summary>
        /// 
        /// </summary>
        public ArrayList ObjectGroup
        {
            get
            {
                return objectGroup;
            }
            //set
            //{
            //    objectGroup = value;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneType"></param>
        /// <param name="objectGroup"></param>
        public Scene(int sceneType, ArrayList objectGroup)
        {
            this.sceneType = sceneType;
            this.objectGroup = objectGroup;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="object1"></param>
        public void AppendObject(Object3d object1)
        {
            objectGroup.Add(object1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object1"></param>
        public void RemoveObject(Object3d object1)
        {
            objectGroup.Remove(object1);
        }
    }
}