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

// This should really be the only file that interfaces with SDL except for Isotope top level class.
// There are routines in skins and isometric. Isometric shouldn't have SDL routines. Skins manages
// images so it may not be neccessary to separate it. 

using System;
using System.Collections;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace SdlDotNetExamples.Isotope
{
    //class sprite_group:
    //   def __init__(self,dimension):
    //      self.num_sprites=dimension
    //      self.sprites=range(dimension)

    static class Sprites
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skin_group"></param>
        /// <param name="object_group"></param>
        /// <returns></returns>
        public static Sprite[] UpdateImages(Skin[] skinGroup, Object3d[] objectGroup)
        {
            /* updates all the images for every object

                object_group: the objects whose state will be analysed to find the right image: list of objects_3d or subclass
                skin_group: a list of skins which will be used to find the correct image for the objects sprite: list of skins
                Returns sprite_group : sprite_group: a list of sprites which hold the new images: list of sprites 
            */
            Sprite[] sprite_group = new Sprite[objectGroup.Length];
            int i = 0;
            foreach (Object3d obj in objectGroup)
            {
                Sprite sprite = new Sprite();
                sprite.Surface = skinGroup[obj.ObjectType].GetImage(obj);
                sprite.Rectangle = sprite.Surface.Rectangle;
                sprite.Surface.TransparentColor = System.Drawing.Color.White;
                sprite.Surface.Transparent = true;
                sprite_group[i] = sprite;
                i++;
            }
            return (sprite_group);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite_group"></param>
        /// <param name="iso_draw_order"></param>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static Rectangle[] OrderedDraw(Sprite[] spriteGroup, int[] isoDrawOrder, Surface surface)
        {
            /*/ Draws the sprites to the surface and returns a rect list for surface update

                sprite_group: a list of sprites which will be plotted on the surface: list of sprites
                order: an array of the object numbers in depth order: list of integers
                surface: The pygame display area to be drawn into: surface
                Returns rect: a list of drawn rectangles for updating : list of rect
            /*/
            Rectangle[] rect = new Rectangle[spriteGroup.Length];
            for (int sp = 0; sp < spriteGroup.Length; sp++)
            {
                //Console.WriteLine(sprite_group[iso_draw_order[sp]].Rectangle.Top);
                //Console.WriteLine(sprite_group[iso_draw_order[sp]].Rectangle.Left);
                rect[sp] = surface.Blit(spriteGroup[isoDrawOrder[sp]], spriteGroup[isoDrawOrder[sp]].Rectangle);
            }
            return (rect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite_rect"></param>
        /// <param name="old_rect"></param>
        /// <returns></returns>
        public static Rectangle[] CombineRectangles(Rectangle[] spriteRect, Rectangle[] oldRect)
        {
            /* Combine the old sprite rectangles with the new sprite rectangles and update those rectangles

                sprite_rect: the list of new sprite rectangles : list of rect
                old_rect: the list of old rectangles for updating : list of rect
                Returns update_rect: the combined list of rectangles : list of rect

                the function ensures that if a new object appears or disappears then the old rectangles or new sprites
                are included in the update and no dirty lines are left on the display */
            ArrayList update_rect;
            Rectangle[] update_rect_array;
            //Case of the rectangle lists being the same length
            if (oldRect.Length > 0 && oldRect.Length == spriteRect.Length)
            {
                //Console.WriteLine("Case1");
                update_rect = new ArrayList(spriteRect.Length);
                for (int rect = 0; rect < spriteRect.Length; rect++)
                {
                    update_rect.Add(Rectangle.Union(spriteRect[rect], oldRect[rect]));
                }
            }
            //Case of the old rectangle list is bigger than the sprite rectangle list        
            else if (oldRect.Length > 0 && oldRect.Length > spriteRect.Length)
            {
                //Console.WriteLine("Case2");
                update_rect = new ArrayList(spriteRect.Length);
                for (int rect = 0; rect < spriteRect.Length; rect++)
                {
                    update_rect.Add(Rectangle.Union(spriteRect[rect], oldRect[rect]));
                }
                for (int rect = spriteRect.Length - 1; rect < oldRect.Length; rect++)
                {
                    update_rect.Add(oldRect[rect]);
                }
            }
            //Case of the old rectangle list is smaller than the sprite rectangle list 
            else if (oldRect.Length > 0 && oldRect.Length < spriteRect.Length)
            {
                //Console.WriteLine("Case3");
                update_rect = new ArrayList(oldRect.Length);
                for (int rect = 0; rect < oldRect.Length; rect++)
                {
                    update_rect.Add(Rectangle.Union(spriteRect[rect], oldRect[rect]));
                }
                for (int rect = oldRect.Length - 1; rect < spriteRect.Length; rect++)
                {
                    update_rect.Add(spriteRect[rect]);
                }
            }
            else
            {
                //Console.WriteLine("Case4");
                //KLUDGY STUFF!! Copy Array into Array List
                update_rect = new ArrayList(spriteRect.Length);
                for (int rect = 0; rect < spriteRect.Length; rect++)
                {
                    update_rect.Add(spriteRect[rect]);
                }
            }
            update_rect_array = new Rectangle[update_rect.Count];
            update_rect.CopyTo(update_rect_array);
            return (update_rect_array);
        }
    }
}