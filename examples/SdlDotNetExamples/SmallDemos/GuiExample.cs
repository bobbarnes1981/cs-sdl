#region Header

/*
 * Copyright (C) 2010 Pikablu
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

#endregion Header

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Widgets;

namespace SdlDotNetExamples.SmallDemos
{
    public class GuiExample : IDisposable
    {
        Surface screen;
        string dataDirectory = "Data";
        string filePath = Path.Combine("..", "..");
        string fontName = "FreeSans.ttf";

        Label resultsLabel;
        ListBox messageBoxButtonsSelectionListBox;

        [STAThread]
        public static void Run() {
            GuiExample guiExample = new GuiExample();
            guiExample.Go();
        }

        public void Go() {
            if (File.Exists(Path.Combine(dataDirectory, fontName))) {

                filePath = "";
            }
            Video.WindowIcon();
            Video.WindowCaption = "SDL.NET - Gui Example";
            Video.UseResolutionScaling = true;

            Resolution.SetStandardResolution(640, 480);
            Resolution.SetResolution(1024, 768);
            screen = Video.SetVideoMode(Resolution.ResolutionWidth, Resolution.ResolutionHeight, 16, true);

            Widgets.Initialize(screen, Path.Combine(filePath, Path.Combine(dataDirectory, "Widgets")),
                Path.Combine(filePath, Path.Combine(dataDirectory, fontName)), 12, true);

            CreateWindow();

            Events.Tick += new EventHandler<TickEventArgs>(this.Tick);
            Events.Quit += new EventHandler<QuitEventArgs>(this.Quit);
            Events.VideoResize += new EventHandler<VideoResizeEventArgs>(Events_VideoResize);
            Events.Run();
        }

        private void CreateWindow() {
            Window testWindow = new Window("testWindow");
            testWindow.Location = new Point(50, 50);
            testWindow.Size = new Size(300, 250);
            testWindow.Text = "Gui Example: Test Window";

            Button testButton = new Button("testButton");
            testButton.Location = new Point(5, 10);
            testButton.Size = new Size(120, 30);
            testButton.Text = "MessageBox Test";
            testButton.Click += new EventHandler<MouseButtonEventArgs>(testButton_Click);

            resultsLabel = new Label("resultsLabel");
            resultsLabel.Size = new Size(120, 100);
            resultsLabel.Location = new Point(5, 50);

            messageBoxButtonsSelectionListBox = new ListBox("messageBoxButtonsSelectionListBox");
            messageBoxButtonsSelectionListBox.Location = new Point(130, 10);
            messageBoxButtonsSelectionListBox.Size = new Size(150, 100);
            string[] buttonOptions = Enum.GetNames(typeof(MessageBoxButtons));
            for (int i = 0; i < buttonOptions.Length; i++) {
                messageBoxButtonsSelectionListBox.Items.Add(buttonOptions[i]);
            }
            messageBoxButtonsSelectionListBox.SelectItem("YesNoCancel");

            testWindow.AddWidget(testButton);
            testWindow.AddWidget(resultsLabel);
            testWindow.AddWidget(messageBoxButtonsSelectionListBox);

            testWindow.Show();
        }

        void testButton_Click(object sender, MouseButtonEventArgs e) {
            //resultsLabel.Text = "Status:\nWaiting for input...";
            //MessageBoxButtons buttons = (MessageBoxButtons)Enum.Parse(typeof(MessageBoxButtons), messageBoxButtonsSelectionListBox.SelectedItem.TextIdentifier, true);
            //DialogResult result = MessageBox.Show("Pick a button! Any button!", "Button Selection", buttons);
            //resultsLabel.Text = "Status:\n\"" + result.ToString() + "\" selected!";

            FileBrowserDialog fbd = new FileBrowserDialog("fileBrowserDialog");
            fbd.Filter = "All Files|*.*|Text Files|*.txt;*.dll;*.exe";
            fbd.ShowDialog();
        }

        void Tick(object sender, TickEventArgs e) {
            screen.Fill(Color.Black);

            WindowManager.DrawWindows(e);

            screen.Update();
        }

        private void Quit(object sender, QuitEventArgs e) {
            Events.QuitApplication();
        }

        void Events_VideoResize(object sender, VideoResizeEventArgs e) {
            Resolution.SetResolution(e.Width, e.Height);
            screen = Video.SetVideoMode(Resolution.ResolutionWidth, Resolution.ResolutionHeight, 16, true);
        }

        /// <summary>
        /// Lesson Title
        /// </summary>
        public static string Title {
            get {
                return "Gui Example: Creating a Gui";
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        public void Close() {
            Dispose();
        }

        /// <summary>
        /// Destroy object
        /// </summary>
        ~GuiExample() {
            Dispose(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    // Dispose everything here

                }
                this.disposed = true;
            }
        }
        #endregion
    }
}
