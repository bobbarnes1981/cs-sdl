#region LICENSE
/*
 * Copyright (C) 2004 - 2006 David Hudson (jendave@yahoo.com)
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
#endregion LICENSE

using System;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.IO;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;
using SdlDotNet.Core;

// Simple SDL.NET Example
// Just draws a bunch of primitives to the screen, to quit hit 'Q' or Esc.

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// 
    /// </summary>
    public class PrimitivesExample
    {
        Circle circle;
        Ellipse ellipse;
        Line line;
        Triangle triangle;
        Polygon polygon;
        TexturedPolygon texturedPolygon;
        Pie pie;
        Bezier bezier;
        Box box;
        const int MAXCOUNT = 3;
        const int SLEEPTIME = 200;
        int times;
        static Random rand = new Random();
        int width = 640;
        int height = 480;
        Surface surf;
        Surface screen;
        string filePath = Path.Combine("..", "..");
        string fileDirectory = "Data";
        string fileName = "gameOver.bmp";
        string file;

        /// <summary>
        /// 
        /// </summary>
        public PrimitivesExample()
        {
            if (File.Exists(fileName))
            {
                filePath = "";
                fileDirectory = "";
            }
            else if (File.Exists(Path.Combine(fileDirectory, fileName)))
            {
                filePath = "";
            }

            file = Path.Combine(Path.Combine(filePath, fileDirectory), fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Primitives Example";
            Mouse.ShowCursor = false;
            screen = Video.SetVideoMode(width, height, true);
            surf =
                screen.CreateCompatibleSurface(width, height, true);
            //fill the surface with black
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);
            Events.Run();
        }

        private void KeyboardDown(
            object sender,
            KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape ||
                e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void Tick(object sender, TickEventArgs e)
        {
            while (times < MAXCOUNT)
            {
                circle = new Circle(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100));
                surf.Draw(circle,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                circle = new Circle(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100));
                surf.Draw(circle,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            while (times < MAXCOUNT)
            {
                ellipse = new Ellipse(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100),
                    (short)rand.Next(20, 100));
                surf.Draw(ellipse,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                ellipse = new Ellipse(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100),
                    (short)rand.Next(20, 100));
                surf.Draw(ellipse,
                    Color.FromArgb(rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            while (times < MAXCOUNT)
            {
                line = new Line(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height));
                surf.Draw(line,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            while (times < MAXCOUNT)
            {
                triangle = new Triangle(
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2),
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2),
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2));
                surf.Draw(triangle,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)
                    , rand.Next(255)));
                triangle = new Triangle(
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2),
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2),
                    (short)rand.Next(0, width / 2),
                    (short)rand.Next(0, height / 2));
                surf.Draw(triangle,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);


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
                surf.Draw(polygon,
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
                surf.Draw(polygon,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            try
            {
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
                    texturedPolygon = new TexturedPolygon(new Surface(file), x, y, 10, 10);
                    surf.Draw(texturedPolygon,
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
                    //short[] a = {
                    //                            0, 200, 200, 0
                    //                        };
                    //short[] b = {
                    //                            0, 0, 200, 200

                    //                        };
                    texturedPolygon = new TexturedPolygon(new Surface(file), a, b, 10, 20);
                    surf.Draw(texturedPolygon,
                        Color.FromArgb(
                        rand.Next(255),
                        rand.Next(255),
                        rand.Next(255),
                        rand.Next(255)), false, true);
                    times++;
                    screen.Update();
                    screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                    Thread.Sleep(SLEEPTIME);
                }
            }
            catch (EntryPointNotFoundException ex)
            {
                Console.WriteLine("Using old version of SDL_gfx. Please upgrade to >=2.0.16");
                Console.WriteLine(ex);
            }


            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            while (times < MAXCOUNT)
            {
                pie = new Pie((short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100),
                    (short)rand.Next(0, 360),
                    (short)rand.Next(0, 360));

                surf.Draw(pie,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                pie = new Pie((short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(20, 100),
                    (short)rand.Next(0, 360),
                    (short)rand.Next(0, 360));

                surf.Draw(pie,
                    Color.FromArgb(rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                times++;
                screen.Update();
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
                surf.Draw(bezier,
                    Color.FromArgb(rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }

            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);


            while (times < MAXCOUNT)
            {
                box = new Box(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height));
                surf.Draw(box,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)));
                box = new Box(
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height),
                    (short)rand.Next(0, width),
                    (short)rand.Next(0, height));
                surf.Draw(box,
                    Color.FromArgb(
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255),
                    rand.Next(255)), false, true);
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
                Thread.Sleep(SLEEPTIME);
            }
            Thread.Sleep(SLEEPTIME);
            times = 0;
            surf.Fill(new Rectangle(new Point(0, 0), surf.Size), Color.Black);

            int xpixel;
            int ypixel;
            int rpixel;
            int gpixel;
            int bpixel;

            while (times < 100)
            {
                xpixel = rand.Next(10, width);
                ypixel = rand.Next(10, height);
                rpixel = rand.Next(255);
                gpixel = rand.Next(255);
                bpixel = rand.Next(255);

                surf.GetColorValue(Color.FromArgb(rpixel, gpixel, bpixel));
                //colorValue = screen.MapColor(Color.FromArgb(254, 0, 0));
                //screen.DrawPixel(x, y, Color.Red);
                //Console.WriteLine("colorValue: " + colorValue.ToString(CultureInfo.CurrentCulture));
                surf.Draw(new Point(xpixel, ypixel), Color.FromArgb(rpixel, gpixel, bpixel));
                //screen.DrawPixel(x, y, Color.Red);
                //Console.WriteLine("GetPixel: " + screen.GetPixel(xpixel, ypixel).ToString());
                //Console.WriteLine("GetPixel: " + screen.GetColorValue(screen.GetPixel(xpixel, ypixel)).ToString(CultureInfo.CurrentCulture));
                times++;
                screen.Update();
                screen.Blit(surf, new Rectangle(new Point(0, 0), screen.Size));
            }
        }

        [STAThread]
        public static void Run()
        {
            PrimitivesExample primitivesExample = new PrimitivesExample();
            primitivesExample.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "PrimitiveExample: Displays primitives";
            }
        }
    }
}
