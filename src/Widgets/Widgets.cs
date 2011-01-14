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

namespace SdlDotNet.Widgets
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using SdlDotNet.Graphics;

    public class Widgets
    {
        #region Fields

        static string defaultFontPath = "SdlDotNet.Widgets Resources\\tahoma.ttf";
        static int defaultFontSize = 12;
        static string resourceDirectory = "SdlDotNet.Widgets Resources\\Graphics";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the path to the default font.
        /// </summary>
        /// <value>The default font path.</value>
        public static string DefaultFontPath {
            get { return defaultFontPath; }
            set { defaultFontPath = value; }
        }

        /// <summary>
        /// Gets or sets the default size of the font.
        /// </summary>
        /// <value>The default size of the font.</value>
        public static int DefaultFontSize {
            get { return defaultFontSize; }
            set { defaultFontSize = value; }
        }

        /// <summary>
        /// Gets or sets the resource directory.
        /// </summary>
        /// <value>The resource directory.</value>
        public static string ResourceDirectory {
            get { return resourceDirectory; }
            set {
                if (value.EndsWith("\\")) {
                    value = value.Remove(value.Length - 1, 1);
                }
                resourceDirectory = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes the widget subsystem.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="resourceDirectory">The resource directory.</param>
        /// <param name="defaultFontPath">The default font path.</param>
        /// <param name="defaultFontPointSize">Default size of the font.</param>
        public static void Initialize(SdlDotNet.Graphics.Surface destinationSurface,
            string resourceDirectory,
            string defaultFontPath,
            int defaultFontPointSize) {
            Initialize(destinationSurface, resourceDirectory, defaultFontPath, defaultFontPointSize, false);
        }

        /// <summary>
        /// Initializes the widget subsystem.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        /// <param name="resourceDirectory">The resource directory.</param>
        /// <param name="defaultFontPath">The default font path.</param>
        /// <param name="defaultFontPointSize">Default size of the font.</param>
        /// <param name="subscribeToInputEvents">if set to <c>true</c> automatically subscribe to input events.</param>
        public static void Initialize(Surface destinationSurface, string resourceDirectory,
            string defaultFontPath,
            int defaultFontPointSize,
            bool subscribeToInputEvents) {
            Widgets.resourceDirectory = resourceDirectory;
            Widgets.defaultFontPath = defaultFontPath;
            defaultFontSize = defaultFontPointSize;
            WindowManager.Initialize(destinationSurface);

            if (subscribeToInputEvents) {
                SdlDotNet.Core.Events.KeyboardDown += new EventHandler<Input.KeyboardEventArgs>(Events_KeyboardDown);
                SdlDotNet.Core.Events.KeyboardUp += new EventHandler<Input.KeyboardEventArgs>(Events_KeyboardUp);

                SdlDotNet.Core.Events.MouseButtonDown += new EventHandler<Input.MouseButtonEventArgs>(Events_MouseButtonDown);
                SdlDotNet.Core.Events.MouseButtonUp += new EventHandler<Input.MouseButtonEventArgs>(Events_MouseButtonUp);
                SdlDotNet.Core.Events.MouseMotion += new EventHandler<Input.MouseMotionEventArgs>(Events_MouseMotion);
            }
        }

        public static void CloseWidgets() {
            if (WindowManager.Initialized) {
                // First, close all screen widgets
                Screen.ClearWidgets();
                // Close all open windows
                for (int i = 0; i < WindowManager.Windows.Count; i++) {
                    Window windowToClose = WindowManager.Windows[i];
                    WindowManager.Windows.RemoveWindow(windowToClose);
                    windowToClose.FreeResources();
                }
                WindowManager.ToggleWindowSwitcher(false);
                WindowManager.windowSwitcher.FreeResources();
            }
        }

        static void Events_KeyboardDown(object sender, Input.KeyboardEventArgs e) {
            WindowManager.HandleKeyboardDown(e);
        }

        static void Events_KeyboardUp(object sender, Input.KeyboardEventArgs e) {
            WindowManager.HandleKeyboardUp(e);
        }

        static void Events_MouseButtonDown(object sender, Input.MouseButtonEventArgs e) {
            WindowManager.HandleMouseButtonDown(e);
        }

        static void Events_MouseButtonUp(object sender, Input.MouseButtonEventArgs e) {
            WindowManager.HandleMouseButtonUp(e);
        }

        /// <summary>
        /// Handles the MouseMotion event of the Events control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SdlDotNet.Input.MouseMotionEventArgs"/> instance containing the event data.</param>
        static void Events_MouseMotion(object sender, Input.MouseMotionEventArgs e) {
            WindowManager.HandleMouseMotion(e);
        }

        public static IEnumerable<Widget> EnumerateActiveWidgets() {
            if (WindowManager.Initialized) {
                for (int i = 0; i < Screen.Widgets.Count; i++) {
                    yield return Screen.Widgets[i];
                    if (Screen.Widgets[i] is IContainer) {
                        foreach (Widget childWidget in ((IContainer)Screen.Widgets[i]).ChildWidgets.EnumerateWidgets()) {
                            yield return childWidget;
                        }
                    }
                }
                for (int i = 0; i < WindowManager.Windows.Count; i++) {
                    yield return WindowManager.Windows[i];
                    foreach (Widget childWidget in WindowManager.Windows[i].ChildWidgets.EnumerateWidgets()) {
                        yield return childWidget;
                    }
                }
            }
        }


        #endregion Methods
    }
}