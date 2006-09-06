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

using System;
using System.Drawing;

namespace SdlDotNet.Examples.Isotope
{
	/// <summary>
	/// 
	/// </summary>
	public class View
	{
		/*Draws a scene in an isometric view 

		   surface: The surface to draw the isometric view into: surface class
		   scene: The scene to draw in the isometric view: scene class
		   skin_group: The skins to use for the images representing the objects: list of skin class
		   display_offset: 2 dimensional vector of the offset for the 0,0,0 origin point
			  in the view: list of 2 integers [x,y]

		   old_rect: The areas of the view that are tagged to be updated: list of rect class
		*/   

		public int[] ViewSize={400,300};
		//remember the surface
		public Surface Surface;
		//offset from the top left corner of the window for the isotope display
		public int[] DisplayOffset={200,150};
		public Rectangle[] OldRect={};
      
		// Initialize
		public View(Surface surface,Scene scene, Skin[] skin_group,int[] display_offset)
		{
			//Dimensions of the window for the isotope display
			//view_size=(int[])surface.Size;
			//remember the surface
			this.Surface=surface;
			//offset from the top left corner of the window for the isotope display
			this.DisplayOffset=display_offset;
			RedrawDisplay(scene,skin_group);
		}
	   
		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="skin_group"></param>
		public void DisplayUpdate(Scene scene, Skin[] skin_group)
		{
			// Updates the isometric display using update rectangles
			// Clear the old sprite positions with the background
			Surface background=(Surface)skin_group[scene.sceneType].images[0];
			//Surface subsurface;
			if (OldRect.Length>0)
			{
				foreach (Rectangle clear_rect in OldRect)
				{
					//subsurface=background.CreateSurfaceFromClipRectangle(clear_rect);
					Surface.Blit(background,clear_rect,clear_rect);
				}
			}
			// Update Isometric view
			// Copy the scene objects Array List KLUDGY!!!
			Object3d[] objectGroup=new Object3d[scene.ObjectGroup.Count];
			//for(int obj=0;obj<=scene.objectGroup.Count;obj++)
			//   objectGroup[obj]=scene.objectGroup[obj];
			scene.ObjectGroup.CopyTo(objectGroup,0);
			OldRect=Isometric.ViewUpdate(Surface,objectGroup,skin_group,
				DisplayOffset, OldRect);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="skin_group"></param>
		public void RedrawDisplay(Scene scene, Skin[] skin_group)
		{
			// Redraws the entire display, including background
			//Display the background
			Console.WriteLine(scene);
			Surface.Blit((Surface)skin_group[scene.sceneType].images[0],((Surface)skin_group[scene.sceneType].images[0]).Rectangle);
			Surface.Flip();
			DisplayUpdate(scene,skin_group);
		}
	}
}
