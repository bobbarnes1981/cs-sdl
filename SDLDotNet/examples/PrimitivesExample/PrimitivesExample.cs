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
using SdlDotNet;

// Simple SDL.NET Example
// Just draws a bunch of rectangles to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNet.Examples 
{
	public class PrimitivesExample
	{
		private bool quitFlag;
		

		public PrimitivesExample() 
		{
			quitFlag = false;
		}

		public void Run() 
		{
			int width = 640;
			int height = 480;
			Random rand = new Random();
			

			Video video = Video.Instance;
			WindowManager wm = WindowManager.Instance;
			Events events = Events.Instance;
			
			events.KeyboardDown += 
				new KeyboardEventHandler(this.KeyboardDown); 
			events.Quit += new QuitEventHandler(this.Quit);
			
			try 
			{
				// set the video mode
				Surface screen = video.SetVideoModeWindow(width, height, true); 
				wm.Caption = "Primitives Example";
				video.HideMouseCursor();

				Surface surf = 
					screen.CreateCompatibleSurface(width, height, true);
				//fill the surface with black
				surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
				Circle circle;
				Ellipse ellipse;
				Line line;
				Triangle triangle;
				Polygon polygon;
				Pie pie;
				Bezier bezier;
				Box box;

				const int MAXCOUNT = 3;
				const int SLEEPTIME = 200;
				int times = 0;
		
				while (!quitFlag) 
				
				{
					while (events.PollAndDelegate()) 
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
							surf.CreateFilledCircle(circle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							circle = new Circle(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height), 
								(short)rand.Next(20, 100));
							surf.CreateCircle(circle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						while (events.PollAndDelegate()) 
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
							surf.CreateEllipse(ellipse,
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
							surf.CreateFilledEllipse(ellipse,
								Color.FromArgb(rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

						while (times < MAXCOUNT)
						{
							line = new Line(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height));
							surf.CreateLine(line,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

						while (times < MAXCOUNT)
						{
							triangle = new Triangle(
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2),
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2), 
								(short)rand.Next(0, width/2), 
								(short)rand.Next(0, height/2));
							surf.CreateTriangle(triangle,
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
							surf.CreateFilledTriangle(triangle,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

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
							surf.CreatePolygon(polygon, 
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
							surf.CreateFilledPolygon(polygon, 
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

						while (times < MAXCOUNT)
						{
							pie = new Pie((short)rand.Next(0, width), 
								(short)rand.Next(0, height), 
								(short)rand.Next(20, 100), 
								(short)rand.Next(0, 360), 
								(short)rand.Next(0, 360));

							surf.CreatePie(pie, 
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							pie = new Pie((short)rand.Next(0, width), 
								(short)rand.Next(0, height) , 
								(short)rand.Next(20, 100), 
								(short)rand.Next(0, 360), 
								(short)rand.Next(0, 360));

							surf.CreateFilledPie(pie, 
								Color.FromArgb(rand.Next(255), 
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 
						
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
							surf.CreateBezier(bezier, 
								Color.FromArgb(rand.Next(255), 
								rand.Next(255), 
								rand.Next(255),
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
						times = 0;
						surf.FillRectangle(new Rectangle(new Point(0, 0), surf.Size), Color.Black); 

						while (times < MAXCOUNT)
						{
							box = new Box(
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height),
								(short)rand.Next(0, width), 
								(short)rand.Next(0, height));
							surf.CreateBox(box,
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
							surf.CreateFilledBox(box,
								Color.FromArgb(
								rand.Next(255), 
								rand.Next(255), 
								rand.Next(255) ,
								rand.Next(255)));
							times++;
							screen.Flip();
							surf.Blit(screen, new Rectangle(new Point(0, 0), screen.Size));
							Thread.Sleep(SLEEPTIME);
						}

						Thread.Sleep(SLEEPTIME);
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
