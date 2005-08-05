/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Drawing;
using System.Threading;
using System.Globalization;

using SdlDotNet;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNet.Examples 
{
	/// <summary>
	/// 
	/// </summary>
	class PrimitivesExample
	{
		private bool quitFlag;
		
		/// <summary>
		/// 
		/// </summary>
		public PrimitivesExample() 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public void Run() 
		{
			int width = 640;
			int height = 480;
			Random rand = new Random();
			
			Events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			Events.Quit += new QuitEventHandler(this.Quit);
			
			try 
			{
				// set the video mode
				Surface screen = Video.SetVideoModeWindow(width, height, true); 
				Video.WindowCaption = "SdlDotNet - Primitives Example";
				Video.Mouse.ShowCursor = false;

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				//fill the surface with black
				surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
				Circle circle;
				Ellipse ellipse;
				Line line;
				Triangle triangle;
				Polygon polygon;
				//Pie pie;
				Bezier bezier;
				Box box;

				const int MAXCOUNT = 3;
				const int SLEEPTIME = 200;
				int times = 0;
		
				while (!quitFlag) 
				
				{
					while (Events.Poll()) 
					{
						// handle events till the queue is empty
					} 
					
					try 
					{
						while (times < MAXCOUNT)
						{
							circle = new Circle(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height), 
								(short)rand.Next(20, 100));
							surf.DrawFilledCircle(circle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							circle = new Circle(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height), 
								(short)rand.Next(20, 100));
							surf.DrawCircle(circle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						while (times < MAXCOUNT)
						{
							ellipse = new Ellipse(
								(short)rand.Next(0, width),
								(short)rand.Next(0, height), 
								(short)rand.Next(20, 100),
								(short)rand.Next(20,100));
							surf.DrawEllipse(ellipse,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255) ,
								rand.Next(255)));
							ellipse = new Ellipse(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(20, 100), 
								(short)rand.Next(20,100));
							surf.DrawFilledEllipse(ellipse,
								Color.FromArgb(rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						while (times < MAXCOUNT)
						{
							line = new Line(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height));
							surf.DrawLine(line,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						while (times < MAXCOUNT)
						{
							triangle = new Triangle(
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2),
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2), 
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2));
							surf.DrawTriangle(triangle,
								Color.FromArgb(
								rand.Next(255),
								rand.Next(255), 
								rand.Next(255) 
								,rand.Next(255)));
							triangle = new Triangle(
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2),
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2), 
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2));
							surf.DrawFilledTriangle(triangle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						while (times < MAXCOUNT)
						{
							short[] x = {
											(short)rand.Next(0, width), 
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width)
										};
							short[] y = {
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height)
										};
							polygon = new Polygon(x, y);
							surf.DrawPolygon(polygon, 
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255)));
							short[] a = {
											(short)rand.Next(0, width), 
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width)
										};
							short[] b = {
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height),
											(short)rand.Next(0, height)
										};
							polygon = new Polygon(a, b);
							surf.DrawFilledPolygon(polygon, 
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 
// //Ubuntu Linux has an old version of SDL_gfx that does not support the Pie primitive.
//						while (times < MAXCOUNT)
//						{
//							pie = new Pie((short)rand.Next(0, width), 
//								(short)rand.Next(0, height), 
//								(short)rand.Next(20, 100), 
//								(short)rand.Next(0, 360), 
//								(short)rand.Next(0, 360));
//
//							surf.DrawPie(pie, 
//								Color.FromArgb(
//								rand.Next(255), 
//								rand.Next(255), 
//								rand.Next(255) ,
//								rand.Next(255)));
//							pie = new Pie((short)rand.Next(0, width), 
//								(short)rand.Next(0, height) , 
//								(short)rand.Next(20, 100), 
//								(short)rand.Next(0, 360), 
//								(short)rand.Next(0, 360));
//
//							surf.DrawFilledPie(pie, 
//								Color.FromArgb(rand.Next(255), 
//								rand.Next(255), 
//								rand.Next(255),
//								rand.Next(255)));
//							times++;
//							screen.Flip();
//							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
//							Thread.Sleep(SLEEPTIME);
//						}
//
//						Thread.Sleep(SLEEPTIME);
//						times = 0;
//						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
//						while (Events.Poll()) 
//						{
//							// handle events till the queue is empty
//						} 
						
						while (times < MAXCOUNT)
						{
							short[] c = {(short)rand.Next(0, width), 
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width),
											(short)rand.Next(0, width)};
							short[] d = {(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height), 
											(short)rand.Next(0, height),
											(short)rand.Next(0, height)};

							bezier = new Bezier(c, d, 0);
							surf.DrawBezier(bezier, 
								Color.FromArgb(rand.Next(255), 
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						while (times < MAXCOUNT)
						{
							box = new Box(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height));
							surf.DrawBox(box,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							box = new Box(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height));
							surf.DrawFilledBox(box,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}
						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

						while (Events.Poll()) 
						{
							// handle events till the queue is empty
						} 

						int xpixel;
						int ypixel;
						int rpixel;
						int gpixel;
						int bpixel;
						int colorValue;

						while (times < 100)
						{
							xpixel = rand.Next(10,width);
							ypixel = rand.Next(10,height);
							rpixel = rand.Next(255);
							gpixel = rand.Next(255);
							bpixel = rand.Next(255);

							colorValue = surf.GetColorValue(Color.FromArgb(rpixel, gpixel, bpixel));
							//colorValue = screen.MapColor(Color.FromArgb(254, 0, 0));
							//screen.DrawPixel(x, y, Color.Red);
							Console.WriteLine("colorValue: " + colorValue.ToString(CultureInfo.CurrentCulture));
							surf.DrawPixel(xpixel, ypixel, Color.FromArgb(rpixel, gpixel, bpixel));
							//screen.DrawPixel(x, y, Color.Red);
							Console.WriteLine("GetPixel: " + screen.GetPixel(xpixel, ypixel).ToString());
							Console.WriteLine("GetPixel: " + screen.GetColorValue(screen.GetPixel(xpixel, ypixel)).ToString(CultureInfo.CurrentCulture));
							times++;
							screen.Flip();
							screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
						}
					} 
					catch (SurfaceLostException) 
					{
						// if we are fullscreen and the user hits alt-tab 
						// we can get this, for this simple app we can ignore it
					}
				}
			} 
			catch 
			{
				//sdl.Dispose(); 
				// quit sdl so the window goes away, then handle the error...
				throw; // for this example we'll just throw it to the debugger
			}
		}

		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e) 
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
		}

		private void Quit(object sender, QuitEventArgs e) 
		{
			quitFlag = true;
		}

		[STAThread]
		static void Main() 
		{
			PrimitivesExample primitivesExample = new PrimitivesExample();
			primitivesExample.Run();
		}
	}
}
