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
    using System.Drawing;
    using System.Text;

    public static class WindowManager
    {
        #region Fields

        internal static SdlDotNet.Graphics.Surface destSurf;
        internal static Widget heldScroller;

        static Window activeWindow;
        static Window currentModalWindow;
        static SdlDotNet.Graphics.Font defaultWindowFont;
        static bool initialized;
        static List<InvocationItem> invocationList;
        static int mainThreadID = -1;
        static WidgetOverlayCollection overlayCollection;
        static WindowCollection windows;
        internal static WindowSwitcher windowSwitcher;
        static bool windowSwitcherEnabled;

        #endregion Fields

        #region Events

        /// <summary>
        /// Fires when a window is added.
        /// </summary>
        public static event EventHandler<WindowAddedEventArgs> WindowAdded;

        /// <summary>
        /// Fires when a window is removed.
        /// </summary>
        public static event EventHandler<WindowRemovedEventArgs> WindowRemoved;

        #endregion Events

        #region Properties

        public static Window CurrentModalWindow {
            get { return currentModalWindow; }
            internal set {
                if (value != null) {
                    BringWindowToFront(value);
                }
                currentModalWindow = value;
                activeWindow = value;
            }
        }

        public static SdlDotNet.Graphics.Font DefaultWindowFont {
            get { return defaultWindowFont; }
            set { defaultWindowFont = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="WindowManager"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public static bool Initialized {
            get { return initialized; }
        }

        /// <summary>
        /// Gets a value indicating whether an invoke to the main thread is required.
        /// </summary>
        /// <value><c>true</c> if an invoke to the main thread is required; otherwise, <c>false</c>.</value>
        public static bool InvokeRequired {
            get {
                return (mainThreadID != System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        /// <summary>
        /// Gets the size of the screen.
        /// </summary>
        /// <value>The size of the screen.</value>
        public static Size ScreenSize {
            get { return destSurf.Size; }
        }

        public static WindowCollection Windows {
            get { return windows; }
        }

        /// <summary>
        /// Gets a value indicating whether the window switcher is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the window switcher is enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool WindowSwitcherEnabled {
            get { return windowSwitcherEnabled; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a window.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void AddWindow(Window window) {
            if (string.IsNullOrEmpty(window.Name)) {
                window.Name = "window" + windows.Count.ToString();
            }
            AddWindow(window.Name, window);
        }

        /// <summary>
        /// Adds a window and assigns it the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="window">The window.</param>
        public static void AddWindow(string name, Window window) {
            if (initialized) {
                if (window.Name != name) {
                    window.Name = name;
                }
                window.InvokeLoad();
                windows.AddWindow(name, window);
                for (int w = 0; w < windows.Count; w++) {
                    if (windows[w].AlwaysOnTop) {
                        BringWindowToFront(windows[w]);
                    }
                }
                if (windowSwitcherEnabled && window.ShowInWindowSwitcher) {
                    windowSwitcher.AddButton(new WindowSwitcherButton(window));
                }
                window.InvokeShown();
                for (int i = 0; i < windows.Count; i++) {
                    windows[i].TopMost = false;
                }
                window.TopMost = true;
                if (WindowAdded != null)
                    WindowAdded(null, new WindowAddedEventArgs(window));

            }
        }

        /// <summary>
        /// Brings the window to the front.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void BringWindowToFront(Window window) {
            if (initialized) {
                if (windows.Contains(window)) {
                    int oldSlot = windows.IndexOf(window);
                    int newSlot = windows.Count - 1;
                    for (int i = windows.Count - 1; i >= 0; i--) {
                        if (windows[i].AlwaysOnTop == false) {
                            newSlot = i;
                            break;
                        } else if (window.AlwaysOnTop) {
                            if (windows[i].AlwaysOnTop) {
                                newSlot = i;
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < windows.Count; i++) {
                        windows[i].TopMost = false;
                    }
                    if (oldSlot != newSlot) {
                        windows[oldSlot].TopMost = true;
                        windows.SwitchWindows(oldSlot, newSlot);
                    } else {
                        windows[newSlot].TopMost = true;
                    }
                }
            }
        }

        /// <summary>
        /// Brings the window to the front.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void BringWindowToFront(string name) {
            if (initialized) {
                if (windows.Contains(name)) {
                    BringWindowToFront(FindWindow(name));
                }
            }
        }

        /// <summary>
        /// Draws all windows to the screen.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Core.TickEventArgs"/> instance containing the event data.</param>
        public static void DrawWindows(SdlDotNet.Core.TickEventArgs e) {
            if (initialized) {
                Screen.DrawWidgets(e);
                for (int i = 0; i < windows.Count; i++) {
                    if (IsWindowActive(windows[i])) {
                        windows[i].OnTick(e);
                        windows[i].BlitToScreen(destSurf);
                    }
                }
                if (windowSwitcherEnabled) {
                    windowSwitcher.OnTick(e);
                    windowSwitcher.BlitToScreen(destSurf);
                }
                if (overlayCollection.Items.Count > 0) {
                    overlayCollection.DrawWidgets(e);
                }
                for (int i = 0; i < invocationList.Count; i++) {
                    invocationList[i].Delegate.Method.Invoke(invocationList[i].Delegate.Target, invocationList[i].Parameters);
                    invocationList[i].ResetEvent.Set();
                }
                if (invocationList.Count > 0) {
                    invocationList.Clear();
                }
            }
        }

        /// <summary>
        /// Finds the window.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Window FindWindow(string name) {
            if (initialized && windows.Contains(name)) {
                return windows[name];
            } else {
                return null;
            }
        }

        /// <summary>
        /// Handles the keyboard down event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.KeyboardEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool HandleKeyboardDown(SdlDotNet.Input.KeyboardEventArgs e) {
            if (initialized) {
                if (currentModalWindow != null) {
                    currentModalWindow.OnKeyboardDown(e);
                    return true;
                }

                if (Screen.activeWidget != null) {
                    bool value = Screen.HandleKeyboardDown(e);
                    if (value) {
                        return true;
                    }
                }

                if (activeWindow != null) {
                    activeWindow.OnKeyboardDown(e);
                    return true;
                } else if (windows.Count > 0) {
                    int topMostWindow = GetTopMostWindow(windows.Count - 1);
                    if (topMostWindow > -1) {
                        if (IsWindowActive(windows[topMostWindow])) {
                            windows[topMostWindow].OnKeyboardDown(e);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the keyboard up event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.KeyboardEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool HandleKeyboardUp(SdlDotNet.Input.KeyboardEventArgs e) {
            if (initialized) {

                if (currentModalWindow != null) {
                    currentModalWindow.OnKeyboardUp(e);
                    return true;
                }

                if (Screen.activeWidget != null) {
                    bool value = Screen.HandleKeyboardUp(e);
                    if (value) {
                        return true;
                    }
                }

                if (activeWindow != null) {
                    activeWindow.OnKeyboardUp(e);
                    return true;
                } else if (windows.Count > 0) {
                    int topMostWindow = GetTopMostWindow(windows.Count - 1);
                    if (topMostWindow > -1) {
                        if (IsWindowActive(windows[topMostWindow])) {
                            windows[topMostWindow].OnKeyboardUp(e);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Handles the mouse button down event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool HandleMouseButtonDown(SdlDotNet.Input.MouseButtonEventArgs e) {
            bool clicked = false;
            if (initialized) {

                if (overlayCollection.Items.Count > 0) {
                    bool clickedOnOverlay = false;
                    for (int i = 0; i < overlayCollection.Items.Count; i++) {
                        if (DrawingSupport.PointInBounds(e.Position, overlayCollection.Items[i].Bounds)) {
                            clickedOnOverlay = true;
                            overlayCollection.Items[i].OnMouseDown(new MouseButtonEventArgs(e, e.Position));
                            break;
                        }
                    }
                    if (!clickedOnOverlay) {
                        overlayCollection.ClearItems();
                    } else {
                        return true;
                    }
                }

                if (currentModalWindow != null) {
                    if (DrawingSupport.PointInBounds(e.Position, currentModalWindow.FullBounds)) {
                        currentModalWindow.OnMouseDown(new MouseButtonEventArgs(e, e.Position));
                        return true;
                    } else {
                        return false;
                    }
                }
                if (windows.Count > 0) {
                    for (int i = windows.Count - 1; i >= 0; i--) {
                        if (IsWindowActive(windows[i]) && DrawingSupport.PointInBounds(e.Position, windows[i].FullBounds)) {
                            if (windows[i].BackColor != Color.Transparent || DrawingSupport.PointInBounds(e.Position, windows[i].TitleBar.Bounds)) {
                                windows[i].OnMouseDown(new MouseButtonEventArgs(e, e.Position));
                                int topMostWindow = GetTopMostWindow(windows.Count - 1);
                                if (topMostWindow > -1) {
                                    BringWindowToFront(windows[i]);
                                    activeWindow = windows[i];
                                }
                                //								for (int w = 0; w < windows.Count; w++) {
                                //									if (windows[w].AlwaysOnTop) {
                                //										BringWindowToFront(windows[w]);
                                //									}
                                //								}
                                clicked = true;
                                break;
                            } else {
                                Point relPoint = new Point(e.Position.X - windows[i].X, e.Position.Y - windows[i].Y);
                                for (int n = windows[i].ChildWidgets.Count; n >= 0; n--) {
                                    if (DrawingSupport.PointInBounds(relPoint, windows[i].ChildWidgets[n].Bounds)) {
                                        windows[i].ChildWidgets[n].OnMouseDown(new MouseButtonEventArgs(e, e.Position));
                                        clicked = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (windowSwitcherEnabled && DrawingSupport.PointInBounds(e.Position, new Rectangle(windowSwitcher.Location, windowSwitcher.Size))) {
                    windowSwitcher.OnMouseDown(new MouseButtonEventArgs(e, e.Position));
                    clicked = true;
                }
            }
            if (clicked == false) {
                Screen.HandleMouseButtonDown(new MouseButtonEventArgs(e, e.Position));
            } else {
                Screen.activeWidget = null;
            }
            return clicked;
        }

        /// <summary>
        /// Handles the mouse button up event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool HandleMouseButtonUp(SdlDotNet.Input.MouseButtonEventArgs e) {
            if (heldScroller != null) {
                heldScroller = null;
                return true;
            }
            if (initialized) {

                if (overlayCollection.Items.Count > 0) {
                    bool clickedOnOverlay = false;
                    for (int i = 0; i < overlayCollection.Items.Count; i++) {
                        if (DrawingSupport.PointInBounds(e.Position, overlayCollection.Items[i].Bounds)) {
                            clickedOnOverlay = true;
                            overlayCollection.Items[i].OnMouseUp(new MouseButtonEventArgs(e, e.Position));
                            break;
                        }
                    }
                    if (!clickedOnOverlay) {
                        overlayCollection.ClearItems();
                    } else {
                        return true;
                    }
                }

                if (currentModalWindow != null) {
                    if (DrawingSupport.PointInBounds(e.Position, currentModalWindow.FullBounds)) {
                        currentModalWindow.OnMouseUp(new MouseButtonEventArgs(e, e.Position));
                        return true;
                    } else {
                        return false;
                    }
                }
                if (windows.Count > 0) {
                    for (int i = windows.Count - 1; i >= 0; i--) {
                        if (IsWindowActive(windows[i]) && DrawingSupport.PointInBounds(e.Position, windows[i].FullBounds)) {
                            if (windows[i].BackColor != Color.Transparent || DrawingSupport.PointInBounds(e.Position, windows[i].TitleBar.Bounds)) {
                                windows[i].OnMouseUp(new MouseButtonEventArgs(e, e.Position));
                                return true;
                            } else {
                                Point relPoint = new Point(e.Position.X - windows[i].X, e.Position.Y - windows[i].Y);
                                for (int n = 0; n < windows[i].ChildWidgets.Count; n++) {
                                    if (DrawingSupport.PointInBounds(relPoint, windows[i].ChildWidgets[n].Bounds)) {
                                        windows[i].ChildWidgets[n].OnMouseUp(new MouseButtonEventArgs(e, e.Position));
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Screen.HandleMouseButtonUp(new MouseButtonEventArgs(e, e.Position));
            return false;
        }

        /// <summary>
        /// Handles the mouse motion event.
        /// </summary>
        /// <param name="e">The <see cref="SdlDotNet.Input.MouseMotionEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool HandleMouseMotion(SdlDotNet.Input.MouseMotionEventArgs e) {
            if (heldScroller != null) {
                heldScroller.OnMouseMotion(e);
                return true;
            }
            if (initialized) {

                if (overlayCollection.Items.Count > 0) {
                    bool clickedOnOverlay = false;
                    for (int i = 0; i < overlayCollection.Items.Count; i++) {
                        if (DrawingSupport.PointInBounds(e.Position, overlayCollection.Items[i].Bounds)) {
                            clickedOnOverlay = true;
                            overlayCollection.Items[i].OnMouseMotion(e);
                            break;
                        }
                    }
                    if (clickedOnOverlay) {
                        return true;
                    }
                }

                if (currentModalWindow != null) {
                    if (DrawingSupport.PointInBounds(e.Position, currentModalWindow.FullBounds)) {
                        currentModalWindow.OnMouseMotion(e);
                        return true;
                    } else {
                        return false;
                    }
                }
                if (windows.Count > 0) {
                    for (int i = windows.Count - 1; i >= 0; i--) {
                        if (IsWindowActive(windows[i]) && DrawingSupport.PointInBounds(e.Position, windows[i].FullBounds)) {
                            if (windows[i].BackColor != Color.Transparent || DrawingSupport.PointInBounds(e.Position, windows[i].TitleBar.Bounds)) {
                                windows[i].OnMouseMotion(e);
                                return true;
                            } else {
                                Point relPoint = new Point(e.Position.X - windows[i].X, e.Position.Y - windows[i].Y);
                                for (int n = 0; n < windows[i].ChildWidgets.Count; n++) {
                                    if (DrawingSupport.PointInBounds(relPoint, windows[i].ChildWidgets[n].Bounds)) {
                                        windows[i].ChildWidgets[n].OnMouseMotion(e);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Screen.HandleMouseMotion(e);
            return false;
        }

        public static void Invoke(Delegate @delegate, params object[] parameters) {
            InvocationItem item = new InvocationItem(@delegate, parameters);
            invocationList.Add(item);
            item.ResetEvent.WaitOne();
        }

        /// <summary>
        /// Determines whether a window is active.
        /// </summary>
        /// <param name="windowToTest">The window to test.</param>
        /// <returns>
        /// 	<c>true</c> if the window is active; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindowActive(Window windowToTest) {
            if (windowToTest != null) {
                return (windowToTest.Visible && windowToTest.WindowState == WindowState.Normal);
            } else {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified window is open.
        /// </summary>
        /// <param name="windowToTest">The window to test.</param>
        /// <returns>
        /// 	<c>true</c> if the specified window is open; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWindowOpen(Window windowToTest) {
            if (initialized) {
                return (windows.Contains(windowToTest));
            } else {
                return false;
            }
        }

        /// <summary>
        /// Sets the main thread.
        /// </summary>
        public static void SetMainThread() {
            mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Toggles the window switcher.
        /// </summary>
        /// <param name="enable">if set to <c>true</c>, the window switcher will be enabled and visible.</param>
        public static void ToggleWindowSwitcher(bool enable) {
            if (enable) {
                windowSwitcherEnabled = true;
                CreateActiveWindowButtons();
            } else {
                windowSwitcherEnabled = false;
            }
        }

        /// <summary>
        /// Adds a widget to the overlay collection.
        /// </summary>
        /// <param name="widget">The widget.</param>
        /// <param name="active">if set to <c>true</c>, the widget will be marked as the active widget.</param>
        internal static void AddToOverlayCollection(Widget widget, bool active) {
            overlayCollection.Items.Add(widget);
            if (active) {
                overlayCollection.ActiveWidget = widget;
            }
        }

        /// <summary>
        /// Initializes the window manager.
        /// </summary>
        /// <param name="destinationSurface">The destination surface.</param>
        internal static void Initialize(SdlDotNet.Graphics.Surface destinationSurface) {
            if (windows != null) {
                for (int i = 0; i < windows.Count; i++) {
                    windows[i].FreeResources();
                }
            }
            invocationList = new List<InvocationItem>();
            if (mainThreadID == -1) {
                mainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            }
            windows = new WindowCollection();
            if (destSurf != null) {
                destSurf.Dispose();
                destSurf = null;
            }
            destSurf = destinationSurface;
            windowSwitcher = new WindowSwitcher(destSurf.Width);
            windowSwitcher.Location = new Point(0, destSurf.Height - windowSwitcher.Height);

            Screen.Initialize();
            overlayCollection = new WidgetOverlayCollection();

            SetMainThread();

            initialized = true;
        }

        /// <summary>
        /// Removes a widget from overlay collection.
        /// </summary>
        /// <param name="widget">The widget.</param>
        internal static void RemoveFromOverlayCollection(Widget widget) {
            if (overlayCollection.Items.Contains(widget)) {
                overlayCollection.Items.Remove(widget);
            }
        }

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="window">The window.</param>
        internal static void RemoveWindow(Window window) {
            if (initialized && windows.Contains(window)) {
                int index = windows.IndexOf(window);
                if (windowSwitcherEnabled) {
                    windowSwitcher.RemoveButton(windows[index]);
                }
                if (WindowRemoved != null)
                    WindowRemoved(null, new WindowRemovedEventArgs(windows[index]));
                windows[index].FreeResources();
                windows.RemoveWindow(window);
                if (windows.Count > 0) {
                    windows[windows.Count - 1].TopMost = true;
                }
            }
        }

        /// <summary>
        /// Removes the window.
        /// </summary>
        /// <param name="name">The name.</param>
        internal static void RemoveWindow(string name) {
            if (initialized) {
                if (windowSwitcherEnabled && windows.Contains(name)) {
                    windowSwitcher.RemoveButton(windows[windows.IndexOf(name)]);
                }
                windows[windows.IndexOf(name)].FreeResources();
                windows.RemoveWindow(name);
            }
        }

        /// <summary>
        /// Creates the active window buttons used by the window switcher.
        /// </summary>
        private static void CreateActiveWindowButtons() {
            if (initialized) {
                windowSwitcher.ClearButtons();
                for (int i = 0; i < windows.Count; i++) {
                    if (windows[i].ShowInWindowSwitcher) {
                        windowSwitcher.AddButton(new WindowSwitcherButton(windows[i]));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the top most window.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        private static int GetTopMostWindow(int start) {
            if (start != -1) {
                if (IsWindowActive(windows[start]) == false) {
                    return GetTopMostWindow(start - 1);
                } else {
                    return start;
                }
            } else {
                return -1;
            }
        }

        #endregion Methods
    }
}