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
using System.Drawing;
using System.Collections;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public static class Isometric
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int[] Transform(int[] coordinate, int[] offset)
        {
            /* Transform 3d coordinates into 2d positions based on the size of the screen and an a y offset

                coord: 3 dimensional coordinate to be transformed to an isometric 2d coordinate: list of 3 integers [x,y,z]
                offset: 2 dimensional offset for the isometric coordinate: list of 2 integers [x,y]
                Returns trans_coord: a 2d isometric coordinate: list of 2 integers [x,y]

                Note: A side effect for speed: isometric transform scales x and y values to 1.118 times their
                actual value and the z scale coordinate stays as 1: therefore all sprites need to be drawn with this ratio. */
            //transformation coordinates that are returned
            if (offset == null)
            {
                throw new ArgumentNullException("offset");
            }
            if (coordinate == null)
            {
                throw new ArgumentNullException("coordinate");
            }
            int[] transCoord ={ 0, 0 };
            //calculate x coordinate
            transCoord[0] = (coordinate[0] - coordinate[1]) + offset[0];
            //calculates y coordinate
            transCoord[1] = ((coordinate[0] + coordinate[1]) >> 1) - coordinate[2] + offset[1];
            return transCoord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectGroup"></param>
        /// <param name="sprite_group"></param>
        /// <param name="offset"></param>
        public static void GroupTransform(Object3d[] objectGroup, Sprite[] spriteGroup, int[] offset)
        {
            /* Calculate the isometric positions of an object group.

                objectGroup: a list of objects for the 3d coordinates to be transformed: list of objects_3d or subclass
                sprite_group: a list of sprites which will be plotted with the isometric coordinates: list of sprites
                offset: 2d vector to add to the isometric coordinates: list of 2 integers [x,y]

                Relies on the same scale system as transform_iso. This is usingant for matching
                sprite dimensions to object coordinates. */
            if (objectGroup == null)
            {
                throw new ArgumentNullException("objectGroup");
            }
            if (spriteGroup == null)
            {
                throw new ArgumentNullException("spriteGroup");
            }
            for (int obj = 0; obj < objectGroup.Length; obj++)
            {
                //finds the isometric coordinate based on the objects position vector and a display offset
                int[] pos = Transform(objectGroup[obj].GetPosition(), offset);
                //Console.WriteLine(pos[0]);
                //Console.WriteLine(pos[1]);
                //Console.WriteLine(objectGroup[obj].size[1]);
                //Console.WriteLine(objectGroup[obj].size[2]);
                //put the new isometric coordinates of the current object into the sprite array
                spriteGroup[obj].X += pos[0] - objectGroup[obj].GetSize()[1];
                spriteGroup[obj].Y += pos[1] - objectGroup[obj].GetSize()[2];
                //sprite_group[obj].Rectangle.Offset(pos[0]-objectGroup[obj].size[1],pos[1]-objectGroup[obj].size[2]);
                //Console.WriteLine(sprite_group[obj].Rectangle.Top);
                //Console.WriteLine(sprite_group[obj].Rectangle.Left);
                //sprite_group[obj].Rectangle.Top=pos[1]-objectGroup[obj].size[2];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectGroup"></param>
        /// <returns></returns>
        public static int[] Order(Object3d[] objectGroup)
        {
            /* Sorts the objects into a depth order for drawing in the isometric view

                objectGroup: a list of objects to be depth sorted: list of objects_3d or subclass
                Returns order: an array of the object numbers in depth order: list of integers

                Note: Under rare conditions this algorithm will not produce a perfect
                representation due to the use of single sprites for any dimension boundary box.
                This is most obvious when 3 long objects overlap in a three way tie */

            //define the array to return with the object number in an isometric order
            //int[] ordered;
            if (objectGroup == null)
            {
                throw new ArgumentNullException("objectGroup");
            }
            int[] ordered = new int[objectGroup.Length];
            for (int i = 0; i < objectGroup.Length; i++)
            {
                ordered[i] = i;
            }

            //define front precalculate matrix
            int[][] front;
            front = new int[objectGroup.Length][];//[objectGroup.Length];
            //for(int i=0;i<=objectGroup.Length;i++)
            //   front[i]=i;
            //for i in range(3):
            //   front[i]=range(len(objectGroup))

            int[] negones ={ -1, -1, -1 };

            //precalculate the front position of each objects coordinates
            for (int obj = 0; obj < objectGroup.Length; obj++)
            {
                front[obj] = Vector.AddVector(objectGroup[obj].GetPosition(), Vector.AddVector(objectGroup[obj].GetSize(), negones));
            }

            int swap;
            //sort the objects, based on x then y then z of the objects being in front of the other object
            for (int i = 0; i < objectGroup.Length; i++)
            {
                for (int j = 0; j < objectGroup.Length - 1; j++)
                {
                    for (int k = 0; k <= 2; k++)
                    {
                        if (objectGroup[ordered[j]].GetPosition()[k] > front[ordered[j + 1]][k])
                        {
                            swap = ordered[j + 1];
                            ordered[j + 1] = ordered[j];
                            ordered[j] = swap;
                            break;
                        }
                    }
                }
            }
            return ordered;
        }

        // NOTE: The below routines have dependencies on the graphics subsystem and should be
        // Moved into a separate file so that this file can be system independent.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="objectGroup"></param>
        /// <param name="skin_group"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Rectangle[] ViewDraw(Surface view, Object3d[] objectGroup, Skin[] skinGroup, int[] offset)
        {
            /* Draw the sprites to the screen based on isometric ordered sorting

                objectGroup: a list of objects to be displayed: list of objects_3d or subclass
                skin_group: a list of skins which will be used to find the correct image for the objects sprite: list of skins
                offset: 2d vector to add to the isometric coordinates: list of 2 integers [x,y]
                Returns rect: A list of pygame rectangles where the sprites were drawn : list of rect
            */
            //Calculate the isometric order of drawing the sprites from object information
            int[] draw_order = Order(objectGroup);
            //Put the correct images for all the skins into a sprite group
            Sprite[] sprite_group = Sprites.UpdateImages(skinGroup, objectGroup);
            //Calculate the screen coordinates of the sprites from the object data
            GroupTransform(objectGroup, sprite_group, offset);
            //Draw sprites in isometric order to the screen
            Rectangle[] rect;
            rect = (Rectangle[])Sprites.OrderedDraw(sprite_group, draw_order, view);
            return (rect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="objectGroup"></param>
        /// <param name="skin_group"></param>
        /// <param name="offset"></param>
        /// <param name="old_rect"></param>
        /// <returns></returns>
        public static Rectangle[] ViewUpdate(Surface surface, Object3d[] objectGroup, Skin[] skinGroup, int[] offset, Rectangle[] oldRect)
        {
            /* Update the isometric view based only on the changes in the screen

                surface: The pygame display area to be drawn into: surface
                object_group: a list of objects to be displayed: list of objects_3d or subclass
                skin_group: a list of skins which will be used to find the correct image for the objects sprite: list of skins
                offset: 2d vector to add to the isometric coordinates: list of 2 integers [x,y]
                old_rect: A list of pygame rectangles where the old sprites were drawn for updating: list of rect
                Returns old_rect: see above
            */
            if (skinGroup == null)
            {
                throw new ArgumentNullException("skinGroup");
            }
            if (objectGroup == null)
            {
                throw new ArgumentNullException("objectGroup");
            }
            if (surface == null)
            {
                throw new ArgumentNullException("surface");
            }

            // Find out what objects are visable
            int visable_limit = skinGroup.Length;
            //Object3d[] visable_object_group;
            ArrayList visable_object_list = new ArrayList();
            foreach (Object3d obj in objectGroup)
            {
                if (obj.ObjectType < visable_limit)
                {
                    visable_object_list.Add(obj);
                }
            }
            Object3d[] visable_object_group = new Object3d[visable_object_list.Count];
            visable_object_list.CopyTo(visable_object_group);

            // Draw the isometric view in the display surface
            Rectangle[] sprite_rect = ViewDraw(surface, (Object3d[])visable_object_group, skinGroup, offset);

            // Combines the rectangles that need updating: the new sprites and the old background rectangles   
            Rectangle[] update_rect = Sprites.CombineRectangles(sprite_rect, oldRect);

            // Update the display
            surface.Update(update_rect);

            // Remember the sprite rectangles
            oldRect = new Rectangle[sprite_rect.Length];
            for (int rect = 0; rect < sprite_rect.Length; rect++)
            {
                oldRect[rect] = sprite_rect[rect];
            }
            return (oldRect);
        }
    }
}