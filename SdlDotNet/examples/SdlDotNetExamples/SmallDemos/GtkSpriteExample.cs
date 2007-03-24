#region LICENSE
/*
 * Copyright (C) 2007 by Thomas Krieger
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
 *
 * Exclusive rights to for use to SDL.NET & GTK# projects !!!
 * Others may contact me on SDL.NET forums under nickname Shoky
 */
#endregion LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

using Gtk;
using Glade;
using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;
using SdlDotNet.GtkSharp;

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// Description of MainWindow.
    /// </summary>
    public class GtkSpriteExample : IDisposable
    {
        /// <summary>
        /// Ansichtselement des Graphen
        /// </summary>
        [CLSCompliant(false)]
        protected SurfaceGtk graphView;

        /// <summary>
        /// Graphenansicht
        /// </summary>
        [CLSCompliant(false)]
        [Glade.Widget]
        protected Gtk.Window GraphWindow;

        /// <summary>
        /// Container für die Graphenansicht
        /// </summary>
        [CLSCompliant(false)]
        [Glade.Widget]
        protected Gtk.VBox vbox1;

        /// <summary>
        /// Collection of Sprites
        /// </summary>
        protected SpriteDictionary spriteDictionary;

        /// <summary>
        /// Timer
        /// </summary>
        protected System.Timers.Timer tickTimer;
        string filePath;
        string fileDirectory;

        /// <summary>
        /// Defaultkonstruktor
        /// </summary>
        public GtkSpriteExample()
        {
            filePath = Path.Combine("..", "..");
            fileDirectory = "Data";
            string fileName = "circle.png";
            if (File.Exists(fileName))
            {
                filePath = "";
                fileDirectory = "";
            }
            else if (File.Exists(Path.Combine(fileDirectory, fileName)))
            {
                filePath = "";
            }

            Glade.XML gxml = new Glade.XML(null, "SdlDotNetExamples.SmallDemos.ui.glade", "GraphWindow", null);
            gxml.Autoconnect(this);

            // init graph viewer
            initGraphView();

            tickTimer = new System.Timers.Timer(200);
            tickTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnRedrawTick);
            tickTimer.Enabled = true;

            GraphWindow.ShowAll();
        }

        /// <summary>
        /// Timer Handler für die Aktualisierung der Ansicht
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void OnRedrawTick(object sender, System.Timers.ElapsedEventArgs args)
        {
            Collection<Rectangle> rects = new Collection<Rectangle>();

            graphView.Surface.Fill(new Rectangle(0, 0, 800, 600), System.Drawing.Color.AliceBlue);
            rects = graphView.Surface.Blit(spriteDictionary);
            graphView.Surface.Update(rects);
            graphView.QueueDraw();
        }

        /// <summary>
        /// inits the view for the first time with an background image
        /// </summary>
        protected void initGraphView()
        {
            GtkSprite node = new GtkSprite(Path.Combine(filePath, Path.Combine(fileDirectory, "circle.png")));
            GtkSprite node2 = new GtkSprite(Path.Combine(filePath, Path.Combine(fileDirectory, "circle.png")));

            node2.X = 100;
            node2.Y = 100;

            spriteDictionary = new SpriteDictionary();

            spriteDictionary.EnableMouseButtonEvent();
            spriteDictionary.EnableMouseMotionEvent();

            spriteDictionary.Add(node);
            spriteDictionary.Add(node2);

            graphView = new SurfaceGtk();
            graphView.Surface = new Surface(Path.Combine(filePath, Path.Combine(fileDirectory, "background.jpg")));
            graphView.Surface.Blit(spriteDictionary);
            graphView.Surface.Update();

            vbox1.PackEnd(graphView);
        }

        /// <summary>
        /// Quit Handler für das Schließen des Hauptfensters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [CLSCompliant(false)]
        protected void OnDeleteWindow(object sender, Gtk.DeleteEventArgs args)
        {
            Gtk.Application.Quit();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Go()
        {
            Gtk.Application.Run();
        }


        /// <summary>
        /// Haupteinstiegspunkt für das Programm
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Run()
        {
            Gtk.Application.Init();
            GtkSpriteExample gtkSpriteExample = new GtkSpriteExample();
            gtkSpriteExample.Go();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "GtkSpriteExample: GTK Widgets and Sprites using Glade";
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.graphView != null)
                    {
                        this.graphView.Dispose();
                        this.graphView = null;
                    }
                    if (this.tickTimer != null)
                    {
                        this.tickTimer.Dispose();
                        this.tickTimer = null;
                    }
                }
                this.disposed = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        ~GtkSpriteExample()
        {
            Dispose(false);
        }

        #endregion
    }
}
