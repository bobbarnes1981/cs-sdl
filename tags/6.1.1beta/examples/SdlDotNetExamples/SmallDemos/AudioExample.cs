#region LICENSE
/*
 * $RCSfile: AudioExample.cs,v $
 * Copyright (C) 2005 Rob Loach (http://www.robloach.net)
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
using System.Resources;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Globalization;

using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;
using SdlDotNet.Input;

// SDL.NET Audio Example
// Simple example to demonstrate audio in SDL.NET.
// Click plays the sound, space changes the music, arrows change volume.

namespace SdlDotNetExamples.SmallDemos
{
    /// <summary>
    /// A simple SDL.NET example which demonstrates audio in SDL.NET.
    /// Click plays sound, space changes music and the arrow keys change volume.
    /// </summary>
    public class AudioExample : IDisposable
    {
        private const int width = 400;
        private const int height = 100;
        private Surface screen;
        string fileDirectory = "Data";
        string filePath = Path.Combine("..", "..");
        string fileName = "boing.wav";
        private TextSprite textDisplay;

        // Create the music and sound variables.
        private MusicDictionary music = new MusicDictionary();
        private Sound boing; // There is also a SoundDictionary class.

        /// <summary>
        /// 
        /// </summary>
        public AudioExample()
        {
            // Setup events
            Events.Tick +=
                new EventHandler<TickEventArgs>(Events_TickEvent);
            Events.KeyboardDown +=
                new EventHandler<KeyboardEventArgs>(Events_KeyboardDown);
            Events.KeyboardUp +=
                new EventHandler<KeyboardEventArgs>(Events_KeyboardUp);
            Events.MouseButtonDown +=
                new EventHandler<MouseButtonEventArgs>(Events_MouseButtonDown);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.MusicFinished +=
                new EventHandler<MusicFinishedEventArgs>(Events_MusicFinished);

            if (File.Exists(fileName))
            {
                filePath = "";
                fileDirectory = "";
            }
            else if (File.Exists(Path.Combine(fileDirectory, fileName)))
            {
                filePath = "";
            }

            Keyboard.UnicodeEnabled = true;
        }

        public void Go()
        {
            // Load the music and sounds.
            music["mason2"] = new Music(Path.Combine(Path.Combine(filePath, fileDirectory), "mason2.mid"));
            music["fard-two"] = new Music(Path.Combine(Path.Combine(filePath, fileDirectory), "fard-two.ogg"));
            boing = new Sound(Path.Combine(Path.Combine(filePath, fileDirectory), "boing.wav"));

            textDisplay = new TextSprite(" ", new SdlDotNet.Graphics.Font(Path.Combine(Path.Combine(filePath, fileDirectory), "FreeSans.ttf"), 20), Color.Red);

            // Start up SDL
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - AudioExample";
            screen = Video.SetVideoMode(width, height);

            // Play the music and setup the queues.
            music["mason2"].Play();
            //music["fard-two"].Play();

            // Set up the music queue and start it
            music["mason2"].QueuedMusic = music["fard-two"];
            music["fard-two"].QueuedMusic = music["mason2"];
            MusicPlayer.EnableMusicFinishedCallback();

            // Begin the SDL ticker
            Events.Fps = 50;

            textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "AudioExampleDirections", CultureInfo.CurrentUICulture);
            textDisplay.TextWidth = 200;
            Events.Run();
        }

        private void Events_TickEvent(object sender, TickEventArgs e)
        {
            screen.Fill(Color.Black);
            screen.Blit(textDisplay);
            screen.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        [STAThread]
        public static void Run()
        {
            AudioExample t = new AudioExample();
            t.Go();
        }

        private void Events_KeyboardUp(object sender, KeyboardEventArgs e)
        {
            //Console.WriteLine("Keyboard Up: " + e.KeyboardCharacter);
            //Console.WriteLine("Unicode Up: " + e.UnicodeCharacter);
        }

        private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            //Console.WriteLine("Keyboard Down: " + e.KeyboardCharacter);
            //Console.WriteLine("Unicode Down: " + e.UnicodeCharacter);

            switch (e.Key)
            {
                case Key.Space:
                    try
                    {
                        // Switch the music 
                        MusicPlayer.Fadeout(1500);
                        textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "MusicIsFading", CultureInfo.CurrentUICulture);

                        // The next music sample plays because queuing is enabled.
                    }
                    catch (SdlException exception)
                    {
                        exception.ToString();
                        textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "MusicIsFading", CultureInfo.CurrentUICulture);
                    }
                    break;

                case Key.UpArrow:
                    // Increase the music volume.
                    MusicPlayer.Volume += 10;
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "MusicVolume", CultureInfo.CurrentUICulture) + ": " + MusicPlayer.Volume;
                    break;
                case Key.DownArrow:
                    // Decrease the music volume.
                    MusicPlayer.Volume -= 10;
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "MusicVolume", CultureInfo.CurrentUICulture) + ": " + MusicPlayer.Volume;
                    break;
                case Key.RightArrow:
                    // Play the sound on the right
                    boing.Play().SetPanning(50, 205);
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "SoundRight", CultureInfo.CurrentUICulture);
                    break;
                case Key.LeftArrow:
                    // Play the sound on the left
                    boing.Play().SetPanning(205, 50);
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "SoundLeft", CultureInfo.CurrentUICulture);
                    break;
                case Key.Q:
                case Key.Escape:
                    // Quit the example
                    Events.QuitApplication();
                    break;
            }
        }

        private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.PrimaryButton:
                    // Play on left side
                    boing.Play().SetPanning(205, 50);
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "SoundLeft", CultureInfo.CurrentUICulture);
                    break;
                case MouseButton.SecondaryButton:
                    // Play on right side 
                    boing.Play().SetPanning(50, 205);
                    textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "SoundRight", CultureInfo.CurrentUICulture);
                    break;
            }
        }

        private void Events_MusicFinished(object sender, MusicFinishedEventArgs e)
        {
            // Switch the music....
            textDisplay.Text = SdlDotNetExamplesBrowser.StringManager.GetString(
                        "MusicSwitched", CultureInfo.CurrentUICulture);
            //Console.WriteLine("Music Finished");
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title
        {
            get
            {
                return "AudioExample: Plays music and sounds";
            }
        }

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
        ~AudioExample()
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
                    if (this.textDisplay != null)
                    {
                        this.textDisplay.Dispose();
                        this.textDisplay = null;
                    }
                    if (this.boing != null)
                    {
                        this.boing.Dispose();
                        this.boing = null;
                    }
                }
                this.disposed = true;
            }
        }

        #endregion
    }
}
