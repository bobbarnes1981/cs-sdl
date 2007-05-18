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
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;

using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Input;
using SCSharp;
using SCSharp.MpqLib;


namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GameScreen : UIScreen
    {
        //Mpq scenario_mpq;

        Surface hud;
        Chk scenario;
        Got template;

        /* the deltas associated with scrolling */
        int horiz_delta;
        int vert_delta;

        /* the x,y of the topleft of the screen */
        int topleft_x, topleft_y;

        /* magic number alert.. */
        const int MINIMAP_X = 7;
        const int MINIMAP_Y = 349;
        const int MINIMAP_WIDTH = 125;
        const int MINIMAP_HEIGHT = 125;

        const int SCROLL_DELTA = 15;
        const int MOUSE_MOVE_BORDER = 10;

        const int SCROLL_CURSOR_UL = 0;
        const int SCROLL_CURSOR_U = 1;
        const int SCROLL_CURSOR_UR = 2;
        const int SCROLL_CURSOR_R = 3;
        const int SCROLL_CURSOR_DR = 4;
        const int SCROLL_CURSOR_D = 5;
        const int SCROLL_CURSOR_DL = 6;
        const int SCROLL_CURSOR_L = 7;

        CursorAnimator[] ScrollCursors;

        const int TARG_CURSOR_G = 0;
        const int TARG_CURSOR_Y = 1;
        const int TARG_CURSOR_R = 2;

        CursorAnimator[] TargetCursors;

        const int MAG_CURSOR_G = 0;
        const int MAG_CURSOR_Y = 1;
        const int MAG_CURSOR_R = 2;

        CursorAnimator[] MagCursors;

        //byte[] unit_palette;
        byte[] tileset_palette;

        //		Player[] players;

        List<Unit> units;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="scenario_mpq"></param>
        /// <param name="scenario"></param>
        /// <param name="template"></param>
        public GameScreen(Mpq mpq,
                   Mpq scenario_mpq,
                   Chk scenario,
                   Got template)
            : base(mpq)
        {
            this.EffectpalPath = "game\\tblink.pcx";
            this.ArrowgrpPath = "cursor\\arrow.grp";
            this.FontpalPath = "game\\tfontgam.pcx";
            //this.scenario_mpq = scenario_mpq;
            this.scenario = scenario;
            this.template = template;
            ScrollCursors = new CursorAnimator[8];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="prefix"></param>
        /// <param name="scenario"></param>
        public GameScreen(Mpq mpq,
                   string prefix,
                   Chk scenario)
            : base(mpq)
        {
            this.EffectpalPath = "game\\tblink.pcx";
            this.ArrowgrpPath = "cursor\\arrow.grp";
            this.FontpalPath = "game\\tfontgam.pcx";
            //this.scenario_mpq = scenario_mpq;
            this.scenario = scenario;
            ScrollCursors = new CursorAnimator[8];
        }

        Surface[] starfield_layers;

        void PaintStarfield(Surface surf, DateTime dt)
        {
            float scroll_factor = 1.0f;
            float[] factors = new float[starfield_layers.Length];

            for (int i = 0; i < starfield_layers.Length; i++)
            {
                factors[i] = scroll_factor;
                scroll_factor *= 0.75f;
            }

            for (int i = starfield_layers.Length - 1; i >= 0; i--)
            {
                int scroll_x = (int)(topleft_x * factors[i]);
                int scroll_y = (int)(topleft_y * factors[i]);

                if (scroll_x > Painter.ScreenResX)
                {
                    scroll_x %= Painter.ScreenResX;
                }
                if (scroll_y > Painter.ScreenResY)
                {
                    scroll_y %= Painter.ScreenResY;
                }

                surf.Blit(starfield_layers[i],
                       new Rectangle(new Point(0, 0),
                              new Size(Painter.ScreenResX - scroll_x,
                                    Painter.ScreenResY - scroll_y)),
                       new Rectangle(new Point(scroll_x, scroll_y),
                              new Size(Painter.ScreenResX - scroll_x,
                                    Painter.ScreenResY - scroll_y)));

                if (scroll_x != 0)
                {
                    surf.Blit(starfield_layers[i],
                           new Rectangle(new Point(Painter.ScreenResX - scroll_x, 0),
                                  new Size(scroll_x, Painter.ScreenResY - scroll_y)),
                           new Rectangle(new Point(0, scroll_y),
                                  new Size(scroll_x, Painter.ScreenResY - scroll_y)));
                }

                if (scroll_y != 0)
                {
                    surf.Blit(starfield_layers[i],
                           new Rectangle(new Point(0, Painter.ScreenResY - scroll_y),
                                  new Size(Painter.ScreenResX - scroll_x, scroll_y)),
                           new Rectangle(new Point(scroll_x, 0),
                                  new Size(Painter.ScreenResX - scroll_x, scroll_y)));
                }

                if (scroll_x != 0 || scroll_y != 0)
                {
                    surf.Blit(starfield_layers[i],
                           new Rectangle(new Point(Painter.ScreenResX - scroll_x, Painter.ScreenResY - scroll_y),
                                  new Size(scroll_x, scroll_y)),
                           new Rectangle(new Point(0, 0),
                                  new Size(scroll_x, scroll_y)));
                }
            }
        }

        Surface map_surf;

        void PaintMap(Surface surf, DateTime dt)
        {
            surf.Blit(map_surf,
                   new Rectangle(new Point(0, 0),
                          new Size(Painter.ScreenResX - topleft_x,
                                Painter.ScreenResY - topleft_y)),
                   new Rectangle(new Point(topleft_x, topleft_y),
                          new Size(Painter.ScreenResX,
                                Painter.ScreenResY)));
        }

        void PaintHud(Surface surf, DateTime dt)
        {
            surf.Blit(hud);
        }

        void PaintMinimap(Surface surf, DateTime dt)
        {
            Rectangle rect = new Rectangle(new Point((int)((float)topleft_x / (float)map_surf.Width * MINIMAP_WIDTH + MINIMAP_X),
                                   (int)((float)topleft_y / (float)map_surf.Height * MINIMAP_HEIGHT + MINIMAP_Y)),
                            new Size((int)((float)Painter.ScreenResX / (float)map_surf.Width * MINIMAP_WIDTH),
                                  (int)((float)Painter.ScreenResY / (float)map_surf.Height * MINIMAP_HEIGHT)));

            surf.Draw(new Box(rect.Location, rect.Size), Color.Green);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void AddToPainter(Painter painter)
        {
            base.AddToPainter(painter);

            painter.Add(Layer.Hud, PaintHud);
            painter.Add(Layer.Hud, PaintMinimap);

            if (scenario.Tileset == Tileset.Platform)
            {
                painter.Add(Layer.Background, PaintStarfield);
            }

            painter.Add(Layer.Map, PaintMap);
            SpriteManager.AddToPainter(painter);
            painter.Add(Layer.Background, ScrollPainter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painter"></param>
        public override void RemoveFromPainter(Painter painter)
        {
            base.RemoveFromPainter(painter);
            painter.Remove(Layer.Hud, PaintHud);
            painter.Remove(Layer.Hud, PaintMinimap);

            if (scenario.Tileset == Tileset.Platform)
            {
                painter.Remove(Layer.Background, PaintStarfield);
            }

            painter.Remove(Layer.Map, PaintMap);
            SpriteManager.RemoveFromPainter(painter);

            painter.Remove(Layer.Background, ScrollPainter);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ResourceLoader()
        {
            base.ResourceLoader();

            Pcx pcx = new Pcx();
            pcx.ReadFromStream((Stream)this.Mpq.GetResource("game\\tunit.pcx"), -1, -1);
            //unit_palette = pcx.Palette;

            pcx = new Pcx();
            pcx.ReadFromStream((Stream)this.Mpq.GetResource("tileset\\badlands\\dark.pcx"), 0, 0);
            tileset_palette = pcx.Palette;

            hud = GuiUtil.SurfaceFromStream((Stream)this.Mpq.GetResource(String.Format(Builtins.GameConsolePcx,
                                                 Utilities.RaceCharLower[(int)Game.Instance.Race])),
                             254, 0);

            if (scenario.Tileset == Tileset.Platform)
            {
                Spk starfield = (Spk)this.Mpq.GetResource("parallax\\star.spk");

                starfield_layers = new Surface[starfield.Layers.Length];
                for (int i = 0; i < starfield_layers.Length; i++)
                {
                    starfield_layers[i] = new Surface(Painter.ScreenResX, Painter.ScreenResY);

                    starfield_layers[i].TransparentColor = Color.Black;

                    for (int o = 0; o < starfield.Layers[i].Objects.Length; o++)
                    {
                        ParallaxObject obj = starfield.Layers[i].Objects[o];

                        starfield_layers[i].Fill(new Rectangle(new Point(obj.X, obj.Y), new Size(2, 2)),
                                      Color.White);
                    }
                }
            }

            map_surf = MapRenderer.RenderToSurface(this.Mpq, scenario);

            // load the cursors we'll show when scrolling with the mouse
            string[] cursornames = new string[] {
				"cursor\\ScrollUL.grp",
				"cursor\\ScrollU.grp",
				"cursor\\ScrollUR.grp",
				"cursor\\ScrollR.grp",
				"cursor\\ScrollDR.grp",
				"cursor\\ScrollD.grp",
				"cursor\\ScrollDL.grp",
				"cursor\\ScrollL.grp",
			};
            ScrollCursors = new CursorAnimator[cursornames.Length];
            for (int i = 0; i < cursornames.Length; i++)
            {
                ScrollCursors[i] = new CursorAnimator((Grp)this.Mpq.GetResource(cursornames[i]),
                                       Effectpal.Palette);
                ScrollCursors[i].SetHotSpot(60, 60);
            }

            // load the mag cursors
            string[] magcursornames = new string[] {
				"cursor\\MagG.grp",
				"cursor\\MagY.grp",
				"cursor\\MagR.grp"
			};
            MagCursors = new CursorAnimator[magcursornames.Length];
            for (int i = 0; i < magcursornames.Length; i++)
            {
                MagCursors[i] = new CursorAnimator((Grp)this.Mpq.GetResource(magcursornames[i]),
                                    Effectpal.Palette);
                MagCursors[i].SetHotSpot(60, 60);
            }

            // load the targeting cursors
            string[] targetcursornames = new string[] {
				"cursor\\TargG.grp",
				"cursor\\TargY.grp",
				"cursor\\TargR.grp"
			};
            TargetCursors = new CursorAnimator[targetcursornames.Length];
            for (int i = 0; i < targetcursornames.Length; i++)
            {
                TargetCursors[i] = new CursorAnimator((Grp)this.Mpq.GetResource(targetcursornames[i]),
                                       Effectpal.Palette);
                TargetCursors[i].SetHotSpot(60, 60);
            }

            PlaceInitialUnits();
        }

        void ClipTopLeft()
        {
            if (topleft_x < 0) topleft_x = 0;
            if (topleft_y < 0) topleft_y = 0;

            if (topleft_x > map_surf.Width - Painter.ScreenResX) topleft_x = map_surf.Width - Painter.ScreenResX;
            if (topleft_y > map_surf.Height - Painter.ScreenResY) topleft_y = map_surf.Height - Painter.ScreenResY;
        }

        void UpdateCursor()
        {
            /* are we over a unit?  if so, display the mag cursor */
            unitUnderCursor = null;
            for (int i = 0; i < units.Count; i++)
            {
                Unit u = units[i];
                Sprite s = u.Sprite;

                int sx, sy;

                s.GetTopLeftPosition(out sx, out sy);

                CursorAnimator c = Game.Instance.Cursor;

                if (c.X + topleft_x > sx && c.X + topleft_x <= sx + 100 /* XXX */
                    && c.Y + topleft_y > sy && c.Y + topleft_y <= sy + 100 /* XXX */)
                {
                    Game.Instance.Cursor = MagCursors[MAG_CURSOR_G];
                    unitUnderCursor = u;
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surf"></param>
        /// <param name="dt"></param>
        public void ScrollPainter(Surface surf, DateTime dt)
        {
            topleft_x += horiz_delta;
            topleft_y += vert_delta;

            ClipTopLeft();

            SpriteManager.SetUpperLeft(topleft_x, topleft_y);

            UpdateCursor();
        }

        bool buttonDownInMinimap;
        Unit unitUnderCursor;

        void Recenter(int x, int y)
        {
            topleft_x = x - Painter.ScreenResX / 2;
            topleft_y = y - Painter.ScreenResY / 2;

            ClipTopLeft();
        }

        void RecenterFromMinimap(int x, int y)
        {
            int map_x = (int)((float)(x - MINIMAP_X) / MINIMAP_WIDTH * map_surf.Width);
            int map_y = (int)((float)(y - MINIMAP_Y) / MINIMAP_HEIGHT * map_surf.Height);

            Recenter(map_x, map_y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonDown(MouseButtonEventArgs args)
        {
            if (args.X > MINIMAP_X && args.X < MINIMAP_X + MINIMAP_WIDTH &&
                args.Y > MINIMAP_Y && args.Y < MINIMAP_Y + MINIMAP_HEIGHT)
            {
                RecenterFromMinimap(args.X, args.Y);
                buttonDownInMinimap = true;
            }
            else
            {
                if (unitUnderCursor != null)
                {
                    Console.WriteLine("selected unit: {0}", unitUnderCursor);
                    Console.WriteLine("selectioncircle = {0}", unitUnderCursor.SelectionCircleOffset);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void MouseButtonUp(MouseButtonEventArgs args)
        {
            if (buttonDownInMinimap)
            {
                buttonDownInMinimap = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void PointerMotion(MouseMotionEventArgs args)
        {
            if (buttonDownInMinimap)
            {
                RecenterFromMinimap(args.X, args.Y);
            }
            else
            {
                if (args.X < MOUSE_MOVE_BORDER)
                {
                    horiz_delta = -SCROLL_DELTA;
                }
                else if (args.X + MOUSE_MOVE_BORDER > Painter.ScreenResX)
                {
                    horiz_delta = SCROLL_DELTA;
                }
                else
                {
                    horiz_delta = 0;
                }

                if (args.Y < MOUSE_MOVE_BORDER)
                {
                    vert_delta = -SCROLL_DELTA;
                }
                else if (args.Y + MOUSE_MOVE_BORDER > Painter.ScreenResY)
                {
                    vert_delta = SCROLL_DELTA;
                }
                else
                {
                    vert_delta = 0;
                }

                if (horiz_delta < 0)
                {
                    if (vert_delta < 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_UL];
                    }
                    else if (vert_delta > 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_DL];
                    }
                    else
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_L];
                    }
                }
                else if (horiz_delta > 0)
                {
                    if (vert_delta < 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_UR];
                    }
                    else if (vert_delta > 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_DR];
                    }
                    else
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_R];
                    }
                }
                else
                {
                    if (vert_delta < 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_U];
                    }
                    else if (vert_delta > 0)
                    {
                        Game.Instance.Cursor = ScrollCursors[SCROLL_CURSOR_D];
                    }
                    else
                    {
                        Game.Instance.Cursor = Cursor;
                    }
                }

                UpdateCursor();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardUp(KeyboardEventArgs args)
        {
            if (args.Key == Key.RightArrow
                || args.Key == Key.LeftArrow)
            {
                horiz_delta = 0;
            }
            else if (args.Key == Key.UpArrow
                 || args.Key == Key.DownArrow)
            {
                vert_delta = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void KeyboardDown(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Key.F10:
                    GameMenuDialog d = new GameMenuDialog(this, this.Mpq);

                    d.ReturnToGame += delegate() 
                    { 
                        DismissDialog(); 
                    };
                    ShowDialog(d);
                    break;

                case Key.RightArrow:
                    horiz_delta = SCROLL_DELTA;
                    break;
                case Key.LeftArrow:
                    horiz_delta = -SCROLL_DELTA;
                    break;
                case Key.DownArrow:
                    vert_delta = SCROLL_DELTA;
                    break;
                case Key.UpArrow:
                    vert_delta = -SCROLL_DELTA;
                    break;
            }
        }

        void PlaceInitialUnits()
        {
            List<UnitInfo> unit_infos = scenario.Units;

            List<Unit> startLocations = new List<Unit>();

            units = new List<Unit>();

            foreach (UnitInfo unitinfo in unit_infos)
            {
                if (unitinfo.UnitId == 0xffff)
                {
                    break;
                }

                Unit unit = new Unit(unitinfo);

                /* we handle start locations in a special way, below */
                if (unit.FlingyId == 140)
                {
                    startLocations.Add(unit);
                    continue;
                }

                //players[unitinfo.player].AddUnit (unit);

                unit.CreateSprite(this.Mpq, tileset_palette);
                units.Add(unit);
            }

            if (template != null && (template.InitialUnits != InitialUnits.UseMapSettings))
            {
                foreach (Unit sl in startLocations)
                {
                    /* terran command center = 106,
                       zerg hatchery = 131,
                       protoss nexus = 154 */

                    Unit unit = new Unit(154);
                    unit.X = sl.X;
                    unit.Y = sl.Y;

                    unit.CreateSprite(this.Mpq, tileset_palette);
                    units.Add(unit);
                }
            }

            /* for now assume the player is at startLocations[0], and center the view there */
            Recenter(startLocations[0].X, startLocations[0].Y);
        }
    }
}
