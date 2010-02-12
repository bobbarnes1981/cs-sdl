/**
 *     Copyright (C) 2010  snacko
 *
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *
 *     You should have received a copy of the GNU General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


package arcane.ui.util;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Rectangle;

import javax.swing.Icon;
import javax.swing.ImageIcon;
import javax.swing.JComponent;
import javax.swing.JMenuItem;

import com.sun.java.swing.plaf.windows.WindowsMenuItemUI;

public class MenuItem extends JMenuItem {
	public MenuItem (String text, Image image) {
		this(text);
		setIcon(new ImageIcon(image));
	}

	public MenuItem (String text, int nm) {
		super(text, nm);
		fixUI();
	}

	public MenuItem (String text) {
		super(text);
		fixUI();
	}

	private void fixUI () {
		if (!(getUI() instanceof WindowsMenuItemUI)) return;
		setUI(new WindowsMenuItemUI() {
			protected void paintMenuItem (Graphics g, JComponent c, Icon checkIcon, Icon arrowIcon, Color background,
				Color foreground, int defaultTextIconGap) {
				super.paintMenuItem(g, c, null, arrowIcon, background, foreground, 0);
			}

			protected void paintText (Graphics g, JMenuItem menuItem, Rectangle textRect, String text) {
				textRect.x += 6;
				super.paintText(g, menuItem, textRect, text);
			}
		});
	}
}
