/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
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

using SdlDotNet.Utility;
using SdlDotNet.Sprites;
using SdlDotNet;
using System;
using System.Drawing;

namespace MfGames.Sdl.Gui
{
	/// <summary>
	/// This class manages and controls the various GUI elements inside
	/// the SDL GUI system. It controls both the appearance (theme) of
	/// the control and also some basic functionality. Almost all of the
	/// GUI elements take this as a constructor. This class is intended
	/// to be extended, the default implement uses just rectangles and
	/// no graphics for its rendering.
	/// </summary>
	public class GuiManager
	{
		/// <summary>
		/// Constructs the GUI manager with the minimum required to keep
		/// the entire system running properly. The sprite manager is used
		/// to control the actual window elements while the baseFont is
		/// used for any requests for fonts. Specific fonts may assigned,
		/// the base system will always fall back to the baseFont.
		/// </summary>
		public GuiManager(SpriteContainer spriteManager, SdlDotNet.Font baseFont,
			Size size)
		{
			this.manager = spriteManager;
			this.baseFontGui = baseFont;
			this.size = size;
		}

		#region Singleton
		private GuiManager singleton = null;

		/// <summary>
		/// Contains the singleton instance of the GuiManager. This is
		/// used for all the widgets that are not given a manager at their
		/// creation.
		/// </summary>
		public GuiManager Singleton
		{
			get { return singleton; }
			set { singleton = value; }
		}
		#endregion

		#region Fonts
		// Contains the fall-back font for the system
		protected SdlDotNet.Font baseFontGui = null;

		// Contains the font for any window titles
		private SdlDotNet.Font titleFont = null;

		// Contains the font for menus
		private SdlDotNet.Font menuFont = null;

		public SdlDotNet.Font BaseFont
		{
			get { return baseFontGui; }
			set
			{
				if (value == null)
					throw new GuiException("Cannot assign a null font to the GUI");

				baseFontGui = value;
			}
		}

		public SdlDotNet.Font MenuFont
		{
			get
			{
				if (menuFont == null)
					return BaseFont;
				else
					return menuFont; 
			}
			set { menuFont = value; }
		}

		public SdlDotNet.Font TitleFont
		{
			get
			{
				if (titleFont == null)
					return BaseFont;
				else
					return titleFont; 
			}
			set { titleFont = value; }
		}

		/// <summary>
		/// Renders a given text with the given font and returns the size
		/// of the surface rendered.
		/// </summary>
		public Size GetTextSize(SdlDotNet.Font font, string text)
		{
			// Render the text
			Surface ts = font.Render(text, Color.FromArgb(255, 255, 255));
      
			return new Size(ts.Width, ts.Height);
		}
		#endregion

		#region Components
		public Padding TickerPadding
		{
			get { return new Padding(2); }
		}

		public void Render(RenderArgs args, GuiTicker ticker)
		{
			// Draw a frame
			args.Surface.Fill(args.Translate(ticker.Bounds), backgroundColor);
			DrawRect(args.Surface, args.Translate(ticker.Bounds), frameColor);
		}
		#endregion

		#region Menus
		public Padding MenuBarPadding
		{
			get { return new Padding(10, 2); }
		}

		public Padding MenuItemPadding
		{
			get { return new Padding(10, 2); }
		}

		public Padding MenuItemInnerPadding
		{
			get { return new Padding(0); }
		}

		public Padding MenuPopupPadding
		{
			get { return new Padding(2, 2, 1, 1); }
		}

		public Padding MenuSpacerPadding
		{
			get { return new Padding(10, 1, 10, 2);}
		}

		public Padding MenuTitlePadding
		{
			get { return new Padding(10, 2); }
		}

		public void Render(RenderArgs args, GuiMenuBar menubar)
		{
			// Draw a frame
			args.Surface.Fill(args.Translate(menubar.OuterBounds),
				backgroundColor);
			DrawRect(args.Surface, args.Translate(menubar.OuterBounds), frameColor);
		}

		public void Render(RenderArgs args, GuiMenuItem item)
		{
			// Draw a background frame depending on if the title is selected
			// or not. This draws out the outer bounds to include the
			// padding. This has to be adjusted slightly because menu items
			// are actually placed back on their outer coordinates, not
			// their inner ones.
			if (item.IsSelected)
			{
				// Adjust the size of the select box
				Size d = item.OuterSize;

				// Draw the selection
				args.Surface.Fill(args.Translate(new Rectangle(item.Coords.X, item.Coords.Y, d.Width, d.Height)),
					selectedColor);
			}
		}

		public void Render(RenderArgs args, GuiMenuPopup menu)
		{
			// Clear out the background and draw the frame line
			Rectangle rect = args.Translate(new Rectangle(menu.OuterBounds.X, menu.OuterBounds.Y, menu.OuterBounds.Width, menu.OuterBounds.Height));
			args.Surface.Fill(rect, backgroundColor);
			DrawRect(args.Surface, rect, frameColor);
		}

		public void Render(RenderArgs args, GuiMenuTitle title)
		{
			// Draw a background frame depending on if the title is selected
			// or not. We have to add the padding because of the title
			// doesn't know about the menu's padding.
			if (title.IsSelected)
			{
				// Get the rectangle
				Rectangle rect = args.Translate(new Rectangle(title.OuterBounds.X, title.OuterBounds.Y, 0, 0));
				int transX;
				int transY;
				transY = rect.Location.Y - MenuBarPadding.Top + MenuTitlePadding.Top;
				transX = rect.Location.X - MenuTitlePadding.Left;
				rect.Location = new Point(transX, transY);
//				rect.Coords.Y -= MenuBarPadding.Top + MenuTitlePadding.Top;
//				rect.Coords.X -= MenuTitlePadding.Left;

				int tempHeight;
				int tempWidth;
				tempHeight = rect.Size.Height + MenuBarPadding.Vertical
					+ MenuTitlePadding.Vertical;
				tempWidth = rect.Size.Width + MenuTitlePadding.Horizontal;
				rect.Size = new Size(tempWidth, tempHeight);
				

				// Draw it
				args.Surface.Fill(rect,  selectedColor);
			}
		}
		#endregion

