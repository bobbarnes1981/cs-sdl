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

// Defines the low level interface elements for Isotope

using System;
using SdlDotNet;
using System.Drawing;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public static class Simulator
    {
        // simulates a 3 dimensional scene

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        public static void Update(Scene scene)
        {
            /*Updates the physics simulation of the objects

               scene: scene to update: scene class
            */
            // Object tick: let each object run its tick() action.
            // make a copy of the object pointers in case the objects remove themselves from the master list
            if (scene == null)
            {
                throw new ArgumentNullException("scene");
            }
            Object3d[] update_group = new Object3d[scene.ObjectGroup.Count];
            scene.ObjectGroup.CopyTo(update_group, 0);
            foreach (Object3d obj in update_group)
            {
                obj.Tick();
            }
            // Note: the objects must not modify the object lists or positions in their event functions called by the
            // Collision and Touch processors.
            // Detect collisons
            update_group = new Object3d[scene.ObjectGroup.Count];
            scene.ObjectGroup.CopyTo(update_group, 0);
            //Console.WriteLine("Simulator Update.Collision_Processor");
            Physics.CollisionProcessor(update_group);
            // Detect touches
            //Console.WriteLine("Simulator Update.Physics.touch_processor");
            Physics.TouchProcessor(update_group);
        }
    }
}