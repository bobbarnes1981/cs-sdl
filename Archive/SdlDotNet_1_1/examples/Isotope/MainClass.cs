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

// project created on 05/06/2006 at 14:56
using System;
using System.IO;
using System.Collections;
using SdlDotNet;
using System.Drawing;

namespace SdlDotNet.Examples.Isotope
{
	/// <summary>
	/// 
	/// </summary>
	public class MainClass
	{	
		static string filePath;
		/// <summary>
		/// 
		/// </summary>
		public static string FilePath
		{
			get
			{
				return filePath;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{

			filePath = Path.Combine("..", "..");
			string fileDirectory = "Data";
			string fileName = "amp.png";
			if (File.Exists(fileName))
			{
				filePath = "";
				fileDirectory = "";
			}
			else if (File.Exists(Path.Combine(fileDirectory, fileName)))
			{
				filePath = "";
			}

			filePath = Path.Combine(filePath, fileDirectory);

			string file = Path.Combine(filePath, fileName);

			// Setup the pygame display, the window caption and its icon

			Video.WindowIcon();
			Video.WindowCaption = "SDL.NET - Isotope";
			Surface surface = Video.SetVideoModeWindow(400,360);
            
			// Setup the two scenes of a bedroom and a lounge with Ian Curtis as the lead actor  

			ArrayList joy_world=new ArrayList();
		
			Scene bedroom = new Scene(0,new ArrayList());
			Scene lounge = new Scene(1,new ArrayList());
			joy_world.Add(bedroom);
			joy_world.Add(lounge);

			// Scene 0

			ObjectPortable guitar=new ObjectPortable(new int[]{60,0,40},new int[]{20,12,20},3,false);
			bedroom.AppendObject(guitar);

			Object3d bed=new Object3d(new int[]{10,100,0},new int[]{70,52,28},6,false);
			bedroom.AppendObject(bed);

			LeadActor ian_curtis=new LeadActor(new int[]{90,90,0},new int[]{14,14,50},4,bedroom,false);
			bedroom.AppendObject(ian_curtis);

			Portal door=new Portal(new int[]{180,105,0},new int[]{10,30,56},5,lounge,new int[]{10,115,0});
			bedroom.AppendObject(door);
		
			DisolverRandomCreator ampfactory=new DisolverRandomCreator(new int[]{180,105,60},new int[]{10,10,10},4000,bedroom,true);
			bedroom.AppendObject(ampfactory);

			// walls and floor
			Object3d ground=new Object3d(new int[]{-1000,-1000,-100},new int[]{2000,2000,100},4000,true);
			bedroom.AppendObject(ground);

			Object3d wall0=new Object3d(new int[]{180,0,-20},new int[]{20,180,120},4000,true);
			bedroom.AppendObject(wall0);

			Object3d wall1=new Object3d(new int[]{0,180,-20},new int[]{180,20,120},4000,true);
			bedroom.AppendObject(wall1);

			Object3d wall2=new Object3d(new int[]{0,-20,-20},new int[]{180,20,120},4000,true);
			bedroom.AppendObject(wall2);
		 
			Object3d wall3=new Object3d(new int[]{-20,0,-20},new int[]{20,180,120},4000,true);
			bedroom.AppendObject(wall3);

			// Scene 1
			Object3d sofa=new Object3d(new int[]{0,0,0},new int[]{39,66,37},7,false);
			lounge.AppendObject(sofa);

			ObjectPortable amp=new ObjectPortable(new int[]{60,0,25},new int[]{16,10,18},2,false);
			lounge.AppendObject(amp);

			Portal door2=new Portal(new int[]{0,105,0},new int[]{10,30,56},5,bedroom,new int[]{160,115,0});
			lounge.AppendObject(door2);

			// walls and floor
			Object3d ground2=new Object3d(new int[]{-1000,-1000,-100},new int[]{2000,2000,100},4000,true);
			lounge.AppendObject(ground2);

			Object3d wall4=new Object3d(new int[]{180,0,-20},new int[]{20,180,120},4000,true);
			lounge.AppendObject(wall4);

			Object3d wall5=new Object3d(new int[]{0,180,-20},new int[]{180,20,120},4000,true);
			lounge.AppendObject(wall5);

			Object3d wall6=new Object3d(new int[]{0,-20,-20},new int[]{180,20,120},4000,true);
			lounge.AppendObject(wall6);
		 
			Object3d wall7=new Object3d(new int[]{-20,0,-20},new int[]{20,180,120},4000,true);
			lounge.AppendObject(wall7);

			// Images for the Backgrounds and the objects

			Skin[] skin_group=new Skin[8];

			ArrayList bedroom_image=SkinsLib.LoadImages(new string[]{Path.Combine(filePath, "bedroom.png")});
			skin_group[0]=(new Skin(bedroom_image,"Bedroom"));

			ArrayList lounge_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "lounge.png")});
			skin_group[1]=(new Skin(lounge_image,"Lounge"));
		
			ArrayList amp_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "amp.png")});
			skin_group[2]=(new Skin(amp_image,"Amp"));
		
			ArrayList guitar_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "guitar.png")});
			skin_group[3]=(new Skin(guitar_image,"Guitar"));
		
			ArrayList ian_curtis_images=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "ian_curtis0.png"),Path.Combine(filePath, "ian_curtis5.png"),Path.Combine(filePath, "ian_curtis3.png"),Path.Combine(filePath, "ian_curtisF4.png"),
																			  Path.Combine(filePath, "ian_curtisF6.png"),Path.Combine(filePath, "ian_curtisF7.png"),Path.Combine(filePath, "ian_curtisF0.png"),Path.Combine(filePath, "ian_curtisF5.png"),Path.Combine(filePath, "ian_curtisF3.png"),Path.Combine(filePath, "ian_curtis4.png"),Path.Combine(filePath, "ian_curtis6.png"),Path.Combine(filePath, "ian_curtis7.png")});
			skin_group[4]=(Skin)new Pointing(ian_curtis_images,"Ian Curtis");

			// Mirror the images to complete the animation
			//for(int i=3;i<9;i++)
			//   ((Surface)ian_curtis_images[i]).FlipHorizontal();
		 
			ArrayList door_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "door.png")});
			skin_group[5]=(new Skin(door_image,"Door")); 
		
			ArrayList bed_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "bed.png")});
			skin_group[6]=(new Skin(bed_image,"Bed"));
		
			ArrayList sofa_image=SkinsLib.LoadImages(new string[] {Path.Combine(filePath, "sofa.png")});
			skin_group[7]=(new Skin(sofa_image,"Sofa"));

			Keys joykeys=new Keys(Key.O,Key.P,Key.A,Key.Z,
				Key.M,Key.G,Key.H,Key.B,Key.U);

			// Create an isotope engine using the skin_group and the scene_group
			Engine joy_engine=new Engine(ian_curtis,skin_group,surface,joykeys,Path.Combine(filePath, "titlebar.png"));

			// Start the isotope engine
			joy_engine.Start();
		}
	}
}