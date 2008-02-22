#region LICENSE
/*
 * Copyright (C) 2004 - 2007 David Hudson (jendave@yahoo.com)
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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Collections.ObjectModel;

using SdlDotNet;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics.Primitives;

namespace SdlDotNetExamples
{
    /// <summary>
    /// 
    /// </summary>
    public class TextButtonSpriteEventArgs : SdlEventArgs
    {
        TextButtonSprite sprite;

        /// <summary>
        /// 
        /// </summary>
        public TextButtonSprite Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        string textItem;

        /// <summary>
        /// 
        /// </summary>
        public string TextItem
        {
            get { return textItem; }
            set { textItem = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="textItem"></param>
        public TextButtonSpriteEventArgs(TextButtonSprite sprite, string textItem)
        {
            this.sprite = sprite;
            this.textItem = textItem;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SdlDotNetExamplesBrowser
    {
        #region Fields
        SortedDictionary<string, SortedDictionary<string, string>> demoList;
        private Surface screen; //video screen
        private int width = 740; //screen width
        private int height = 550; //screen height
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");
        string fontName = "FreeSans.ttf";
        Line line;
        SpriteCollection listBoxDemos = new SpriteCollection();
        static ResourceManager stringManager;
        string currentNamespace = "LargeDemos";
        SpriteCollection comboBoxNamespaces;

        #endregion Fields

        /// <summary>
        /// 
        /// </summary>
        public static ResourceManager StringManager
        {
            get { return SdlDotNetExamplesBrowser.stringManager; }
            set { SdlDotNetExamplesBrowser.stringManager = value; }
        }

        #region EventHandler Methods
        //Handles keyboard events. The 'Escape' and 'Q'keys will cause the app to exit
        private void KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        //A ticker is running to update the sprites constantly.
        //This method will fill the screen with black to clear it of the sprites.
        //Then it will Blit all of the sprites to the screen.
        //Then it will refresh the screen and display it.
        private void Tick(object sender, TickEventArgs args)
        {
            screen.Fill(Color.Black);
            screen.Draw(line, Color.White);
            screen.Blit(this.comboBoxNamespaces);
            screen.Blit(this.listBoxDemos);
            screen.Update();
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
        #endregion EventHandler Methods

        #region Methods
        //Main program loop
        private void Go()
        {
            //Set up screen
            if (File.Exists(Path.Combine(dataDirectory, "FreeSans.ttf")))
            {
                filePath = "";
            }
            line = new Line(new Point(0, 20), new Point(width, 20));
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Demo Browser";
            screen = Video.SetVideoMode(width, height);
            
            demoList = new SortedDictionary<string, SortedDictionary<string, string>>();
            stringManager =
                new ResourceManager("SdlDotNetExamples.Properties.Resources", Assembly.GetExecutingAssembly());
            Load();
            EnableSpriteEvents();
            //These bind the events to the above methods.
            EnableEvents();
            //Start the event ticker
            Events.Run();
        }

        private void EnableEvents()
        {
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
        }

        private void DisableEvents()
        {
            Events.KeyboardDown -=
                new EventHandler<KeyboardEventArgs>(this.KeyboardDown);
            Events.Tick -= new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit -= new EventHandler<QuitEventArgs>(this.Quit);
        }

        private void EnableSpriteEvents()
        {
            //The collection will respond to mouse button clicks, mouse movement and the ticker.
            this.comboBoxNamespaces.EnableMouseButtonEvent();
            this.comboBoxNamespaces.EnableMouseMotionEvent();
            this.listBoxDemos.EnableMouseButtonEvent();
            this.listBoxDemos.EnableMouseMotionEvent();
        }

        private void DisableSpriteEvents()
        {
            //The collection will respond to mouse button clicks, mouse movement and the ticker.
            this.comboBoxNamespaces.DisableMouseButtonEvent();
            this.comboBoxNamespaces.DisableMouseMotionEvent();
            this.listBoxDemos.DisableMouseButtonEvent();
            this.listBoxDemos.DisableMouseMotionEvent();
        }

        private void Load()
        {
            LoadDemos();
            LoadComboBox();
            LoadListBox();
        }

        /// <summary>
        /// Entry point for App.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            SdlDotNetExamplesBrowser demos = new SdlDotNetExamplesBrowser();
            demos.Go();
        }

        private void LoadDemos()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                MemberInfo[] runMethods = type.GetMember("Run");

                if (runMethods.Length > 0 && type != typeof(SdlDotNetExamplesBrowser))
                {
                    string result = (string)type.InvokeMember("Title",
                             BindingFlags.GetProperty, null, type, null, CultureInfo.CurrentCulture);
                    if (!this.demoList.ContainsKey(type.Namespace.Substring(type.Namespace.IndexOf('.') + 1)))
                    {
                        SortedDictionary<string, string> list = new SortedDictionary<string, string>();
                        list.Add(result, type.Name);
                        this.demoList.Add(type.Namespace.Substring(type.Namespace.IndexOf('.') + 1), list);
                    }
                    else
                    {
                        this.demoList[type.Namespace.Substring(type.Namespace.IndexOf('.') + 1)].Add(result, type.Name);
                    }
                }
            }
        }

        private void RunExample(string textItem)
        {
            try
            {
                string typeString = "SdlDotNetExamples." + this.currentNamespace + "." + this.demoList[this.currentNamespace][textItem].ToString();
                Type example = Assembly.GetExecutingAssembly().GetType(typeString, true, true);
                DisableSpriteEvents();
                DisableEvents();
                SdlDotNet.Core.Events.QuitApplication();
                SdlDotNet.Core.Events.CloseVideo();
                example.InvokeMember("Run", BindingFlags.InvokeMethod, null, null, null, CultureInfo.CurrentCulture);
            }
            catch (TypeLoadException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (System.MissingMethodException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                SdlDotNet.Core.Events.QuitApplication();
            }
        }

        private void LoadListBox()
        {
            LoadListBox("LargeDemos");
        }
        private void LoadListBox(string comboBoxNamespace)
        {
            this.listBoxDemos.Clear();

            int positionX = 23;
            int positionY = 0;
            foreach (string s in this.demoList[comboBoxNamespace].Keys)
            {
                TextButtonSprite sprite = new TextButtonSprite(s, new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, fontName)), 9));
                sprite.TextButtonSpriteSelected += new EventHandler<TextButtonSpriteEventArgs>(sprite_TextButtonSpriteSelected);

                if (positionY == 34)
                {
                    positionX = 23;
                    sprite.X = 270;
                }
                if (positionY > 34)
                {
                    sprite.X = 270;
                }
                sprite.Y = positionX;
                this.listBoxDemos.Add(sprite);
                positionX = positionX + 15;
                positionY++;
            }
        }

        private void LoadComboBox()
        {
            int positionX = 0;
            comboBoxNamespaces = new SpriteCollection();
            foreach (string s in this.demoList.Keys)
            {
                TextButtonSprite sprite = new TextButtonSprite(s, new SdlDotNet.Graphics.Font(Path.Combine(filePath, Path.Combine(dataDirectory, fontName)), 11));
                sprite.X = positionX;
                sprite.TextButtonSpriteSelected += new EventHandler<TextButtonSpriteEventArgs>(sprite_TextButtonSpriteSelected);
                this.comboBoxNamespaces.Add(sprite);
                positionX = positionX + 100;
            }
        }

        void sprite_TextButtonSpriteSelected(object sender, TextButtonSpriteEventArgs e)
        {
            //Console.WriteLine(e.TextItem);
            if (this.comboBoxNamespaces.Contains(e.Sprite))
            {
                currentNamespace = e.TextItem;
                LoadListBox(e.TextItem);
            }
            else if (this.listBoxDemos.Contains(e.Sprite))
            {
                this.RunExample(e.TextItem);
            }
        }

        #endregion Methods

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~SdlDotNetExamplesBrowser()
        {
            Dispose(false);
        }
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

                }
                this.disposed = true;
            }
        }

        #endregion
    }
}