		#region Windows
		private int windowPad = 1;

		public void Render(RenderArgs args, GuiWindow window)
		{
			// Pull out the fields
			Rectangle bounds = args.Translate(window.OuterBounds);
			//Vector2 coords = bounds.Coords;
			Point coords = bounds.Location;
			Size size = bounds.Size;

			// Clear out the background and draw the frame line
			args.Surface.Fill(bounds, backgroundColor);
			DrawRect(args.Surface, bounds, frameColor);

			// Check for a title
			if (window.Title != null)
			{
				// Copy the args
				RenderArgs args1 = args.Clone();

				// Blank out the title
				//Rectangle2 tr = new Rectangle2(new Vector2(coords.X
				Rectangle tr = new Rectangle(new Point(coords.X
					+ windowPad,
					coords.Y
					+ windowPad),
					new Size(size.Width,
					TitleFont.Height));
				Rectangle clip = new Rectangle(tr.Location,
					new Size(tr.Width - windowPad,
					tr.Height));
				args1.SetClipping(clip);
				args.Surface.Fill(tr, frameColor);

				// Draw the title, centered
				Surface ts = titleFont.Render(window.Title, titleColor);

				if (ts.Width < tr.Width)
				{
					int transX;
					transX = tr.Location.X + (tr.Width - ts.Width) / 2;
					tr.Location = new Point(transX, tr.Location.Y);
					//tr.Coords.X += (tr.Width - ts.Width) / 2;
				}

				args.Surface.Blit(ts, tr);
				args1.ClearClipping();
			}
		}

		public Padding GetPadding(GuiWindow window)
		{
			// Create a new padding
			Padding pad = new Padding(windowPad);

			// Check for title
			if (window.Title != null)
				pad.Top += TitleFont.Height + windowPad;

			return pad;
		}
		#endregion

		#region Drawing Routines
		/// <summary>
		/// This is one of the core drawing functions to draw a rectangle
		/// in a specified color on the given args.Surface.
		/// </summary>
		public static void DrawRect(Surface surface, Rectangle bounds, Color color)
		{
			// FIX: Very sloppy code.

			// Ignore blanks
			if (surface.Width == 0 || surface.Height == 0)
				return;

			// Draw the lines
			int l = bounds.Left;
			int r = bounds.Right;
			int t = bounds.Top;
			int b = bounds.Bottom;

			// Draw the pixels
			for (int i = l; i <= r; i++)
			{
				if (i >= 0 && i <= surface.Width)
				{
					try 
					{
						if (t >= 0)
							surface.DrawPixel(i, t, color);
					} 
					catch { }
	  
					try 
					{
						if (b <= surface.Height)
							surface.DrawPixel(i, b, color);
					} 
					catch { }
				}
			}

			for (int i = t; i < b; i++)
			{
				if (i >= 0 && i <= surface.Height)
				{
					try 
					{
						if (l >= 0)
							surface.DrawPixel(l, i, color);
					} 
					catch { }
	  
					try 
					{
						if (r <= surface.Width)
							surface.DrawPixel(r, i, color);
					} 
					catch { }
				}
			}
		}
		#endregion

		#region Colors
		private Color backgroundColor = Color.FromArgb(200, 0, 0, 25);
		private Color frameColor = Color.FromArgb(25, 25, 50);
		private Color selectedColor = Color.FromArgb(25, 25, 50);
		private Color traceBoundsColor = Color.FromArgb(200, 25, 25);
		private Color traceOuterColor = Color.FromArgb(100, 0, 0);
		private Color traceInnerColor = Color.FromArgb(255, 50, 50);
		private Color traceColor = Color.CornflowerBlue;

		public Color TraceColor
		{
			get { return traceColor; }
			set { traceColor = value; }
		}

		public Color BoundsTraceColor
		{
			get { return traceBoundsColor; }
			set { traceBoundsColor = value; }
		}

		public Color OuterBoundsTraceColor
		{
			get { return traceOuterColor; }
			set { traceOuterColor = value; }
		}

		public Color InnerBoundsTraceColor
		{
			get { return traceInnerColor; }
			set { traceInnerColor = value; }
		}
		#endregion

		/*************************************************************/

		#region Information
		public int GetTitleHeight(string title)
		{
			if (title == null || titleFont == null)
				return 0;
			else
				return titleFont.Render(title, titleColor).Height;
		}

		public int GetTitleWidth(string title)
		{
			if (title == null || titleFont == null)
				return 0;
			else
				return titleFont.Render(title, titleColor).Width
					+ 2 * titleHorzPad;
		}
		#endregion

		#region Properties
		private SpriteContainer manager = null;
		private Size size = Size.Empty;

		private Color titleColor = Color.FromArgb(250, 250, 250);
		private int titleHorzPad = 3;
		private int dragZOrder = 10000;

		public int DragZOrder
		{
			get { return dragZOrder; }
			set { dragZOrder = value; }
		}

		public Size Size
		{
			get { return size; }
			set { size = value; }
		}

		public SpriteContainer SpriteContainer
		{
			get { return manager; }
			set
			{
				if (value == null)
					throw new Exception("Cannot assign null sprite manager to gui");

				manager = value;
			}
		}

		#endregion
	}
}
