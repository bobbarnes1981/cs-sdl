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
using System.Collections;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public class LeadActor : Actor
    {
        /* Class capable of travelling between scenes and picking up and dropping objects
	 
            scene: the current scene of the lead actor: scene class

            inventory: the objects that the lead actor is carrying: list of Object3d class or subclass
            usingob: the current object that is in the hand of the actor: integer
            max_inventory: the maximum number of objects the lead actor can carry: integer
            drop_command: Flag for the event handler to show when a drop request has been received: boolean
            pick_up_command: Flag for the event handler to show when a pick_up request has been received: boolean
            new_pos: Used when changing scenes for new postion vector: list of 3 integers [x,y,z]
            new_scene: Used when changing scenes for the new scene: scene class
	       
            * Note: This class directly modifies the scene list. Care must be taken
            when writing any code which does this as objects disappear when other actors may think
            they are still available.
        */
        private Scene newScene;

        public Scene NewScene
        {
            get { return newScene; }
            set { newScene = value; }
        }
        private int[] newPosition ={ 0, 0, 0 };

        private Scene scene;

        public Scene Scene
        {
            get { return scene; }
            set { scene = value; }
        }
        //Flags for pick up and drop messages from external control: perhaps this means we really need a player object
        private bool pickupCommand;

        public bool PickupCommand
        {
            get { return pickupCommand; }
            set { pickupCommand = value; }
        }
        private bool dropCommand;

        public bool DropCommand
        {
            get { return dropCommand; }
            set { dropCommand = value; }
        }
        private ArrayList inventory = new ArrayList();

        public ArrayList Inventory
        {
            get { return inventory; }
        }
        private int maxInventory = 4;

        public int MaxInventory
        {
            get { return maxInventory; }
            set { maxInventory = value; }
        }
        //object being used
        private int usingObject;

        public int UsingObject
        {
            get { return usingObject; }
            set { usingObject = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="objtype"></param>
        /// <param name="scene"></param>
        /// <param name="fixedob"></param>
        public LeadActor(int[] pos, int[] size, int objectType, Scene scene, bool fixedObject)
            : base(pos, size, objectType, fixedObject)
        {
            newScene = scene;
            newPosition = pos;
            this.scene = scene;
            //Flags for pick up and drop messages from external control: perhaps this means we really need a player object
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Tick()
        {
            //System.Console.WriteLine("Lead Actor Tick entry");
            /*/Redefined tick function to allow movement between scenes /*/
            if (scene != newScene)
            {
                Vector.CopyVector(newPosition, GetPosition());
                newScene.ObjectGroup.Add(this);
                scene.ObjectGroup.Remove(this);
                scene = newScene;
            }
            if (pickupCommand == true)
            {
                Pickup();
                pickupCommand = false;
            }
            if (dropCommand == true)
            {
                Drop();
                dropCommand = false;
            }
            base.Tick();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Pickup()
        {
            /* pick_up object action */
            int face = Vector.VectorToFace(GetFacing());
            for (int i = 0; i < TouchedObjects.Count; i++)
            {

                // Pick up the first object we are touching
                if (face == (int)TouchedFaces[i] && TouchedObjects[i] is ObjectPortable
                    && inventory.Count < maxInventory)
                {
                    //System.Console.WriteLine("Pick up");
                    ObjectPortable pick_up_object = (ObjectPortable)TouchedObjects[i];
                    //print touched_objects[i],scene.object_group
                    if (pick_up_object.RequestPickup() == true)
                    {
                        inventory.Add(pick_up_object);
                        scene.ObjectGroup.Remove(pick_up_object);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Drop()
        {
            /*/ drop object action /*/
            // Check if we are carrying something to drop
            // Routine does not take into account facing rotation of an object
            if (inventory.Count == 0)
            {
                return;
            }
            // Get the candidate object to be dropped
            ObjectPortable drop_object = (ObjectPortable)inventory[usingObject];
            // Test if there is space for the object to be dropped
            // Create a test object to put in the drop position
            int[] test_pos = Physics.DropPosition(this, drop_object, GetFacing(), 4);
            Object3d test_object = new Object3d(test_pos, drop_object.GetSize(), 0, false);
            // Check if the test object collides with any other object in the scene
            if (Physics.TestCollisionGroup(test_object, scene.ObjectGroup) == true)
            {
                return;
            }
            // Put the object into the current scene by adding it to the scenes object list
            // Check if the object wants to be dropped
            if (drop_object.RequestDrop() == false)
            {
                return;
            }
            // Drop the object
            Vector.CopyVector(test_object.GetPosition(), drop_object.GetPosition());
            inventory.Remove(drop_object);
            scene.ObjectGroup.Add(drop_object);
            if (usingObject != 0)
            {
                usingObject = usingObject - 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="pos"></param>
        public void EventChangeScene(Scene scene, int[] pos)
        {
            /* Event handler for a requested change scene, usually asked by doors who touched this lead actor
	          
                scene: the new scene to change the lead actor to: scene class
                pos: the new position vector in the new scene: list of 3 integers [x,y,z]
            */
            newScene = scene;
            Vector.CopyVector(pos, newPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool EventPickup()
        {
            /*Pick up event handler*/
            pickupCommand = true;
            return (true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool EventDrop()
        {
            /*Drop event handler*/
            dropCommand = true;
            return (true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool EventUsingObject()
        {
            // Event handler for request to use the next object in the inventory
            if (inventory.Count > 0)
            {
                usingObject = (usingObject + 1) % inventory.Count;
            }
            return (true);
        }
    }
}