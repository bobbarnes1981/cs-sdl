#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SdlDotNet.Graphics;
using SdlDotNet.Input;
using SdlDotNet.Core;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class UIScreen
    {
        Surface background;

        /// <summary>
        ///
        /// </summary>
        private CursorAnimator cursor;
        /// <summary>
        ///
        /// </summary>
        protected CursorAnimator Cursor
        {
            get { return cursor; }
            set { cursor = value; }
        }
        /// <summary>
        ///
        /// </summary>
        private UIPainter uiPainter;
        /// <summary>
        ///
        /// </summary>
        protected UIPainter UIPainter
        {
            get { return uiPainter; }
            set { uiPainter = value; }
        }

        /// <summary>
        /// /
        /// </summary>
        private Bin bin;
        /// <summary>
        ///
        /// </summary>
        protected Bin Bin
        {
            get { return bin; }
            set { bin = value; }
        }
        Mpq mpq;
        Painter painter;
        UIScreen parent;

        /// <summary>
        ///
        /// </summary>
        public UIScreen Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string prefix;
        /// <summary>
        ///
        /// </summary>
        protected string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string binFile;
        /// <summary>
        ///
        /// </summary>
        protected string BinFile
        {
            get { return binFile; }
            set { binFile = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string backgroundPath;

        /// <summary>
        ///
        /// </summary>
        protected string BackgroundPath
        {
            get { return backgroundPath; }
            set { backgroundPath = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private int backgroundTransparent;
        /// <summary>
        ///
        /// </summary>
        protected int BackgroundTransparent
        {
            get { return backgroundTransparent; }
            set { backgroundTransparent = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private int backgroundTranslucent;
        /// <summary>
        ///
        /// </summary>
        protected int BackgroundTranslucent
        {
            get { return backgroundTranslucent; }
            set { backgroundTranslucent = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string fontPalettePath;
        /// <summary>
        ///
        /// </summary>
        protected string FontPalettePath
        {
            get { return fontPalettePath; }
            set { fontPalettePath = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string effectPalettePath;
        /// <summary>
        ///
        /// </summary>
        protected string EffectPalettePath
        {
            get { return effectPalettePath; }
            set { effectPalettePath = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private string arrowGrpPath;
        /// <summary>
        ///
        /// </summary>
        protected string ArrowGrpPath
        {
            get { return arrowGrpPath; }
            set { arrowGrpPath = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private Pcx fontPalette;
        /// <summary>
        ///
        /// </summary>
        protected Pcx FontPalette
        {
            get { return fontPalette; }
            set { fontPalette = value; }
        }
        /// <summary>
        ///
        /// </summary>
        private Pcx effectPalette;
        /// <summary>
        ///
        /// </summary>
        protected Pcx EffectPalette
        {
            get { return effectPalette; }
            set { effectPalette = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private Collection<UIElement> elements;
        /// <summary>
        ///
        /// </summary>
        protected Collection<UIElement> Elements
        {
            get { return elements; }
        }

        //protected Painter painter;
        /// <summary>
        ///
        /// </summary>
        private UIDialog dialog; /* the currently popped up dialog */
        /// <summary>
        ///
        /// </summary>
        protected UIDialog Dialog
        {
            get { return dialog; }
            set { dialog = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="prefix"></param>
        /// <param name="binFile"></param>
        protected UIScreen(Mpq mpq, string prefix, string binFile)
        {
            this.mpq = mpq;
            this.prefix = prefix;
            this.binFile = binFile;

            if (prefix != null)
            {
                backgroundPath = prefix + "\\Backgnd.pcx";
                fontPalettePath = prefix + "\\tFont.pcx";
                effectPalettePath = prefix + "\\tEffect.pcx";
                arrowGrpPath = prefix + "\\arrow.grp";
            }

            //backgroundTransparent = 0;
            backgroundTranslucent = 254;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        protected UIScreen(Mpq mpq)
        {
            this.mpq = mpq;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SwooshIn()
        {
            try
            {
                Console.WriteLine("swooshing in");
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(RaiseDoneSwooshing)));
            }
            catch (SdlException e)
            {
                Console.WriteLine("failed pushing UIScreen.RiseDoneSwooshing: {0}", e);
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(Game.Quit)));
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void SwooshOut()
        {
            try
            {
                Console.WriteLine("swooshing out");
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(RaiseDoneSwooshing)));
            }
            catch (SdlException e)
            {
                Console.WriteLine("failed pushing UIScreen.RiseDoneSwooshing: {0}", e);
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(Game.Quit)));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public virtual void AddToPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            this.painter = painter;

            painter.Add(Layer.Background, FirstPaint);

            if (background != null)
            {
                painter.Add(Layer.Background, BackgroundPainter);
            }

            if (uiPainter != null)
            {
                painter.Add(Layer.UI, uiPainter.Paint);
            }

            if (cursor != null)
            {
                Game.Instance.Cursor = cursor;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="painter"></param>
        public virtual void RemoveFromPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            painter.Remove(Layer.Background, FirstPaint);

            if (background != null)
            {
                painter.Remove(Layer.Background, BackgroundPainter);
            }
            if (uiPainter != null)
            {
                painter.Remove(Layer.UI, uiPainter.Paint);
            }
            if (cursor != null)
            {
                Game.Instance.Cursor = null;
            }

            this.painter = null;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool UseTiles
        {
            get { return false; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Painter Painter
        {
            get { return painter; }
            set { this.painter = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Mpq Mpq
        {
            get { return mpq; }
            set { this.mpq = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Surface Background
        {
            get { return background; }
        }

        UIElement XYToElement(int x, int y, bool onlyUI)
        {
            if (elements == null)
            {
                return null;
            }

            foreach (UIElement e in elements)
            {
                if (e.Type == ElementType.DialogBox)
                {
                    continue;
                }

                if (onlyUI &&
                e.Type == ElementType.Image)
                {
                    continue;
                }

                if (e.Visible && e.PointInside(x, y))
                {
                    return e;
                }
            }
            return null;
        }

        UIElement mouseDownElement;
        UIElement mouseOverElement;

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        public virtual void MouseEnterElement(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.MouseEnter();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        public virtual void MouseLeaveElement(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.MouseLeave();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        public virtual void ActivateElement(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (!element.Visible || !element.Sensitive)
            {
                return;
            }

            Console.WriteLine("activating element {0}", elements.IndexOf(element));
            element.OnActivate();
        }

        /// <summary>
        /// SDL Event handling
        /// </summary>
        /// <param name="args"></param>
        public virtual void MouseButtonDown(MouseButtonEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Button != MouseButton.PrimaryButton &&
            args.Button != MouseButton.WheelUp &&
            args.Button != MouseButton.WheelDown)
            {
                return;
            }

            if (mouseDownElement != null)
            {
                Console.WriteLine("mouseDownElement already set in MouseButtonDown");
            }

            UIElement element = XYToElement(args.X, args.Y, true);
            if (element != null && element.Visible && element.Sensitive)
            {
                mouseDownElement = element;
                if (args.Button == MouseButton.PrimaryButton)
                {
                    mouseDownElement.MouseButtonDown(args);
                }
                else
                {
                    mouseDownElement.MouseWheel(args);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void HandleMouseButtonDown(MouseButtonEventArgs args)
        {
            if (dialog != null)
            {
                dialog.HandleMouseButtonDown(args);
            }
            else
            {
                MouseButtonDown(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void MouseButtonUp(MouseButtonEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Button != MouseButton.PrimaryButton &&
            args.Button != MouseButton.WheelUp &&
            args.Button != MouseButton.WheelDown)
            {
                return;
            }

            if (mouseDownElement != null)
            {
                if (args.Button == MouseButton.PrimaryButton)
                {
                    mouseDownElement.MouseButtonUp(args);
                }

                mouseDownElement = null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void HandleMouseButtonUp(MouseButtonEventArgs args)
        {
            if (dialog != null)
            {
                dialog.HandleMouseButtonUp(args);
            }
            else
            {
                MouseButtonUp(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void PointerMotion(MouseMotionEventArgs args)
        {
            if (mouseDownElement != null)
            {
                mouseDownElement.PointerMotion(args);
            }
            else
            {
                UIElement newMouseOverElement = XYToElement(args.X, args.Y, true);

                if (newMouseOverElement != mouseOverElement)
                {
                    if (mouseOverElement != null)
                    {
                        MouseLeaveElement(mouseOverElement);
                    }
                    if (newMouseOverElement != null)
                    {
                        MouseEnterElement(newMouseOverElement);
                    }
                }

                mouseOverElement = newMouseOverElement;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void HandlePointerMotion(MouseMotionEventArgs args)
        {
            if (dialog != null)
            {
                dialog.HandlePointerMotion(args);
            }
            else
            {
                PointerMotion(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void KeyboardUp(KeyboardEventArgs args)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void HandleKeyboardUp(KeyboardEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            /* just return if the modifier keys are released */
            if (args.Key >= Key.NumLock && args.Key <= Key.Compose)
            {
                return;
            }

            if (dialog != null)
            {
                dialog.HandleKeyboardUp(args);
            }
            else
            {
                KeyboardUp(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public virtual void KeyboardDown(KeyboardEventArgs args)
        {
            if (elements != null)
            {
                foreach (UIElement e in elements)
                {
                    if ((args.Key == e.Hotkey)
                    ||
                    (args.Key == Key.Return
                    && (e.Flags & SCElement.DefaultButton) == SCElement.DefaultButton)
                    ||
                    (args.Key == Key.Escape
                    && (e.Flags & SCElement.CancelButton) == SCElement.CancelButton))
                    {
                        ActivateElement(e);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        public void HandleKeyboardDown(KeyboardEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            /* just return if the modifier keys are pressed */
            if (args.Key >= Key.NumLock && args.Key <= Key.Compose)
            {
                return;
            }

            if (dialog != null)
            {
                dialog.HandleKeyboardDown(args);
            }
            else
            {
                KeyboardDown(args);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ScreenDisplayed()
        {
        }

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> FirstPainted;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> DoneSwooshing;

        /// <summary>
        ///
        /// </summary>
        public event EventHandler<SCEventArgs> Ready;

        bool loaded;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="now"></param>
        protected virtual void FirstPaint(Surface surf, DateTime now)
        {
            if (FirstPainted != null)
            {
                FirstPainted(this, new SCEventArgs());
            }

            painter.Remove(Layer.Background, FirstPaint);
        }

        /// <summary>
        ///
        /// </summary>
        protected void RaiseReadyEvent(object sender, EventArgs e)
        {
            if (Ready != null)
            {
                Ready(this, new SCEventArgs());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void RaiseDoneSwooshing(object sender, EventArgs e)
        {
            if (DoneSwooshing != null)
            {
                DoneSwooshing(this, new SCEventArgs());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="dt"></param>
        protected void BackgroundPainter(Surface surf, DateTime dt)
        {
            if (surf == null)
            {
                throw new ArgumentNullException("surf");
            }
            surf.Blit(background,
            new Point((surf.Width - background.Width) / 2,
            (surf.Height - background.Height) / 2));

        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResourceLoader()
        {
            Stream s;

            fontPalette = null;
            effectPalette = null;

            if (fontPalettePath != null)
            {
                Console.WriteLine("loading font palette");
                s = (Stream)mpq.GetResource(fontPalettePath);
                if (s != null)
                {
                    fontPalette = new Pcx();
                    fontPalette.ReadFromStream(s, -1, -1);
                }
            }
            if (effectPalettePath != null)
            {
                Console.WriteLine("loading cursor palette");
                s = (Stream)mpq.GetResource(effectPalettePath);
                if (s != null)
                {
                    effectPalette = new Pcx();
                    effectPalette.ReadFromStream(s, -1, -1);
                }
                if (effectPalette != null && arrowGrpPath != null)
                {
                    Console.WriteLine("loading arrow cursor");
                    Grp arrowgrp = (Grp)mpq.GetResource(arrowGrpPath);
                    if (arrowgrp != null)
                    {
                        cursor = new CursorAnimator(arrowgrp, effectPalette.Palette);
                        cursor.SetHotspot(64, 64);
                    }
                }
            }

            if (backgroundPath != null)
            {
                Console.WriteLine("loading background");
                background = GuiUtility.SurfaceFromStream((Stream)mpq.GetResource(backgroundPath),
                backgroundTranslucent, backgroundTransparent);
            }

            if (binFile != null)
            {
                Console.WriteLine("loading ui elements");
                bin = (Bin)mpq.GetResource(binFile);

                if (bin == null)
                {
                    throw new SCException(String.Format("specified file '{0}' does not exist",
                    binFile));
                }

                /* convert all the BinElements to UIElements for our subclasses to use */
                elements = new Collection<UIElement>();
                foreach (BinElement el in bin.Elements)
                {
                    // Console.WriteLine ("{0}: {1}", el.text, el.flags);

                    UIElement ui_el = null;
                    switch (el.Type)
                    {
                        case ElementType.DialogBox:
                            ui_el = new DialogBoxElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.Image:
                            ui_el = new ImageElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.TextBox:
                            ui_el = new TextBoxElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.ListBox:
                            ui_el = new ListBoxElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.ComboBox:
                            ui_el = new ComboBoxElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.LabelLeftAlign:
                        case ElementType.LabelCenterAlign:
                        case ElementType.LabelRightAlign:
                            ui_el = new LabelElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.Button:
                        case ElementType.DefaultButton:
                        case ElementType.ButtonWithoutBorder:
                            ui_el = new ButtonElement(this, el, fontPalette.RgbData);
                            break;
                        case ElementType.Slider:
                        case ElementType.OptionButton:
                        case ElementType.CheckBox:
                            ui_el = new UIElement(this, el, fontPalette.RgbData);
                            break;
                        default:
                            Console.WriteLine("unhandled case {0}", el.Type);
                            ui_el = new UIElement(this, el, fontPalette.RgbData);
                            break;
                    }

                    elements.Add(ui_el);
                }

                uiPainter = new UIPainter(elements);
            }
        }

        void LoadResources(object sender, EventArgs e)
        {
            ResourceLoader();
            Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(FinishedLoading)));
        }

        /// <summary>
        ///
        /// </summary>
        public void Load()
        {
            if (loaded)
            {
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(RaiseReadyEvent)));
            }
            else
            {
#if MULTI_THREADED
ThreadPool.QueueUserWorkItem (delegate (object state) { LoadResources (); })
#else
                Events.PushUserEvent(new UserEventArgs(new EventHandler<SCEventArgs>(LoadResources)));
#endif
            }
        }

        void FinishedLoading(object sender, EventArgs e)
        {
            loaded = true;
            RaiseReadyEvent(this, new SCEventArgs());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialog"></param>
        public virtual void ShowDialog(UIDialog dialog)
        {
            Console.WriteLine("showing {0}", dialog);

            if (this.dialog != null)
            {
                throw new SCException("only one active dialog is allowed");
            }
            this.dialog = dialog;

            dialog.Load();
            dialog.Ready += delegate(object sender, SCEventArgs e)
            {
                dialog.AddToPainter(painter);
            };
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void DismissDialog()
        {
            if (dialog == null)
            {
                return;
            }

            dialog.RemoveFromPainter(painter);
            dialog = null;
        }
    }
}
