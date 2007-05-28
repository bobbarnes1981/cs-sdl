#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SCSharp;
using SCSharp.MpqLib;
using System.Drawing;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MarkupScreen : UIScreen
    {
        SCFont fnt;
        byte[] pal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        protected MarkupScreen(Mpq mpq)
            : base(mpq)
        {
            pages = new List<MarkupPage>();
        }

        enum PageLocation
        {
            Center,
            Top,
            Bottom,
            Left,
            Right,
            LowerLeft
        }

        class MarkupPage
        {
            PageLocation location;
            List<string> lines;
            List<Surface> lineSurfaces;
            Surface newBackground;
            SCFont fnt;
            byte[] pal;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="loc"></param>
            /// <param name="font"></param>
            /// <param name="palette"></param>
            public MarkupPage(PageLocation loc, SCFont font, byte[] palette)
            {
                location = loc;
                lines = new List<string>();
                fnt = font;
                pal = palette;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="background"></param>
            public MarkupPage(Stream background)
            {
                newBackground = GuiUtility.SurfaceFromStream(background, 254, 0);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="line"></param>
            public void AddLine(string line)
            {
                lines.Add(line);
            }

            /// <summary>
            /// 
            /// </summary>
            public void Layout()
            {
                lineSurfaces = new List<Surface>();
                foreach (string l in lines)
                {
                    if (l.Trim().Length == 0)
                    {
                        lineSurfaces.Add(null);
                    }
                    else
                    {
                        lineSurfaces.Add(GuiUtility.ComposeText(l, fnt, pal));
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public Surface Background
            {
                get { return newBackground; }
            }

            /// <summary>
            /// 
            /// </summary>
            public bool HasText
            {
                get { return lines != null && lines.Count > 0; }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="surf"></param>
            public void Paint(Surface surf)
            {
                int y;

                switch (location)
                {
                    case PageLocation.Top:
                        y = 10;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point((surf.Width - s.Width) / 2, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                    case PageLocation.Bottom:
                        y = surf.Height - 10 - fnt.LineSize * lines.Count;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point((surf.Width - s.Width) / 2, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                    case PageLocation.Left:
                        y = (surf.Height - fnt.LineSize * lines.Count) / 2;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point(60, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                    case PageLocation.LowerLeft:
                        y = surf.Height - 10 - fnt.LineSize * lines.Count;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point(60, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                    case PageLocation.Right:
                        y = (surf.Height - fnt.LineSize * lines.Count) / 2;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point(surf.Width - s.Width - 60, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                    case PageLocation.Center:
                        y = (surf.Height - fnt.LineSize * lines.Count) / 2;
                        foreach (Surface s in lineSurfaces)
                        {
                            if (s != null)
                            {
                                surf.Blit(s, new Point((surf.Width - s.Width) / 2, y));
                            }
                            y += fnt.LineSize;
                        }
                        break;
                }
            }
        }

        List<MarkupPage> pages;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        protected void AddMarkup(Stream stream)
        {
            string line;
            MarkupPage currentPage = null;

            StreamReader sr = new StreamReader(stream);

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("</"))
                {
                    if (line.StartsWith("</PAGE>"))
                    {
                        currentPage.Layout();
                        pages.Add(currentPage);
                        currentPage = null;
                    }
                    else if (line.StartsWith("</SCREENCENTER>"))
                    {
                        currentPage = new MarkupPage(PageLocation.Center, fnt, pal);
                    }
                    else if (line.StartsWith("</SCREENLEFT>"))
                    {
                        currentPage = new MarkupPage(PageLocation.Left, fnt, pal);
                    }
                    else if (line.StartsWith("</SCREENLOWERLEFT>"))
                    {
                        currentPage = new MarkupPage(PageLocation.LowerLeft, fnt, pal);
                    }
                    else if (line.StartsWith("</SCREENRIGHT>"))
                    {
                        currentPage = new MarkupPage(PageLocation.Right, fnt, pal);
                    }
                    else if (line.StartsWith("</SCREENTOP>"))
                    {
                        currentPage = new MarkupPage(PageLocation.Top, fnt, pal);
                    }
                    else if (line.StartsWith("</SCREENBOTTOM>"))
                    {
                        currentPage = new MarkupPage(PageLocation.Bottom, fnt, pal);
                    }
                    else if (line.StartsWith("</BACKGROUND "))
                    {
                        string bg = line.Substring("</BACKGROUND ".Length);
                        bg = bg.Substring(0, bg.Length - 1);
                        pages.Add(new MarkupPage((Stream)this.Mpq.GetResource(bg)));
                    }
                    /* skip everything else */
#if false
					else if (l.StartsWith ("</FONTCOLOR")
						 || l.StartsWith ("</COMMENT")
						 || l.StartsWith ("</FADESPEED")) {
					}
#endif

                }
                else if (currentPage != null)
                {
                    currentPage.AddLine(line);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ResourceLoader()
        {
            Console.WriteLine("loading font palette");
            Stream palStream = (Stream)this.Mpq.GetResource("glue\\palmm\\tfont.pcx");
            Pcx pcx = new Pcx();
            pcx.ReadFromStream(palStream, -1, -1);

            pal = pcx.RgbData;

            Console.WriteLine("loading font");
            fnt = GuiUtility.GetFonts(this.Mpq)[3];

            Console.WriteLine("loading markup");
            LoadMarkup();

            /* set things up so we're ready to go */
            millisDelay = 4000;
            pageEnumerator = pages.GetEnumerator();
            AdvanceToNextPage();
        }

        // painting
        Surface currentBackground;
        IEnumerator<MarkupPage> pageEnumerator;

        int millisDelay;
        int totalElapsed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void AddToPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            base.AddToPainter(painter);
            painter.Add(Layer.Background, PaintBackground);
            painter.Add(Layer.UI, PaintMarkup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void RemoveFromPainter(Painter painter)
        {
            if (painter == null)
            {
                throw new ArgumentNullException("painter");
            }
            base.RemoveFromPainter(painter);
            painter.Remove(Layer.Background, PaintBackground);
            painter.Remove(Layer.UI, PaintMarkup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="now"></param>
        protected override void FirstPaint(Surface surf, DateTime now)
        {
            base.FirstPaint(surf, now);

            /* set ourselves up to invalidate at a regular interval*/
            Events.Tick += FlipPage;
        }

        void PaintBackground(Surface surf, DateTime now)
        {
            if (currentBackground != null)
            {
                surf.Blit(currentBackground);
            }
        }

        void PaintMarkup(Surface surf, DateTime now)
        {
            pageEnumerator.Current.Paint(surf);
        }

        void FlipPage(object sender, TickEventArgs e)
        {
            totalElapsed += e.TicksElapsed;

            if (totalElapsed < millisDelay)
            {
                return;
            }

            totalElapsed = 0;
            AdvanceToNextPage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardDown(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Escape:
                    Events.Tick -= FlipPage;
                    MarkupFinished();
                    break;
                case Key.Space:
                case Key.Return:
                    totalElapsed = 0;
                    AdvanceToNextPage();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardUp(KeyboardEventArgs args)
        {
        }

        void AdvanceToNextPage()
        {
            while (pageEnumerator.MoveNext())
            {
                if (pageEnumerator.Current.Background != null)
                {
                    currentBackground = pageEnumerator.Current.Background;
                }
                if (pageEnumerator.Current.HasText)
                {
                    return;
                }
            }

            Console.WriteLine("finished!");
            Events.Tick -= FlipPage;
            MarkupFinished();
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void LoadMarkup();

        /// <summary>
        /// 
        /// </summary>
        protected abstract void MarkupFinished();
    }
}
