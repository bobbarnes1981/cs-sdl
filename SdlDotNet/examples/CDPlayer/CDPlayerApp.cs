#region LICENSE
/*
 * $RCSfile: CDPlayer.cs,v $
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
 * Copyright (C) 2004,2005 David Hudson (jendave@yahoo.com)
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
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Globalization;

using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Windows;

namespace SdlDotNetExamples.CDPlayer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CDPlayerApp : System.Windows.Forms.Form
    {
        private CDDrive _drive;
        private int _track;

        string filePath = Path.Combine("..", "..");
        string dataDirectory = "Data";
        static ResourceManager stringManager;

        /// <summary>
        /// 
        /// </summary>
        public CDPlayerApp()
        {
            stringManager =
                new ResourceManager("SdlDotNetExamples.CDPlayer.Properties.Resources", Assembly.GetExecutingAssembly());
            InitializeComponent();
            this.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Title", CultureInfo.CurrentUICulture);
            this.label1.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                       "SelectDrive", CultureInfo.CurrentUICulture);
            this.buttonPlay.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Play", CultureInfo.CurrentUICulture);
            this.buttonPause.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Pause", CultureInfo.CurrentUICulture);
            this.buttonStop.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Stop", CultureInfo.CurrentUICulture);
            this.buttonEject.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Eject", CultureInfo.CurrentUICulture);
            this.labelStatus.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Track", CultureInfo.CurrentUICulture);
            this.buttonPrevious.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Previous", CultureInfo.CurrentUICulture);
            this.buttonNext.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Next", CultureInfo.CurrentUICulture);
            this.KeyPreview = true;
            surf =
                new Surface(
                this.surfaceControl.Width,
                this.surfaceControl.Height);
            if (File.Exists(Path.Combine(dataDirectory, "marble1.png")))
            {
                filePath = "";
            }
            SurfaceCollection marbleSurfaces = new SurfaceCollection();
            marbleSurfaces.Add(new Surface(Path.Combine(filePath, Path.Combine(dataDirectory, "marble1.png"))), new Size(50, 50));

            for (int i = 0; i < 1; i++)
            {
                //Create a new Sprite at a random location on the screen
                BounceSprite bounceSprite =
                    new BounceSprite(marbleSurfaces,
                    new Point(rand.Next(0, 350),
                    rand.Next(0, 200)));
                bounceSprite.Bounds = new Rectangle(new Point(0, 0), this.surfaceControl.Size);

                // Randomize rotation direction
                bounceSprite.AnimateForward = rand.Next(2) == 1 ? true : false;

                //Add the sprite to the SpriteCollection
                master.Add(bounceSprite);
            }

            //The collection will respond to mouse button clicks, mouse movement and the ticker.
            master.EnableMouseButtonEvent();
            master.EnableMouseMotionEvent();
            master.EnableVideoResizeEvent();
            master.EnableKeyboardEvent();
            master.EnableTickEvent();

            SdlDotNet.Core.Events.Fps = 30;
            SdlDotNet.Core.Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            SdlDotNet.Core.Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);

            try
            {
                int num = CDRom.NumberOfDrives;
                _drive = CDRom.OpenDrive(0);
                for (int i = 0; i < num; i++)
                {
                    comboBoxDrive.Items.Add(CDRom.DriveName(i));
                }

                if (comboBoxDrive.Items.Count > 0)
                {
                    comboBoxDrive.SelectedIndex = 0;
                }
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ResourceManager StringManager
        {
            get { return SdlDotNetExamples.CDPlayer.CDPlayerApp.stringManager; }
            set { SdlDotNetExamples.CDPlayer.CDPlayerApp.stringManager = value; }
        }

        private SpriteCollection master = new SpriteCollection();

        /// <summary>
        /// Entry point for App.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new CDPlayerApp());
            SdlDotNet.Core.Events.QuitApplication();
        }

        private static System.Random rand = new Random();
        private Surface surf;

        private void Tick(object sender, TickEventArgs e)
        {
            if (surf != null)
            {
                surf.Fill(Color.Black);
                surf.Blit(master);
                this.surfaceControl.Blit(surf);
            }
        }

        private void Quit(object sender, QuitEventArgs e)
        {
            SdlDotNet.Core.Events.QuitApplication();
        }

        private void comboBoxDrive_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonPlay_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    _drive.Play(_track, _drive.NumberOfTracks - _track);
                }
                TimeSpan timeSpan = SdlDotNet.Core.Timer.SecondsToTime(_drive.TrackLength(_drive.CurrentTrack));
                this.labelStatus.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Track", CultureInfo.CurrentUICulture) + ": " + _drive.CurrentTrack + "     " + SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Length", CultureInfo.CurrentUICulture) + ": " + timeSpan.Minutes + ":" + timeSpan.Seconds;
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonPause_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    _drive.Pause();
                }
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    _drive.Stop();
                    _track = 0;
                }
                this.labelStatus.Text = SdlDotNetExamples.CDPlayer.CDPlayerApp.StringManager.GetString(
                        "Track", CultureInfo.CurrentUICulture) + ": " + _drive.CurrentTrack;
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonEject_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    _drive.Eject();
                    _track = 0;
                }
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonPrev_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    if (_track != 0)
                    {
                        _track--;
                    }
                    buttonPlay_Click(null, null);
                }
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void buttonNext_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (_drive != null)
                {
                    if (_track != _drive.NumberOfTracks - 1)
                    {
                        _track++;
                    }
                    buttonPlay_Click(null, null);
                }
            }
            catch (SdlException ex)
            {
                Console.WriteLine(ex);
            }
        }

        //Thread thread;

        private void CDPlayer_Load(object sender, System.EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(SdlDotNet.Core.Events.Run));
            thread.IsBackground = true;
            thread.Name = "SDL.NET";
            thread.Priority = ThreadPriority.Normal;
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            try
            {
                surf =
                    new Surface(
                    this.surfaceControl.Width,
                    this.surfaceControl.Height);
                base.OnResize(e);
            }
            catch (AccessViolationException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.surfaceControl.KeyPressed(e);
            base.OnKeyDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.surfaceControl.KeyReleased(e);
            base.OnKeyUp(e);
        }

        private void CDPlayer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        //private void surfaceControl11_Load(object sender, EventArgs e)
        //{

        //}
    }
}
